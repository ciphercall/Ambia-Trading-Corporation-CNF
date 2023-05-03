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
using alchemy.accounts.Interface;
using alchemy.accounts.DataAccess;
using alchemySoft;

namespace alchemy.accounts.UI
{
    public partial class OpeningBalance : System.Web.UI.Page
    {
        AccountDataAccess dob = new AccountDataAccess();
        AccountInterface iob = new AccountInterface();
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
                string formLink = "/Accounts/UI/OpeningBalance.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            lblMY.Visible = false;
                            lblTotCount.Visible = false;
                            lblVCount.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            date_Vouch();
            BindEmployeeDetails();
        }

        public void date_Vouch()
        {
            DateTime transdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string month = transdate.ToString("MMM").ToUpper();
            string years = transdate.ToString("yy");

            lblMY.Text = month + "-" + years;
            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_MASTER where TRANSMY='" + lblMY.Text + "' and TRANSTP = 'OPEN'", lblVCount);
            if (lblVCount.Text == "")
            {
                lblTotCount.Text = "1";
            }
            else
            {
                int vNo = int.Parse(lblVCount.Text);
                int totVno = vNo + 1;
                lblTotCount.Text = totVno.ToString();
            }
        }

        //protected void txtCreditAmt_TextChanged(object sender, EventArgs e)
        //{

        //}
        //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        //public static string[] GetCompletionListDebit(string prefixText, int count, string contextKey)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(dbFunctions.Connection);
        //    SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE STATUSCD='P' and ACCOUNTNM LIKE '" + prefixText + "%'", conn);
        //    SqlDataReader oReader;
        //    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
        //    List<String> CompletionSet = new List<string>();
        //    oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    while (oReader.Read())
        //        CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
        //    return CompletionSet.ToArray();
        //}

        //protected void txtDebitCD_TextChanged(object sender, EventArgs e)
        //{

        //    GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        //    //NamingContainer return the container that the control sits in
        //    TextBox other = (TextBox)row.FindControl("txtDebitCD");
        //    lbltxtChg.Text = other.Text;

        //    dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + lbltxtChg.Text + "'", lbltxtShw);
        //    TextBox DebtAmt = (TextBox)row.FindControl("txtDbAmt");
        //    DebtAmt.Focus();
        //}
        protected void BindEmployeeDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            DateTime transdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string transDT = transdate.ToString("yyyy/MM/dd");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT convert(nvarchar(20),dbo.GL_MASTER.TRANSDT,103) as TRANSDT,dbo.GL_MASTER.TRANSNO,  dbo.GL_ACCHART.ACCOUNTNM, dbo.GL_MASTER.DEBITAMT, dbo.GL_MASTER.CREDITAMT " +
                                           " FROM dbo.GL_MASTER INNER JOIN dbo.GL_ACCHART ON dbo.GL_MASTER.DEBITCD = dbo.GL_ACCHART.ACCOUNTCD WHERE (dbo.GL_MASTER.TRANSTP = 'OPEN') and  dbo.GL_MASTER.TRANSDT = '" + transDT + "' order by TRANSNO", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();


                if (gvDetails.EditIndex == -1)
                {
                    TextBox txtAccHead = (TextBox)gvDetails.FooterRow.FindControl("txtDebitCD");
                    txtAccHead.Focus();
                    Decimal totDebitAmnt = 0;
                    Decimal a = 0;
                    foreach (GridViewRow grid in gvDetails.Rows)
                    {
                        Label Per = (Label)grid.Cells[3].FindControl("lblDebitAmt");
                        if (Per.Text == "")
                        {
                            Per.Text = "0";
                        }
                        else
                        {
                            Per.Text = Per.Text;
                        }
                        String Perf = Per.Text;
                        totDebitAmnt = Convert.ToDecimal(Perf);
                        a += totDebitAmnt;
                        txtTotDebit.Text = a.ToString();
                    }
                    a += totDebitAmnt;

                    Decimal totCreditAmnt = 0;
                    Decimal b = 0;
                    foreach (GridViewRow grid in gvDetails.Rows)
                    {
                        Label Per = (Label)grid.Cells[5].FindControl("lblCrAmt");
                        if (Per.Text == "")
                        {
                            Per.Text = "0";
                        }
                        else
                        {
                            Per.Text = Per.Text;
                        }
                        String Perf1 = Per.Text;
                        totCreditAmnt = Convert.ToDecimal(Perf1);
                        b += totCreditAmnt;
                        txtTotCredit.Text = b.ToString();
                    }
                    b += totCreditAmnt;
                }
                else
                {

                }
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
                TextBox txtAccHead = (TextBox)gvDetails.FooterRow.FindControl("txtDebitCD");
                txtAccHead.Focus();
            }
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            BindEmployeeDetails();
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string userName = CookiesData["USERID"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            //iob.InTM = DateTime.Now;
            string query = "";
            SqlCommand comm = new SqlCommand(query, conn);


            DateTime openDT = dbFunctions.timezone(DateTime.Now);
            string No;

            if (e.CommandName.Equals("AddNew"))
            {
                string DebitAmt, CreditAmnt;
                string TransTp = "OPEN";
                DateTime OpeningDate = new DateTime();
                OpeningDate = DateTime.Parse(gvDetails.FooterRow.Cells[0].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                No = gvDetails.FooterRow.Cells[1].Text;
                TextBox AccHead = (TextBox)gvDetails.FooterRow.FindControl("txtDebitCD");
                TextBox txtDebitCDCode = (TextBox)gvDetails.FooterRow.FindControl("txtDebitCDCode");
                TextBox DbAmt = (TextBox)gvDetails.FooterRow.FindControl("txtDbAmt");
                if (DbAmt.Text == "")
                {
                    DebitAmt = "0";
                }
                else
                {
                    DebitAmt = DbAmt.Text;
                }
                decimal dbamt = Convert.ToDecimal(DebitAmt);
                TextBox CrAmt = (TextBox)gvDetails.FooterRow.FindControl("txtCrAmt");
                if (CrAmt.Text == "")
                {
                    CreditAmnt = "0";
                }
                else
                {
                    CreditAmnt = CrAmt.Text;
                }
                decimal cdamt = Convert.ToDecimal(CreditAmnt);
                if (dbamt > 0 && cdamt > 0)
                {
                    lblMsg.Visible = true; lblMsg.Text = "Fill Only one, either Debit Amount or Credit Amount";
                    // dbFunctions.showMessage(Page,"Fill Only one, either Debit Amount or Credit Amount");
                }
                else if (AccHead.Text == "")
                {
                    lblMsg.Visible = true; lblMsg.Text = "Account Head Empty";
                    // dbFunctions.showMessage(Page,"Account Head Empty");
                    TextBox txtAccHead = (TextBox)gvDetails.FooterRow.FindControl("txtDebitCD");
                    txtAccHead.Focus();
                }
                else
                {
                    lblMsg.Visible = false;
                    query = ("INSERT INTO GL_MASTER (TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                             " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                             " VALUES (@TRANSTP,@TRANSDT,@TRANSMY,@TRANSNO,@SERIALNO,@TRANSDRCR,@TRANSFOR,@COSTPID,@TRANSMODE, " +
                             " @DEBITCD,@CREDITCD,@DEBITAMT,@CREDITAMT,@CHEQUENO,@CHEQUEDT,@REMARKS,@TABLEID,@USERPC,@USERID,@ACTDTI,@IPADDRESS)");

                    comm = new SqlCommand(query, conn);
                    comm.Parameters.AddWithValue("@TRANSTP", TransTp);
                    comm.Parameters.AddWithValue("@TRANSDT", OpeningDate);
                    comm.Parameters.AddWithValue("@TRANSMY", lblMY.Text);
                    comm.Parameters.AddWithValue("@TRANSNO", No);
                    comm.Parameters.AddWithValue("@SERIALNO", 0);
                    comm.Parameters.AddWithValue("@TRANSDRCR", "");
                    comm.Parameters.AddWithValue("@TRANSFOR", "");
                    comm.Parameters.AddWithValue("@COSTPID", "");
                    comm.Parameters.AddWithValue("@TRANSMODE", "");
                    comm.Parameters.AddWithValue("@DEBITCD", txtDebitCDCode.Text);
                    comm.Parameters.AddWithValue("@CREDITCD", "");
                    comm.Parameters.AddWithValue("@DEBITAMT", DebitAmt);
                    comm.Parameters.AddWithValue("@CREDITAMT", CreditAmnt);
                    comm.Parameters.AddWithValue("@CHEQUENO", "");
                    comm.Parameters.AddWithValue("@CHEQUEDT", DateTime.Parse("01/01/1900"));

                    comm.Parameters.AddWithValue("@REMARKS", "");
                    comm.Parameters.AddWithValue("@TABLEID", "");
                    comm.Parameters.AddWithValue("@USERPC", "");
                    comm.Parameters.AddWithValue("@USERID", userName);
                    comm.Parameters.AddWithValue("@ACTDTI", DateTime.Parse("01/01/1900"));
                    comm.Parameters.AddWithValue("@IPADDRESS", "");

                    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                    int result = comm.ExecuteNonQuery();
                    if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                    date_Vouch();
                    BindEmployeeDetails();
                }
            }
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string userName = CookiesData["USERID"].ToString();

            Label lblDocNo = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblVouchNo");
            Label lblOpenDT = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblDate");
            DateTime OpenDT = DateTime.Parse(lblOpenDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string openDate = OpenDT.ToString("yyyy/MM/dd");

            string TRANSTP = "";
            string TRANSDT = "";
            string TRANSMY = "";
            string TRANSNO = "";
            string SERIALNO = "";
            string TRANSDRCR = "";
            string TRANSFOR = "";
            string COSTPID = "";
            string TRANSMODE = "";
            string DEBITCD = "";
            string CREDITCD = "";
            string DEBITAMT = "";
            string CREDITAMT = "";
            string CHEQUENO = "";
            string CHEQUEDT = "";
            string REMARKS = "";
            string TABLEID = "";
            string USERPC = "";
            string USERID = "";
            string ACTDTI = "";
            string INTIME = "";
            string IPADDRESS = "";

            string userpc = CookiesData["PCName"].ToString(); ;
            string IpAddress = CookiesData["IpAddress"].ToString();

            SqlCommand cmdselectdata = new SqlCommand("SELECT * FROM GL_MASTER where TRANSTP = 'OPEN' and TRANSDT = '" + openDate + "' and TRANSNO = '" + lblDocNo.Text + "'", conn);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlDataReader dr = cmdselectdata.ExecuteReader();
            while (dr.Read())
            {
                TRANSTP = dr["TRANSTP"].ToString();
                TRANSDT = dr["TRANSDT"].ToString();
                TRANSMY = dr["TRANSMY"].ToString();
                TRANSNO = dr["TRANSNO"].ToString();
                SERIALNO = dr["SERIALNO"].ToString();
                TRANSDRCR = dr["TRANSDRCR"].ToString();
                TRANSFOR = dr["TRANSFOR"].ToString();
                COSTPID = dr["COSTPID"].ToString();
                TRANSMODE = dr["TRANSMODE"].ToString();
                DEBITCD = dr["DEBITCD"].ToString();
                CREDITCD = dr["CREDITCD"].ToString();
                DEBITAMT = dr["DEBITAMT"].ToString();
                CREDITAMT = dr["CREDITAMT"].ToString();
                CHEQUENO = dr["CHEQUENO"].ToString();
                CHEQUEDT = dr["CHEQUEDT"].ToString();
                REMARKS = dr["REMARKS"].ToString();
                TABLEID = dr["TABLEID"].ToString();
                USERPC = dr["USERPC"].ToString();
                USERID = dr["USERID"].ToString();
                ACTDTI = dr["ACTDTI"].ToString();
                INTIME = dr["INTIME"].ToString();
                IPADDRESS = dr["IPADDRESS"].ToString();
            }
            dr.Close();

            string alldatabackup = TRANSTP + ", " + TRANSDT + ", " + TRANSMY + ", " + TRANSNO + ", " + SERIALNO + ", " + TRANSDRCR + ", " + TRANSFOR
                + ", " + COSTPID + ", " + TRANSMODE + ", " + DEBITCD + ", " + CREDITCD + ", " + DEBITAMT + ", " + CREDITAMT + ", " + CHEQUENO
                + ", " + CHEQUEDT + ", " + REMARKS + ", " + TABLEID + ", " + USERPC + ", " + USERID + ", " + ACTDTI + ", " + INTIME + ", " + IPADDRESS;

            iob.InTM = dbFunctions.timezone(DateTime.Now);

            string lotileng = iob.LotiLengTudeUpdate;
            string logid = "DELETE";
            string tableid = "GL_MASTER";
            dbFunctions.insertLogData(lotileng, logid, tableid, alldatabackup);

            //if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("delete from GL_MASTER where TRANSTP = 'OPEN' and TRANSDT = '" + openDate + "' and TRANSNO = '" + lblDocNo.Text + "'", conn);
            cmd.ExecuteNonQuery();
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            gvDetails.EditIndex = -1;
            date_Vouch();
            BindEmployeeDetails();
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            BindEmployeeDetails();
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string userName = CookiesData["USERID"].ToString();

            Label lblDocNo = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblVouchNo");
            Label lblOpenDT = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblDate");
            DateTime OpenDT = DateTime.Parse(lblOpenDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string openDate = OpenDT.ToString("yyyy/MM/dd");
            TextBox txtAccNM = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtDebitCDEdit");
            TextBox txtAccNMCode = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtDebitCDCodeEdit");
            TextBox txtDebitAmnt = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtDbAmtEdit");
            if (txtDebitAmnt.Text == "")
            {
                txtDebitAmnt.Text = "0";
            }
            else
                txtDebitAmnt.Text = txtDebitAmnt.Text;
            TextBox txtCreditAmt = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtCrAmtEdit");
            if (txtCreditAmt.Text == "")
            {
                txtCreditAmt.Text = "0";
            }
            else
                txtCreditAmt.Text = txtCreditAmt.Text;
            decimal dbAmt;
            if (txtDebitAmnt.Text == "")
            {
                dbAmt = 0;
            }
            else
                dbAmt = Convert.ToDecimal(txtDebitAmnt.Text);

            decimal cdAmt;
            if (txtCreditAmt.Text == "")
            {
                cdAmt = 0;
            }
            else
                cdAmt = Convert.ToDecimal(txtCreditAmt.Text);
            if (dbAmt > 0 && cdAmt > 0)
            {
                lblMsg.Visible = true; lblMsg.Text = "Fill Either Debit Amount or Credit.";
            }
            else
            {
                lblMsg.Visible = false;

                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                SqlCommand cmd = new SqlCommand("update GL_MASTER set DEBITCD='" + txtAccNMCode.Text + "', DEBITAMT = '" + txtDebitAmnt.Text + "' , CREDITAMT= '" + txtCreditAmt.Text + "'  where TRANSTP = 'OPEN' and TRANSDT = '" + openDate + "' and TRANSNO = '" + lblDocNo.Text + "'", conn);
                cmd.ExecuteNonQuery();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                gvDetails.EditIndex = -1;
                BindEmployeeDetails();
            }
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = txtDate.Text;
                e.Row.Cells[1].Text = lblTotCount.Text;
            }
        }

        //protected void txtDebitCDEdit_TextChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        //    //NamingContainer return the container that the control sits in
        //    TextBox other = (TextBox)row.FindControl("txtDebitCDEdit");
        //    lbltxtChg.Text = other.Text;

        //    dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM like '" + lbltxtChg.Text + "%'", lbltxtShw);
        //}
    }
}