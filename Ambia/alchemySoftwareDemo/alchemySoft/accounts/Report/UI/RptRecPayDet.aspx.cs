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
    public partial class RptRecPayDet : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/RptRecPayDet.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    { 
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtFrom.Text = td;
                        txtTo.Text = td;
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
            if (txtFrom.Text == "" || txtTo.Text == "")
            {
                Response.Write("<script>alert('Fill Required Data');</script>");
            }
            else
            {
                Session["From"] = txtFrom.Text;
                Session["To"] = txtTo.Text;
                ScriptManager.RegisterStartupScript(this,
                   this.GetType(), "OpenWindow", "window.open('../Report/rptRecPayDet.aspx','_newtab');", true);
                // Response.Redirect("~/Accounts/Report/Report/rptRecPayDet.aspx");
            }
        }
    }
}