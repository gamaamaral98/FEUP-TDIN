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
                invalidLoginLabel.Text = "Registering User";
                invalidLoginLabel.Visible = true;
                server.RegisterAddress(username.Text, "tcp://localhost:" + port.ToString() + "/Message");
                this.Hide();
                Chat chatRoom = new Chat(server, username.Text, port.ToString());
                chatRoom.Show();
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