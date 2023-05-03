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
    public partial class RptLedgerBook : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/RptLedgerBook.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    {
                        txtacchead.Text = "";
                        //refresh(); 
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtFrom.Text = td;
                        txtTo.Text = td;
                        txtHeadNM.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }

        }

        protected void txtHeadNM_TextChanged(object sender, EventArgs e)
        {
            if (txtHeadNM.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P'and ACCOUNTNM = '" + txtHeadNM.Text + "'", txtacchead);
            }
            else
                txtHeadNM.Text = "";
            txtHeadNM.Focus();
        }
        public void Refresh()
        {
            txtHeadNM.Text = "";
            txtFrom.Text = "";
            txtTo.Text = "";
            txtacchead.Text = "";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtHeadNM.Text == "" || txtFrom.Text == "" || txtTo.Text == "")
            {
                Response.Write("<script>alert('Fill Required Data');</script>");
            }
            else
            {
                Session["AccCode"] = txtacchead.Text;
                Session["AccNM"] = txtHeadNM.Text;
                Session["From"] = txtFrom.Text;
                Session["To"] = txtTo.Text;
                ScriptManager.RegisterStartupScript(this,
                   this.GetType(), "OpenWindow", "window.open('../Report/ReportLedgerBook.aspx','_newtab');", true);
                //Response.Redirect("~/Accounts/Report/Report/ReportLedgerBook.aspx");
            }
        }
    }
}