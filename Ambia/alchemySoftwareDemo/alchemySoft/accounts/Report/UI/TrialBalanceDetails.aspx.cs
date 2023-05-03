using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlchemyAccounting;
using alchemySoft;

namespace DynamicMenu.Accounts.Report.UI
{
    public partial class TrialBalanceDetails : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
protected void Page_Load(object sender, EventArgs e)
        { 
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/TrialBalanceDetails.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    { 
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtFromDate.Text = td;
                        txtToDate.Text = td;
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
            if (txtFromDate.Text == "")
            {
                Response.Write("<script>alert('Select From Date.');</script>");
            }
            else if (txtToDate.Text == "")
            {
                Response.Write("<script>alert('Select To Date.');</script>");
            }
            else
            {
                Session["FromDate"] = txtFromDate.Text;
                Session["ToDate"] = txtToDate.Text;

                 ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "OpenWindow", "window.open('../Report/rptTrialBalanceDetails.aspx','_newtab');", true);
            }
        }
    }
}