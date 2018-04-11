using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryUI
{
    class Book
    {
        private int bookID;
        private String author, genre, status, title;

        public int BookID
        {
            get
            {
                return bookID;
            }

            set
            {
                bookID = value;
            }
        }

        public string Author
        {
            get
            {
                return author;
            }

            set
            {
                author = value;
            }
        }

        public string Genre
        {
            get
            {
                return genre;
            }

            set
            {
                genre = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
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

        public Book(int bookID, String title, String author, String genre, String status)
        {
            this.bookID = bookID;
            this.title = title;
            this.author = author;
            this.genre = genre;
            this.status = status;
        }
    }
}
