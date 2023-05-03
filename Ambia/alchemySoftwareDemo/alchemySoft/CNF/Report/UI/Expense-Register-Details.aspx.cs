using System;
using System.Web;
using System.Web.UI;
using alchemySoft;
using AlchemyAccounting;

namespace DynamicMenu.CNF.Report.UI
{
    // ReSharper disable once InconsistentNaming
    public partial class Expense_Register_Details : Page
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
                string formLink = "/CNF/Report/UI/Expense-Register-Details.aspx";
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
            if (txtExpenseNM.Text == "" || txtExpenseID.Text == "")
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Input Date Missing.";
            }

            else
            {
                lblErrmsg.Text = "";
                lblErrmsg.Visible = false;
                Session["fromdate"] = txtFromDate.Text;
                Session["todate"] = txtToDate.Text;
                Session["expenseID"] = txtExpenseID.Text;

                 ScriptManager.RegisterStartupScript(this,
                          GetType(), "OpenWindow", "window.open('../Report/ExpenseRegisterDetails.aspx','_newtab');", true);
            }
        }
    }
}