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
    public partial class Home : Form
    {
        SqlCommand cmd;
        SqlConnection cn;
        SqlDataReader dr;

        string _username = string.Empty;

        public Home(string username)
        {
            this._username = username;
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            SetVisibility(false);
        }

        #region Button Events

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SetVisibility(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetVisibility(false);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFirstName.Text)) { MessageBox.Show("Please enter first name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (string.IsNullOrEmpty(txtFamilyName.Text)) { MessageBox.Show("Please enter family name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (string.IsNullOrEmpty(txtPostalCode.Text))
                {
                    MessageBox.Show("Please enter postal code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }

                cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Personal Projects\ACME\DroneDeliveryService2\DDS\DDS\Database.mdf;Integrated Security=True");
                cn.Open();

                cmd = new SqlCommand("update tblUserLogin set firstname=@firstname, familyname=@familyname, postalcode=@postalcode where username='" + _username + "'", cn);
                cmd.Parameters.AddWithValue("firstname", txtFirstName.Text);
                cmd.Parameters.AddWithValue("familyname", txtFamilyName.Text);
                cmd.Parameters.AddWithValue("postalcode", txtPostalCode.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Your account is updated successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetVisibility(false);
            }
            catch (Exception ex)
            {
                SetVisibility(true);
                MessageBox.Show("Unexpected error occured, please try later ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
        }

        #endregion

        #region Private methods

        private void SetVisibility(bool isEditMode)
        {
            pnlEditable.Visible = btnUpdate.Visible = btnCancel.Visible = isEditMode;
            btnEdit.Visible = btnLogout.Visible = pnlReadOnly.Visible = !isEditMode;

            PopulateData();
        }

        private void PopulateData()
        {
            try
            {
                cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Personal Projects\ACME\DroneDeliveryService2\DDS\DDS\Database.mdf;Integrated Security=True");
                cn.Open();

                cmd = new SqlCommand("select * from tblUserLogin where username='" + _username + "'", cn);

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lblUserName.Text = dr["username"].ToString();
                    lblFirstName.Text = txtFirstName.Text = dr["firstname"].ToString();
                    lblFamilyName.Text = txtFamilyName.Text = dr["familyname"].ToString();
                    lblPostalCode.Text = txtPostalCode.Text = dr["postalcode"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error occured, please try later ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
                dr.Close();
            }
        }

        #endregion
    }
}
