using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasesConnection
{
    class Services
    {
        // AKA transaction or orders object

        /*
         * Description:
         * This class will be the main object of the system.
         * 
         * Manages the billing information
         * Manages the service/order description
         * Contains a pointer to the customer object.
         * Amounts are store in cents eg. 1.00 is 100
         * 
         * Helper methods:
         *  UpdateRecord
         *  InsertRecord
         *  selectRecord
         *  DeleteRecord
         *  ValidateRecord
         *  setId  aka service/order number
         *  getId
         *  setDescription
         *  getDescription
         *  setWorkPerformed
         *  getWorkPerformed
         *  setBrandModel
         *  getBrandModel
         *  setAccess
         *  getAccess
         *  setCondition
         *  getCondition
         *  setParts1
         *  getParts1
         *  setParts2
         *  getParts2
         *  setParts3
         *  getParts3
         *  setPartsCost1
         *  getPartsCost1
         *  setPartsCost2
         *  getPartsCost2
         *  setPartsCost3
         *  getPartsCost3
         *  setLaborcost
         *  getLaborCost
         *  setTaxRate
         *  getTaxRate
         *  setAmountPaid
         *  getAmountPaid
         *  setSubtotal
         *  getSubtotal
         *  setTotal
         *  getTotal
         *  setDateCreated
         *  getDateCreated
         *  setDateETAofComp
         *  getDateETAofComp
         *  setDateofComp
         *  getDateofComp
         *  setDataOfDelivered
         *  getDateofDelivered
         *  setStatus
         *  getStatus
         *  setCustomerId
         *  getCustomerId
         * */

        private int id;
        private String description;
        private String workPerformed;
        private String brandModel;
        private String access;
        private String condition;
        private String part1;
        private String part2;
        private String part3;
        private int partCost1;
        private int partCost2;
        private int partCost3;
        private int laborCost;
        private int paidAmt;
        private double taxRate;
        private int subTotalAmt;
        private int totalAmt;
        private String dateCreated;
        private String dateETAOfComp;
        private String dateOfComp;
        private String dateOfDelivered;
        private String status;
        private int customerId;

        //here we define all the input boxes related to services table
        private string[] names = new string[]
        {
            "dtETAComp",
            "dtDateComp",
            "dtDelivered",
            "txtBrandModel",
            "cBPowerCord",
            "txtOtherAccess",
            "txtDescription",
            "txtWorkPerformed",
            "txtPart1",
            "txtPart2",
            "txtPart3",
            "txtPartCost1",
            "txtPartCost2",
            "txtPartCost3",
            "txtLabor",
            "txtSubTotal",
            "txtTax",
            "txtTotal",
        };

        /**************************
        * 
        ***************************/
        public void UpdateRecord(SQLiteConnection dbh)
        {


        }

        /**************************
        * 
        ***************************/
        public void UpdateStatus(SQLiteConnection dbh, string status, string id)
        {

            Hashtable dbElements = new Hashtable();
            dbElements.Add("status", status);
            dbElements.Add("id", id);

            System.Console.WriteLine("Service Id: " + id);

            SQLiteCommand command = new
                SQLiteCommand("UPDATE service SET status = @Status WHERE id = @Id", dbh);

            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@Id", id);
            bool result = false;

            if (command.ExecuteNonQuery() > 0) result = true;

            //string sql = "UPDATE services SET status = '" + status + "' WHERE id = '" + id + "'";
        }
        /**************************
        * 
        ***************************/
        public void InsertRecord(SQLiteConnection dbh )
        {
            DatabaseManager new_db = new DatabaseManager();

            Hashtable dbElements = new Hashtable();

            //TODO: do validation before insert

            //dbElements.Add("id", generate_id);
            dbElements.Add("customerId", CustomerId );
            dbElements.Add("description", Description);
            dbElements.Add("brand_model", BrandModel);
            dbElements.Add("condition", Condition);
            dbElements.Add("access", Access);
            dbElements.Add("date_in", DateCreated);
            dbElements.Add("date_eta_comp", DateETAOfComp);
            dbElements.Add("date_comp", DateOfComp);
            dbElements.Add("date_delivered", DateOfDelivered);
            dbElements.Add("status", Status);
            dbElements.Add("work_performed", WorkPerformed);
            dbElements.Add("part1", Part1);
            dbElements.Add("part1Cost", PartCost1);
            dbElements.Add("part2", Part2);
            dbElements.Add("part2Cost", PartCost2);
            dbElements.Add("part3", Part3);
            dbElements.Add("part3Cost", PartCost3);
            dbElements.Add("laborCost", LaborCost);
            dbElements.Add("taxRate", TaxRate);
            //dbElements.Add("amountPaid", PaidAmt);
            dbElements.Add("subTotalAmt", SubTotalAmt);
            dbElements.Add("totalAmt", TotalAmt);

            String tblName = "service";
            
            // Get next id
            Id = new_db.getId(tblName, dbh);
            dbElements.Add("id", Id);

            System.Console.WriteLine("Service Id: " + Id);

            new_db.doSQL(dbh, dbElements, tblName); 
        }
        /**************************
        * 
        ***************************/
        public void SelectRecord(SQLiteConnection dbh)
        {

            String sql = "select * from service";
            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
                Console.WriteLine("id: " + reader["id"] + "\tdescription: " + reader["description"] + "\tdate_in: " + reader["date_in"]);
        }
        /**************************
        * 
        ***************************/
        public void DeleteRecord(SQLiteConnection dbh)
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
        public int Id
        {
            get{return id;}
            set{id = value;}
        }
        /**************************
        * 
        ***************************/
        public String Description
        {
            get{return description;}
            set{description = value;}
        }
        /**************************
        * 
        ***************************/
        public String WorkPerformed
        {
            get{return workPerformed;}
            set{workPerformed = value;}
        }
        /**************************
        * 
        ***************************/
        public String BrandModel
        {
            get{return brandModel;}
            set{brandModel = value;}
        }
        /**************************
        * 
        ***************************/
        public String Access
        {
            get{return access;}
            set{ access = value;}
        }
        /**************************
        * 
        ***************************/
        public String Condition
        {
            get{return condition;}
            set{condition = value;}
        }
        /**************************
        * 
        ***************************/
        public String Part1
        {
            get{return part1;}
            set{part1 = value;}
        }
        /**************************
        * 
        ***************************/
        public String Part2
        {
            get{return part2;}
            set{part2 = value;}
        }
        /**************************
        * 
        ***************************/
        public String Part3
        {
            get{return part3;}
            set{part3 = value;}
        }
        /**************************
        * 
        ***************************/
        public int PartCost1
        {
            get{return partCost1;}
            set{partCost1 = value;}
        }
        /**************************
        * 
        ***************************/
        public int PartCost2
        {
            get{return partCost2;}
            set{partCost2 = value;}
        }
        /**************************
        * 
        ***************************/
        public int PartCost3
        {
            get{return partCost3;}
            set{partCost3 = value;}
        }
        /**************************
        * 
        ***************************/
        public int LaborCost
        {
            get { return laborCost; }
            set { laborCost = value; }
        }
        /**************************
        * 
        ***************************/
        public int PaidAmt
        {
            get{return paidAmt;}
            set{paidAmt = value;}
        }
        /**************************
        * 
        ***************************/
        public double TaxRate
        {
            get{return taxRate;}
            set{taxRate = value;}
        }
        /**************************
        * 
        ***************************/
        public int SubTotalAmt
        {
            get{return subTotalAmt;}
            set{subTotalAmt = value;}
        }
        /**************************
        * 
        ***************************/
        public int TotalAmt
        {
            get{return totalAmt;}
            set{totalAmt = value;}
        }
        /**************************
        * 
        ***************************/
        public string DateCreated
        {
            get{return dateCreated;}
            set{dateCreated = value;}
        }
        /**************************
        * 
        ***************************/
        public string DateETAOfComp
        {
            get{return dateETAOfComp;}
            set{dateETAOfComp = value;}
        }
        /**************************
        * 
        ***************************/
        public string DateOfComp
        {
            get{return dateOfComp;}
            set{dateOfComp = value;}
        }
        /*************************
        * 
        ***************************/
        public string DateOfDelivered
        {
            get { return dateOfDelivered; }
            set { dateOfDelivered = value; }
        }
        /*************************
        * 
        ***************************/
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        /*************************
        * 
        ***************************/
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public string[] getNames()
        {
            return names;
        }
    }
}
