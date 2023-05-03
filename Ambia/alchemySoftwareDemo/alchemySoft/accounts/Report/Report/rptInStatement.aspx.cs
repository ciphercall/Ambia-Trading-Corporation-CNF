using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using AlchemyAccounting;
using System.Data.SqlClient;
using System.Web.Services;
using System.Text;
using System.Collections.Specialized;
using System.Threading;

namespace alchemySoft.accounts.Report.Report
{
    public partial class rptInStatement : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        string strPreviousRowID = string.Empty;

        int intSubTotalIndex = 1;

        decimal dblSubTotalAMT = 0;

        decimal subTotal_IN = 0;
        decimal subTotal_Ex = 0;

        string dblSubTotalAMTComma = "";

        string chkAccCD;

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
                lblFrom.Text = FDate.ToString("dd-MMM-yyyy");

                DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                lblTo.Text = TDate.ToString("dd-MMM-yyyy");

                showGrid();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public void showGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FrDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT A.ACCD1, A.ACCD2, A.AMT, A.AMTI, A.AMTE, GL_ACCHART.ACCOUNTNM " +
                                      " FROM (SELECT SUBSTRING(DEBITCD, 1, 1) + '00000000000' AS ACCD1, SUBSTRING(DEBITCD, 1, 3) + '000000000' AS ACCD2, SUM(CREDITAMT) - SUM(DEBITAMT) " +
                                              " AS AMT, SUM(CREDITAMT) - SUM(DEBITAMT) AS AMTI, 0 AS AMTE " +
                                              " FROM GL_MASTER WHERE (SUBSTRING(DEBITCD, 1, 1) = '3') AND TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "' " +
                                              " GROUP BY SUBSTRING(DEBITCD, 1, 1) + '00000000000', SUBSTRING(DEBITCD, 1, 3) + '000000000' " +
                                              " UNION " +
                                              " SELECT SUBSTRING(DEBITCD, 1, 1) + '00000000000' AS ACCD1, SUBSTRING(DEBITCD, 1, 3) + '000000000' AS ACCD2, SUM(DEBITAMT) - SUM(CREDITAMT) " +
                                              " AS AMT, 0 AS AMTI, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMTE FROM GL_MASTER AS GL_MASTER_1 " +
                                              " WHERE (SUBSTRING(DEBITCD, 1, 1) = '4') AND TRANSDT BETWEEN '" + FrDT + "' AND '" + ToDT + "' " +
                                      " GROUP BY SUBSTRING(DEBITCD, 1, 1) + '00000000000', SUBSTRING(DEBITCD, 1, 3) + '000000000') AS A INNER JOIN " +
                                      " GL_ACCHART ON A.ACCD2 = GL_ACCHART.ACCOUNTCD", conn);

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
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString())
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                //IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }
            //#region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null))
            {
                GridView GridView1 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                chkAccCD = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
            }
            //#endregion
            if (IsSubTotalRowNeedToAdd)
            {
                //#region Adding Sub Total Row
                GridView GridView1 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                if (chkAccCD == "300000000000")
                {
                    cell.Text = "Income Total : ";
                }
                else
                {
                    cell.Text = "Expense Total :";
                }
                cell.HorizontalAlign = HorizontalAlign.Left;
                //cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding DRAMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalAMTComma);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid      
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                //#endregion
                //#region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "ACCD1") != null)
                {
                    //row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    //cell = new TableCell();
                    //cell.Text = "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCD2").ToString(); //////// Sub Header Name
                    //cell.ColumnSpan = 3;
                    //cell.CssClass = "GroupHeaderStyle";
                    //row.Cells.Add(cell);
                    chkAccCD = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();
                    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                }
                //#endregion
                //#region Reseting the Sub Total Variables
                dblSubTotalAMT = 0;
                //dblSubTotalCRAMT_OP = 0;
                //#endregion
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
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "ACCD1").ToString();


                string ACCNM = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCNM;

                decimal AMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMT").ToString());
                string Amount = dbFunctions.SpellAmount.comma(AMT);
                e.Row.Cells[1].Text = Amount;

                decimal dblAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMT").ToString());

                // Cumulating Sub Total            
                if (chkAccCD == "300000000000")
                {
                    dblSubTotalAMT += dblAMT;
                    dblSubTotalAMTComma = dbFunctions.SpellAmount.comma(dblSubTotalAMT);
                    subTotal_IN = Convert.ToDecimal(dblSubTotalAMTComma);
                }
                else
                {
                    dblSubTotalAMT += dblAMT;
                    dblSubTotalAMTComma = dbFunctions.SpellAmount.comma(dblSubTotalAMT);
                    subTotal_Ex = Convert.ToDecimal(dblSubTotalAMTComma);
                }

                // Cumulating Grand Total           
                //dblGrandTotalAMT += dblAMT;
                //dblGrandTotalAMTComma = SpellAmount.comma(dblGrandTotalAMT);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Net Profit/Loss :   ";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                decimal NProfit = subTotal_IN - subTotal_Ex;

                string NetProfit = dbFunctions.SpellAmount.comma(NProfit);
                e.Row.Cells[1].Text = NetProfit;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;

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