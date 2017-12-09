using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabasesConnection;
using System.Configuration;
using System.IO;

namespace CustomerManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Console.WriteLine("Starting program -------------------------------");

            String log_prefix = "[Main] ";
            // The task: Create a customer manager program with queues etc similar to lantec
            /*
             1. Connect to any database
             2. Create db basic schema
             3. Create connector
            */
          

            //######################

            DBConn connection = new DBConn();

            connection.getMessage();

            String configDBSource = ConfigurationManager.AppSettings["dbSource"];

            SQLiteConnection dbh;

            bool db_exist = connection.databaseExist(configDBSource);

            if( db_exist)
            {
                System.Console.WriteLine(log_prefix + "Loading dbh... " + configDBSource );
                dbh = connection.getDBConn(configDBSource);
            }
            else
            {
                System.Console.WriteLine(log_prefix + "<<<<<<<<<<<<<<<<<<<<<File does not exist. Initializing default database>>>>>>>>>>>>>>>>>>>>");
                // call create method
                DatabaseManager new_db = new DatabaseManager();
                dbh = new_db.createDatabase(configDBSource);

                //dbh = connection.getDBConn(defaultDataBase);
            }


            dbh.Open();
            String sql = "select * from customers";
            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("first_name: " + reader["first_name"] + "\tlast_name: " + reader["last_name"]);

            dbh.Close();

            Customer customer = new Customer();
            customer.First_Name = "Jesus";
            System.Console.WriteLine("<<<<<<<<<<<<<<<<<<<<< first name >>>>>>>>>>>>>>>>>>>>: " + customer.First_Name);
            customer.Email = "jesus.ochoa@gmail.com";
            System.Console.WriteLine("<<<<<<<<<<<<<<<<<<<<< email >>>>>>>>>>>>>>>>>>>>: " + customer.Email);

            // Open the main window
            Application.Run(new MainWindow());

          
        }
    }
}
