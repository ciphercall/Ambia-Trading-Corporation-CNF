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
using alchemySoft;

namespace alchemy.accounts.UI
{
    public partial class MultipleVoucher : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {

                string formLink = "/Accounts/UI/MultipleVoucher.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {

                        if (ddlTransType.Text == "MPAY")
                        {
                            DateTime today = dbFunctions.timezone(DateTime.Now);
                            txtTransDate.Text = today.ToString("dd/MM/yyyy");

                            string mon = dbFunctions.timezone(DateTime.Today.Date).ToString("MMM").ToUpper();
                            string year = today.ToString("yy");
                            txtTransYear.Text = mon + "-" + year;
                            dbFunctions.lblAdd(
                                @"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtTransYear.Text +
                                "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                            if (lblVCount.Text == "")
                            {
                                txtVouchNo.Text = "1";
                            }
                            else
                            {
                                int vNo = int.Parse(lblVCount.Text);
                                int totVno = vNo + 1;
                                txtVouchNo.Text = totVno.ToString();
                            }
                            ddlTransType.Focus();
                            Session["Transtype"] = ddlTransType.Text;
                        }
                        ShowGrid();

                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                Session["Transtype"] = "";
                lblVCount.Text = "";
                if (ddlTransType.Text == "MREC")
                {
                    Session["Transtype"] = ddlTransType.Text;
                    //gvDetails.Visible = false; 
                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
                    string year = today.ToString("yy");
                    txtTransYear.Text = mon + "-" + year;
                    dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                    if (lblVCount.Text == "")
                    {
                        txtVouchNo.Text = "1";
                    }
                    else
                    {
                        int vNo = int.Parse(lblVCount.Text);
                        int totVno = vNo + 1;
                        txtVouchNo.Text = totVno.ToString();
                    }
                    ddlTransType.Focus();
                }
                else if (ddlTransType.Text == "MPAY")
                {
                    Session["Transtype"] = ddlTransType.Text;
                    //gvDetails.Visible = false;
                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
                    string year = today.ToString("yy");
                    txtTransYear.Text = mon + "-" + year;
                    dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                    if (lblVCount.Text == "")
                    {
                        txtVouchNo.Text = "1";
                    }
                    else
                    {
                        int vNo = int.Parse(lblVCount.Text);
                        int totVno = vNo + 1;
                        txtVouchNo.Text = totVno.ToString();
                    }
                    ddlTransType.Focus();
                }
                else if (ddlTransType.Text == "JOUR")
                {
                    Session["Transtype"] = ddlTransType.Text;
                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
                    string year = today.ToString("yy");
                    txtTransYear.Text = mon + "-" + year;
                    dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                    if (lblVCount.Text == "")
                    {
                        txtVouchNo.Text = "1";
                    }
                    else
                    {
                        int vNo = int.Parse(lblVCount.Text);
                        int totVno = vNo + 1;
                        txtVouchNo.Text = totVno.ToString();
                    }
                    ddlTransType.Focus();
                }
                else if (ddlTransType.Text == "CONT")
                {
                    Session["Transtype"] = ddlTransType.Text;
                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
                    string year = today.ToString("yy");
                    txtTransYear.Text = mon + "-" + year;
                    dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                    if (lblVCount.Text == "")
                    {
                        txtVouchNo.Text = "1";
                    }
                    else
                    {
                        int vNo = int.Parse(lblVCount.Text);
                        int totVno = vNo + 1;
                        txtVouchNo.Text = totVno.ToString();
                    }
                    ddlTransType.Focus();
                }
                else
                {
                    return;
                }
                ShowGrid();
                txtTransDate.Focus();
            }

        }


        protected void txtTransDate_TextChanged(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                DateTime transdate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string trDT = transdate.ToString("yyyy-MM-dd");
                string month = transdate.ToString("MMM").ToUpper();
                string years = transdate.ToString("yy");

                txtTransYear.Text = month + "-" + years;

                if (btnEdit.Text == "EDIT")
                {
                    dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                    if (lblVCount.Text == "")
                    {
                        txtVouchNo.Text = "1";
                    }
                    else
                    {
                        int vNo = int.Parse(lblVCount.Text);
                        int totVno = vNo + 1;
                        txtVouchNo.Text = totVno.ToString();
                    }

                    TextBox txtDebited = (TextBox)gvDetails.FooterRow.FindControl("txtDebited");
                    txtDebited.Focus();
                }
                else
                {
                    dbFunctions.dropDown_Bind(ddlVouch, "", "select", "SELECT TRANSNO nm FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSDT = '" + trDT + "'");
                    ddlVouch.Focus();
                }
                txtVouchNo.Focus();
            }
        }



        public void ShowGrid()
        {
            DateTime eddate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string edate = eddate.ToString("yyyy-MM-dd");


            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();

            SqlCommand cmd = new SqlCommand();
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            cmd = new SqlCommand(" SELECT GL_MTRANS.TRANSTP, GL_MTRANS.TRANSDT, GL_MTRANS.TRANSMY, GL_MTRANS.TRANSNO, GL_MTRANS.SERIALNO, GL_MTRANS.TRANSFOR, " +
                                        " GL_MTRANS.COSTPID, GL_MTRANS.TRANSMODE, GL_MTRANS.DEBITCD, GL_MTRANS.CREDITCD, GL_MTRANS.CHEQUENO, CASE WHEN CONVERT(DATE, GL_MTRANS.CHEQUEDT) = '01-01-1900' THEN '' ELSE convert(nvarchar(10),GL_MTRANS.CHEQUEDT,103) end as CHEQUEDT , " +
                                        " GL_MTRANS.AMOUNT, GL_MTRANS.REMARKS, GL_MTRANS.USERPC, GL_ACCHART.ACCOUNTNM AS DEBITNM, GL_ACCHART_1.ACCOUNTNM AS CREDITNM, " +
                                        " GL_COSTP.COSTPNM FROM GL_MTRANS LEFT JOIN GL_ACCHART ON GL_MTRANS.DEBITCD = GL_ACCHART.ACCOUNTCD LEFT OUTER JOIN " +
                                        " GL_ACCHART AS GL_ACCHART_1 ON GL_MTRANS.CREDITCD = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_COSTP ON GL_MTRANS.COSTPID = GL_COSTP.COSTPID " +
                                        " WHERE GL_MTRANS.TRANSTP = '" + ddlTransType.Text + "' AND GL_MTRANS.TRANSDT = '" + edate + "' AND GL_MTRANS.TRANSMY = '" + txtTransYear.Text + "' AND GL_MTRANS.TRANSNO = " + txtVouchNo.Text + "", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                gvDetails.Visible = true;

                if (gvDetails.EditIndex == -1)
                {
                    Decimal totAmt = 0;
                    Decimal a = 0;
                    foreach (GridViewRow grid in gvDetails.Rows)
                    {
                        Label Per = (Label)grid.Cells[9].FindControl("lblAmount");
                        if (Per.Text == "")
                        {
                            Per.Text = "0";
                        }
                        else
                        {
                            Per.Text = Per.Text;
                        }
                        String Perf = Per.Text;
                        totAmt = Convert.ToDecimal(Perf);
                        a += totAmt;
                        txtTotAmount.Text = a.ToString();
                    }
                    a += totAmt;

                    txtTotInWords.Text = "";
                    decimal dec;
                    Boolean ValidInput = Decimal.TryParse(txtTotAmount.Text, out dec);
                    if (!ValidInput)
                    {
                        txtTotInWords.ForeColor = System.Drawing.Color.Red;
                        txtTotInWords.Text = "Enter the Proper Amount...";
                        return;
                    }
                    if (txtTotAmount.Text.ToString().Trim() == "")
                    {
                        txtTotInWords.ForeColor = System.Drawing.Color.Red;
                        txtTotInWords.Text = "Amount Cannot Be Empty...";
                        return;
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtTotAmount.Text) == 0)
                        {
                            txtTotInWords.ForeColor = System.Drawing.Color.Red;
                            txtTotInWords.Text = "Amount Cannot Be Empty...";
                            return;
                        }
                    }

                    string x1 = "";
                    string x2 = "";

                    if (txtTotAmount.Text.Contains("."))
                    {
                        x1 = txtTotAmount.Text.ToString().Trim().Substring(0, txtTotAmount.Text.ToString().Trim().IndexOf("."));
                        x2 = txtTotAmount.Text.ToString().Trim().Substring(txtTotAmount.Text.ToString().Trim().IndexOf(".") + 1);
                    }
                    else
                    {
                        x1 = txtTotAmount.Text.ToString().Trim();
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

                    txtTotAmount.Text = x1 + "." + x2;

                    if (x2.Length > 2)
                    {
                        txtTotAmount.Text = Math.Round(Convert.ToDouble(txtTotAmount.Text), 2).ToString().Trim();
                    }

                    string AmtConv = dbFunctions.SpellAmount.MoneyConvFn(txtTotAmount.Text.ToString().Trim());




                    txtTotInWords.Text = AmtConv.Trim();
                    txtTotInWords.ForeColor = System.Drawing.Color.Green;
                    txtTotInWords.Focus();
                }
                else
                {

                }

                TextBox txtDebited = (TextBox)gvDetails.FooterRow.FindControl("txtDebited");
                txtDebited.Focus();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                int columncount = gvDetails.Rows[0].Cells.Count;
                gvDetails.Rows[0].Cells.Clear();
                gvDetails.Rows[0].Cells.Add(new TableCell());
                gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                gvDetails.Rows[0].Cells[0].Text = "No Records Found";
                gvDetails.Rows[0].Visible = false;
            }

            var txtCostPNM = (DropDownList)gvDetails.FooterRow.FindControl("txtCostPNM");
            dbFunctions.dropDown_Bind(txtCostPNM, "id", "select", @"SELECT COSTPNM nm,COSTPID id FROM GL_COSTP");
        }


        public void ShowGrid_Edit()
        {
            DateTime eddate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string edate = eddate.ToString("yyyy-MM-dd");


            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            Int64 EditTransNo = 0;
            if (ddlVouch.Text == "--SELECT--")
            {
                EditTransNo = 0;
            }
            else
                EditTransNo = Convert.ToInt64(ddlVouch.Text);

            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["BrCD"].ToString();

            SqlCommand cmd = new SqlCommand();

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            //if (uTp == "COMPADMIN")
            //{
            cmd = new SqlCommand(" SELECT GL_MTRANS.TRANSTP, GL_MTRANS.TRANSDT, GL_MTRANS.TRANSMY, GL_MTRANS.TRANSNO, GL_MTRANS.SERIALNO, GL_MTRANS.TRANSFOR, " +
                                            " GL_MTRANS.COSTPID, GL_MTRANS.TRANSMODE, GL_MTRANS.DEBITCD, GL_MTRANS.CREDITCD, GL_MTRANS.CHEQUENO, CASE WHEN CONVERT(DATE, GL_MTRANS.CHEQUEDT) = '01-01-1900' THEN '' ELSE convert(nvarchar(10),GL_MTRANS.CHEQUEDT,103) end as CHEQUEDT , " +
                                            " GL_MTRANS.AMOUNT, GL_MTRANS.REMARKS, GL_MTRANS.USERPC, GL_ACCHART.ACCOUNTNM AS DEBITNM, GL_ACCHART_1.ACCOUNTNM AS CREDITNM, " +
                                            " GL_COSTP.COSTPNM FROM GL_MTRANS LEFT JOIN GL_ACCHART ON GL_MTRANS.DEBITCD = GL_ACCHART.ACCOUNTCD LEFT OUTER  JOIN " +
                                            " GL_ACCHART AS GL_ACCHART_1 ON GL_MTRANS.CREDITCD = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_COSTP ON GL_MTRANS.COSTPID = GL_COSTP.COSTPID " +
                                            " WHERE GL_MTRANS.TRANSTP = '" + ddlTransType.Text + "' AND GL_MTRANS.TRANSDT = '" + edate + "' AND GL_MTRANS.TRANSMY = '" + txtTransYear.Text + "' AND GL_MTRANS.TRANSNO = " + EditTransNo + "", conn);
            //}
            //else
            //{
            //    cmd = new SqlCommand(" SELECT GL_MTRANS.TRANSTP, GL_MTRANS.TRANSDT, GL_MTRANS.TRANSMY, GL_MTRANS.TRANSNO, GL_MTRANS.SERIALNO, GL_MTRANS.TRANSFOR, " +
            //                                    " GL_MTRANS.COSTPID, GL_MTRANS.TRANSMODE, GL_MTRANS.DEBITCD, GL_MTRANS.CREDITCD, GL_MTRANS.CHEQUENO, CASE WHEN CONVERT(DATE, GL_MTRANS.CHEQUEDT) = '01-01-1900' THEN '' ELSE convert(nvarchar(10),GL_MTRANS.CHEQUEDT,103) end as CHEQUEDT , " +
            //                                    " GL_MTRANS.AMOUNT, GL_MTRANS.REMARKS, GL_MTRANS.USERPC, GL_ACCHART.ACCOUNTNM AS DEBITNM, GL_ACCHART_1.ACCOUNTNM AS CREDITNM, " +
            //                                    " GL_COSTP.COSTPNM FROM GL_MTRANS INNER JOIN GL_ACCHART ON GL_MTRANS.DEBITCD = GL_ACCHART.ACCOUNTCD INNER JOIN " +
            //                                    " GL_ACCHART AS GL_ACCHART_1 ON GL_MTRANS.CREDITCD = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_COSTP ON GL_MTRANS.COSTPID = GL_COSTP.COSTPID " +
            //                                    " WHERE GL_MTRANS.TRANSTP = '" + ddlTransType.Text + "' AND GL_MTRANS.TRANSDT = '" + edate + "' AND GL_MTRANS.TRANSMY = '" + txtTransYear.Text + "' AND GL_MTRANS.TRANSNO = " + EditTransNo + " AND GL_COSTP.CATID ='" + brCD + "'", conn);
            //}
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                gvDetails.Visible = true;

                if (gvDetails.EditIndex == -1)
                {
                    Decimal totAmt = 0;
                    Decimal a = 0;
                    foreach (GridViewRow grid in gvDetails.Rows)
                    {
                        Label Per = (Label)grid.Cells[9].FindControl("lblAmount");
                        if (Per.Text == "")
                        {
                            Per.Text = "0";
                        }
                        else
                        {
                            Per.Text = Per.Text;
                        }
                        String Perf = Per.Text;
                        totAmt = Convert.ToDecimal(Perf);
                        a += totAmt;
                        txtTotAmount.Text = a.ToString();
                    }
                    a += totAmt;

                    txtTotInWords.Text = "";
                    decimal dec;
                    Boolean ValidInput = Decimal.TryParse(txtTotAmount.Text, out dec);
                    if (!ValidInput)
                    {
                        txtTotInWords.ForeColor = System.Drawing.Color.Red;
                        txtTotInWords.Text = "Enter the Proper Amount...";
                        return;
                    }
                    if (txtTotAmount.Text.ToString().Trim() == "")
                    {
                        txtTotInWords.ForeColor = System.Drawing.Color.Red;
                        txtTotInWords.Text = "Amount Cannot Be Empty...";
                        return;
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtTotAmount.Text) == 0)
                        {
                            txtTotInWords.ForeColor = System.Drawing.Color.Red;
                            txtTotInWords.Text = "Amount Cannot Be Empty...";
                            return;
                        }
                    }

                    string x1 = "";
                    string x2 = "";

                    if (txtTotAmount.Text.Contains("."))
                    {
                        x1 = txtTotAmount.Text.ToString().Trim().Substring(0, txtTotAmount.Text.ToString().Trim().IndexOf("."));
                        x2 = txtTotAmount.Text.ToString().Trim().Substring(txtTotAmount.Text.ToString().Trim().IndexOf(".") + 1);
                    }
                    else
                    {
                        x1 = txtTotAmount.Text.ToString().Trim();
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

                    txtTotAmount.Text = x1 + "." + x2;

                    if (x2.Length > 2)
                    {
                        txtTotAmount.Text = Math.Round(Convert.ToDouble(txtTotAmount.Text), 2).ToString().Trim();
                    }

                    string AmtConv = dbFunctions.SpellAmount.MoneyConvFn(txtTotAmount.Text.ToString().Trim());
                    //string amntComma = SpellAmount.comma(Convert.ToDecimal(txtAmount.Text));
                    //Label3.Text = amntComma;

                    txtTotInWords.Text = AmtConv.Trim();
                    txtTotInWords.ForeColor = System.Drawing.Color.Green;
                    txtTotInWords.Focus();
                }
                else
                {

                }

                TextBox txtDebited = (TextBox)gvDetails.FooterRow.FindControl("txtDebited");
                txtDebited.Focus();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                int columncount = gvDetails.Rows[0].Cells.Count;
                gvDetails.Rows[0].Cells.Clear();
                gvDetails.Rows[0].Cells.Add(new TableCell());
                gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                gvDetails.Rows[0].Cells[0].Text = "No Records Found";
                gvDetails.Rows[0].Visible = false;
            }
            var txtCostPNM = (DropDownList)gvDetails.FooterRow.FindControl("txtCostPNM");
            dbFunctions.dropDown_Bind(txtCostPNM, "id", "select", @"SELECT COSTPNM,COSTPID FROM GL_COSTP");
        }


        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                Label lblDebit = new Label();
                Label lblCredit = new Label();

                lblDebit = (Label)e.Row.FindControl("lblDebit");
                lblCredit = (Label)e.Row.FindControl("lblCredit");

                if (ddlTransType.Text == "MREC")
                {
                    lblDebit.Text = "Received To";
                    lblCredit.Text = "Received From";
                }
                else if (ddlTransType.Text == "MPAY")
                {
                    lblDebit.Text = "Payment To";
                    lblCredit.Text = "Payment From";
                }
                else if (ddlTransType.Text == "JOUR")
                {
                    lblDebit.Text = "Debited To";
                    lblCredit.Text = "Credited To";
                }
                else if (ddlTransType.Text == "CONT")
                {
                    lblDebit.Text = "Deposited To";
                    lblCredit.Text = "Withdrawn From";
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (btnEdit.Text == "EDIT")
                {
                    DateTime eddate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    string edate = eddate.ToString("yyyy-MM-dd");

                    dbFunctions.lblAdd(@"SELECT MAX(SERIALNO) FROM GL_MTRANS WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + edate + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + txtVouchNo.Text + "", lblSLCount);

                    int sl = 0;
                    int slno = 0;
                    if (lblSLCount.Text == "")
                    {
                        e.Row.Cells[0].Text = "1";
                    }
                    else
                    {
                        sl = int.Parse(lblSLCount.Text);
                        slno = sl + 1;
                        e.Row.Cells[0].Text = slno.ToString();
                    }
                }
                else  /////////Edit Mode
                {
                    DateTime eddate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    string edate = eddate.ToString("yyyy-MM-dd");

                    Int64 EditTransNo = 0;
                    if (ddlVouch.Text == "--SELECT--")
                    {
                        EditTransNo = 0;
                    }
                    else
                        EditTransNo = Convert.ToInt64(ddlVouch.Text);

                    dbFunctions.lblAdd(@"SELECT MAX(SERIALNO) FROM GL_MTRANS WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + edate + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + EditTransNo + "", lblSLCount);

                    int sl = 0;
                    int slno = 0;
                    if (lblSLCount.Text == "")
                    {
                        e.Row.Cells[0].Text = "1";
                    }
                    else
                    {
                        sl = int.Parse(lblSLCount.Text);
                        slno = sl + 1;
                        e.Row.Cells[0].Text = slno.ToString();
                    }
                }

                //DropDownList ddlTransMode = (DropDownList)e.Row.FindControl("ddlTransMode");
                //TextBox txtCheque = (TextBox)e.Row.FindControl("txtCheque");
                //TextBox txtChequeDate = (TextBox)e.Row.FindControl("txtChequeDate");
                //TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                //if (ddlTransMode.Text == "CASH CHEQUE")
                //{
                //    txtCheque.Enabled = true;
                //    txtChequeDate.Enabled = true;
                //    txtCheque.Focus();
                //}
                //else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
                //{
                //    txtCheque.Enabled = true;
                //    txtChequeDate.Enabled = true;
                //    txtCheque.Focus();
                //}
                //else
                //{
                //    txtCheque.Text = "";
                //    txtChequeDate.Text = "";
                //    txtCheque.Enabled = false;
                //    txtChequeDate.Enabled = false;
                //}
            }
        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>

        //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        //public static string[] GetCompletionListDebit(string prefixText, int count, string contextKey)
        //{
        //    string Transtype = HttpContext.Current.Session["Transtype"].ToString();
        //    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(dbFunctions.Connection);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("", conn);

        //    string uTp = CookiesData["USERTYPE"].ToString();
        //    string brCD = CookiesData["BrCD"].ToString();

        //    if (Transtype == "MREC")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }

        //    else if (Transtype == "MPAY")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }
        //    else if (Transtype == "JOUR")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }
        //    else if (Transtype == "CONT")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }
        //    else
        //    {
        //        Transtype = "";
        //    }

        //    SqlDataReader oReader;
        //    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
        //    List<String> CompletionSet = new List<string>();
        //    oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    while (oReader.Read())
        //        CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
        //    return CompletionSet.ToArray();

        //}

        //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        //public static string[] GetCompletionListCredit(string prefixText, int count, string contextKey)
        //{
        //    string Transtype = HttpContext.Current.Session["Transtype"].ToString();
        //    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(dbFunctions.Connection);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("", conn);

        //    string uTp = CookiesData["USERTYPE"].ToString();
        //    string brCD = CookiesData["BrCD"].ToString();

        //    if (Transtype == "MREC")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }

        //    else if (Transtype == "MPAY")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }
        //    else if (Transtype == "JOUR")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }
        //    else if (Transtype == "CONT")
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        if (uTp == "COMPADMIN")
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'");
        //        }
        //        else
        //        {
        //            cmd.CommandText = ("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') AND STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' AND (BRANCHCD ='" + brCD + "' OR BRANCHCD IS NULL OR BRANCHCD ='')");
        //        }
        //    }
        //    else
        //    {
        //        Transtype = "";
        //    }

        //    SqlDataReader oReader;
        //    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
        //    List<String> CompletionSet = new List<string>();
        //    oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    while (oReader.Read())
        //        CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
        //    return CompletionSet.ToArray();

        //}


        //protected void txtDebited_TextChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        //    TextBox txtDebited = (TextBox)row.FindControl("txtDebited");
        //    TextBox txtCredited = (TextBox)row.FindControl("txtCredited");

        //    dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtDebited.Text + "'", txtDebitCD);
        //    txtCredited.Focus();
        //}

        //protected void txtCredited_TextChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        //    TextBox txtCredited = (TextBox)row.FindControl("txtCredited");
        //    DropDownList txtCostPNM = (DropDownList)row.FindControl("txtCostPNM");

        //    dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtCredited.Text + "'", txtCreditCD);
        //    txtCostPNM.Focus();
        //}

        protected void ddlTransfor_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList ddlTransfor = (DropDownList)row.FindControl("ddlTransfor");

            DropDownList txtCostPNM = (DropDownList)row.FindControl("txtCostPNM");
            DropDownList ddlTransMode = (DropDownList)row.FindControl("ddlTransMode");

            if (ddlTransfor.Text == "PROJECT")
            {
                txtCostPNM.Enabled = true;
                txtCostPNM.Focus();
            }
            else
            {
                txtCostPNM.Text = "";
                txtCostPNM.Enabled = false;
                ddlTransMode.Focus();
            }
        }


        protected void ddlTransMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList ddlTransMode = (DropDownList)row.FindControl("ddlTransMode");
            TextBox txtCheque = (TextBox)row.FindControl("txtCheque");
            TextBox txtChequeDate = (TextBox)row.FindControl("txtChequeDate");
            TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

            if (ddlTransMode.Text == "CASH CHEQUE")
            {
                txtCheque.Enabled = true;
                txtChequeDate.Enabled = true;
                txtCheque.Focus();
            }
            else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {
                txtCheque.Enabled = true;
                txtChequeDate.Enabled = true;
                txtCheque.Focus();
            }
            else
            {
                txtCheque.Text = "";
                txtChequeDate.Text = "";
                txtCheque.Enabled = false;
                txtChequeDate.Enabled = false;
                txtRemarks.Focus();
            }
        }



        protected void txtChequeDate_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");
            txtRemarks.Focus();
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
            ImageButton imgbtnAdd = (ImageButton)row.FindControl("imgbtnAdd");

            txtCumInWords.Text = "";
            decimal dec;
            Boolean ValidInput = Decimal.TryParse(txtAmount.Text, out dec);
            if (!ValidInput)
            {
                txtCumInWords.ForeColor = System.Drawing.Color.Red;
                txtCumInWords.Text = "Enter the Proper Amount...";
                return;
            }
            if (txtAmount.Text.ToString().Trim() == "")
            {
                txtCumInWords.ForeColor = System.Drawing.Color.Red;
                txtCumInWords.Text = "Amount Cannot Be Empty...";
                return;
            }
            else
            {
                if (Convert.ToDecimal(txtAmount.Text) == 0)
                {
                    txtCumInWords.ForeColor = System.Drawing.Color.Red;
                    txtCumInWords.Text = "Amount Cannot Be Empty...";
                    return;
                }
            }

            string x1 = "";
            string x2 = "";

            if (txtAmount.Text.Contains("."))
            {
                x1 = txtAmount.Text.ToString().Trim().Substring(0, txtAmount.Text.ToString().Trim().IndexOf("."));
                x2 = txtAmount.Text.ToString().Trim().Substring(txtAmount.Text.ToString().Trim().IndexOf(".") + 1);
            }
            else
            {
                x1 = txtAmount.Text.ToString().Trim();
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

            txtAmount.Text = x1 + "." + x2;

            if (x2.Length > 2)
            {
                txtAmount.Text = Math.Round(Convert.ToDouble(txtAmount.Text), 2).ToString().Trim();
            }

            string AmtConv = dbFunctions.SpellAmount.MoneyConvFn(txtAmount.Text.ToString().Trim());
            //string amntComma = SpellAmount.comma(Convert.ToDecimal(txtAmount.Text));
            //Label3.Text = amntComma;

            txtCumInWords.Text = AmtConv.Trim();
            txtCumInWords.ForeColor = System.Drawing.Color.Black;


            imgbtnAdd.Focus();
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string userName = CookiesData["USERID"].ToString();
            string ipAddress = CookiesData["IpAddress"].ToString();
            string PCName = CookiesData["PCName"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            string inTm = dbFunctions.timezone(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss tt");
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string query = "";
            SqlCommand comm = new SqlCommand(query, conn);

            DateTime transDT = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string trDT = transDT.ToString("yyyy-MM-dd");

            if (e.CommandName.Equals("SaveCon"))
            {
                if (CookiesData == null)
                {
                    Response.Redirect("~/login/ui/SignIn");
                }
                else
                {
                    string serial = gvDetails.FooterRow.Cells[0].Text;
                    int sl = int.Parse(serial);
                    TextBox txtDebited = (TextBox)gvDetails.FooterRow.FindControl("txtDebited");
                    TextBox txtCredited = (TextBox)gvDetails.FooterRow.FindControl("txtCredited");
                    DropDownList ddlTransfor = (DropDownList)gvDetails.FooterRow.FindControl("ddlTransfor");
                    DropDownList txtCostPNM = (DropDownList)gvDetails.FooterRow.FindControl("txtCostPNM");
                    DropDownList ddlTransMode = (DropDownList)gvDetails.FooterRow.FindControl("ddlTransMode");
                    TextBox txtCheque = (TextBox)gvDetails.FooterRow.FindControl("txtCheque");
                    TextBox txtChequeDate = (TextBox)gvDetails.FooterRow.FindControl("txtChequeDate");
                    DateTime CQDT = new DateTime();
                    string cqdt;
                    if (txtCheque.Text == "")
                    {
                        CQDT = DateTime.Parse("01/01/1900 00:00:00", dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        cqdt = CQDT.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        CQDT = DateTime.Parse(txtChequeDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        cqdt = CQDT.ToString("yyyy-MM-dd");
                    }
                    TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");
                    TextBox txtAmount = (TextBox)gvDetails.FooterRow.FindControl("txtAmount");


                    dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtDebited.Text + "'", txtDebitCD);
                    dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtCredited.Text + "'", txtCreditCD);
                    if (txtTransDate.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select a Date.";
                        ddlTransType.Focus();
                    }
                    else if (txtDebited.Text == "" || txtDebitCD.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Debit Particulars.";
                        txtDebited.Focus();
                    }
                    else if (txtCredited.Text == "" || txtCreditCD.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Credit Particulars.";
                        txtCredited.Focus();
                    }
                    else if ((ddlTransfor.Text == "PROJECT") && (txtCostPNM.Text == "--SELECT--"))
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Project.";
                        txtCostPNM.Focus();
                    }
                    else if (txtAmount.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Type Amount.";
                        txtAmount.Focus();
                    }
                    else
                    {
                        lblError.Visible = false;

                        if (ddlTransfor.Text == "PROJECT")
                        {
                        }
                        else
                        {
                            txtCostPNM.SelectedIndex = -1;
                        }

                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand();

                        if (btnEdit.Text == "EDIT")
                        {
                            cmd = new SqlCommand("SELECT TRANSNO FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + txtVouchNo.Text + "", conn);
                        }
                        else  //////// Edit Mode
                        {
                            Int64 EditTransNo = 0;
                            if (ddlVouch.Text == "--SELECT--")
                            {
                                EditTransNo = 0;
                            }
                            else
                                EditTransNo = Convert.ToInt64(ddlVouch.Text);

                            cmd = new SqlCommand("SELECT TRANSNO FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + EditTransNo + "", conn);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (btnEdit.Text == "EDIT")
                            {
                                query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "', @INTIME, '" + ipAddress + "')");

                                comm = new SqlCommand(query, conn);
                                comm.Parameters.Clear();
                                comm.Parameters.AddWithValue("@INTIME", inTm.ToString());
                                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                                int result = comm.ExecuteNonQuery();
                                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                                ShowGrid();
                            }
                            else ////Edit Mode
                            {
                                Int64 EditTransNo = 0;
                                if (ddlVouch.Text == "--SELECT--")
                                {
                                    EditTransNo = 0;
                                }
                                else
                                    EditTransNo = Convert.ToInt64(ddlVouch.Text);

                                query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, UPDATEUSERID, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + EditTransNo + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "','" + ipAddress + "')");

                                comm = new SqlCommand(query, conn);
                                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                                int result = comm.ExecuteNonQuery();
                                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                                ShowGrid_Edit();
                            }
                        }
                        else
                        {
                            query = " INSERT INTO GL_MTRANSMST (TRANSTP, TRANSDT, TRANSMY, TRANSNO, USERPC, USERID, INTIME, IPADDRESS) " +
                                    " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + ",'" + PCName + "','" + userName + "', @INTIME,'" + ipAddress + "')";
                            comm = new SqlCommand(query, conn);
                            comm.Parameters.Clear();
                            comm.Parameters.AddWithValue("@INTIME", inTm);
                            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                            comm.ExecuteNonQuery();
                            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                            query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "', @INTIME,'" + ipAddress + "')");

                            comm = new SqlCommand(query, conn);
                            comm.Parameters.Clear();
                            comm.Parameters.AddWithValue("@INTIME", inTm);
                            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                            comm.ExecuteNonQuery();
                            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                            ShowGrid();
                        }
                    }
                }
            }

            else if (e.CommandName.Equals("Complete"))
            {
                if (CookiesData == null)
                {
                    Response.Redirect("~/login/ui/SignIn");
                }
                else
                {
                    string serial = gvDetails.FooterRow.Cells[0].Text;
                    int sl = int.Parse(serial);
                    TextBox txtDebited = (TextBox)gvDetails.FooterRow.FindControl("txtDebited");
                    TextBox txtCredited = (TextBox)gvDetails.FooterRow.FindControl("txtCredited");
                    DropDownList ddlTransfor = (DropDownList)gvDetails.FooterRow.FindControl("ddlTransfor");
                    DropDownList txtCostPNM = (DropDownList)gvDetails.FooterRow.FindControl("txtCostPNM");
                    DropDownList ddlTransMode = (DropDownList)gvDetails.FooterRow.FindControl("ddlTransMode");
                    TextBox txtCheque = (TextBox)gvDetails.FooterRow.FindControl("txtCheque");
                    TextBox txtChequeDate = (TextBox)gvDetails.FooterRow.FindControl("txtChequeDate");
                    DateTime CQDT = new DateTime();
                    string cqdt;
                    if (txtCheque.Text == "")
                    {
                        CQDT = DateTime.Parse("01/01/1900 00:00:00", dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        cqdt = CQDT.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        CQDT = DateTime.Parse(txtChequeDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        cqdt = CQDT.ToString("yyyy-MM-dd");
                    }
                    TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");
                    TextBox txtAmount = (TextBox)gvDetails.FooterRow.FindControl("txtAmount");

                    dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtDebited.Text + "'", txtDebitCD);
                    dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtCredited.Text + "'", txtCreditCD);
                    if (txtTransDate.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select a Date.";
                        ddlTransType.Focus();
                    }
                    else if (txtDebited.Text == "" || txtDebitCD.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Debit Particulars.";
                        txtDebited.Focus();
                    }
                    else if (txtCredited.Text == "" || txtCreditCD.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Credit Particulars.";
                        txtCredited.Focus();
                    }
                    else if ((ddlTransfor.Text == "PROJECT") && (txtCostPNM.Text == "--SELECT--"))
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Project.";
                        txtCostPNM.Focus();
                    }
                    else if (txtAmount.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Type Amount.";
                        txtAmount.Focus();
                    }
                    else
                    {
                        lblError.Visible = false;

                        if (ddlTransfor.Text == "PROJECT")
                        {
                        }
                        else
                        {
                            txtCostPNM.SelectedIndex = -1;
                        }

                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand();

                        if (btnEdit.Text == "EDIT")
                        {
                            cmd = new SqlCommand("SELECT TRANSNO FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + txtVouchNo.Text + "", conn);
                        }
                        else  //////// Edit Mode
                        {
                            Int64 EditTransNo = 0;
                            if (ddlVouch.Text == "--SELECT--")
                            {
                                EditTransNo = 0;
                            }
                            else
                                EditTransNo = Convert.ToInt64(ddlVouch.Text);

                            cmd = new SqlCommand("SELECT TRANSNO FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + EditTransNo + "", conn);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (btnEdit.Text == "EDIT")
                            {
                                query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "', @INTIME,'" + ipAddress + "')");

                                comm = new SqlCommand(query, conn);
                                comm.Parameters.Clear();
                                comm.Parameters.AddWithValue("@INTIME", inTm);
                                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                                int result = comm.ExecuteNonQuery();
                                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                                ///////////////// Refresh //////////////////

                                txtDebited.Text = "";
                                txtCredited.Text = "";
                                ddlTransfor.SelectedIndex = -1;
                                txtCostPNM.SelectedIndex = -1;
                                ddlTransMode.SelectedIndex = -1;
                                txtCheque.Text = "";
                                txtCheque.Enabled = false;
                                txtChequeDate.Text = "";
                                txtChequeDate.Enabled = false;
                                txtRemarks.Text = "";
                                txtAmount.Text = "";
                                txtCumInWords.Text = "";
                                txtTotInWords.Text = "";
                                txtTotAmount.Text = "";

                                dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                                if (lblVCount.Text == "")
                                {
                                    txtVouchNo.Text = "1";
                                }
                                else
                                {
                                    int vNo = int.Parse(lblVCount.Text);
                                    int totVno = vNo + 1;
                                    txtVouchNo.Text = totVno.ToString();
                                }

                                ShowGrid();
                                ddlTransType.Focus();
                            }
                            else ////Edit Mode
                            {
                                Int64 EditTransNo = 0;
                                if (ddlVouch.Text == "--SELECT--")
                                {
                                    EditTransNo = 0;
                                }
                                else
                                    EditTransNo = Convert.ToInt64(ddlVouch.Text);

                                query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, UPDATEUSERID, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + EditTransNo + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "','" + ipAddress + "')");

                                comm = new SqlCommand(query, conn);
                                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                                int result = comm.ExecuteNonQuery();
                                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                                ///////////////// Refresh //////////////////

                                ddlVouch.SelectedIndex = -1;
                                txtDebited.Text = "";
                                txtCredited.Text = "";
                                ddlTransfor.SelectedIndex = -1;
                                txtCostPNM.SelectedIndex = -1;
                                ddlTransMode.SelectedIndex = -1;
                                txtCheque.Text = "";
                                txtCheque.Enabled = false;
                                txtChequeDate.Text = "";
                                txtChequeDate.Enabled = false;
                                txtRemarks.Text = "";
                                txtAmount.Text = "";
                                txtCumInWords.Text = "";
                                txtTotInWords.Text = "";
                                txtTotAmount.Text = "";

                                ShowGrid_Edit();
                                ddlTransType.Focus();
                            }
                        }
                        else
                        {
                            query = " INSERT INTO GL_MTRANSMST (TRANSTP, TRANSDT, TRANSMY, TRANSNO, USERPC, USERID, INTIME, IPADDRESS) " +
                                    " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + ",'" + PCName + "','" + userName + "', @INTIME,'" + ipAddress + "')";
                            comm = new SqlCommand(query, conn);
                            comm.Parameters.Clear();
                            comm.Parameters.AddWithValue("@INTIME", inTm);
                            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                            comm.ExecuteNonQuery();
                            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                            query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "', @INTIME,'" + ipAddress + "')");

                            comm = new SqlCommand(query, conn);
                            comm.Parameters.Clear();
                            comm.Parameters.AddWithValue("@INTIME", inTm);
                            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                            int result = comm.ExecuteNonQuery();
                            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                            /////////// Refresh ///////////////
                            txtDebited.Text = "";
                            txtCredited.Text = "";
                            ddlTransfor.SelectedIndex = -1;
                            txtCostPNM.SelectedIndex = -1;
                            ddlTransMode.SelectedIndex = -1;
                            txtCheque.Text = "";
                            txtCheque.Enabled = false;
                            txtChequeDate.Text = "";
                            txtChequeDate.Enabled = false;
                            txtRemarks.Text = "";
                            txtAmount.Text = "";
                            txtCumInWords.Text = "";
                            txtTotInWords.Text = "";
                            txtTotAmount.Text = "";


                            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                            if (lblVCount.Text == "")
                            {
                                txtVouchNo.Text = "1";
                            }
                            else
                            {
                                int vNo = int.Parse(lblVCount.Text);
                                int totVno = vNo + 1;
                                txtVouchNo.Text = totVno.ToString();
                            }

                            ShowGrid();
                            ddlTransType.Focus();
                        }
                    }
                }
            }

            else if (e.CommandName.Equals("SavePrint"))
            {
                if (CookiesData == null)
                {
                    Response.Redirect("~/login/ui/SignIn");
                }
                else
                {
                    Session["TransType"] = "";
                    Session["TransDate"] = "";
                    Session["MonthYear"] = "";
                    Session["VouchNo"] = "";

                    Session["TransType"] = ddlTransType.Text;
                    Session["TransDate"] = txtTransDate.Text;
                    Session["MonthYear"] = txtTransYear.Text;


                    string serial = gvDetails.FooterRow.Cells[0].Text;
                    int sl = int.Parse(serial);
                    TextBox txtDebited = (TextBox)gvDetails.FooterRow.FindControl("txtDebited");
                    TextBox txtCredited = (TextBox)gvDetails.FooterRow.FindControl("txtCredited");
                    DropDownList ddlTransfor = (DropDownList)gvDetails.FooterRow.FindControl("ddlTransfor");
                    DropDownList txtCostPNM = (DropDownList)gvDetails.FooterRow.FindControl("txtCostPNM");
                    DropDownList ddlTransMode = (DropDownList)gvDetails.FooterRow.FindControl("ddlTransMode");
                    TextBox txtCheque = (TextBox)gvDetails.FooterRow.FindControl("txtCheque");
                    TextBox txtChequeDate = (TextBox)gvDetails.FooterRow.FindControl("txtChequeDate");
                    DateTime CQDT = new DateTime();
                    string cqdt;
                    if (txtCheque.Text == "")
                    {
                        CQDT = DateTime.Parse("01/01/1900 00:00:00", dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        cqdt = CQDT.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        CQDT = DateTime.Parse(txtChequeDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        cqdt = CQDT.ToString("yyyy-MM-dd");
                    }
                    TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");
                    TextBox txtAmount = (TextBox)gvDetails.FooterRow.FindControl("txtAmount");

                    if (txtTransDate.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select a Date.";
                        ddlTransType.Focus();
                    }
                    else if (txtDebited.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Debit Particulars.";
                        txtDebited.Focus();
                    }
                    else if (txtCredited.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Credit Particulars.";
                        txtCredited.Focus();
                    }
                    else if ((ddlTransfor.Text == "PROJECT") && (txtCostPNM.Text == "--SELECT--"))
                    {
                        lblError.Visible = true;
                        lblError.Text = "Select Project.";
                        txtCostPNM.Focus();
                    }
                    else if (txtAmount.Text == "")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Type Amount.";
                        txtAmount.Focus();
                    }
                    else
                    {
                        lblError.Visible = false;

                        dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtDebited.Text + "'", txtDebitCD);
                        dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtCredited.Text + "'", txtCreditCD);
                        if (ddlTransfor.Text == "PROJECT")
                        {
                        }
                        else
                        {
                            txtCostPNM.SelectedIndex = -1;
                        }

                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand();

                        if (btnEdit.Text == "EDIT")
                        {
                            cmd = new SqlCommand("SELECT TRANSNO FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + txtVouchNo.Text + "", conn);
                        }
                        else  //////// Edit Mode
                        {
                            Int64 EditTransNo = 0;
                            if (ddlVouch.Text == "--SELECT--")
                            {
                                EditTransNo = 0;
                            }
                            else
                                EditTransNo = Convert.ToInt64(ddlVouch.Text);

                            cmd = new SqlCommand("SELECT TRANSNO FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + EditTransNo + "", conn);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (btnEdit.Text == "EDIT")
                            {
                                Session["VouchNo"] = txtVouchNo.Text;

                                query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "', @INTIME,'" + ipAddress + "')");

                                comm = new SqlCommand(query, conn);
                                comm.Parameters.Clear();
                                comm.Parameters.AddWithValue("@INTIME", inTm);
                                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                                int result = comm.ExecuteNonQuery();
                                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                                ///////////////// Refresh //////////////////

                                txtDebited.Text = "";
                                txtCredited.Text = "";
                                ddlTransfor.SelectedIndex = -1;
                                txtCostPNM.SelectedIndex = -1;
                                ddlTransMode.SelectedIndex = -1;
                                txtCheque.Text = "";
                                txtCheque.Enabled = false;
                                txtChequeDate.Text = "";
                                txtChequeDate.Enabled = false;
                                txtRemarks.Text = "";
                                txtAmount.Text = "";
                                txtTotAmount.Text = "";
                                txtTotInWords.Text = "";
                                txtCumInWords.Text = "";

                                dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                                if (lblVCount.Text == "")
                                {
                                    txtVouchNo.Text = "1";
                                }
                                else
                                {
                                    int vNo = int.Parse(lblVCount.Text);
                                    int totVno = vNo + 1;
                                    txtVouchNo.Text = totVno.ToString();
                                }

                                ShowGrid();
                                ddlTransType.Focus();

                                ScriptManager.RegisterStartupScript(this,
                                this.GetType(), "OpenWindow", "window.open('../Report/Report/rptMultipleVoucher.aspx','_newtab');", true);
                            }
                            else ////Edit Mode
                            {
                                Session["VouchNo"] = ddlVouch.Text;

                                Int64 EditTransNo = 0;
                                if (ddlVouch.Text == "--SELECT--")
                                {
                                    EditTransNo = 0;
                                }
                                else
                                    EditTransNo = Convert.ToInt64(ddlVouch.Text);

                                query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, UPDATEUSERID, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + EditTransNo + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "','" + ipAddress + "')");

                                comm = new SqlCommand(query, conn);
                                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                                int result = comm.ExecuteNonQuery();
                                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                                ///////////////// Refresh //////////////////

                                ddlVouch.SelectedIndex = -1;
                                txtDebited.Text = "";
                                txtCredited.Text = "";
                                ddlTransfor.SelectedIndex = -1;
                                txtCostPNM.SelectedIndex = -1;
                                ddlTransMode.SelectedIndex = -1;
                                txtCheque.Text = "";
                                txtCheque.Enabled = false;
                                txtChequeDate.Text = "";
                                txtChequeDate.Enabled = false;
                                txtRemarks.Text = "";
                                txtAmount.Text = "";
                                txtTotAmount.Text = "";
                                txtTotInWords.Text = "";
                                txtCumInWords.Text = "";


                                ShowGrid_Edit();
                                ddlTransType.Focus();

                                ScriptManager.RegisterStartupScript(this,
                                this.GetType(), "OpenWindow", "window.open('../Report/Report/rptMultipleVoucherEdit.aspx','_newtab');", true);
                            }
                        }
                        else
                        {
                            Session["VouchNo"] = txtVouchNo.Text;

                            query = " INSERT INTO GL_MTRANSMST (TRANSTP, TRANSDT, TRANSMY, TRANSNO, USERPC, USERID, INTIME, IPADDRESS) " +
                                    " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + ",'" + PCName + "','" + userName + "', '" + inTm + "','" + ipAddress + "')";
                            comm = new SqlCommand(query, conn);
                            cmd.Parameters.Clear();
                            // cmd.Parameters.AddWithValue("@INTIME", inTm);
                            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                            comm.ExecuteNonQuery();
                            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                            query = (" INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, " +
                                         " REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                                         " VALUES ('" + ddlTransType.Text + "','" + trDT + "','" + txtTransYear.Text + "'," + txtVouchNo.Text + "," + sl + ",'" + ddlTransfor.Text + "', " +
                                         " '" + txtCostPNM.SelectedValue + "','" + ddlTransMode.Text + "','" + txtDebitCD.Text + "','" + txtCreditCD.Text + "','" + txtCheque.Text + "', " +
                                         " '" + cqdt + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','" + PCName + "','" + userName + "', '" + inTm + "','" + ipAddress + "')");

                            comm = new SqlCommand(query, conn);
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@INTIME", inTm);
                            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                            int result = comm.ExecuteNonQuery();
                            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                            /////////// Refresh ///////////////
                            txtDebited.Text = "";
                            txtCredited.Text = "";
                            ddlTransfor.SelectedIndex = -1;
                            txtCostPNM.SelectedIndex = -1;
                            ddlTransMode.SelectedIndex = -1;
                            txtCheque.Text = "";
                            txtCheque.Enabled = false;
                            txtChequeDate.Text = "";
                            txtChequeDate.Enabled = false;
                            txtRemarks.Text = "";
                            txtAmount.Text = "";
                            txtTotAmount.Text = "";
                            txtTotInWords.Text = "";
                            txtCumInWords.Text = "";

                            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MTRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
                            if (lblVCount.Text == "")
                            {
                                txtVouchNo.Text = "1";
                            }
                            else
                            {
                                int vNo = int.Parse(lblVCount.Text);
                                int totVno = vNo + 1;
                                txtVouchNo.Text = totVno.ToString();
                            }

                            ShowGrid();
                            ddlTransType.Focus();

                            ScriptManager.RegisterStartupScript(this,
                                this.GetType(), "OpenWindow", "window.open('../Report/Report/rptMultipleVoucher.aspx','_newtab');", true);
                        }
                    }
                }
            }
        }


        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                DateTime transDT = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string trDT = transDT.ToString("yyyy-MM-dd");

                if (btnEdit.Text == "EDIT")
                {
                    gvDetails.EditIndex = e.NewEditIndex;
                    ShowGrid();

                    Label lblSLEdit = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblSLEdit");
                    DropDownList ddlTransforEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlTransforEdit");

                    dbFunctions.lblAdd(@"SELECT TRANSFOR FROM GL_MTRANS WHERE TRANSTP='" + ddlTransType.Text + "' AND TRANSDT='" + trDT + "' AND TRANSMY='" + txtTransYear.Text + "' AND TRANSNO= " + txtVouchNo.Text + " AND SERIALNO= " + lblSLEdit.Text + "", lblTransforEdit);
                    ddlTransforEdit.Text = lblTransforEdit.Text;

                    DropDownList txtCostPNMEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("txtCostPNMEdit");

                    if (ddlTransforEdit.Text == "PROJECT")
                    {
                        txtCostPNMEdit.Enabled = true;
                    }
                    else
                    {
                        txtCostPNMEdit.SelectedIndex = -1;
                        txtCostPNMEdit.Enabled = false;
                    }

                    DropDownList ddlTransModeEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlTransModeEdit");
                    dbFunctions.lblAdd(@"SELECT TRANSMODE FROM GL_MTRANS WHERE TRANSTP='" + ddlTransType.Text + "' AND TRANSDT='" + trDT + "' AND TRANSMY='" + txtTransYear.Text + "' AND TRANSNO= " + txtVouchNo.Text + " AND SERIALNO= " + lblSLEdit.Text + "", lblTransmodeEdit);
                    ddlTransModeEdit.Text = lblTransmodeEdit.Text;

                    TextBox txtChequeEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtChequeEdit");
                    TextBox txtChequeDateEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtChequeDateEdit");
                    TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtRemarksEdit");

                    DropDownList ddlCostPoolName = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("txtCostPNMEdit");
                    dbFunctions.dropDown_Bind(ddlCostPoolName, "id", "select", @"SELECT COSTPNM nm, COSTPID id FROM GL_COSTP");
                    var costpid = dbFunctions.StringData(@"SELECT COSTPID FROM GL_MTRANS WHERE TRANSTP='" + ddlTransType.Text +
                                               "' AND TRANSDT='" + trDT + "' AND TRANSMY='" + txtTransYear.Text +
                                               "' AND TRANSNO= " + txtVouchNo.Text + " AND SERIALNO= " + lblSLEdit.Text +
                                               "");
                    if (costpid != "")
                        ddlCostPoolName.SelectedIndex = ddlCostPoolName.Items.IndexOf(ddlCostPoolName.Items.FindByValue(costpid));


                    if (ddlTransModeEdit.Text == "CASH CHEQUE")
                    {
                        txtChequeEdit.Enabled = true;
                        txtChequeDateEdit.Enabled = true;
                    }
                    else if (ddlTransModeEdit.Text == "A/C PAYEE CHEQUE")
                    {
                        txtChequeEdit.Enabled = true;
                        txtChequeDateEdit.Enabled = true;
                    }
                    else
                    {
                        txtChequeEdit.Text = "";
                        txtChequeDateEdit.Text = "";
                        txtChequeEdit.Enabled = false;
                        txtChequeDateEdit.Enabled = false;
                    }
                }
                else //// Edit mode
                {
                    gvDetails.EditIndex = e.NewEditIndex;
                    ShowGrid_Edit();

                    Int64 EditTransNo = 0;
                    if (ddlVouch.Text == "--SELECT--")
                    {
                        EditTransNo = 0;
                    }
                    else
                        EditTransNo = Convert.ToInt64(ddlVouch.Text);

                    Label lblSLEdit = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblSLEdit");
                    DropDownList ddlTransforEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlTransforEdit");

                    dbFunctions.lblAdd(@"SELECT TRANSFOR FROM GL_MTRANS WHERE TRANSTP='" + ddlTransType.Text + "' AND TRANSDT='" + trDT + "' AND TRANSMY='" + txtTransYear.Text + "' AND TRANSNO= " + EditTransNo + " AND SERIALNO= " + lblSLEdit.Text + "", lblTransforEdit);
                    ddlTransforEdit.Text = lblTransforEdit.Text;

                    DropDownList txtCostPNMEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("txtCostPNMEdit");

                    if (ddlTransforEdit.Text == "PROJECT")
                    {
                        txtCostPNMEdit.Enabled = true;
                    }
                    else
                    {
                        txtCostPNMEdit.SelectedIndex = -1;
                        txtCostPNMEdit.Enabled = false;
                    }

                    DropDownList ddlTransModeEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlTransModeEdit");
                    dbFunctions.lblAdd(@"SELECT TRANSMODE FROM GL_MTRANS WHERE TRANSTP='" + ddlTransType.Text + "' AND TRANSDT='" + trDT + "' AND TRANSMY='" + txtTransYear.Text + "' AND TRANSNO= " + EditTransNo + " AND SERIALNO= " + lblSLEdit.Text + "", lblTransmodeEdit);
                    ddlTransModeEdit.Text = lblTransmodeEdit.Text;


                    DropDownList ddlCostPoolName = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("txtCostPNMEdit");
                    dbFunctions.dropDown_Bind(ddlCostPoolName, "id", "select", @"SELECT COSTPNM nm, COSTPID id FROM GL_COSTP");
                    var costpid = dbFunctions.StringData(@"SELECT COSTPID FROM GL_MTRANS WHERE TRANSTP='" + ddlTransType.Text + "' AND TRANSDT='" + trDT + "' AND TRANSMY='" + txtTransYear.Text + "' AND TRANSNO= " + EditTransNo + " AND SERIALNO= " + lblSLEdit.Text + "");
                    if (costpid != "")
                        ddlCostPoolName.SelectedIndex = ddlCostPoolName.Items.IndexOf(ddlCostPoolName.Items.FindByValue(costpid));



                    TextBox txtChequeEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtChequeEdit");
                    TextBox txtChequeDateEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtChequeDateEdit");
                    TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtRemarksEdit");

                    if (ddlTransModeEdit.Text == "CASH CHEQUE")
                    {
                        txtChequeEdit.Enabled = true;
                        txtChequeDateEdit.Enabled = true;
                    }
                    else if (ddlTransModeEdit.Text == "A/C PAYEE CHEQUE")
                    {
                        txtChequeEdit.Enabled = true;
                        txtChequeDateEdit.Enabled = true;
                    }
                    else
                    {
                        txtChequeEdit.Text = "";
                        txtChequeDateEdit.Text = "";
                        txtChequeEdit.Enabled = false;
                        txtChequeDateEdit.Enabled = false;
                    }

                }

                TextBox txtDebitedEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtDebitedEdit");
                txtDebitedEdit.Focus();
            }
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (btnEdit.Text == "EDIT")
            {
                gvDetails.EditIndex = -1;
                ShowGrid();
            }
            else   ///////// Edit Mode
            {
                gvDetails.EditIndex = -1;
                ShowGrid_Edit();
            }
        }

        //protected void txtDebitedEdit_TextChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        //    TextBox txtDebitedEdit = (TextBox)row.FindControl("txtDebitedEdit");
        //    TextBox txtCreditedEdit = (TextBox)row.FindControl("txtCreditedEdit");

        //    dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtDebitedEdit.Text + "'", txtDebitCD);
        //    txtCreditedEdit.Focus();
        //}

        //protected void txtCreditedEdit_TextChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        //    TextBox txtCreditedEdit = (TextBox)row.FindControl("txtCreditedEdit");
        //    DropDownList ddlTransforEdit = (DropDownList)row.FindControl("ddlTransforEdit");

        //    dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtCreditedEdit.Text + "'", txtCreditCD);
        //    ddlTransforEdit.Focus();
        //}

        protected void ddlTransforEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList ddlTransforEdit = (DropDownList)row.FindControl("ddlTransforEdit");

            DropDownList txtCostPNMEdit = (DropDownList)row.FindControl("txtCostPNMEdit");
            DropDownList ddlTransModeEdit = (DropDownList)row.FindControl("ddlTransModeEdit");

            if (ddlTransforEdit.Text == "PROJECT")
            {
                txtCostPNMEdit.Enabled = true;
                txtCostPNMEdit.Focus();
            }
            else
            {
                txtCostPNMEdit.SelectedIndex = -1;
                txtCostPNMEdit.Enabled = false;
                ddlTransModeEdit.Focus();
            }
        }

        protected void txtCostPNMEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtCostPNMEdit = (TextBox)row.FindControl("txtCostPNMEdit");
            DropDownList ddlTransModeEdit = (DropDownList)row.FindControl("ddlTransModeEdit");

            ddlTransModeEdit.Focus();
        }

        protected void ddlTransModeEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList ddlTransModeEdit = (DropDownList)row.FindControl("ddlTransModeEdit");
            TextBox txtChequeEdit = (TextBox)row.FindControl("txtChequeEdit");
            TextBox txtChequeDateEdit = (TextBox)row.FindControl("txtChequeDateEdit");
            TextBox txtRemarksEdit = (TextBox)row.FindControl("txtRemarksEdit");

            if (ddlTransModeEdit.Text == "CASH CHEQUE")
            {
                txtChequeEdit.Enabled = true;
                txtChequeDateEdit.Enabled = true;
                txtChequeEdit.Focus();
            }
            else if (ddlTransModeEdit.Text == "A/C PAYEE CHEQUE")
            {
                txtChequeEdit.Enabled = true;
                txtChequeDateEdit.Enabled = true;
                txtChequeEdit.Focus();
            }
            else
            {
                txtChequeEdit.Text = "";
                txtChequeDateEdit.Text = "";
                txtChequeEdit.Enabled = false;
                txtChequeDateEdit.Enabled = false;
                txtRemarksEdit.Focus();
            }
        }

        protected void txtChequeDateEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtRemarksEdit = (TextBox)row.FindControl("txtRemarksEdit");
            txtRemarksEdit.Focus();
        }

        protected void txtAmountEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtAmountEdit = (TextBox)row.FindControl("txtAmountEdit");
            ImageButton imgbtnUpdate = (ImageButton)row.FindControl("imgbtnUpdate");

            txtCumInWords.Text = "";
            decimal dec;
            Boolean ValidInput = Decimal.TryParse(txtAmountEdit.Text, out dec);
            if (!ValidInput)
            {
                txtCumInWords.ForeColor = System.Drawing.Color.Red;
                txtCumInWords.Text = "Enter the Proper Amount...";
                return;
            }
            if (txtAmountEdit.Text.ToString().Trim() == "")
            {
                txtCumInWords.ForeColor = System.Drawing.Color.Red;
                txtCumInWords.Text = "Amount Cannot Be Empty...";
                return;
            }
            else
            {
                if (Convert.ToDecimal(txtAmountEdit.Text) == 0)
                {
                    txtCumInWords.ForeColor = System.Drawing.Color.Red;
                    txtCumInWords.Text = "Amount Cannot Be Empty...";
                    return;
                }
            }

            string x1 = "";
            string x2 = "";

            if (txtAmountEdit.Text.Contains("."))
            {
                x1 = txtAmountEdit.Text.ToString().Trim().Substring(0, txtAmountEdit.Text.ToString().Trim().IndexOf("."));
                x2 = txtAmountEdit.Text.ToString().Trim().Substring(txtAmountEdit.Text.ToString().Trim().IndexOf(".") + 1);
            }
            else
            {
                x1 = txtAmountEdit.Text.ToString().Trim();
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

            txtAmountEdit.Text = x1 + "." + x2;

            if (x2.Length > 2)
            {
                txtAmountEdit.Text = Math.Round(Convert.ToDouble(txtAmountEdit.Text), 2).ToString().Trim();
            }

            string AmtConv = dbFunctions.SpellAmount.MoneyConvFn(txtAmountEdit.Text.ToString().Trim());
            //string amntComma = SpellAmount.comma(Convert.ToDecimal(txtAmount.Text));
            //Label3.Text = amntComma;

            txtCumInWords.Text = AmtConv.Trim();
            txtCumInWords.ForeColor = System.Drawing.Color.Black;


            imgbtnUpdate.Focus();
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string userName = CookiesData["USERID"].ToString();
                string ipAddress = CookiesData["IpAddress"].ToString();
                string PCName = CookiesData["PCName"].ToString();
                string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
                SqlConnection conn = new SqlConnection(dbFunctions.Connection);

                string query = "";
                SqlCommand comm = new SqlCommand(query, conn);

                DateTime transDT = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string trDT = transDT.ToString("yyyy-MM-dd");

                Label lblSLEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSLEdit");
                int sl = int.Parse(lblSLEdit.Text);
                TextBox txtDebitedEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtDebitedEdit");
                TextBox txtCreditedEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtCreditedEdit");
                DropDownList ddlTransforEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlTransforEdit");
                DropDownList txtCostPNMEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("txtCostPNMEdit");
                DropDownList ddlTransModeEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlTransModeEdit");
                TextBox txtChequeEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtChequeEdit");
                TextBox txtChequeDateEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtChequeDateEdit");
                DateTime CQDT = new DateTime();
                string cqdt;
                if (txtChequeEdit.Text == "")
                {
                    CQDT = DateTime.Parse("01/01/1900 00:00:00", dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    cqdt = CQDT.ToString("yyyy-MM-dd");
                }
                else
                {
                    CQDT = DateTime.Parse(txtChequeDateEdit.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    cqdt = CQDT.ToString("yyyy-MM-dd");
                }
                TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarksEdit");
                TextBox txtAmountEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtAmountEdit");

                if (txtTransDate.Text == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Select a Date.";
                    ddlTransType.Focus();
                }
                else if (txtDebitedEdit.Text == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Select Debit Particulars.";
                    txtDebitedEdit.Focus();
                }
                else if (txtCreditedEdit.Text == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Select Credit Particulars.";
                    txtCreditedEdit.Focus();
                }
                else if ((ddlTransforEdit.Text == "PROJECT") && (txtCostPNMEdit.Text == "--SELECT--"))
                {
                    lblError.Visible = true;
                    lblError.Text = "Select Project.";
                    txtCostPNMEdit.Focus();
                }
                else if (txtAmountEdit.Text == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Type Amount.";
                    txtAmountEdit.Focus();
                }
                else
                {
                    lblError.Visible = false;

                    dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtDebitedEdit.Text + "'", txtDebitCD);
                    dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + txtCreditedEdit.Text + "'", txtCreditCD);
                    if (ddlTransforEdit.Text == "PROJECT")
                    {
                    }
                    else
                    {
                        txtCostPNMEdit.SelectedIndex = -1;
                    }

                    string intime = dbFunctions.timezone(DateTime.Now).ToString("yyyy-MM-dd");

                    //DateTime intime = DateTime.Now;
                    if (btnEdit.Text == "EDIT")
                    {
                        try
                        {
                            // logdata add start //
                            string lotileng = CookiesData["Location"].ToString();
                            ipAddress = CookiesData["IpAddress"].ToString();
                            string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  
                        '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+CONVERT(NVARCHAR(50),SERIALNO,103)+'  '+ISNULL(TRANSFOR,'(NULL)')+'  '+ISNULL(COSTPID,'(NULL)')+'  '+ISNULL(TRANSMODE,'(NULL)')+'  
                        '+ISNULL(DEBITCD,'(NULL)')+'  '+ISNULL(CREDITCD,'(NULL)')+'  '+ISNULL(CHEQUENO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),CHEQUEDT,103),'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),AMOUNT,103),'(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  
                        '+ISNULL(UPUSERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),
                        '(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDTIME,103),'(NULL)')+'  
                        '+ISNULL(IPADDRESS,'(NULL)')+'  '+ISNULL(UPDIPADD,'(NULL)') FROM GL_MTRANS 
                       WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + txtVouchNo.Text + " AND SERIALNO = " + sl + "");
                            string logid = "UPDATE";
                            string tableid = "GL_MTRANS";
                            dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                            // logdata add end //
                        }
                        catch (Exception ex)
                        {

                        }

                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand(" UPDATE GL_MTRANS SET TRANSFOR = '" + ddlTransforEdit.Text + "', COSTPID = '" + txtCostPNMEdit.SelectedValue + "', TRANSMODE = '" + ddlTransModeEdit.Text + "', DEBITCD = '" + txtDebitCD.Text + "', CREDITCD = '" + txtCreditCD.Text + "', CHEQUENO = '" + txtChequeEdit.Text + "', CHEQUEDT = '" + cqdt + "', " +
                                                        " AMOUNT = " + txtAmountEdit.Text + ", REMARKS = '" + txtRemarksEdit.Text + "', UPUSERPC = '" + PCName + "',UPDTIME='" + intime + "' , UPDATEUSERID = '" + userName + "', UPDIPADD = '" + ipAddress + "' WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + txtVouchNo.Text + " AND SERIALNO = " + sl + "", conn);
                        cmd.ExecuteNonQuery();
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                        gvDetails.EditIndex = -1;
                        ShowGrid();
                    }
                    else /// Edit Mode
                    {
                        Int64 EditTransNo = 0;
                        if (ddlVouch.Text == "--SELECT--")
                        {
                            EditTransNo = 0;
                        }
                        else
                            EditTransNo = Convert.ToInt64(ddlVouch.Text);


                        try
                        {
                            // logdata add start //
                            string lotileng = CookiesData["Location"].ToString();
                            ipAddress = CookiesData["IpAddress"].ToString();
                            string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  
                        '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+CONVERT(NVARCHAR(50),SERIALNO,103)+'  '+ISNULL(TRANSFOR,'(NULL)')+'  '+ISNULL(COSTPID,'(NULL)')+'  '+ISNULL(TRANSMODE,'(NULL)')+'  
                        '+ISNULL(DEBITCD,'(NULL)')+'  '+ISNULL(CREDITCD,'(NULL)')+'  '+ISNULL(CHEQUENO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),CHEQUEDT,103),'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),AMOUNT,103),'(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  
                        '+ISNULL(UPUSERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),
                        '(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDTIME,103),'(NULL)')+'  
                        '+ISNULL(IPADDRESS,'(NULL)')+'  '+ISNULL(UPDIPADD,'(NULL)') FROM GL_MTRANS 
                        WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + EditTransNo + " AND SERIALNO = " + sl + " ");
                            string logid = "UPDATE";
                            string tableid = "GL_MTRANS";
                            dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                            // logdata add end //
                        }
                        catch (Exception ex)
                        {

                        }



                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand(" UPDATE GL_MTRANS SET TRANSFOR = '" + ddlTransforEdit.Text + "', COSTPID = '" + txtCostPNMEdit.SelectedValue + "', TRANSMODE = '" + ddlTransModeEdit.Text + "', DEBITCD = '" + txtDebitCD.Text + "', CREDITCD = '" + txtCreditCD.Text + "', CHEQUENO = '" + txtChequeEdit.Text + "', CHEQUEDT = '" + cqdt + "', " +
                                                        " AMOUNT = " + txtAmountEdit.Text + ", REMARKS = '" + txtRemarksEdit.Text + "', UPUSERPC = '" + PCName + "', UPDTIME='" + intime + "' ,UPDATEUSERID = '" + userName + "', UPDIPADD = '" + ipAddress + "' WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + EditTransNo + " AND SERIALNO = " + sl + "", conn);
                        cmd.ExecuteNonQuery();
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                        gvDetails.EditIndex = -1;
                        ShowGrid_Edit();
                    }
                }
            }

        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string userName = CookiesData["USERID"].ToString();
                string ipAddress = CookiesData["IpAddress"].ToString();
                string PCName = CookiesData["PCName"].ToString();
                string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
                SqlConnection conn = new SqlConnection(dbFunctions.Connection);

                string query = "";
                SqlCommand comm = new SqlCommand(query, conn);

                DateTime transDT = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string trDT = transDT.ToString("yyyy-MM-dd");

                if (btnEdit.Text == "EDIT")
                {

                    Label lblSL = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSL");

                    SqlCommand cmd1 = new SqlCommand(" SELECT * FROM GL_MTRANS WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + txtVouchNo.Text + " ", conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1);
                    if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                    try
                    {
                        // logdata add start //
                        string lotileng = CookiesData["Location"].ToString();
                        ipAddress = CookiesData["IpAddress"].ToString();
                        string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  
                        '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+CONVERT(NVARCHAR(50),SERIALNO,103)+'  '+ISNULL(TRANSFOR,'(NULL)')+'  '+ISNULL(COSTPID,'(NULL)')+'  '+ISNULL(TRANSMODE,'(NULL)')+'  
                        '+ISNULL(DEBITCD,'(NULL)')+'  '+ISNULL(CREDITCD,'(NULL)')+'  '+ISNULL(CHEQUENO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),CHEQUEDT,103),'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),AMOUNT,103),'(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  
                        '+ISNULL(UPUSERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),
                        '(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDTIME,103),'(NULL)')+'  
                        '+ISNULL(IPADDRESS,'(NULL)')+'  '+ISNULL(UPDIPADD,'(NULL)') FROM GL_MTRANS 
                        WHERE TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + txtVouchNo.Text + "' AND TRANSMY = '" + txtTransYear.Text + "' " +
                        "and  TRANSDT = '" + trDT + "' and SERIALNO = '" + lblSL.Text + "'");
                        string logid = "DELETE";
                        string tableid = "GL_MTRANS";
                        dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                        // logdata add end //
                    }
                    catch (Exception ex)
                    {

                    }

                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand("delete from GL_MTRANS where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + txtVouchNo.Text + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "' and SERIALNO = '" + lblSL.Text + "'", conn);
                        cmd.ExecuteNonQuery();
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                    }
                    else
                    {
                        try
                        {
                            // logdata add start //
                            string lotileng = CookiesData["Location"].ToString();
                            ipAddress = CookiesData["IpAddress"].ToString();
                            string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  
                            '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+ISNULL(USERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  
                            '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  
                            '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+ISNULL(IPADDRESS,'(NULL)') FROM GL_MTRANSMST 
                            where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + txtVouchNo.Text + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "'");
                            string logid = "DELETE";
                            string tableid = "GL_MTRANSMST";
                            dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                            // logdata add end //
                        }
                        catch (Exception ex)
                        {

                        }


                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand("delete from GL_MTRANS where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + txtVouchNo.Text + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "' and SERIALNO = '" + lblSL.Text + "'", conn);
                        cmd.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand("delete from GL_MTRANSMST where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + txtVouchNo.Text + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "'", conn);
                        cmd2.ExecuteNonQuery();
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                    }


                    gvDetails.EditIndex = -1;
                    ShowGrid();
                }

                else   /////// Edit Mode
                {
                    Int64 EditTransNo = 0;
                    if (ddlVouch.Text == "--SELECT--")
                    {
                        EditTransNo = 0;
                    }
                    else
                        EditTransNo = Convert.ToInt64(ddlVouch.Text);

                    Label lblSL = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSL");

                    SqlCommand cmd1 = new SqlCommand(" SELECT * FROM GL_MTRANS WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSDT = '" + trDT + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSNO = " + EditTransNo + " ", conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1);
                    if (conn.State != System.Data.ConnectionState.Closed) conn.Close();


                    try
                    {
                        // logdata add start //
                        string lotileng = CookiesData["Location"].ToString();
                        ipAddress = CookiesData["IpAddress"].ToString();
                        string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  
                        '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+CONVERT(NVARCHAR(50),SERIALNO,103)+'  '+ISNULL(TRANSFOR,'(NULL)')+'  '+ISNULL(COSTPID,'(NULL)')+'  '+ISNULL(TRANSMODE,'(NULL)')+'  
                        '+ISNULL(DEBITCD,'(NULL)')+'  '+ISNULL(CREDITCD,'(NULL)')+'  '+ISNULL(CHEQUENO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),CHEQUEDT,103),'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),AMOUNT,103),'(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  
                        '+ISNULL(UPUSERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),
                        '(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDTIME,103),'(NULL)')+'  
                        '+ISNULL(IPADDRESS,'(NULL)')+'  '+ISNULL(UPDIPADD,'(NULL)') FROM GL_MTRANS 
                        WHERE TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + EditTransNo + "' AND TRANSMY = '" + txtTransYear.Text + "' " +
                        "and  TRANSDT = '" + trDT + "' and SERIALNO = '" + lblSL.Text + "'");
                        string logid = "DELETE";
                        string tableid = "GL_MTRANS";
                        dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                        // logdata add end //
                    }
                    catch (Exception ex)
                    {

                    }



                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand("delete from GL_MTRANS where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + EditTransNo + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "' and SERIALNO = '" + lblSL.Text + "'", conn);
                        cmd.ExecuteNonQuery();
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                    }
                    else
                    {
                        try
                        {
                            // logdata add start //
                            string lotileng = CookiesData["Location"].ToString();
                            ipAddress = CookiesData["IpAddress"].ToString();
                            string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  
                            '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+ISNULL(USERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  
                            '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  
                            '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+ISNULL(IPADDRESS,'(NULL)') FROM GL_MTRANSMST 
                             where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + EditTransNo + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "'");
                            string logid = "DELETE";
                            string tableid = "GL_MTRANSMST";
                            dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                            // logdata add end //
                        }
                        catch (Exception ex)
                        {

                        }


                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        SqlCommand cmd = new SqlCommand("delete from GL_MTRANS where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + EditTransNo + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "' and SERIALNO = '" + lblSL.Text + "'", conn);
                        cmd.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand("delete from GL_MTRANSMST where TRANSTP = '" + ddlTransType.Text + "' and TRANSNO ='" + EditTransNo + "' AND TRANSMY = '" + txtTransYear.Text + "' and  TRANSDT = '" + trDT + "'", conn);
                        cmd2.ExecuteNonQuery();
                        if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                    }

                    gvDetails.EditIndex = -1;
                    ShowGrid_Edit();
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                DateTime transDT = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string trDT = transDT.ToString("yyyy-MM-dd");

                string uTp = CookiesData["USERTYPE"].ToString();
                string brCD = CookiesData["BrCD"].ToString();

                if (btnEdit.Text == "EDIT")
                {
                    btnEdit.Text = "NEW";
                    btnPrint.Visible = true;
                    txtVouchNo.Visible = false;
                    ddlVouch.Visible = true;
                    txtTotAmount.Text = "";
                    txtTotInWords.Text = "";
                    txtCumInWords.Text = "";

                    //if (uTp == "COMPADMIN")
                    //{
                    dbFunctions.dropDown_Bind(ddlVouch, "", "select", "SELECT TRANSNO nm FROM GL_MTRANSMST WHERE TRANSTP = '" + ddlTransType.Text + "' AND TRANSMY = '" + txtTransYear.Text + "' AND TRANSDT = '" + trDT + "'");
                    //}
                    //else
                    //{

                    //    dbFunctions.dropDownAddWithSelect(ddlVouch, "SELECT DISTINCT GL_MTRANS.TRANSNO FROM GL_MTRANS INNER JOIN GL_COSTP ON GL_MTRANS.COSTPID = GL_COSTP.COSTPID WHERE GL_MTRANS.TRANSTP ='" + ddlTransType.Text + "' AND GL_MTRANS.TRANSMY ='" + txtTransYear.Text + "' AND GL_MTRANS.TRANSDT ='" + trDT + "' AND GL_COSTP.CATID ='" + brCD + "'");
                    //}
                    ddlVouch.SelectedIndex = -1;
                    ShowGrid_Edit();
                    ddlTransType.Focus();
                }
                else //// Edit Mode
                {
                    btnEdit.Text = "EDIT";
                    btnPrint.Visible = false;
                    txtVouchNo.Visible = true;
                    ddlVouch.Visible = false;
                    txtTotAmount.Text = "";
                    txtTotInWords.Text = "";
                    txtCumInWords.Text = "";
                    ShowGrid();
                    ddlTransType.Focus();
                }
            }
        }

        protected void ddlVouch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowGrid_Edit();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                if (ddlVouch.Text == "--SELECT--")
                {
                    lblError.Visible = true;
                    lblError.Text = "Select Voucher No.";
                }
                else
                {
                    lblError.Visible = false;
                    Session["TransType"] = ddlTransType.Text;
                    Session["TransDate"] = txtTransDate.Text;
                    Session["MonthYear"] = txtTransYear.Text;
                    if (btnEdit.Text != "EDIT")
                        Session["VouchNo"] = ddlVouch.Text;
                    else
                        Session["VouchNo"] = txtVouchNo.Text;

                    ScriptManager.RegisterStartupScript(this,
                                    this.GetType(), "OpenWindow", "window.open('../Report/Report/rptMultipleVoucherEdit.aspx','_newtab');", true);
                }
            }
        }

    }
}