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
using System.Collections;

namespace alchemySoft.accounts.Report.Report
{
    public partial class rptTrailBalanceDetails : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        decimal dblGrandTotalOPDRAmount = 0;
        decimal dblGrandTotalOPCRAmount = 0;
        decimal dblGrandTotalFRDRAmount = 0;
        decimal dblGrandTotalTOCRAmount = 0;
        decimal dblGrandTotalCLDRAmount = 0;
        decimal dblGrandTotalCLCRAmount = 0;

        string dblGrandTotalOPDRAmountComma = "";
        string dblGrandTotalOPCRAmountComma = "";
        string dblGrandTotalFRDRAmountComma = "";
        string dblGrandTotalTOCRAmountComma = "";
        string dblGrandTotalCLDRAmountComma = "";
        string dblGrandTotalCLCRAmountComma = "";

        [Serializable]
        private class MergedColumnsInfo
        {
            // indexes of merged columns
            public List<int> MergedColums = new List<int>();
            // key-value pairs: key = the first column index, value = number of the merged columns
            public Hashtable StartColumns = new Hashtable();
            // key-value pairs: key = the first column index, value = common title of the merged columns 
            public Hashtable Titles = new Hashtable();

            //parameters: the merged columns indexes, common title of the merged columns 
            public void AddMergedColumns(int[] columnsIndexes, string title)
            {
                MergedColums.AddRange(columnsIndexes);
                StartColumns.Add(columnsIndexes[0], columnsIndexes.Length);
                Titles.Add(columnsIndexes[0], title);
            }
        }

        //property for storing of information about merged columns
        private MergedColumnsInfo info
        {
            get
            {
                if (ViewState["info"] == null)
                    ViewState["info"] = new MergedColumnsInfo();
                return (MergedColumnsInfo)ViewState["info"];
            }
        }

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
protected void Page_Load(object sender, EventArgs e)
        {
            dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
            dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

            lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

            string fromDT =  Session["FromDate"].ToString();
            string toDT = Session["ToDate"].ToString();
            DateTime FDate = DateTime.Parse(fromDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblFrom.Text = FDate.ToString("dd-MMM-yyyy");

            DateTime TDate = DateTime.Parse(toDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblTo.Text = TDate.ToString("dd-MMM-yyyy");

            //merge the second, third and fourth, fifth and sixth columns
            info.AddMergedColumns(new int[] { 1, 2 }, "OPENING");
            info.AddMergedColumns(new int[] { 3, 4 }, "DATE PERIOD");
            info.AddMergedColumns(new int[] { 5, 6 }, "CLOSING");
            showGrid();
        }

        public void showGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string fromDT = Session["FromDate"].ToString();
            DateTime FDT = DateTime.Parse(fromDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FromDT = FDT.ToString("yyyy/MM/dd");
            string toDT = Session["ToDate"].ToString();
            DateTime TDT = DateTime.Parse(toDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDT.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TOT.DEBITCD, GL_ACCHART.ACCOUNTNM, SUM(TOT.OPDEBIT) AS OPDEBIT , SUM(TOT.OPCREDIT) AS OPCREDIT, SUM(TOT.FRDEBIT) AS FRDEBIT, SUM(TOT.TOCREDIT) AS TOCREDIT, SUM(TOT.CLDEBIT) AS CLDEBIT, SUM(TOT.CLCREDIT) AS CLCREDIT " +
                                            " FROM (SELECT     DEBITCD, DEBIT AS OPDEBIT, CREDIT AS OPCREDIT, 0 AS FRDEBIT, 0 AS TOCREDIT, 0 AS CLDEBIT, 0 AS CLCREDIT " +
                                            " FROM (SELECT     DEBITCD, (CASE WHEN a.BAMT > 0 THEN a.BAMT ELSE 0 END) AS DEBIT, (CASE WHEN a.BAMT < 0 THEN a.BAMT * - 1 ELSE 0 END) AS CREDIT " +
                                            " FROM          (SELECT     DEBITCD, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS BAMT " +
                                                                       " FROM          GL_MASTER " +
                                                                       " WHERE      (SUBSTRING(DEBITCD, 1, 1) IN ('1', '4')) AND (TRANSDT < '" + FromDT + "') " +
                                                                       " GROUP BY DEBITCD) AS a " +
                                               " UNION " +
                                               " SELECT     DEBITCD, (CASE WHEN b.BAMT < 0 THEN b.BAMT * - 1 ELSE 0 END) AS DEBIT, (CASE WHEN b.BAMT > 0 THEN B.BAMT ELSE 0 END) AS CREDIT " +
                                               " FROM         (SELECT     DEBITCD, SUM(ISNULL(CREDITAMT, 0)) - SUM(ISNULL(DEBITAMT, 0)) AS BAMT " +
                                                                       " FROM          GL_MASTER AS GL_MASTER_1 " +
                                                                       " WHERE      (SUBSTRING(DEBITCD, 1, 1) IN ('2', '3')) AND (TRANSDT < '" + FromDT + "') " +
                                                                       " GROUP BY DEBITCD) AS b) AS OP " + 
                        " UNION " +
                        " SELECT     DEBITCD, 0 AS OPDEBIT, 0 AS OPCREDIT, DEBIT AS FRDEBIT, CREDIT AS TOCREDIT, 0 AS CLDEBIT, 0 AS CLCREDIT " +
                        " FROM         (SELECT     DEBITCD, (CASE WHEN a_2.BAMT > 0 THEN a_2.BAMT ELSE 0 END) AS DEBIT, (CASE WHEN a_2.BAMT < 0 THEN a_2.BAMT * - 1 ELSE 0 END) AS CREDIT " +
                                              " FROM          (SELECT     DEBITCD, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS BAMT " +
                                                                      " FROM          GL_MASTER AS GL_MASTER_3 " +
                                                                      " WHERE      (SUBSTRING(DEBITCD, 1, 1) IN ('1', '4')) AND (TRANSDT BETWEEN '" + FromDT + "' AND '" + ToDT + "') " +
                                                                      " GROUP BY DEBITCD) AS a_2 " +
                                              " UNION " +
                                              " SELECT     DEBITCD, (CASE WHEN b_2.BAMT < 0 THEN b_2.BAMT * - 1 ELSE 0 END) AS DEBIT, (CASE WHEN b_2.BAMT > 0 THEN b_2.BAMT ELSE 0 END) " +
                                                                     " AS CREDIT " +
                                              " FROM         (SELECT     DEBITCD, SUM(ISNULL(CREDITAMT, 0)) - SUM(ISNULL(DEBITAMT, 0)) AS BAMT " +
                                                                     " FROM          GL_MASTER AS GL_MASTER_1 " +
                                                                     " WHERE      (SUBSTRING(DEBITCD, 1, 1) IN ('2', '3')) AND (TRANSDT BETWEEN '" + FromDT + "' AND '" + ToDT + "') " +
                                                                     " GROUP BY DEBITCD) AS b_2) AS DT " +
                        " UNION " +
                        " SELECT     DEBITCD, 0 AS OPDEBIT, 0 AS OPCREDIT, 0 AS FRDEBIT, 0 AS TOCREDIT, DEBIT AS CLDEBIT, CREDIT AS CLCREDIT " +
                        " FROM         (SELECT     DEBITCD, (CASE WHEN a_1.BAMT > 0 THEN a_1.BAMT ELSE 0 END) AS DEBIT, (CASE WHEN a_1.BAMT < 0 THEN a_1.BAMT * - 1 ELSE 0 END) AS CREDIT " +
                                               " FROM          (SELECT     DEBITCD, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS BAMT " +
                                                                      " FROM          GL_MASTER AS GL_MASTER_2 " +
                                                                      " WHERE      (SUBSTRING(DEBITCD, 1, 1) IN ('1', '4')) AND (TRANSDT <= '" + ToDT + "') " +
                                                                      " GROUP BY DEBITCD) AS a_1 " +
                                              " UNION " +
                                              " SELECT     DEBITCD, (CASE WHEN b_1.BAMT < 0 THEN b_1.BAMT * - 1 ELSE 0 END) AS DEBIT, (CASE WHEN b_1.BAMT > 0 THEN b_1.BAMT ELSE 0 END) AS CREDIT " +
                                              " FROM         (SELECT     DEBITCD, SUM(ISNULL(CREDITAMT, 0)) - SUM(ISNULL(DEBITAMT, 0)) AS BAMT " +
                                                                     " FROM          GL_MASTER AS GL_MASTER_1 " +
                                                                     " WHERE      (SUBSTRING(DEBITCD, 1, 1) IN ('2', '3')) AND (TRANSDT <= '" + ToDT + "') " +
                                                                     " GROUP BY DEBITCD) AS b_1) AS CL) AS TOT INNER JOIN " +
                      " GL_ACCHART ON TOT.DEBITCD = GL_ACCHART.ACCOUNTCD GROUP BY TOT.DEBITCD, GL_ACCHART.ACCOUNTNM", conn);

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
            //call the method for custom rendering the columns headers	
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.SetRenderMethodDelegate(RenderHeader);
        }

        //method for rendering the columns headers	
        private void RenderHeader(HtmlTextWriter output, Control container)
        {
            for (int i = 0; i < container.Controls.Count; i++)
            {
                TableCell cell = (TableCell)container.Controls[i];
                //stretch non merged columns for two rows
                if (!info.MergedColums.Contains(i))
                {
                    cell.Attributes["rowspan"] = "2";
                    cell.RenderControl(output);
                }
                else //render merged columns common title
                    if (info.StartColumns.Contains(i))
                    {
                        output.Write(string.Format("<th align='center' colspan='{0}'>{1}</th>",
                                 info.StartColumns[i], info.Titles[i]));
                    }
            }

            //close the first row	
            output.RenderEndTag();
            //set attributes for the second row
            GridView1.HeaderStyle.AddAttributesToRender(output);
            //start the second row
            output.RenderBeginTag("tr");

            //render the second row (only the merged columns)
            for (int i = 0; i < info.MergedColums.Count; i++)
            {
                TableCell cell = (TableCell)container.Controls[info.MergedColums[i]];
                cell.RenderControl(output);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string PARTICULARS = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                string ACCCD = DataBinder.Eval(e.Row.DataItem, "DEBITCD").ToString();


                string from = Session["ToDate"].ToString();
                PARTICULARS = "<a href='ReportDdaFromAnother.aspx?AccNM=" + PARTICULARS + "&AccCode=" + ACCCD + "&From=" + from + "' target ='_blank'>" + PARTICULARS + "</a>";


                e.Row.Cells[0].Text = "&nbsp;" + PARTICULARS;

                decimal DRAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OPDEBIT").ToString());
                string DRAmnt = dbFunctions.SpellAmount.comma(DRAMT);
                e.Row.Cells[1].Text = DRAmnt + "&nbsp;";

                decimal CRAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OPCREDIT").ToString());
                string CRAmnt = dbFunctions.SpellAmount.comma(CRAMT);
                e.Row.Cells[2].Text = CRAmnt + "&nbsp;";

                decimal FRDR = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FRDEBIT").ToString());
                string FRDRAmnt = dbFunctions.SpellAmount.comma(FRDR);
                e.Row.Cells[3].Text = FRDRAmnt + "&nbsp;";

                decimal TOCR = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TOCREDIT").ToString());
                string TOCRAmnt = dbFunctions.SpellAmount.comma(TOCR);
                e.Row.Cells[4].Text = TOCRAmnt + "&nbsp;";

                decimal CLDR = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CLDEBIT").ToString());
                string CLDRAmnt = dbFunctions.SpellAmount.comma(CLDR);
                e.Row.Cells[5].Text = CLDRAmnt + "&nbsp;";

                decimal CLCR = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CLCREDIT").ToString());
                string CLCRAmnt = dbFunctions.SpellAmount.comma(CLCR);
                e.Row.Cells[6].Text = CLCRAmnt + "&nbsp;";


                dblGrandTotalOPDRAmount += DRAMT;
                dblGrandTotalOPDRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalOPDRAmount);

                dblGrandTotalOPCRAmount += CRAMT;
                dblGrandTotalOPCRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalOPCRAmount);

                dblGrandTotalFRDRAmount += FRDR;
                dblGrandTotalFRDRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalFRDRAmount);

                dblGrandTotalTOCRAmount += TOCR;
                dblGrandTotalTOCRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalTOCRAmount);

                dblGrandTotalCLDRAmount += CLDR;
                dblGrandTotalCLDRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalCLDRAmount);

                dblGrandTotalCLCRAmount += CLCR;
                dblGrandTotalCLCRAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalCLCRAmount);
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL :";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Text = dblGrandTotalOPDRAmountComma;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = dblGrandTotalOPCRAmountComma;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = dblGrandTotalFRDRAmountComma;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].Text = dblGrandTotalTOCRAmountComma;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].Text = dblGrandTotalCLDRAmountComma;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].Text = dblGrandTotalCLCRAmountComma;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
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