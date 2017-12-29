using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace DatabasesConnection
{
    class DatabaseManager
    {
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

//TODO: f an error, return to the caller, form should still load
                //System.Environment.Exit(-1);
            }
           

            SQLiteConnection dbh;

            dbh =
            new SQLiteConnection("Data Source=" + database_name + ";Version=3;");
            dbh.Open();

            //create the tables
            createCustomerTable(dbh);
            createCustomerServiceTable(dbh);

            // load test data
            loadCustomerDummyData(dbh);
            //loadCustomerServiceDummyData(dbh);

            dbh.Close();

            return dbh;
        }

        private void doSQL(SQLiteConnection dbh, Hashtable dbElements, String tblName)
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

            command.CommandText = "INSERT INTO " + tblName + "( " + columnString + ") " +
                "values ( " + paramString + " )";

            Console.WriteLine("Command: " + "INSERT INTO " + tblName + "( " + columnString + ") " +
                "values ( " + paramString + " )" );

            command.Prepare();

            foreach (DictionaryEntry elem in dbElements)
            {
                command.Parameters.AddWithValue("@" + elem.Key, elem.Value);

                // command.CommandText = "INSERT INTO customers (id, first_name, last_name, address_1, address_2, telephone, email ) " +
                //  "values ( @id, @f_name, @l_name, @addr1, @addr2, @telephone, @email )";
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

        private void loadCustomerDummyData(SQLiteConnection dbh)
        {
            string generate_id = generateID();

            Hashtable dbElements = new Hashtable();

            dbElements.Add("id", generate_id);
            dbElements.Add("first_name", "John");
            dbElements.Add("last_name", "Doe");
            dbElements.Add("address_1", "Some  street1");
            dbElements.Add("address_2", "Some  street2");
            dbElements.Add("telephone", "343554543");
            dbElements.Add("email", "dummy_doe@hotmail.com");

            String tblName = "customers";

            doSQL(dbh, dbElements, tblName);
/*
            SQLiteCommand command = new SQLiteCommand(null, dbh);

            command.CommandText = "INSERT INTO customers (id, first_name, last_name, address_1, address_2, telephone, email ) " +
               "values ( @id, @f_name, @l_name, @addr1, @addr2, @telephone, @email )";


            command.Prepare();

            command.Parameters.AddWithValue("@id", generate_id);
            command.Parameters.AddWithValue("@f_name", "John");
            command.Parameters.AddWithValue("@l_name", "Doe");
            command.Parameters.AddWithValue("@addr1", "Some  street1");
            command.Parameters.AddWithValue("@addr2", "Some  street1");
            command.Parameters.AddWithValue("@telephone", "343554543");
            command.Parameters.AddWithValue("@email", "dummy_doe@hotmail.com");

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: {0}", e);
                //throw;
            }
 */           

        }

        private void createCustomerTable(SQLiteConnection dbh)
        {

            string sql = "CREATE TABLE customers (" +
                "id TEXT, " +
                "first_name VARCHAR(20), " +
                "last_name VARCHAR(20), " +
                "address_1 VARCHAR(75), " +
                "address_2 VARCHAR(75), " +
                "telephone VARCHAR(20), " +
                "email VARCHAR(50))";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }

        private void createCustomerServiceTable(SQLiteConnection dbh)
        {

            string sql = "CREATE TABLE customer_service (" +
                "serviceId INTEGER, " +
                "customerId TEXT, " +
                "description VARCHAR(100), " +
                "brand VARCHAR(75), " +
                "type VARCHAR(75), " +
                "model VARCHAR(75), " +
                "s_number VARCHAR(75), " +
                "service_description  VARCHAR(75), " +
                "date_in TEXT, " +
                "date_due TEXT, " +
                "date_out TEXT, " +
                "status TEXT )";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }

        private void createIdsTable(SQLiteConnection dbh)
        {
            string sql = "CREATE TABLE ids ( name TEXT " +
                "counter INTEGER, )";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }

        private void loadCustomerServiceDummyData(SQLiteConnection dbh)
        {

            String sql = "INSERT INTO customer_service (customerId, service_id, description, brand, type, model, s_number" +
                " service_description, date_in, date_out, status ) " +
                "values ('1','Doe', 'Some  street1', 'Some street 2', '343554543', 'dummy_doe@hotmail.com')";
            SQLiteCommand command = new SQLiteCommand(sql, dbh);
            command.ExecuteNonQuery();

        }


        //TODO: move to tools
        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
