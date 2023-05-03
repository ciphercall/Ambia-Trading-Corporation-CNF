using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft;

namespace alchemySoft.Accounts.Report.Report
{
    public partial class rptUserLogReport : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        // To keep track of the previous row Group Identifier    
        string strPreviousRowID = string.Empty;
        // To keep track the Index of Group Total    
        int intSubTotalIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (Session["USERID"] == null)
                    Response.Redirect("~/login/ui/signin");
                else
                {
                    dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
                    dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

                    lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];
                    showGrid();
                }
        }

        public void showGrid()
        {
            string connectionString = dbFunctions.Connection;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string username = Session["UNm"].ToString();
            string userid = Session["Uid"].ToString();
            string fdt = Session["From"].ToString();
            string tdt = Session["To"].ToString();
            string logtype = Session["LOGTYPE"].ToString();

            lblfdt.Text = fdt;
            lbltdt.Text = tdt;
            lblusername.Text = username;
            DateTime From = DateTime.Parse(fdt, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime To = DateTime.Parse(tdt, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FdT = From.ToString("yyyy/MM/dd");
            string TdT = To.ToString("yyyy/MM/dd");
            string query = "";

            if (conn.State != ConnectionState.Open)conn.Open();
            string user = " ";
            if (userid == "ALL")
                user = " ";
            else user = " AND (ASL_LOG.USERID = '" + userid + "') ";

            if (logtype == "ALL")
                logtype = " ";
            else logtype = " AND (ASL_LOG.LOGTYPE = '" + logtype + "') ";

            query = @"SELECT        ASL_USERCO.USERNM, ASL_LOG.LOGTYPE, ASL_LOG.LOGSLNO, CONVERT (NVARCHAR,ASL_LOG.LOGDT,109) LOGDT, ASL_LOG.LOGIPNO, ASL_LOG.TABLEID ,
            CASE WHEN ASL_LOG.TABLEID='STK_TRANS' THEN (
            CASE WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='SALE' THEN 'SALES - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='BUY' THEN 'PURCHASE - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IREC' THEN 'RECEIVE - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IISS' THEN 'ISSUE - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IRTB' THEN 'PURCHASE RETURN - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IRTS' THEN 'SALES RETURN - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IWTG' THEN 'WASTAGE - DETAILS'
            ELSE '' END) 
            WHEN ASL_LOG.TABLEID='STK_TRANSMST' THEN (
            CASE WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='SALE' THEN 'SALES - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='BUY' THEN 'PURCHASE - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IREC' THEN 'RECEIVE - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IISS' THEN 'ISSUE - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IRTB' THEN 'PURCHASE RETURN - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IRTS' THEN 'SALES RETURN - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='IWTG' THEN 'WASTAGE - MASTER'
            ELSE '' END) 
            WHEN ASL_LOG.TABLEID='STK_WOMST' THEN 'WORK ORDER - MASTER'
             WHEN ASL_LOG.TABLEID='STK_WORDER' THEN 'WORK ORDER - DETAILS'
             WHEN ASL_LOG.TABLEID='STK_ITEM' THEN 'ITEM INFORMATION'
             WHEN ASL_LOG.TABLEID='ASL_USERCO' THEN 'LOG IN INFORMATION'
            WHEN ASL_LOG.TABLEID='HR_ATREG' THEN 'ATTENDANCE INFORMATION'
             WHEN ASL_LOG.TABLEID='HR_SALDRCR' THEN 'SALARY INCREMENT/DECREMENT'
            WHEN ASL_LOG.TABLEID='GL_MTRANS' THEN (
            CASE WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='JOUR' THEN 'MULTIPLE VOUCHER JOURNAL - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='MPAY' THEN 'MULTIPLE VOUCHER PAYMENT - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='MREC' THEN 'MULTIPLE VOUCHER RECEIVE - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='CONT' THEN 'MULTIPLE VOUCHER CONTRA - DETAILS'
            ELSE '' END) 
            WHEN ASL_LOG.TABLEID='GL_MTRANSMST' THEN (
            CASE WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='JOUR' THEN 'MULTIPLE VOUCHER JOURNAL - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='MPAY' THEN 'MULTIPLE VOUCHER PAYMENT - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='MREC' THEN 'MULTIPLE VOUCHER RECEIVE - MASTER'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='CONT' THEN 'MULTIPLE VOUCHER CONTRA - MASTER'
            ELSE '' END) 
            WHEN ASL_LOG.TABLEID='GL_STRANS' THEN (
            CASE WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='JOUR' THEN 'SINGLE VOUCHER JOURNAL - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='MPAY' THEN 'SINGLE VOUCHER PAYMENT - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='MREC' THEN 'SINGLE VOUCHER RECEIVE - DETAILS'
            WHEN SUBSTRING(ASL_LOG.LOGDATA,1,4)='CONT' THEN 'SINGLE VOUCHER CONTRA - DETAILS'
            ELSE '' END) 
            ELSE '' END AS  TRANSACTIONTP, 
            ASL_LOG.LOGDATA, ASL_LOG.userPc 
            FROM            ASL_LOG INNER JOIN
                                     ASL_USERCO ON ASL_LOG.COMPID = ASL_USERCO.COMPID AND ASL_LOG.USERID = ASL_USERCO.USERID
            WHERE      (ASL_LOG.LOGDT BETWEEN '" + FdT + "' AND '" + TdT + "') AND (ASL_LOG.USERID not in ('10001','10101')) " + user + logtype + " ORDER BY ASL_USERCO.USERNM,  ASL_LOG.LOGDT, ASL_LOG.TABLEID";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != ConnectionState.Closed)conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                GridView1.DataSource = ds;
                GridView1.DataBind();
                int columncount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
                GridView1.Rows[0].Cells[0].Text = "No Records Found";
            }
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "USERNM").ToString();

                    string LOGDT = DataBinder.Eval(e.Row.DataItem, "LOGDT").ToString();
                    e.Row.Cells[0].Text = LOGDT;

                    string LOGTYPE = DataBinder.Eval(e.Row.DataItem, "LOGTYPE").ToString();
                    e.Row.Cells[1].Text = LOGTYPE;

                    string TABLEID = DataBinder.Eval(e.Row.DataItem, "TRANSACTIONTP").ToString();
                    e.Row.Cells[2].Text = TABLEID;

                    string LOGIPNO = DataBinder.Eval(e.Row.DataItem, "LOGIPNO").ToString();
                    e.Row.Cells[3].Text = LOGIPNO;

                    string userPc = DataBinder.Eval(e.Row.DataItem, "userPc").ToString();
                    e.Row.Cells[4].Text = userPc;

                    string LOGDATA = DataBinder.Eval(e.Row.DataItem, "LOGDATA").ToString();
                    e.Row.Cells[5].Text = LOGDATA;
                }
                catch (Exception ex)
                {
                    dbFunctions.showMessage(Page,ex.Message);
                }
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

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd = false;
            //bool IsGrandTotalRowNeedtoAdd = false;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "USERNM") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "USERNM").ToString())
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "USERNM") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                //    IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }
            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "USERNM") != null))
            {
                GridView GridView1 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = DataBinder.Eval(e.Row.DataItem, "USERNM").ToString();
                cell.ColumnSpan = 6;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
            }
            #endregion
            if (IsSubTotalRowNeedToAdd)
            {
                //    #region Adding Sub Total Row
                GridView GridView1 = (GridView)sender;
                //    // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();

                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                //#endregion
                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "USERNM") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = DataBinder.Eval(e.Row.DataItem, "USERNM").ToString();
                    cell.ColumnSpan = 6;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                }
                #endregion

            }

        }
    }
}