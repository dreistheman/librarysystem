using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryUI
{
    class User
    {
        private int contactNo;
        private String name;


        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
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

        public User(int contactNo, String name)
        {
            this.contactNo = contactNo;
            this.name = name;
            
        }
        public User() { }
    }
}
