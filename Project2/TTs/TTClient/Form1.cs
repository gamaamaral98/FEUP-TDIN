using System.Data;
using System.ServiceModel;
using System.Windows.Forms;
using TTService;
using System.Data.SqlClient;
using System.Net.Mail;

namespace TTClient {
  public partial class Form1 : Form {
    TTProxy proxy;
        private SqlConnection con;

    public Form1() {
      int k;

      InitializeComponent();
      proxy = new TTProxy();
      DataTable users = proxy.GetUsers();
      DataTable supervisors = proxy.GetSupervisors();
      DataTable specializedSupervisors = proxy.GetSpecializedSupervisors();

      for (k = 0; k < users.Rows.Count; k++)
        listBox1.Items.Add(users.Rows[k][1] + " - Worker");   // Row 0 is empty; the author name is in column 1

      for (k = 0; k < supervisors.Rows.Count; k++)
        listBox1.Items.Add(supervisors.Rows[k][1] + " - Supervisor");

      for (k = 0; k < specializedSupervisors.Rows.Count; k++)
        listBox1.Items.Add(specializedSupervisors.Rows[k][1] + " - " + specializedSupervisors.Rows[k][2]);
    }

        private void hide_Supervisor()
        {
            label2.Text = "No ticket assigned.";
            comboBox2.Visible = true;
            button3.Visible = true;
            label3.Visible = false;
            label6.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            comboBox1.Visible = false;
            textBox1.Visible = false;
        }
        private void show_Supervisor()
        {
            label3.Visible = true;
            label6.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            comboBox1.Visible = true;
            textBox1.Visible = true;
            comboBox2.Visible = false;
            button3.Visible = false;
        }

        private void hide_SpecializedSupervisor()
        {
            label11.Text = "No ticket assigned.";
            label10.Visible = false;
            label7.Visible = false;
            label9.Visible = false;
            button4.Visible = false;
            textBox2.Visible = false;
        }
        private void show_SpecializedSupervisor()
        {
            label10.Visible = true;
            label7.Visible = true;
            label9.Visible = true;
            button4.Visible = true;
            textBox2.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e) {
            proxy = new TTProxy();
      
            DataTable users = proxy.GetUsers();
            DataTable supervisors = proxy.GetSupervisors();
            DataTable specializedSupervisors = proxy.GetSpecializedSupervisors();

            string user = (listBox1.SelectedIndex).ToString();   // the Author id as a string

            if ((listBox1.SelectedIndex) >= 0 && (listBox1.SelectedIndex) < users.Rows.Count)
            {
                panel1.Visible = false;
                dataGridView1.Visible = true;
                DataTable tickets = proxy.GetTickets(user);
                dataGridView1.DataSource = tickets;             // display all the tickets for the specified user in a data grid
            }
            else if (listBox1.SelectedIndex < (supervisors.Rows.Count + users.Rows.Count))
            {
                DataTable tickets = proxy.GetAllTickets();

                var supervisorIndex = listBox1.SelectedIndex - users.Rows.Count;
                var supervisor = supervisors.Select("ID = " + supervisorIndex.ToString());
                
                if(supervisor[0]["TicketId"].ToString() != "")
                {
                    show_Supervisor();
                   
                    var ticketId = supervisor[0]["TicketId"];
                    var ticket = tickets.Select("ID = " + ticketId.ToString());
                    label2.Text = "Problem " + ticketId.ToString();
                    label3.Text = "Author: " + (users.Select("ID = " + ticket[0]["Author"]))[0]["Name"];
                    label6.Text = ticket[0]["Problem"].ToString();
                    label4.Text = "Answer: ";
                    label5.Text = "Select a specialized supervisor:";

                    comboBox1.DataSource = specializedSupervisors.DefaultView;
                    comboBox1.DisplayMember = "Name";
                    comboBox1.BindingContext = this.BindingContext;
                    
                    dataGridView1.Visible = false;
                    panel1.Visible = true;
                    panel2.Visible = false;
                }
                else
                {
                    hide_Supervisor();
                    DataTable unassignedTickets = proxy.GetAllUnassignedTickets();

                    comboBox2.DataSource = unassignedTickets.DefaultView;
                    comboBox2.DisplayMember = "Problem";
                    comboBox2.BindingContext = this.BindingContext;

                    dataGridView1.Visible = false;
                    panel1.Visible = true;
                    panel2.Visible = false;
                }
            }
            else
            {
                DataTable tickets = proxy.GetAllTickets();

                var specializedSupervisorIndex = listBox1.SelectedIndex - (users.Rows.Count + supervisors.Rows.Count);
                var specializedSupervisor = specializedSupervisors.Select("ID = " + specializedSupervisorIndex.ToString());
               
                if (specializedSupervisor[0]["TicketId"].ToString() != "")
                {
                    show_SpecializedSupervisor();

                    var ticketId = specializedSupervisor[0]["TicketId"];
                    var ticket = tickets.Select("ID = " + ticketId.ToString());
                    label11.Text = "Problem " + ticketId.ToString();
                    label7.Text = "Author: " + (users.Select("ID = " + ticket[0]["Author"]))[0]["Name"];
                    label10.Text = ticket[0]["Problem"].ToString();
                    label9.Text = "Answer: ";

                    dataGridView1.Visible = false;
                    panel1.Visible = false;
                    panel2.Visible = true;
                }
                else
                {
                    hide_SpecializedSupervisor();
                    dataGridView1.Visible = false;
                    panel1.Visible = false;
                    panel2.Visible = true;
                }
      
            }

    }

        private void button3_Click(object sender, System.EventArgs e)
        {
            DataTable tickets = proxy.GetAllTickets();
            DataTable users = proxy.GetUsers();

            var ticket = tickets.Select("Problem = " + "'" + comboBox2.Text + "'");
            var supervisorId = (listBox1.SelectedIndex - users.Rows.Count).ToString();
            var ticketId = ticket[0]["Id"].ToString();

            proxy.updateStatus(ticketId, 2, "");
            proxy.AssignTicket(ticketId, supervisorId);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            DataTable tickets = proxy.GetAllTickets();
            DataTable users = proxy.GetUsers();
            DataTable supervisors = proxy.GetSupervisors();

            var status = 4;
            var answer = textBox1.Text;

            var ticket = tickets.Select("ID = " + label2.Text.Substring(7));
            var ticketId = ticket[0]["Id"].ToString();
            var ticketAuthorId = ticket[0]["Author"].ToString();

            var supervisorId = (listBox1.SelectedIndex - users.Rows.Count).ToString();
            var supervisor = supervisors.Select("ID = " + supervisorId);
            var supervisorEmail = supervisor[0]["Email"].ToString();

            var user = users.Select("ID = " + ticketAuthorId);
            var userEmail = user[0]["Email"].ToString();;

            //update do status + answer
            proxy.updateStatus(ticketId, status, answer);

            //unassign no supervisor
            proxy.AssignTicket("NULL", supervisorId);

            //enviar email ao worker
            //sendEmail(supervisorEmail, userEmail, answer);
           
        }

        private void sendEmail(string sender, string receiver, string answer)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(sender.Trim());
            mail.To.Add(receiver.Trim());
            mail.Subject = "Response to problem submitted";
            mail.Body = answer;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            label5.Visible = false;
            comboBox1.Visible = false;
            button2.Visible = false;
            textBox1.Text = "Email sent!";
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
