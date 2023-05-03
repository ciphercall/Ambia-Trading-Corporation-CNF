using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using alchemySoft;

namespace DynamicMenu.Accounts.Report.UI
{
    public partial class RptNotesAcc : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
protected void Page_Load(object sender, EventArgs e)
        { 
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/RptNotesAcc.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    { 
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtFrom.Text = td;
                        txtTo.Text = td;
                        ddlLevelID.Focus();
                    }
                }
                else
                {
                   Response.Redirect("/default");
                }
            }
        }

        protected void ddlLevelID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ddlLevelID"] = "";
            if (ddlLevelID.Text == "1")
            {
                Session["ddlLevelID"] = ddlLevelID.Text;
                Session["LevelCD"] = "";
                Session["AccNM"] = "";
            }
            else if (ddlLevelID.Text == "2")
            {
                Session["ddlLevelID"] = ddlLevelID.Text;
                Session["LevelCD"] = "";
                Session["AccNM"] = "";
            }
            else if (ddlLevelID.Text == "3")
            {
                Session["ddlLevelID"] = ddlLevelID.Text;
                Session["LevelCD"] = "";
                Session["AccNM"] = "";
            }
            else if (ddlLevelID.Text == "4")
            {
                Session["ddlLevelID"] = ddlLevelID.Text;
                Session["LevelCD"] = "";
                Session["AccNM"] = "";
            }
            txtHeadNM.Text = "";
            txtAccHeadCD.Text = "";
            txtHeadNM.Focus();
        }

       

        protected void txtHeadNM_TextChanged(object sender, EventArgs e)
        {
            Session["LevelCD"] = "";
            Session["AccNM"] = "";
            if (txtHeadNM.Text != "")
            {
                string txtHDNM = txtHeadNM.Text;
                string trimHDNM = txtHDNM.Substring(0, txtHDNM.Length - 8);
                int Lvl = txtHDNM.LastIndexOf(" (");
                string l = txtHDNM.Substring(Lvl);
                string Level = l.Substring(6, 1);

                Session["AccNM"] = trimHDNM;
                Session["LevelCD"] = Level;

                txtAccHeadCD.Text = "";

                dbFunctions.txtAdd("Select ACCOUNTCD from GL_ACCHART where LEVELCD= '" + Level + "' and ACCOUNTNM = '" + trimHDNM + "'", txtAccHeadCD);
            }
            else
                txtHeadNM.Text = "";
            txtHeadNM.Focus();

            btnSearch.Focus();
        }

        protected void txtFrom_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Focus();
        }

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Focus();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlLevelID.Text == "SELECT")
            {
                dbFunctions.showMessage(Page,"Select Transaction Type.");
                ddlLevelID.Focus();
            }
            else if (txtHeadNM.Text == "")
            {
                dbFunctions.showMessage(Page,"Select Account Head.");
                txtHeadNM.Focus();
            }
            else if (txtFrom.Text == "")
            {
                dbFunctions.showMessage(Page,"Select From Date.");
            }
            else if (txtTo.Text == "")
            {
                dbFunctions.showMessage(Page,"Select To Date.");
            }
            else
            {
                string txtHDNM = txtHeadNM.Text;
                string trimHDNM = txtHDNM.Substring(0, txtHDNM.Length - 8);
                int Lvl = txtHDNM.LastIndexOf(" (");
                string l = txtHDNM.Substring(Lvl);
                string Level = l.Substring(6, 1);

                Session["AccNM"] = trimHDNM;
                Session["LevelCD"] = Level;

                Session["TransLevel"] = ddlLevelID.Text;
                Session["AccCode"] = txtAccHeadCD.Text;
                Session["From"] = txtFrom.Text;
                Session["To"] = txtTo.Text;

                 ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "OpenWindow", "window.open('../Report/rptNotesAccount.aspx','_newtab');", true);
            }
        }
    }
}