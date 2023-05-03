using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicMenu.Error
{
    public partial class error : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
protected void Page_Load(object sender, EventArgs e)
        {

        } 
    }
}