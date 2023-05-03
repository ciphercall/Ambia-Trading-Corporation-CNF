using alchemySoft.stock.dataAccess;
using alchemySoft.stock.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft
{
    public class dbFunctions
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
       // public  static String Connection = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ToString();
       public static String Connection = new SqlConnectionStringBuilder { DataSource = "70.32.28.3", InitialCatalog = "asl_ambia", UserID = "ambia", Password = "asl@123%rr" ,MultipleActiveResultSets=true}.ToString();

   
        public static string execute(string str)
        {
            string s = "false";
            SqlConnection Conn = new SqlConnection(Connection);
            try
            {

                if (Conn.State != ConnectionState.Open) Conn.Open();
                SqlCommand cmd = new SqlCommand(str, Conn);
                int count = cmd.ExecuteNonQuery();
                if (count == 0)
                    s = "null";
                else
                    s = "true";
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            catch { if (Conn.State != ConnectionState.Closed) Conn.Close(); }
            return s;
        }
        public static string ExecuteQuery(String sql)
        {
            string data = "";
            SqlConnection con = new SqlConnection(Connection);
            con.Open();
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();

                SqlCommand cmd = new SqlCommand(sql, con) { Transaction = tran };

                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            catch (Exception exception)
            {
                tran.Rollback();
                data = exception.ToString();
            }
            return data;
        }
        public static string StringData(String sql)
        {
            string data = "";
            try
            {
                SqlConnection con = new SqlConnection(Connection);
                if (con.State != ConnectionState.Open) con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    data = reader[0].ToString();
                }
                if (con.State != ConnectionState.Closed)
                    con.Close();
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            return data;
        }
        public static void gridViewAdd(GridView ob, String sql)
        {
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(Connection);
            try
            {

                if (con.State != ConnectionState.Open) con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(table);
                ob.DataSource = table;
                ob.DataBind();
                if (con.State != ConnectionState.Closed) con.Close();
            }
            catch
            {
                if (con.State != ConnectionState.Closed) con.Close();
            }
        }

        public static DateTime timezone(DateTime datetime)
        {
            var timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime printDate = TimeZoneInfo.ConvertTime(datetime, timezoneInfo);
            return printDate;
        }
        public static string ipAddress()
        {

#pragma warning disable 618
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
#pragma warning restore 618
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            return ipAddress.ToString();
        }

        public static string userPc()
        {
            return Dns.GetHostName();
        }
        //public static string FbProfilePicture(string userId)
        //{
        //    //string userid = userId;
        //    //string fbusername = StringData("SELECT FBPIMG FROM ASL_USERCO WHERE USERID='" + userid + "'");
        //    //var fbImageLink = "http:/" + "/graph.facebook.com/" + fbusername + "/picture?type=large";
        //    //return fbImageLink;
        //} 
        public static string splitText(string text, char sumbol, int indexNo)
        {
            string returnText = "";
            string searchPar = text;
            int splitter = searchPar.IndexOf(sumbol);
            if (splitter != -1)
            {
                string[] lineSplit = searchPar.Split(sumbol);
                returnText = lineSplit[indexNo];
            }
            return returnText;
        }

        public static void dropDown_Bind(DropDownList ob, String ID, String Root, String sql)
        {
            SqlConnection con = new SqlConnection(Connection);
            try
            {
                ID = ID.ToUpper();
                Root = Root.ToUpper();
                if (con.State != ConnectionState.Open) con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ob.DataSource = ds;
                ob.DataTextField = "NM";
                if (ID == "ID")
                    ob.DataValueField = "ID";
                ob.DataBind();
                if (Root == "ALL")
                    ob.Items.Insert(0, new ListItem("ALL"));
                else if (Root == "SELECT")
                    ob.Items.Insert(0, new ListItem("--SELECT--"));
                if (con.State != ConnectionState.Closed) con.Close();
            }
            catch
            {
                if (con.State != ConnectionState.Closed) con.Close();
            }
        }
        public static void listBoxBindOnlyName(ListBox ob, String sql)
        {
            SqlConnection con = new SqlConnection(Connection);
            try
            {
                if (con.State != ConnectionState.Open) con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ob.Items.Clear();
                da.Fill(ds);
                ob.DataSource = ds;
                ob.DataTextField = "NM";
                //ddlCountry.DataValueField = "UserId";
                ob.DataBind();
                if (con.State != ConnectionState.Closed) con.Close();
            }
            catch
            {
                if (con.State != ConnectionState.Closed) con.Close();
            }
        }
        public static void gridBlankRow(GridView ob)
        {
            int columncount = ob.Rows[0].Cells.Count;
            ob.Rows[0].Cells.Clear();
            ob.Rows[0].Cells.Add(new TableCell());
            ob.Rows[0].Cells[0].ColumnSpan = columncount;
            ob.Rows[0].Cells[0].Text = "No Records Found";
        }
        public static void txtAdd(String sql, TextBox TxtAdd)
        {
            //String mystring = "";
            SqlConnection con = new SqlConnection(Connection);
            try
            {
                if (con.State != ConnectionState.Open) con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TxtAdd.Text = reader[0].ToString();
                }

                reader.Close();
                if (con.State != ConnectionState.Closed) con.Close();
            }
            catch
            {
                //ignore
                if (con.State != ConnectionState.Closed) con.Close();
            }
            //return List;
        }

        public static void lblAdd(String sql, Label LblAdd)
        {
            SqlConnection con = new SqlConnection(Connection);
            try
            {

                if (con.State != ConnectionState.Open) con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    LblAdd.Text = reader[0].ToString();
                }

                reader.Close();
                if (con.State != ConnectionState.Closed) con.Close();
            }
            catch
            {
                //ignore
                if (con.State != ConnectionState.Closed) con.Close();
            }
        }
        public static void showMessage(Page Page, string msg)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "MessageBox", "alert('" + msg + "')", true);
        }
        public static DateTime dateConvert(string DT)
        {
            if (DT == "")
                DT = "01/01/1900";
            IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
            DateTime date = DateTime.Parse(DT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            return date;
        }
        public static string dateConvertYMD(string DT)
        {
            if (DT == "")
                DT = "01/01/1900";
            IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
            string date = DateTime.Parse(DT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal).ToString("yyyy/MM/dd");
            return date;
        }
        public static string getData(string str)
        {
            string Result = "";
            SqlConnection Conn = new SqlConnection(Connection);
            try
            {


                if (Conn.State != ConnectionState.Open) Conn.Open();
                SqlCommand cmd = new SqlCommand(str, Conn);
                SqlDataReader DR = cmd.ExecuteReader();
                while (DR.Read())
                    Result = DR[0].ToString();
                DR.Close();
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            catch
            {
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            return Result;
        }
        public static decimal decimalConvert(string x)
        {
            if (x == "")
                x = "0";
            decimal a = Convert.ToDecimal(x);
            return a;
        }
        public static double doubleConvert(string x)
        {
            if (x == "")
                x = "0";
            double a = Convert.ToDouble(x);
            return a;
        }
        public static int intConvert16(string x)
        {
            if (x == "")
                x = "0";
            int a = Convert.ToInt16(x);
            return a;
        }
        public static Int64 intConvert64(string x)
        {
            if (x == "")
                x = "0";
            Int64 a = Convert.ToInt64(x);
            return a;
        }
        internal static void focusWithMaster(Page page, string p)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "script", "$('#ContentPlaceHolder1_" + p + "').focus();", true);
        }
        internal static void focusWithOutMaster(Page page, string p)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "script", "$('#" + p + "').focus();", true);
        }
        public static string timeCalculation(string fr, string to)
        {
            DateTime StartDate = Convert.ToDateTime(fr);
            DateTime EndDate = Convert.ToDateTime(to);
            //String strResult = (EndDate - StartDate).TotalDays.ToString();

            DateTime oldDate;

            DateTime.TryParse(StartDate.ToShortDateString(), out oldDate);
            DateTime currentDate = EndDate;

            TimeSpan difference = currentDate.Subtract(oldDate);

            // This is to convert the timespan to datetime object
            DateTime DateTimeDifferene = DateTime.MinValue + difference;

            // Min value is 01/01/0001
            // subtract our addition or 1 on all components to get the 
            //actual date.

            int InYears = DateTimeDifferene.Year - 1;
            int InMonths = DateTimeDifferene.Month - 1;
            int InDays = DateTimeDifferene.Day - 1;

            return InYears.ToString() + " Years " + InMonths.ToString() + " Months " + InDays.ToString() + " Days";
        }
        public static bool checkParmit(string formLink, string permissionType)
        {
            permissionType = "ASL_ROLE." + permissionType;
            HttpCookie getData = new HttpCookie("UserInfo");//Input data
            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
            string userId = CookiesData["USERID"].ToString();
            string permission = dbFunctions.getData(@"SELECT CASE (" + permissionType + ") WHEN 'A' THEN 'true' else 'false' end As STATS FROM ASL_MENU INNER JOIN " +
            "ASL_ROLE ON ASL_MENU.MODULEID = ASL_ROLE.MODULEID AND ASL_MENU.MENUID = ASL_ROLE.MENUID  WHERE ASL_MENU.FLINK='" + formLink + "' AND ASL_ROLE.USERID='" + userId + "'");
            if (permission == "")
                permission = "false";
            return Convert.ToBoolean(permission);
        }
        public static string encrypt(string clearText)
        {
            try
            {
                byte[] hashBytes = computeHash(clearText + "asl");
                byte[] hashWithSaltBytes = new byte[hashBytes.Length];
                for (int i = 0; i < hashBytes.Length; i++)
                    hashWithSaltBytes[i] = hashBytes[i];
                string hashValue = Convert.ToBase64String(hashWithSaltBytes);

                return hashValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static byte[] computeHash(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            HashAlgorithm hash = new SHA256Managed();
            return hash.ComputeHash(plainTextBytes);
        }
        public static bool emailValidation(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static bool sendMail(string from, string to, string cc, string bcc, string body, string sub, string host, string userName, string pass)
        {
            bool ret = false;
            try
            {
                MailMessage oMail = new MailMessage(new MailAddress(from), new MailAddress(to));
                if (bcc != "")
                    oMail.Bcc.Add(bcc);
                if (cc != "")
                    oMail.Bcc.Add(cc);
                oMail.Body = body;
                oMail.Subject = sub;
                SmtpClient oSmtp = new SmtpClient();
                oSmtp.Host = host;
                oSmtp.Credentials = new NetworkCredential(userName, pass);
                oSmtp.EnableSsl = false;
                oSmtp.Send(oMail);
                ret = true;
            }
            catch { }
            return ret;
        }

        public static bool sendMailPage(string from, string to, string bcc, string cc, string body, string sub, string host, string userName, string pass)
        {
            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
            bool ret = false;
            MailMessage oMail = new MailMessage(new MailAddress(from), new MailAddress(to));
            // Add the alternate body to the message.
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
            oMail.AlternateViews.Add(alternate);
            if (bcc != "")
                oMail.CC.Add(bcc);
            if (cc != "")
                oMail.CC.Add(cc);
            //oMail.Body = body;
            oMail.Subject = sub;
            SmtpClient oSmtp = new SmtpClient();
            oSmtp.Host = host;
            oSmtp.Credentials = new NetworkCredential(userName, pass);
            oSmtp.EnableSsl = false;
            oSmtp.Send(oMail);
            ret = true;
            return ret;
        }
        public static string passwordStrongChecker(string pass)
        {
            string pSword = "true";
            int len = pass.Length;
            Int64 value;
            if (Int64.TryParse(pass, out value))
                pSword = "Password must be a combination of alphanumeric characters of 6-15 character";
            else if (string.IsNullOrEmpty(pass))
                pSword = "Password required !";
            else if (len < 6)
                pSword = "Password must be 6-15 character";
            else if (len >= 6)
            {
                string x = "false", y = "false";
                foreach (char c in pass)
                {
                    if (char.IsDigit(c))
                        x = "true";
                    if (!Int64.TryParse(c.ToString(), out value) && !String.IsNullOrWhiteSpace(c.ToString()))
                        y = "true";
                    if (String.IsNullOrWhiteSpace(c.ToString()))
                    {
                        x = "false"; y = "false";
                        pSword = "Please remove white space !";
                        break;
                    }
                }
                if (x != "true" || y != "true")
                    pSword = "Password must be a combination of alphanumeric characters";

            }
            return pSword;
        }
        public static string trimAll(string txt)
        {
            string result = txt.Trim();
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ");
            return result;
        }
        public static DataTable dataTable(string Script)
        {
            DataTable dt = new DataTable();
            SqlConnection Conn = new SqlConnection(Connection);
            try
            {
                if (Conn.State != ConnectionState.Open) Conn.Open();
                SqlCommand cmd = new SqlCommand(Script, Conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            catch
            {
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            return dt;
        }
        public static void repeaterAdd(Repeater ob, String sql)
        {
            DataTable table = new DataTable();
            SqlConnection Conn = new SqlConnection(Connection);
            try
            {

                if (Conn.State != ConnectionState.Open) Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(table);
                ob.DataSource = table;
                ob.DataBind();
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            catch
            {
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            //return List;
        }
        public static void formViewAdd(FormView ob, String sql)
        {
            DataTable table = new DataTable();
            SqlConnection Conn = new SqlConnection(Connection);
            try
            {

                if (Conn.State != ConnectionState.Open) Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(table);
                ob.DataSource = table;
                ob.DataBind();
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            catch
            {
                if (Conn.State != ConnectionState.Closed) Conn.Close();
            }
            //return List;
        }
        internal static void popupAlert(Page page, string msg, string cls)
        {
            string clas = "";
            cls = cls.ToUpper();
            if (cls == "E")
                clas = "$(\"#headPopup\").removeClass('alertWarning alertSuccess');$(\"#headPopup\").addClass(\"modal-header alertError\");";
            else if (cls == "S")
                clas = "$(\"#headPopup\").removeClass('alertWarning alertSuccess');$(\"#headPopup\").addClass(\"modal-header alertSuccess\");";
            else if (cls == "W")
                clas = "$(\"#headPopup\").removeClass('alertError alertSuccess');$(\"#headPopup\").addClass(\"modal-header alertWarning\");";
            // ScriptManager.RegisterStartupScript(page, page.GetType(), "script", "", true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "script", "$('#messagePopup').text('" + msg + "');$('#exampleModal').modal('show');" + clas + "", true);
        }

        internal static void Redirect(string p, Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "script", "window.open('" + p + "');", true);
        }
        public static bool permit()
        {
            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
            bool x = false;
            string UserNM = encrypt(CookiesData["USERNAME"].ToString());
            string PermitID = CookiesData["PERMITID"].ToString();
            if (UserNM == PermitID)
                x = true;
            return x;
        }
        public static bool FieldCheck(string[] field)
        {
            bool checkResult = false;
            foreach (var data in field)
            {
                if (data == "")
                {
                    checkResult = false;
                    break;
                }
                else
                    checkResult = true;
            }
            return checkResult;
        }
        internal static void ErrorLog(Page Page, Exception ex, string ActionFR)
        {

            SqlConnection con = new SqlConnection(Connection);
            SqlCommand cmd = new SqlCommand("", con);
            string s = "";
            Int64 UserID = 0;
            SqlTransaction tran = null;
            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
            if (CookiesData["USERID"] != null)
                UserID = Convert.ToInt64(CookiesData["USERID"].ToString());
            try
            {
                if (con.State != ConnectionState.Open)
                    if (con.State != ConnectionState.Open) con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO ERRLOG(FORMNM,ERROR,ACTIONFR,USERID,INTIME)
 				Values 
				(@FORMNM,@ERROR,@ACTIONFR,@USERID,@INTIME)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@FORMNM", SqlDbType.NVarChar).Value = Page.ToString();
                cmd.Parameters.Add("@ERROR", SqlDbType.NVarChar).Value = ex.ToString();
                cmd.Parameters.Add("@ACTIONFR", SqlDbType.NVarChar).Value = ActionFR;
                cmd.Parameters.Add("@USERID", SqlDbType.BigInt).Value = UserID;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = timezone(DateTime.Now);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed) con.Close();

            }
            catch (Exception ex1)
            {
                tran.Rollback();
            }
        }

        internal static void ErrorLog(Page page, Exception ex, object sender)
        {
            string ID = "";
            string action = splitText(sender.ToString(), '.', 4);
            if (action == "TextBox")
            {
                TextBox textBox = (TextBox)sender;
                ID = textBox.ID;
            }
            else if (action == "Button")
            {
                Button button = (Button)sender;
                ID = button.ID;
            }
            else if (action == "DropDownList")
            {
                DropDownList dropDownList = (DropDownList)sender;
                ID = dropDownList.ID;
            }
            else if (action == "LinkButton")
            {
                LinkButton linkButton = (LinkButton)sender;
                ID = linkButton.ID;
            }
            else if (action == "ImageButton")
            {
                ImageButton imageButton = (ImageButton)sender;
                ID = imageButton.ID;
            }
            else if (action == "ListBox")
            {
                ListBox lListBox = (ListBox)sender;
                ID = lListBox.ID;
            }
            else if (action == "CheckBox")
            {
                CheckBox checkBox = (CheckBox)sender;
                ID = checkBox.ID;
            }
            else if (action == "RadioButton")
            {
                RadioButton radioButton = (RadioButton)sender;
                ID = radioButton.ID;
            }
            SqlConnection con = new SqlConnection(Connection);
            SqlCommand cmd = new SqlCommand("", con);
            string s = "";
            Int64 UserID = 0;
            SqlTransaction tran = null;
            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
            if (CookiesData["USERID"] != null)
                UserID = Convert.ToInt64(CookiesData["USERID"].ToString());
            try
            {
                if (con.State != ConnectionState.Open)
                    if (con.State != ConnectionState.Open) con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO ERRLOG(FORMNM,ERROR,ACTIONFR,USERID,INTIME)
 				Values 
				(@FORMNM,@ERROR,@ACTIONFR,@USERID,@INTIME)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@FORMNM", SqlDbType.NVarChar).Value = page.ToString();
                cmd.Parameters.Add("@ERROR", SqlDbType.NVarChar).Value = ex.ToString();
                cmd.Parameters.Add("@ACTIONFR", SqlDbType.NVarChar).Value = ID;
                cmd.Parameters.Add("@USERID", SqlDbType.BigInt).Value = UserID;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = timezone(DateTime.Now);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex1)
            {
                tran.Rollback();
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }
        public static void insertLogData(string lotileng, string logid, string tableid, string LogData)
        {
            try
            {
                models iob = new models();
                data_Access dob = new data_Access();
                iob.LotiLengTudeInsert = lotileng;
                iob.IpAddressInsert = dbFunctions.ipAddress();
                iob.UserIdInsert = Convert.ToInt64(HttpContext.Current.Session["USERID"].ToString());
                iob.UserPcInsert = dbFunctions.userPc();
                iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);

                string logSl = dbFunctions.StringData("SELECT ISNULL(MAX(LOGSLNO+1),1) AS MAXLOGSLNO FROM ASL_LOG WHERE USERID= " + iob.UserIdInsert + "");
                iob.LogSlNo = Convert.ToInt64(logSl);
                iob.LogType = logid;
                iob.CompanyId = Convert.ToInt64(HttpContext.Current.Session["COMPANYID"].ToString());
                iob.CompanyUserId = Convert.ToInt64(HttpContext.Current.Session["USERID"].ToString());
                iob.TableId = tableid;
                iob.LogDatA = LogData;
                dob.INSERT_ASL_LOG(iob);
            }
            catch (Exception)
            {

            }
        }
        public static string getDataParemeter(String sql, string value)
        {
            string s = "";
            SqlConnection con = new SqlConnection(Connection);
            try
            { 
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlCommand cmd1 = new SqlCommand(sql, con);
                cmd1.Parameters.Clear();
                cmd1.Parameters.Add("@TXT", SqlDbType.NVarChar).Value = value;
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    s = dr[0].ToString();
                }
                dr.Close();
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            catch
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return s;
        }
        public static void showMessage(Label ob, string msg, string action)
        {
            action = action.ToUpper();
            ob.Visible = true;
            if (action == "W")
            {
                ob.CssClass = "alert alert-warning";
                ob.Text = "<b>Warning </b> " + msg;
            }
            else if (action == "E")
            {
                ob.CssClass = "alert alert-danger";
                ob.Text = "<b>Error </b> " + msg;
            }
            else if (action == "S")
            {
                ob.CssClass = "alert alert-success";
                ob.Text = "<b>Success </b> " + msg;
            }

        }
        public static void showMessage(Label ob, string msg, string action, TextBox fcs)
        {
            action = action.ToUpper();
            ob.Visible = true;
            if (action == "W")
            {
                ob.CssClass = "alert alert-warning";
                ob.Text = "<b>Warning </b> " + msg;
                fcs.Focus();
            }
            else if (action == "E")
            {
                ob.CssClass = "alert alert-danger";
                ob.Text = "<b>Error </b> " + msg;
            }
            else if (action == "S")
            {
                ob.CssClass = "alert alert-success";
                ob.Text = "<b>Success </b> " + msg;
            } 
        }
        public static void showMessage(Label ob, string msg, string action, DropDownList fcs)
        {
            action = action.ToUpper();
            ob.Visible = true;
            if (action == "W")
            {
                ob.CssClass = "alert alert-warning";
                ob.Text = "<b>Warning </b> " + msg;
                fcs.Focus();
            }
            else if (action == "E")
            {
                ob.CssClass = "alert alert-danger";
                ob.Text = "<b>Error </b> " + msg;
            }
            else if (action == "S")
            {
                ob.CssClass = "alert alert-success";
                ob.Text = "<b>Success </b> " + msg;
            }
        }
        /****************************** Spell Amount *****************************/
        public class SpellAmount
        {
            public static string MoneyConvFn(string MVal)
            {
                string TxtMon = MVal;
                string iout = "";

                if (TxtMon.Trim().Length > 21)
                {
                    iout = "Maximum 21 digits only Allowed";
                }
                else
                {
                    iout = NToW(TxtMon.Trim());
                }

                return iout;
            }

            public static string QuantityConvFn(string MVal)
            {
                string TxtMon = MVal;
                string iout = "";

                if (TxtMon.Trim().Length > 21)
                {
                    iout = "Maximum 21 digits only Allowed";
                }
                else
                {
                    iout = NToWQuantity(TxtMon.Trim());
                }

                return iout;
            }

            private static string One(Int64 x)
            {
                switch (x)
                {
                    case 1:
                        return "One";
                    case 2:
                        return "Two";
                    case 3:
                        return "Three";
                    case 4:
                        return "Four";
                    case 5:
                        return "Five";
                    case 6:
                        return "Six";
                    case 7:
                        return "Seven";
                    case 8:
                        return "Eight";
                    case 9:
                        return "Nine";
                    default:
                        return "";

                }
            }

            private static string two(Int64 x, Int64 y)
            {
                switch (x)
                {
                    case 1:
                        switch (y)
                        {
                            case 0:
                                return "Ten";
                            case 1:
                                return "Eleven";
                            case 2:
                                return "Twelve";
                            case 3:
                                return "Thirteen";
                            case 4:
                                return "Fourteen";
                            case 5:
                                return "Fifteen";
                            case 6:
                                return "Sixteen";
                            case 7:
                                return "Seventeen";
                            case 8:
                                return "Eighteen";
                            case 9:
                                return "Nineteen";
                            default:
                                return "";
                        }
                    case 2:
                        return "Twenty ";
                    case 3:
                        return "Thirty ";
                    case 4:
                        return "Forty ";
                    case 5:
                        return "Fifty ";
                    case 6:
                        return "Sixty ";
                    case 7:
                        return "Seventy ";
                    case 8:
                        return "Eighty ";
                    case 9:
                        return "Ninety ";
                    default:
                        return "";
                }
            }

            private static string three(Int64 x)
            {
                if (x != 0)  //to avoid empty hundred for 1000
                {
                    string xx = One(x) + " Hundred ";
                    return xx;
                }
                else
                    return "";
            }

            private static string frfv(Int64 x, Int64 y)
            {
                if (x == 0 && y == 0)
                {
                    return "";
                }
                else
                {
                    if (x != 0)
                    {
                        if (x != 1)
                        {
                            return two(x, y) + One(y) + " Thousand ";
                        }
                        else
                        {
                            return two(x, y) + " Thousand ";
                        }
                    }
                    else
                    {
                        return One(y) + " Thousand ";
                    }
                }
            }

            private static string sxsn(Int64 x, Int64 y)
            {
                if (x == 0 && y == 0)
                {
                    return "";
                }
                else
                {
                    if (x != 0)
                    {
                        if (x != 1)
                        {
                            return two(x, y) + One(y) + " Lacs ";
                        }
                        else
                        {
                            return two(x, y) + " Lacs ";
                        }
                    }
                    else
                    {
                        if (y == 1)
                            return One(y) + " Lac ";
                        else return One(y) + " Lacs ";
                    }
                }
            }

            private static string etnn(Int64 x, Int64 y)
            {
                if (x == 0 && y == 0)
                {
                    return "";
                }
                else
                {
                    if (x != 0)
                    {
                        if (x != 1)
                        {
                            return two(x, y) + One(y) + " Crores ";
                        }
                        else
                        {
                            return two(x, y) + " Crores ";
                        }
                    }
                    else
                    {
                        return One(y) + " Crores ";
                    }
                }
            }

            private static string NToWConv(String aaa)
            {
                Int64[] no = new Int64[10];
                int k;
                string a = "";
                k = aaa.Length;
                for (int j = 0; j < 7; j++)
                {
                    if (k > 0)
                    {
                        no[j] = Convert.ToInt64(aaa.Substring(k - 1, 1));
                    }
                    else
                    {
                        no[j] = 0;
                    }
                    k = k - 1;
                }

                a = a + sxsn(no[6], no[5]);
                a = a + frfv(no[4], no[3]);
                a = a + three(no[2]);
                //if (no[1] != 0 || no[0] != 0)
                //{
                //    a = a + " and ";
                //}
                if (a.Trim().Length > 0)
                {
                    if (no[1] != 0 || no[0] != 0)
                    {
                        a = a + " ";
                    }
                }

                a = a + two(no[1], no[0]);
                if (no[1] != 1)
                {
                    a = a + One(no[0]);
                }
                return a;
            }

            public static string NToW(String ParaNum)
            {
                //*****************************************
                //Int64 can have maximum value as 9,223,372,036,854,775,807.
                if (ParaNum.Length > 21)
                {
                    return "Maximum 21 digits only Allowed";
                }
                Decimal a1 = Convert.ToDecimal(ParaNum);
                Int64 a2 = (Int64)Decimal.Floor(a1);
                Decimal a3 = a1 - a2;
                if (a2.ToString().Length > 21)
                {
                    return "Maximum 21 digits only Allowed";
                }

                Int64 Part1, Part2, Part3, TempV1, TempV2;
                TempV1 = TempV2 = Part1 = Part2 = Part3 = 0;
                Int64 Fr = (Int64)(a3 * 100);
                TempV1 = a2;

                TempV2 = (Int64)Decimal.Floor(TempV1 / 10000000);
                Part1 = TempV1 - TempV2 * 10000000;
                TempV1 = TempV2;

                if (TempV1 >= 10000000)
                {
                    TempV2 = (Int64)Decimal.Floor(TempV1 / 10000000);
                    Part2 = TempV1 - TempV2 * 10000000;
                    TempV1 = TempV2;
                }
                else
                {
                    Part2 = TempV1;
                    TempV1 = 0;
                }

                if (TempV1 >= 10000000)
                {
                    TempV2 = (Int64)Decimal.Floor(TempV1 / 10000000);
                    Part3 = TempV1 - TempV2 * 10000000;
                    TempV1 = TempV2;
                }
                else
                {
                    Part3 = TempV1;
                    TempV1 = 0;
                }
                if (TempV1 >= 10000000)
                {
                    return "Taka Conversion Error: Number exceeds the Length 21 digits";
                }

                string NName = "";
                int ln = NName.Trim().Length;

                if (Part3 > 0)
                {
                    NName = NName.Trim() + NToWConv(Part3.ToString()) + " Crore Crore";
                }
                if (Part2 > 0)
                {
                    if (Part2 == 1)
                    {
                        NName = NName.Trim() + "" + NToWConv(Part2.ToString()) + " Crore";
                    }
                    else
                    {
                        NName = NName.Trim() + "" + NToWConv(Part2.ToString()) + " Crores";
                    }
                }
                if (Part1 > 0)
                {
                    NName = NName.Trim() + "" + NToWConv(Part1.ToString());
                }
                if (NName.Trim().Length > 0)
                {
                    NName = "" + NName; // In quotation marks "Taka"/ for Rasput Traders Withdraw it.
                }
                if (Fr > 0)
                {
                    if (NName.Trim().Length > 0)
                    {
                        NName = NName.Trim() + " And " + NToWConv(Fr.ToString()) + " Paisa";
                    }
                    else
                    {
                        NName = NToWConv(Fr.ToString()) + " Paisa";
                    }
                }
                if (NName.Trim().Length > 0)
                {
                    NName = NName + " Only";
                }
                return NName;
            }

            public static string NToWQuantity(String ParaNum)
            {
                //*****************************************
                //Int64 can have maximum value as 9,223,372,036,854,775,807.
                if (ParaNum.Length > 21)
                {
                    return "Maximum 21 digits only Allowed";
                }
                Decimal a1 = Convert.ToDecimal(ParaNum);
                Int64 a2 = (Int64)Decimal.Floor(a1);
                Decimal a3 = a1 - a2;
                if (a2.ToString().Length > 21)
                {
                    return "Maximum 21 digits only Allowed";
                }

                Int64 Part1, Part2, Part3, TempV1, TempV2;
                TempV1 = TempV2 = Part1 = Part2 = Part3 = 0;
                Int64 Fr = (Int64)(a3 * 100);
                TempV1 = a2;

                TempV2 = (Int64)Decimal.Floor(TempV1 / 10000000);
                Part1 = TempV1 - TempV2 * 10000000;
                TempV1 = TempV2;

                if (TempV1 >= 10000000)
                {
                    TempV2 = (Int64)Decimal.Floor(TempV1 / 10000000);
                    Part2 = TempV1 - TempV2 * 10000000;
                    TempV1 = TempV2;
                }
                else
                {
                    Part2 = TempV1;
                    TempV1 = 0;
                }

                if (TempV1 >= 10000000)
                {
                    TempV2 = (Int64)Decimal.Floor(TempV1 / 10000000);
                    Part3 = TempV1 - TempV2 * 10000000;
                    TempV1 = TempV2;
                }
                else
                {
                    Part3 = TempV1;
                    TempV1 = 0;
                }
                if (TempV1 >= 10000000)
                {
                    return "Taka Conversion Error: Number exceeds the Length 21 digits";
                }

                string NName = "";
                int ln = NName.Trim().Length;

                if (Part3 > 0)
                {
                    NName = NName.Trim() + NToWConv(Part3.ToString()) + " Crore Crore";
                }
                if (Part2 > 0)
                {
                    if (Part2 == 1)
                    {
                        NName = NName.Trim() + "" + NToWConv(Part2.ToString()) + " Crore";
                    }
                    else
                    {
                        NName = NName.Trim() + "" + NToWConv(Part2.ToString()) + " Crores";
                    }
                }
                if (Part1 > 0)
                {
                    NName = NName.Trim() + "" + NToWConv(Part1.ToString());
                }
                if (NName.Trim().Length > 0)
                {
                    NName = "Quantity " + NName;
                }
                if (Fr > 0)
                {
                    if (NName.Trim().Length > 0)
                    {
                        NName = NName.Trim() + " And " + NToWConv(Fr.ToString()) + " Paisa";
                    }
                    else
                    {
                        NName = NToWConv(Fr.ToString()) + " Paisa";
                    }
                }
                if (NName.Trim().Length > 0)
                {
                    NName = NName + "";
                }
                return NName;
            }

            public static String comma(decimal amount)
            {
                string result = "";
                string amt = "";
                string amt_paisa = "";
                string minusCheck = "";

                string amnt = amount.ToString();
                minusCheck = amnt.Substring(0, 1);
                if (minusCheck == "-")
                {
                    decimal Famount = (amount * -1);

                    amt = Famount.ToString();
                    int aaa = Famount.ToString().IndexOf(".", 0);
                    amt_paisa = Famount.ToString().Substring(aaa + 1);

                    if (amt == amt_paisa)
                    {
                        amt_paisa = "";
                    }
                    else
                    {
                        amt = Famount.ToString().Substring(0, Famount.ToString().IndexOf(".", 0));
                        amt = (amt.Replace(",", "")).ToString();
                    }
                    switch (amt.Length)
                    {
                        case 15:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 2) + "," + amt.Substring(12, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 2) + "," + amt.Substring(12, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 14:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," + amt.Substring(7, 2) + "," +
                                         amt.Substring(9, 2) + "," + amt.Substring(11, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," + amt.Substring(7, 2) + "," +
                                         amt.Substring(9, 2) + "," + amt.Substring(11, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 13:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 12:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," +
                                         amt.Substring(7, 2) + "," + amt.Substring(9, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," +
                                         amt.Substring(7, 2) + "," + amt.Substring(9, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 11:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," +
                                         amt.Substring(6, 2) + "," + amt.Substring(8, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," +
                                         amt.Substring(6, 2) + "," + amt.Substring(8, 3) + "." +
                                         amt_paisa;
                            }
                            break;

                        case 10:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," +
                                         amt.Substring(5, 2) + "," + amt.Substring(7, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," +
                                         amt.Substring(5, 2) + "," + amt.Substring(7, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 9:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 2) + "," + amt.Substring(6, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 2) + "," + amt.Substring(6, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 8:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 2) + "," + amt.Substring(5, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 2) + "," + amt.Substring(5, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 7:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 3) + "." + amt_paisa;
                            }
                            break;
                        case 6:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 3) + "." + amt_paisa;
                            }
                            break;
                        case 5:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 2) + "," + amt.Substring(2, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 4:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 3);
                            }
                            else
                            {
                                result = "-" + amt.Substring(0, 1) + "," + amt.Substring(1, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        default:
                            if (amt_paisa == "")
                            {
                                result = "-" + amt;
                            }
                            else
                            {
                                result = "-" + amt + "." + amt_paisa;
                            }
                            break;
                    }
                }
                else
                {
                    amt = amount.ToString();
                    int aaa = amount.ToString().IndexOf(".", 0);
                    amt_paisa = amount.ToString().Substring(aaa + 1);

                    if (amt == amt_paisa)
                    {
                        amt_paisa = "";
                    }
                    else
                    {
                        amt = amount.ToString().Substring(0, amount.ToString().IndexOf(".", 0));
                        amt = (amt.Replace(",", "")).ToString();
                    }
                    switch (amt.Length)
                    {
                        case 15:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 2) + "," + amt.Substring(12, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 2) + "," + amt.Substring(12, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 14:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," + amt.Substring(7, 2) + "," +
                                         amt.Substring(9, 2) + "," + amt.Substring(11, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," + amt.Substring(7, 2) + "," +
                                         amt.Substring(9, 2) + "," + amt.Substring(11, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 13:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," + amt.Substring(6, 2) + "," +
                                         amt.Substring(8, 2) + "," + amt.Substring(10, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 12:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," +
                                         amt.Substring(7, 2) + "," + amt.Substring(9, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," + amt.Substring(5, 2) + "," +
                                         amt.Substring(7, 2) + "," + amt.Substring(9, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 11:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," +
                                         amt.Substring(6, 2) + "," + amt.Substring(8, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + amt.Substring(4, 2) + "," +
                                         amt.Substring(6, 2) + "," + amt.Substring(8, 3) + "." +
                                         amt_paisa;
                            }
                            break;

                        case 10:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," +
                                         amt.Substring(5, 2) + "," + amt.Substring(7, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + amt.Substring(3, 2) + "," +
                                         amt.Substring(5, 2) + "," + amt.Substring(7, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 9:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 2) + "," + amt.Substring(6, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 2) + "," + amt.Substring(6, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 8:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 2) + "," + amt.Substring(5, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 2) + "," + amt.Substring(5, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 7:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," +
                                         amt.Substring(4, 3) + "." + amt_paisa;
                            }
                            break;
                        case 6:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," +
                                         amt.Substring(3, 3) + "." + amt_paisa;
                            }
                            break;
                        case 5:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 2) + "," + amt.Substring(2, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        case 4:
                            if (amt_paisa == "")
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 3);
                            }
                            else
                            {
                                result = amt.Substring(0, 1) + "," + amt.Substring(1, 3) + "." +
                                         amt_paisa;
                            }
                            break;
                        default:
                            if (amt_paisa == "")
                            {
                                result = amt;
                            }
                            else
                            {
                                result = amt + "." + amt_paisa;
                            }
                            break;
                    }
                    //return result;
                }
                return result;
            }

        }
    }
}