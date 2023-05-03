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
    public partial class rptNotesAccount : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        decimal dblGrandTotalAmount = 0;

        string dblGrandTotalAmountComma = "";

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
            dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

            lblTime.Text = dbFunctions.timezone(DateTime.Now).ToString("dd-MMM-yyyy hh:mm: tt") + " | " + Session["USERNAME"];

            string AccNM = Session["AccNM"].ToString();
            string txtFrom = Session["From"].ToString();
            string txtTo = Session["To"].ToString();
            string LevelCD = Session["LevelCD"].ToString();

            lblHeadNM.Text = AccNM;
            DateTime FDate = DateTime.Parse(txtFrom, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblFrom.Text = FDate.ToString("dd-MMM-yyyy");

            DateTime TDate = DateTime.Parse(txtTo, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            lblTo.Text = TDate.ToString("dd-MMM-yyyy");

            if (LevelCD == "1")
            {
                showGrid();
            }
            else if (LevelCD == "2")
            {
                showGrid_L2();
            }
            else if (LevelCD == "3")
            {
                showGrid_L3();
            }
            else if (LevelCD == "4")
            {
                showGrid_L4();
            }
        }

        public void showGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string ddlLevelID = Session["TransLevel"].ToString();
            string lblAccHeadCD = Session["AccCode"].ToString();
            string AccountHD = lblAccHeadCD.Substring(0, 1);
            string AccNM = Session["AccNM"].ToString();
            string txtFrom = Session["From"].ToString();
            string txtTo = Session["To"].ToString();
            string LevelCD = Session["LevelCD"].ToString();

            DateTime FDate = DateTime.Parse(txtFrom, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(txtTo, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string TDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("", conn);
            if (ddlLevelID == "1")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM (SELECT SUBSTRING(DEBITCD, 1, 3) + '000000000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 1) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '1') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 3)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }
            else if (ddlLevelID == "2")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 3) + '000000000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 1) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '2') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 3)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }
            else if (ddlLevelID == "3")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 3) + '000000000' AS ACCCD, SUM(CREDITAMT) - SUM(DEBITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 1) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '3') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 3)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }
            else if (ddlLevelID == "4")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 3) + '000000000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 1) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '4') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 3)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }

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

        public void showGrid_L2()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string ddlLevelID = Session["TransLevel"].ToString();
            string lblAccHeadCD = Session["AccCode"].ToString();
            string AccountHD = lblAccHeadCD.Substring(0, 3);
            string AccNM = Session["AccNM"].ToString();
            string txtFrom = Session["From"].ToString();
            string txtTo = Session["To"].ToString();
            string LevelCD = Session["LevelCD"].ToString();

            DateTime FDate = DateTime.Parse(txtFrom, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(txtTo, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string TDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("", conn);
            if (ddlLevelID == "1")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 5) + '0000000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 3) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '1') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 5)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }
            else if (ddlLevelID == "2")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 5) + '0000000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 3) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '2') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 5)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }
            else if (ddlLevelID == "3")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 5) + '0000000' AS ACCCD, SUM(CREDITAMT) - SUM(DEBITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 3) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '3') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 5)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }
            else if (ddlLevelID == "4")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 5) + '0000000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 3) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '4') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 5)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00) AS a");
            }

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

        public void showGrid_L3()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string ddlLevelID = Session["TransLevel"].ToString();
            string lblAccHeadCD = Session["AccCode"].ToString();
            string AccountHD = lblAccHeadCD.Substring(0, 5);
            string AccNM = Session["AccNM"].ToString();
            string txtFrom = Session["From"].ToString();
            string txtTo = Session["To"].ToString();
            string LevelCD = Session["LevelCD"].ToString();

            DateTime FDate = DateTime.Parse(txtFrom, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(txtTo, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string TDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("", conn);
            if (ddlLevelID == "1")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 5) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '1') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 7)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00)AS a");
            }
            else if (ddlLevelID == "2")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 5) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '2') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 7)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00 )AS a");
            }
            else if (ddlLevelID == "3")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCCD, SUM(CREDITAMT) - SUM(DEBITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 5) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '3') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 7)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00)AS a");
            }
            else if (ddlLevelID == "4")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM         (SELECT     SUBSTRING(DEBITCD, 1, 7) + '00000' AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM          GL_MASTER " +
                                   " WHERE      (SUBSTRING(DEBITCD, 1, 5) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '4') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 7)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00)AS a");
            }

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

        public void showGrid_L4()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string ddlLevelID = Session["TransLevel"].ToString();
            string lblAccHeadCD = Session["AccCode"].ToString();
            string AccountHD = lblAccHeadCD.Substring(0, 7);
            string AccNM = Session["AccNM"].ToString();
            string txtFrom = Session["From"].ToString();
            string txtTo = Session["To"].ToString();
            string LevelCD = Session["LevelCD"].ToString();

            DateTime FDate = DateTime.Parse(txtFrom, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FDT = FDate.ToString("yyyy/MM/dd");

            DateTime TDate = DateTime.Parse(txtTo, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string TDT = TDate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("", conn);
            if (ddlLevelID == "1")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT    ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM (SELECT SUBSTRING(DEBITCD, 1, 12) AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM GL_MASTER " +
                                   " WHERE (SUBSTRING(DEBITCD, 1, 7) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '1') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 12)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00 )AS a");
            }
            else if (ddlLevelID == "2")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM (SELECT SUBSTRING(DEBITCD, 1, 12) AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM GL_MASTER " +
                                   " WHERE (SUBSTRING(DEBITCD, 1, 7) = '" + AccountHD + "') AND (TRANSDT <= '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '2') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 12)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00)AS a");
            }
            else if (ddlLevelID == "3")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM (SELECT SUBSTRING(DEBITCD, 1, 12) AS ACCCD, SUM(CREDITAMT) - SUM(DEBITAMT) AS AMT " +
                                   " FROM GL_MASTER " +
                                   " WHERE (SUBSTRING(DEBITCD, 1, 7) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '3') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 12)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00)AS a");
            }
            else if (ddlLevelID == "4")
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = (" SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.ACCCD)AS SL, A.ACCCD, A.AMT, GL_ACCHART.ACCOUNTNM " +
                                   " FROM (SELECT     SUBSTRING(DEBITCD, 1, 12) AS ACCCD, SUM(DEBITAMT) - SUM(CREDITAMT) AS AMT " +
                                   " FROM GL_MASTER " +
                                   " WHERE (SUBSTRING(DEBITCD, 1, 7) = '" + AccountHD + "') AND (TRANSDT BETWEEN '" + FDT + "' AND '" + TDT + "') AND (SUBSTRING(DEBITCD, 1, 1) = '4') " +
                                   " GROUP BY SUBSTRING(DEBITCD, 1, 12)) AS A INNER JOIN " +
                                   " GL_ACCHART ON A.ACCCD = GL_ACCHART.ACCOUNTCD WHERE a.AMT<>0.00)AS a");
            }

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
                string SL = DataBinder.Eval(e.Row.DataItem, "SL").ToString();
                e.Row.Cells[0].Text = SL;

                string ACCCD = DataBinder.Eval(e.Row.DataItem, "ACCCD").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + ACCCD;

                string PARTICULARS = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                string LevelCD = Session["LevelCD"].ToString();
                string from = Session["To"].ToString();
                if (LevelCD == "4")
                {
                    PARTICULARS = "<a href='ReportDdaFromAnother.aspx?AccNM=" + PARTICULARS + "&AccCode=" + ACCCD + "&From=" + from + "' target ='_blank'>" + PARTICULARS + "</a>";
                }



                e.Row.Cells[2].Text = PARTICULARS;

                decimal AMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMT").ToString());
                string Amount =dbFunctions.SpellAmount.comma(AMT);
                e.Row.Cells[3].Text = Amount + "&nbsp;";

                decimal R_Amount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AMT").ToString());

                dblGrandTotalAmount += R_Amount;
                dblGrandTotalAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalAmount);

            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "TOTAL :";
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = dblGrandTotalAmountComma;
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