using DatabasesConnection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerManager
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            System.Console.WriteLine("Window is loaded");
            DBConn connection = new DBConn();

            String configDBSource = ConfigurationManager.AppSettings["dbSource"];

            SQLiteConnection dbh = connection.getDBConn(configDBSource);

            dbh.Open();
            String sql = "select * from customers";
            SQLiteCommand command = new SQLiteCommand(sql, dbh);

            var dataAdapter = new SQLiteDataAdapter(sql, "Data Source=" + configDBSource + ";version=3;");
            var commandBuilder = new SQLiteCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds, "customers");
            //dataGridView1.ReadOnly = true;
            //dataGridView1.DataSource = ds.Tables[0];
            loadListView(ds);


            dbh.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void loadListView( DataSet ds )
        {
            DataTable dTable = ds.Tables["customers"];

            listViewReady.Items.Clear();

            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                DataRow drow = dTable.Rows[i];

                if(drow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lwi = new ListViewItem(drow["id"].ToString());
                    lwi.SubItems.Add (drow["first_name"].ToString());
                    lwi.SubItems.Add(drow["last_name"].ToString());

                    listViewReady.Items.Add(lwi);
                }
            }

        }

        private void formToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {

        }

        private void btnChooseColor_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = false;

            colorDialog1.ShowHelp = true;

            // Sets initial color
            colorDialog1.Color = lblBackgroundColor.ForeColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
                lblBackgroundColor.ForeColor = colorDialog1.Color;

        }


    }
}
