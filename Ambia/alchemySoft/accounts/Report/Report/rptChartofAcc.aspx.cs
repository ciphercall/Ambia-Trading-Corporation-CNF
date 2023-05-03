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
    public partial class rptChartofAcc : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='101' ", lblCompNM);
                dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY  WHERE COMPID='101' ", lblAddress);

                DateTime PrintDate = DateTime.Today.Date;
                string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                lblTime.Text = td;

                showgrid();
            }
            catch (Exception ex)
            {
                dbFunctions.showMessage(Page,ex.Message);
            }
        }

        public void showgrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("", conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = (" SELECT ACCOUNTCD AS [ACCOUNT CODE] , ACCOUNTNM AS [ACCOUNT NAME], STATUSCD + '       ' + CONVERT (NVARCHAR(10), LEVELCD, 103) AS STATUS, CONTROLCD AS [CONTROL CODE]   FROM GL_ACCHART");

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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string ACCCD = DataBinder.Eval(e.Row.DataItem, "[ACCOUNT CODE]").ToString();
                e.Row.Cells[0].Text = ACCCD;

                string ACCNM = DataBinder.Eval(e.Row.DataItem, "[ACCOUNT NAME]").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + ACCNM;

                string STATUS = DataBinder.Eval(e.Row.DataItem, "STATUS").ToString();
                e.Row.Cells[2].Text = STATUS;

                string CONCD = DataBinder.Eval(e.Row.DataItem, "[CONTROL CODE]").ToString();
                e.Row.Cells[3].Text = CONCD;

                if (STATUS != "P       5")
                {
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[1].Font.Bold = true;
                    e.Row.Cells[2].Font.Bold = true;
                    e.Row.Cells[3].Font.Bold = true;
                }
                else
                {
                    e.Row.Cells[0].Font.Bold = false;
                    e.Row.Cells[1].Font.Bold = false;
                    e.Row.Cells[2].Font.Bold = false;
                    e.Row.Cells[3].Font.Bold = false;
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
