using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasesConnection
{
    public static class Constants
    {
        // the different queues
        public const string REMOVED = "0";
        public const string STANDBY = "1";
        public const string IN_PROGRESS = "2";
        public const string READY = "3";
        public const string COMPLETED = "4";

    }
}
