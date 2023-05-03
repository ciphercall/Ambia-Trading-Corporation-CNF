using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using alchemySoft;

namespace DynamicMenu.Accounts.Report.UI
{
    public partial class RptReceiptsPayStat : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/RptReceiptsPayStat.aspx";
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
                dbFunctions.showMessage(Page,"Fill Required Data");
            }
            else
            {
                Session["From"] = txtFrom.Text;
                Session["To"] = txtTo.Text;
                ScriptManager.RegisterStartupScript(this,
                   this.GetType(), "OpenWindow", "window.open('../Report/rptReceiptPaymentState.aspx','_newtab');", true);
                //Response.Redirect("../Report/rptReceiptPaymentState.aspx");
            }
        }
    }
}