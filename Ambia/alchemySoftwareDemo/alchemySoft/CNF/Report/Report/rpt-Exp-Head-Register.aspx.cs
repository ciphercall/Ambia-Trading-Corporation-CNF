using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.CNF.Report.Report
{
    public partial class rpt_Exp_Head_Register : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        decimal totAmount = 0;

        string totAmountComma = "0";
        string ttAmt = "0";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }

            else
            {
                if (!IsPostBack)
                {

                    ShowGrid();
                }
            }
        }

        public void ShowGrid()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string fromDate = Session["fromdate"].ToString();
            DateTime FRDT = DateTime.Parse(fromDate, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FDT = FRDT.ToString("yyyy-MM-dd");

            DateTime dt = dbFunctions.timezone(DateTime.Now);

            lblPrintDate.Text = dt.ToString("yyyy-MM-dd");

            lblFromdate.Text = fromDate;

            string todate = Session["todate"].ToString();
            DateTime TODT = DateTime.Parse(todate, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string TDT = TODT.ToString("yyyy-MM-dd");
            lblTodate.Text = todate;

            string expid = Session["expenseID"].ToString();

            dbFunctions.lblAdd("select EXPNM from CNF_EXPENSE where EXPID='" + expid + "'", lblExpenseNM);
            lblExpenseID.Text = expid;

            SqlCommand cmd = new SqlCommand(@"SELECT  ROW_NUMBER() OVER(ORDER BY CNF_JOBEXP.JOBTP) AS SL, CONVERT(NVARCHAR(20),CNF_JOBEXP.TRANSDT ,103) AS TRANSD,
                CNF_JOBEXP.JOBNO, CNF_JOBEXP.JOBTP, CNF_JOBEXP.JOBYY, CNF_JOBEXP.EXPAMT, GL_ACCHART.ACCOUNTNM, ASL_BRANCH.BRANCHID FROM  CNF_JOBEXP 
                INNER JOIN CNF_JOB ON CNF_JOBEXP.JOBYY = CNF_JOB.JOBYY AND CNF_JOBEXP.JOBTP = CNF_JOB.JOBTP AND CNF_JOBEXP.JOBNO = CNF_JOB.JOBNO 
                INNER JOIN GL_ACCHART ON CNF_JOB.PARTYID = GL_ACCHART.ACCOUNTCD INNER JOIN ASL_BRANCH ON CNF_JOBEXP.COMPID = ASL_BRANCH.BRANCHCD
                WHERE CNF_JOBEXP.TRANSDT BETWEEN @FROMDATE AND @TODATE AND CNF_JOBEXP.EXPID='" + expid + "'", conn);

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@FROMDATE", FDT);
            cmd.Parameters.AddWithValue("@TODATE", TDT);
            if (conn.State != ConnectionState.Open) conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvReport.DataSource = ds;
                gvReport.DataBind();
                gvReport.Visible = true;
            }
            else
            {
                gvReport.DataSource = ds;
                gvReport.DataBind();
                gvReport.Visible = true;
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string SL = DataBinder.Eval(e.Row.DataItem, "SL").ToString();
                e.Row.Cells[0].Text = SL;

                string TRANSD = DataBinder.Eval(e.Row.DataItem, "TRANSD").ToString();
                e.Row.Cells[1].Text = TRANSD;

                string JOBNO = DataBinder.Eval(e.Row.DataItem, "JOBNO").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + JOBNO;

                string JOBYY = DataBinder.Eval(e.Row.DataItem, "JOBYY").ToString();
                e.Row.Cells[3].Text = JOBYY;

                string JOBTP = DataBinder.Eval(e.Row.DataItem, "JOBTP").ToString();
                e.Row.Cells[4].Text = JOBTP;

                string BRANCHID = DataBinder.Eval(e.Row.DataItem, "BRANCHID").ToString();
                e.Row.Cells[5].Text = BRANCHID;

                string ACCOUNTNM = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[6].Text = "&nbsp;" + ACCOUNTNM;

                decimal EXPAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "EXPAMT").ToString());
                string Amnt = dbFunctions.SpellAmount.comma(EXPAMT);
                e.Row.Cells[7].Text = Amnt + "&nbsp;";

                totAmount += EXPAMT;
                ttAmt = totAmount.ToString();
                totAmountComma = dbFunctions.SpellAmount.comma(totAmount);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = "Total : ";
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].Text = totAmountComma;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;

                lblInWords.Text = "";
                decimal dec;
                Boolean ValidInput = Decimal.TryParse(ttAmt, out dec);
                if (!ValidInput)
                {
                    lblInWords.ForeColor = System.Drawing.Color.Red;
                    lblInWords.Text = "Enter the Proper Amount...";
                    return;
                }
                if (ttAmt.ToString().Trim() == "")
                {
                    lblInWords.ForeColor = System.Drawing.Color.Red;
                    lblInWords.Text = "Amount Cannot Be Empty...";
                    return;
                }
                else
                {
                    if (Convert.ToDecimal(ttAmt) == 0)
                    {
                        lblInWords.ForeColor = System.Drawing.Color.Red;
                        lblInWords.Text = "Amount Cannot Be Empty...";
                        return;
                    }
                }

                string x1 = "";
                string x2 = "";

                if (ttAmt.Contains("."))
                {
                    x1 = ttAmt.ToString().Trim().Substring(0, ttAmt.ToString().Trim().IndexOf("."));
                    x2 = ttAmt.ToString().Trim().Substring(ttAmt.ToString().Trim().IndexOf(".") + 1);
                }
                else
                {
                    x1 = ttAmt.ToString().Trim();
                    x2 = "00";
                }

                if (x1.ToString().Trim() != "")
                {
                    x1 = Convert.ToInt64(x1.Trim()).ToString().Trim();
                }
                else
                {
                    x1 = "0";
                }

                ttAmt = x1 + "." + x2;

                if (x2.Length > 2)
                {
                    ttAmt = Math.Round(Convert.ToDouble(ttAmt), 2).ToString().Trim();
                }

                string AmtConv = dbFunctions.SpellAmount.MoneyConvFn(ttAmt.ToString().Trim());

                lblInWords.Text = AmtConv.Trim();

            }
            ShowHeader(gvReport);
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