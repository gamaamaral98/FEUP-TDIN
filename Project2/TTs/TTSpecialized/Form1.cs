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
using System.Collections.Generic;

namespace TTSpecialized {
  public partial class Form1 : Form {
        TTProxy proxy;
        Queue<String[]> Messages = new Queue<String[]>();
        
        public Form1() {
            proxy = new TTProxy();

            InitializeComponent();
            ListenToMessageQueue();

            DataTable specializedSupervisors = proxy.GetSpecializedSupervisors();
            comboBox1.DataSource = specializedSupervisors.DefaultView;
            comboBox1.DisplayMember = "Name";
            comboBox1.BindingContext = this.BindingContext;
        }

        public void ListenToMessageQueue()
        {
            MessageQueue messageQueue = new MessageQueue();
            messageQueue.Path = @".\private$\mymsmq";
            messageQueue.ReceiveCompleted += HandleReceivedMessage;
            messageQueue.BeginReceive();
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

                msgQueue.BeginReceive();

                Messages.Enqueue(messageData);

                if (Messages.Count != 0)
                {
                    label3.Visible = true;
                    label6.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    textBox1.Visible = true;
                    comboBox1.Visible = true;
                    button2.Visible = true;

                    String[] fill = Messages.Peek();
                    problemId.Text = "Problem " + fill[0];
                    label3.Text = "Author:" + fill[1];
                    label6.Text = fill[2];
                    label4.Text = "Answer:";
                    label5.Text = "Assign to:";
                }
            }
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
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DataTable tickets = proxy.GetAllTickets();
            DataTable users = proxy.GetUsers();
            DataTable supervisors = proxy.GetSpecializedSupervisors();

            var status = 4;
            var answer = textBox1.Text;

            var ticket = tickets.Select("ID = " + problemId.Text.Substring(7));
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
            proxy.AssignTicket(ticketId, supervisorId);

            Messages.Dequeue();

            //enviar email ao worker
            sendEmail(supervisorEmail, userEmail, answer);

            if(Messages.Count != 0)
            {
                label3.Visible = true;
                label6.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                textBox1.Visible = true;
                comboBox1.Visible = true;

                String[] fill = Messages.Peek();
                problemId.Text = "Problem " + fill[0];
                label3.Text = "Author:" + fill[1];
                label6.Text = fill[2];
                label4.Text = "Answer:";
                label5.Text = "Assign to:";
                textBox1.Text = "";
            }
            else
            {
                problemId.Text = "No Tickets available to solve!";
                label3.Visible = false;
                label6.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox1.Visible = false;
                comboBox1.Visible = false;
                button2.Visible = false;
            }
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
