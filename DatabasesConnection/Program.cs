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

            DBConn connection = new DBConn();

            connection.getMessage();

            // Read db source from config file
            String configDBSource = ConfigurationManager.AppSettings["dbSource"];

            SQLiteConnection dbh;
            // Check if the database exits
            bool db_exist = connection.databaseExist(configDBSource);

            if( db_exist)
            {
                System.Console.WriteLine(log_prefix + "Loading dbh... " + configDBSource );
                dbh = connection.getDBConn(configDBSource);

            }
            else
            {
                System.Console.WriteLine(log_prefix + "Database file does not exist. Initializing default database");
                
                // call create method from database manager and return the db handler
                DatabaseManager new_db = new DatabaseManager();
                dbh = new_db.createDatabase(configDBSource);

            }

            // Open the main window
            Application.Run(new MainWindow());

          
        }
    }
}
