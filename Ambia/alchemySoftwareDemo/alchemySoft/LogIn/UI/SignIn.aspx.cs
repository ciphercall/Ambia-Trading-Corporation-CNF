using alchemySoft.LogIn.DataAccess;
using alchemySoft.LogIn.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.LogIn.UI
{
    public partial class SignIn : System.Web.UI.Page
    {
        LogInDataAccess dob = new LogInDataAccess();
        LogInInterface iob = new LogInInterface();
        HttpCookie GetData = new HttpCookie("UserInfo");//Input data
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CookiesData != null)
                    Response.Redirect("/Default.aspx");
                else
                    txtUser.Focus();
            }
        }

        public string FieldCheck()
        {
            string checkResult = "";
            if (txtUser.Text == "")
            {
                checkResult = "Please write email of user name.";
                txtUser.Focus();
            }
            else if (txtPassword.Text == "")
            {
                checkResult = "Please write password.";
                txtPassword.Focus();
            }
            else
            {
                checkResult = "true";
            }

            return checkResult;
        }

        public void SessionDeclare(string user)
        {
            SqlConnection con = new SqlConnection(dbFunctions.Connection);
            con.Open();
            string query = "SELECT USERNM, OPTP, COMPID, USERID FROM ASL_USERCO WHERE LOGINID='" + user + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                GetData["PERMITID"] = dbFunctions.encrypt(dr[0].ToString());
                GetData["USERNAME"] = dr[0].ToString();
                GetData["USERTYPE"] = dr[1].ToString();
                GetData["COMPANYID"] = dr[2].ToString();
                GetData["USERID"] = dr[3].ToString();
                //Session["BrCD"] = dr[4].ToString();
                //Session["BRANCHID"] = dr[4].ToString();
            }
            dr.Close();
            con.Close();
            GetData["LOGINID"] = user;


            GetData["ipAddress"] = txtIp.Text;
            GetData["PCName"] = dbFunctions.userPc();
            GetData["LOCATION"] = txtLotiLongTude.Text;
            GetData.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(GetData);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            if (FieldCheck() == "true")
            {
                string passByEmial =
                    dbFunctions.getData("SELECT LOGINPW FROM ASL_USERCO WHERE LOGINID='" + txtUser.Text + "'");

                if (passByEmial != "")
                {

                    if (passByEmial == dbFunctions.encrypt(txtPassword.Text))
                    {
                        string timeFrom =
                            dbFunctions.getData("SELECT TIMEFR FROM ASL_USERCO WHERE LOGINID='" + txtUser.Text +
                                                   "'");
                        string timeTo =
                            dbFunctions.getData("SELECT TIMETO FROM ASL_USERCO WHERE LOGINID='" + txtUser.Text +
                                                   "'");
                        string userStatus =
                            dbFunctions.getData("SELECT STATUS FROM ASL_USERCO WHERE LOGINID='" + txtUser.Text +
                                                   "'");
                        DateTime todayDate = dbFunctions.timezone(DateTime.Now);
                        TimeSpan logTimeSpan = TimeSpan.Parse(todayDate.ToString("HH:mm:ss"));
                        TimeSpan timeForSpan = TimeSpan.Parse(timeFrom);
                        TimeSpan timeToSpan = TimeSpan.Parse(timeTo);
                        if (timeForSpan <= logTimeSpan && logTimeSpan <= timeToSpan && userStatus == "A")
                        {
                            SessionDeclare(txtUser.Text);
                            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
                            // logdata add start //
                            string lotileng = txtLotiLongTude.Text;
                            string logdata = "Log in Id: " + txtUser.Text + ", User Type: " + CookiesData["USERTYPE"].ToString();
                            string logid = "LOGIN";
                            string tableid = "ASL_USERCO";
                            LogData.InsertLogData(lotileng, logid, tableid, logdata, txtIp.Text);
                            // logdata add start //
                            string urllink = txtlink.Text;
                            if (urllink == "" || urllink == "javascript:__doPostBack('ctl00$lnkLogOut','')")
                                Response.Redirect("/Default.aspx");
                            else
                                Response.Redirect(urllink);

                        }
                        else
                        {
                            lblMsg.Text = "Your log in time: " +
                                          DateTime.ParseExact(timeFrom, "HH:mm", null).ToString("hh:mm tt") + " to." +
                                          DateTime.ParseExact(timeTo, "HH:mm", null).ToString("hh:mm tt") + "";
                            lblMsg.Visible = true;
                        }

                    }
                    else
                    {
                        lblMsg.Text = "Wrong user name & password.";
                        lblMsg.Visible = true;
                    }
                }
                else
                {
                    lblMsg.Text = "Wrong user name & password.";
                    lblMsg.Visible = true;
                }
            }
            else
            {
                lblMsg.Text = FieldCheck();
                lblMsg.Visible = true;
            }
        }
    }
}
