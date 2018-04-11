using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace LibraryUI
{
    class DatabaseUtilities
    {
        
        public static SqlConnection getConnection()
        {
            String connectionString = "Data Source=ANDRE;Initial Catalog=general;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }

        SqlConnection conn = null;
        public void addBook(Book book)
        {
            conn = getConnection();
            String insertStmt = "INSERT INTO Book VALUES(" + book.BookID + ",'" + book.Title
                                + "','" + book.Author + "','" + book.Genre + "','" + book.Status + "')";
            SqlCommand command = new SqlCommand(insertStmt,conn);

            conn.Open();
            command.ExecuteNonQuery();
      
        }

        public void addUser(User user)
        {
            conn = getConnection();
            String insertStmt = "INSERT INTO Register VALUES(" + user.ContactNo + ",'" + user.Name + "')";
            SqlCommand command = new SqlCommand(insertStmt, conn);

            conn.Open();
            command.ExecuteNonQuery();

        }


        public void addBorrowedBook(BorrowedBook book)
        {
            conn = getConnection();
            String insertStmt = "INSERT INTO Borrowed_Book VALUES(" + book.Id + ",'" + book.Title
                                + "','" + book.BName + "'," + book.ContactNo + ",'" + book.DateBorrowed + "','"+ book.DueDate + "')";
            String updateStmt = "UPDATE Book SET status='On Loan' WHERE bookID=" + book.Id;
            SqlCommand command = new SqlCommand(insertStmt, conn);
            SqlCommand command2 = new SqlCommand(updateStmt, conn);


            conn.Open();
            command.ExecuteNonQuery();
            command2.ExecuteNonQuery();
        }

        //new
        public void addCondemnedBook(BorrowedBook book)
        {
            conn = getConnection();
            String insertStmt = "INSERT INTO Borrowed_Book VALUES(" + book.Id + ",'" + book.Title
                                + "','" + book.BName + "'," + book.ContactNo + ")";
            String updateStmt = "UPDATE Book SET status='Condemned' WHERE bookID=" + book.Id;
            SqlCommand command = new SqlCommand(insertStmt, conn);
            SqlCommand command2 = new SqlCommand(updateStmt, conn);


            conn.Open();
            command.ExecuteNonQuery();
            command2.ExecuteNonQuery();
        }
     

        public User checkIfRegistered(int resNo)
        {
            
            conn = getConnection();
            SqlCommand com = new SqlCommand("Select * from Register where contactNo =" + resNo, conn);
            
            try
            {
                conn.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    User user = new User();
                    user.ContactNo = reader.GetInt32(0);
                    user.Name = reader.GetString(1);
                    return user;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                conn.Close();
            }
            

           
            return null;
        }

        public void updateBook(Book book)
        {
            conn = getConnection();
            String updateStmt = "UPDATE Book SET title='" + book.Title + "', author='" + book.Author
                                + "', genre='" + book.Genre + "' WHERE bookID=" + book.BookID;
            SqlCommand command = new SqlCommand(updateStmt,conn);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        //new
        public void condemnBook(int id)
        {
            conn = getConnection();
            //String updateStmt = "UPDATE Book SET title='" + book.Title + "', author='" + book.Author
            //                   + "', genre='" + book.Genre + "', status='" + book.Status
              //                 + "' WHERE bookID=" + book.BookID;
            String updateStmt = "UPDATE Book SET status='Condemned' WHERE bookID=" + id;
            SqlCommand command = new SqlCommand(updateStmt, conn);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public void returnBook(int id)
        {
            conn = getConnection();
            String updateStmt = "UPDATE Book SET status='Available' WHERE bookID=" + id;
            String deleteStmt = "DELETE FROM Borrowed_Book WHERE bookID=" + id;
            SqlCommand command = new SqlCommand(updateStmt, conn);
            SqlCommand command2 = new SqlCommand(deleteStmt, conn);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                command2.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public void deleteBorrowedBook(int id)
        {
            conn = getConnection();
            
            String deleteStmt = "DELETE FROM Borrowed_Book WHERE bookID=" + id;
           
            SqlCommand command2 = new SqlCommand(deleteStmt, conn);

            try
            {
                conn.Open();
                
                command2.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public void searchBook(String text, String choice, DataGridView dgv)
        {

            conn = getConnection();
            String searchStmt = "SELECT * FROM Book WHERE " + choice + " LIKE '%" + text + "%'";
            SqlCommand command = new SqlCommand(searchStmt, conn);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgv.AutoGenerateColumns = true;
                    dgv.DataSource = dt;
                    dgv.Refresh();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }

        }

        public void searchBorrowedBook(String text, String choice, DataGridView dgv)
        {
            
            conn = getConnection();
            String searchStmt = "SELECT * FROM Borrowed_Book WHERE " + choice + " LIKE '%" + text + "%'";
            SqlCommand command = new SqlCommand(searchStmt, conn);
            
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgv.AutoGenerateColumns = true;
                    dgv.DataSource = dt;
                    dgv.Refresh();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }


        }

        public bool checkIfAvailable(int id)
        {
            conn = getConnection();

            String select = "SELECT * FROM Book WHERE status='On Loan' AND bookID=" + id;
            SqlCommand command = new SqlCommand(select, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }

                /*
                if (affectedRows == -1)
                {
                    
                    return true;
                }
                */
            }

            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

            public bool checkIfCondemned(int id)
        {
            conn = getConnection();

            String select = "SELECT * FROM Book WHERE status='Condemned' AND bookID=" + id;
            SqlCommand command = new SqlCommand(select, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }

                /*
                if (affectedRows == -1)
                {
                    
                    return true;
                }
                */
            }

            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return false;

        }

        public void deleteBook(int bookID)
        {
            conn = getConnection();
            String deleteStmt = "DELETE FROM Book WHERE bookID=" + bookID;
            SqlCommand command = new SqlCommand(deleteStmt,conn);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

    }




}
