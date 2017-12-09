using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasesConnection
{
    class Customer
    {
        private String first_name;
        private String last_name;
        private String address_1;
        private String address_2;
        private String telephone;
        private String email;

        public Customer()
        {
            System.Console.WriteLine("<<<<<<<<<<<<<<<<<<<<< Customer class >>>>>>>>>>>>>>>>>>>>");
        }


         // GET/SETs  *************************************
        public string First_Name
        {
            get
            {
                return first_name;
            }
            set
            {
                first_name = value;
            }
        }

        public string Last_Name
        {
            get
            {
                return last_name;
            }
            set
            {
                last_name = value;
            }
        }

        public string Address_1
        {
            get
            {
                return address_1;
            }
            set
            {
                address_1 = value;
            }
        }

        public string Address_2
        {
            get
            {
                return address_2;
            }
            set
            {
                address_2 = value;
            }
        }

        public string Telephone
        {
            get
            {
                return telephone;
            }
            set
            {
                telephone = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        // GET/SETs end *************************************
    }
}
