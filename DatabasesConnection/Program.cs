using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            System.Console.WriteLine("Starting program <<<<<<<<<<<<<<<<<<<<<<<<<<");
            // The task: Create a customer manager program with queues etc similar to lantec
            /*
             1. Connect to any database
             2. Create db basic schema
             3. Create connector
            */
            int test = 12;
            Application.Run(new MainWindow());
        }
    }
}
