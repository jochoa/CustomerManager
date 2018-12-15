using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;

namespace DatabasesConnection
{
    class DatabaseManager
    {
        private SQLiteConnection dbh1;

        public DatabaseManager()
        {
           
        }

        public DatabaseManager(SQLiteConnection dbh)
        {
            setDbh(dbh);
        }

        public SQLiteConnection createDatabase(String database_name)
        {
            try
            {
                SQLiteConnection.CreateFile(database_name);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: {0}", e);
                //throw;
            }
            finally
            {

//TODO: if an error, return to the caller, form should still load
                //System.Environment.Exit(-1);
            }
           

            SQLiteConnection dbh;

            dbh =
            new SQLiteConnection("Data Source=" + database_name + ";Version=3;");
            dbh.Open();

            //create the tables
            createCustomerTable(dbh);
            createCustomerServiceTable(dbh);
            createIdsTable(dbh);

            initializeTableIds(dbh);

            // load test data
            loadCustomerDummyData(dbh);
            loadServiceDummyData(dbh);

            dbh.Close();

            return dbh;
        }

        public void setDbh(SQLiteConnection dbh)
        {
            this.dbh1 = dbh;
        }

        private void initializeTableIds(SQLiteConnection dbh)
        {
            int customerStartId = 1000;
            int serviceStartId = 100;

            Hashtable dbElements = new Hashtable();

            String tblName = "ids";

            // column name, value
            dbElements.Add("table_name", "customers");
            dbElements.Add("counter", customerStartId);
            doSQL(dbh, dbElements, tblName);

            dbElements["table_name"] = "service";
            dbElements["counter"] = serviceStartId;
            doSQL(dbh, dbElements, tblName);
        }

        public int getNextAvailableServiceId( SQLiteConnection dbh )
        {
            //TODO combine with existing code
            int newId = 0;

            String sql = "SELECT counter FROM ids WHERE table_name = 'service' ";
            SQLiteCommand command = new SQLiteCommand(sql, dbh);
            Console.WriteLine("getId +++++++++++++ ");
            Console.WriteLine(sql);

            SQLiteDataReader reader = command.ExecuteReader();

            newId = Convert.ToInt16(reader["counter"]);

            return ++newId;
        }

        public int getId(String tblName, SQLiteConnection dbh)
        {
            int newId = 0;

            String sql = "SELECT counter FROM ids WHERE table_name = '" + tblName + "'";
            SQLiteCommand command = new SQLiteCommand(sql, dbh);
            Console.WriteLine("getId +++++++++++++ ");
            Console.WriteLine(sql);

            SQLiteDataReader reader = command.ExecuteReader();

            newId = Convert.ToInt16(reader["counter"]);

            //int nextId = newId + 1;

            //Update next id
            String updateSql = "UPDATE ids SET counter = counter + 1 WHERE table_name = '" + tblName + "'";

            command = new SQLiteCommand(null, dbh);
            command.CommandText = updateSql;

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: {0}", e);
                return 0;
                //throw;
            }

            return newId;
        }

        public void doSQL(SQLiteConnection dbh, Hashtable dbElements, String tblName)
        {
            String columnString = "";
            String paramString = "";

            int count = dbElements.Count;
            int counter = 1;
            foreach (DictionaryEntry elem in dbElements)
            {
                Console.WriteLine("Key = {0}, Value = {1}", elem.Key, elem.Value);

                if (counter < count)
                {
                    columnString += elem.Key + ", ";
                    paramString += "@" + elem.Key + ", ";
                }
                else
                {
                    columnString += elem.Key;
                    paramString += "@" + elem.Key;
                }


                counter++;
            }
            Console.WriteLine("Count: " + count);
            Console.WriteLine("Columns: " + columnString);
            Console.WriteLine("Params: " + paramString);

            SQLiteCommand command = new SQLiteCommand(null, dbh);

            String commandStr = "INSERT INTO " + tblName + "( " + columnString + ") " +
                "values ( " + paramString + " )";

            Console.WriteLine("Command: " + commandStr);
            command.CommandText = commandStr;

            command.Prepare();

            foreach (DictionaryEntry elem in dbElements)
            {
                command.Parameters.AddWithValue("@" + elem.Key, elem.Value);
            }

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: {0}", e);
                //throw;
            }
        }

        /*
        * 
        * 
        * Update single column using single where clause value
        */
        public void doUpdateSql( Hashtable dbElements, String tblName, String colSetName, string whereClause )
        {
            //UPDATE tableNAME SET value = value WHERE name = name

            string updateQuery = "";
            foreach (DictionaryEntry elem in dbElements)
            {
                Console.WriteLine("Key = {0}, Value = {1}", elem.Key, elem.Value);
                
                updateQuery = "UPDATE " + tblName +
                    " SET " + colSetName + " = '" + elem.Value + "'" +
                    " WHERE " + whereClause + " = '" + elem.Key + "'"; 
                
                /*
                updateQuery = "INSERT INTO " + tblName + "( name, value ) " +
                "values ( '" + elem.Key + "' , '" + elem.Value + "' )"; */

                // DELETE FROM settings WHERE id > 10 
                Console.WriteLine(updateQuery);

                SQLiteCommand command = new SQLiteCommand(updateQuery, this.dbh1);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception occured: {0}", e);
                    //return 0;
                    //throw;
                }
            }
        }

        private void createCustomerTable(SQLiteConnection dbh)
        {

            string sql = "CREATE TABLE customers (" +
                "id INTEGER PRIMARY KEY, " +
                "first_name VARCHAR(20), " +
                "last_name VARCHAR(20), " +
                "address_1 VARCHAR(75), " +
                "location VARCHAR(75), " +
                "telephone VARCHAR(20), " +
                "email VARCHAR(50), " +
                "footprint VARCHAR(50))";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }
        /* This table holds information about specific orders
         *It is one to many from customers table
         */
        private void createCustomerServiceTable(SQLiteConnection dbh)
        {

            string sql = "CREATE TABLE service (" +
                "id INTEGER PRIMARY KEY, " +
                "customerId INTEGER, " +
                "description VARCHAR(200), " +
                "brand_model VARCHAR(75), " +
                "access VARCHAR(75), " +
                "condition VARCHAR(75), " +
                "work_performed  VARCHAR(200), " +
                "part1  VARCHAR(75), " +
                "part1Cost  Integer, " +
                "part2  VARCHAR(75), " +
                "part2Cost  Integer, " +
                "part3  VARCHAR(75), " +
                "part3Cost  Integer, " +
                "laborCost  Integer, " +
                "taxRate  Double, " +
                "amountPaid  Integer, " +
                "subTotalAmt  Integer, " +
                "totalAmt  Integer, " +
                "date_in TEXT, " +
                "date_eta_comp TEXT, " +
                "date_comp TEXT, " +
                "date_delivered TEXT, " +
                "status TEXT )";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }

        private void createServiceLogTable(SQLiteConnection dbh)
        {
            string sql = "CREATE TABLE serviceLog (" +
                "id INTEGER PRIMARY KEY, " +
                "serviceId INTEGER, " +
                "reason_description VARCHAR(200), " +
                "date TEXT, " +
                "type VARCHAR(75), " +
                "counter VARCHAR(75), " +
                "employee TEXT)";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }

        private void createCompanyInfoTable(SQLiteConnection dbh)
        {
            string sql = "CREATE TABLE company_profile (" +
               "id INTEGER PRIMARY KEY, " +
               "name TEXT, " +
               "co_address1 TEXT, " +
               "co_address2 TEXT, " +
               "co_telephone VARCHAR(20), " +
               "co_email VARCHAR(50) )";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }
        private void createImagesTable(SQLiteConnection dbh)
        {

            string sql = "CREATE TABLE images (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "image blob, " +
                "type TEXT ) " ;

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }
        //TODO: test and activate
        private void createSettingsTable(SQLiteConnection dbh)
        {
            string sql = "CREATE TABLE settings (" +
               "id INTEGER PRIMARY KEY, " +
               "name TEXT, " +
               "value TEXT )";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }
        // TODO test and activate, call array names using configurable class 
        public void initializeTableSettings(SQLiteConnection dbh)
        {
            // TODO: use a hashtable to names and values
            // create two arrays of name/values for the initialization of the settigs
            string[] names = new string[]
            {   "txtStandbyWarning",
                "txtStandbyCritical",
                "txtInProgressWarning",
                "txtInProgressCritical",
                "txtReadyWarning",
                "txtReadyCritical",
                "cbReady",
                "cbInProgress",
                "cbStandby",
                "cbDisableAllThresholds",
                "txtInvoiceTitle",
                "rtbCompanyHeader",
                "rTBCompanyAddr1",
                "rTBCompanyAddr2",
                "rTBCompanyPhone",
                "rTBCompanyEmail",
                "rTBCompanyWebsite",
                "cBCompanyHeaderItalic",
                "cBCompanyHeaderBold",
                "rtbDisclaimerTitle",
                "rtbDisclaimerText",
                "cBDisclaimerHeaderItalic",
                "cBDisclaimerHeaderBold",
                "cBCurrency",
                "txtTaxRate"
            };

            string wng = Constants.WARNING;
            string ctl = Constants.CRITICAL;
            string empty = "";
            string uncheck = "false";

            string[] values = new string[]
            {
                wng, ctl, wng, ctl, wng, ctl,
                Constants.ENABLED, Constants.ENABLED, Constants.ENABLED, Constants.DISABLED,
                empty, empty, empty, empty, empty, empty, empty,
                uncheck, uncheck, empty, empty, uncheck, uncheck, empty, empty
            };

            for ( int i=0; i < names.Length; i++ )
            {
                // Create an array of hash tables
                Hashtable dbElements = new Hashtable();

                dbElements.Add("name", names[i] );
                dbElements.Add("value", values[i] );

                String tblName = "settings";

                doSQL(dbh, dbElements, tblName);
            }
        
        }

            private void createIdsTable(SQLiteConnection dbh)
        {
            string sql = "CREATE TABLE ids ( " +
                "table_name TEXT, " +
                "counter INTEGER )";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }

        private void loadServiceDummyData(SQLiteConnection dbh)
        {
            string generate_id = generateID();

            Hashtable dbElements = new Hashtable();

            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("en-US");

            dbElements.Add("customerId", "1");
            dbElements.Add("description", "fix it");
            dbElements.Add("brand_model", "Hp Desktop 12445");
            dbElements.Add("date_in", localDate.ToString(culture));
            dbElements.Add("date_eta_comp", "");
            dbElements.Add("status", "1");

            String tblName = "service";

            doSQL(dbh, dbElements, tblName);

        }

        private void loadCustomerDummyData(SQLiteConnection dbh)
        {
            string generate_id = generateID();

            Hashtable dbElements = new Hashtable();

            dbElements.Add("first_name", "John");
            dbElements.Add("last_name", "Doe");
            dbElements.Add("address_1", "Some  street1");
            dbElements.Add("telephone", "343554543");
            dbElements.Add("email", "dummy_doe@hotmail.com");

            String tblName = "customers";

            doSQL(dbh, dbElements, tblName);

        }

        //TODO: move to tools
        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public DataTable readSettingsTable()
        {

            string columns = "*";
            string from = "settings";

            DataSet data = selectStatement(columns, from, null);

            DataTable dTable = data.Tables["settings"];
            return dTable;


            /*
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict = Enumerable.Range(0, reader.FieldCount)
                .ToDictionary(reader.GetName, reader.GetValue);

            //Console.WriteLine("Exception occured: {0}", Range);
            foreach (KeyValuePair<string, object> item in dict)
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            } */


        }
        //TODO improve with sql injection protection
        public DataSet selectStatement(String columns, String from, String where)
        {
            String sql = "SELECT " + columns + " FROM " + from;

            if( where != null )
            {
                sql += " WHERE " + where;
            }
            

            //SQLiteCommand command = new SQLiteCommand(sql, dbh1);

            var dataAdapter = new SQLiteDataAdapter(sql, dbh1);
            var commandBuilder = new SQLiteCommandBuilder(dataAdapter);
            var ds = new DataSet();

            dataAdapter.Fill(ds, from);

            //SQLiteDataReader reader = command.ExecuteReader();
            //Console.WriteLine("====================", reader["txtStandbyWarning"].ToString());
            return ds;
        }

        public SQLiteDataReader searchForOrder(SQLiteConnection dbh, string valueToSearch )
        {
            try
            {
                string searchFor = valueToSearch;

                String sql = "SELECT * FROM service S inner join customers C ON C.id = S.customerId WHERE ( S.id = " 
                    + valueToSearch + " OR C.telephone =  '" + valueToSearch + "' )";
                Console.WriteLine(sql);
                SQLiteCommand command = new SQLiteCommand(sql, dbh);

                SQLiteDataReader reader = command.ExecuteReader();

                return reader;

            }
            catch ( Exception ex )
            {
                Console.WriteLine("Error in sql");
                Console.WriteLine( ex.ToString() );
                MessageBox.Show("Error occurred.");
                return null;
            }
        }
    }
}
