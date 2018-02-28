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
        public const string REMOVED                 = "0";
        public const string STANDBY                 = "1";
        public const string IN_PROGRESS             = "2";
        public const string READY                   = "3";
        public const string COMPLETED               = "4";

        // thresholds record count
        public const string WARNING                 = "5";
        public const string CRITICAL                = "10";

        // controls
        public const string ENABLED                 = "1";
        public const string DISABLED                = "0";

        // Tab pages
        public const int TAB_QUEUES                 = 0;
        public const int TAB_INFORMATION            = 1;
        public const int TAB_DATABASE               = 2;
        public const int TAB_CONFIGURATION          = 3;

    }
}
