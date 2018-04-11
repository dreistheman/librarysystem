using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibraryUI
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {

            if (txt_User.Text == "libuser" && txt_Pass.Text == "libpass")
            {
                MessageBox.Show("Login Successful!");
                Form1 libForm = new Form1();
                libForm.Show();
            }

            else if (txt_User.Text == "staffuser" && txt_Pass.Text == "staffpass")
            {
                MessageBox.Show("Login Successful!");
                StaffForm stafForm = new StaffForm();
                stafForm.Show();
            }

            else
            {
                MessageBox.Show("Login is incorrect! Please login again.");
            }
        }
    }
}
