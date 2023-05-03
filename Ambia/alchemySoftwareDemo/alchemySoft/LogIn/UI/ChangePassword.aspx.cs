using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft;
using alchemySoft.LogIn.DataAccess;
using alchemySoft.LogIn.Interface;

namespace alchemySoft.LogIn.UI
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        LogInDataAccess dob = new LogInDataAccess();
        LogInInterface iob = new LogInInterface();
        SqlConnection con = new SqlConnection(dbFunctions.Connection);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CookiesData == null ||  !dbFunctions.permit())
                {
                    Response.Redirect("~/logIn/ui/signin");
                }
                else
                { 
                        txtOldPass.Focus();
                     
                }
            }
        }

        public void Refresh()
        {
            txtOldPass.Text = "";
            txtNewPass.Text = "";
            txtConfirmPass.Text = "";
            txtOldPass.Focus();
        }
        public class CheckResultWithMsg
        {
            public string Msg { get; set; }
            public bool CheckResult { get; set; }
        }

        public CheckResultWithMsg FieldCheck()
        {
            bool checkResult = false;
            string msg = "";
            if (txtConfirmPass.Text == "")
            {
                msg = "Fill confirm password field.";
            }
            else if (txtNewPass.Text == "")
            {
                msg = "Fill new password field.";
            }
            else if (txtOldPass.Text == "")
            {
                msg = "Fill old password fiel.";

            }
            else if (txtNewPass.Text != txtConfirmPass.Text)
            {
                msg = "Password mismatch.";
            }
            else
            {
                checkResult = true;
            }
            return new CheckResultWithMsg()
            {
                Msg = msg,
                CheckResult = checkResult
            };
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //string unm = Session["UserName"].ToString();

            if (FieldCheck().CheckResult == false)
            {
                Response.Write("<script>alert('" + FieldCheck().Msg + "')</script>");
                Refresh();
            }
            else
            {
                string userId = CookiesData["USERID"].ToString();
                string logPassword = dbFunctions.getData("SELECT LOGINPW FROM ASL_USERCO WHERE  USERID='" + userId + "'");
                if (logPassword == dbFunctions.encrypt(txtOldPass.Text))
                {
                    var txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
                    iob.LotiLengTudeUpdate = txtLotiLongTude.Text;
                    iob.ipAddressUpdate = dbFunctions.ipAddress();
                    iob.UserIdUpdate = Convert.ToInt64(CookiesData["USERID"].ToString());
                    iob.userPcUpdate = dbFunctions.userPc();
                    iob.InTimeUpdate = dbFunctions.timezone(DateTime.Now);

                    iob.UserID = Convert.ToInt64(CookiesData["USERID"].ToString());
                    iob.Password = dbFunctions.encrypt(txtNewPass.Text);

                    string s = dob.UPDATE_ASL_PASSWORD(iob);
                    if (s == "")
                        NullSession();
                    else
                    {
                        dbFunctions.showMessage(Page, "Change Password Failed,Try again.");
                        Refresh();
                    }

                }
                else
                {
                    Response.Write("<script>alert('Current Password Is Wrong')</script>");
                    txtOldPass.Focus();
                    txtOldPass.Text = "";
                }

            }
        }

        public void NullSession()
        {
            HttpCookie GetData = new HttpCookie("UserInfo");//Input data
            dbFunctions.showMessage(Page, "Passwrod Successfully Changed");
            
            GetData.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(GetData);
            Response.Redirect("SignIn.aspx");
        }
    }
}
