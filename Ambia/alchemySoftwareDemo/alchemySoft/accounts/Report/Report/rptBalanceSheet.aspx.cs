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
    public partial class rptBalanceSheet : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        // To keep track of the previous row Group Identifier    
        string strPreviousRowID_Asset = string.Empty;
        string strPreviousRowID_Lia = string.Empty;

        // To keep track the Index of Group Total    
        int intSubTotalIndex_Asset = 1;
        int intSubTotalIndex_Lia = 1;

        // To temporarily store Sub Total    
        decimal dblSubTotalAMT_Asset = 0;
        decimal dblSubTotalAMT_Lia = 0;

        // To temporarily store Grand Total    
        decimal dblGrandTotalAMT_Asset = 0;
        decimal dblGrandTotalAMT_Lia = 0;

        string dblSubTotalAMTComma_Asset = "";
        string dblSubTotalAMTComma_Lia = "";

        string dblGrandTotalAMTComma_Asset = "";
        string dblGrandTotalAMTComma_Lia = "";

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
            dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

            lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

            string Date = Session["Date"].ToString();

            DateTime stDate = DateTime.Parse(Date, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblDate.Text = stDate.ToString("dd-MMM-yyyy");

            showgrid_Asset();
            showgrid_Liability();
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.Message);
            //}
        }

        public void showgrid_Asset()
        { 
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string Date = Session["Date"].ToString();

            DateTime stDate = DateTime.Parse(Date, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string st_DT = stDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("", conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = (" SELECT A.ACCD1, GL_ACCHART.ACCOUNTNM AS ACCDNM1, A.ACCD2, GL_ACCHART_1.ACCOUNTNM AS ACCDNM2, A.ACCD3, GL_ACCHART_2.ACCOUNTNM AS ACCDNM3, A.AMT " +
                               " FROM (SELECT SUBSTRING(DEBITCD, 1, 1) + '00000000000' AS ACCD1, SUBSTRING(DEBITCD, 1, 3) + '000000000' AS ACCD2, SUBSTRING(DEBITCD, 1, 5) + '0000000' AS ACCD3, " +
                               " SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT FROM GL_MASTER WHERE (SUBSTRING(DEBITCD, 1, 1) = '1') AND TRANSDT <= '" + st_DT + "' " +
                               " GROUP BY SUBSTRING(DEBITCD, 1, 1) + '00000000000', SUBSTRING(DEBITCD, 1, 3) + '000000000', SUBSTRING(DEBITCD, 1, 5) + '0000000') AS A INNER JOIN " +
                               " GL_ACCHART ON A.ACCD1 = GL_ACCHART.ACCOUNTCD INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD2 = GL_ACCHART_1.ACCOUNTCD INNER JOIN " +
                               " GL_ACCHART AS GL_ACCHART_2 ON A.ACCD3 = GL_ACCHART_2.ACCOUNTCD");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd_Ast = false;
            if ((strPreviousRowID_Asset != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD2") != null))
                if (strPreviousRowID_Asset != DataBinder.Eval(e.Row.DataItem, "ACCD2").ToString())
                    IsSubTotalRowNeedToAdd_Ast = true;
            if ((strPreviousRowID_Asset != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD2") == null))
            {
                IsSubTotalRowNeedToAdd_Ast = true;
                intSubTotalIndex_Asset = 0;
            }
            //#region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID_Asset == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD2") != null))
            {
                GridView GridView1 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCDNM2").ToString(); //////// Sub Header Name
                cell.ColumnSpan = 3;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_Asset, row);
                intSubTotalIndex_Asset++;
            }
            //#endregion
            if (IsSubTotalRowNeedToAdd_Ast)
            {
                //#region Adding Sub Total Row
                GridView GridView1 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.HorizontalAlign = HorizontalAlign.Right;
                //cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);


                //Adding AMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalAMTComma_Asset);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_Asset, row);
                intSubTotalIndex_Asset++;
                //#endregion
                //#region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "ACCD2") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCDNM2").ToString(); //////// Sub Header Name
                    cell.ColumnSpan = 3;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_Asset, row);
                    intSubTotalIndex_Asset++;
                }
                //#endregion
                //#region Reseting the Sub Total Variables
                dblSubTotalAMT_Asset = 0;
                //#endregion
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID_Asset = DataBinder.Eval(e.Row.DataItem, "ACCD2").ToString();


                string ACCNM = DataBinder.Eval(e.Row.DataItem, "ACCDNM3").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCNM;

                decimal AMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMT").ToString());
                string Amount = dbFunctions.SpellAmount.comma(AMT);
                e.Row.Cells[1].Text = Amount;

                // Cumulating Sub Total            
                dblSubTotalAMT_Asset += AMT;
                dblSubTotalAMTComma_Asset = dbFunctions.SpellAmount.comma(dblSubTotalAMT_Asset);

                // Cumulating Grand Total           
                dblGrandTotalAMT_Asset += AMT;
                dblGrandTotalAMTComma_Asset = dbFunctions.SpellAmount.comma(dblGrandTotalAMT_Asset);

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Grand Total : ";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Text = dblGrandTotalAMTComma_Asset;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }
            MakeGridViewPrinterFriendly(GridView1);
        }

        public void showgrid_Liability()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string Date = Session["Date"].ToString();

            DateTime stDate = DateTime.Parse(Date, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string st_DT = stDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("", conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = (@" SELECT A.ACCD1, GL_ACCHART.ACCOUNTNM AS ACCDNM1, A.ACCD2, GL_ACCHART_1.ACCOUNTNM AS ACCDNM2 , GL_ACCHART_2.ACCOUNTNM AS ACCDNM3, 
             A.ACCD3,SUM(ISNULL(A.AMT,0)) AMT  FROM GL_ACCHART INNER JOIN (
             SELECT SUBSTRING(DEBITCD, 1, 1) + '00000000000' AS ACCD1, SUBSTRING(DEBITCD, 1, 3) 
             + '000000000' AS ACCD2,  SUBSTRING(DEBITCD, 1, 5) + '0000000' AS ACCD3, ISNULL(SUM(CREDITAMT) - SUM(DEBITAMT),0) AS AMT 
             FROM GL_MASTER  WHERE (SUBSTRING(DEBITCD, 1, 1) = '2') AND (TRANSDT <= @TRANSDT)  
             GROUP BY SUBSTRING(DEBITCD, 1, 1) + '00000000000', SUBSTRING(DEBITCD, 1, 3) + '000000000', SUBSTRING(DEBITCD, 1, 5) + '0000000'  
             UNION  
             SELECT '2' + '00000000000' AS ACCD1, '201' + '000000000' AS ACCD2, '20102' + '0000000' AS ACCD3, SUM(CREDITAMT) - 
             ISNULL(SUM(DEBITAMT),0) AS AMT  FROM GL_MASTER AS GL_MASTER_1 
             WHERE (SUBSTRING(DEBITCD, 1, 1) IN ('3', '4')) AND (TRANSDT <= @TRANSDT)
             ) AS A  
             ON GL_ACCHART.ACCOUNTCD = A.ACCD1 INNER JOIN GL_ACCHART AS GL_ACCHART_1 ON A.ACCD2 = GL_ACCHART_1.ACCOUNTCD INNER JOIN  
             GL_ACCHART AS GL_ACCHART_2 ON A.ACCD3 = GL_ACCHART_2.ACCOUNTCD 
             GROUP BY A.ACCD1, GL_ACCHART.ACCOUNTNM, A.ACCD2, GL_ACCHART_1.ACCOUNTNM, GL_ACCHART_2.ACCOUNTNM, A.ACCD3");
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@TRANSDT", st_DT);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Rows.Count > 0)
            {
                GridView2.DataSource = ds;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            else
            {
                GridView2.DataSource = ds;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
        }

        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd_Lia = false;
            if ((strPreviousRowID_Lia != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD2") != null))
                if (strPreviousRowID_Lia != DataBinder.Eval(e.Row.DataItem, "ACCD2").ToString())
                    IsSubTotalRowNeedToAdd_Lia = true;
            if ((strPreviousRowID_Lia != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD2") == null))
            {
                IsSubTotalRowNeedToAdd_Lia = true;
                intSubTotalIndex_Lia = 0;
            }
            //#region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID_Lia == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ACCD2") != null))
            {
                GridView GridView2 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCDNM2").ToString(); //////// Sub Header Name
                cell.ColumnSpan = 3;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                GridView2.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_Lia, row);
                intSubTotalIndex_Lia++;
            }
            //#endregion
            if (IsSubTotalRowNeedToAdd_Lia)
            {
                //#region Adding Sub Total Row
                GridView GridView2 = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.HorizontalAlign = HorizontalAlign.Right;
                //cell.ColumnSpan = 2;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);


                //Adding AMT Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalAMTComma_Lia);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                GridView2.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_Lia, row);
                intSubTotalIndex_Lia++;
                //#endregion
                //#region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "ACCD2") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "ACCDNM2").ToString(); //////// Sub Header Name
                    cell.ColumnSpan = 3;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView2.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex_Lia, row);
                    intSubTotalIndex_Lia++;
                }
                //#endregion
                //#region Reseting the Sub Total Variables
                dblSubTotalAMT_Lia = 0;
                //#endregion
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID_Lia = DataBinder.Eval(e.Row.DataItem, "ACCD2").ToString();


                string ACCNM = DataBinder.Eval(e.Row.DataItem, "ACCDNM3").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCNM;

                decimal AMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMT").ToString());
                string Amount = dbFunctions.SpellAmount.comma(AMT);
                e.Row.Cells[1].Text = Amount;

                // Cumulating Sub Total            
                dblSubTotalAMT_Lia += AMT;
                dblSubTotalAMTComma_Lia = dbFunctions.SpellAmount.comma(dblSubTotalAMT_Lia);

                // Cumulating Grand Total           
                dblGrandTotalAMT_Lia += AMT;
                dblGrandTotalAMTComma_Lia = dbFunctions.SpellAmount.comma(dblGrandTotalAMT_Lia);

            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Grand Total : ";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Text = dblGrandTotalAMTComma_Lia;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }
            MakeGridViewPrinterFriendly(GridView2);
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