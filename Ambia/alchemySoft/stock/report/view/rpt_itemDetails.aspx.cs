using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.stock.report.view
{
    public partial class rpt_itemDetails : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        // To keep track of the previous row Group Identifier    
        string strPreviousRowID = string.Empty;
        // To keep track the Index of Group Total    
        int intSubTotalIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string formLink = "/stock/ui/wastage";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY", lblCompNM);
                        dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY", lblAddress);

                        DateTime PrintDate = DateTime.Now;
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm tt");
                        lblTime.Text = td;

                        showGrid();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        public void showGrid()
        {
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT STK_ITEMMST.CATID, STK_ITEMMST.CATNM,STK_ITEM.ITEMCD,STK_ITEM.COLOR,STK_ITEM.BUYRT,STK_ITEM.SALERT, STK_ITEM.ITEMID, STK_ITEM.ITEMNM  " +
                                            " FROM STK_ITEMMST INNER JOIN STK_ITEM ON STK_ITEMMST.CATID = STK_ITEM.CATID", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "No Data Found.";
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
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "CATID") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "CATID").ToString())
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "CATID") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                //    IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }
            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "CATID") != null))
            {
                GridView GridView1 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "Category Particulars : " + DataBinder.Eval(e.Row.DataItem, "CATNM").ToString();
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
                //    cell.Text = "Category Wise Total";
                //    cell.HorizontalAlign = HorizontalAlign.Left;
                //    //cell.ColumnSpan = 2;
                //    cell.CssClass = "SubTotalRowStyle";
                //    row.Cells.Add(cell);

                //    //Adding Carton Column         
                //    cell = new TableCell();
                //    cell.Text = string.Format("{0:0.00}", dblSubTotalCartonQtyComma);
                //    cell.HorizontalAlign = HorizontalAlign.Right;
                //    cell.CssClass = "SubTotalRowStyle";
                //    row.Cells.Add(cell);

                //    //Adding Pieces Column         
                //    cell = new TableCell();
                //    cell.Text = string.Format("{0:0.00}", dblSubTotalPiecesComma);
                //    cell.HorizontalAlign = HorizontalAlign.Right;
                //    cell.CssClass = "SubTotalRowStyle";
                //    row.Cells.Add(cell);

                //    //Adding CLQTY Column         
                //    cell = new TableCell();
                //    cell.Text = string.Format("{0:0.00}", dblSubTotalCLQtyComma);
                //    cell.HorizontalAlign = HorizontalAlign.Right;
                //    cell.CssClass = "SubTotalRowStyle";
                //    row.Cells.Add(cell);

                //    //Adding Amount Column         
                //    cell = new TableCell();
                //    cell.Text = string.Format("{0:0.00}", dblSubTotalRateComma);
                //    cell.HorizontalAlign = HorizontalAlign.Right;
                //    cell.CssClass = "SubTotalRowStyle";
                //    row.Cells.Add(cell);

                //    //Adding Amount Column         
                //    cell = new TableCell();
                //    cell.Text = string.Format("{0:0.00}", dblSubTotalAmountComma);
                //    cell.HorizontalAlign = HorizontalAlign.Right;
                //    cell.CssClass = "SubTotalRowStyle";
                //    row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                //#endregion
                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "CATID") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "Category Particulars : " + DataBinder.Eval(e.Row.DataItem, "CATNM").ToString();
                    cell.ColumnSpan = 6;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                }
                #endregion
                //    #region Reseting the Sub Total Variables
                //    dblSubTotalCartonQty = 0;
                //    dblSubTotalPieces = 0;
                //    dblSubTotalCLQty = 0;
                //    dblSubTotalAmount = 0;
                //    #endregion
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
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "CATID").ToString();


                string ItemNM = DataBinder.Eval(e.Row.DataItem, "ITEMNM").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ItemNM;

             


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

        string parseValueIntoCurrency(double number)
        {
            // set currency format
            string curCulture = Thread.CurrentThread.CurrentCulture.ToString();
            System.Globalization.NumberFormatInfo currencyFormat = new
                System.Globalization.CultureInfo(curCulture).NumberFormat;

            currencyFormat.CurrencyNegativePattern = 1;

            return number.ToString("c", currencyFormat);
        }
    }
}