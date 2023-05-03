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
using System.Globalization;

namespace alchemySoft.accounts.Report.Report
{
    public partial class rptRecPayDet : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        // To keep track of the previous row Group Identifier    
        string strPreviousRowID_OP = string.Empty;
        string strPreviousRowID_MREC = string.Empty;
        string strPreviousRowID_MPAY = string.Empty;
        string strPreviousRowID_CL = string.Empty;
        // To temporarily store Sub Total    
        decimal dblSubTotalDRAMT_OP = 0;
        decimal dblSubTotalDRAMT_MREC = 0;
        decimal dblSubTotalDRAMT_MPAY = 0;
        decimal dblSubTotalDRAMT_CL = 0;
        decimal dblSubTotalCRAMT_OP = 0;
        decimal dblSubTotalCRAMT_MREC = 0;
        decimal dblSubTotalCRAMT_MPAY = 0;
        decimal dblSubTotalCRAMT_CL = 0;
        // To temporarily store Grand Total    
        decimal dblGrandTotalDRAMT_OP = 0;
        decimal dblGrandTotalDRAMT_MREC = 0;
        decimal dblGrandTotalDRAMT_MPAY = 0;
        decimal dblGrandTotalDRAMT_CONT = 0;
        decimal dblGrandTotalDRAMT_CL = 0;
        decimal dblGrandTotalCRAMT_OP = 0;
        decimal dblGrandTotalCRAMT_MREC = 0;
        decimal dblGrandTotalCRAMT_MPAY = 0;
        decimal dblGrandTotalCRAMT_CONT = 0;
        decimal dblGrandTotalCRAMT_CL = 0;
        decimal dblGrandTotalAMT_JR = 0;
        //string AmountComma = "";
        string dblSubTotalDRAMTComma_OP = "";
        string dblSubTotalDRAMTComma_MREC = "";
        string dblSubTotalDRAMTComma_MPAY = "";
        //string dblSubTotalDRAMTComma_CONT = "";
        string dblSubTotalDRAMTComma_CL = "";
        string dblSubTotalCRAMTComma_OP = "";
        string dblSubTotalCRAMTComma_MREC = "";
        string dblSubTotalCRAMTComma_MPAY = "";
        string dblSubTotalCRAMTComma_CL = "";


        string dblGrandTotalDRAMTComma_OP = "0";
        string dblGrandTotalDRAMTComma_MREC = "0";
        string dblGrandTotalDRAMTComma_MPAY = "0";
        string dblGrandTotalDRAMTComma_CONT = "0";
        string dblGrandTotalDRAMTComma_CL = "0";
        string dblGrandTotalCRAMTComma_OP = "0";
        string dblGrandTotalCRAMTComma_MREC = "0";
        string dblGrandTotalCRAMTComma_MPAY = "0";
        string dblGrandTotalCRAMTComma_CONT = "0";
        string dblGrandTotalCRAMTComma_CL = "0";
        string dblGrandTotalAMTComma_JR = "0";

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
                dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

                lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

                string From = Session["From"].ToString();
                string To = Session["To"].ToString();

                DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                lblFDate.Text = FDate.ToString("dd-MMM-yyyy");

                DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                lblTDate.Text = TDate.ToString("dd-MMM-yyyy");

                showGrid_OP();
                showGrid_MREC();
                showGrid_MPAY();
                showGrid_CONT();
                showGrid_CL();
                showGrid_Jour();

                decimal G_Total_OP = 0;
                decimal G_Total_MREC = 0;
                decimal G_Total_MPAY = 0;
                decimal G_Total_CL = 0;
                decimal G_Total_DR = 0;

                G_Total_OP = Convert.ToDecimal(dblGrandTotalDRAMTComma_OP);
                G_Total_MREC = Convert.ToDecimal(dblGrandTotalDRAMTComma_MREC);
                G_Total_MPAY = Convert.ToDecimal(dblGrandTotalDRAMTComma_MPAY);
                G_Total_CL = Convert.ToDecimal(dblGrandTotalDRAMTComma_CL);
                G_Total_DR = G_Total_OP + G_Total_MREC + G_Total_MPAY + G_Total_CL;
                string G_DR = dbFunctions.SpellAmount.comma(G_Total_DR);
                lblGrDrAmt.Text = G_DR;

                decimal G_Total_CR_OP = 0;
                decimal G_Total_CR_MREC = 0;
                decimal G_Total_CR_MPAY = 0;
                decimal G_Total_CR_CL = 0;
                decimal G_Total_CR = 0;

                G_Total_CR_OP = Convert.ToDecimal(dblGrandTotalCRAMTComma_OP);
                G_Total_CR_MREC = Convert.ToDecimal(dblGrandTotalCRAMTComma_MREC);
                G_Total_CR_MPAY = Convert.ToDecimal(dblGrandTotalCRAMTComma_MPAY);
                G_Total_CR_CL = Convert.ToDecimal(dblGrandTotalCRAMTComma_CL);
                G_Total_CR = G_Total_CR_OP + G_Total_CR_MREC + G_Total_CR_MPAY + G_Total_CR_CL;
                string G_CR = dbFunctions.SpellAmount.comma(G_Total_CR);
                lblGrCrAmt.Text = G_CR;
            }
            catch (Exception ex)
            {
                dbFunctions.showMessage(Page,ex.Message);
            }
        }

        public void showGrid_OP()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");
            string FDT = FDate.ToString("yyyy-MM-dd");
            DateTime FTest = DateTime.Parse(FDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");
            string TDT = TDate.ToString("yyyy-MM-dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
                                          " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
                                          " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
                                          " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
                                          " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
                                          " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
                                              " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
                                              " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') " +
                                              " GROUP BY DEBITCD " +
                                              " UNION " +
                                              " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
                                              " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
                                              " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
                                              " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
                                              " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
                                              " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
                                              " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
                                              " (TRANSDRCR = 'DEBIT') " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
                                              " '' AS REMARKS, '' AS TABLEID " +
                                              " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') GROUP BY DEBITCD) AS A " +
                    " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
                    " WHERE (A.SL = 1) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
            //                                  " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
            //                                  " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
            //                                  " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
            //                                  " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
            //                                  " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
            //                                      " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
            //                                      " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') " +
            //                                      " GROUP BY DEBITCD " +
            //                                      " UNION " +
            //                                      " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
            //                                      " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
            //                                      " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') " +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
            //                                      " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') " +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
            //                                      " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
            //                                      " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
            //                                      " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
            //                                      " (TRANSDRCR = 'DEBIT') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
            //                                      " '' AS REMARKS, '' AS TABLEID " +
            //                                      " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') GROUP BY DEBITCD) AS A " +
            //            " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
            //            " WHERE (A.SL = 1) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblOpening.Text = "OPENING BALANCE";
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                lblOpening.Visible = false;
                GridView1.Visible = false;
            }
        }

        public void showGrid_MREC()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");
            string FDT = FDate.ToString("yyyy-MM-dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");
            string TDT = TDate.ToString("yyyy-MM-dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
                                          " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
                                          " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
                                          " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
                                          " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
                                          " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
                                              " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
                                              " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') " +
                                              " GROUP BY DEBITCD " +
                                              " UNION " +
                                              " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
                                              " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
                                              " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
                                              " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
                                              " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
                                              " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
                                              " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
                                              " (TRANSDRCR = 'DEBIT') " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
                                              " '' AS REMARKS, '' AS TABLEID " +
                                              " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') GROUP BY DEBITCD) AS A " +
                    " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
                    " WHERE (A.SL = 2) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
            //                                  " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
            //                                  " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
            //                                  " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
            //                                  " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
            //                                  " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
            //                                      " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
            //                                      " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') " +
            //                                      " GROUP BY DEBITCD " +
            //                                      " UNION " +
            //                                      " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
            //                                      " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
            //                                      " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) " +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
            //                                      " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') " +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
            //                                      " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
            //                                      " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
            //                                      " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
            //                                      " (TRANSDRCR = 'DEBIT') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
            //                                      " '' AS REMARKS, '' AS TABLEID " +
            //                                      " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') GROUP BY DEBITCD) AS A " +
            //            " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
            //            " WHERE (A.SL = 2) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblReceive.Text = "RECEIVED DURING THE PERIOD";
                GridView2.DataSource = ds;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            else
            {
                lblReceive.Visible = false;
                GridView2.Visible = false;
            }
        }

        public void showGrid_MPAY()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");
            string FDT = FDate.ToString("yyyy-MM-dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");
            string TDT = TDate.ToString("yyyy-MM-dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
                                          " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
                                          " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
                                          " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
                                          " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
                                          " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
                                              " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
                                              " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') " +
                                              " GROUP BY DEBITCD " +
                                              " UNION " +
                                              " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
                                              " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
                                              " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
                                              " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
                                              " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
                                              " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
                                              " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
                                              " (TRANSDRCR = 'DEBIT') " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
                                              " '' AS REMARKS, '' AS TABLEID " +
                                              " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') GROUP BY DEBITCD) AS A " +
                    " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
                    " WHERE (A.SL = 3) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
            //                                                  " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
            //                                                  " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
            //                                                  " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
            //                                                  " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
            //                                                  " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
            //                                                      " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
            //                                                      " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                                      " GROUP BY DEBITCD " +
            //                                                      " UNION " +
            //                                                      " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
            //                                                      " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
            //                                                      " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                                      " UNION " +
            //                                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
            //                                                      " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                                      " UNION " +
            //                                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
            //                                                      " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
            //                                                      " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
            //                                                      " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
            //                                                      " (TRANSDRCR = 'DEBIT') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                                      " UNION " +
            //                                                      " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
            //                                                      " '' AS REMARKS, '' AS TABLEID " +
            //                                                      " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') GROUP BY DEBITCD) AS A " +
            //                            " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
            //                            " WHERE (A.SL = 3) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblPayment.Text = "PAYMENT DURING THE PERIOD";
                GridView3.DataSource = ds;
                GridView3.DataBind();
                GridView3.Visible = true;
            }
            else
            {
                lblPayment.Visible = false;
                GridView3.Visible = false;
            }
        }

        public void showGrid_CONT()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");
            string FDT = FDate.ToString("yyyy-MM-dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");
            string TDT = TDate.ToString("yyyy-MM-dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
                                          " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
                                          " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
                                          " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
                                          " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
                                          " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
                                              " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
                                              " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') " +
                                              " GROUP BY DEBITCD " +
                                              " UNION " +
                                              " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
                                              " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
                                              " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
                                              " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
                                              " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
                                              " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
                                              " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
                                              " (TRANSDRCR = 'DEBIT') " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
                                              " '' AS REMARKS, '' AS TABLEID " +
                                              " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') GROUP BY DEBITCD) AS A " +
                    " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
                    " WHERE (A.SL = 4) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
            //                                  " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
            //                                  " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
            //                                  " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
            //                                  " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
            //                                  " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
            //                                      " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
            //                                      " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD " +
            //                                      " UNION " +
            //                                      " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
            //                                      " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
            //                                      " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
            //                                      " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
            //                                      " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
            //                                      " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
            //                                      " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
            //                                      " (TRANSDRCR = 'DEBIT') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
            //                                      " '' AS REMARKS, '' AS TABLEID " +
            //                                      " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') GROUP BY DEBITCD) AS A " +
            //            " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
            //            " WHERE (A.SL = 4) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblTransfer.Text = "TRANSFER DURING THE PERIOD";
                GridView6.DataSource = ds;
                GridView6.DataBind();
                GridView6.Visible = true;
            }
            else
            {
                lblTransfer.Visible = false;
                GridView6.Visible = false;
            }
        }

        public void showGrid_CL()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");
            string FDT = FDate.ToString("yyyy-MM-dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");
            string TDT = TDate.ToString("yyyy-MM-dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
                                          " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
                                          " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
                                          " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
                                          " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
                                          " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
                                              " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
                                              " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') " +
                                              " GROUP BY DEBITCD " +
                                              " UNION " +
                                              " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
                                              " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
                                              " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
                                              " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
                                              " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
                                              " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
                                              " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
                                              " (TRANSDRCR = 'DEBIT') " +
                                              " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
                                              " UNION " +
                                              " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
                                              " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
                                              " '' AS REMARKS, '' AS TABLEID " +
                                              " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') GROUP BY DEBITCD) AS A " +
                    " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
                    " WHERE (A.SL = 5) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT A.TRANSDT, A.TRANSNO, (CASE WHEN TABLEID = 'GL_STRANS' THEN SUBSTRING(TABLEID, 4, 1) WHEN TABLEID = 'GL_MTRANS' THEN SUBSTRING(TABLEID, 4, 1) " +
            //                                  " WHEN TABLEID = '' THEN '' ELSE 'A' END) " +
            //                                  " + (CASE WHEN TRANSTP = 'OPEN' THEN '' WHEN TRANSTP = 'MREC' THEN 'RV-' WHEN TRANSTP = 'MPAY' THEN 'PV-' WHEN TRANSTP = 'CONT' THEN 'CV-' WHEN TRANSTP " +
            //                                  " = 'CLOSE' THEN '' ELSE 'APV-' END) + (CASE WHEN TRANSTP = 'OPEN' THEN '0' WHEN TRANSTP = 'MREC' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) " +
            //                                  " WHEN TRANSTP = 'MPAY' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) WHEN TRANSTP = 'CONT' THEN CONVERT(NVARCHAR(20), TRANSNO, 103) ELSE '0' END) " +
            //                                  " AS DOCNO, A.TRANSTP, A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.CRCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2, A.REMARKS, A.TD, GL_ACCHART_2.ACCOUNTNM AS CREDITNM " +
            //                                      " FROM (SELECT '" + From + "' AS TRANSDT, 0 AS TRANSNO, 'OPEN' AS TRANSTP, 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS DRAMT, 0 AS CRAMT,  " +
            //                                      " '" + FDT + "' AS TD, '' AS REMARKS, '' AS TABLEID FROM GL_MASTER WHERE (TRANSDT < '" + FrDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD " +
            //                                      " UNION " +
            //                                      " SELECT     CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, " +
            //                                      " SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, 0 AS CRAMT, " +
            //                                      " TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_3 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (TRANSTP IN ('MREC')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'PAYMENT DURING THE PERIOD' AS HEADER, 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID " +
            //                                      " FROM GL_MASTER AS GL_MASTER_2 WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('MPAY')) AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT CONVERT(nvarchar(20), TRANSDT, 103) AS TRANSDT, TRANSNO, TRANSTP, 'TRANSFER DURING THE PERIOD' AS HEADER, 4 AS SL, " +
            //                                      " SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, CREDITCD AS CRCD2, SUM(ISNULL(DEBITAMT, 0)) AS DRAMT, " +
            //                                      " SUM(ISNULL(DEBITAMT, 0)) AS CRAMT, TRANSDT AS TD, REMARKS, TABLEID FROM GL_MASTER AS GL_MASTER_2 " +
            //                                      " WHERE (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(CREDITCD, 1, 5) = '10201') AND (TRANSTP IN ('CONT')) AND " +
            //                                      " (TRANSDRCR = 'DEBIT') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')" +
            //                                      " GROUP BY DEBITCD, CREDITCD, TRANSDT, TRANSNO, TRANSTP, REMARKS, TABLEID " +
            //                                      " UNION " +
            //                                      " SELECT '" + To + "' AS TRANSDT, 0 AS TRANSNO, 'CLOSE' AS TRANSTP, 'CLOSING BALANCE' AS HEADER, 5 AS SL, SUBSTRING(DEBITCD, 1, 7) " +
            //                                      " + '00000' AS ACCD1, DEBITCD AS ACCD2, '' AS CRCD2, 0 AS DRAMT, SUM(ISNULL(DEBITAMT, 0)) - SUM(CREDITAMT) AS CRAMT, '" + TDT + "' AS TD, " +
            //                                      " '' AS REMARKS, '' AS TABLEID " +
            //                                      " FROM         GL_MASTER AS GL_MASTER_1 WHERE (TRANSDT <= '" + ToDT + "') AND (SUBSTRING(DEBITCD, 1, 5) = '10201') AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "') GROUP BY DEBITCD) AS A " +
            //            " INNER JOIN GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_ACCHART AS GL_ACCHART_2 ON A.CRCD2 = GL_ACCHART_2.ACCOUNTCD " +
            //            " WHERE (A.SL = 5) ORDER BY A.TRANSTP, A.TD, A.TRANSNO, ACCOUNTNM2", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblClosing.Text = "CLOSING BALANCE";
                GridView4.DataSource = ds;
                GridView4.DataBind();
                GridView4.Visible = true;
            }
            else
            {
                lblClosing.Visible = false;
                GridView4.Visible = false;
            }
        }

        public void showGrid_Jour()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");
            string FDT = FDate.ToString("yyyy-MM-dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");
            string TDT = TDate.ToString("yyyy-MM-dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();
            SqlCommand cmd = new SqlCommand();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(" SELECT * FROM (SELECT CONVERT(NVARCHAR(20), GL_MASTER.TRANSDT, 103) AS TRANSDT, GL_MASTER.TRANSDT AS TD, (CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) WHEN GL_MASTER.TABLEID = 'GL_MTRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) " +
                                          " ELSE 'A' END) +'-'+ CONVERT(NVARCHAR(10),GL_MASTER.TRANSNO,103) AS TRANSNO, GL_MASTER.DEBITCD, " +
                                          " GL_MASTER.CREDITCD, GL_MASTER.REMARKS, GL_MASTER.DEBITAMT AS AMOUNT, GL_ACCHART.ACCOUNTNM AS DEBITNM, GL_ACCHART_1.ACCOUNTNM AS CREDITNM " +
                                          " FROM GL_MASTER INNER JOIN GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN " +
                                          " GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD " +
                                          " WHERE     (GL_MASTER.TRANSTP = 'JOUR') AND (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(GL_MASTER.DEBITCD, 1, 5) <> '10201') " +
                                          " AND (GL_MASTER.TRANSDRCR = 'DEBIT') AND GL_MASTER.DEBITAMT<>0) AS A ORDER BY TD,TRANSNO DESC", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT * FROM (SELECT CONVERT(NVARCHAR(20), GL_MASTER.TRANSDT, 103) AS TRANSDT, GL_MASTER.TRANSDT AS TD, (CASE WHEN GL_MASTER.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) WHEN GL_MASTER.TABLEID = 'GL_MTRANS' THEN SUBSTRING(GL_MASTER.TABLEID, 4, 1) " +
            //                                  " ELSE 'A' END) +'-'+ CONVERT(NVARCHAR(10),GL_MASTER.TRANSNO,103) AS TRANSNO, GL_MASTER.DEBITCD, " +
            //                                  " GL_MASTER.CREDITCD, GL_MASTER.REMARKS, GL_MASTER.DEBITAMT AS AMOUNT, GL_ACCHART.ACCOUNTNM AS DEBITNM, GL_ACCHART_1.ACCOUNTNM AS CREDITNM " +
            //                                  " FROM GL_MASTER INNER JOIN GL_ACCHART ON GL_MASTER.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN " +
            //                                  " GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER.CREDITCD = GL_ACCHART_1.ACCOUNTCD " +
            //                                  " WHERE     (GL_MASTER.TRANSTP = 'JOUR') AND (GL_MASTER.TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (SUBSTRING(GL_MASTER.DEBITCD, 1, 5) <> '10201') " +
            //                                  " AND (GL_MASTER.TRANSDRCR = 'DEBIT') AND GL_MASTER.DEBITAMT<>0 AND (SUBSTRING(COSTPID, 1, 3) ='" + brCD + "')) AS A ORDER BY TD,TRANSNO DESC", conn);
            //}

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblNonCash.Text = "NON CASH JOURNAL";
                GridView5.DataSource = ds;
                GridView5.DataBind();
                GridView5.Visible = true;
            }
            else
            {
                lblNonCash.Visible = false;
                GridView5.Visible = false;
            }
        }

        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //strPreviousRowID_OP = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();

                string TD = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                e.Row.Cells[0].Text = TD;

                string DOCNO = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + DOCNO;

                string ACCNM = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + ACCNM;

                string CREDITNM = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + CREDITNM;

                string REMARKS = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + REMARKS;

                decimal DrAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount);
                e.Row.Cells[5].Text = DRAMT;

                decimal dblDRAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount);
                e.Row.Cells[6].Text = CRAMT;

                decimal dblCRAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());

                // Cumulating Sub Total            
                dblSubTotalDRAMT_OP += dblDRAMT;
                dblSubTotalDRAMTComma_OP = dbFunctions.SpellAmount.comma(dblSubTotalDRAMT_OP);
                dblSubTotalCRAMT_OP += dblCRAMT;
                dblSubTotalCRAMTComma_OP = dbFunctions.SpellAmount.comma(dblSubTotalCRAMT_OP);

                // Cumulating Grand Total           
                dblGrandTotalDRAMT_OP += dblDRAMT;
                dblGrandTotalDRAMTComma_OP = dbFunctions.SpellAmount.comma(dblGrandTotalDRAMT_OP);
                dblGrandTotalCRAMT_OP += dblCRAMT;
                dblGrandTotalCRAMTComma_OP = dbFunctions.SpellAmount.comma(dblGrandTotalCRAMT_OP);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Sub Total :   ";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[5].Text = dblGrandTotalDRAMTComma_OP;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[6].Text = dblGrandTotalCRAMTComma_OP;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            MakeGridViewPrinterFriendly(GridView1);
        }

        private void MakeGridViewPrinterFriendly(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //strPreviousRowID_MREC = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();


                string TD_MREC = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                e.Row.Cells[0].Text = TD_MREC;

                string DOCNO_MREC = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + DOCNO_MREC;

                string ACCNM_MREC = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + ACCNM_MREC;

                string CREDITNM = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + CREDITNM;

                string REMARKS_MREC = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + REMARKS_MREC;

                decimal DrAmount_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount_MREC);
                e.Row.Cells[5].Text = DRAMT;

                decimal dblDRAMT_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount_MREC);
                e.Row.Cells[6].Text = CRAMT;

                decimal dblCRAMT_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());

                // Cumulating Sub Total            
                dblSubTotalDRAMT_MREC += dblDRAMT_MREC;
                dblSubTotalDRAMTComma_MREC = dbFunctions.SpellAmount.comma(dblSubTotalDRAMT_MREC);
                dblSubTotalCRAMT_MREC += dblCRAMT_MREC;
                dblSubTotalCRAMTComma_MREC = dbFunctions.SpellAmount.comma(dblSubTotalCRAMT_MREC);

                // Cumulating Grand Total           
                dblGrandTotalDRAMT_MREC += dblDRAMT_MREC;
                dblGrandTotalDRAMTComma_MREC = dbFunctions.SpellAmount.comma(dblGrandTotalDRAMT_MREC);
                dblGrandTotalCRAMT_MREC += dblCRAMT_MREC;
                dblGrandTotalCRAMTComma_MREC = dbFunctions.SpellAmount.comma(dblGrandTotalCRAMT_MREC);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Sub Total :   ";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[5].Text = dblGrandTotalDRAMTComma_MREC;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[6].Text = dblGrandTotalCRAMTComma_MREC;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            MakeGridViewPrinterFriendly2(GridView2);
        }

        private void MakeGridViewPrinterFriendly2(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //strPreviousRowID_MPAY = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();

                string TD_MPAY = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                e.Row.Cells[0].Text = TD_MPAY;

                string DOCNO_MPAY = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + DOCNO_MPAY;

                string ACCNM_MPAY = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + ACCNM_MPAY;

                string CREDITNM = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + CREDITNM;

                string REMARKS_MPAY = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + REMARKS_MPAY;

                decimal DrAmount_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount_MPAY);
                e.Row.Cells[5].Text = DRAMT;

                decimal dblDRAMT_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount_MPAY);
                e.Row.Cells[6].Text = CRAMT;

                decimal dblCRAMT_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());

                // Cumulating Sub Total            
                dblSubTotalDRAMT_MPAY += dblDRAMT_MPAY;
                dblSubTotalDRAMTComma_MPAY = dbFunctions.SpellAmount.comma(dblSubTotalDRAMT_MPAY);
                dblSubTotalCRAMT_MPAY += dblCRAMT_MPAY;
                dblSubTotalCRAMTComma_MPAY = dbFunctions.SpellAmount.comma(dblSubTotalCRAMT_MPAY);

                // Cumulating Grand Total           
                dblGrandTotalDRAMT_MPAY += dblDRAMT_MPAY;
                dblGrandTotalDRAMTComma_MPAY = dbFunctions.SpellAmount.comma(dblGrandTotalDRAMT_MPAY);
                dblGrandTotalCRAMT_MPAY += dblCRAMT_MPAY;
                dblGrandTotalCRAMTComma_MPAY = dbFunctions.SpellAmount.comma(dblGrandTotalCRAMT_MPAY);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Sub Total :   ";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[5].Text = dblGrandTotalDRAMTComma_MPAY;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[6].Text = dblGrandTotalCRAMTComma_MPAY;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            MakeGridViewPrinterFriendly3(GridView3);
        }

        private void MakeGridViewPrinterFriendly3(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

        /// <summary>
        /// transfer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView6_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //strPreviousRowID_MPAY = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();

                string TD_CONT = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                e.Row.Cells[0].Text = TD_CONT;

                string DOCNO_CONT = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + DOCNO_CONT;

                string ACCNM_CONT = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + ACCNM_CONT;

                string CREDITNM = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + CREDITNM;

                string REMARKS_CONT = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + REMARKS_CONT;

                decimal DrAmount_CONT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount_CONT);
                e.Row.Cells[5].Text = DRAMT;

                decimal dblDRAMT_CONT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount_CONT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount_CONT);
                e.Row.Cells[6].Text = CRAMT;

                decimal dblCRAMT_CONT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());

                //// Cumulating Sub Total            
                //dblSubTotalDRAMT_MPAY += dblDRAMT_MPAY;
                //dblSubTotalDRAMTComma_MPAY = SpellAmount.comma(dblSubTotalDRAMT_MPAY);
                //dblSubTotalCRAMT_MPAY += dblCRAMT_MPAY;
                //dblSubTotalCRAMTComma_MPAY = SpellAmount.comma(dblSubTotalCRAMT_MPAY);

                // Cumulating Grand Total           
                dblGrandTotalDRAMT_CONT += dblDRAMT_CONT;
                dblGrandTotalDRAMTComma_CONT = dbFunctions.SpellAmount.comma(dblGrandTotalDRAMT_CONT);
                dblGrandTotalCRAMT_CONT += dblCRAMT_CONT;
                dblGrandTotalCRAMTComma_CONT = dbFunctions.SpellAmount.comma(dblGrandTotalCRAMT_CONT);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Sub Total :   ";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[5].Text = dblGrandTotalDRAMTComma_CONT;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[6].Text = dblGrandTotalCRAMTComma_CONT;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            MakeGridViewPrinterFriendly6(GridView6);
        }

        private void MakeGridViewPrinterFriendly6(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }
        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //strPreviousRowID_CL = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();

                string TD_CL = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                e.Row.Cells[0].Text = TD_CL;

                string DOCNO_CL = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + DOCNO_CL;

                string ACCNM_CL = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + ACCNM_CL;

                string CREDITNM = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + CREDITNM;

                string REMARKS_CL = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + REMARKS_CL;

                decimal DrAmount_CL = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount_CL);
                e.Row.Cells[5].Text = DRAMT;

                decimal dblDRAMT_CL = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount_CL = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount_CL);
                e.Row.Cells[6].Text = CRAMT;

                decimal dblCRAMT_CL = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());

                // Cumulating Sub Total            
                dblSubTotalDRAMT_CL += dblDRAMT_CL;
                dblSubTotalDRAMTComma_CL = dbFunctions.SpellAmount.comma(dblSubTotalDRAMT_CL);
                dblSubTotalCRAMT_CL += dblCRAMT_CL;
                dblSubTotalCRAMTComma_CL = dbFunctions.SpellAmount.comma(dblSubTotalCRAMT_CL);

                // Cumulating Grand Total           
                dblGrandTotalDRAMT_CL += dblDRAMT_CL;
                dblGrandTotalDRAMTComma_CL = dbFunctions.SpellAmount.comma(dblGrandTotalDRAMT_CL);
                dblGrandTotalCRAMT_CL += dblCRAMT_CL;
                dblGrandTotalCRAMTComma_CL = dbFunctions.SpellAmount.comma(dblGrandTotalCRAMT_CL);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Sub Total :   ";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[5].Text = dblGrandTotalDRAMTComma_CL;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[6].Text = dblGrandTotalCRAMTComma_CL;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            MakeGridViewPrinterFriendly4(GridView4);
        }

        private void MakeGridViewPrinterFriendly4(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.UseAccessibleHeader = true;
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                //gridView.HeaderRow.Style["display"] = "table-header-group";
            }
        }

        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void GridView5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //strPreviousRowID_CL = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();

                string TD_CL = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                e.Row.Cells[0].Text = TD_CL;

                string TRANSNO = DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString();
                e.Row.Cells[1].Text = TRANSNO;

                string DEBITNM = DataBinder.Eval(e.Row.DataItem, "DEBITNM").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + DEBITNM;

                string CREDITNM = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + CREDITNM;

                string REMARKS_JR = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + REMARKS_JR;

                decimal Amount_JR = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMOUNT").ToString());
                string AMT = dbFunctions.SpellAmount.comma(Amount_JR);
                e.Row.Cells[5].Text = AMT;

                // Cumulating Grand Total           
                dblGrandTotalAMT_JR += Amount_JR;
                dblGrandTotalAMTComma_JR = dbFunctions.SpellAmount.comma(dblGrandTotalAMT_JR);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Grand Total :   ";
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[5].Text = dblGrandTotalAMTComma_JR;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

            MakeGridViewPrinterFriendly5(GridView5);
        }

        private void MakeGridViewPrinterFriendly5(GridView grid)
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