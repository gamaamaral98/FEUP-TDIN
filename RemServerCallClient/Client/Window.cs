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
            if (server.Login(username.Text, password.Text) != 1)
            {
                invalidLoginLabel.Visible = false;
                server.RegisterAddress(username.Text, "tcp://localhost:" + port.ToString() + "/Message");
                this.Hide();
                //ChatRoom chatRoom = new ChatRoom(server, username.Text);
                //chatRoom.Show();
            }
            else
                invalidLoginLabel.Visible = true;

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

    public class RemMessage : MarshalByRefObject, IClientRem
    {
        private Window win;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void PutMyForm(Window form)
        {
            win = form;
        }

        public void SomeMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}