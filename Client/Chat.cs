using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using System.Configuration;

namespace Twacker
{
    public partial class Chat : Form
    {
        private OAuth _oauth;
        private IrcClient _ircClient;
        private Followers _followers;
        private Reminder _reminder;

        public Chat()
        {
            InitializeComponent();

            this.AcceptButton = this.sendChat;

            _oauth = new OAuth();

            getUserSettings();
            authorize();
        }

        ~Chat()
        {
            _ircClient.Disconnect();
            _followers.Stop();
            _reminder.Stop();
        }

        private void getUserSettings(bool force = false)
        {
            if (force ||
                (
                    Properties.Settings.Default.UserName == "" ||
                    Properties.Settings.Default.Channel == "")
                )
            {
                SettingsForm settings = new SettingsForm();
                settings.ShowDialog();
                settings = null;
            }
        }

        #region IRC Methods

        private void startIrcClient()
        {
            configureIrcClient();
            _ircClient.Connect();
        }

        private void configureIrcClient()
        {
            string server = Properties.Settings.Default.IrcServer;
            int port = Properties.Settings.Default.IrcPort;

            _ircClient = new IrcClient(server, port);

            // Nicks are all lowercase for Twitch IRC
            _ircClient.Nick = Properties.Settings.Default.UserName.ToLower();

            _ircClient.ServerPass = Properties.Settings.Default.OAuthPassword;
            _ircClient.DefaultChannel = Properties.Settings.Default.Channel;
            
            addListeners();
        }

        private void restartIrcClient()
        {
            _ircClient.Disconnect();
            startIrcClient();
        }

        private string channelName() {
            return "#" + Properties.Settings.Default.Channel.ToLower();
        }

        private void addListeners()
        {
            // tee, hee, hee
            _ircClient.ChannelMessage += (c, u, m) =>
            {
                writeText(u + ": " + m);
                say(u + " " + m, u);
            };
            _ircClient.OnConnect += () =>
            {
                _ircClient.JoinChannel(channelName());
                say("Twaacker Connected to the IRC Server");
            };
            _ircClient.UserJoined += (c, u) =>
            {
                string user = Properties.Settings.Default.UserName;
                string channel = Properties.Settings.Default.Channel;

                if (u.ToLower() == user.ToLower())
                { say("Joined. " + user + " is now monitoring channel " + channel); }
                else
                { say("Welcome to my channel " + u, u); }
            };
            _ircClient.UpdateUsers += (c, u) =>
            {
                this.chatMembers.Items.Clear();
                this.chatMembers.Items.AddRange(u);
            };
            _ircClient.UserLeft += (c, u) =>
             {
                 say(u + "left your channel.", u);
             };
            _ircClient.NoticeMessage += (c, m) =>
            {
                writeText(m);
                if (m.ToLower() == "login unsuccessful")
                {
                    DialogResult result = MessageBox.Show("Do you want to retry?", "Login Failure", MessageBoxButtons.RetryCancel);
                    if (result == System.Windows.Forms.DialogResult.Retry)
                    { reauthorize(); }
                }
            };
        }

        private void say(string text, string user = "")
        {
            if (user.ToLower() != Properties.Settings.Default.UserName.ToLower())
            { Speech.Say(text); }
        }

        private void sendChat_Click(object sender, EventArgs e)
        {
            _ircClient.SendMessage(channelName(), chatEntry.Text);
            chatHistory.AppendText("You: " + chatEntry.Text + "\n");
            chatEntry.Clear();
            chatEntry.Focus();
        }
        #endregion

        #region OAuth Functionality

        private void authorize()
        {
            clearStatus();

            if (Properties.Settings.Default.OAuthPassword == "")
            {
                OAuthCallBack callback = new OAuthCallBack(authorizedCallback);
                _oauth.Authorize(callback);
            }
            else
            {
                authorized(Properties.Settings.Default.OAuthPassword);
            }
        }

        private void authorizedCallback(OAuthEventArgs e)
        {
            switch (e.State)
            {
                case OAuthStates.Error:
                    showError(e.Message);
                    break;
                case OAuthStates.Authorization:
                    showOauth();
                    break;
                case OAuthStates.Success:
                    authorized(e.Message);
                    break;
            }

        }        

        /// <summary>
        /// reauthorization forces a logout to make sure we have
        /// the correct user getting the correct oauth password.
        /// </summary>
        private void reauthorize()
        {
            _oauth.ClearPassword();
            OAuthCallBack callback = new OAuthCallBack(loggedOut);
            _oauth.Logout(callback);
        }


        private void authorized(string password)
        {
            hideOauth();
            Properties.Settings.Default.OAuthPassword = password;
            Properties.Settings.Default.Save();
            writeText("Authorized");
            showStatus();
            startIrcClient();
            startFollowersWatcher();
        }

        private void showStatus()
        {
            this.statusLabel.Text = Properties.Settings.Default.UserName + " monitoring " + Properties.Settings.Default.Channel;
        }

        private void clearStatus()
        {
            this.statusLabel.Text = "...";
        }

        private void showOauth()
        {
            container1.Visible = false;
            WebBrowser browser = _oauth.Browser;
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(browser);
        }

        private void hideOauth()
        {
            container1.Visible = true;
            this.Controls.Remove(_oauth.Browser);
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            reauthorize();
        }

        public void loggedOut(OAuthEventArgs e)
        {

            switch (e.State)
            {
                case OAuthStates.Error:
                    showError(e.Message);
                    break;
                case OAuthStates.Success:
                    writeText("logged out");
                    getUserSettings();
                    authorize();
                    break;
            }

        }

        #endregion

        #region Text Messages

        private void showError(string message)
        {
            hideOauth();
            writeText("error: " + message);
        }

        private void writeText(string message)
        {
            message = DateTime.Now.ToShortTimeString() + " " + message + "\n";
            chatHistory.AppendText(message);
            chatHistory.ScrollToCaret();
        }

        #endregion

        public void startFollowersWatcher()
        {
            _followers = new Followers(Properties.Settings.Default.Channel.ToLower());
            _followers.Start();
        }

        public void startReminder()
        {
            string message = "Thanks for watching. Remember to hit that follow button and if you already have, thank you.";
            _reminder = new Reminder(message);
            _reminder.Start();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            string user = Properties.Settings.Default.UserName;
            string chan = Properties.Settings.Default.Channel;
            getUserSettings(true);

            if (user != Properties.Settings.Default.UserName)
            {
                reauthorize();
            }
            else if (chan != Properties.Settings.Default.Channel)
            {
                restartIrcClient();
            }
            
        }

    }
}
