using System.Collections;
namespace Client
{
    partial class Chat
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.msgToSend = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.onlineUsersGroup = new System.Windows.Forms.GroupBox();
            this.onlineUsers = new System.Windows.Forms.ListView();
            this.startChat = new System.Windows.Forms.Button();
            this.listMessages = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.onlineUsersGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgToSend
            // 
            this.msgToSend.Enabled = false;
            this.msgToSend.Location = new System.Drawing.Point(165, 340);
            this.msgToSend.Multiline = true;
            this.msgToSend.Name = "msgToSend";
            this.msgToSend.Size = new System.Drawing.Size(327, 37);
            this.msgToSend.TabIndex = 1;
            this.msgToSend.TextChanged += new System.EventHandler(this.msgToSend_TextChanged);
            // 
            // sendBtn
            // 
            this.sendBtn.Enabled = false;
            this.sendBtn.Location = new System.Drawing.Point(498, 340);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(64, 37);
            this.sendBtn.TabIndex = 3;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // onlineUsersGroup
            // 
            this.onlineUsersGroup.Controls.Add(this.onlineUsers);
            this.onlineUsersGroup.Location = new System.Drawing.Point(12, 12);
            this.onlineUsersGroup.Name = "onlineUsersGroup";
            this.onlineUsersGroup.Size = new System.Drawing.Size(147, 322);
            this.onlineUsersGroup.TabIndex = 5;
            this.onlineUsersGroup.TabStop = false;
            this.onlineUsersGroup.Text = "Online Users";
            // 
            // onlineUsers
            // 
            this.onlineUsers.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.onlineUsers.HideSelection = false;
            this.onlineUsers.Location = new System.Drawing.Point(7, 20);
            this.onlineUsers.Name = "onlineUsers";
            this.onlineUsers.Size = new System.Drawing.Size(134, 296);
            this.onlineUsers.TabIndex = 0;
            this.onlineUsers.UseCompatibleStateImageBehavior = false;
            this.onlineUsers.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.onlineUsers_ItemSelectionChanged);
            // 
            // startChat
            // 
            this.startChat.Enabled = false;
            this.startChat.Location = new System.Drawing.Point(86, 340);
            this.startChat.Name = "startChat";
            this.startChat.Size = new System.Drawing.Size(67, 37);
            this.startChat.TabIndex = 1;
            this.startChat.Text = "New Chat";
            this.startChat.UseVisualStyleBackColor = true;
            this.startChat.Click += new System.EventHandler(this.startChat_Click);
            // 
            // listMessages
            // 
            this.listMessages.FormattingEnabled = true;
            this.listMessages.Location = new System.Drawing.Point(165, 12);
            this.listMessages.Name = "listMessages";
            this.listMessages.Size = new System.Drawing.Size(388, 316);
            this.listMessages.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 340);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 37);
            this.button1.TabIndex = 7;
            this.button1.Text = "Logout";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(574, 387);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listMessages);
            this.Controls.Add(this.startChat);
            this.Controls.Add(this.onlineUsersGroup);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.msgToSend);
            this.Name = "Chat";
            this.Text = "Chat Service";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatRoom_FormClosed);
            this.onlineUsersGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.TextBox msgToSend;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.GroupBox onlineUsersGroup;

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

        private System.Windows.Forms.ListView onlineUsers;
        private System.Windows.Forms.Button startChat;
        private System.Windows.Forms.ListBox listMessages;
        private System.Windows.Forms.Button button1;
    }
}