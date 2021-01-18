using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDS
{
    public partial class Login : Form
    {
        SqlCommand cmd;
        SqlConnection cn;
        SqlDataReader dr;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Personal Projects\ACME\DroneDeliveryService2\DDS\DDS\Database.mdf;Integrated Security=True");
            cn.Open();
        }

        private void Btnregister_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registration registration = new Registration();
            registration.ShowDialog();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtpassword.Text != string.Empty || txtusername.Text != string.Empty)
            {

                cmd = new SqlCommand("select * from tblUserLogin where username='" + txtusername.Text + "' and password='"+txtpassword.Text+"'", cn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    Home home = new Home(txtusername.Text);
                    home.Show();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("No Account avilable with this username and password ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Please enter correct username and password to login.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
