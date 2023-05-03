using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using alchemySoft;

namespace DynamicMenu.Accounts.Report.UI
{
    public partial class ProjectExpenseStatement : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/ProjectExpenseStatement.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        DateTime today = DateTime.Today.Date;
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtFrom.Text = td;
                        txtTo.Text = td;
                        txtProjectNm.Focus();
                    }
                }
                else
                {
                   Response.Redirect("/default");
                }
            }
        }


        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            // Try to use parameterized inline query/sp to protect sql injection
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            if (uTp == "COMPADMIN")
            {
                cmd = new SqlCommand("SELECT COSTPNM FROM GL_COSTP WHERE COSTPNM LIKE '" + prefixText + "%'", conn);
            }
            else
            {
                cmd = new SqlCommand("SELECT COSTPNM FROM GL_COSTP WHERE COSTPNM LIKE '" + prefixText + "%' AND (CATID ='" + brCD + "' OR CATID IS NULL OR CATID ='')", conn);
            }
            SqlDataReader oReader;
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["COSTPNM"].ToString());
            return CompletionSet.ToArray();
        }

        protected void txtProjectNm_TextChanged(object sender, EventArgs e)
        {
            if (txtProjectNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select COSTPID from GL_COSTP where COSTPNM = '" + txtProjectNm.Text + "'", txtProjectCD);
            }
            else
                txtProjectNm.Text = "";
            txtProjectNm.Focus();
        }

        protected void txtFrom_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Focus();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtProjectNm.Text == "" || txtFrom.Text == "" || txtTo.Text == "")
            {
                dbFunctions.showMessage(Page,"Fill Required Data");
            }
            else if (ddlType.SelectedItem.Text == "--SELECT--")
            {
                dbFunctions.showMessage(Page,"Select Transaction Type");
            }
            else
            {
                Session["ProjectCD"] = txtProjectCD.Text;
                Session["ProjectNM"] = txtProjectNm.Text;
                Session["Typecd"] = ddlType.SelectedValue;
                Session["TypeName"] = ddlType.SelectedItem.Text;
                Session["From"] = txtFrom.Text;
                Session["To"] = txtTo.Text;
                 ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "OpenWindow", "window.open('../Report/rptProjectExpense.aspx','_newtab');", true);
            }
        }
    }
}