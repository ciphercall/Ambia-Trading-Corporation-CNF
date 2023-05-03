using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using alchemySoft;

namespace DynamicMenu.Accounts.Report.UI
{
    public partial class CostPoolWIseTransaction : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
                Response.Redirect("~/login/ui/SignIn");
            else
            {
                const string formLink = "/Accounts/Report/UI/CostPoolWIseTransaction.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    {
                        DateTime today = DateTime.Today.Date;
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtFrom.Text = td;
                        txtTo.Text = td;
                        txtCostPool.Focus();
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
            if (txtCostPool.Text == "")
            {
                lblMsg.Text = "Select cost pool name.";
                lblMsg.Visible = true;
                txtCostPool.Focus();
            }
            else if (txtFrom.Text == "")
            {
                lblMsg.Text = "Select from date.";
                lblMsg.Visible = true;
                txtFrom.Focus();
            }
            else if (txtTo.Text == "")
            {
                lblMsg.Text = "Select to date.";
                lblMsg.Visible = true;
                txtTo.Focus();
            }
            else
            {
                string costpoolid = dbFunctions.StringData("SELECT COSTPID FROM GL_COSTP WHERE COSTPNM='" + txtCostPool.Text + "'");
                if (costpoolid == "")
                {
                    lblMsg.Text = "Select cost pool name.";
                    lblMsg.Visible = true;
                    txtCostPool.Focus();
                }
                else
                {
                    lblMsg.Visible = false;

                    Session["costpoolnm"] = null;
                    Session["costpoolid"] = null;
                    Session["From"] = null;
                    Session["To"] = null;

                    Session["costpoolnm"] = txtCostPool.Text;
                    Session["costpoolid"] = costpoolid;
                    Session["From"] = txtFrom.Text;
                    Session["To"] = txtTo.Text;

                     ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "OpenWindow", "window.open('../Report/rpt-costpool-wise-transaction.aspx','_newtab');", true);

                }
            }
        }
    }
}