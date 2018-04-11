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
    public partial class BookForm : Form
    {
        DatabaseUtilities dbUtils = new DatabaseUtilities();

        public BookForm()
        {
            InitializeComponent();
            Text = "BookForm";
        }

        private void BookForm_Load(object sender, EventArgs e)
        {

           //this.borrowed_BookTableAdapter.Fill(this.generalDataSet1.Borrowed_Book);
            refreshTable();
            txtBookID.Enabled = false;
            txtTitle.Enabled = false;


            
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtBookID.Text);
            dbUtils.returnBook(id);
            MessageBox.Show("Book Returned!");
            //this.borrowed_BookTableAdapter.Fill(this.generalDataSet1.Borrowed_Book);
            refreshTable();

        }

        private void dgvBorrowedBooks_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = null;

            foreach (DataGridViewCell selectedCell in dgvBorrowedBooks.SelectedCells)
            {
                cell = selectedCell;
                break;

            }

            if (cell != null)
            {
                DataGridViewRow row = cell.OwningRow;
                txtBookID.Text = row.Cells[0].Value.ToString();
                txtTitle.Text = row.Cells[1].Value.ToString();


            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(txtSearch.Text == "")
            {
                //this.borrowed_BookTableAdapter.Fill(this.generalDataSet1.Borrowed_Book);
                refreshTable();
            }
            if(cmbChoice.SelectedItem == null)
            {
                
                MessageBox.Show("Please pick search filter");
                cmbChoice.Focus();
                return;
            }
            
            String id = txtBookID.Text;
            String title = txtTitle.Text;
            String text = txtSearch.Text;
            String choice = cmbChoice.SelectedItem.ToString();
            dbUtils.searchBorrowedBook(text, choice, dgvBorrowedBooks);

            
        }

        private void cmbChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
        }

        public void refreshTable()
        {
            SqlConnection conn = DatabaseUtilities.getConnection();
            SqlCommand command = new SqlCommand(" select * from Borrowed_Book", conn);
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dgvBorrowedBooks.DataSource = bSource;
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

        private void dgvBorrowedBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvBorrowedBooks_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
