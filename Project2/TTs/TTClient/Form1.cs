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

    private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e) {
      proxy = new TTProxy();
      
      DataTable users = proxy.GetUsers();
      DataTable supervisors = proxy.GetSupervisors();
      DataTable specializedSupervisors = proxy.GetSpecializedSupervisors();
      string user = (listBox1.SelectedIndex).ToString();   // the Author id as a string
      
      if((listBox1.SelectedIndex) >= 0 && (listBox1.SelectedIndex) < users.Rows.Count)
      {
            panel1.Visible = false;
            dataGridView1.Visible = true;
            DataTable tickets = proxy.GetTickets(user);
            dataGridView1.DataSource = tickets;             // display all the tickets for the specified user in a data grid
      }
      else if (listBox1.SelectedIndex < (supervisors.Rows.Count + users.Rows.Count))
      { 
                dataGridView1.Visible = false;
                panel1.Visible = true;
                textBox1.Visible = true;
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
    
    }

}
