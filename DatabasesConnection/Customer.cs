using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private String location;
        private String telephone;
        private String email;
        private String footprint;
        private int newId;

        public Customer()
        {
            System.Console.WriteLine("<<<<<<<<<<<<<<<<<<<<< Customer class >>>>>>>>>>>>>>>>>>>>");
        }

        /**************************
        * 
        ***************************/
        public void UpdateRecord(SQLiteConnection dbh)
        {


        }
        public void InsertRecord(SQLiteConnection dbh)
        {
            DatabaseManager new_db = new DatabaseManager();

            Hashtable dbElements = new Hashtable();

            dbElements.Add("first_name", First_Name);
            dbElements.Add("last_name", Last_Name);
            dbElements.Add("address_1", Address_1);
            dbElements.Add("location", Location);
            dbElements.Add("telephone", Telephone);
            dbElements.Add("email", Email);

            String tblName = "customers";

            // Get next id
            Id = new_db.getId(tblName, dbh);
            dbElements.Add("id", Id);

            System.Console.WriteLine("Customer Id: " + Id);
            new_db.doSQL(dbh, dbElements, tblName);

        }
        /**************************
       * 
       ***************************/
        public void SelectRecord(SQLiteConnection dbh)
        {

        }

        /**************************
        * 
        ***************************/
        public void ValidateRecord()
        {


        }
        /**************************
        * 
        ***************************/
        public void DeleteRecord(SQLiteConnection dbh)
        {


        }

        // GET/SETs  *************************************
        public int Id
        {
            get { return newId; }
            set { newId = value; }
        }
        public string Footprint
        {
            get { return footprint; }
            set { footprint = value; }
        }
        public string First_Name
        {
            get{return first_name;}
            set{first_name = value;}
        }
        public string Last_Name
        {
            get{return last_name;}
            set{last_name = value;}
        }
        public string Address_1
        {
            get{return address_1;}
            set{address_1 = value;}
        }
        public string Location
        {
            get{return location;}
            set{location = value;}
        }
        public string Telephone
        {
            get{return telephone;}
            set{telephone = value;}
        }

        public string Email
        {
            get{return email;}
            set{email = value;}
        }
        // GET/SETs end *************************************
    }
}
