/* ********************************************
 * 
 * Created by KoBE at TechLifeForum
 * http://tech.reboot.pro
 * 
 *
 * Additional functionality by Mike Bethany
 * http://mikebethany.com
 * 
 **********************************************
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.ComponentModel;

using System.Diagnostics;

namespace Twacker
{
    public class IrcClient
    {
        #region Variables

        private const int DEFAULT_PORT = 6667;

        private int _rateLimit;
        public int RateLimitMilliseconds
        {
            get { return _rateLimit; }
            set { _rateLimit = value; }
        }

        private DateTime lastMsgAt = new DateTime(1900);

        private string _server;
        private int _port;
        private string _server_pass;
        private string _nick;
        private string _altNick;

        private TcpClient _irc;

        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        // AsyncOperation used to handle cross-thread wonderness
        private AsyncOperation _op;

        #endregion

        #region Constructors

        public IrcClient(string Server, int Port)
        {
            _op = AsyncOperationManager.CreateOperation(null);
            _server = Server;
            _port = Port;
        }

        public IrcClient(string Server) : this (Server, DEFAULT_PORT)
        {}

        #endregion

        #region Properties

        public string Server
        {
            get { return _server; }
        }

        public int Port
        {
            get { return _port; }
        }

        // There is no nick password at the moment
        public string ServerPass
        {
            get { return _server_pass; }
            set { _server_pass = value; }
        }

        // User is the same as nick for now
        public string User
        {
            get { return _nick; }
            set { _nick = value; }
        }

        public string Nick
        {
            get { return _nick; }
            set { _nick = value; }
        }

        public string AltNick
        {
            get { return _altNick; }
            set { _altNick = value; }
        }

        public bool Connected
        {
            get
            {
                return _irc != null && _irc.Connected;
            }
        }

        private string _defaultChannel;
        public string DefaultChannel
        {
            get { return _defaultChannel; }
            set { _defaultChannel = value; }
        }

        #endregion

        #region Events

        public event UpdateUserListEventDelegate UpdateUsers;
        public event UserJoinedEventDelegate UserJoined;
        public event UserLeftEventDelegate UserLeft;
        public event UserNickChangeEventDelegate UserNickChange;

        public event ChannelMesssageEventDelegate ChannelMessage;
        public event NoticeMessageEventDelegate NoticeMessage;
        public event PrivateMessageEventDelegate PrivateMessage;
        public event ServerMessageEventDelegate ServerMessage;

        public event NickTakenEventDelegate NickTaken;

        public event ConnectedEventDelegate OnConnect;

        public event ExceptionThrownEventDelegate ExceptionThrown;

        private void fireUpdateUsers(oUserList o)
        {
            if (UpdateUsers != null) UpdateUsers(o.Channel, o.UserList);
        }

        private void fireUserJoined(oUserJoined o)
        {
            if (UserJoined != null) UserJoined(o.Channel, o.User);
        }

        private void fireUserLeft(oUserLeft o)
        {
            if (UserLeft != null) UserLeft(o.Channel, o.User);
        }

        private void fireNickChanged(oUserNickChanged o)
        {
            if (UserNickChange != null) UserNickChange(o.Old, o.New);
        }

        private void fireChannelMessage(oChannelMessage o)
        {
            if (ChannelMessage != null) ChannelMessage(o.Channel, o.From, o.Message);
        }

        private void fireNoticeMessage(oNoticeMessage o)
        {
            if (NoticeMessage != null) NoticeMessage(o.From, o.Message);
        }

        private void firePrivateMessage(oPrivateMessage o)
        {
            if (PrivateMessage != null) PrivateMessage(o.From, o.Message);
        }

        private void fireServerMesssage(string s)
        {
            if (ServerMessage != null) ServerMessage(s);
        }

        private void fireNickTaken(string s)
        {
            if (NickTaken != null) NickTaken(s);
        }

        private void fireConnected()
        {
            if (OnConnect != null) OnConnect();
        }

        private void fireExceptionThrown(Exception ex)
        {
            if (ExceptionThrown != null) ExceptionThrown(ex);
        }

        #endregion

        #region PublicMethods

        public void Connect()
        {
            Thread t = new Thread(connect);
            t.IsBackground = true;
            t.Start();
        }

        private void connect()
        {
            try
            {
                _irc = new TcpClient(_server, _port);
                _stream = _irc.GetStream();
                _reader = new StreamReader(_stream);
                _writer = new StreamWriter(_stream);

                if (!string.IsNullOrEmpty(_server_pass))
                    send("PASS " + _server_pass);

                send("NICK " + _nick);
                send("USER " + _nick + " 0 * :" + _nick);

                listen();
            }
            catch (Exception ex)
            {
                _op.Post(x => fireExceptionThrown((Exception)x), ex);
            }
        }

        public void Disconnect()
        {
            if (_irc != null)
            {
                if (_irc.Connected)
                {
                    send("QUIT Client Disconnected: " + _nick);
                }
                _irc = null;
            }
        }

        public void JoinChannel(string Channel)
        {
            if (_irc != null && _irc.Connected)
            {
                send("JOIN " + Channel);
            }
        }

        public void PartChannel(string Channel)
        {
            send("PART " + Channel);
        }

        public void SendNotice(string nick, string message)
        {
            send("NOTICE " + nick + " :" + message);
        }
        
        public void SendMessage(string channel, string message)
        {
            // raw messages start with a slash
            if (message[0] == '/')
            {
                SendRaw(message.Substring(1));
            }

            send("PRIVMSG " + channel + " :" + message);
        }


        public void SendRaw(string message)
        {
            send(message);
        }

        #endregion

        #region Private Methods

        private void listen()
        {
            string inputLine;

            while ((inputLine = _reader.ReadLine()) != null)
            {
                Debug.WriteLine(inputLine);
                parseData(inputLine);
            }
        }

        private void parseData(string data)
        {
            // split the data into parts
            string[] ircData = data.Split(' ');

            // if the message starts with PING we must PONG back
            if (data.Length > 4)
            {
                if (data.Substring(0, 4) == "PING")
                {
                    send("PONG " + ircData[1]);
                    return;
                }

            }

            // react according to the IRC Commands
            switch (ircData[1])
            {
                // server welcome message, after this we can join
                case "001": 
                    send("MODE " + _nick + " +B");
                    _op.Post((x) => fireConnected(), null);    //TODO: this might not work
                    break;

                // member list
                case "353":
                    _op.Post(x => fireUpdateUsers((oUserList)x), new oUserList(ircData[4], joinArray(ircData, 5).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));
                    break;

                case "433":
                    _op.Post(x => fireNickTaken((string)x), ircData[3]);

                    if (ircData[3] == _altNick)
                    {
                        Random rand = new Random();
                        string randomNick = "Guest" + rand.Next(0, 9) + rand.Next(0, 9) + rand.Next(0, 9);
                        send("NICK " + randomNick);
                        send("USER " + randomNick + " 0 * :" + randomNick);
                        _nick = randomNick;
                    }
                    else
                    {
                        send("NICK " + _altNick);
                        send("USER " + _altNick + " 0 * :" + _altNick);
                        _nick = _altNick;
                    }
                    break;


                case "JOIN": 
                    _op.Post(x => fireUserJoined((oUserJoined)x), new oUserJoined(ircData[2], ircData[0].Substring(1, ircData[0].IndexOf("!") - 1)));
                    break;

                case "NICK":
                    _op.Post(x => fireNickChanged((oUserNickChanged)x), new oUserNickChanged(ircData[0].Substring(1, ircData[0].IndexOf("!") - 1), joinArray(ircData, 3)));
                    break;

                case "NOTICE":
                    if (ircData[0].Contains("!"))
                    {
                        _op.Post(x => fireNoticeMessage((oNoticeMessage)x), new oNoticeMessage(ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), joinArray(ircData, 3)));
                    }
                    else
                    {
                        _op.Post(x => fireNoticeMessage((oNoticeMessage)x), new oNoticeMessage(_server, joinArray(ircData, 3)));
                    }
                    break;

                // message sent to channel or user
                case "PRIVMSG":
                    if (ircData[2].ToLower() == _nick.ToLower())
                    {
                        _op.Post(x => firePrivateMessage((oPrivateMessage)x), new oPrivateMessage(ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), joinArray(ircData, 3)));
                    }
                    else
                    {
                        _op.Post(x => fireChannelMessage((oChannelMessage)x), new oChannelMessage(ircData[2], ircData[0].Substring(1, ircData[0].IndexOf('!') - 1), joinArray(ircData, 3)));
                    }
                    break;

                case "PART":
                case "QUIT":
                    _op.Post(x => fireUserLeft((oUserLeft)x), new oUserLeft(ircData[2], ircData[0].Substring(1, data.IndexOf("!") - 1)));
                    send("NAMES " + ircData[2]);
                    break;

                default:
                    Debug.WriteLine("***** Unhandled Message *****");
                    Debug.WriteLine(String.Join(" : " , ircData));
                    Debug.WriteLine("");

                    if (ircData.Length > 3)
                        _op.Post(x => fireServerMesssage((string)x), joinArray(ircData, 3));

                    break;
            }

        }

        private string stripMessage(string message)
        {
            // remove IRC Color Codes
            foreach (Match m in new Regex((char)3 + @"(?:\d{1,2}(?:,\d{1,2})?)?").Matches(message))
                message = message.Replace(m.Value, "");

            if (message == "")
                return "";
            else if (message.Substring(0, 1) == ":" && message.Length > 2)
                return message.Substring(1, message.Length - 1);
            else
                return message;
        }

        private string joinArray(string[] strArray, int startIndex)
        {
            return stripMessage(String.Join(" ", strArray, startIndex, strArray.Length - startIndex));
        }

        private void send(string message)
        {
            // throttle sends
            while (((TimeSpan)(DateTime.Now - lastMsgAt)).TotalMilliseconds < _rateLimit)
            {
                Thread.Sleep((int)((TimeSpan)(DateTime.Now - lastMsgAt)).TotalMilliseconds);
            }

            _writer.WriteLine(message);
            _writer.Flush();
        }
        
        #endregion

        #region Structs

        public struct oUserList
        {
            public string Channel;
            public string[] UserList;
            public oUserList(string channel, string[] userList)
            {
                this.Channel = channel;
                this.UserList = userList;
            }
        }
        public struct oUserJoined
        {
            public string Channel;
            public string User;
            public oUserJoined(string channel, string user)
            {
                this.Channel = channel;
                this.User = user;
            }
        }
        public struct oUserLeft
        {
            public string Channel;
            public string User;
            public oUserLeft(string channel, string user)
            {
                this.Channel = channel;
                this.User = user;
            }
        }

        public struct oChannelMessage
        {
            public string Channel;
            public string From;
            public string Message;
            public oChannelMessage(string channel, string from, string message)
            {
                this.Channel = channel;
                this.From = from;
                this.Message = message;
            }
        }
        public struct oNoticeMessage
        {
            public string From;
            public string Message;
            public oNoticeMessage(string from, string message)
            {
                this.From = from;
                this.Message = message;
            }
        }
        public struct oPrivateMessage
        {
            public string From;
            public string Message;
            public oPrivateMessage(string from, string message)
            {
                this.From = from;
                this.Message = message;
            }
        }
        public struct oUserNickChanged
        {
            public string Old;
            public string New;
            public oUserNickChanged(string oldName, string newName)
            {
                this.Old = oldName;
                this.New = newName;
            }
        }

        #endregion
    }

}