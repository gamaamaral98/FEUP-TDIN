using System.Data;
using System.ServiceModel;
using System.Windows.Forms;
using TTService;

namespace TTClient {
  public partial class Form1 : Form {
    TTProxy proxy;

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
    }

    // Manual proxy to the service (in alternative to direct HTTP requests)
    class TTProxy : ClientBase<ITTService>, ITTService {
        public DataTable GetUsers() {
            return Channel.GetUsers();
        }

        public DataTable GetTickets(string author) {
            return Channel.GetTickets(author);
        }

        public int AddTicket(string author, string desc) {
            return Channel.AddTicket(author, desc);
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
    
    }

}
