using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft;

namespace alchemySoft.Asl.Report.UI
{
    public partial class LogReport : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
                Response.Redirect("~/login/ui/signin.aspx");
            else
            {
                const string formLink = "/Asl/Report/UI/LogReport.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission)
                {
                    if (!IsPostBack)
                    { 
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtFrom.Text = td;
                        txtTo.Text = td;
                        dbFunctions.dropDown_Bind(ddlHeadName,"id","all", @"SELECT USERNM NM, USERID ID FROM ASL_USERCO WHERE COMPID='101' AND (USERID not in ('10001','10101'))");
                        dbFunctions.dropDown_Bind(ddlLogType, "id", "all", @"SELECT DISTINCT LOGTYPE NM,LOGTYPE ID FROM ASL_LOG");
                        ddlHeadName.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlHeadName.Text == "" || txtFrom.Text == "" || txtTo.Text == "")
            {
                dbFunctions.showMessage(Page,"Fill Required Data");
            }
            else
            {
                Session["Uid"] = ddlHeadName.SelectedValue;
                Session["UNm"] = ddlHeadName.SelectedItem.Text;
                Session["LOGTYPE"] = ddlLogType.SelectedItem.Text;
                Session["From"] = txtFrom.Text;
                Session["To"] = txtTo.Text;
                 ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "OpenWindow", "window.open('../Report/rptUserLogReport.aspx','_newtab');", true);
            }
        }
    }
}