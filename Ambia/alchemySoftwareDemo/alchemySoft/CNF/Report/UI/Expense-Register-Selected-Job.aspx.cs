using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft;
using AlchemyAccounting;

namespace alchemySoft.CNF.Report.UI
{  // ReSharper disable once InconsistentNaming
    public partial class Expense_Register_Selected_Job : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
                Response.Redirect("~/Login/UI/signin.aspx");
            else
            {
                string formLink = "/CNF/Report/UI/Expense-Register-Selected-Job.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission)
                {
                    if (!IsPostBack)
                    {
                        txtJobID.Focus();
                    }
                }
                else
                {
                   Response.Redirect("/default");
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtJobID.Text == "")
            {
                lblErrmsg.Text = "Input Field Missing";
                txtJobID.Focus();
            }
            else
            {
                Session["jobno"] = txtJobID.Text;
                Session["jobtp"] = txtJobType.Text;
                Session["jobyy"] = txtJobYear.Text;
                 ScriptManager.RegisterStartupScript(this,
                          this.GetType(), "OpenWindow", "window.open('../Report/RptExpenseRegCat.aspx','_newtab');", true);
            }
        }
    }
}