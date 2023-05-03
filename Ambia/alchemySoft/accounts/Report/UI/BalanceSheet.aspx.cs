using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using alchemySoft;

namespace DynamicMenu.Accounts.Report.UI
{
    public partial class BasicInformation : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/BalanceSheet.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    { 
                        txtDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    }
                }
                else
                {
                   Response.Redirect("/default");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtDate.Text == "")
            {
                dbFunctions.showMessage(Page,"Select Date.");
            }
            else
            {
                Session["Date"] = txtDate.Text; 
                ScriptManager.RegisterStartupScript(this,
                   this.GetType(), "OpenWindow", "window.open('../Report/rptBalanceSheet.aspx','_newtab');", true);
            }
        }



    }
}