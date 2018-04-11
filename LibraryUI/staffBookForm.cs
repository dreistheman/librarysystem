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
    public partial class staffBookForm : Form
    {

        DatabaseUtilities dbUtils = new DatabaseUtilities();
        public staffBookForm()
        {
            InitializeComponent();
        }

        private void staffBookForm_Load(object sender, EventArgs e)
        {
            refreshTable();
            txtBookID.Enabled = false;
            txt_Title.Enabled = false;
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

        private void btn_Cancel_Click(object sender, EventArgs e)
        {

            this.Close();
            StaffForm form = new StaffForm();
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
                txt_Title.Text = row.Cells[1].Value.ToString();

            }
        }
    }
    }
