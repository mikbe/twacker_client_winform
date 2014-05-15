using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace Twacker
{
    public partial class SettingsForm : Form
    {

        public SettingsForm()
        {
            InitializeComponent();
            this.botUserName.Text = Properties.Settings.Default.UserName;
            this.channelName.Text = Properties.Settings.Default.Channel;

            Debug.WriteLine("user: " + Properties.Settings.Default.UserName);
            Debug.WriteLine("channel: " + Properties.Settings.Default.Channel);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserName = this.botUserName.Text;
            Properties.Settings.Default.Channel = this.channelName.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
