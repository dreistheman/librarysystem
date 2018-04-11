using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Threading;

namespace LibraryUI
{
    public partial class StaffForm : Form
    {

        DatabaseUtilities dbUtils = new DatabaseUtilities();
        static Boolean addWasClicked, updateWasClicked;
        int id;
        String title, author, genre;
        public StaffForm()
        {
            InitializeComponent();

            deselectCells();
            txtBName.Text = String.Empty;
            txtContactNo.Text = String.Empty;
        }

        private void StaffForm_Load(object sender, EventArgs e)
        {
            
            // TODO: This line of code loads data into the 'generalDataSet3.Book' table. You can move, or remove it, as needed.
            //this.bookTableAdapter1.Fill(this.generalDataSet3.Book);
            refreshTable();
            deselectCells();
            //this.bookTableAdapter.Fill(this.generalDataSet.Book);
            datagridview1.ReadOnly = true;
            
            reset();
        }


        private void dgvBooks_SelectionChanged(object sender, EventArgs e)
        {

            DataGridViewCell cell = null;

            foreach (DataGridViewCell selectedCell in datagridview1.SelectedCells)
            {
                cell = selectedCell;

                break;

            }

            if (cell != null)
            {
                DataGridViewRow row = cell.OwningRow;
                txtID.Text = row.Cells[0].Value.ToString();
                txtTitle.Text = row.Cells[1].Value.ToString();

            }
        }


        public void refreshTable()
        {
            SqlConnection conn = DatabaseUtilities.getConnection();
            SqlCommand command = new SqlCommand(" select * from Book", conn);
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                datagridview1.DataSource = bSource;
                sda.Update(dbdataset);
                //dgvBooks.AutoResizeColumns();
                //dgvBooks.AutoSizeRowsMode =
                //DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void reset()
        {
            deselectCells();
            setLendEnabled(false);
            setButtonsEnabled(true);
            datagridview1.Enabled = true;





        }


        public void setButtonsEnabled(bool set)
        {
            btn_ViewBorrowedBooks.Enabled = set;
        }

        public void deselectCells()
        {
            for (int i = 0; i < datagridview1.SelectedCells.Count; i++)
            {
                datagridview1.SelectedCells[i].Selected = false;
            }
        }

        public void setLendEnabled(bool set)
        {
            groupBox1.Enabled = set;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void dgvBooks_SelectionChanged_1(object sender, EventArgs e)
        {
            DataGridViewCell cell = null;

            foreach (DataGridViewCell selectedCell in datagridview1.SelectedCells)
            {
                cell = selectedCell;

                break;

            }

            if (cell != null)
            {
                DataGridViewRow row = cell.OwningRow;
                txtID.Text = row.Cells[0].Value.ToString();
                txtTitle.Text = row.Cells[1].Value.ToString();

            }
        }

        private void txtContactNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int resNo = Int32.Parse(txtContactNo.Text);
                User user = dbUtils.checkIfRegistered(resNo);
                if (user != null)
                {
                    txtBName.Text = user.Name;
                }
                else
                {
                    txtBName.Text = String.Empty;
                }

            }
            catch (Exception ex)
            {
                txtBName.Text = String.Empty;
            }

        }

        private void btn_ViewBorrowedBooks_Click(object sender, EventArgs e)
        {
            staffBookForm frm = new staffBookForm();
            frm.Show();
            this.Hide();
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {

            String dueDate = dateTimePicker1.Value.Date.ToString("MM dd yyyy");
            String dateBorrowed = DateTime.Today.ToString("MM dd yyyy");
            try
            {
                if (txtBName.Text == "")
                {
                    MessageBox.Show("Error", "Please enter a valid borrower");
                }
                else
                {

                    String name = txtBName.Text;
                    int contactNo = Int32.Parse(txtContactNo.Text); //RESIDENT NUMBER SIZE
                    title = txtTitle.Text;
                    id = Int32.Parse(txtID.Text);
                    dbUtils.addBorrowedBook(new BorrowedBook(id, title, name, contactNo, dateBorrowed, dueDate));
                    MessageBox.Show("Book borrowed!", "Thank you");
                    refreshTable();
                    reset();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }


        }

        private void btn_LendBook_Click(object sender, EventArgs e)
        {
            txtBName.Enabled = false;

            if (txtID.Text == "")
            {
                MessageBox.Show("Please choose a book above");
            }
            else
            {

                id = Int32.Parse(txtID.Text);
                if (dbUtils.checkIfAvailable(id))
                {
                    MessageBox.Show("Sorry! Book is BORROWED already.", "Error");
                    return;
                }
                else if (dbUtils.checkIfCondemned(id))
                {
                    MessageBox.Show("Sorry! Book is CONDEMNED already.", "Error");
                    return;
                }
                else
                {
                    setLendEnabled(true);
                    setButtonsEnabled(false);
                    datagridview1.Enabled = false;
                }


            }

        }

    }
}
