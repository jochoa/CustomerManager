using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasesConnection
{

    class Mail
    {
        private String smtp;
        private String port;
        private String email;
        private String password;

        public Mail()
        {
            System.Console.WriteLine("<<<<<<<<<<<<<<<<<<<<< Email class >>>>>>>>>>>>>>>>>>>>");
        }

        public bool createConnection(string smtp, string port, string email, string password )
        {
            if(smtp == "" 
                || port == ""
                || email == ""
                || password == "")
            {
                return false;
            }


            return true;
        
        }

    }
}
