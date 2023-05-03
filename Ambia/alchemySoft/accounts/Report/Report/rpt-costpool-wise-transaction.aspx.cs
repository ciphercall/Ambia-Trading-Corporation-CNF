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
using alchemySoft;

namespace DynamicMenu.Accounts.Report.Report
{
    public partial class rpt_costpool_wise_transaction : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        string strPreviousRowID = string.Empty;

        int intSubTotalIndex = 1;

        decimal subTotalAmt = 0;
        string subTotalAmtComma = "0";

        decimal grandTotalAmt = 0;
        string grandTotalAmtComma = "0";

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
                dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

                lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

                lblCostpool.Text = Session["costpoolnm"].ToString();
                string From = Session["From"].ToString();
                string To = Session["To"].ToString();

                DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                lblFrom.Text = FDate.ToString("dd-MMM-yyyy");

                DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                lblTo.Text = TDate.ToString("dd-MMM-yyyy");

                showGrid();
            }
            catch (Exception ex)
            {
                dbFunctions.showMessage(Page,ex.Message);
            }
        }

        public void showGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string costpoolid = Session["costpoolid"].ToString();
            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT        SUBSTRING(A.HSL, 3, 15) AS HTP, A.TRANSTP, CONVERT(NVARCHAR(20),A.TRANSDT,103) TRANSDT, A.TRANSNO, A.DEBITCD, A.CREDITCD, A.REMARKS, A.AMOUNT, dbo.GL_ACCHART.ACCOUNTNM AS DBNM, 
                         GL_ACCHART_1.ACCOUNTNM AS CDNM
FROM            (SELECT        '1.ASSET' AS HSL, TRANSTP, TRANSDT, TRANSNO, DEBITCD, CREDITCD, REMARKS, DEBITAMT AS AMOUNT
                          FROM            dbo.GL_MASTER
                          WHERE        (TRANSDRCR = 'DEBIT') AND (SUBSTRING(DEBITCD, 1, 1) = '1') AND (COSTPID = '" + costpoolid + "') AND (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') " +
                          " UNION " +
                          " SELECT        '2.LIABILITY' AS HSL, TRANSTP, TRANSDT, TRANSNO, DEBITCD, CREDITCD, REMARKS, CREDITAMT AS AMOUNT " +
                          " FROM            dbo.GL_MASTER AS GL_MASTER_3 " +
                          " WHERE        (TRANSDRCR = 'CREDIT') AND (SUBSTRING(DEBITCD, 1, 1) = '2') AND (COSTPID = '" + costpoolid + "') AND (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') " +
                          " UNION " +
                          " SELECT        '3.INCOME' AS HSL, TRANSTP, TRANSDT, TRANSNO, DEBITCD, CREDITCD, REMARKS, CREDITAMT AS AMOUNT " +
                          " FROM            dbo.GL_MASTER AS GL_MASTER_2 " +
                          " WHERE        (TRANSDRCR = 'CREDIT') AND (SUBSTRING(DEBITCD, 1, 1) = '3') AND (COSTPID = '" + costpoolid + "') AND (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "') " +
                          " UNION " +
                          " SELECT        '4.EXPENDITURE' AS HSL, TRANSTP, TRANSDT, TRANSNO, DEBITCD, CREDITCD, REMARKS, DEBITAMT AS AMOUNT " +
                          " FROM            dbo.GL_MASTER AS GL_MASTER_1 " +
                          " WHERE        (TRANSDRCR = 'DEBIT') AND (SUBSTRING(DEBITCD, 1, 1) = '4') AND (COSTPID = '" + costpoolid + "') AND (TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "')) AS A INNER JOIN " +
                         " dbo.GL_ACCHART ON A.DEBITCD = dbo.GL_ACCHART.ACCOUNTCD INNER JOIN " +
                         " dbo.GL_ACCHART AS GL_ACCHART_1 ON A.CREDITCD = GL_ACCHART_1.ACCOUNTCD ORDER BY A.HSL", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                GridView1.Visible = true;
            }
        }

        /// <summary>   
        /// /// Event fires for every row creation   
        /// /// Used for creating SubTotal row when next group starts by adding Group Total at previous row manually    
        /// </summary>    /// <param name="sender"></param>    /// <param name="e"></param>   
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd = false;
            //bool IsGrandTotalRowNeedtoAdd = false;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "HTP") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "HTP").ToString())
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "HTP") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                //IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }
            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "HTP") != null))
            {
                GridView GridView1 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "HTP").ToString();
                cell.ColumnSpan = 7;
                cell.Font.Bold = true;
                row.Cells.Add(cell);
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
            }
            #endregion
            if (IsSubTotalRowNeedToAdd)
            {
                #region Adding Sub Total Row
                GridView GridView1 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                ////Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.Font.Bold = true;
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.ColumnSpan = 6;
                //cell.CssClass = "gridHeadStyle";
                row.Cells.Add(cell);

                ////Adding Carton Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", subTotalAmtComma);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.Font.Bold = true;
                //cell.CssClass = "gridHeadStyle";
                row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid      
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                #endregion
                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "HTP") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "HTP").ToString();
                    cell.ColumnSpan = 7;
                    cell.Font.Bold = true;
                    //cell.CssClass = "gridHeadStyle";
                    row.Cells.Add(cell);
                    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                }
                #endregion
                #region Reseting the Sub Total Variables
                subTotalAmt = 0;
                #endregion
            }

            //if (IsGrandTotalRowNeedtoAdd)
            //{
            //    //#region Grand Total Row
            //    GridView GridView1 = (GridView)sender;
            //    // Creating a Row      
            //    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            //    //Adding Total Cell           
            //    TableCell cell = new TableCell();
            //    cell.Text = "Balance Total : ";
            //    cell.HorizontalAlign = HorizontalAlign.Left;
            //    //cell.ColumnSpan = 2;
            //    cell.CssClass = "GrandTotalRowStyle";
            //    row.Cells.Add(cell);

            //    ////Adding DRAMT Column          
            //    cell = new TableCell();
            //    cell.Text = string.Format("{0:0.00}", dblGrandTotalAMTComma);
            //    cell.HorizontalAlign = HorizontalAlign.Right;
            //    cell.CssClass = "GrandTotalRowStyle";
            //    row.Cells.Add(cell);

            //    //Adding the Row at the RowIndex position in the Grid     
            //    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
            //    //#endregion
            //}

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
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "HTP").ToString();

                string TRANSTP = DataBinder.Eval(e.Row.DataItem, "TRANSTP").ToString();
                e.Row.Cells[0].Text = TRANSTP;

                string TRANSDT = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                e.Row.Cells[1].Text = TRANSDT;

                Int64 TRANSNO = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString());
                e.Row.Cells[2].Text = TRANSNO.ToString();

                string DBNM = DataBinder.Eval(e.Row.DataItem, "DBNM").ToString();
                e.Row.Cells[3].Text = "&nbsp;" + DBNM;

                string CDNM = DataBinder.Eval(e.Row.DataItem, "CDNM").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + CDNM;

                string REMARKS = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[5].Text = "&nbsp;" + REMARKS;

                decimal AMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMOUNT").ToString());
                string Amount = dbFunctions.SpellAmount.comma(AMT);
                e.Row.Cells[6].Text = Amount;

                subTotalAmt += AMT;
                subTotalAmtComma = dbFunctions.SpellAmount.comma(subTotalAmt);

                grandTotalAmt += AMT;
                grandTotalAmtComma = dbFunctions.SpellAmount.comma(grandTotalAmt);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[5].Text = "Grand Total :   ";
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].Text = grandTotalAmtComma;
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
    }
}