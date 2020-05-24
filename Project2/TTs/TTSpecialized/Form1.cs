using System.Data;
using System.ServiceModel;
using System.Windows.Forms;
using TTService;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Messaging;
using System;

namespace TTSpecialized {
  public partial class Form1 : Form {
    TTProxy proxy;
    private SqlConnection con;
    string msgID = "teste";
    string msgAuthor = "";
    string msgProb = "";

        public Form1() {
      int k;

      InitializeComponent();
      ListenToMessageQueue();

            proxy = new TTProxy();
      DataTable users = proxy.GetUsers();
      DataTable supervisors = proxy.GetSupervisors();


        DataTable specializedSupervisors = proxy.GetSpecializedSupervisors();


        DataTable tickets = proxy.GetAllTickets();

        var supervisorIndex = 1;
        var supervisor = supervisors.Select("ID = " + 0.ToString());

        show_Supervisor();

        var ticketId = supervisor[0]["TicketId"];
        var ticket = tickets.Select("ID = " + ticketId.ToString());
        label2.Text = "Problem " + ticketId.ToString();
        label3.Text = "Author: " + (users.Select("ID = " + ticket[0]["Author"]))[0]["Name"];
        label6.Text = ticket[0]["Problem"].ToString();
        label4.Text = "Answer: ";
        label5.Text = "Specialized Supervisor Name:";

        comboBox1.DataSource = specializedSupervisors.DefaultView;
        comboBox1.DisplayMember = "Name";
        comboBox1.BindingContext = this.BindingContext;

        panel1.Visible = true;
        }

        public void ListenToMessageQueue()
        {
            MessageQueue messageQueue = new MessageQueue();
            messageQueue.Path = @".\private$\myMSMQ";
            //messageQueue.ReceiveCompleted += HandleReceivedMessage;
            //messageQueue.BeginReceive();
        }

        public void HandleReceivedMessage(object obj, ReceiveCompletedEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    HandleReceivedMessage(obj, args);
                });
            }
            else
            {
                MessageQueue msgQueue = (MessageQueue)obj;
                System.Messaging.Message newMessage = null;

                newMessage = msgQueue.EndReceive(args.AsyncResult);
                newMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(String[]) });

                String[] messageData = (String[])newMessage.Body;
                msgID = messageData[0];
                msgAuthor = messageData[1];
                msgProb = messageData[2];


                msgQueue.BeginReceive();
            }
        }

        private void show_Supervisor()
        {
            label3.Visible = true;
            label6.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            button2.Visible = true;
            comboBox1.Visible = true;
            textBox1.Visible = true;
        }


        private void sendEmail(string sender, string receiver, string answer)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential("tdinfeup2020@gmail.com", "tdinfeup2020!#$");

            MailMessage mail = new MailMessage();
            mail.To.Add(receiver.Trim());
            mail.From = new MailAddress(sender.Trim());
            mail.Subject = "Answer";
            mail.Body = answer;

            SmtpServer.Send(mail);

            label5.Visible = false;
            comboBox1.Visible = false;
            button2.Visible = false;
            textBox1.Text = "Email sent!";
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DataTable tickets = proxy.GetAllTickets();
            DataTable users = proxy.GetUsers();
            DataTable supervisors = proxy.GetSpecializedSupervisors();

            var status = 4;
            var answer = textBox1.Text;

            var ticket = tickets.Select("ID = " + label2.Text.Substring(7));
            var ticketId = ticket[0]["Id"].ToString();
            var ticketAuthorId = ticket[0]["Author"].ToString();

            var supervisorId = comboBox1.SelectedIndex.ToString();
            var supervisor = supervisors.Select("ID = " + supervisorId);
            var supervisorEmail = supervisor[0]["Email"].ToString();

            var user = users.Select("ID = " + ticketAuthorId);
            var userEmail = user[0]["Email"].ToString(); ;

            //update do status + answer
            proxy.updateStatus(ticketId, status, answer);

            //unassign no supervisor
            proxy.AssignTicket("NULL", supervisorId);

            //enviar email ao worker
            sendEmail(supervisorEmail, userEmail, answer);
        }
    }

    // Manual proxy to the service (in alternative to direct HTTP requests)
    class TTProxy : ClientBase<ITTService>, ITTService {
        public DataTable GetUsers() {
            return Channel.GetUsers();
        }

        public DataTable GetTickets(string author) {
            return Channel.GetTickets(author);
        }

        public int AddTicket(string author, string desc, string title)
        {
            return Channel.AddTicket(author, desc, title);
        }

        public int AssignTicket(string ticketId, string supervisorId)
        {
            return Channel.AssignTicket(ticketId, supervisorId);
        }

        public int updateStatus(string ticketId, int status, string answer)
        {
            return Channel.updateStatus(ticketId, status, answer);
        }

        public DataTable GetSupervisors()
        {
            return Channel.GetSupervisors();
        }
        public DataTable GetSpecializedSupervisors()
        {
            return Channel.GetSpecializedSupervisors();
        }

        public DataTable GetAllTickets()
        {
            return Channel.GetAllTickets();
        }

        public DataTable GetAllUnassignedTickets()
        {
            return Channel.GetAllUnassignedTickets();
        }
    
    }

}
