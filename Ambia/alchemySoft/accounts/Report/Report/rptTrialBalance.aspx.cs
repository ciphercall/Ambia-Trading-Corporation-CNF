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
    public partial class rptTrialBalance : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        decimal dblGrandTotalDRAmount = 0;
        decimal dblGrandTotalCRAmount = 0;

        string dblGrandTotalDRAmountComma = "";
        string dblGrandTotalCRAmountComma = "";

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
            dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

            lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];
            string PDT = Session["Date"].ToString();
            DateTime FDate = DateTime.Parse(PDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblDate.Text = FDate.ToString("dd-MMM-yyyy");

            showGrid();
        }

        public void showGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string PDT = Session["Date"].ToString();
            DateTime P_Date = DateTime.Parse(PDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string DT = P_Date.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT        C.DEBITCD, GL_ACCHART.ACCOUNTNM, C.DEBIT, C.CREDIT
            FROM(SELECT        DEBITCD, (CASE WHEN a.BAMT > 0 THEN a.BAMT ELSE 0 END) AS DEBIT, (CASE WHEN a.BAMT < 0
            THEN a.BAMT * -1 ELSE 0 END) AS CREDIT
            FROM(SELECT        DEBITCD, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS BAMT
            FROM            GL_MASTER
            WHERE(SUBSTRING(DEBITCD, 1, 1) IN('1', '4')) AND(TRANSDT <= @TRANSDT)
            GROUP BY DEBITCD) AS a
            UNION
            SELECT        DEBITCD, (CASE WHEN b.BAMT < 0 THEN b.BAMT * -1 ELSE 0 END) AS DEBIT, (CASE WHEN b.BAMT > 0
            THEN B.BAMT ELSE 0 END) AS CREDIT
            FROM(SELECT        DEBITCD, SUM(ISNULL(CREDITAMT, 0)) - SUM(ISNULL(DEBITAMT, 0)) AS BAMT
            FROM            GL_MASTER AS GL_MASTER_1
            WHERE(SUBSTRING(DEBITCD, 1, 1) IN('2', '3')) AND(TRANSDT <= @TRANSDT)
            GROUP BY DEBITCD) AS b) AS C INNER JOIN
            GL_ACCHART ON C.DEBITCD = GL_ACCHART.ACCOUNTCD  ORDER BY  C.DEBITCD", conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@TRANSDT", DT);
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ACCCD = DataBinder.Eval(e.Row.DataItem, "DEBITCD").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + ACCCD;

                string PARTICULARS = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();

                string from = Session["Date"].ToString();
                PARTICULARS = "<a href='ReportDdaFromAnother.aspx?AccNM=" + PARTICULARS + "&AccCode=" + ACCCD + "&From=" + from + "' target ='_blank'>" + PARTICULARS + "</a>";

                e.Row.Cells[1].Text = "&nbsp;" + PARTICULARS;

                decimal DRAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBIT").ToString());
                string DRAmnt = dbFunctions.SpellAmount.comma(DRAMT);
                e.Row.Cells[2].Text = DRAmnt + "&nbsp;";

                decimal DBAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBIT").ToString());

                decimal CRAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CREDIT").ToString());
                string CRAmnt = dbFunctions.SpellAmount.comma(CRAMT);
                e.Row.Cells[3].Text = CRAmnt + "&nbsp;";

                decimal CRAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CREDIT").ToString());

                dblGrandTotalDRAmount += DBAmount;
                dblGrandTotalDRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalDRAmount);

                dblGrandTotalCRAmount += CRAmount;
                dblGrandTotalCRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalCRAmount);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "TOTAL :";
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = dblGrandTotalDRAmountComma;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = dblGrandTotalCRAmountComma;
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