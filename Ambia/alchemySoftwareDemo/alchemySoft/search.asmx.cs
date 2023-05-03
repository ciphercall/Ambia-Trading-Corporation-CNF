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
        public List<string> GetCompletionListUserNameForMenuRole(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string usrid = "";
            string menuType = "";
            if (HttpContext.Current.Session["COMPANYID"] != null)
            {
                usrid = HttpContext.Current.Session["COMPANYID"].ToString();
                //menuType = HttpContext.Current.Session["MenuType"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT DISTINCT ASL_USERCO.USERNM AS txt FROM ASL_USERCO INNER JOIN
                                        ASL_ROLE ON ASL_USERCO.COMPID = ASL_ROLE.COMPID AND ASL_USERCO.USERID = ASL_ROLE.USERID 
                                        WHERE ASL_USERCO.COMPID='" + usrid + "' AND ASL_USERCO.USERNM LIKE @SearchText +'%' ORDER BY txt", conn))
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



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListCountryName(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();

            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT COUNTRYNM AS txt FROM GEO_COUNTRY WHERE COUNTRYNM LIKE @SearchText +'%' ", conn))
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
        public List<string> GetCompletionListDivisionName(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string countryCd = "";
            if (HttpContext.Current.Session["COUNTRYCD"] != null)
                countryCd = HttpContext.Current.Session["COUNTRYCD"].ToString();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT DIVISIONNM AS txt FROM GEO_DIVISION WHERE COUNTRYCD='" + countryCd + "' AND DIVISIONNM LIKE @SearchText +'%' ", conn))
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
        public List<string> GetCompletionListZoneName(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string countryCd = "";
            string countryDivision = "";
            if (HttpContext.Current.Session["COUNTRYCD"] != null)
            {
                countryCd = HttpContext.Current.Session["COUNTRYCD"].ToString();
                countryDivision = HttpContext.Current.Session["DIVISIONCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT ZONENM AS txt FROM GEO_ZONE WHERE COUNTRYCD='" + countryCd + "' AND DIVISIONCD='" + countryDivision + "' AND ZONENM LIKE @SearchText +'%' ", conn))
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

        public static string Slipt(string text, int position, char sumbol)
        {
            string s = "";
            string searchPar = text;
            int splitter = searchPar.IndexOf(sumbol);
            if (splitter != -1)
            {
                string[] lineSplit = searchPar.Split(sumbol);
                s = lineSplit[position];
            }
            return s;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListCompanyNameWithBrancId(string term)
        {
            // your code to query the database goes here
            List<TowValue> lst = new List<TowValue>();
            string uTp = "";
            string brCD = "";
            string query;

            if (HttpContext.Current.Session["COMPANYID"] != null)
            {
                brCD = HttpContext.Current.Session["COMPANYID"].ToString();
                uTp = HttpContext.Current.Session["USERTYPE"].ToString();
                //menuType = HttpContext.Current.Session["MenuType"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (uTp == "COMPADMIN")
                query = @"SELECT (BRANCHID + '-' + COMPNM) AS COMPNM, ASL_BRANCH.BRANCHCD AS COMPID FROM ASL_BRANCH INNER JOIN ASL_COMPANY ON ASL_COMPANY.COMPID=ASL_BRANCH.COMPID WHERE /*COMPID ='' AND*/ (BRANCHID + '-' + COMPNM) LIKE @SearchText +'%'";
            else
                query = @"SELECT (BRANCHID + '-' + COMPNM) AS COMPNM, ASL_BRANCH.BRANCHCD AS COMPID FROM ASL_BRANCH INNER JOIN ASL_COMPANY ON ASL_COMPANY.COMPID=ASL_BRANCH.COMPID  WHERE /*COMPID ='" + brCD + "' AND*/ (BRANCHID + '-' + COMPNM) LIKE @SearchText +'%'";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", term);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["COMPNM"].ToString().TrimEnd();
                    string id = obj_result["COMPID"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return (List<TowValue>)q;
            }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListConsigName(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT DISTINCT CONSIGNEENM as txt FROM CNF_JOB WHERE CONSIGNEENM LIKE @SearchText +'%' ", conn))
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
        public List<string> GetCompletionListSupplier(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(@"SELECT DISTINCT SUPPLIERNM as txt FROM CNF_JOB WHERE SUPPLIERNM LIKE @SearchText +'%' ", conn))
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
        public List<TowValue> GetCompletionListPartyName(string term, string uTp, string brCD)
        {
            List<TowValue> lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string query;
            if (uTp == "COMPADMIN")
                query = "SELECT ACCOUNTNM, ACCOUNTCD   FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10202') and ACCOUNTNM LIKE @SearchText +'%' AND STATUSCD = 'P'";
            else
                query = "SELECT ACCOUNTNM, ACCOUNTCD  FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10202') and ACCOUNTNM LIKE @SearchText +'%' AND STATUSCD = 'P' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", term);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["ACCOUNTNM"].ToString().TrimEnd();
                    string id = obj_result["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return (List<TowValue>)q;
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return null;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<JobReceive> GetCompletionListJobNoYearType(string term, string uTp)
        {
            List<JobReceive> lst = new List<JobReceive>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string query;
            string utp = CookiesData["USERTYPE"].ToString();
            string CmpID = CookiesData["COMPANYID"].ToString();
            if (utp == "COMPADMIN")
            {
                query = @"SELECT TOP 100 CONVERT(NVARCHAR(20), CNF_JOB.JOBNO, 103) + '|' + CONVERT(NVARCHAR(20), CNF_JOB.JOBYY, 103) + '|' + CNF_JOB.JOBTP AS JOBPAR,CNF_JOB.PARTYID,CNF_JOB.COMPID,ASL_BRANCH.BRANCHID+'-'+ASL_BRANCH.BRANCHNM AS COMPNM, 
                            GL_ACCHART.ACCOUNTNM FROM CNF_JOB
                            INNER JOIN ASL_BRANCH ON CNF_JOB.COMPID=ASL_BRANCH.BRANCHCD
                            INNER JOIN GL_ACCHART ON CNF_JOB.PARTYID=GL_ACCHART.ACCOUNTCD
                            WHERE (CONVERT(NVARCHAR(20), CNF_JOB.JOBNO, 103) + '|' + CONVERT(NVARCHAR(20), CNF_JOB.JOBYY, 103) + '|' + CNF_JOB.JOBTP 
                            LIKE @SearchText  +'%') ORDER BY CNF_JOB.JOBNO,CNF_JOB.JOBYY DESC";
            }
            else
            {
                query = @"SELECT TOP 100 CONVERT(NVARCHAR(20), CNF_JOB.JOBNO, 103) + '|' + CONVERT(NVARCHAR(20), CNF_JOB.JOBYY, 103) + '|' + CNF_JOB.JOBTP AS JOBPAR,CNF_JOB.PARTYID,CNF_JOB.COMPID,ASL_BRANCH.BRANCHID+'-'+ASL_BRANCH.BRANCHNM AS COMPNM, 
                            GL_ACCHART.ACCOUNTNM FROM CNF_JOB
                            INNER JOIN ASL_BRANCH ON CNF_JOB.COMPID=ASL_BRANCH.BRANCHCD
                            INNER JOIN GL_ACCHART ON CNF_JOB.PARTYID=GL_ACCHART.ACCOUNTCD
                            WHERE (CONVERT(NVARCHAR(20), CNF_JOB.JOBNO, 103) + '|' + CONVERT(NVARCHAR(20), CNF_JOB.JOBYY, 103) + '|' + CNF_JOB.JOBTP 
                            LIKE @SearchText +'%' AND CNF_JOB.COMPID LIKE '" + CmpID + "%') ORDER BY CNF_JOB.JOBNO,CNF_JOB.JOBYY DESC";
            }
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", term);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string jobno = obj_result["JOBPAR"].ToString().TrimEnd();
                    string partyid = obj_result["PARTYID"].ToString().TrimEnd();
                    string compid = obj_result["COMPID"].ToString().TrimEnd();
                    string compnm = obj_result["COMPNM"].ToString().TrimEnd();
                    string accountnm = obj_result["ACCOUNTNM"].ToString().TrimEnd();

                    lst.Add(new JobReceive
                    {
                        JobNoAndTp = jobno,
                        PartyId = partyid,
                        CompanyId = compid,
                        CompanyName = compnm,
                        AccountName = accountnm
                    });
                }
                var q = lst.ToList();
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                return (List<JobReceive>)q;
            }
            return null;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListCash_Bank(string term, string uTp, string brCD, string rcvTp)
        {
            List<TowValue> lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string query;
            string utp = CookiesData["USERTYPE"].ToString();
            string CmpID = CookiesData["COMPANYID"].ToString();
            if (rcvTp == "Discount")
            {
                query = @"SELECT ACCOUNTNM  ,ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) IN ('4010105')  and ACCOUNTNM LIKE @SearchText +'%' AND STATUSCD = 'P'";
            }
            else
            {
                if (utp == "COMPADMIN")
                {
                    query = @"SELECT ACCOUNTNM  ,ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10201','20202') and ACCOUNTNM LIKE @SearchText +'%' AND STATUSCD = 'P'";
                }
                else
                {
                    query = @"SELECT ACCOUNTNM ,ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10201','20202') and ACCOUNTNM LIKE @SearchText +'%' AND STATUSCD = 'P' AND (CAST (BRANCHCD AS NVARCHAR) = '" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')";
                }
            }
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", term);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["ACCOUNTNM"].ToString().TrimEnd();
                    string id = obj_result["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                return (List<TowValue>)q;
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListPartyNameJobExpense(string txt)
        {
            // your code to query the database goes here
            List<TowValue> lst = new List<TowValue>();
            string uTp = "";
            string brCD = "";
            string query;
            if (HttpContext.Current.Session["USERTYPE"] != null && HttpContext.Current.Session["BrCD"] != null)
            {
                uTp = HttpContext.Current.Session["USERTYPE"].ToString();
                brCD = HttpContext.Current.Session["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (uTp == "COMPADMIN")
                query = "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10201','10203','20202','20201','20203') and ACCOUNTNM LIKE @SearchText +'%' AND STATUSCD = 'P'";
            else
                query = "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10201','10203','20202','20201','20203') and ACCOUNTNM LIKE @SearchText +'%' AND STATUSCD = 'P' /*AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')*/";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string result = obj_result["ACCOUNTNM"].ToString().TrimEnd();
                    string id = obj_result["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                return (List<TowValue>)q;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionRemarksJobExpns(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand objSqlcommand = new SqlCommand(
                @"SELECT DISTINCT ISNULL(REMARKS,'') REMARKS  FROM CNF_JOBEXP where REMARKS LIKE @SearchText +'%'", conn))
            {
                conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    result.Add(objResult["REMARKS"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListJobYear(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            string uTp = "";
            string brCD = "";
            string trmode = "";
            string query;
            if (CookiesData["USERTYPE"] != null && CookiesData["COMPANYID"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["COMPANYID"].ToString();
                // trmode = HttpContext.Current.Session["TRMODE"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            //if (trmode == "ALL")
            //{
            //if (uTp == "COMPADMIN")
            //{
            //    query = ("SELECT (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) AS JOBPAR FROM CNF_JOB WHERE (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) LIKE @SearchText +'%' ORDER BY JOBNO");
            //}
            //else
            //{
            //    query = ("SELECT (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) AS JOBPAR FROM CNF_JOB WHERE (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) LIKE @SearchText +'%' /*AND COMPID ='" + brCD + "'*/ ORDER BY JOBNO");
            //}
            ////}
            ////else if (trmode == "C&F")
            ////{
            //if (uTp == "COMPADMIN")
            //{
            //    query = ("SELECT (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) AS JOBPAR FROM CNF_JOB WHERE (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) LIKE @SearchText +'%' AND JOBTP IN ('EXPORT','IMPORT') ORDER BY JOBNO");
            //}
            //else
            //{
            //    query = ("SELECT (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) AS JOBPAR FROM CNF_JOB WHERE (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) LIKE @SearchText +'%' /*AND COMPID ='" + brCD + "'*/ AND JOBTP IN ('EXPORT','IMPORT') ORDER BY JOBNO");
            //}
            ////}
            ////else
            ////{
            if (uTp == "COMPADMIN")
            {
                query = ("SELECT TOP 100 (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) AS JOBPAR FROM CNF_JOB WHERE (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) LIKE @SearchText +'%' ORDER BY JOBNO");
            }
            else
            {
                query = ("SELECT TOP 100 (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) AS JOBPAR FROM CNF_JOB WHERE (CONVERT(NVARCHAR(20),JOBNO,103) + '|' + CONVERT(NVARCHAR(20),JOBYY,103) + '|' + JOBTP + '|' + COMPID) LIKE @SearchText +'%' /*AND COMPID ='" + brCD + "'*/ ORDER BY JOBNO");
            }
            //}
            using (SqlCommand objSqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    result.Add(objResult["JOBPAR"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListExpensePerticulars(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand objSqlcommand = new SqlCommand("SELECT EXPNM FROM CNF_EXPENSE WHERE EXPNM LIKE @SearchText +'%' ORDER BY EXPNM", conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    result.Add(objResult["EXPNM"].ToString().TrimEnd());
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            return result;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListExpensePerticularsWithExpId(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand objSqlcommand = new SqlCommand("SELECT (EXPNM + '|' +  EXPID) AS EXPNM FROM CNF_EXPENSE WHERE (EXPNM + '|' + EXPID) LIKE @SearchText +'%' ORDER BY EXPNM", conn))
            {
                conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    result.Add(objResult["EXPNM"].ToString().TrimEnd());
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetCompletionListExpensePerticularHead(string txt)
        {
            // your code to query the database goes here
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (SqlCommand objSqlcommand = new SqlCommand("SELECT EXPCNM FROM CNF_EXPMST WHERE EXPCNM LIKE @SearchText +'%' ORDER BY EXPCNM", conn))
            {
                conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    result.Add(objResult["EXPCNM"].ToString().TrimEnd());
                }
            }
            return result;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListPartyNameWithId(string txt)
        {
            var lst = new List<TowValue>();
            string uTp = ""; string brCD = ""; string query;
            if (CookiesData["USERTYPE"] != null && CookiesData["COMPANYID"] != null)
            {
                uTp = HttpContext.Current.Session["USERTYPE"].ToString();
                brCD = HttpContext.Current.Session["BrCD"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            query = uTp == "COMPADMIN" ? "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10202') AND ACCOUNTNM LIKE @SearchText +'%'  AND STATUSCD = 'P'" :
                "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10202') AND ACCOUNTNM LIKE @SearchText +'%'  AND STATUSCD = 'P'  /*AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')*/";

            using (var objSqlcommand = new SqlCommand(query, conn))
            {
                conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    string result = objResult["ACCOUNTNM"].ToString().TrimEnd();
                    string id = objResult["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return q;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListExpenseName(string txt)
        {
            var lst = new List<TowValue>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            using (var objSqlcommand = new SqlCommand(@"SELECT  EXPNM, EXPID FROM CNF_EXPENSE WHERE EXPNM LIKE @SearchText +'%'", conn))
            {
                conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    string result = objResult["EXPNM"].ToString().TrimEnd();
                    string id = objResult["EXPID"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return q;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<PartyInformation> GetPartyAllInformation(string accountcd)
        {
            List<PartyInformation> lst = new List<PartyInformation>();
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string query = @"SELECT  ADDRESS, CONTACTNO, EMAILID, WEBID, APNM, APNO, STATUS,LOGINID,LOGINPW
            FROM CNF_PARTY WHERE PARTYID='" + accountcd + "'";
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string address = obj_result["ADDRESS"].ToString().TrimEnd();
                    string contactno = obj_result["CONTACTNO"].ToString().TrimEnd();
                    string email = obj_result["EMAILID"].ToString().TrimEnd();
                    string webid = obj_result["WEBID"].ToString().TrimEnd();
                    string apnm = obj_result["APNM"].ToString().TrimEnd();
                    string apno = obj_result["APNO"].ToString().TrimEnd();
                    string status = obj_result["STATUS"].ToString().TrimEnd();
                    string loginid = obj_result["LOGINID"].ToString().TrimEnd();
                    string loginpw = obj_result["LOGINPW"].ToString().TrimEnd();

                    lst.Add(new PartyInformation { Address = address, ContactNo = contactno, EmailId = email, WebId = webid, Apname = apnm, ApNo = apno, Status = status, LoginID = loginid, LoginPw = loginpw });
                }
                var q = lst.ToList();
                return (List<PartyInformation>)q;
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TowValue> GetCompletionListAccountHeadWithId(string txt)
        {
            var lst = new List<TowValue>();
            string uTp = ""; string brCD = ""; string query;
            if (CookiesData["USERTYPE"] != null && CookiesData["COMPANYID"] != null)
            {
                uTp = CookiesData["USERTYPE"].ToString();
                brCD = CookiesData["COMPANYID"].ToString();
            }
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            query = uTp == "COMPADMIN" ? "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) IN ('1020101', '1020301') AND ACCOUNTNM LIKE @SearchText +'%'  AND STATUSCD = 'P'" :
                "SELECT ACCOUNTNM, ACCOUNTCD FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) IN ('1020101', '1020301') AND ACCOUNTNM LIKE @SearchText +'%'  AND STATUSCD = 'P'  /*AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')*/";

            using (var objSqlcommand = new SqlCommand(query, conn))
            {
                conn.Open();
                objSqlcommand.Parameters.AddWithValue("@SearchText", txt);
                SqlDataReader objResult = objSqlcommand.ExecuteReader();
                while (objResult.Read())
                {
                    string result = objResult["ACCOUNTNM"].ToString().TrimEnd();
                    string id = objResult["ACCOUNTCD"].ToString().TrimEnd();
                    lst.Add(new TowValue { value = id, label = result });
                }
                var q = lst.ToList();
                return q;
            }
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
