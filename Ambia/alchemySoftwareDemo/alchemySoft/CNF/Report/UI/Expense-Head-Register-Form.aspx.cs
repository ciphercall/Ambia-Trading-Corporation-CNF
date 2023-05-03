using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.CNF.Report.UI
{
    public partial class Expense_Head_Register_Form : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }
            else
            {
                string formLink = "/CNF/REPORT/UI/Expense-Head-Register-Form.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");

                if (permission)
                {
                    if (!IsPostBack)
                    {
                        DateTime td = dbFunctions.timezone(DateTime.Now);
                        txtFromDate.Text = td.ToString("dd/MM/yyyy");
                        txtToDate.Text = td.ToString("dd/MM/yyyy");
                        txtExpenseNM.Focus();
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
            Session["fromdate"] = txtFromDate.Text;
            Session["todate"] = txtToDate.Text;
            Session["expenseID"] = txtExpenseID.Text;


            //Response.Write("/CNF/Report/Report/rpt-Exp-Head-Register.aspx");
             ScriptManager.RegisterStartupScript(this,
                      this.GetType(), "OpenWindow", "window.open('../Report/rpt-Exp-Head-Register.aspx','_newtab');", true);


            // ScriptManager.RegisterStartupScript(this,
            //          this.GetType(), "OpenWindow", "window.open('../Report/RptExpHeadRegister.aspx','_newtab');", true);

        }
    }
}