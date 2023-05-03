using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using alchemySoft;

namespace DynamicMenu.Accounts.Report.UI
{
    public partial class RptTrialBalance : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {

            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/RptTrialBalance.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    { 
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtDate.Text = td;
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
                   this.GetType(), "OpenWindow", "window.open('../Report/rptTrialBalance.aspx','_newtab');", true);
                //dbFunctions.showMessage(Page,"../Report/rptTrialBalance.aspx");
            }
        }
    }
}