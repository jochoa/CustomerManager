using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasesConnection
{
    class Configurable
    {
        private SQLiteConnection dbh;

        public Dictionary<string, string> settings;

        private string[] names = new string[]
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
            "cBDisclaimerHeaderBold"
        };

        public Configurable(SQLiteConnection dbh)
        {
            this.dbh = dbh;

            loadSystemSettings();
        }

        private void loadSystemSettings()
        {
            //TODO define default values
            DatabaseManager dbManager = new DatabaseManager();
            dbManager.setDbh(this.dbh);

            DataTable data = dbManager.readSettingsTable();
            settings = new Dictionary<string, string>();

            for (int i = 0; i < data.Rows.Count; i++)
            {

                DataRow drow = data.Rows[i];

                if (drow.RowState != DataRowState.Deleted)
                {
                    settings.Add(drow["name"].ToString(), drow["value"].ToString() );
                }
            }

        }

        public void updateSettings( Hashtable updateData )
        {

            //TODO: do prechecks

            string table = "settings";
            string whereClause = "name";
            string colSetName = "value";

            DatabaseManager dbManager = new DatabaseManager();
            dbManager.setDbh(this.dbh);

            dbManager.doUpdateSql(updateData, table, colSetName, whereClause );

        }

        public void reload()
        {
            loadSystemSettings();
        }

        public string[] getNames()
        {
            return names;
        }

    }
}
