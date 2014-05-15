/* ***********************************
* Created by KoBE at TechLifeForum
* http://tech.reboot.pro
*************************************/
using System;

namespace Twacker
{
    public delegate void UserNickChangeEventDelegate(string oldUser, string newUser);
    public delegate void NickTakenEventDelegate(string nick);

    public delegate void ServerMessageEventDelegate(string message);
    public delegate void ChannelMesssageEventDelegate(string Channel, string User, string Message);
    public delegate void NoticeMessageEventDelegate(string User, string Message);
    public delegate void PrivateMessageEventDelegate(string User, string Message);

    public delegate void UpdateUserListEventDelegate(string Channel, string[] userlist);
    public delegate void UserJoinedEventDelegate(string Channel, string User);
    public delegate void UserLeftEventDelegate(string Channel, string User);

    public delegate void ConnectedEventDelegate();
    public delegate void DisconnectedEventDelegate();

    public delegate void ExceptionThrownEventDelegate(Exception ex);

}