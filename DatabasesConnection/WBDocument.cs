using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabasesConnection
{
    class WBDocument
    {
        private WebBrowser document;  // the  final page
        private string page;          // the working page
        private string titleSection = ""; 
        private string style = "";
        private string separationLine = " <hr> ";
        private string emptyLine = "<br>";
        
        // Global variables
        private string working_title;
        private DataRow working_dr;


        //verbatim strings https://stackoverflow.com/questions/3538484/heredoc-strings-in-c-sharp
        //css https://stackoverflow.com/questions/5496549/how-to-inject-css-in-webbrowser-control
        // Constructor
        public WBDocument(WebBrowser wBDisplay, string title)
        {
            document = wBDisplay;
            setStyle();
            this.working_title = title;
        }

        public void setDataRow(DataRow dr)
        {
            working_dr = dr;
        }

        public void setStyle()
        {
            // For now is ok but make this dynamic  https://social.msdn.microsoft.com/Forums/vstudio/en-US/f11a2a3a-f246-4a7e-94de-9df05a2c00fe/how-to-dynamically-add-array-element-to-stringformat-?forum=csharpgeneral
            string miniStyleTemplate = @"
<style type='text/css'> 
{0} 
{1}
{2}
{3}
{4}
{5}
{6}
{7}
</style>
";
            string h3_tag = "h3 {color:blue }";
            string hr_tag = " hr { margin-top:0em; margin-down:0em;}";
            string p_tag = " p {margin-top:-1em; margin-down:-1em;}";
            string data_class =
                "  #data { width:100%; border: 1px solid #ddd; margin-down:2em;  border-radius:9px; - moz - border - radius:9px; }";
            string data_tr = "#data tr { border: 1px solid #ddd; }";
            string data_th = "#data th { text-align: left; border: 1px solid #ddd; font-weight: normal; }";
            string costLeftFormat = " #costLeft { width : 80% }";
            string costRightFormat = " #costRight { width : 20% }";


            this.style = string.Format(miniStyleTemplate, 
                h3_tag,
                hr_tag,
                p_tag,
                data_class,
                data_tr,
                data_th,
                costLeftFormat,
                costRightFormat
                );  //string format dynamic?
           // System.Console.WriteLine(miniStyleTemplate);
        }

        // Opens the document and sets any css styles
        private void setOpenDocument()
        {

            string miniTemplate = @"
<html><head> {0} </head><body>

";
            this.page = "";
            this.page = string.Format(miniTemplate, this.style);

        }

        private void setCloseDocument()
        {
            this.document.DocumentText = page + "</body></html>";
            System.Console.WriteLine(page + "</body></html>");
        }

        public void createTitle()
        {   
            this.titleSection = " <center><h3><b>" + this.working_title + "</b></h3></center> ";
            this.page = this.page + this.titleSection + this.separationLine;
        }

        private void createBody()
        {
            // Already inside body tags

            string miniTemplate = @"
<table id='data'>
<tr>
<th><b>Customer:</b> {0} </th>  <th><b> Date in:</b>  {1}  </th> <th> <b>Service Id:</b> {2}</th>
</tr>
<tr>
<th> <b>Address:</b> {3} </th> <th>&emsp;</th> <th>&emsp;</th> 
</tr>
<tr>
<th> <b>Telephone:</b> {4} </th> <th>  <b>E-mail:</b> {5}</th> <th>&emsp;</th> 
</tr>
</table>

{6} <!-- Separation line -->

<table id='data'>
<tr>
<th><b>Status:</b>{7}</th><th>&emsp;</th><th>&emsp;</th> 
</tr>
<tr>
<th><b>ETA of Comp:</b></th> <th><b>Date of Comp:</b></th> <th><b>Date of Delivered or P/U:</b></th> 
</tr>
<tr>
<th>{8}</th><th>{9}</th><th>{10}</th> 
</tr>
</table>

{11} <!-- Separation line -->

<table id='data'>
<tr>
<th><b>Brand/Model:</b></th> <th><b>Access:</b></th> <th><b>condition:</b></th> 
</tr>
<tr>
<th>{12}</th><th>{13}</th><th>{14}</th> 
</tr>
</table>

<table id='data'> <!-- One column -->
<tr>
<th><b>Description:</b></th> 
</tr>
<tr>
<th>{15}<th> 
</tr>
<tr>
<th><b>Work Performed:</b></th> 
</tr>
<tr>
<th>{16}</th> 
</tr>
</table>

{17} <!-- Separation line -->

{18}  <!-- Additional cost template here-->

<table id='data'>  <!-- Two columns -->
<tr>
<th id='costLeft'><b>Labor:</b></th><th id='costRight'>dssd&emsp; </th>
</tr>
<tr>
<th id='costLeft'><b>Subtotal:</b></th><th id='costRight'>sds&emsp; </th>
</tr>
<tr>
<th id='costLeft'><b>Tax:</b></th><th id='costRight'>sdsds&emsp; </th>
</tr>
<tr>
<th id='costLeft'><b>Total:</b></th><th id='costRight'>sdss&emsp; </th>
</tr>
</table>
";
            // <p><b></b></p>
            string completeName = working_dr["first_name"].ToString()
                + " "
                + working_dr["last_name"].ToString();

            string telephone, email, addr, status = "";

            string brand_model, access, condition, description, worked_perfomed = "";

            addr = working_dr["address_1"].ToString();
           
            telephone = working_dr["telephone"].ToString();
            email = working_dr["email"].ToString();

            if (working_dr["status"].ToString() == Constants.STANDBY)
            {
                status = "Standby";
            }
            else if (working_dr["status"].ToString() == Constants.IN_PROGRESS)
            {
                status = "Completed";
            }
            else if (working_dr["status"].ToString() == Constants.READY)
            {
                status = "In progress";
            }
            else if (working_dr["status"].ToString() == Constants.COMPLETED)
            {
                status = "Ready for pick up/deliver";
            }
            else if (working_dr["status"].ToString() == Constants.REMOVED)
            {
                status = "Service removed";
            }

            brand_model = (working_dr["brand_model"].ToString() != "") ? working_dr["brand_model"].ToString() : "N/A";
            access = (working_dr["access"].ToString() != "") ? working_dr["access"].ToString() : "N/A";
            condition = (working_dr["condition"].ToString() != "") ? working_dr["condition"].ToString() : "N/A";
            description = ( working_dr["description"].ToString() != "" ) ? working_dr["description"].ToString()  : "N/A";
            worked_perfomed = (working_dr["work_performed"].ToString() != "") ? working_dr["work_performed"].ToString() : "N/A";

            string additionalCostsSection = createAdditionalCosts();

            this.page = this.page 
                + 
                string.Format(miniTemplate, 
                completeName, 
                "01/01/2000", 
                "110", 
                addr,
                telephone,
                email,
                separationLine,
                status,
                "02/01/2000",
                "03/01/2000",
                "04/01/2000",
                separationLine,
                brand_model,
                access,
                condition,
                description,
                worked_perfomed,
                separationLine,
                additionalCostsSection
                );


            
        }
        private string createAdditionalCosts()
        {
            string additionalCostminiTemplate = @"
<table id='data'>
<tr>
<th><b>Additional costs</b></th><th>&emsp;</th>
</tr>
{0}
{1}
{2}
</table>
";
            //string cost1Template, cost2Template, cost3Template = "";
            string[] costTemplates = new string[4];
            int countAdditionalCost = 0;
            for (int i = 1; i <= 3; i++)
            {
                if (!string.IsNullOrEmpty(working_dr["part" + i].ToString()) && !string.IsNullOrEmpty(working_dr["part" + i + "Cost"].ToString()))
                {

                    string temp = @"
<tr>
<th id='costLeft'>{0}</th><th id='costRight'>{1}</th>
</tr>
";
                    costTemplates[i] = string.Format(temp, working_dr["part" + i].ToString(), working_dr["part" + i + "Cost"].ToString());
                    countAdditionalCost++;
                }
                else
                {
                    costTemplates[i] = "";
                }
            }

            string additionalCosts = "";
            if (countAdditionalCost == 0)
            {
                return additionalCosts = "";
            }
            else
            {
                return additionalCosts = string.Format(additionalCostminiTemplate, costTemplates);
            }

        }

        public WebBrowser create()
        {
            //Create the opening tags
            setOpenDocument();

            //create the title 
            createTitle();

            // create the body
            createBody();

            // Adds the closing tags
            setCloseDocument();


            return document;


        }
    }
}
