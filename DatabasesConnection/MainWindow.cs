using DatabasesConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        private int numberOfChars = 0;

        double labor = 0.0;
        double subTotal = 0.0;
        double taxAmount = 0.0;
        double total = 0.0;
        double part1Cost = 0.0;
        double part2Cost = 0.0;
        double part3Cost = 0.0;
        double TAX = 0.0825;
        int DEBUG = 1;

        Configurable config;
        //private Constants programConst = new Constants();


        public MainWindow()
        {
            InitializeComponent();

            //tabControl1.SelectedIndexChanged += new EventHandler();

            System.Console.WriteLine("Window is loaded");

            System.Console.WriteLine(Constants.READY);

            DBConn connection = new DBConn();

            String configDBSource = ConfigurationManager.AppSettings["dbSource"];

            dbh = connection.getDBConn(configDBSource);

            dbh.Open();

            // load the system configuration
            config = new Configurable(dbh);

            InitializeFormElements();

            loadQueues();

            //loadImage();

            toggleControlsOnOff(false);

            initializeToolTips();

            //TODO: set thresholds. Graph view of each the queues. Green means queue is normal , yellow close to reach peak level, red something is wrong
            //https://stackoverflow.com/questions/33525881/draw-a-graph-in-windows-forms-application-from-a-datatable-in-a-data-access-clas
            //dbh.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void toggleControlsOnOff(Boolean value)
        {
            gBAdditionalCosts.Enabled = value;
            gBTotalCosts.Enabled = value;
            gBCustomerInfo.Enabled = value;
            gBCondition.Enabled = value;
            gBBrandModel.Enabled = value;
            gBAccessories.Enabled = value;
            txtDescription.Enabled = value;
            txtWorkPerformed.Enabled = value;

            btnSave.Enabled = value;
            btnCancel.Enabled = value;

            if (value == false)
            {
                btnNew.Enabled = true;
            }

        }

        private void clearGroupControls(GroupBox gBox)
        {
            foreach (Control ctr in gBox.Controls)
            {
                // TODO test
                if (ctr is TextBox)
                {
                    ctr.Text = "";
                    // clear any error provider
                    errorProvider1.SetError(ctr, "");

                }
                else if (ctr is CheckedListBox)
                {

                    CheckedListBox clb = (CheckedListBox)ctr;
                    foreach (int checkedItemIndex in clb.CheckedIndices)
                    {
                        clb.SetItemChecked(checkedItemIndex, false);
                    }
                }
                else if (ctr is CheckBox)
                {
                    ((CheckBox)ctr).Checked = false;
                }
                else if (ctr is ComboBox)
                {
                    ((ComboBox)ctr).SelectedIndex = 0;
                }
            }
        }

        private void loadQueues()
        {

            listViewStandby.Clear();
            listViewProgress.Clear();
            listViewReady.Clear();

            // Initialize the counters
            int standbyCounter = 0;
            int inprogressCounter = 0;
            int readyCounter = 0;

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
                    standbyCounter = ds.Tables["service"].Rows.Count;
                }
                else if (i == Convert.ToInt16(Constants.IN_PROGRESS))
                {
                    loadListView(ds, listViewProgress);
                    inprogressCounter = ds.Tables["service"].Rows.Count;
                }
                else if (i == Convert.ToInt16(Constants.READY))
                {
                    loadListView(ds, listViewReady);
                    readyCounter = ds.Tables["service"].Rows.Count;

                }

            }

            lblReadyCountVar.Text = readyCounter.ToString();
            lblInprogressCountVar.Text = inprogressCounter.ToString();
            lblStandbyCountVar.Text = standbyCounter.ToString();

            refreshThresholdsAlerts();
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

        private void loadConfiguration()
        {
            config.reload();
            
            //Checkpoint
            //https://stackoverflow.com/questions/1536739/get-a-windows-forms-control-by-name-in-c-sharp

            string[] ctrNames = config.getNames();

            foreach( string ctr in ctrNames )
            {
                //System.Console.WriteLine("Accessing name " + ctr);

                Control[] controlForm = this.Controls.Find(ctr, true);
                if( controlForm.Length > 0 )
                {
                    //System.Console.WriteLine("Control found " + controlForm[0].Name);

                    if( controlForm[0] is TextBox || controlForm[0] is RichTextBox )
                    {
                        //System.Console.WriteLine("Text " + controlForm[0].Name );
                        String value;
                        if (config.settings.TryGetValue(ctr, out value))
                        {
                            controlForm[0].Text = config.settings[ctr];
                        }
                    }
                    else
                    {
                        //System.Console.WriteLine("Another type");
                    }
                }
                else
                {
                    System.Console.WriteLine("Control not found ********************" + ctr);
                }
              

            }

            Control[] contro = this.Controls.Find("rTBCompanyAddr2", true);
            System.Console.WriteLine("Control return  ===========dfsdfsdf=d========================" + contro[0].Name);
            foreach (Control cr in contro)
            {
                //System.Console.WriteLine("Control return  ===========dfsdfsdf=d========================" + cr.Name);
            }

            /*
            string key = "rTBCompanyAddr2";
            Control ctn = this.Controls[key];
            //System.Console.WriteLine("Control return  ====================================" + ctn.Name );

            foreach (Control ctr in gBCompanyInfo.Controls)
            {
                System.Console.WriteLine("Control return  ====================================" + ctr.Name);
                if (ctr.Name == "splitContainer1")
                {
                    System.Console.WriteLine("Splitcontainer  ===============");
                    foreach (Control ctr2 in ctr.Controls)
                    {
                        System.Console.WriteLine("Ctr2  ====================================" );
                    }
                }
            }
                string value;
            if(config.settings.TryGetValue("rTBCompanyAddr21", out value) )
            {
                rTBCompanyAddr2.Text = config.settings["rTBCompanyAddr2"];
            }
            else
            {
                System.Console.WriteLine("No such key====================================" + rTBCompanyAddr2.Name );
            }
            */
            //TODO: complete for the rest of the configuration setttings
        }

        private void loadListView(DataSet ds, ListView myListView)
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

                if (drow.RowState != DataRowState.Deleted)
                {

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

        private void initializeToolTips()
        {
            //tTCompanyAddress.SetToolTip(lblCoAddressConfiguration, "Maximum 4 lines. Maximum 60 characters per line");
        }

        private void InitializeFormElements()
        {
            txtPartCost1.Text = "0.00";
            txtPartCost2.Text = "0.00";
            txtPartCost3.Text = "0.00";
            txtLabor.Text = "0.00";
            txtSubTotal.Text = "0.00";
            txtTotal.Text = "0.00";

            //todo: to be fetch from configuration
            txtTax.Text = "0";
  
            txtReadyWarning.Text = config.settings["txtReadyWarning"];
            txtReadyCritical.Text = config.settings["txtReadyCritical"];
            txtInProgressWarning.Text = config.settings["txtInProgressWarning"];
            txtInProgressCritical.Text = config.settings["txtInProgressCritical"];
            txtStandbyWarning.Text = config.settings["txtStandbyWarning"];
            txtStandbyCritical.Text = config.settings["txtStandbyCritical"];

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

        private List<String> validateControlsIteration(string[] ctrNames)
        {

            List<String> errors = new List<String>();

            foreach (string ctr in ctrNames)
            {
                Control[] controlForm = this.Controls.Find(ctr, true);

                if (controlForm.Length > 0)
                {
                    //System.Console.WriteLine("Control found " + controlForm[0].Name);

                    if (controlForm[0] is TextBox || controlForm[0] is RichTextBox)
                    {
                        System.Console.WriteLine("Text " + controlForm[0].Name );
                        System.Console.WriteLine("Text " + controlForm[0].Text );
                        String msg = "Require field missing or incorrect";
                        System.Windows.Forms.TextBox txtTmp = new TextBox();
                        txtTmp.Name = controlForm[0].Name;
                        txtTmp.Text = controlForm[0].Text;

                        bool precheck = CheckEmptyInput(txtTmp, msg);

                        CheckEmptyInputControl(controlForm[0], msg);

                        if ( precheck == false)
                        {
                            
                            errors.Add(txtTmp.Name);
                        }

                    }
                    else
                    {
                        //System.Console.WriteLine("Another type");
                    }
                }
                else
                {
                    System.Console.WriteLine("Control not found ********************" + ctr);
                }
            }

            return errors;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            Services services = new Services();
            String logFix = "[Save button]: ";
            System.Console.WriteLine(logFix + " in progress... ");

            // https://docs.microsoft.com/en-us/dotnet/framework/winforms/user-input-validation-in-windows-forms

            bool precheck = true;
            String msg = "Require field missing or incorrect";
            
            // Create customer object
            Customer newCustomer = new Customer();
            string[] ctrCustomerNames = newCustomer.getNames();

            List<String> errors = validateControlsIteration(ctrCustomerNames);
           
            if ( DEBUG == 1)
            {
                System.Console.WriteLine("******************************************");
                errors.ForEach(i => Console.Write("{0}\n", i));
                System.Console.WriteLine("******************************************");
            }

            if( errors.Count > 0 )
            {
                // the form contain errors or missing required fields
                precheck = false;
            }

            String first_name = txtFirstName.Text;
            String last_name = txtLastName.Text;
            String address = txtAddress.Text;
            String phone = txtPhone.Text.Trim();
            String location = txtLocation.Text;
            String email = txtEmail.Text;

            String dateIn = getCurrentDateTime(); // 6/17/2018 1:56:42 AM
            String dateETA = dtETAComp.Text;

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

            /************************
             After pre checks ...
            ************************/
            newCustomer.First_Name = first_name;
            newCustomer.Last_Name = last_name;
            newCustomer.Address_1 = address;
            newCustomer.Telephone = phone;
            newCustomer.Location = location;
            newCustomer.Email = email;

            if(precheck == true)
            {
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
            else
            {
                MessageBox.Show(msg);
            }
           
          
        }
        private void calculateTotals( string amountTxtBox )
        {

            part1Cost = Convert.ToDouble(txtPartCost1.Text);
            part2Cost = Convert.ToDouble(txtPartCost2.Text);
            part3Cost = Convert.ToDouble(txtPartCost3.Text);
            labor = Convert.ToDouble(txtLabor.Text);

            subTotal = part1Cost + part2Cost + part3Cost + labor;
            
            txtSubTotal.Text = FormatToDecimals(subTotal);

            taxAmount = subTotal * TAX;
            txtTax.Text = FormatToDecimals(taxAmount);

            total = subTotal + taxAmount;
            txtTotal.Text = FormatToDecimals(total);

        }
        // ******************************************************************
        // Button's events
        // ******************************************************************
        private void txtLabor_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtLabor);
            calculateTotals(txtLabor.Text);
        }
        private void txtLabor_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtPartCost1_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtPartCost1_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtPartCost1);
            calculateTotals(txtPartCost1.Text);
        }
        private void txtPartCost2_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtPartCost2_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtPartCost2);
            calculateTotals(txtPartCost2.Text);
        }
        private void txtPartCost3_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }
        private void txtPartCost3_Leave(object sender, EventArgs e)
        {
            ResetAmount(sender, e, txtPartCost3);
            calculateTotals(txtPartCost3.Text);
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
            String msg = "Please enter a first name";
            CheckEmptyInput(txtFirstName, msg);
        }
        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            String msg = "Please enter a last name";
            CheckEmptyInput(txtLastName, msg);
        }
        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            String msg = "Please enter an address";
            CheckEmptyInput(txtAddress, msg);
        }
        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            String msg = "Please enter city";
            CheckEmptyInput(txtLocation, msg);
        }
        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            String msg = "Please enter a phone number";
            CheckEmptyInput(txtPhone, msg);
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            String msg = "Please enter an email address";
            CheckEmptyInput(txtEmail, msg);
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

        public string getCurrentDateTime()
        {
            DateTime localDate = DateTime.Now;
            String[] cultureNames = { "en-US", "en-GB", "fr-FR",
                                "de-DE", "ru-RU" };
            var culture = new CultureInfo("en-US");
            return localDate.ToString(culture);
        }

        private bool CheckEmptyInputControl(Control txtBox, String msg)
        {
            bool status = true;

            if (txtBox.Text == "")
            {
                errorProvider1.SetError(txtBox, msg);
                status = false;
            }
            else
            {
                errorProvider1.SetError(txtBox, "");
            }
            return status;
        }

        private bool CheckEmptyInput(TextBox txtBox, String msg)
        {
            bool status = true;

            if (txtBox.Text == "")
            {
                errorProvider1.SetError(txtBox, msg);
                status = false;
            }
            else
            {
                errorProvider1.SetError(txtBox, "");
            }
            return status;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Closing the form
            dbh.Close();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {

            if (e.TabPageIndex == Constants.TAB_QUEUES)
            {
                loadQueues();
            }

            if (e.TabPageIndex == Constants.TAB_DATABASE)
            {
                loadDatabase();
            }

            if (e.TabPageIndex == Constants.TAB_CONFIGURATION)
            {
                loadConfiguration();
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

                DataRow[] returnedRow;
                returnedRow = ds1.Tables["service"].Select("id='" + tmp + "'");

                DataRow dr1;

                dr1 = returnedRow[0];
                string title = "Work Order";
                WBDocument document = new WBDocument( wBDisplay, title );
                document.setDataRow(dr1);  // data from database
                
                documentToPrint = document.create();

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

        private void cbDisableAllThresholds_CheckedChanged(object sender, EventArgs e)
        {
            txtReadyWarning.Enabled = false;
        }

        private void refreshThresholdsAlerts()
        {
            int standbyWarning = Convert.ToInt16(txtStandbyWarning.Text);
            int standbyCritical = Convert.ToInt16(txtStandbyCritical.Text);

            // TODO get the number of standby records
            int tmp = 5;
            if( tmp > standbyCritical )
            {
                this.pBStandby.Image = global::DatabasesConnection.Properties.Resources.red_btn;
            }
            else if (tmp > standbyWarning )
            {
                this.pBStandby.Image = global::DatabasesConnection.Properties.Resources.yellow_btn;
            }
            else
            {
                this.pBStandby.Image = global::DatabasesConnection.Properties.Resources.green_btn;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // enable controls
            toggleControlsOnOff(true);
            btnNew.Enabled = false;

            //getNextAvailableServiceId from Database manager
            DatabaseManager new_db = new DatabaseManager();
            int nextId =  new_db.getNextAvailableServiceId(dbh);

            lblId.Text = Convert.ToString(nextId);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("This will reset the form. Are you sure?",
                                     "Confirm Reset!",
                                     MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                clearGroupControls(gBCustomerInfo);
                // disable controls
                toggleControlsOnOff(false);
                lblId.Text = "-";

            }
            else
            {

            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearGroupControls(gBCustomerInfo);
           
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if( !string.IsNullOrEmpty(txtSearch.Text))
            {
                DatabaseManager new_db = new DatabaseManager();
                SQLiteDataReader reader = new_db.searchForOrder(dbh, txtSearch.Text.Trim());
                Boolean keepLooping = true;

                if (reader.HasRows)
                {
                    while (reader.Read() && keepLooping == true )
                    {
                        switch (MessageBox.Show("Select this account?" + "\nName: " + reader["first_name"] + "\nDate: " + reader["date_in"] + "\nDescription: " + reader["description"] + "\nBrand/Model: " + reader["brand_model"], "Found customer(s) ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                        {
                            case DialogResult.Yes:
                                clearGroupControls(gBCustomerInfo);
                                toggleControlsOnOff(true);
                                
                                //load data
                                lblId.Text = reader["id"].ToString();
                                txtFirstName.Text = reader["first_name"].ToString();
                                keepLooping = false;

                                break;
                            case DialogResult.No:

                                break;
                            case DialogResult.Cancel:

                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No record was found");
                }
            }
            else
            {
                MessageBox.Show("You must enter a valid id");
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                )
            {
                e.Handled = true;
            }
        }

        private void btnImageLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PictureBox PictureBox1 = new PictureBox();

                    // Create a new Bitmap object from the picture file on disk,
                    // and assign that to the PictureBox.Image property
                    //PictureBox1.Image = new Bitmap(dlg.FileName);
                    Image img = Image.FromFile(dlg.FileName);
                    MemoryStream tmpStream = new MemoryStream();
                    img.Save(tmpStream, ImageFormat.Png); // change to other format
                    tmpStream.Seek(0, SeekOrigin.Begin);
                    //todo check the size
                    byte[] imgBytes = new byte[2400000];
                    tmpStream.Read(imgBytes, 0, 2400000);

                    try
                    {
                        SQLiteCommand command = new SQLiteCommand(null, dbh);

                        command.CommandText = "INSERT INTO images(image, type) VALUES (@param1, @param2)";
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SQLiteParameter("@param1", imgBytes));
                        command.Parameters.Add(new SQLiteParameter("@param2", "logo"));
     
                        command.ExecuteNonQuery(); 


                        MessageBox.Show("Image succesfully set", "File upload");

                    }
                    catch ( Exception ex )
                    {
                        Console.WriteLine("Exception occured: {0}", ex);
                    }
                    

                    //MessageBox.Show(dlg.FileName, "File!");
                }
            }
        }

        private void loadImage()
        {
            String sql = "SELECT image FROM images WHERE type = 'logo'";

            SQLiteCommand command = new SQLiteCommand(sql, dbh);
            SQLiteDataReader reader = command.ExecuteReader();

            byte[] imageBytes = null;
            imageBytes = (byte[])reader["image"];

            using (var ms = new MemoryStream(imageBytes))
            {
                this.pictureBox1.BackgroundImage = Image.FromStream(ms);
            }

            //this.pictureBox1.BackgroundImage = imageBytes;
        }

        private void btnCompanyConfigApply_Click(object sender, EventArgs e)
        {
            
            //lblCompanyAddress.Text = rtbCompanyaddress.Text;
            //lblDisclaimerText.Text = rtbDisclaimerText.Text;
            //rtbCompanyText.Text = rtbCompanyaddress.Text;

            string invoincetitle = txtInvoiceTitle.Text;
            string companyHeader = rtbCompanyHeader.Text;
            string companyAddr1 = rTBCompanyAddr1.Text;
            string companyAddr2 = rTBCompanyAddr2.Text;
            string companyPhone = rTBCompanyPhone.Text;
            string companyEmail = rTBCompanyEmail.Text;
            string companyWebsite = rTBCompanyWebsite.Text;

            string companyHeaderItalic = "false";
            if (cBCompanyHeaderItalic.Checked == true)
            {
                companyHeaderItalic = "true";
            }

            string companyHeaderBold = "false";
            if (cBCompanyHeaderBold.Checked == true)
            {
                companyHeaderBold = "true";
            }

            string disclaimerTitle = rtbDisclaimerTitle.Text;
            string disclaimerText = rtbDisclaimerText.Text;

            string disclaimerHeaderItalic = "false";
            if (cBDisclaimerHeaderItalic.Checked == true)
            {
                disclaimerHeaderItalic = "true";
            }

            string disclaimerHeaderBold = "false";
            if (cBDisclaimerHeaderBold.Checked == true)
            {
                disclaimerHeaderBold = "true";
            }

            Hashtable updateData = new Hashtable();
            //Dictionary<string, string> updateData = new Dictionary<string, string>();

            updateData.Add("txtInvoiceTitle", invoincetitle);
            updateData.Add("rtbCompanyHeader", companyHeader);
            updateData.Add("rTBCompanyAddr1", companyAddr1 );
            updateData.Add("rTBCompanyAddr2", companyAddr2 );
            updateData.Add("rTBCompanyPhone", companyPhone);
            updateData.Add("rTBCompanyEmail", companyEmail );
            updateData.Add("rTBCompanyWebsite", companyWebsite );
            updateData.Add("cBCompanyHeaderItalic", companyHeaderItalic );
            updateData.Add("cBCompanyHeaderBold", companyHeaderBold );
            updateData.Add("rtbDisclaimerTitle", disclaimerTitle );
            updateData.Add("rtbDisclaimerText", disclaimerText );
            updateData.Add("cBDisclaimerHeaderItalic", disclaimerHeaderItalic );
            updateData.Add("cBDisclaimerHeaderBold", disclaimerHeaderBold );

            config.updateSettings(updateData);
        }

        private void txtTaxRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyDigitsAndSinglePoint(sender, e);
        }

        private void btnSendEmailTest_Click(object sender, EventArgs e)
        {
            MailAddress from = new MailAddress("give me email address", "give me name");
            MailAddress to = new MailAddress("give me email address", "give me name");
            List<MailAddress> cc = new List<MailAddress>();
            //cc.Add(new MailAddress("Someone@domain.topleveldomain", "Name and stuff"));
            SendEmail("Want to test this damn thing", from, to, cc);
        }
//TODO: move to own class
        protected void SendEmail(string _subject, MailAddress _from, MailAddress _to, List<MailAddress> _cc, List<MailAddress> _bcc = null)
        {
            //https://stackoverflow.com/questions/10940732/sending-emails-from-a-windows-forms-application
            string Text = "";
            SmtpClient mailClient = new SmtpClient("give me smtp settings", 25);

            NetworkCredential cred = new System.Net.NetworkCredential("emailAdressToLogin", "password");

            MailMessage msgMail;
            Text = "Stuff";
            msgMail = new MailMessage();
            msgMail.From = _from;
            msgMail.To.Add(_to);
            foreach (MailAddress addr in _cc)
            {
                msgMail.CC.Add(addr);
            }
            if (_bcc != null)
            {
                foreach (MailAddress addr in _bcc)
                {
                    msgMail.Bcc.Add(addr);
                }
            }
            msgMail.Subject = _subject;
            msgMail.Body = Text;
            msgMail.IsBodyHtml = true;
            // Send our account login details to the client.
            mailClient.Credentials = cred;
            mailClient.EnableSsl = true;
            mailClient.Send(msgMail);
            msgMail.Dispose();
        }

        private void listViewReady_MouseClick(object sender, MouseEventArgs e)
        {
            // https://stackoverflow.com/questions/13437889/showing-a-context-menu-for-an-item-in-a-listview
            if (e.Button == MouseButtons.Right)
            {
                if (listViewReady.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        /*
       private void rtbCompanyaddress_KeyPress(object sender, KeyPressEventArgs e)
       {
           //https://stackoverflow.com/questions/1048425/limit-number-of-lines-in-net-textbox
           //https://stackoverflow.com/questions/2425847/current-line-and-column-numbers-in-a-richtextbox-in-a-winforms-application
           //System.Console.WriteLine(e.KeyChar);
           numberOfChars++;
           //System.Console.WriteLine("=" + numberOfChars);
           //System.Console.WriteLine("Line index:" + this.rtbCompanyaddress.SelectionStart);
           int index = this.rtbCompanyaddress.SelectionStart;
           System.Console.WriteLine("Line :" + this.rtbCompanyaddress.GetLineFromCharIndex(index));
           int line = this.rtbCompanyaddress.GetLineFromCharIndex(index);
           // Get the column.
           int firstChar = this.rtbCompanyaddress.GetFirstCharIndexFromLine(line);
           int column = index - firstChar;
           System.Console.WriteLine("column :" + column);

           if (numberOfChars >= Constants.COMPANY_ADDRESS_MAX_CHARS )
           {
               System.Console.WriteLine("Need new line");
           }

           if( e.KeyChar == '\r' )
           {
               numberOfChars = 0;
               System.Console.WriteLine("Need reset");
           }
           //System.Console.WriteLine(this.rtbCompanyaddress.Lines.Length);
           if (this.rtbCompanyaddress.Lines.Length >= Constants.COMPANY_ADDRESS_MAX_LINES &&
               e.KeyChar == '\r')
           {
               System.Console.WriteLine( "4 lines reached" );
               e.Handled = true;
           }
       }

       private void rtbCompanyaddress_TextChanged(object sender, EventArgs e)
       {
           if (this.rtbCompanyaddress.Lines.Length > Constants.COMPANY_ADDRESS_MAX_LINES)
           {
               this.rtbCompanyaddress.Undo();
               this.rtbCompanyaddress.ClearUndo();
               MessageBox.Show("Only " + 
                   Constants.COMPANY_ADDRESS_MAX_LINES + 
                   " lines are allowed.");
           }
       } */
    } 
}
