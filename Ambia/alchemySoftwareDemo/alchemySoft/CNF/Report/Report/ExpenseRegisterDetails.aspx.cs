using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Entity;
using alchemySoft;

namespace alchemySoft.CNF.report.vis_rep
{
    public partial class ExpenseRegisterDetails : System.Web.UI.Page
    {
        string strPreviousRowID = string.Empty;
        string strPreviousRowID2 = string.Empty;
        string strPreviousRowID3 = string.Empty;
        string strPreviousRowID4 = string.Empty;
        string strPreviousRowID5 = string.Empty;

        int intSubTotalIndex = 1;
        decimal dblSubTotalAmount = 0;
        decimal dblGrandTotalAmount = 0;
        string dblSubTotalAmountComma = "0";
        string dblGrandTotalAmountComma = "0";
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {

            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/Login/UI/signin.aspx");
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
            //DateTime eddate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime td = dbFunctions.timezone(DateTime.Now);
            lblPrintDate.Text = td.ToString("yyyy-MM-dd hh:mm:ss:tt");

            string expcd = Session["expenseID"].ToString();
            dbFunctions.lblAdd("select ACCOUNTNM from GL_ACCHART where ACCOUNTCD='" + expcd + "' AND STATUSCD ='P'", lblExpenseHD);

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            string fromDate = Session["fromdate"].ToString();
            DateTime FRDT = DateTime.Parse(fromDate, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string FDT = FRDT.ToString("yyyy-MM-dd");
            lblFromdate.Text = fromDate;

            string todate = Session["todate"].ToString();
            DateTime TODT = DateTime.Parse(todate, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string TDT = TODT.ToString("yyyy-MM-dd");
            lblTodate.Text = todate;

            SqlCommand cmd = new SqlCommand(@"SELECT ( CONVERT(NVARCHAR(20), CNF_JOBEXP.JOBNO,103 ) + '-' + CNF_JOBEXP.JOBTP + '-' +  CONVERT(NVARCHAR(20),CNF_JOBEXP.JOBYY,103)) AS JB, ASL_BRANCH.BRANCHID, GL_ACCHART.ACCOUNTNM,  " +
                      " CONVERT(NVARCHAR(20), CNF_JOBEXP.TRANSDT, 103) AS TD, CNF_JOBEXP.TRANSNO, CNF_EXPENSE.EXPNM, CNF_JOBEXP.EXPAMT, CNF_JOBEXP.REMARKS, CNF_JOBEXP.JOBNO, CNF_JOBEXP.JOBTP, CNF_JOBEXP.JOBYY  " +
                      " FROM CNF_JOBEXP INNER JOIN " +
                      " ASL_BRANCH ON CNF_JOBEXP.COMPID = ASL_BRANCH.BRANCHCD INNER JOIN " +
                      " CNF_EXPENSE ON CNF_JOBEXP.EXPID = CNF_EXPENSE.EXPID INNER JOIN " +
                      " CNF_JOB ON CNF_JOBEXP.JOBYY = CNF_JOB.JOBYY AND CNF_JOBEXP.JOBTP = CNF_JOB.JOBTP AND CNF_JOBEXP.JOBNO = CNF_JOB.JOBNO INNER JOIN " +
                      " GL_ACCHART ON CNF_JOB.PARTYID = GL_ACCHART.ACCOUNTCD " +
                      " WHERE CNF_JOBEXP.TRANSDT BETWEEN @FROMDATE AND @TODATE AND CNF_JOBEXP.EXPCD='" + expcd + "' ORDER BY CNF_JOBEXP.JOBNO, CNF_JOBEXP.JOBTP, CNF_JOBEXP.JOBYY, CNF_JOBEXP.TRANSDT", conn);

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@FROMDATE", FDT);
            cmd.Parameters.AddWithValue("@TODATE", TDT);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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

        protected void gvReport_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd = false;
            //bool IsGrandTotalRowNeedtoAdd = false;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "JB") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "JB").ToString())
                    IsSubTotalRowNeedToAdd = true;
            //else if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "JOBTP").ToString())
            //    IsSubTotalRowNeedToAdd = true;
            //else if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "JOBYY").ToString())
            //    IsSubTotalRowNeedToAdd = true;
            //else if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "BRANCHID").ToString())
            //    IsSubTotalRowNeedToAdd = true;
            //else if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString())
            //    IsSubTotalRowNeedToAdd = true;


            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "JB") == null) && (DataBinder.Eval(e.Row.DataItem, "BRANCHID") == null) && (DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                //IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }

            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "JB") != null))
            {
                GridView gvReport = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = DataBinder.Eval(e.Row.DataItem, "JB").ToString();
                //cell.ColumnSpan = 5;
                cell.Visible = false;
                cell.CssClass = "GroupHeaderStyle";
                cell.Font.Bold = true;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Job No :  " + DataBinder.Eval(e.Row.DataItem, "JOBNO").ToString();
                //cell.ColumnSpan = 5;
                cell.CssClass = "GroupHeaderStyle";
                cell.Font.Bold = true;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Job Type :  " + DataBinder.Eval(e.Row.DataItem, "JOBTP").ToString();
                //cell.ColumnSpan = 5;
                cell.CssClass = "GroupHeaderStyle";
                cell.Font.Bold = true;

                row.Cells.Add(cell);
                cell = new TableCell();
                cell.Text = "Year :  " + DataBinder.Eval(e.Row.DataItem, "JOBYY").ToString();
                //cell.ColumnSpan = 5;
                cell.CssClass = "GroupHeaderStyle";
                cell.Font.Bold = true;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Branch :  " + DataBinder.Eval(e.Row.DataItem, "BRANCHID").ToString();
                //cell.ColumnSpan = 5;
                cell.CssClass = "GroupHeaderStyle";
                cell.Font.Bold = true;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Party :  " + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                //cell.ColumnSpan = 5;
                cell.CssClass = "GroupHeaderStyle";
                cell.Font.Bold = true;
                row.Cells.Add(cell);

                gvReport.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
            }
            #endregion
            if (IsSubTotalRowNeedToAdd)
            {
                #region Adding Sub Total Row
                GridView gvReport = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total : ";
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.ColumnSpan = 3;
                cell.Font.Bold = true;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding Amount Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalAmountComma);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.Font.Bold = true;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding Amount Column         
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblSubTotalAmountComma);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                gvReport.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                #endregion
                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "JB") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = DataBinder.Eval(e.Row.DataItem, "JB").ToString();
                    //cell.ColumnSpan = 5;
                    cell.Visible = false;
                    cell.CssClass = "GroupHeaderStyle";
                    cell.Font.Bold = true;
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Job No :  " + DataBinder.Eval(e.Row.DataItem, "JOBNO").ToString();
                    //cell.ColumnSpan = 5;
                    cell.CssClass = "GroupHeaderStyle";
                    cell.Font.Bold = true;
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Job Type :  " + DataBinder.Eval(e.Row.DataItem, "JOBTP").ToString();
                    //cell.ColumnSpan = 5;
                    cell.CssClass = "GroupHeaderStyle";
                    cell.Font.Bold = true;

                    row.Cells.Add(cell);
                    cell = new TableCell();
                    cell.Text = "Year :  " + DataBinder.Eval(e.Row.DataItem, "JOBYY").ToString();
                    //cell.ColumnSpan = 5;
                    cell.CssClass = "GroupHeaderStyle";
                    cell.Font.Bold = true;
                    row.Cells.Add(cell);


                    cell = new TableCell();
                    cell.Text = "Branch :  " + DataBinder.Eval(e.Row.DataItem, "BRANCHID").ToString();
                    //cell.ColumnSpan = 5;
                    cell.CssClass = "GroupHeaderStyle";
                    cell.Font.Bold = true;
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Party :  " + DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();
                    //cell.ColumnSpan = 5;
                    cell.CssClass = "GroupHeaderStyle";
                    cell.Font.Bold = true;
                    row.Cells.Add(cell);

                    gvReport.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                }
                #endregion
                #region Reseting the Sub Total Variables
                dblSubTotalAmount = 0;
                #endregion
            }
        }


        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "JB").ToString();

                //strPreviousRowID2 = DataBinder.Eval(e.Row.DataItem, "JOBYY").ToString();
                //strPreviousRowID3 = DataBinder.Eval(e.Row.DataItem, "JOBTP").ToString();
                //strPreviousRowID4 = DataBinder.Eval(e.Row.DataItem, "BRANCHID").ToString();
                //strPreviousRowID5 = DataBinder.Eval(e.Row.DataItem, "ACCOUNTNM").ToString();




                string TD = DataBinder.Eval(e.Row.DataItem, "TD").ToString();
                e.Row.Cells[0].Text = TD;

                string TRANSNO = DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString();
                e.Row.Cells[1].Text = TRANSNO;

                string EXPNM = DataBinder.Eval(e.Row.DataItem, "EXPNM").ToString();
                e.Row.Cells[2].Text = "&nbsp;" + "&nbsp;" + EXPNM;

                decimal EXPAMT = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "EXPAMT").ToString());
                string INWORDS = dbFunctions.SpellAmount.comma(EXPAMT);
                //e.Row.Cells[3].Text = styleQty;
                e.Row.Cells[3].Text = INWORDS;


                dblSubTotalAmount += EXPAMT;
                dblSubTotalAmountComma = dbFunctions.SpellAmount.comma(dblSubTotalAmount);
                e.Row.Font.Bold = true;

                dblGrandTotalAmount += EXPAMT;
                dblGrandTotalAmountComma = dbFunctions.SpellAmount.comma(dblGrandTotalAmount);

                string REMARKS = DataBinder.Eval(e.Row.DataItem, "REMARKS").ToString();
                e.Row.Cells[4].Text = "&nbsp;" + "&nbsp;" + REMARKS;



            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "Grand Total : ";
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = dblGrandTotalAmountComma;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }
        }

    }
}
