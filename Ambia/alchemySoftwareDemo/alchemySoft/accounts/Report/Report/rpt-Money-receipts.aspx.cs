using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlchemyAccounting;
using alchemySoft;

namespace DynamicMenu.Accounts.Report.Report
{
    public partial class rpt_Money_receipts : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        decimal dblGrandTotalDRAmount = 0;

        string dblGrandTotalDRAmountComma = "";

        string strPreviousRowID = string.Empty;

        int intSubTotalIndex = 1;

        decimal subTotalAmt = 0;
        string subTotalAmtComma = "0";


        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
                dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

                lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

                ShowGrid();
            }
        }

        public void ShowGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string frmDT = Session["From"].ToString();
            string toDT = Session["To"].ToString();
            string acccode = Session["AccCode"].ToString();
            acccode = acccode.Substring(0, 7);
            lblfdt.Text = frmDT;
            lbltdt.Text = toDT;
            DateTime From = DateTime.Parse(frmDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime To = DateTime.Parse(toDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FdT = From.ToString("yyyy/MM/dd");
            string TdT = To.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT UPPER(REPLACE(CONVERT(NVARCHAR(10),TRANSDT,103),'/','.')) AS TRANSDTT, ACCOUNTNM CREDITNM, ACCOUNTCD, 
            (CASE WHEN ISNULL(TRANSMODE,'A') = 'CASH' THEN 'CASH' WHEN ISNULL(TRANSMODE,'A') <> 'CASH' 
            THEN TRANSMODE+' NO: '+CHEQUENO+' DT: '+CONVERT(NVARCHAR(10),CHEQUEDT,103) END) RCVTP, 
            REMARKS, DEBITAMT FROM GL_MASTER A
            INNER JOIN GL_ACCHART B ON A.CREDITCD = B.ACCOUNTCD
            WHERE TRANSTP = 'MREC'  AND TRANSDT BETWEEN @fdt AND @tdt 
            AND TRANSDRCR = 'DEBIT' AND SUBSTRING(CREDITCD,1,7) = @acc
            ORDER BY TRANSDT, TRANSNO, SERIALNO", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@fdt", FdT);
            cmd.Parameters.AddWithValue("@tdt", TdT);
            cmd.Parameters.AddWithValue("@acc", acccode);
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

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd = false;
            //bool IsGrandTotalRowNeedtoAdd = false;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "TRANSDTT") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "TRANSDTT").ToString())
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "TRANSDTT") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                //IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }

            #region Inserting first Row and populating fist Group Header details

            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "TRANSDTT") != null))
            {
                GridView GridView1 = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "TRANSDTT").ToString();
                cell.ColumnSpan = 4;
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
                cell.ColumnSpan = 3;
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

                if (DataBinder.Eval(e.Row.DataItem, "TRANSDTT") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "&nbsp;" + "&nbsp;" + "&nbsp;" + DataBinder.Eval(e.Row.DataItem, "TRANSDTT").ToString();
                    cell.ColumnSpan = 4;
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
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "TRANSDTT").ToString();

                string ACCOUNTCD = DataBinder.Eval(e.Row.DataItem, "ACCOUNTCD").ToString();
                string CREDITNM = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();

                string from = Session["To"].ToString();
                CREDITNM = "<a href='ReportDdaFromAnother.aspx?AccNM=" + CREDITNM + "&AccCode=" + ACCOUNTCD + "&From=" + from + "' target ='_blank'>" + CREDITNM + "</a>";

                e.Row.Cells[0].Text = "&nbsp;" + CREDITNM;

                string RCVTP = DataBinder.Eval(e.Row.DataItem, "RCVTP").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + RCVTP;

                string REMARKS = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + REMARKS;

                decimal DEBITAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());
                string DRAmnt = dbFunctions.SpellAmount.comma(DEBITAMT);
                e.Row.Cells[3].Text = DRAmnt + "&nbsp;";

                decimal DBAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());

                subTotalAmt += DBAmount;
                subTotalAmtComma = dbFunctions.SpellAmount.comma(subTotalAmt);


                dblGrandTotalDRAmount += DBAmount;
                dblGrandTotalDRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalDRAmount);

            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "TOTAL :";
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = dblGrandTotalDRAmountComma;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
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
    }
}