using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            createCustomerTable(dbh);
            //createCustomerItemTable(dbh);
            loadCustomerDummyData(dbh);

            dbh.Close();

            return dbh;
        }

        private void doSql(SQLiteConnection dbh)
        {

        }

        private void loadCustomerDummyData(SQLiteConnection dbh)
        {

            String sql = "INSERT INTO customers (first_name, last_name, address_1, address_2, telephone, email ) " +
                "values ('John','Doe', 'Some  street1', 'Some street 2', '343554543', 'dummy_doe@hotmail.com')";
            SQLiteCommand command = new SQLiteCommand(sql, dbh);
            command.ExecuteNonQuery();

        }

        private void createCustomerTable(SQLiteConnection dbh)
        {
            string sql = "CREATE TABLE customers (first_name VARCHAR(20), " +
                "last_name VARCHAR(20), " +
                "address_1 VARCHAR(75), " +
                "address_2 VARCHAR(75), " +
                "telephone VARCHAR(20), " +
                "email VARCHAR(50))";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }

        private void createCustomerItemTable(SQLiteConnection dbh)
        {
            string sql = "CREATE TABLE customer_item (customerId INT, " +
                "description VARCHAR(100), " +
                "brand VARCHAR(75), " +
                "type VARCHAR(75), " +
                "model VARCHAR(75), " +
                "s_number VARCHAR(75), " +
                "repair_description  VARCHAR(75), " +
                "date_in )";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            command.ExecuteNonQuery();
        }
    }
}
