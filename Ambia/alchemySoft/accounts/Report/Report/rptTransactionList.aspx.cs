using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

using System.Data.SqlClient;
using System.Web.Services;
using System.Text;
using System.Collections.Specialized;
using System.Threading;

namespace alchemySoft.accounts.Report.Report
{
    public partial class rptTransactionList : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        // To keep track of the previous row Group Identifier    
        string strPreviousRowID = string.Empty;

        decimal dblGrandTotalAmount_MREC = 0;
        decimal dblGrandTotalAmount_MPAY = 0;
        decimal dblGrandTotalAmount_JOUR = 0;
        decimal dblGrandTotalAmount_CONT = 0;

        string dblGrandTotalAmountComma_MREC = "";
        string dblGrandTotalAmountComma_MPAY = "";
        string dblGrandTotalAmountComma_JOUR = "";
        string dblGrandTotalAmountComma_CONT = "";

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
            dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

            lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblFrom.Text = FDate.ToString("dd-MMM-yyyy");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblTo.Text = TDate.ToString("dd-MMM-yyyy");
            showGrid_Credit();
            showGrid_Debit();
            showGrid_Journal();
            showGrid_Contra();
        }

        public void showGrid_Credit()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            //string uTp = CookiesData["USERTYPE"].ToString();
            //string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(@"SELECT     GL_MASTER.TRANSTP, (CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) 
                      + (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT'
                       THEN 'CV ' ELSE 'APV ' END) WHEN GL_MASTER.TABLEID = 'STK_TRANS' THEN 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MRV ' END) 
                      + '- ' + + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, CONVERT(nvarchar(20), GL_MASTER.TRANSDT, 105) AS TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, 
                      GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, 
                      GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR, ISNULL(GL_COSTP.COSTPNM,'&nbsp;') AS COSTPNM
FROM         GL_MASTER INNER JOIN
                      GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN
                      GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD INNER JOIN
                      GL_COSTP ON GL_MASTER.COSTPID = GL_COSTP.COSTPID
WHERE     (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS', 'GL_MTRANS')) AND (GL_MASTER.TRANSDRCR = 'DEBIT') AND  " +
"                      (GL_MASTER.TRANSTP = 'MREC') " +
"ORDER BY TRANSDT, GL_MASTER.TRANSTP DESC, GL_MASTER.TRANSNO", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT GL_MASTER.TRANSTP,(CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) " +
            //                                  " + (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT' THEN 'CV ' ELSE " +
            //                                  " 'APV ' END) " +
            //                                  " WHEN GL_MASTER.TABLEID = 'STK_TRANS' then 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MRV ' END)+'- '+  " +
            //                                  " + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, convert(nvarchar(20),GL_MASTER.TRANSDT,105) as TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, " +
            //                      " GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, " +
            //                      " GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR " +
            //                      " FROM GL_MASTER INNER JOIN " +
            //                      " GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN " +
            //                      " GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD " +
            //                      " WHERE (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS','GL_MTRANS')) AND GL_MASTER.TRANSDRCR='DEBIT' and GL_MASTER.TRANSTP = 'MREC' AND (SUBSTRING(GL_MASTER.COSTPID, 1,3)='" + brCD + "')" +
            //                      " ORDER BY TRANSDT, TRANSTP DESC,TRANSNO ASC", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblCreditHD.Text = "CREDIT VOUCHER";
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                lblCreditHD.Visible = false;
                GridView1.Visible = false;
            }
        }

        public void showGrid_Debit()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(@" SELECT     GL_MASTER.TRANSTP, (CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) 
                      + (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT'
                       THEN 'CV ' ELSE 'APV ' END) WHEN GL_MASTER.TABLEID = 'STK_TRANS' THEN 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MPV ' END) 
                      + '- ' + + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, CONVERT(nvarchar(20), GL_MASTER.TRANSDT, 105) AS TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, 
                      GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, 
                      GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR, ISNULL(GL_COSTP.COSTPNM,'&nbsp;') AS COSTPNM
FROM         GL_MASTER INNER JOIN
                      GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN
                      GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD INNER JOIN
                      GL_COSTP ON GL_MASTER.COSTPID = GL_COSTP.COSTPID
WHERE     (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS', 'LC_EXPENSE', 'GL_MTRANS')) AND (GL_MASTER.TRANSDRCR = 'DEBIT') AND " +
"                      (GL_MASTER.TRANSTP = 'MPAY') " +
"ORDER BY TRANSDT, GL_MASTER.TRANSTP DESC, GL_MASTER.TRANSNO", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT GL_MASTER.TRANSTP,(CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) " +
            //                                  " + (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT' THEN 'CV ' ELSE " +
            //                                  " 'APV ' END) " +
            //                                  " WHEN GL_MASTER.TABLEID = 'STK_TRANS' then 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MPV ' END)+'- '+  " +
            //                                  " + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, convert(nvarchar(20),GL_MASTER.TRANSDT,105) as TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, " +
            //                      " GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, " +
            //                      " GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR " +
            //                      " FROM GL_MASTER INNER JOIN " +
            //                      " GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN " +
            //                      " GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD " +
            //                      " WHERE (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS','LC_EXPENSE','GL_MTRANS')) AND GL_MASTER.TRANSDRCR='DEBIT' and GL_MASTER.TRANSTP = 'MPAY' AND (SUBSTRING(GL_MASTER.COSTPID, 1,3)='" + brCD + "')" +
            //                      " ORDER BY TRANSDT, TRANSTP DESC,TRANSNO ASC", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblDebitHD.Text = "DEBIT VOUCHER";
                GridView2.DataSource = ds;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            else
            {
                lblDebitHD.Visible = false;
                GridView2.Visible = false;
            }
        }

        public void showGrid_Journal()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(@"SELECT     GL_MASTER.TRANSTP, (CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) 
                      + (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT'
                       THEN 'CV ' ELSE 'APV ' END) WHEN GL_MASTER.TABLEID = 'STK_TRANS' THEN 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MJV ' END) 
                      + '- ' + + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, CONVERT(nvarchar(20), GL_MASTER.TRANSDT, 105) AS TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, 
                      GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, 
                      GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR, ISNULL(GL_COSTP.COSTPNM,'&nbsp;') AS COSTPNM
FROM         GL_MASTER INNER JOIN
                      GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN
                      GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD left outer JOIN
                      GL_COSTP ON GL_MASTER.COSTPID = GL_COSTP.COSTPID
WHERE     (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS', 'STK_TRANS', 'GL_MTRANS')) AND (GL_MASTER.TRANSDRCR = 'DEBIT') AND " +
" (GL_MASTER.TRANSTP = 'JOUR') " +
" ORDER BY TRANSDT, GL_MASTER.TRANSTP DESC, GL_MASTER.TRANSNO", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT GL_MASTER.TRANSTP,(CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) " +
            //                                 " + (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT' THEN 'CV ' ELSE " +
            //                                 " 'APV ' END) " +
            //                                 " WHEN GL_MASTER.TABLEID = 'STK_TRANS' then 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MJV ' END)+'- '+  " +
            //                                 " + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, convert(nvarchar(20),GL_MASTER.TRANSDT,105) as TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, " +
            //                     " GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, " +
            //                     " GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR " +
            //                     " FROM GL_MASTER INNER JOIN " +
            //                     " GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN " +
            //                     " GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD " +
            //                     " WHERE (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS','STK_TRANS','GL_MTRANS')) AND GL_MASTER.TRANSDRCR='DEBIT' and GL_MASTER.TRANSTP = 'JOUR' AND (SUBSTRING(GL_MASTER.COSTPID, 1,3)='" + brCD + "')" +
            //                     " ORDER BY TRANSDT, TRANSTP DESC,TRANSNO ASC", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblJournalHD.Text = "JOURNAL VOUCHER";
                GridView3.DataSource = ds;
                GridView3.DataBind();
                GridView3.Visible = true;
            }
            else
            {
                lblJournalHD.Visible = false;
                GridView3.Visible = false;
            }
        }

        public void showGrid_Contra()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(@" SELECT GL_MASTER.TRANSTP, (CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) 
+ (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT'
THEN 'CV ' ELSE 'APV ' END) WHEN GL_MASTER.TABLEID = 'STK_TRANS' THEN 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MCV ' END) 
+ '- ' + + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, CONVERT(nvarchar(20), GL_MASTER.TRANSDT, 105) AS TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, 
GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, 
GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR, ISNULL(GL_COSTP.COSTPNM,'&nbsp;') AS COSTPNM
FROM GL_MASTER INNER JOIN
GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN
GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD INNER JOIN
GL_COSTP ON GL_MASTER.COSTPID = GL_COSTP.COSTPID
WHERE (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS', 'GL_MTRANS')) AND (GL_MASTER.TRANSDRCR = 'DEBIT') AND " +
"(GL_MASTER.TRANSTP = 'CONT') ORDER BY TRANSDT, GL_MASTER.TRANSTP DESC, GL_MASTER.TRANSNO", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT GL_MASTER.TRANSTP,(CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) " +
            //                                                   " + (CASE WHEN GL_MASTER.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER.TRANSTP = 'CONT' THEN 'CV ' ELSE " +
            //                                                   " 'APV ' END) " +
            //                                                   " WHEN GL_MASTER.TABLEID = 'STK_TRANS' then 'AJV ' WHEN GL_MASTER.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'MCV ' END)+'- '+  " +
            //                                                   " + CONVERT(nvarchar(10), GL_MASTER.TRANSNO, 103) AS DOCNO, convert(nvarchar(20),GL_MASTER.TRANSDT,105) as TRANSDT, GL_MASTER.TRANSNO, GL_MASTER.DEBITCD, GL_MASTER.CREDITCD, GL_MASTER.DEBITAMT, " +
            //                                       " GL_MASTER.CHEQUENO, GL_MASTER.CHEQUEDT, GL_MASTER.REMARKS, GL_MASTER.TABLEID, GL_ACCHART.ACCOUNTNM AS DebitHead, " +
            //                                       " GL_ACCHART_1.ACCOUNTNM AS CreditHead, GL_MASTER.TRANSMODE, GL_MASTER.TRANSDRCR " +
            //                                       " FROM GL_MASTER INNER JOIN " +
            //                                       " GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN " +
            //                                       " GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD " +
            //                                       " WHERE (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (GL_MASTER.TABLEID IN ('GL_STRANS','GL_MTRANS')) AND GL_MASTER.TRANSDRCR='DEBIT' and GL_MASTER.TRANSTP = 'CONT' AND (SUBSTRING(GL_MASTER.COSTPID, 1,3)='" + brCD + "')" +
            //                                       " ORDER BY TRANSDT, TRANSTP DESC,TRANSNO ASC", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblContraHD.Text = "CONTRA VOUCHER";
                GridView4.DataSource = ds;
                GridView4.DataBind();
                GridView4.Visible = true;
            }
            else
            {
                lblContraHD.Visible = false;
                GridView4.Visible = false;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString();


                string TrDT = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                //DateTime TDate = DateTime.Parse(TrDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                //string TransDT = TDate.ToString("dd-MMM-yyyy");
                e.Row.Cells[0].Text = TrDT;

                string DNo = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = DNo;
                string COSTPNM = DataBinder.Eval(e.Row.DataItem, "COSTPNM").ToString();
                e.Row.Cells[2].Text = COSTPNM;

                string DebitHead = DataBinder.Eval(e.Row.DataItem, "DebitHead").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + DebitHead;

                string CreditHead = DataBinder.Eval(e.Row.DataItem, "CreditHead").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + CreditHead;

                decimal DEBITAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());
                string Amnt = dbFunctions.SpellAmount.comma(DEBITAMT);
                e.Row.Cells[5].Text = Amnt + "&nbsp;";

                decimal dblAmount_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());

                dblGrandTotalAmount_MREC += dblAmount_MREC;
                dblGrandTotalAmountComma_MREC = dbFunctions.SpellAmount.comma(dblGrandTotalAmount_MREC);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                int RowNo = GridView1.Rows.Count;
                e.Row.Cells[3].Text = "Total Number of Voucher :   " + RowNo;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].Text = "Total :";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].Text = dblGrandTotalAmountComma_MREC;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            ShowHeader(GridView1);
        }

        private void ShowHeader(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString();


                string TrDT = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                //DateTime TDate = DateTime.Parse(TrDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                //string TransDT = TDate.ToString("dd-MMM-yyyy");
                e.Row.Cells[0].Text = TrDT;

                string DNo = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = DNo;
                string COSTPNM = DataBinder.Eval(e.Row.DataItem, "COSTPNM").ToString();
                e.Row.Cells[2].Text = COSTPNM;
                string DebitHead = DataBinder.Eval(e.Row.DataItem, "DebitHead").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + DebitHead;

                string CreditHead = DataBinder.Eval(e.Row.DataItem, "CreditHead").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + CreditHead;

                decimal DEBITAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());
                string Amnt = dbFunctions.SpellAmount.comma(DEBITAMT);
                e.Row.Cells[5].Text = Amnt + "&nbsp;";

                decimal dblAmount_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());

                dblGrandTotalAmount_MPAY += dblAmount_MPAY;
                dblGrandTotalAmountComma_MPAY = dbFunctions.SpellAmount.comma(dblGrandTotalAmount_MPAY);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                int RowNo = GridView2.Rows.Count;
                e.Row.Cells[3].Text = "Total Number of Voucher :   " + RowNo;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].Text = "Total :";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].Text = dblGrandTotalAmountComma_MPAY;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }
            ShowHeader2(GridView2);
        }

        private void ShowHeader2(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString();


                string TrDT = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                //DateTime TDate = DateTime.Parse(TrDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                //string TransDT = TDate.ToString("dd-MMM-yyyy");
                e.Row.Cells[0].Text = TrDT;

                string DNo = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = DNo;
                string COSTPNM = DataBinder.Eval(e.Row.DataItem, "COSTPNM").ToString();
                e.Row.Cells[2].Text = COSTPNM;
                string DebitHead = DataBinder.Eval(e.Row.DataItem, "DebitHead").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + DebitHead;
                string CreditHead = DataBinder.Eval(e.Row.DataItem, "CreditHead").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + CreditHead;

                decimal DEBITAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());
                string Amnt = dbFunctions.SpellAmount.comma(DEBITAMT);
                e.Row.Cells[5].Text = Amnt + "&nbsp;";

                decimal dblAmount_JOUR = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());

                dblGrandTotalAmount_JOUR += dblAmount_JOUR;
                dblGrandTotalAmountComma_JOUR = dbFunctions.SpellAmount.comma(dblGrandTotalAmount_JOUR);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                int RowNo = GridView3.Rows.Count;
                e.Row.Cells[3].Text = "Total Number of Voucher :   " + RowNo;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].Text = "Total :";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].Text = dblGrandTotalAmountComma_JOUR;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            ShowHeader3(GridView3);
        }

        private void ShowHeader3(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString();


                string TrDT = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                //DateTime TDate = DateTime.Parse(TrDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                //string TransDT = TDate.ToString("dd-MMM-yyyy");
                e.Row.Cells[0].Text = TrDT;

                string DNo = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = DNo;
                string COSTPNM = DataBinder.Eval(e.Row.DataItem, "COSTPNM").ToString();
                e.Row.Cells[2].Text = COSTPNM;
                string DebitHead = DataBinder.Eval(e.Row.DataItem, "DebitHead").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + DebitHead;

                string CreditHead = DataBinder.Eval(e.Row.DataItem, "CreditHead").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + CreditHead;

                decimal DEBITAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());
                string Amnt = dbFunctions.SpellAmount.comma(DEBITAMT);
                e.Row.Cells[5].Text = Amnt + "&nbsp;";

                decimal dblAmount_CONT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());

                dblGrandTotalAmount_CONT += dblAmount_CONT;
                dblGrandTotalAmountComma_CONT = dbFunctions.SpellAmount.comma(dblGrandTotalAmount_CONT);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                int RowNo = GridView4.Rows.Count;
                e.Row.Cells[3].Text = "Total Number of Voucher :   " + RowNo;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].Text = "Total :";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].Text = dblGrandTotalAmountComma_CONT;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            ShowHeader4(GridView4);
        }

        private void ShowHeader4(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

    }
}