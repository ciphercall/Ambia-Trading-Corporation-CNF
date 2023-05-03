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
using System.Collections;

namespace alchemySoft.accounts.Report.Report
{
    public partial class rptProjectExpense : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        decimal totAmount = 0;

        string totAmountComma = "0";

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
            dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

            lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

            string ProjectNM = Session["ProjectNM"].ToString();
            string From = Session["From"].ToString();
            string To = Session["To"].ToString();

            DateTime FDate = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblFrom.Text = FDate.ToString("dd-MMM-yyyy");
            DateTime TDate = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblTo.Text = TDate.ToString("dd-MMM-yyyy");

            lblProjectName.Text = ProjectNM;

            showGrid();
        }

        public void showGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string typeCD = Session["Typecd"].ToString();
            string typeNM = Session["TypeName"].ToString();

            lblType.Text = typeNM;
            string ProjectCD = Session["ProjectCD"].ToString();
            string From = Session["From"].ToString();
            DateTime FDT = DateTime.Parse(From, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FromDT = FDT.ToString("yyyy-MM-dd");

            string To = Session["To"].ToString();
            DateTime TDT = DateTime.Parse(To, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string ToDT = TDT.ToString("yyyy-MM-dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@" SELECT GL_MASTER.DEBITCD, A.ACCOUNTNM, GL_MASTER.CREDITCD, B.ACCOUNTNM CREDITNM, SUM(GL_MASTER.DEBITAMT) AS AMOUNT FROM GL_MASTER 
            INNER JOIN GL_ACCHART A ON GL_MASTER.DEBITCD  = A.ACCOUNTCD 
            INNER JOIN GL_ACCHART B ON GL_MASTER.CREDITCD = B.ACCOUNTCD WHERE (GL_MASTER.COSTPID = '" + ProjectCD + "') " +
            "AND (GL_MASTER.TRANSDT BETWEEN '" + FromDT + "' AND '" + ToDT + "')  AND (GL_MASTER.TRANSDRCR = 'DEBIT') AND " +
            "(GL_MASTER.TRANSTP = '" + typeCD + "')  GROUP BY GL_MASTER.DEBITCD, A.ACCOUNTNM, GL_MASTER.CREDITCD, B.ACCOUNTNM", conn);

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
                string DEBITCD = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                e.Row.Cells[0].Text = "&nbsp;" + DEBITCD;

                string PARTICULARS = DataBinder.Eval(e.Row.DataItem, "CREDITNM").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + PARTICULARS;

                decimal AMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMOUNT").ToString());
                string Amnt = dbFunctions.SpellAmount.comma(AMT);
                e.Row.Cells[2].Text = Amnt + "&nbsp;";

                totAmount += AMT;
                totAmountComma = dbFunctions.SpellAmount.comma(totAmount);

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total : ";
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = totAmountComma;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }
        }

    }
}