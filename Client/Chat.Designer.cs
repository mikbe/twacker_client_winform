namespace Twacker
{
    partial class Chat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.container1 = new System.Windows.Forms.SplitContainer();
            this.loginButton = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.settingsButton = new System.Windows.Forms.Button();
            this.container2 = new System.Windows.Forms.SplitContainer();
            this.viewers_chat_container = new System.Windows.Forms.SplitContainer();
            this.chatMembers = new System.Windows.Forms.ListBox();
            this.chatHistory = new System.Windows.Forms.RichTextBox();
            this.container3 = new System.Windows.Forms.SplitContainer();
            this.chatEntry = new System.Windows.Forms.TextBox();
            this.sendChat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.container1)).BeginInit();
            this.container1.Panel1.SuspendLayout();
            this.container1.Panel2.SuspendLayout();
            this.container1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.container2)).BeginInit();
            this.container2.Panel1.SuspendLayout();
            this.container2.Panel2.SuspendLayout();
            this.container2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewers_chat_container)).BeginInit();
            this.viewers_chat_container.Panel1.SuspendLayout();
            this.viewers_chat_container.Panel2.SuspendLayout();
            this.viewers_chat_container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.container3)).BeginInit();
            this.container3.Panel1.SuspendLayout();
            this.container3.Panel2.SuspendLayout();
            this.container3.SuspendLayout();
            this.SuspendLayout();
            // 
            // container1
            // 
            this.container1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.container1.Location = new System.Drawing.Point(0, 0);
            this.container1.Name = "container1";
            this.container1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // container1.Panel1
            // 
            this.container1.Panel1.Controls.Add(this.loginButton);
            this.container1.Panel1.Controls.Add(this.logoutButton);
            this.container1.Panel1.Controls.Add(this.statusLabel);
            this.container1.Panel1.Controls.Add(this.settingsButton);
            // 
            // container1.Panel2
            // 
            this.container1.Panel2.Controls.Add(this.container2);
            this.container1.Size = new System.Drawing.Size(944, 909);
            this.container1.SplitterDistance = 33;
            this.container1.TabIndex = 1;
            // 
            // loginButton
            // 
            this.loginButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Location = new System.Drawing.Point(669, 0);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(82, 33);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.logoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logoutButton.Location = new System.Drawing.Point(751, 0);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(123, 33);
            this.logoutButton.TabIndex = 2;
            this.logoutButton.Text = "erase all settings";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(12, 5);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(25, 24);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "...";
            // 
            // settingsButton
            // 
            this.settingsButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Location = new System.Drawing.Point(874, 0);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(70, 33);
            this.settingsButton.TabIndex = 0;
            this.settingsButton.Text = "config";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // container2
            // 
            this.container2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.container2.Location = new System.Drawing.Point(0, 0);
            this.container2.Name = "container2";
            this.container2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // container2.Panel1
            // 
            this.container2.Panel1.Controls.Add(this.viewers_chat_container);
            // 
            // container2.Panel2
            // 
            this.container2.Panel2.Controls.Add(this.container3);
            this.container2.Panel2MinSize = 23;
            this.container2.Size = new System.Drawing.Size(944, 872);
            this.container2.SplitterDistance = 840;
            this.container2.TabIndex = 2;
            // 
            // viewers_chat_container
            // 
            this.viewers_chat_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewers_chat_container.Location = new System.Drawing.Point(0, 0);
            this.viewers_chat_container.Name = "viewers_chat_container";
            // 
            // viewers_chat_container.Panel1
            // 
            this.viewers_chat_container.Panel1.Controls.Add(this.chatMembers);
            // 
            // viewers_chat_container.Panel2
            // 
            this.viewers_chat_container.Panel2.Controls.Add(this.chatHistory);
            this.viewers_chat_container.Size = new System.Drawing.Size(944, 840);
            this.viewers_chat_container.SplitterDistance = 215;
            this.viewers_chat_container.TabIndex = 2;
            // 
            // chatMembers
            // 
            this.chatMembers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatMembers.FormattingEnabled = true;
            this.chatMembers.Location = new System.Drawing.Point(0, 0);
            this.chatMembers.Name = "chatMembers";
            this.chatMembers.Size = new System.Drawing.Size(215, 840);
            this.chatMembers.TabIndex = 0;
            // 
            // chatHistory
            // 
            this.chatHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatHistory.Location = new System.Drawing.Point(0, 0);
            this.chatHistory.Name = "chatHistory";
            this.chatHistory.Size = new System.Drawing.Size(725, 840);
            this.chatHistory.TabIndex = 1;
            this.chatHistory.Text = "";
            // 
            // container3
            // 
            this.container3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.container3.Location = new System.Drawing.Point(0, 0);
            this.container3.Name = "container3";
            // 
            // container3.Panel1
            // 
            this.container3.Panel1.Controls.Add(this.chatEntry);
            // 
            // container3.Panel2
            // 
            this.container3.Panel2.Controls.Add(this.sendChat);
            this.container3.Size = new System.Drawing.Size(944, 28);
            this.container3.SplitterDistance = 820;
            this.container3.TabIndex = 0;
            // 
            // chatEntry
            // 
            this.chatEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatEntry.Location = new System.Drawing.Point(0, 0);
            this.chatEntry.Name = "chatEntry";
            this.chatEntry.Size = new System.Drawing.Size(820, 29);
            this.chatEntry.TabIndex = 0;
            // 
            // sendChat
            // 
            this.sendChat.BackColor = System.Drawing.Color.YellowGreen;
            this.sendChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sendChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendChat.Location = new System.Drawing.Point(0, 0);
            this.sendChat.Name = "sendChat";
            this.sendChat.Size = new System.Drawing.Size(120, 28);
            this.sendChat.TabIndex = 0;
            this.sendChat.Text = "send";
            this.sendChat.UseVisualStyleBackColor = false;
            this.sendChat.Click += new System.EventHandler(this.sendChat_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 909);
            this.Controls.Add(this.container1);
            this.MaximizeBox = false;
            this.Name = "Chat";
            this.Text = "Twacker - Voice-to-Text Chat Reader";
            this.container1.Panel1.ResumeLayout(false);
            this.container1.Panel1.PerformLayout();
            this.container1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.container1)).EndInit();
            this.container1.ResumeLayout(false);
            this.container2.Panel1.ResumeLayout(false);
            this.container2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.container2)).EndInit();
            this.container2.ResumeLayout(false);
            this.viewers_chat_container.Panel1.ResumeLayout(false);
            this.viewers_chat_container.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewers_chat_container)).EndInit();
            this.viewers_chat_container.ResumeLayout(false);
            this.container3.Panel1.ResumeLayout(false);
            this.container3.Panel1.PerformLayout();
            this.container3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.container3)).EndInit();
            this.container3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer container1;
        private System.Windows.Forms.RichTextBox chatHistory;
        private System.Windows.Forms.SplitContainer container3;
        private System.Windows.Forms.TextBox chatEntry;
        private System.Windows.Forms.SplitContainer container2;
        private System.Windows.Forms.Button sendChat;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.SplitContainer viewers_chat_container;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ListBox chatMembers;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Button loginButton;
    }
}