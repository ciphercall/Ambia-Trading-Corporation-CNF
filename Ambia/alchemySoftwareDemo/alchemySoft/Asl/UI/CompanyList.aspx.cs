using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.Asl.UI
{
    public partial class CompanyList : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        protected void Page_Load(object sender, EventArgs e)
        {
            string TP = CookiesData["USERTYPE"].ToString();
            if (TP == "SUPERADMIN" || TP == "COMPADMIN")
            {
                if (!IsPostBack)
                {
                    if (CookiesData == null || !dbFunctions.permit())
                    {
                        Response.Redirect("~/login/ui/SignIn.aspx");
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                Response.Redirect("default");
            }
           
        }
    }
}