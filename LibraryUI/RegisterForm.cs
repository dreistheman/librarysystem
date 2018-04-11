using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LibraryUI
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnLendConfirm_Click(object sender, EventArgs e)
        {
            /*SqlConnection sc = new SqlConnection();
            SqlCommand com = new SqlCommand();
            sc.ConnectionString = ("Data Source = DESKTOP - A0H7M2N\\SQLEXPRESS; Initial Catalog = general; Integrated Security = True");
            sc.Open();
            com.Connection = sc;
            com.CommandText = ("INSERT INTO Register (ContactNo, borrower_name) VALUES ('" + txtNumber.Text + "''" + txtName.Text + "');");
            com.ExecuteNonQuery();
            sc.Close();
            MessageBox.Show("Register Successful!");*/
            DatabaseUtilities dbUtils = new DatabaseUtilities();

            String name = txtName.Text;
            int contactNo = Int32.Parse(txtNumber.Text); //RESIDENT NUMBER SIZE

            try
            {
                dbUtils.addUser(new User(contactNo, name));
            }
            catch
            {
                MessageBox.Show("Resident ID is already in use, Please Try Again!");
                return;
            }


            MessageBox.Show("User is now Registered!", "Thank You.");
            //this.bookTableAdapter1.Fill(this.generalDataSet3.Book);
            // refreshTable();
            // reset();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnLendCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 libForm = new Form1();
            libForm.Show();
        }
    }
}
