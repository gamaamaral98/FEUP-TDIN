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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat));
            this.msgToSend = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.onlineUsersGroup = new System.Windows.Forms.GroupBox();
            this.startChat = new System.Windows.Forms.Button();
            this.onlineUsers = new System.Windows.Forms.ListView();
            this.activeConversations = new System.Windows.Forms.TabControl();
            this.newGroupChat = new System.Windows.Forms.Button();
            this.groupChatsGroup = new System.Windows.Forms.GroupBox();
            this.inviteToGroup = new System.Windows.Forms.Button();
            this.joinGroupChat = new System.Windows.Forms.Button();
            this.groupChats = new System.Windows.Forms.ListView();
            this.onlineUsersGroup.SuspendLayout();
            this.groupChatsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgToSend
            // 
            this.msgToSend.Enabled = false;
            this.msgToSend.Location = new System.Drawing.Point(145, 340);
            this.msgToSend.Multiline = true;
            this.msgToSend.Name = "msgToSend";
            this.msgToSend.Size = new System.Drawing.Size(333, 37);
            this.msgToSend.TabIndex = 1;
            this.msgToSend.TextChanged += new System.EventHandler(this.msgToSend_TextChanged);
            // 
            // sendBtn
            // 
            this.sendBtn.Enabled = false;
            this.sendBtn.Location = new System.Drawing.Point(484, 340);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(78, 37);
            this.sendBtn.TabIndex = 3;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // onlineUsersGroup
            // 
            this.onlineUsersGroup.Controls.Add(this.startChat);
            this.onlineUsersGroup.Controls.Add(this.onlineUsers);
            this.onlineUsersGroup.Location = new System.Drawing.Point(12, 12);
            this.onlineUsersGroup.Name = "onlineUsersGroup";
            this.onlineUsersGroup.Size = new System.Drawing.Size(119, 184);
            this.onlineUsersGroup.TabIndex = 4;
            this.onlineUsersGroup.TabStop = false;
            this.onlineUsersGroup.Text = "Online Users";
            // 
            // startChat
            // 
            this.startChat.Enabled = false;
            this.startChat.Location = new System.Drawing.Point(7, 155);
            this.startChat.Name = "startChat";
            this.startChat.Size = new System.Drawing.Size(106, 23);
            this.startChat.TabIndex = 1;
            this.startChat.Text = "New message";
            this.startChat.UseVisualStyleBackColor = true;
            this.startChat.Click += new System.EventHandler(this.startChat_Click);
            // 
            // onlineUsers
            // 
            this.onlineUsers.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.onlineUsers.Location = new System.Drawing.Point(7, 20);
            this.onlineUsers.Name = "onlineUsers";
            this.onlineUsers.Size = new System.Drawing.Size(106, 129);
            this.onlineUsers.TabIndex = 0;
            this.onlineUsers.UseCompatibleStateImageBehavior = false;
            this.onlineUsers.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.onlineUsers_ItemSelectionChanged);
            // 
            // activeConversations
            // 
            this.activeConversations.Location = new System.Drawing.Point(145, 13);
            this.activeConversations.Name = "activeConversations";
            this.activeConversations.SelectedIndex = 0;
            this.activeConversations.Size = new System.Drawing.Size(417, 321);
            this.activeConversations.TabIndex = 5;
            this.activeConversations.SelectedIndexChanged += new System.EventHandler(this.activeConversations_SelectedIndexChanged);
            // 
            // newGroupChat
            // 
            this.newGroupChat.Location = new System.Drawing.Point(7, 124);
            this.newGroupChat.Name = "newGroupChat";
            this.newGroupChat.Size = new System.Drawing.Size(53, 23);
            this.newGroupChat.TabIndex = 6;
            this.newGroupChat.Text = "New";
            this.newGroupChat.UseVisualStyleBackColor = true;
            //this.newGroupChat.Click += new System.EventHandler(this.newGroupChat_Click);
            // 
            // groupChatsGroup
            // 
            this.groupChatsGroup.Controls.Add(this.inviteToGroup);
            this.groupChatsGroup.Controls.Add(this.joinGroupChat);
            this.groupChatsGroup.Controls.Add(this.groupChats);
            this.groupChatsGroup.Controls.Add(this.newGroupChat);
            this.groupChatsGroup.Location = new System.Drawing.Point(12, 202);
            this.groupChatsGroup.Name = "groupChatsGroup";
            this.groupChatsGroup.Size = new System.Drawing.Size(119, 175);
            this.groupChatsGroup.TabIndex = 7;
            this.groupChatsGroup.TabStop = false;
            this.groupChatsGroup.Text = "Group Chats";
            // 
            // inviteToGroup
            // 
            this.inviteToGroup.Enabled = false;
            this.inviteToGroup.Location = new System.Drawing.Point(7, 148);
            this.inviteToGroup.Name = "inviteToGroup";
            this.inviteToGroup.Size = new System.Drawing.Size(106, 23);
            this.inviteToGroup.TabIndex = 8;
            this.inviteToGroup.Text = "Invite";
            this.inviteToGroup.UseVisualStyleBackColor = true;
            //this.inviteToGroup.Click += new System.EventHandler(this.inviteToGroup_Click);
            // 
            // joinGroupChat
            // 
            this.joinGroupChat.Enabled = false;
            this.joinGroupChat.Location = new System.Drawing.Point(60, 124);
            this.joinGroupChat.Name = "joinGroupChat";
            this.joinGroupChat.Size = new System.Drawing.Size(53, 23);
            this.joinGroupChat.TabIndex = 7;
            this.joinGroupChat.Text = "Join";
            this.joinGroupChat.UseVisualStyleBackColor = true;
            //this.joinGroupChat.Click += new System.EventHandler(this.joinGroupChat_Click);
            // 
            // groupChats
            // 
            this.groupChats.Location = new System.Drawing.Point(7, 20);
            this.groupChats.Name = "groupChats";
            this.groupChats.Size = new System.Drawing.Size(106, 99);
            this.groupChats.TabIndex = 0;
            this.groupChats.UseCompatibleStateImageBehavior = false;
            this.groupChats.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.groupChats_ItemSelectionChanged);
            // 
            // ChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(574, 387);
            this.Controls.Add(this.groupChatsGroup);
            this.Controls.Add(this.activeConversations);
            this.Controls.Add(this.onlineUsersGroup);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.msgToSend);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Chat";
            this.Text = "Chat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatRoom_FormClosed);
            this.onlineUsersGroup.ResumeLayout(false);
            this.groupChatsGroup.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl activeConversations;
        private System.Windows.Forms.Button newGroupChat;
        private System.Windows.Forms.GroupBox groupChatsGroup;
        private System.Windows.Forms.Button joinGroupChat;
        private System.Windows.Forms.ListView groupChats;
        private System.Windows.Forms.Button startChat;
        private System.Windows.Forms.Button inviteToGroup;
    }
}