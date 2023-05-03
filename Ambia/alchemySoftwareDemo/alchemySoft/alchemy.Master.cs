using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft
{
    public partial class alchemy : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString.ToString());
            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn.aspx");
            }
        }
        protected void lblLogout_Click(object sender, EventArgs e)
        {
            HttpCookie GetData = new HttpCookie("UserInfo");//Input data
            GetData.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(GetData);
            Response.Redirect("/login/ui/SignIn.aspx");
        }
    }
}