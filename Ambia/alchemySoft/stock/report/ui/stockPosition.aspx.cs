using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.stock.report.ui
{
    public partial class stockPosition : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string formLink = "/stock/report/ui/stockPosition";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtDateFr.Text = td;
                        txtDateTo.Text = td;
                        dbFunctions.dropDown_Bind(ddlStore, "id", "all", "SELECT STORENM NM, STOREID ID FROM STK_STORE");
                        ddlStore.Focus();
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
            Session["FRDT_!"] = txtDateFr.Text;
            Session["TODT_!"] = txtDateTo.Text;
            Session["STOREID_!"] = ddlStore.Text;
            Session["STORENM_!"] = ddlStore.SelectedItem;
            dbFunctions.Redirect("/stock/report/view/rpt_stockPosition",Page);
        }
    }
}