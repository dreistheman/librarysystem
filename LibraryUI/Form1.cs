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
    public partial class Form1 : Form
    {
        
        DatabaseUtilities dbUtils = new DatabaseUtilities();
        static Boolean addWasClicked, updateWasClicked, condemnWasClicked;
        int id;
        String title, author, genre;

        
        public Form1()
        {
            InitializeComponent();
         

        }

        private void button5_Click(object sender, EventArgs e)
        {
            BookForm frm = new BookForm();
            frm.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'generalDataSet3.Book' table. You can move, or remove it, as needed.
            //this.bookTableAdapter1.Fill(this.generalDataSet3.Book);
            refreshTable();
            //this.bookTableAdapter.Fill(this.generalDataSet.Book);
            dgvBooks.ReadOnly = true;
            reset();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            deselectCells();
            addWasClicked = true;
            setManagementEnabled(true);
            setButtonsEnabled(false);
            dgvBooks.Enabled = false;
            clear();
            groupBox2.Text = "Add Book";

        }

        private void dgvBooks_SelectionChanged(object sender, EventArgs e)
        {
           
            DataGridViewCell cell = null;

            foreach (DataGridViewCell selectedCell in dgvBooks.SelectedCells)
            {
                cell = selectedCell;
                
                break;

            }
            
            if (cell != null)
            {
                DataGridViewRow row = cell.OwningRow;
                txtID.Text = row.Cells[0].Value.ToString();
                txtTitle.Text = row.Cells[1].Value.ToString();
                txtAuthor.Text = row.Cells[2].Value.ToString();
                txtGenre.Text = row.Cells[3].Value.ToString();
        
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "") 
            {
                MessageBox.Show("Pick the book to be updated above");
            }
            else {
                deselectCells();
                updateWasClicked = true;
                setManagementEnabled(true);
                setButtonsEnabled(false);
                //dgvBooks.Enabled = false;
                groupBox2.Text = "Update Book";
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnManageConfirm_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Please fill up all fields.");
            }
            else
            {
                id = Int32.Parse(txtID.Text);
                title = txtTitle.Text;
                author = txtAuthor.Text;
                genre = txtGenre.Text;

                if (addWasClicked)
                {

                   try
                    {
                        dbUtils.addBook(new Book(id, title, author, genre, "Available"));

                    }
                    catch
                    {
                        MessageBox.Show("Book ID already exists!", "Error");
                        return;
                    }
                    MessageBox.Show("Book added!");
                    //this.bookTableAdapter1.Fill(this.generalDataSet3.Book);
                    refreshTable();
                    addWasClicked = false;
                    reset();


                }
                else if (updateWasClicked)
                {
                    dbUtils.updateBook(new Book(id, title, author, genre, "Available"));
                    MessageBox.Show("Book updated");
                    //this.bookTableAdapter1.Fill(this.generalDataSet3.Book);
                    refreshTable();
                    updateWasClicked = false;
                    reset();
                }
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
                dgvBooks.DataSource = bSource;
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

        private void btnManageCancel_Click(object sender, EventArgs e)
        {
            reset();
            setButtonsEnabled(true);
        }

        public void setManagementEnabled(bool set)
        {
            groupBox2.Enabled = set;
        }

        private void btnLendBook_Click(object sender, EventArgs e)
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
                else if(dbUtils.checkIfCondemned(id))
                {
                    MessageBox.Show("Sorry! Book is CONDEMNED already.", "Error");
                    return;
                }
                else
                {
                    setLendEnabled(true);
                    setButtonsEnabled(false);
                    dgvBooks.Enabled = false;
                }
               

            }
        }

        private void btnLendCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        public void setLendEnabled(bool set)
        {
            groupBox1.Enabled = set;
        }
        public void setButtonsEnabled(bool set)
        {
            btnAdd.Enabled = set;
            btnUpdate.Enabled = set;
            btnExport.Enabled = set;
            btnLendBook.Enabled = set;
        }

        private void btnLendConfirm_Click(object sender, EventArgs e)
        {
            //dateTimePicker1 = new DateTimePicker();
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
                    dbUtils.addBorrowedBook(new BorrowedBook(id, title, name, contactNo, dateBorrowed,dueDate));
                    MessageBox.Show("Book borrowed!", "Thank you");
                    refreshTable();
                    reset();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            
            
           
            //this.bookTableAdapter1.Fill(this.generalDataSet3.Book);
  
        }

        public void clear()
        {
            txtID.Text = String.Empty;
            txtTitle.Text = String.Empty;
            txtAuthor.Text = String.Empty;
            txtGenre.Text = String.Empty;
            txtBName.Text = String.Empty;
            txtContactNo.Text = String.Empty;

        }
        public void deselectCells()
        {
            for (int i = 0; i < dgvBooks.SelectedCells.Count; i++)
            {
                dgvBooks.SelectedCells[i].Selected = false;
            }
        }
        public void reset()
        {
            deselectCells();
            setManagementEnabled(false);
            setLendEnabled(false);
            setButtonsEnabled(true);
            dgvBooks.Enabled = true;
            clear();
            groupBox2.Text = "Manage Book";
            
            



        }

        private void txtContactNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm RegForm = new RegisterForm();
            RegForm.Show();
        }
        /*
        private void checkIfRegistered(object sender, EventArgs e)
        {
            int resNo = Int32.Parse(txtContactNo.Text);
            User user = dbUtils.checkIfRegistered(resNo);
            if (user != null) {
                txtBName.Text = user.Name;
            }
        }
        */



        private void txtContactNo_TextChanged_1(object sender, EventArgs e)
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

        private void btnCondemnBook_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Pick the book to be condemned above");
            }

                else if (dbUtils.checkIfCondemned(id))
                {
                    MessageBox.Show("Sorry! Book is CONDEMNED already.", "Error");
                    return;
                }
                else
                {
                    int id = Int32.Parse(txtID.Text);
                    dbUtils.condemnBook(id);
                    MessageBox.Show("Book Condemned, sorry to hear that.");
                    refreshTable();
                }
            }
        

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                //this.borrowed_BookTableAdapter.Fill(this.generalDataSet1.Borrowed_Book);
                refreshTable();
            }
            if (cmbChoice.SelectedItem == null)
            {

                MessageBox.Show("Please pick search filter");
                cmbChoice.Focus();
                return;
            }

            //String id = txtBookID.Text;
            String title = txtTitle.Text;
            String text = txtSearch.Text;
            String choice = cmbChoice.SelectedItem.ToString();
            dbUtils.searchBook(text, choice, dgvBooks);

        }

        private void cmbChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
          /*  saveFileDialog1.InitialDirectory = "C:";
            saveFileDialog1.Title = "Save as Excel File";
            saveFileDialog1.FileName = "Borrowed Books (Date[MM/DD/YYY],Time[00:00])";
            saveFileDialog1.Filter = "Excel Files(2013)|.xlsx|Excel Files(2010)|.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {

                Microsoft.Office.Interop.Excel._Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ExcelApp.Application.Workbooks.Add(Type.Missing);

                //Changing the properties of the Workbook
                ExcelApp.Columns.ColumnWidth = 15;

                //Storing header part in excel
                for (int i = 1; i < dgvBooks.Columns.Count + 1; i++)
                {
                    ExcelApp.Cells[1, i] = dgvBooks.Columns[i - 1].HeaderText;
                }
                //Storing each row and columns value to excel sheet
                for (int i = 0; i < dgvBooks.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvBooks.Columns.Count; j++)
                    {
                        ExcelApp.Cells[i + 2, j + 1] = dgvBooks.Rows[i].Cells[j].Value.ToString();
                    }
                }
                ExcelApp.ActiveWorkbook.SaveCopyAs(saveFileDialog1.FileName.ToString());
                ExcelApp.ActiveWorkbook.Saved = true;
            }*/
        }


        


    }
}
