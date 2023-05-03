using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.LogIn.UI
{
    public partial class forgetPassword : System.Web.UI.Page
    {
        HttpCookie getData = new HttpCookie("aLchEMyasL");//Input data
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["aLchEMyasL"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                //mV.ActiveViewIndex = 1;
                if (CookiesData != null)
                {
                    string token = CookiesData["aSLTn"];
                    string email1 = CookiesData["aSLTnEmail"];
                    if (token == "")
                        mV.ActiveViewIndex = 0;
                    else if (tokenid.Text == "")
                    {
                        mV.ActiveViewIndex = 1;
                        sentEmail.Text = email1;
                        email.Text = email1;
                    }
                    else
                        mV.ActiveViewIndex = 2;
                }
                else
                    mV.ActiveViewIndex = 0;
            }
        }

        protected void reset_Click(object sender, EventArgs e)
        {
            sendMail();
        }
        private void sendMail()
        { 
            if (errorCheck())
            {
                msg.Visible = false;
                msgtokenEntry.Visible = false;
                string token = RandomString().Substring(0, 10);
                //email send
                string message = emailBody(token); 
                bool x = dbFunctions.sendMailPage("noreply@alchemy-bd.com", email.Text, "alchemysoftware@yahoo.com" ,"", message, "Password Reset Code", "mail.alchemy-bd.com", "noreply@alchemy-bd.com", "idUd763~@Al");
                if (x)
                {
                    getData["aSLTn"] = dbFunctions.encrypt(token);
                    getData["aSLTnEmail"] = email.Text;
                    sentEmail.Text = email.Text;
                    getData.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(getData);
                    mV.ActiveViewIndex = 1;
                }
            }
        }
        private static string emailBody(string token)
        {
            string body = "";
            body += "<html> <body> ";
            body += " <div style=\"width: 400px; margin: 0 auto; background:#c3c3c3;border-radius:10px; height:290px;padding:18px;    margin-top: 51px;\">";
            body += "<div style=\"text-align: center; margin-top:-52px !important; width: 100px; margin: 0 auto; background: #190707;border-radius: 87px;height: 100px;\">";
            body += "<img style=\"margin-top: 22px; \" src=\"https://alchemy-bd.com/wp-content/themes/alchemy/images/alchemy.png\" /> </div>";
            body += "<div style=\"margin-bottom: 12px; \"> Hi !</div>Please use this code to reset the password";
            body += "<div style=\"margin: 0 auto; background: #190707;width: 173px;color: #fff;padding: 9px;border-radius: 10px;margin-top: 50px;text-align:center\">" + token + "</div>";
            body += "<br /> Thanks,<br />";
            body += "<a href=\"https://alchemy-bd.com\" target=\"_blank\" style=\"color:#fff;font-weight:bold; text-decoration:none\">Alchemy Software </a>";
            body += "</div></body></html>";
            return body;
        }
        private string RandomString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }
        private bool errorCheck()
        {
            bool result = true;
            if (email.Text == "Your e-mail address")
            {
                msg.Visible = true;
                msgtokenEntry.Visible = true;
                msgtokenEntry.Text = "Please enter your email address to sent token id for recover your account.";
                result = false;
            }
            else
            {
                string check = dbFunctions.getData("SELECT EMAILID FROM ASL_USERCO WHERE EMAILID='" + email.Text + "'");
                if (check == "")
                {
                    msg.Visible = true;
                    msgtokenEntry.Visible = true;
                    msgtokenEntry.Text = "Email not found .";
                    result = false;
                }
                else if (!dbFunctions.emailValidation(check))
                {
                    msg.Visible = true;
                    msgtokenEntry.Visible = true;
                    msgtokenEntry.Text = "Invalied email address.Contact with developer";
                    result = false;
                }
            }
            return result;
        }

        protected void dontGet_Click(object sender, EventArgs e)
        {
            sendMail();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            msg.Visible = false;
            msgtokenEntry.Visible = false;
            if (tokenid.Text == "Enter Token Number")
            {
                msg.Visible = true;
                msgtokenEntry.Visible = true;
                msgtokenEntry.Text = "Please enter a code.";
            }
            else
            {
                string token = CookiesData["aSLTn"];
                string hashToken = dbFunctions.encrypt(tokenid.Text);
                if (hashToken == token)
                {
                    mV.ActiveViewIndex = 2;
                }
                else
                {
                    msg.Visible = true;
                    msgtokenEntry.Visible = true;
                    msgtokenEntry.Text = "The Token Number that you've entered doesn't match your code. Please try again.";
                }
            }
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            string pass = dbFunctions.passwordStrongChecker(Pass.Text);
            if (pass != "")
            {
                msg.Visible = true;
                msgtokenEntry.Visible = true;
                msgtokenEntry.Text = pass;
            }
            else if(cPass.Text == "")
            {
                msg.Visible = true;
                msgtokenEntry.Visible = true;
                msgtokenEntry.Text = "Confirm password required.";
            }
            else if (Pass.Text!= cPass.Text)
            {
                msg.Visible = true;
                msgtokenEntry.Visible = true;
                msgtokenEntry.Text = "Password missmatch ✖";
            } 
            else
            {
                string email = CookiesData["aSLTnEmail"];
                string execute = passwordUpdate(email,Pass.Text);
                if(execute=="true")
                {
                     
                    Session.Clear();
                    dbFunctions.popupAlert(Page, "Success : Password Changed !", "s");
                    HttpCookie GetData = new HttpCookie("aLchEMyasL");//Input data
                    GetData.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(GetData);
                    Response.Redirect("/login/ui/signIn");
                }
            }
        }
        public string passwordUpdate(string email,string password)
        {
           
            string s = "";
            SqlTransaction tran = null;
            SqlConnection con = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand("", con);
            try
            {
                if (con.State != ConnectionState.Open)  con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"UPDATE ASL_USERCO SET LOGINPW=@PASSWORD WHERE EMAILID=@EMAILID ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@PASSWORD", SqlDbType.NVarChar).Value =dbFunctions.encrypt(password);
                cmd.Parameters.Add("@EMAILID", SqlDbType.NVarChar).Value = email;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                s = "true";
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed) con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }
    }
}