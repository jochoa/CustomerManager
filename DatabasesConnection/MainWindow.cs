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
        private SQLiteConnection dbh;
        DataSet ds1;
        WebBrowser documentToPrint;
        //private Constants programConst = new Constants();


        public MainWindow()
        {
            InitializeComponent();

            //tabControl1.SelectedIndexChanged += new EventHandler();

            System.Console.WriteLine("Window is loaded");

            System.Console.WriteLine(Constants.READY );

            DBConn connection = new DBConn();

            String configDBSource = ConfigurationManager.AppSettings["dbSource"];

            dbh = connection.getDBConn(configDBSource);

            dbh.Open();

            loadQueues();

            InitializeTextElements();

            //error provider control: http://www.c-sharpcorner.com/article/using-error-provider-control-in-windows-forms-and-C-Sharp/
            //set some mask text boxes
            txtMaskedLabor.Mask = "00000.00";
            //https://stackoverflow.com/questions/9684221/tabpage-click-events

            //TODO: set thresholds. Graph view of each the queues. Green means queue is normal , yellow close to reach peak level, red something is wrong
            //https://stackoverflow.com/questions/33525881/draw-a-graph-in-windows-forms-application-from-a-datatable-in-a-data-access-clas
            //dbh.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void loadQueues()
        {

            listViewStandby.Clear();
            listViewProgress.Clear();
            listViewReady.Clear();

            for (int i = 1; i <= 3; i++)
            {

                String sql = "SELECT * FROM service S inner join customers C ON C.id = S.customerId WHERE status = '" + i + "'";

                var dataAdapter = new SQLiteDataAdapter(sql, dbh);
                var commandBuilder = new SQLiteCommandBuilder(dataAdapter);
                var ds = new DataSet();

                dataAdapter.Fill(ds, "service");

                if (i == Convert.ToInt16(Constants.STANDBY))
                {
                    loadListView(ds, listViewStandby);
                }
                else if (i == Convert.ToInt16(Constants.IN_PROGRESS))
                {
                    loadListView(ds, listViewProgress);
                }
                else if (i == Convert.ToInt16(Constants.READY))
                {
                    loadListView(ds, listViewReady);

                }

            }
        }
        private void loadDatabase()
        {

            lvDatabase.Clear();

            //TODO> optimize load, maybe display only last 100 records. Using temp table could be a good idea
            //https://books.google.de/books?id=4LF-FYc8JJcC&pg=PA189&lpg=PA189&dq=windows+form+load+listview+with+database+in+pages&source=bl&ots=VrmSNL3qEm&sig=K_C7s7KcGBRUPVn-OyiTPgN_xZM&hl=en&sa=X&ved=0ahUKEwjszZLniJPZAhWD-6QKHZ22BaIQ6AEISTAF#v=onepage&q=windows%20form%20load%20listview%20with%20database%20in%20pages&f=false
            String sql = "SELECT * FROM service S inner join customers C ON C.id = S.customerId";

            var dataAdapter = new SQLiteDataAdapter(sql, dbh);
            var commandBuilder = new SQLiteCommandBuilder(dataAdapter);
            ds1 = new DataSet();

            dataAdapter.Fill(ds1, "service");

            loadListView(ds1, lvDatabase);

        }


        private void loadListView( DataSet ds, ListView myListView )
        {
            DataTable dTable = ds.Tables["service"];

            myListView.Items.Clear();

            myListView.Columns.Add("Service Id", 70, HorizontalAlignment.Left);
            myListView.Columns.Add("First Name", 100, HorizontalAlignment.Left);
            myListView.Columns.Add("Last Name", 100, HorizontalAlignment.Left);
            myListView.Columns.Add("Phone number", 100, HorizontalAlignment.Left);
            myListView.Columns.Add("Arrived date", 100, HorizontalAlignment.Left);
            myListView.Columns.Add("Brand/Model", 100, HorizontalAlignment.Left);
            myListView.Columns.Add("Description", 200, HorizontalAlignment.Left);
            
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                DataRow drow = dTable.Rows[i];

                if(drow.RowState != DataRowState.Deleted)
                {
                    System.Console.WriteLine(" =================== ");
                    System.Console.WriteLine(" =================== " + drow["id"].ToString() );
                    System.Console.WriteLine(" =================== " + drow["description"].ToString() );
                    ListViewItem lwi = new ListViewItem(drow["id"].ToString());
                    lwi.SubItems.Add(drow["first_name"].ToString());
                    lwi.SubItems.Add(drow["last_name"].ToString());
                    lwi.SubItems.Add(drow["telephone"].ToString());
                    lwi.SubItems.Add(drow["date_in"].ToString());
                    lwi.SubItems.Add(drow["brand_model"].ToString());
                    lwi.SubItems.Add(drow["description"].ToString());

                    if (i % 2 == 1)
                    {
                        lwi.BackColor = Color.PowderBlue; 
                    }
                    else
                    {
                        lwi.BackColor = Color.LightCyan;
                    }
                    
                    myListView.Items.Add(lwi);
                }
            } 

        }

        private void InitializeTextElements()
        {
            txtPartCost1.Text = "0.00";
            txtPartCost2.Text = "0.00";
            txtPartCost3.Text = "0.00";
            txtLabor.Text = "0.00";
            txtSubTotal.Text = "0.00";
            txtTotal.Text = "0.00";

            //todo: to be fetch from configuration
            txtTax.Text = "0";

            // Disable some fields
            dtDateComp.Enabled = false;
            dtDelivered.Enabled = false;
        }

        private void formToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            Services services = new Services();
            String logFix = "[Save button]: ";
            System.Console.WriteLine(logFix + " Button Save clicked ");

            // https://docs.microsoft.com/en-us/dotnet/framework/winforms/user-input-validation-in-windows-forms
            String first_name = txtFirstName.Text;
            String last_name = txtLastName.Text;
            String address = txtAddress.Text;
            String phone = txtPhone.Text;
            String location = txtLacation.Text;
            String email = txtEmail.Text;

            //TODO: remove this date and just take the current date from system
            String dateIn = dtDate.Text;

            String dateETA = dtETAComp.Text;
            //String dateComp = dtDateComp.Text;
            //String dateDelivered = dtDelivered.Text;

            String brandModel = txtBrandModel.Text;
            String accessories = "";

            accessories = cBPowerCord.Text;
            accessories += " " + txtOtherAccess.Text;

            String condition = "";

            if( rBNew.Checked)
            {
                condition = "1"; //replace for constant
            }
            else if( rBGood.Checked )
            {
                condition = "2";
            }
            else if( rBOther.Checked )
            {
                condition = txtOtherCondition.Text;
            }

            String description = txtDescription.Text;
            String workPerformed = txtWorkPerformed.Text;

            String part1 = txtPart1.Text;
            String part2 = txtPart2.Text;
            String part3 = txtPart3.Text;

            String partCost1 = txtPartCost1.Text;
            String partCost2 = txtPartCost2.Text;
            String partCost3 = txtPartCost3.Text;

            String labor = txtLabor.Text;
            String subTotal = txtSubTotal.Text;
            String taxRate = txtTax.Text;
            String total = txtTotal.Text;

            // After pre checks ...

            // Create customer object
            Customer newCustomer = new Customer();

            newCustomer.First_Name = first_name;
            newCustomer.Last_Name = last_name;
            newCustomer.Address_1 = address;
            newCustomer.Telephone = phone;
            newCustomer.Location = location;
            newCustomer.Email = email;

            newCustomer.InsertRecord(dbh);
            int customerId = newCustomer.Id;


            // TODO: Create service object after pre check that customer was inserted correctly
            Services newService = new Services();

            newService.CustomerId = customerId;
            newService.Description = description;
            newService.WorkPerformed = workPerformed;
            newService.BrandModel = brandModel;
            newService.Access = accessories;
            newService.Condition = condition;
            newService.Part1 = part1;
            newService.Part2 = part2;
            newService.Part3 = part3;
            newService.PartCost1 = convertToCents(partCost1);
            newService.PartCost2 = convertToCents(partCost2);
            newService.PartCost3 = convertToCents(partCost3);
            newService.LaborCost = convertToCents(labor);
            newService.SubTotalAmt = convertToCents(subTotal);
            newService.TaxRate = Convert.ToDouble(taxRate);
            newService.TotalAmt = convertToCents(total);
            newService.Status = Constants.STANDBY;
            newService.DateCreated = dateIn;
            newService.DateETAOfComp = dateETA;

            newService.InsertRecord(dbh);


        }

        // ******************************************************************
        // Button's events
        // ******************************************************************
        private void txtPartCost1_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtPartCost1_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtPartCost1);
        }
        private void txtPartCost2_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtPartCost2_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtPartCost2);
        }
        private void txtPartCost3_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtPartCost3_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtPartCost3);
        }
        private void txtLabor_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtLabor);
        }
        private void txtLabor_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtSubTotal_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtSubTotal);
        }
        private void txtSubTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtTotal_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtTotal);
        }
        private void txtTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            String msg = "Please enter your first name";
            CheckEmptyInput(txtFirstName, msg);

        }

        // ******************************************************************
        // Tools
        // ******************************************************************
        //todo: move to tools
        private void OnlyDigitsAndSinglePoint(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        //todo: move to tools
        private void ResetAmount(object sender, EventArgs e, TextBox txtBox )
        {
            if (string.IsNullOrEmpty(txtBox.Text))
            {
                //part1Cost = 0.0;
                txtBox.Text = "0.00";
            }
            else
            {
                txtBox.Text = FormatToDecimals(Convert.ToDouble(txtBox.Text));
            }
        }

        //todo: move to tools
        private string FormatToDecimals(double number)
        {
            var s = string.Format("{0:0.00}", number);
            return s;
        }

        private int convertToCents(String amt)
        {
            int cents = (int)(Convert.ToDouble(amt) * 100);
            return cents;
        }

        private void reverseCents(int cents)
        {

        }

        private bool CheckEmptyInput(TextBox txtBox, String msg)
        {
            bool status = true;

            if (txtFirstName.Text == "")
            {
                errorProvider1.SetError(txtFirstName, msg);
                status = false;
            }
            else
            {
                errorProvider1.SetError(txtFirstName, "");
            }
            return status;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Console.WriteLine("Exiting ====================================");
            dbh.Close();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //System.Console.WriteLine("clicked ====================================");
            //System.Console.WriteLine(e.TabPageIndex);

            int TAB_QUEUES = 0;
            int TAB_INFORMATION = 1;
            int TAB_DATABASE = 2;
            int TAB_CONFIGURATION = 3; 

            if (e.TabPageIndex == TAB_QUEUES)
            {
                System.Console.WriteLine("Reload the queues ====================================");
                loadQueues();
                
            }

            if (e.TabPageIndex == TAB_DATABASE)
            {
                System.Console.WriteLine("Reload the database  ====================================");
                loadDatabase();

            }

        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            updateService(sender, e, Constants.REMOVED, listViewReady);
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            updateService(sender, e, Constants.REMOVED, listViewProgress);
        }

        private void btnCancel3_Click(object sender, EventArgs e)
        {
            updateService(sender, e, Constants.REMOVED, listViewStandby);
            
        }

        private void btnUpToInProgress_Click(object sender, EventArgs e)
        {

            updateService(sender, e, Constants.IN_PROGRESS, listViewStandby);
        }

        private void updateService(object sender, EventArgs e, string action, ListView workingListView)
        {
            System.Console.WriteLine("Clicked cancel ====================================");

            // https://stackoverflow.com/questions/10797774/messagebox-with-input-field
            // TODO: Display a warning before removing a record
            //TODO: check if a row has been selected. Alternatively, I can enable the button if a row is selected with an event place in the list view


            try
            {
                string idVal = workingListView.SelectedItems[0].SubItems[0].Text;
                System.Console.WriteLine("Clicked cancel ====================================");
                System.Console.WriteLine(idVal);
                Services serviveObj = new Services();

                serviveObj.UpdateStatus(dbh, action, idVal);

                loadQueues();
            }
            catch (Exception ex)
            {
                MessageBox.Show("You need to select an item. Or error ocurred. " + ex.ToString());
            }
            finally { }
        }

        private void btnDownToStandingBy_Click(object sender, EventArgs e)
        {
            updateService(sender, e, Constants.STANDBY, listViewProgress);
        }

        private void btnUpToReady_Click(object sender, EventArgs e)
        {
            updateService(sender, e, Constants.READY, listViewProgress);
        }

        private void btnDownToInProcess_Click(object sender, EventArgs e)
        {
            updateService(sender, e, Constants.IN_PROGRESS, listViewReady);
        }

        private void btnCompleted_Click(object sender, EventArgs e)
        {
            updateService(sender, e, Constants.COMPLETED, listViewReady);
        }

        private void lvDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selecteIndex = lvDatabase.FocusedItem.SubItems[0].Text;
            System.Console.WriteLine("Selected index ====================================" + selecteIndex);
            
            try
            {
                int tmp = Convert.ToInt16(selecteIndex);
                //tmp++;
                DataRow[] returnedRow;
                returnedRow = ds1.Tables["service"].Select("id='" + tmp + "'");

                DataRow dr1;
                            
                //wBDisplay.DocumentText = "<b>Work Order</>";
                dr1 = returnedRow[0];
                string title = "Work Order";
                WBDocument document = new WBDocument( wBDisplay, title );
                document.setDataRow(dr1);  // data from database
                
                documentToPrint = document.create();

                //txtBigView.Text = dr1["name"].ToString();
                /*
                txtBigView.Clear();
                txtBigView.AppendText("\t\t\t\t               Work Order             \n");
                txtBigView.AppendText("==================================================================================");
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Customer:  {0}", dr1["first_name"].ToString() + " " + dr1["last_name"].ToString()));
                txtBigView.AppendText(string.Format("       Date:  {0}", dr1["date_in"].ToString()));
                txtBigView.AppendText(string.Format("       Customer ID:  {0}", dr1["id"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Address:  {0}", dr1["address_1"].ToString()));
                txtBigView.AppendText(string.Format("       City/St./Zipcode:  {0}", dr1["location"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Primary phone:  {0:(###)###-####}", dr1["telephone"].ToString()));
                txtBigView.AppendText(string.Format("        Secondary phone:  {0:(###)###-####}", dr1["telephone"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText("==================================================================================");
                txtBigView.AppendText("\n");
                string tmpStatus = "";
                if (dr1["status"].ToString() == Constants.STANDBY)
                {
                    tmpStatus = "Waiting";
                }
                else if (dr1["status"].ToString() == Constants.IN_PROGRESS)
                {
                    tmpStatus = "Completed";
                }
                else if (dr1["status"].ToString() == Constants.READY)
                {
                    tmpStatus = "Fixing";
                }
                else if (dr1["status"].ToString() == Constants.COMPLETED)
                {
                    tmpStatus = "Ready for pick up/deliver";
                }
                txtBigView.AppendText(string.Format("Status:  {0}", tmpStatus));//????
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("ETA of Comp:  {0}", Convert.ToDateTime(dr1["date_eta_comp"]).ToString("MM/dd/yyyy")));
                txtBigView.AppendText("\n");
                if (!string.IsNullOrEmpty(dr1["date_comp"].ToString()))
                {
                    txtBigView.AppendText(string.Format("Date of Comp:  {0}", Convert.ToDateTime(dr1["date_comp"]).ToString("MM/dd/yyyy")));
                    txtBigView.AppendText("\n");
                }
                else
                {
                    txtBigView.AppendText("Date of Comp:\n");
                }
                if (!string.IsNullOrEmpty(dr1["date_delivered"].ToString()))
                {
                    txtBigView.AppendText(string.Format("Date of P/U:  {0}", Convert.ToDateTime(dr1["date_delivered"]).ToString("MM/dd/yyyy")));
                    txtBigView.AppendText("\n");
                }
                else
                {
                    txtBigView.AppendText("Date of P/U:\n");
                }
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Brand/Model:  {0}", dr1["brand_model"].ToString()));
                txtBigView.AppendText(string.Format("        Access:  {0}", dr1["access"].ToString()));
                txtBigView.AppendText(string.Format("        Condition:  {0}", dr1["condition"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Description:  {0}", dr1["description"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Work performed:  {0}", dr1["work_performed"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Part:  {0}", dr1["part1"].ToString()));
                txtBigView.AppendText(string.Format("        Cost:  {0}", dr1["part1Cost"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Part:  {0}", dr1["part2"].ToString()));
                txtBigView.AppendText(string.Format("        Cost:  {0}", dr1["part2Cost"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Part:  {0}", dr1["part3"].ToString()));
                txtBigView.AppendText(string.Format("        Cost:  {0}", dr1["part3Cost"].ToString()));
                txtBigView.AppendText("\n");
                txtBigView.AppendText("\n");
                txtBigView.AppendText(string.Format("Labor:  {0}", dr1["laborCost"].ToString()));
                txtBigView.AppendText(string.Format("        SubTotal:  {0}", dr1["subTotalAmt"].ToString()));
                txtBigView.AppendText(string.Format("        Tax:  {0}", dr1["taxRate"].ToString()));
                txtBigView.AppendText(string.Format("        Total:  {0}", dr1["totalAmt"].ToString()));
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Press ok");
            }
            finally { }
            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //TODO: check if document is defined or selectedIndex from list view is better
            documentToPrint.ShowPrintDialog();

        }

    }
}
