using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft;

namespace DynamicMenu.Error
{
    public partial class ErrorList : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string dt = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                txtFR.Text = dt;
                txtTO.Text = dt;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string FR = dbFunctions.dateConvertYMD(txtFR.Text);
                string TO = dbFunctions.dateConvertYMD(txtTO.Text);
                dbFunctions.gridViewAdd(GridView1, @"SELECT        ERRLOG.SL, ERRLOG.FORMNM, ERRLOG.ERROR, ERRLOG.ACTIONFR, ASL_USERCO.USERNM, ERRLOG.INTIME
FROM            ASL_USERCO RIGHT OUTER JOIN ERRLOG ON ASL_USERCO.USERID = ERRLOG.USERID WHERE CONVERT(DATE,INTIME) BETWEEN '" + FR + "' AND '" + TO + "'");
            }
            catch { }
        }
    }
}