using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using alchemySoft;

namespace DynamicMenu
{
    /// <summary>
    /// Summary description for search
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class search : System.Web.Services.WebService
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        [WebMethod(EnableSession = true)]
        public string checkSession(string Session)
        {
            string sess = HttpContext.Current.Session["" + Session + ""].ToString();
            return sess;
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListCompany(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();

            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT COMPNM AS txt FROM ASL_COMPANY WHERE COMPNM LIKE @SearchText +'%' ORDER BY COMPNM", conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListModuleName(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();

            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT top 15 MODULENM AS txt FROM ASL_MENUMST WHERE MODULENM LIKE @SearchText +'%' ORDER BY MODULENM", conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListMenuName(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string moduleId = "";
            if (HttpContext.Current.Session["ModuleId"] != null)
                moduleId = HttpContext.Current.Session["ModuleId"].ToString();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT MENUNM AS txt FROM ASL_MENU WHERE MODULEID='" + moduleId + "' AND MENUNM LIKE  @SearchText +'%' ORDER BY MENUNM", conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListMenuNameByType(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string moduleId = "";
            string menuType = "";
            if (HttpContext.Current.Session["ModuleId"] != null && HttpContext.Current.Session["MenuType"] != null)
            {
                moduleId = HttpContext.Current.Session["ModuleId"].ToString();
                menuType = HttpContext.Current.Session["MenuType"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT MENUNM AS txt FROM ASL_MENU WHERE MODULEID='" + moduleId + "' AND  MENUTP='" + menuType + "' AND MENUNM LIKE  @SearchText +'%' ORDER BY MENUNM", conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }


        
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionUserNameForMenuRole_List(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();

            string query = "";
            string usrid = "";
            string usertype = "";
            //string menuType = "";
            if (HttpContext.Current.Session["COMPANYID"] != null)
            {
                usrid = HttpContext.Current.Session["COMPANYID"].ToString();
                usertype = HttpContext.Current.Session["USERTYPE"].ToString();
                //menuType = HttpContext.Current.Session["MenuType"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();

            if (usertype == "SUPERADMIN")
            {
                query = "SELECT DISTINCT ASL_USERCO.USERNM AS txt FROM ASL_USERCO " +
                        "INNER JOIN ASL_ROLE ON ASL_USERCO.COMPID = ASL_ROLE.COMPID AND ASL_USERCO.USERID = ASL_ROLE.USERID " +
                        "WHERE  ASL_USERCO.USERNM LIKE @SearchText +'%' ORDER BY txt";
            }
            else
            {
                query = "SELECT DISTINCT ASL_USERCO.USERNM AS txt FROM ASL_USERCO " +
                        "INNER JOIN ASL_ROLE ON ASL_USERCO.COMPID = ASL_ROLE.COMPID AND ASL_USERCO.USERID = ASL_ROLE.USERID " +
                        "WHERE ASL_USERCO.COMPID='" + usrid + "' AND ASL_USERCO.USERID!='10101' AND ASL_USERCO.USERNM LIKE @SearchText +'%' ORDER BY txt";
            }


            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }


        //Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End
        //Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End
        //Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End
        //Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End//Accounts End


        /*--------------------------------------------------------------------------------------------------------------*/

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListOpeningBalanceEntryAccountNM(string term)
        {
            List<string> s = new List<string>();
            List<TowValue> lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT ACCOUNTNM AS txt, ACCOUNTCD AS txtid FROM GL_ACCHART WHERE STATUSCD='P' and ACCOUNTNM LIKE  @SearchText +'%'", conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", term);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["txt"].ToString().TrimEnd();
                    string id = obj_result["txtid"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                return (List<TowValue>)q;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListUserNameForMenuRole(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string usrid = "";
            string menuType = "";
            if (CookiesData["COMPANYID"] != null)
            {
                usrid = CookiesData["COMPANYID"].ToString();
                //menuType = HttpContext.Current.Session["MenuType"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT DISTINCT ASL_USERCO.USERNM AS txt FROM ASL_USERCO INNER JOIN
ASL_ROLE ON ASL_USERCO.COMPID = ASL_ROLE.COMPID AND ASL_USERCO.USERID = ASL_ROLE.USERID 
WHERE ASL_USERCO.COMPID='" + usrid + "' AND ASL_USERCO.USERID!='" + usrid + "01' AND ASL_USERCO.USERNM LIKE @SearchText +'%' ORDER BY txt", conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            return result;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListCostPoolSingleVEdit(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            // HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 

            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            query = "GetCompletionListCostPool LIKE  @SearchText +'%'";
            //else
            //    query = "SELECT (GL_COSTP.COSTPNM + '|' + GL_COSTPMST.CATNM) AS txt FROM GL_COSTP INNER JOIN GL_COSTPMST ON GL_COSTP.CATID = GL_COSTPMST.CATID WHERE (GL_COSTP.COSTPNM + ' - ' + GL_COSTPMST.CATNM) LIKE  @SearchText +'%' AND GL_COSTP.CATID ='" + brCD + "'";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListMrecD(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            // if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%'";
            //else
            //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListMrecC(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            // if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            //else
            // query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListMpayD(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            // else
            //   query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListMpayC(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            //  else
            //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'  AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListJourD(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            // if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            // else
            //   query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListJourC(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            // if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%'";
            // else
            // query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListConD(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            // if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%'";
            // else
            // query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListConC(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%'";
            // else
            //   query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListDebit(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            string Transtype = HttpContext.Current.Session["Transtype"].ToString();
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            if (Transtype == "MREC")
            {
                // if (uTp == "COMPADMIN")
                //  {
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //  }
                // else
                //  {
                //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                // }
            }

            else if (Transtype == "MPAY")
            {
                //if (uTp == "COMPADMIN")
                //{
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //}
                //else
                //{
                //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                //}
            }
            else if (Transtype == "JOUR")
            {
                //if (uTp == "COMPADMIN")
                //{
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //}
                //else
                //{
                //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                //}
            }
            else if (Transtype == "CONT")
            {
                //if (uTp == "COMPADMIN")
                //{
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //}
                //else
                //{
                //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                //}
            }
            else
            {
                Transtype = "";
            }
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListCredit(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            string Transtype = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null && HttpContext.Current.Session["Transtype"] != "")
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
                Transtype = HttpContext.Current.Session["Transtype"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            if (Transtype == "MREC")
            {
                //if (uTp == "COMPADMIN")
                //{
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //}
                //else
                //{
                //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                //}
            }

            else if (Transtype == "MPAY")
            {
                //if (uTp == "COMPADMIN")
                //{
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //    }
                //    else
                //    {
                //        query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                //    }
            }
            else if (Transtype == "JOUR")
            {
                //if (uTp == "COMPADMIN")
                //{
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //}
                //else
                //{
                //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                //}
            }
            else if (Transtype == "CONT")
            {
                //if (uTp == "COMPADMIN")
                //{
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
                //}
                //else
                //{
                //    query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                //}
            }
            else
            {
                Transtype = "";
            }
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListCreditSingleVoucherEdit(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            string Transtype = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null && HttpContext.Current.Session["Transtype"] != "")
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
                Transtype = HttpContext.Current.Session["Transtype"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            if (Transtype == "MREC")
            {
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }

            else if (Transtype == "MPAY")
            {
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE   @SearchText +'%'";
            }
            else if (Transtype == "JOUR")
            {
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            else if (Transtype == "CONT")
            {
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            else
            {
                Transtype = "";
            }
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListDebitSingleVoucherEdit(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            string Transtype = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null && HttpContext.Current.Session["Transtype"] != "")
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
                Transtype = HttpContext.Current.Session["Transtype"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            if (Transtype == "MREC")
            {
                query = "SELECT ACCOUNTNM AS txt  FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }

            else if (Transtype == "MPAY")
            {
                query = "SELECT ACCOUNTNM AS txt  FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE   @SearchText +'%'";
            }
            else if (Transtype == "JOUR")
            {
                query = "SELECT ACCOUNTNM AS txt  FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            else if (Transtype == "CONT")
            {
                query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%'";
            }
            else
            {
                Transtype = "";
            }
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListCreditSingleVoucherNew(string txt, string transtype)
        {
            var query = "";
            List<TowValue> lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (transtype == "MREC")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            else if (transtype == "MPAY")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE   @SearchText +'%'";
            }
            else if (transtype == "JOUR")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            else if (transtype == "CONT")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            using (SqlCommand objSqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    string result = objResult["ACCOUNTNM"].ToString().TrimEnd();
                    string id = objResult["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                return (List<TowValue>)q;
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListtDebitSingleVoucherNew(string txt, string transtype)
        {
            var query = "";
            List<TowValue> lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (transtype == "MREC")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD  FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            else if (transtype == "MPAY")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD  FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE   @SearchText +'%'";
            }
            else if (transtype == "JOUR")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD  FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE  @SearchText +'%'";
            }
            else if (transtype == "CONT")
            {
                query = "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE @SearchText +'%'";
            }
            using (SqlCommand objSqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    string result = objResult["ACCOUNTNM"].ToString().TrimEnd();
                    string id = objResult["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                return (List<TowValue>)q;
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListLavelCode(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string lavelCd = "";
            if (HttpContext.Current.Session["LAVELCD"] != null)
                lavelCd = HttpContext.Current.Session["LAVELCD"].ToString();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT ACCOUNTNM+'| (L-'+convert(nvarchar,LEVELCD,103)+')'  AS txt FROM GL_ACCHART WHERE ACCOUNTCD like '" + lavelCd + "' and LEVELCD between 1 and 4 and ACCOUNTNM like  @SearchText +'%'", conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListBankBook(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //  if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) =('1020102') and STATUSCD='P' and ACCOUNTNM LIKE  @SearchText +'%'";
            // else
            //   query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) =('1020102') and STATUSCD='P' and ACCOUNTNM LIKE  @SearchText +'%'  AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListCashBook(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            // if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM As txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) =('1020101') and STATUSCD='P' and ACCOUNTNM LIKE  @SearchText +'%'";
            //  else
            //  query = "SELECT ACCOUNTNM As txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) =('1020101') and STATUSCD='P' and ACCOUNTNM LIKE   @SearchText +'%'  AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListLedgerBook(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //   if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102') and STATUSCD='P' and ACCOUNTNM LIKE  @SearchText +'%'";
            //   else
            //   query = "SELECT ACCOUNTNM  AS txt FROM GL_ACCHART  AS txt WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102') and STATUSCD='P' and ACCOUNTNM LIKE @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListLedgerBookDepoDelear(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //   if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020201','1020202') and STATUSCD='P' and ACCOUNTNM LIKE  @SearchText +'%'";
            //   else
            //   query = "SELECT ACCOUNTNM  AS txt FROM GL_ACCHART  AS txt WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102') and STATUSCD='P' and ACCOUNTNM LIKE @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListLedgerBookGeneral(string term)
        {
            List<string> s = new List<string>();
            List<TowValue> lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string BrCD = CookiesData["BrCDNM"].ToString();
            string USERTYPE = CookiesData["USERTYPE"].ToString();
            string script = "";
            if (USERTYPE == "USER")
                script = " AND (ISNULL(ASL_BRANCH.BRANCHNM,'" + BrCD + "') LIKE '" + BrCD + "%')";

            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT        dbo.GL_ACCHART.ACCOUNTNM AS txt, dbo.GL_ACCHART.ACCOUNTCD AS txtid
FROM            dbo.GL_ACCHART LEFT OUTER JOIN
                         dbo.ASL_BRANCH ON dbo.GL_ACCHART.BRANCHCD = dbo.ASL_BRANCH.BRANCHCD 
WHERE        (GL_ACCHART.STATUSCD = 'P') " + script + " AND  GL_ACCHART.ACCOUNTNM LIKE  @SearchText +'%'", conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", term);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["txt"].ToString().TrimEnd();
                    string id = obj_result["txtid"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return (List<TowValue>)q;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListMoneyreceipts(string term)
        {
            List<string> s = new List<string>();
            List<TowValue> lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT ACCOUNTNM AS txt, ACCOUNTCD AS txtid FROM GL_ACCHART WHERE LEVELCD='4' AND SUBSTRING(ACCOUNTCD,1,5) = '10202' AND ACCOUNTNM LIKE  @SearchText +'%'", conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", term);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["txt"].ToString().TrimEnd();
                    string id = obj_result["txtid"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                return (List<TowValue>)q;
            }
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListNotesAccount(string txt, string lableCode)
        {
            List<TowValue> lst = new List<TowValue>();
            var query = "";
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (lableCode == "1")
            {
                query = "SELECT (ACCOUNTNM+' (L - '+convert(nvarchar(10),LEVELCD,103)+')') as txt, ACCOUNTCD  FROM GL_ACCHART WHERE ACCOUNTCD like '1%' and LEVELCD between 1 and 4 and ACCOUNTNM like @SearchText +'%'";
            }

            else if (lableCode == "2")
            {
                query = "SELECT (ACCOUNTNM+' (L - '+convert(nvarchar(10),LEVELCD,103)+')') as txt, ACCOUNTCD  FROM GL_ACCHART WHERE ACCOUNTCD like '2%' and LEVELCD between 1 and 4  and ACCOUNTNM like @SearchText +'%'";
            }
            else if (lableCode == "3")
            {
                query = "SELECT (ACCOUNTNM+' (L - '+convert(nvarchar(10),LEVELCD,103)+')') as txt, ACCOUNTCD  FROM GL_ACCHART WHERE ACCOUNTCD like '3%'  and LEVELCD between 1 and 4  and ACCOUNTNM like @SearchText +'%'";
            }
            else if (lableCode == "4")
            {
                query = "SELECT (ACCOUNTNM+' (L - '+convert(nvarchar(10),LEVELCD,103)+')') as txt, ACCOUNTCD  FROM GL_ACCHART WHERE ACCOUNTCD like '4%'  and LEVELCD between 1 and 4  and ACCOUNTNM like @SearchText +'%'";
            }
            else
            {
                lableCode = "";
            }
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["txt"].ToString().TrimEnd();
                    string id = obj_result["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return (List<TowValue>)q;
            }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListCostPool(string txt)
        {
            List<TowValue> lst = new List<TowValue>();
            string query = "";
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            query = "SELECT COSTPNM,COSTPID FROM GL_COSTP WHERE COSTPNM LIKE @SearchText +'%'";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["COSTPNM"].ToString().TrimEnd();
                    string id = obj_result["COSTPID"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return (List<TowValue>)q;
            }
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListReceiptStatementSelected(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string query = "";
            if (CookiesData["USERTYPE"] != null && CookiesData["BrCD"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand();
            //  if (uTp == "COMPADMIN")
            query = "SELECT ACCOUNTNM As txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102') and STATUSCD='P' and ACCOUNTNM LIKE   @SearchText +'%'";
            // else
            //    query = "SELECT ACCOUNTNM As txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102') and STATUSCD='P' and ACCOUNTNM LIKE  @SearchText +'%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListItemMasterCategoryName(string txt)
        {
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            // your code to query the database goes here
            try
            {
                
                
                using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT DISTINCT(CATNM) AS txt FROM STK_ITEMMST WHERE CATNM LIKE  @SearchText +'%'", conn))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                    SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                    while (obj_result.Read())
                    {
                        result.Add(obj_result["txt"].ToString().TrimEnd());
                    }
                    if (conn.State != ConnectionState.Closed) conn.Close();
                }
            }catch
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
            }
            return result;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListParty(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10202','20202') AND ACCOUNTNM LIKE  @SearchText +'%' AND STATUSCD = 'P'", conn))
            {
                conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListSuppliar(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand("SELECT ACCOUNTNM AS txt FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) in ('20202') and ACCOUNTNM LIKE  @SearchText +'%' AND STATUSCD = 'P'", conn))
            {
                conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    result.Add(obj_result["txt"].ToString().TrimEnd());
                }
            }
            return result;
        }

    }

    public class PartyInformation
    {
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string WebId { get; set; }
        public string Apname { get; set; }
        public string ApNo { get; set; }
        public string Status { get; set; }
        public string LoginID { get; set; }
        public string LoginPw { get; set; }
    }
    public class ItemNameIdRate
    {
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public string SateRate { get; set; }
    }
    public class JobReceive
    {

        public string JobNoAndTp { get; set; }
        public string PartyId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string AccountName { get; set; }


    }
    public class PartySupplierIdAddress
    {
        public string PartySupplierName { get; set; }
        public string PartySupplierId { get; set; }
        public string PartySupplierAddress { get; set; }
    }
    public class TowValue
    {
        string _value;
        string _label;

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }
        public string label
        {
            get { return _label; }
            set { _label = value; }
        }

    }
    public class TowValue2
    {
        public string Value { get; set; }

        public string Label { get; set; }
    }
}
