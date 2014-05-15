using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;

namespace Twacker
{
    public enum OAuthStates
    {
        Error,
        Authorization,
        Success
    }

    public class OAuthEventArgs : EventArgs
    {
        public OAuthStates State { get; set; }
        public string Message { get; set; }
    }

    public delegate void OAuthCallBack(OAuthEventArgs e);


    // This class is tightly coupled to the configuration settings.
    // I should refactor that out so it's not anti-best practices.
    class OAuth
    {
        private OAuthCallBack _callback;

        private WebBrowser _browser;
        public WebBrowser Browser
        {
            get { return _browser; }
        }

        public OAuth()
        {
            fixServerAddress();
            configBrowser();
        }

        public void ClearPassword()
        {
            Properties.Settings.Default.OAuthPassword = "";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// I constantly forget if I'm supposed to add the slash or not
        /// in a setting so this function makes sure it doesn't matter.
        /// </summary>
        private void fixServerAddress()
        { 
            string server = Properties.Settings.Default.OAuthServer;
            if (server[server.Length - 1] != '/')
            {
                Properties.Settings.Default.OAuthServer += "/";
                Properties.Settings.Default.Save();
            }
        }

        private void configBrowser() 
        {
            _browser = new WebBrowser();
            _browser.ScriptErrorsSuppressed = true;
            _browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browserDocumentCompleted);
        }

        public void Logout(OAuthCallBack callback)
        {
            _callback = callback;
            _browser.Url = new Uri(Properties.Settings.Default.OAuthServer + "logout");
        }

        public void Authorize(OAuthCallBack callback)
        {
            _callback = callback;
            _browser.Url = new Uri(Properties.Settings.Default.OAuthServer + "oauth");
        }

        private void browserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Debug.WriteLine("_browser_DocumentCompleted");
            Debug.WriteLine(_browser.DocumentTitle);
            Debug.WriteLine(_browser.Url);

            switch(_browser.DocumentTitle) 
            {
                case "Navigation Canceled":
                    fireError("Network Error");
                    break;
                case "Twitch - Authorize Application":
                    fireShowAuth();
                    break;
                case "Twacker Error":
                    fireError(findKey("error"));
                    break;
                case "Twacker Success":
                    fireSuccess(findKey("oauth"));
                    break;
                case "Twacker Logged Out":
                    fireSuccess("Logged Out");
                    break;
            }

        }

        private void fireError(string error)
        {
            OAuthEventArgs e = new OAuthEventArgs();
            e.State = OAuthStates.Error;
            e.Message = error;
            _callback(e);
        }

        private void fireShowAuth()
        {
            enterUserName();

            OAuthEventArgs e = new OAuthEventArgs();
            e.State = OAuthStates.Authorization;
            _callback(e);
        }

        private void enterUserName()
        {
            HtmlElement user_id = _browser.Document.GetElementById("user_login");
            if (user_id != null)
            {
                user_id.InnerText = Properties.Settings.Default.UserName;
                //user_id.Enabled = false;
            }
        }


        private void fireSuccess(string message) 
        {
            OAuthEventArgs e = new OAuthEventArgs();
            e.State = OAuthStates.Success;
            e.Message = message;
            _callback(e);
        }

        private string findKey(string key)
        {
            string keyValue = "";
            HtmlElement html_element = _browser.Document.GetElementById(key);
            if (html_element != null && html_element.InnerText != null)
            { keyValue = html_element.InnerText; }

            return keyValue;
        }

    }
}
