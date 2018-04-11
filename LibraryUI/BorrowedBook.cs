using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryUI
{
    class BorrowedBook
    {
        private int id, contactNo;
        private String title, bName,dateBorrowed, dueDate;

        public BorrowedBook(int id, String title, String bName, int contactNo, String dateBorrowed, String dueDate)
        {
            this.id = id;
            this.contactNo = contactNo;
            this.title = title;
            this.bName = bName;
            this.dateBorrowed = dateBorrowed;
            this.dueDate = dueDate;
        }

        public string BName
        {
            get
            {
                return bName;
            }

            set
            {
                bName = value;
            }
        }

        public int ContactNo
        {
            get
            {
                return contactNo;
            }

            set
            {
                contactNo = value;
            }
        }

        public string DateBorrowed
        {
            get
            {
                return dateBorrowed;
            }

            set
            {
                dateBorrowed = value;
            }
        }

        public string DueDate
        {
            get
            {
                return dueDate;
            }

            set
            {
                dueDate = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }
    }
}
