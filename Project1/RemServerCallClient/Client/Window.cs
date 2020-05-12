using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Client
{
    public partial class Window : Form
    {
        int port;
        ISingleServer server;

        public Window(int Port)
        {
            port = Port;
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            server = (ISingleServer)R.New(typeof(ISingleServer));  // get reference to the singleton remote object
        }

        public int ShowRegisterRequest(string username, string password)
        {
            DialogResult dialogResult = MessageBox.Show("Do you wish to register with this username: " + username + "?", "Register Request", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                server.Register(username, password);
                return 0;
            }
            else if (dialogResult == DialogResult.No)
                return 1;

            return 1;
        }

        private void signOn(object sender, EventArgs e)
        {
            int login = server.Login(username.Text, password.Text);
            if(login == 0)
            {
                invalidLoginLabel.Visible = false;
                server.RegisterAddress(username.Text, "tcp://localhost:" + port.ToString() + "/Message");
                this.Hide();
                Chat chatRoom = new Chat(server, username.Text, port.ToString());
                chatRoom.Show();
            }
            if(login == 1)
            {
                invalidLoginLabel.Text = "Incorrect Password!";
                invalidLoginLabel.Visible = true;
            }
            if(login == 2)
            {
                invalidLoginLabel.Text = "User Already Logged In!";
                invalidLoginLabel.Visible = true;
            }
            if(login == 3)
            {
                if(ShowRegisterRequest(username.Text, password.Text) == 0)
                {
                    server.RegisterAddress(username.Text, "tcp://localhost:" + port.ToString() + "/Message");
                    this.Hide();
                    Chat chatRoom = new Chat(server, username.Text, port.ToString());
                    chatRoom.Show();
                }
                else
                {
                    invalidLoginLabel.Text = "User not found!";
                    invalidLoginLabel.Visible = true;
                } 
            }
        }

    }
    class R
    {
        private static IDictionary wellKnownTypes;

        public static object New(Type type)
        {
            if (wellKnownTypes == null)
                InitTypeCache();
            WellKnownClientTypeEntry entry = (WellKnownClientTypeEntry)wellKnownTypes[type];
            if (entry == null)
                throw new RemotingException("Type not found!");
            return Activator.GetObject(type, entry.ObjectUrl);
        }

        public static void InitTypeCache()
        {
            Hashtable types = new Hashtable();
            foreach (WellKnownClientTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            {
                if (entry.ObjectType == null)
                    throw new RemotingException("A configured type could not be found!");
                types.Add(entry.ObjectType, entry);
            }
            wellKnownTypes = types;
        }
    }



}