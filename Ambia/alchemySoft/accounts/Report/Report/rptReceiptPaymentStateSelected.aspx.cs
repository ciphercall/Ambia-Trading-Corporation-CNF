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
    public partial class rptReceiptPaymentStateSelected : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        // To keep track of the previous row Group Identifier    
        string strPreviousRowID_OP = string.Empty;
        string strPreviousRowID_MREC = string.Empty;
        string strPreviousRowID_MPAY = string.Empty;
        string strPreviousRowID_CL = string.Empty;
        // To keep track the Index of Group Total    
        int intSubTotalIndex_OP = 1;
        int intSubTotalIndex_MREC = 1;
        int intSubTotalIndex_MPAY = 1;
        int intSubTotalIndex_CL = 1;
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
        decimal dblGrandTotalDRAMT_CL = 0;
        decimal dblGrandTotalCRAMT_OP = 0;
        decimal dblGrandTotalCRAMT_MREC = 0;
        decimal dblGrandTotalCRAMT_MPAY = 0;
        decimal dblGrandTotalCRAMT_CL = 0;
        //string AmountComma = "";
        string dblSubTotalDRAMTComma_OP = "";
        string dblSubTotalDRAMTComma_MREC = "";
        string dblSubTotalDRAMTComma_MPAY = "";
        string dblSubTotalDRAMTComma_CL = "";
        string dblSubTotalCRAMTComma_OP = "";
        string dblSubTotalCRAMTComma_MREC = "";
        string dblSubTotalCRAMTComma_MPAY = "";
        string dblSubTotalCRAMTComma_CL = "";


        string dblGrandTotalDRAMTComma_OP = "";
        string dblGrandTotalDRAMTComma_MREC = "";
        string dblGrandTotalDRAMTComma_MPAY = "";
        string dblGrandTotalDRAMTComma_CL = "";
        string dblGrandTotalCRAMTComma_OP = "";
        string dblGrandTotalCRAMTComma_MREC = "";
        string dblGrandTotalCRAMTComma_MPAY = "";
        string dblGrandTotalCRAMTComma_CL = "";

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
                string HeadNM = Session["HeadNM"].ToString();

                DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                lblFrom.Text = FDate.ToString("dd-MMM-yyyy");

                DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                lblTo.Text = TDate.ToString("dd-MMM-yyyy");

                lblHeadName.Text = HeadNM;

                showGrid_OP();
                showGrid_MREC();
                showGrid_MPAY();
                showGrid_CL();
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
            string HeadCD = Session["HeadCD"].ToString();
            string HeadNM = Session["HeadNM"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2 " +
                                                    " FROM (SELECT 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) - SUM(isnull(CREDITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM GL_MASTER " +
                                                    " WHERE      (TRANSDT < '" + FrDT + "') AND (DEBITCD = '" + HeadCD + "') " +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, CREDITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_3 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') AND (TRANSTP = 'MREC') " +
                                                    " GROUP BY CREDITCD " +
                                                    " UNION " +
                                                    " SELECT     'PAYMENT DURING THE PERIOD' AS 'HEADER', 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_2 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (CREDITCD = '" + HeadCD + "') AND (TRANSTP = 'MPAY') " +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT     'CLOSING BALANCE' AS HEADER, 4 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) - SUM(CREDITAMT) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_1 " +
                                                    " WHERE     (TRANSDT <= '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') " +
                                                    " GROUP BY DEBITCD) AS A INNER JOIN " +
                                            " GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN " +
                                            " GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD " +
                                            " WHERE A.HEADER ='OPENING BALANCE' " +
                                            "ORDER BY A.SL", conn);

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
            string HeadCD = Session["HeadCD"].ToString();
            string HeadNM = Session["HeadNM"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2 " +
                                                    " FROM (SELECT 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) - SUM(isnull(CREDITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM GL_MASTER " +
                                                   " WHERE      (TRANSDT < '" + FrDT + "') AND (DEBITCD = '" + HeadCD + "')" +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, CREDITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_3 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') AND (TRANSTP IN ('MREC','CONT')) AND ISNULL(DEBITAMT,0)!= 0 " +
                                                    " GROUP BY CREDITCD " +
                                                    " UNION " +
                                                    " SELECT     'PAYMENT DURING THE PERIOD' AS 'HEADER', 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_2 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (CREDITCD = '" + HeadCD + "') AND (TRANSTP IN ('MPAY','CONT')) AND ISNULL(DEBITAMT,0)!= 0" +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT     'CLOSING BALANCE' AS HEADER, 4 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) - SUM(CREDITAMT) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_1 " +
                                                    " WHERE     (TRANSDT <= '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') " +
                                                    " GROUP BY DEBITCD) AS A INNER JOIN " +
                                            " GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN " +
                                            " GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD " +
                                            " WHERE A.HEADER ='RECEIVED DURING THE PERIOD' " +
                                            "ORDER BY A.SL", conn);

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
            string HeadCD = Session["HeadCD"].ToString();
            string HeadNM = Session["HeadNM"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2 " +
                                                    " FROM (SELECT 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) - SUM(isnull(CREDITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM GL_MASTER " +
                                                    " WHERE      (TRANSDT < '" + FrDT + "') AND (DEBITCD = '" + HeadCD + "') " +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, CREDITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_3 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') AND (TRANSTP IN ('MREC','CONT')) AND ISNULL(DEBITAMT,0)!= 0 " +
                                                    " GROUP BY CREDITCD " +
                                                    " UNION " +
                                                    " SELECT     'PAYMENT DURING THE PERIOD' AS 'HEADER', 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_2 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (CREDITCD = '" + HeadCD + "') AND (TRANSTP IN ('MPAY','CONT')) AND ISNULL(DEBITAMT,0)!= 0 " +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT     'CLOSING BALANCE' AS HEADER, 4 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) - SUM(CREDITAMT) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_1 " +
                                                    " WHERE     (TRANSDT <= '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') " +
                                                    " GROUP BY DEBITCD) AS A INNER JOIN " +
                                            " GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN " +
                                            " GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD " +
                                            " WHERE A.HEADER ='PAYMENT DURING THE PERIOD' " +
                                            "ORDER BY A.SL", conn);

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

        public void showGrid_CL()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();
            string HeadCD = Session["HeadCD"].ToString();
            string HeadNM = Session["HeadNM"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT A.SL, A.HEADER, A.ACCD1, A.ACCD2, A.DRAMT, A.CRAMT, GL_ACCHART.ACCOUNTNM, GL_ACCHART_1.ACCOUNTNM AS ACCOUNTNM2 " +
                                                    " FROM (SELECT 'OPENING BALANCE' AS HEADER, 1 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) - SUM(isnull(CREDITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM GL_MASTER " +
                                                    " WHERE      (TRANSDT < '" + FrDT + "') AND (DEBITCD = '" + HeadCD + "') " +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT 'RECEIVED DURING THE PERIOD' AS HEADER, 2 AS SL, SUBSTRING(CREDITCD, 1, 7) + '00000' AS ACCD1, CREDITCD AS ACCD2, SUM(isnull(DEBITAMT,0)) AS DRAMT, 0 AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_3 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') AND (TRANSTP = 'MREC') " +
                                                    " GROUP BY CREDITCD " +
                                                    " UNION " +
                                                    " SELECT     'PAYMENT DURING THE PERIOD' AS 'HEADER', 3 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_2 " +
                                                    " WHERE     (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') AND (CREDITCD = '" + HeadCD + "') AND (TRANSTP = 'MPAY') " +
                                                    " GROUP BY DEBITCD " +
                                                    " UNION " +
                                                    " SELECT     'CLOSING BALANCE' AS HEADER, 4 AS SL, SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCD1, DEBITCD AS ACCD2, 0 AS DRAMT, SUM(isnull(DEBITAMT,0)) - SUM(CREDITAMT) AS CRAMT " +
                                                    " FROM         GL_MASTER AS GL_MASTER_1 " +
                                                    " WHERE     (TRANSDT <= '" + ToDT + "') AND (DEBITCD = '" + HeadCD + "') " +
                                                    " GROUP BY DEBITCD) AS A INNER JOIN " +
                                            " GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD INNER JOIN " +
                                            " GL_ACCHART AS GL_ACCHART_1 ON A.ACCD1 = GL_ACCHART_1.ACCOUNTCD " +
                                            " WHERE A.HEADER ='CLOSING BALANCE' " +
                                            "ORDER BY A.SL", conn);

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

        /// <summary>   
        /// /// Event fires for every row creation   
        /// /// Used for creating SubTotal row when next group starts by adding Group Total at previous row manually    
        /// </summary>    /// <param name="sender"></param>    /// <param name="e"></param>   
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd_OP = false;
            bool IsGrandTotalRowNeedtoAdd_OP = false;
            if ((strPreviousRowID_OP != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
                if (strPreviousRowID_OP != DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString())
                    IsSubTotalRowNeedToAdd_OP = true;
            if ((strPreviousRowID_OP != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") == null))
            {
                IsSubTotalRowNeedToAdd_OP = true;
                IsGrandTotalRowNeedtoAdd_OP = true;
                intSubTotalIndex_OP = 0;
            }
            //#region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID_OP == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
            {
                GridView GridView1 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                cell.ColumnSpan = 3;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_OP, row);
                intSubTotalIndex_OP++;
            }
            //#endregion
            if (IsSubTotalRowNeedToAdd_OP)
            {
                //#region Adding Sub Total Row
                GridView GridView1 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding DRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalDRAMTComma_OP);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalCRAMTComma_OP);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_OP, row);
                intSubTotalIndex_OP++;
                //#endregion
                //#region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                    cell.ColumnSpan = 3;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_OP, row);
                    intSubTotalIndex_OP++;
                }
                //#endregion
                //#region Reseting the Sub Total Variables
                dblSubTotalDRAMT_OP = 0;
                dblSubTotalCRAMT_OP = 0;
                //#endregion
            }
            if (IsGrandTotalRowNeedtoAdd_OP)
            {
                //#region Grand Total Row
                GridView GridView1 = (GridView)sender;
                // Creating a Row      
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cell = new TableCell();
                cell.Text = "Opening Balance Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                ////Adding DRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalDRAMTComma_OP);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalCRAMTComma_OP);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid     
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                //#endregion
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
                strPreviousRowID_OP = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();


                string ACCNM = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCNM;

                decimal DrAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount);
                e.Row.Cells[1].Text = DRAMT;

                decimal dblDRAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount);
                e.Row.Cells[2].Text = CRAMT;

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
        /// /// Event fires for every row creation   
        /// /// Used for creating SubTotal row when next group starts by adding Group Total at previous row manually    
        /// </summary>    /// <param name="sender"></param>    /// <param name="e"></param>   
        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd_MREC = false;
            bool IsGrandTotalRowNeedtoAdd_MREC = false;
            if ((strPreviousRowID_MREC != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
                if (strPreviousRowID_MREC != DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString())
                    IsSubTotalRowNeedToAdd_MREC = true;
            if ((strPreviousRowID_MREC != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") == null))
            {
                IsSubTotalRowNeedToAdd_MREC = true;
                IsGrandTotalRowNeedtoAdd_MREC = true;
                intSubTotalIndex_MREC = 0;
            }
            //#region Inserting_MREC
            if ((strPreviousRowID_MREC == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
            {
                GridView GridView2 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                cell.ColumnSpan = 3;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                GridView2.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_MREC, row);
                intSubTotalIndex_MREC++;
            }
            //#endregion
            if (IsSubTotalRowNeedToAdd_MREC)
            {
                //#region Adding_MREC
                GridView GridView2 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding DRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalDRAMTComma_MREC);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalCRAMTComma_MREC);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                GridView2.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_MREC, row);
                intSubTotalIndex_MREC++;
                //#endregion
                //#region Adding_Next_MREC
                if (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                    cell.ColumnSpan = 3;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView2.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_MREC, row);
                    intSubTotalIndex_MREC++;
                }
                //#endregion
                //#region Reseting the Sub Total Variables for MREC
                dblSubTotalDRAMT_MREC = 0;
                dblSubTotalCRAMT_MREC = 0;
                //#endregion
            }
            if (IsGrandTotalRowNeedtoAdd_MREC)
            {
                //#region Grand_Total_MREC
                GridView GridView2 = (GridView)sender;
                // Creating a Row      
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cell = new TableCell();
                cell.Text = "Received Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                ////Adding DRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalDRAMTComma_MREC);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalCRAMTComma_MREC);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid     
                GridView2.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                //#endregion
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
                strPreviousRowID_MREC = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();


                string ACCNM_MREC = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCNM_MREC;

                decimal DrAmount_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount_MREC);
                e.Row.Cells[1].Text = DRAMT;

                decimal dblDRAMT_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount_MREC = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount_MREC);
                e.Row.Cells[2].Text = CRAMT;

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
        /// /// Event fires for every row creation   
        /// /// Used for creating SubTotal row when next group starts by adding Group Total at previous row manually    
        /// </summary>    /// <param name="sender"></param>    /// <param name="e"></param>   
        protected void GridView3_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd_MPAY = false;
            bool IsGrandTotalRowNeedtoAdd_MPAY = false;
            if ((strPreviousRowID_MPAY != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
                if (strPreviousRowID_MPAY != DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString())
                    IsSubTotalRowNeedToAdd_MPAY = true;
            if ((strPreviousRowID_MPAY != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") == null))
            {
                IsSubTotalRowNeedToAdd_MPAY = true;
                IsGrandTotalRowNeedtoAdd_MPAY = true;
                intSubTotalIndex_MPAY = 0;
            }
            //#region INS_MPAY
            if ((strPreviousRowID_MPAY == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
            {
                GridView GridView3 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                cell.ColumnSpan = 3;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                GridView3.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_MPAY, row);
                intSubTotalIndex_MPAY++;
            }
            //#endregion
            if (IsSubTotalRowNeedToAdd_MPAY)
            {
                //#region SUB_MPAY
                GridView GridView3 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding DRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalDRAMTComma_MPAY);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalCRAMTComma_MPAY);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                GridView3.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_MPAY, row);
                intSubTotalIndex_MPAY++;
                //#endregion
                //#region Next_Group_MPAY
                if (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                    cell.ColumnSpan = 3;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView3.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_MPAY, row);
                    intSubTotalIndex_MPAY++;
                }
                //#endregion
                //#region SUB_TOTAL_MPAY
                dblSubTotalDRAMT_MPAY = 0;
                dblSubTotalCRAMT_MPAY = 0;
                //#endregion
            }
            if (IsGrandTotalRowNeedtoAdd_MPAY)
            {
                //#region GRAND_MPAY
                GridView GridView3 = (GridView)sender;
                // Creating a Row      
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cell = new TableCell();
                cell.Text = "Paid Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                ////Adding DRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalDRAMTComma_MPAY);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalCRAMTComma_MPAY);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid     
                GridView3.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                //#endregion
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
                strPreviousRowID_MPAY = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();


                string ACCNM_MPAY = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCNM_MPAY;

                decimal DrAmount_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount_MPAY);
                e.Row.Cells[1].Text = DRAMT;

                decimal dblDRAMT_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount_MPAY = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount_MPAY);
                e.Row.Cells[2].Text = CRAMT;

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
        /// /// Event fires for every row creation   
        /// /// Used for creating SubTotal row when next group starts by adding Group Total at previous row manually    
        /// </summary>    /// <param name="sender"></param>    /// <param name="e"></param>   
        protected void GridView4_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd_CL = false;
            bool IsGrandTotalRowNeedtoAdd_CL = false;
            if ((strPreviousRowID_CL != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
                if (strPreviousRowID_CL != DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString())
                    IsSubTotalRowNeedToAdd_CL = true;
            if ((strPreviousRowID_CL != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") == null))
            {
                IsSubTotalRowNeedToAdd_CL = true;
                IsGrandTotalRowNeedtoAdd_CL = true;
                intSubTotalIndex_CL = 0;
            }
            //#region CL
            if ((strPreviousRowID_CL == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
            {
                GridView GridView4 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                cell.ColumnSpan = 3;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                GridView4.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_CL, row);
                intSubTotalIndex_CL++;
            }
            //#endregion
            if (IsSubTotalRowNeedToAdd_CL)
            {
                //#region row_CL
                GridView GridView4 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding DRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalDRAMTComma_CL);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalCRAMTComma_CL);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                GridView4.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_CL, row);
                intSubTotalIndex_CL++;
                //#endregion
                //#region grHeader_CL
                if (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM2").ToString(); //////// Sub Header Name
                    cell.ColumnSpan = 3;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView4.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_CL, row);
                    intSubTotalIndex_CL++;
                }
                //#endregion
                //#region res_subtotal_CL
                dblSubTotalDRAMT_CL = 0;
                dblSubTotalCRAMT_CL = 0;
                //#endregion
            }
            if (IsGrandTotalRowNeedtoAdd_CL)
            {
                //#region gr_total_CL
                GridView GridView4 = (GridView)sender;
                // Creating a Row      
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cell = new TableCell();
                cell.Text = "Closing Balance Total : ";
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                ////Adding DRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalDRAMTComma_CL);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding CRAMT Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalCRAMTComma_CL);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid     
                GridView4.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                //#endregion
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
                strPreviousRowID_CL = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();


                string ACCNM_CL = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCNM_CL;

                decimal DrAmount_CL = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());
                string DRAMT = dbFunctions.SpellAmount.comma(DrAmount_CL);
                e.Row.Cells[1].Text = DRAMT;

                decimal dblDRAMT_CL = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DRAMT").ToString());

                decimal CrAmount_CL = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CRAMT").ToString());
                string CRAMT = dbFunctions.SpellAmount.comma(CrAmount_CL);
                e.Row.Cells[2].Text = CRAMT;

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
                e.Row.Cells[0].Text = "Grand Total :   ";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                decimal G_Total_OP = Convert.ToDecimal(dblGrandTotalDRAMTComma_OP);
                decimal G_Total_MREC = 0;
                if (dblGrandTotalDRAMTComma_MREC != "")
                    G_Total_MREC = Convert.ToDecimal(dblGrandTotalDRAMTComma_MREC);

                decimal G_Total_MPAY = 0;
                if (dblGrandTotalDRAMTComma_MPAY != "")
                    G_Total_MPAY = Convert.ToDecimal(dblGrandTotalDRAMTComma_MPAY);

                decimal G_Total_CL = 0;
                if (dblGrandTotalDRAMTComma_MPAY != "")
                    G_Total_CL = Convert.ToDecimal(dblGrandTotalDRAMTComma_CL);

                decimal G_Total_DR = G_Total_OP + G_Total_MREC + G_Total_MPAY + G_Total_CL;
                string G_DR = dbFunctions.SpellAmount.comma(G_Total_DR);
                e.Row.Cells[1].Text = G_DR;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                decimal G_Total_CR_OP = 0;
                if (dblGrandTotalCRAMTComma_OP != "")
                    G_Total_CR_OP = Convert.ToDecimal(dblGrandTotalCRAMTComma_OP);

                decimal G_Total_CR_MREC = 0;
                if (dblGrandTotalCRAMTComma_MREC != "")
                    G_Total_CR_MREC = Convert.ToDecimal(dblGrandTotalCRAMTComma_MREC);

                decimal G_Total_CR_MPAY = 0;
                if (dblGrandTotalCRAMTComma_MPAY != "")
                    G_Total_CR_MPAY = Convert.ToDecimal(dblGrandTotalCRAMTComma_MPAY);

                decimal G_Total_CR_CL = 0;
                if (dblGrandTotalCRAMTComma_CL != "")
                    G_Total_CR_CL = Convert.ToDecimal(dblGrandTotalCRAMTComma_CL);

                decimal G_Total_CR = G_Total_CR_OP + G_Total_CR_MREC + G_Total_CR_MPAY + G_Total_CR_CL;
                string G_CR = dbFunctions.SpellAmount.comma(G_Total_CR);
                e.Row.Cells[2].Text = G_CR;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
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

    }
}