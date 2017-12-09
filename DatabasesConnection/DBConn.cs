using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;
using System.IO;

namespace DatabasesConnection
{
    class DBConn
    {
        String log_prefix = "[DBConn] ";
        private SQLiteConnection dbh;

        public void getMessage()
        {
            
            String configDBSource = ConfigurationManager.AppSettings["dbName"];
            Console.WriteLine(log_prefix + " Loading db: " + configDBSource);
        }

        public DBConn()
        {     
           
        }

        public bool databaseExist(String db_name)
        {
            if (File.Exists(db_name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public SQLiteConnection getDBConn(String db_name)
        {
            
            SQLiteConnection dbh;
            dbh =
                new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
            
            return dbh;
        }


    }
}
