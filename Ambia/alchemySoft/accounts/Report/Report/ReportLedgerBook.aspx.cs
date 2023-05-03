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

namespace alchemySoft.accounts.Report.Report
{
    public partial class ReportLedgerBook : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
                dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

                lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

                string debitCD = Session["AccCode"].ToString();
                string accNM = Session["AccNM"].ToString();
                string frmDT = Session["From"].ToString();
                string toDT = Session["To"].ToString();
                lblHeadNM.Text = accNM;
                lblFrom.Text = frmDT;
                lblTo.Text = toDT;

                DateTime From = DateTime.Parse(frmDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                DateTime To = DateTime.Parse(toDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string FdT = From.ToString("yyyy/MM/dd");
                string TdT = To.ToString("yyyy/MM/dd");

                dbFunctions.lblAdd(@"SELECT SUM(DEBITAMT) - SUM(CREDITAMT) as OpeningBalance FROM dbo.GL_MASTER WHERE (TRANSDT < '" + FdT + "') AND (DEBITCD = '" + debitCD + "')", lblOpenBal);

                if (lblOpenBal.Text == "")
                {
                    lblOpenBal.Text = "0.00";
                }
                else
                {
                    lblOpenBal.Text = dbFunctions.SpellAmount.comma(Convert.ToDecimal(lblOpenBal.Text));
                }
                //DataTable table = new DataTable();
                //AlchemyAccounting.Accounts.DataAccess.SingleVoucher dob = new DataAccess.SingleVoucher();
                //table = dob.LedgerBook(debitCD, From, To, accNM);
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

            string debitCD = Session["AccCode"].ToString();
            string accNM = Session["AccNM"].ToString();
            string frmDT = Session["From"].ToString();
            string toDT = Session["To"].ToString();

            DateTime From = DateTime.Parse(frmDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime To = DateTime.Parse(toDT, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FdT = From.ToString("yyyy/MM/dd");
            string TdT = To.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT '" + FdT + "' AS FromDate, '" + TdT + "' AS ToDate, GL_MASTER_1.TRANSTP, CONVERT(nvarchar(10), GL_MASTER_1.TRANSDT, 103) AS TRANSDT, GL_MASTER_1.TRANSDT AS TR, GL_MASTER_1.TRANSMY, " +
                                            " CONVERT(nvarchar(10), GL_MASTER_1.TRANSNO, 103) AS TRANSNO, GL_MASTER_1.SERIALNO, GL_MASTER_1.TRANSDRCR, GL_MASTER_1.TRANSFOR, GL_MASTER_1.COSTPID, " +
                                            " GL_MASTER_1.TRANSMODE, GL_MASTER_1.DEBITCD, GL_MASTER_1.CREDITCD, CONVERT(varchar, CONVERT(money, GL_MASTER_1.DEBITAMT), 1) AS DEBITAMT, CONVERT(varchar, CONVERT(money, GL_MASTER_1.CREDITAMT), 1) AS CREDITAMT, " +
                                            " GL_MASTER_1.CHEQUENO, CONVERT(nvarchar(20), GL_MASTER_1.CHEQUEDT, 105) AS CHEQUEDT, GL_MASTER_1.REMARKS, GL_MASTER_1.TABLEID, GL_MASTER_1.USERPC, GL_MASTER_1.USERID, " +
                                            " GL_MASTER_1.ACTDTI, GL_MASTER_1.INTIME, GL_MASTER_1.IPADDRESS, GL_ACCHART_1.ACCOUNTNM AS DebitNM,   ' <b>'+" +
                      " CASE WHEN GL_MASTER_1.DEBITAMT > 0 THEN 'FROM ' + dbo.GL_ACCHART.ACCOUNTNM + ((CASE WHEN GL_MASTER_1.CHEQUENO != '' THEN ' Chq #' + GL_MASTER_1.CHEQUENO + ' ' + CONVERT(nvarchar(20), GL_MASTER_1.CHEQUEDT, 105) WHEN GL_MASTER_1.CHEQUENO = '' THEN '' ELSE '' END)) WHEN GL_MASTER_1.CREDITAMT > 0 THEN 'TO ' + dbo.GL_ACCHART.ACCOUNTNM + ((CASE WHEN GL_MASTER_1.CHEQUENO != '' THEN ' Chq #' + GL_MASTER_1.CHEQUENO + ' ' + CONVERT(nvarchar(20), GL_MASTER_1.CHEQUEDT, 105) WHEN GL_MASTER_1.CHEQUENO = '' THEN '' ELSE '' END)) END + ' </b>' + GL_MASTER_1.REMARKS AS CreditNM, " +
                      " (CASE WHEN GL_MASTER_1.TABLEID = 'GL_STRANS' THEN SUBSTRING(GL_MASTER_1.TABLEID, 4, 1) WHEN GL_MASTER_1.TABLEID = 'GL_MTRANS' THEN SUBSTRING(GL_MASTER_1.TABLEID, 4, 1) WHEN GL_MASTER_1.TABLEID = 'LC_EXPENSE' THEN 'APV ' ELSE 'A' END) + (CASE WHEN GL_MASTER_1.TRANSTP = 'MREC' THEN 'RV ' WHEN GL_MASTER_1.TRANSTP = 'MPAY' THEN 'PV ' WHEN GL_MASTER_1.TRANSTP = 'JOUR' THEN 'JV ' WHEN GL_MASTER_1.TRANSTP = 'CONT' THEN 'CV ' ELSE 'APV ' END)+ CONVERT(nvarchar(10), GL_MASTER_1.TRANSNO, 103) AS DOCNO" +
                      " FROM dbo.GL_ACCHART INNER JOIN dbo.GL_MASTER AS GL_MASTER_1 LEFT OUTER JOIN dbo.GL_ACCHART AS GL_ACCHART_1 ON GL_MASTER_1.DEBITCD = GL_ACCHART_1.ACCOUNTCD ON " +
                      " dbo.GL_ACCHART.ACCOUNTCD = GL_MASTER_1.CREDITCD WHERE (GL_MASTER_1.TRANSDT BETWEEN '" + FdT + "' AND '" + TdT + "') AND (GL_MASTER_1.DEBITCD = '" + debitCD + "') AND GL_MASTER_1.DEBITAMT <> GL_MASTER_1.CREDITAMT" +
            " ORDER BY TR, TRANSMY DESC, TRANSDT, GL_MASTER_1.TRANSTP DESC, TRANSNO, GL_MASTER_1.SERIALNO, DEBITAMT DESC", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;

                Balance();

                Decimal totDebitAmnt = 0;
                Decimal a = 0;
                foreach (GridViewRow grid in GridView1.Rows)
                {
                    string Debit = grid.Cells[3].Text;
                    totDebitAmnt = Convert.ToDecimal(Debit);
                    a += totDebitAmnt;
                    decimal tot = a;
                    lblPeriodicDB.Text = dbFunctions.SpellAmount.comma(tot);
                }
                //a += totDebitAmnt;

                Decimal totCreditAmnt = 0;
                Decimal b = 0;
                foreach (GridViewRow grid in GridView1.Rows)
                {
                    string Credit = grid.Cells[4].Text;
                    totCreditAmnt = Convert.ToDecimal(Credit);
                    b += totCreditAmnt;
                    decimal tot = b;
                    lblPeriodicCR.Text = dbFunctions.SpellAmount.comma(tot);
                }
                //b += totCreditAmnt;

                decimal totPeriodicDB = decimal.Parse(lblPeriodicDB.Text);
                decimal totPeriodicCR = decimal.Parse(lblPeriodicCR.Text);
                decimal totPeriodic = totPeriodicDB - totPeriodicCR;
                lblPeriodicBalance.Text = dbFunctions.SpellAmount.comma(totPeriodic);



                lblTotCR.Text = lblPeriodicCR.Text;
                decimal TotCR = decimal.Parse(lblTotCR.Text);
                decimal LastCumBalance = decimal.Parse(lblLastCumBalance.Text);
                decimal TotBalance = TotCR + LastCumBalance;
                lblTotBalance.Text = dbFunctions.SpellAmount.comma(TotBalance);

            }
            else
            {
                //dbFunctions.showMessage(Page,"No Data Found");
                //GridView1.Visible = false;
                lblLastCumBalance.Text = lblOpenBal.Text;
                lblPeriodicBalance.Text = "0.00";
                lblPeriodicCR.Text = "0.00";
                lblPeriodicDB.Text = "0.00";
                lblTotCR.Text = "0.00";
                lblTotBalance.Text = lblLastCumBalance.Text;
            }
        }

        public void Balance()
        {
            try
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        string Debit = GridView1.Rows[i].Cells[3].Text;
                        decimal DbAmt = Convert.ToDecimal(Debit);
                        string Credit = GridView1.Rows[i].Cells[4].Text;
                        decimal CdAmt = Convert.ToDecimal(Credit);
                        decimal OpenBal = Convert.ToDecimal(lblOpenBal.Text);
                        decimal Balance = OpenBal + DbAmt - CdAmt;
                        GridView1.Rows[i].Cells[5].Text = dbFunctions.SpellAmount.comma(Balance);
                        lblLastCumBalance.Text = GridView1.Rows[i].Cells[5].Text;
                    }
                    else
                    {
                        string BlnC = GridView1.Rows[i - 1].Cells[5].Text;
                        decimal CumulativeBalance = decimal.Parse(BlnC);

                        string Debit = GridView1.Rows[i].Cells[3].Text;
                        decimal DbAmt = Convert.ToDecimal(Debit);
                        string Credit = GridView1.Rows[i].Cells[4].Text;
                        decimal CdAmt = Convert.ToDecimal(Credit);
                        decimal Balance = CumulativeBalance + DbAmt - CdAmt;
                        GridView1.Rows[i].Cells[5].Text = dbFunctions.SpellAmount.comma(Balance);
                        lblLastCumBalance.Text = GridView1.Rows[i].Cells[5].Text;
                    }
                }
            }
            catch (Exception ex)
            {
                dbFunctions.showMessage(Page,ex.Message);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    string TRANSDT = DataBinder.Eval(e.Row.DataItem, "TRANSDT").ToString();
                    e.Row.Cells[0].Text = TRANSDT;

                    string DOCNO = DataBinder.Eval(e.Row.DataItem, "DOCNO").ToString();
                    e.Row.Cells[1].Text = DOCNO;

                    string CreditNM = DataBinder.Eval(e.Row.DataItem, "CreditNM").ToString();
                    e.Row.Cells[2].Text = "&nbsp;" + CreditNM;

                    decimal DEBITAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DEBITAMT").ToString());
                    string DRAMT = dbFunctions.SpellAmount.comma(DEBITAMT);
                    e.Row.Cells[3].Text = DRAMT;

                    decimal CREDITAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CREDITAMT").ToString());
                    string CRAMT = dbFunctions.SpellAmount.comma(CREDITAMT);
                    e.Row.Cells[4].Text = CRAMT;
                }
                catch (Exception ex)
                {
                    dbFunctions.showMessage(Page,ex.Message);
                }
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