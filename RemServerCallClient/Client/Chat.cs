using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Chat : Form
    {
        ISingleServer server;
        String username;
        String address;
        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void LVRemDelegate(String lvItem);
        AlterEventRepeater evRepeater;
        RemMessage r;

        List<string> usersList;
        Hashtable activeUsers;
        Hashtable chatTabs;
        String activeUser;
        private IClientRem activeUserRemObj;


        public Chat(ISingleServer server, String username, String port)
        {
            InitializeComponent();
            this.server = server;
            this.username = username;
            this.address = "tcp://localhost:" + port.ToString() + "/Message";
            this.Text = " Chat - " + username;
            onlineUsers.View = View.List;
            activeUsers = new Hashtable();
            chatTabs = new Hashtable();
            UpdateOnlineUsers();
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            server.alterEvent += new AlterDelegate(evRepeater.Repeater);
            r = (RemMessage)RemotingServices.Connect(typeof(RemMessage), "tcp://localhost:" + port.ToString() + "/Message");    // connect to the registered my remote object here
            r.PutMyForm(this);
        }

        public void UpdateOnlineUsers()
        {
            usersList = server.getUsers();
            foreach (string key in usersList)
            {
                if (key != username)
                    onlineUsers.Items.Add(key.ToString());
            }
        }

        public void DoAlterations(Operation op, String username)
        {
            LVAddDelegate lvAdd;
            LVRemDelegate lvRem;

            switch (op)
            {
                case Operation.Add:
                    usersList.Add(username);
                    lvAdd = new LVAddDelegate(onlineUsers.Items.Add);
                    ListViewItem lvItem = new ListViewItem(new string[] { username });
                    BeginInvoke(lvAdd, new object[] { lvItem });
                    break;
                case Operation.Remove:
                    lvRem = new LVRemDelegate(RemoveUser);
                    BeginInvoke(lvRem, new object[] { username });
                    break;
            }
        }

        private void RemoveUser(String username)
        {
            usersList.Remove(username);
            if (chatTabs.Contains(username))
            {
                Tab tab = (Tab)chatTabs[username];
                tab.ChangeMsgStatus(username);
                if (activeUser.Equals(username))
                    DisableSend();
            }

            foreach (ListViewItem lvI in onlineUsers.Items)
                if (lvI.SubItems[0].Text == username)
                {
                    lvI.Remove();
                    break;
                }
        }

        private void ChatRoom_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);
            DisableSend();
            server.Logout(username);
        }

        private void onlineUsers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            String tabUsername = e.Item.Text;
            if (onlineUsers.SelectedItems.Count == 0)
                startChat.Enabled = false;
            else if (chatTabs.Contains(tabUsername))
            {
                Tab tab = (Tab)chatTabs[tabUsername];
                startChat.Enabled = false;
                if (tab.status) //offline
                    DisableSend();
                else
                    msgToSend.Enabled = true;
            }
            else
                startChat.Enabled = true;
        }

        public void PutMessage(Message msg)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { PutMessage(msg); });
            else
            {
                Tab tab;
                if (chatTabs.Contains(msg.tab))
                {
                    tab = (Tab)chatTabs[msg.tab];
                    this.listMessages.Items.Add(tab.AddReceiverText1(msg.msg, msg.sender));
                    this.listMessages.Items.Add("");

                }
                else
                {
                    tab = new Tab(msg.tab);
                    this.listMessages.Items.Add(tab.AddReceiverText1(msg.msg, msg.sender));
                    this.listMessages.Items.Add("");
                    chatTabs.Add(msg.sender, tab);
                    ChangeActiveUser(msg.tab);
                    tab.status = false;
                    msgToSend.Enabled = true;
                }
            }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            activeUserRemObj.SomeMessage(new Message(username, msgToSend.Text, username));
            Tab tab = (Tab)chatTabs[activeUser];
            this.listMessages.Items.Add(tab.AddSenderText1(msgToSend.Text, username));
            this.listMessages.Items.Add("");
            msgToSend.Clear();
        }

        private void ChangeActiveUser(String username)
        {
            activeUserRemObj = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[username]);
            activeUser = username;
        }

        private void msgToSend_TextChanged(object sender, EventArgs e)
        {
            sendBtn.Enabled = !string.IsNullOrWhiteSpace(msgToSend.Text);
        }

        private void DisableSend()
        {
            msgToSend.Enabled = false;
            sendBtn.Enabled = false;
        }

        public void ShowChatRequest(String requester)
        {
            DialogResult dialogResult = MessageBox.Show(requester + " wants to chat with you. Do you accept?", "Conversation request", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                server.AcceptConversation(requester, username);
            else if (dialogResult == DialogResult.No)
                server.RefuseConversation(requester, username);
        }

        public void RequestAccepted(String username, String address)
        {
        
            Tab tab = (Tab)chatTabs[username];
            //tab.AcceptConversation(username);
            tab.status = false;
            activeUsers.Add(username, address);

            BeginInvoke(new Action(() =>
            {
                ChangeActiveUser(username);
                msgToSend.Enabled = true;
                this.listMessages.Items.Add(tab.AcceptConversation1(username));
                this.listMessages.Items.Add("");
            }));
        }

        public void RequestRefused(String username)
        {
            Tab tab = (Tab)chatTabs[username];
            this.listMessages.Items.Add(tab.RefuseConversation1(username));
        }

        public void AddActiveUser(String username, String address)
        {
            activeUsers.Add(username, address);
        }

        private void startChat_Click(object sender, EventArgs e)
        {
            String tabUsername = onlineUsers.SelectedItems[0].Text;
            Tab tab = tab = new Tab(tabUsername);
            chatTabs.Add(tabUsername, tab);
            this.listMessages.Items.Add(tab.ConversationRequest1());
            Request request = new Request(server, username, tabUsername);
            Thread oThread = new Thread(new ThreadStart(request.Send));
            oThread.Start();
            if (tab.status)
                DisableSend();
            else
                msgToSend.Enabled = true;
            startChat.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class RemMessage : MarshalByRefObject, IClientRem
    {
        private Chat win;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void PutMyForm(Chat form)
        {
            win = form;
        }

        public void ReceiveRequest(String username)
        {
            win.ShowChatRequest(username);
        }

        public void AcceptConversation(String username, String address)
        {
            win.RequestAccepted(username, address);
        }

        public void ReceiveAdress(String username, String address)
        {
            win.AddActiveUser(username, address);
        }

        public void RefuseConversation(string username)
        {
            win.RequestRefused(username);
        }

        public void SomeMessage(Message message)
        {
            win.PutMessage(message);
        }
    }

    public class Request
    {
        ISingleServer server;
        String username, tabUsername;

        public Request(ISingleServer server, String username, String tabUsername)
        {
            this.server = server;
            this.username = username;
            this.tabUsername = tabUsername;
        }

        public void Send()
        {
            server.InviteToConversation(username, tabUsername);
        }
    };


}
