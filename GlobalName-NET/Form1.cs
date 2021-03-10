using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace GlobalName_NET
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            String RESTRequest = "";
            String Options = "";

            // *************************************************************************************
            // Set the License String in the Request
            // *************************************************************************************
            RESTRequest += @"&id=" + Uri.EscapeDataString(txtLicense.Text);

            // *************************************************************************************
            // Set the Options in the Request
            // *************************************************************************************
            if (optCorrectFirstName.Checked)
                Options += "CorrectFirstName:ON,";

            Options += "NameHint:" + (optNameHint.SelectedItem) + ",";

            Options += "GenderPopulation:" + (optGenderPop.SelectedItem) + ",";

            Options += "GenderAggression:" + (optGenderAggression.SelectedItem) + ",";

            string SalOrderNew = "";

            switch (optSalutation1.SelectedIndex)
            {
                case 0: SalOrderNew += "Formal|"; break;
                case 1: SalOrderNew += "Informal|"; break;
                case 2: SalOrderNew += "FirstLast|"; break;
                case 3: SalOrderNew += "Blank|"; break;
                default:
                    break;
            }
            switch (optSalutation2.SelectedIndex)
            {
                case 0: SalOrderNew += "Formal|"; break;
                case 1: SalOrderNew += "Informal|"; break;
                case 2: SalOrderNew += "FirstLast|"; break;
                case 3: SalOrderNew += "Blank|"; break;
                default:
                    break;
            }
            switch (optSalutation3.SelectedIndex)
            {
                case 0: SalOrderNew += "Formal|"; break;
                case 1: SalOrderNew += "Informal|"; break;
                case 2: SalOrderNew += "FirstLast|"; break;
                case 3: SalOrderNew += "Blank|"; break;
                default:
                    break;
            }
            switch (optSalutation4.SelectedIndex)
            {
                case 0: SalOrderNew += "Formal|"; break;
                case 1: SalOrderNew += "Informal|"; break;
                case 2: SalOrderNew += "FirstLast|"; break;
                case 3: SalOrderNew += "Blank|"; break;
                default:
                    break;
            }

            if (SalOrderNew != "")
                Options += "Salutation:" + SalOrderNew.TrimEnd('|');

            Options = Options.TrimEnd(',');
 
            RESTRequest += @"&opt=" + Options;
       
            // *************************************************************************************
            // Set the Input Parameters
            // *************************************************************************************
            RESTRequest += @"&full=" + Uri.EscapeDataString(txtNameIn.Text);
            RESTRequest += @"&comp=" + Uri.EscapeDataString(txtCompanyIn.Text);
            RESTRequest += @"&ctry=" + Uri.EscapeDataString(txtCountryIn.Text);

            // Set JSON Response Protocol
            RESTRequest += @"&format=json";

            // Build the final REST String Query
            RESTRequest = @"https://globalname-beta.melissadata.net/V3/WEB/GlobalName" + @"/doGlobalName?t=" + RESTRequest;

            // Output the REST Query
            txtRESTRequest.Text = RESTRequest;

            // *************************************************************************************
            // Submit to the Web Service. 
            // Make sure to set a retry block in case of any timeouts
            // *************************************************************************************
            Boolean Success = false;
            Int16 RetryCounter = 0;
            Stream ResponseReaderFile = null;
            do
            {
                try
                {
                    HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(RESTRequest);
                    WebResponse Response = WebRequestObject.GetResponse();
                    ResponseReaderFile = Response.GetResponseStream();
                    Success = true;
                }
                catch (Exception ex)
                {
                    RetryCounter++;
                    MessageBox.Show("Exception: " + ex.Message);
                    return;
                }
            } while ((Success != true) && (RetryCounter < 5));

            // *************************************************************************************
            // Output Formatted JSON String
            // *************************************************************************************
            StreamReader Reader = new StreamReader(ResponseReaderFile, Encoding.UTF8);
            String ResponseString = Reader.ReadToEnd();

            txtResponse.Text = JValue.Parse(ResponseString).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        // *************************************************************************************
        // Clear all
        // *************************************************************************************
        private void btnClear_Click(object sender, EventArgs e)
        {
            // inputs
            txtNameIn.Text = string.Empty;
            txtCompanyIn.Text = string.Empty;

            // options
            optCorrectFirstName.Checked = false;
            optNameHint.SelectedItem = "Varying";
            optGenderPop.SelectedItem = "Mixed";
            optGenderAggression.SelectedItem = "Neutral";
            optSalutation1.SelectedItem = "Formal";
            optSalutation2.SelectedItem = "Informal";
            optSalutation3.SelectedItem = "FirstLast";
            optSalutation4.SelectedItem = "Blank";

            // request and response
            txtRESTRequest.Text = string.Empty;
            txtResponse.Text = string.Empty;
        }

        // *************************************************************************************
        // Wiki Link
        // *************************************************************************************
        private void lnkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://wiki.melissadata.com/index.php?title=Global_Name");
        }
    }
}
