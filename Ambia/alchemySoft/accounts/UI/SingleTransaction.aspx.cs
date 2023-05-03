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
using System.Globalization;
using alchemy.accounts.Interface;
using alchemy.accounts.DataAccess;
using alchemySoft;

namespace alchemy.accounts.UI
{
    public partial class SingleTransaction : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        AccountDataAccess dob = new AccountDataAccess();
        AccountInterface iob = new AccountInterface();
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        protected void Page_Load(object sender, EventArgs e)
        {

            if (CookiesData == null)
            {
                Response.Redirect("~/Login/UI/Login.aspx");
            }
            else
            {
                string formLink = "/Accounts/UI/SingleTransaction.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        Label1.Visible = true;
                        Label2.Visible = true;
                        txtDebitNm.Visible = false;
                        txtCreditNm.Visible = false;
                        txtJRDebitNm.Visible = false;
                        txtJRCreditNm.Visible = false;
                        txtCNDebitNm.Visible = false;
                        txtCNCreditNm.Visible = false;
                        txtCheque.Enabled = false;
                        txtChequeDate.Enabled = false;
                        lblVCount.Visible = false;
                        txtCostPool.Enabled = false;
                        ddlTransType.AutoPostBack = true;
                        txtCostPool.AutoPostBack = true;
                        ddlTransFor.AutoPostBack = true;
                        ddlTransMode.AutoPostBack = true;
                        txtChequeDate.AutoPostBack = true;

                        if (ddlTransType.Text == "MPAY")
                        {
                            DateTime today = DateTime.Today.Date;
                            string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                            txtTransDate.Text = td;

                            string mon = DateTime.Today.Date.ToString("MMM").ToUpper();
                            string year = today.ToString("yy");
                            txtTransYear.Text = mon + "-" + year;
                            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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
                            Label1.Text = "Payment To";
                            Label2.Text = "Payment From";
                            // txtMPDebitNM.Focus();
                            ddlTransFor.Focus();
                        }
                    }
                }
                else
                {
                    Response.Redirect("../../DeshBoard/UI/Default.aspx");
                }
            }
        }
        protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransType.Text == "MREC")
            {
                txtTransDate.Text = "";
                txtDebitNm.Text = "";
                txtCreditNm.Text = "";
                txtDebited.Text = "";
                txtCredited.Text = "";
                txtMPDebitNM.Visible = false;
                txtMpCreditNm.Visible = false;
                txtJRDebitNm.Visible = false;
                txtJRCreditNm.Visible = false;
                txtCNDebitNm.Visible = false;
                txtCNCreditNm.Visible = false;
                txtDebitNm.Visible = true;
                txtCreditNm.Visible = true;
                Label1.Visible = true;
                Label2.Visible = true;
                Label1.Text = "Received To";
                Label2.Text = "Received From";

                DateTime today = DateTime.Today.Date;
                string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                txtTransDate.Text = td;

                string mon = DateTime.Today.Date.ToString("MMM").ToUpper();
                string year = today.ToString("yy");
                txtTransYear.Text = mon + "-" + year;
                dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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

                GetCompletionListMrecD(prefixText, count, contextKey);
                GetCompletionListMrecC(prefixText, count, contextKey);
                ddlTransType.Focus();

            }
            else if (ddlTransType.Text == "MPAY")
            {
                txtTransDate.Text = "";
                txtMPDebitNM.Text = "";
                txtMpCreditNm.Text = "";
                txtDebited.Text = "";
                txtCredited.Text = "";
                txtDebitNm.Visible = false;
                txtCreditNm.Visible = false;
                txtJRDebitNm.Visible = false;
                txtJRCreditNm.Visible = false;
                txtCNDebitNm.Visible = false;
                txtCNCreditNm.Visible = false;
                txtMPDebitNM.Visible = true;
                txtMpCreditNm.Visible = true;
                Label1.Visible = true;
                Label2.Visible = true;
                Label1.Text = "Payment To";
                Label2.Text = "Payment From";

                DateTime today = DateTime.Today.Date;
                string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                txtTransDate.Text = td;

                string mon = DateTime.Today.Date.ToString("MMM").ToUpper();
                string year = today.ToString("yy");
                txtTransYear.Text = mon + "-" + year;
                dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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

                GetCompletionListMpayD(prefixText, count, contextKey);
                GetCompletionListMpayC(prefixText, count, contextKey);
                ddlTransType.Focus();
            }
            else if (ddlTransType.Text == "JOUR")
            {
                txtTransDate.Text = "";
                txtJRDebitNm.Text = "";
                txtJRCreditNm.Text = "";
                txtDebited.Text = "";
                txtCredited.Text = "";
                txtDebitNm.Visible = false;
                txtCreditNm.Visible = false;
                txtMPDebitNM.Visible = false;
                txtMpCreditNm.Visible = false;
                txtCNDebitNm.Visible = false;
                txtCNCreditNm.Visible = false;
                txtJRDebitNm.Visible = true;
                txtJRCreditNm.Visible = true;
                Label1.Visible = true;
                Label2.Visible = true;
                Label1.Text = "Debited To";
                Label2.Text = "Credited To";

                DateTime today = DateTime.Today.Date;
                string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                txtTransDate.Text = td;

                string mon = DateTime.Today.Date.ToString("MMM").ToUpper();
                string year = today.ToString("yy");
                txtTransYear.Text = mon + "-" + year;
                dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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

                GetCompletionListJourD(prefixText, count, contextKey);
                GetCompletionListJourC(prefixText, count, contextKey);
                ddlTransType.Focus();
            }
            else if (ddlTransType.Text == "CONT")
            {
                txtTransDate.Text = "";
                txtCNDebitNm.Text = "";
                txtCNCreditNm.Text = "";
                txtDebited.Text = "";
                txtCredited.Text = "";
                txtDebitNm.Visible = false;
                txtCreditNm.Visible = false;
                txtMPDebitNM.Visible = false;
                txtMpCreditNm.Visible = false;
                txtJRDebitNm.Visible = false;
                txtJRCreditNm.Visible = false;
                txtCNCreditNm.Visible = true;
                txtCNDebitNm.Visible = true;
                Label1.Visible = true;
                Label2.Visible = true;
                Label1.Text = "Debited To";
                Label2.Text = "Credited To";

                DateTime today = DateTime.Today.Date;
                string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                txtTransDate.Text = td;

                string mon = DateTime.Today.Date.ToString("MMM").ToUpper();
                string year = today.ToString("yy");
                txtTransYear.Text = mon + "-" + year;
                dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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

                GetCompletionListConD(prefixText, count, contextKey);
                GetCompletionListConC(prefixText, count, contextKey);
                ddlTransType.Focus();
            }
            else
            {
                Label1.Visible = false;
                Label2.Visible = false;
            }
        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>

        ///Recept Start
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListMrecD(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListMrecC(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }
        ///Recept End

        ///Payment Start
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListMpayD(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' ", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListMpayC(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE  substring(ACCOUNTCD,1,7)  in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }
        ///Payment End

        ///Journal Start
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListJourD(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' ", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListJourC(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) not in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' ", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }
        ///Journal End

        ///Contra Start
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListConD(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103')  and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%'", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListConC(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,7) in ('1020101','1020102','2020103') and STATUSCD = 'P' and ACCOUNTNM LIKE '" + prefixText + "%' ", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
            return CompletionSet.ToArray();
        }
        ///Contra End


        protected void ddlTransMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransMode.Text == "CASH CHEQUE")
            {
                txtCheque.Enabled = true;
                txtChequeDate.Enabled = true;
            }
            else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {
                txtCheque.Enabled = true;
                txtChequeDate.Enabled = true;
            }
            else
            {
                txtCheque.Enabled = false;
                txtChequeDate.Enabled = false;
            }
            if (ddlTransType.Text == "MREC")
            {
                ddlTransMode.AutoPostBack = false;
                txtDebitNm.Focus();
            }
            else if (ddlTransType.Text == "MPAY")
            {
                ddlTransMode.AutoPostBack = false;
                Page.SetFocus(txtMPDebitNM);
            }
            else if (ddlTransType.Text == "JOUR")
            {
                txtJRDebitNm.Focus();
            }
            else if (ddlTransType.Text == "CONT")
            {
                txtCNDebitNm.Focus();
            }
            else
            {
                txtMPDebitNM.Focus();
            }
        }

        protected void txtTransDate_TextChanged(object sender, EventArgs e)
        {
            DateTime transdate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string month = transdate.ToString("MMM").ToUpper();
            string years = transdate.ToString("yy");

            txtTransYear.Text = month + "-" + years;
            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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
            //txtTransDate.AutoPostBack = false;
            txtTransDate.Focus();
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            txtInwords.Text = "";
            decimal dec;
            Boolean ValidInput = Decimal.TryParse(txtAmount.Text, out dec);
            if (!ValidInput)
            {
                txtInwords.ForeColor = System.Drawing.Color.Red;
                txtInwords.Text = "Enter the Proper Amount...";
                return;
            }
            if (txtAmount.Text.ToString().Trim() == "")
            {
                txtInwords.ForeColor = System.Drawing.Color.Red;
                txtInwords.Text = "Amount Cannot Be Empty...";
                return;
            }
            else
            {
                if (Convert.ToDecimal(txtAmount.Text) == 0)
                {
                    txtInwords.ForeColor = System.Drawing.Color.Red;
                    txtInwords.Text = "Amount Cannot Be Empty...";
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

            string AmtConv =dbFunctions.SpellAmount.MoneyConvFn(txtAmount.Text.ToString().Trim());
            //string amntComma = SpellAmount.comma(Convert.ToDecimal(txtAmount.Text));
            //Label3.Text = amntComma;

            txtInwords.Text = AmtConv.Trim();
            txtInwords.ForeColor = System.Drawing.Color.Green;
            txtInwords.Focus();
        }

        protected void ddlTransFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransFor.Text == "OTHERS")
            {
                //ddlCostPID.AutoPostBack = false;
                txtCostPool.Focus();
                txtCostPool.Enabled = true;
            }
            else
            {
                ddlTransMode.Focus();
                txtCostPool.Enabled = false;
                lblCostpoolID.Text = "";
            }
        }




        protected void btnSave_Click(object sender, EventArgs e)
        {


            if (txtDebited.Text == "")
            {
                Response.Write("<script>alert('Please Select Account Head.');</script>");
                if (ddlTransType.Text == "MREC")
                {
                    txtDebited.Focus();
                }
                else if (ddlTransType.Text == "MPAY")
                {
                    txtMPDebitNM.Focus();
                }
                else if (ddlTransType.Text == "JOUR")
                {
                    txtJRDebitNm.Focus();
                }
                else if (ddlTransType.Text == "CONT")
                {
                    txtCNDebitNm.Focus();
                }
            }
            else if (txtCredited.Text == "")
            {
                Response.Write("<script>alert('Please Select Account Head.');</script>");
                if (ddlTransType.Text == "MREC")
                {
                    txtCreditNm.Focus();
                }
                else if (ddlTransType.Text == "MPAY")
                {
                    txtMpCreditNm.Focus();
                }
                else if (ddlTransType.Text == "JOUR")
                {
                    txtJRCreditNm.Focus();
                }
                else if (ddlTransType.Text == "CONT")
                {
                    txtCNCreditNm.Focus();
                }
            }
            else if (txtAmount.Text == "")
            {
                Response.Write("<script>alert('Please Fill the Amount');</script>");
            }
            else if (ddlTransMode.Text == "CASH CHEQUE")
            {
                if (txtCheque.Text == "")
                {
                    lblChequeDT.Visible = false;
                    string msg = "Fill Cheque No.";
                    lblCheque.Visible = true;
                    lblCheque.Text = msg;
                    txtCheque.Focus();
                }
                else if (txtChequeDate.Text == "")
                {
                    lblCheque.Visible = false;
                    string msg = "Select Cheque Date.";
                    lblChequeDT.Visible = true;
                    lblChequeDT.Text = msg;
                    txtChequeDate.Focus();
                }
                else
                {
                    Save();
                }

            }
            else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {

                if (txtCheque.Text == "")
                {
                    lblChequeDT.Visible = false;
                    string msg = "Fill Cheque No.";
                    lblCheque.Visible = true;
                    lblCheque.Text = msg;
                    txtCheque.Focus();
                }
                else if (txtChequeDate.Text == "")
                {
                    lblCheque.Visible = false;
                    string msg = "Select Cheque Date.";
                    lblChequeDT.Visible = true;
                    lblChequeDT.Text = msg;
                    txtChequeDate.Focus();
                }
                else
                {
                    Save();
                }
            }
            else
            {
                Save();
            }

        }

        public void Save()
        { 
            AccountDataAccess dob = new AccountDataAccess();
            AccountInterface iob = new AccountInterface();

            try
            {
                Session["transtp"] = ddlTransType.Text;
                Session["transdt"] = txtTransDate.Text;
                iob.Transtp = ddlTransType.Text;
                iob.Transdt = (DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
                iob.Monyear = txtTransYear.Text;
                iob.Voucher = int.Parse(txtVouchNo.Text);
                iob.Transfor = ddlTransFor.Text;
                iob.Costpid = lblCostpoolID.Text;
                iob.Transmode = ddlTransMode.Text;
                iob.Debitcd = txtDebited.Text;
                iob.Creditcd = txtCredited.Text;


                iob.Chequeno = txtCheque.Text;
                if (txtChequeDate.Text == "")
                {
                    iob.Chequedt = (DateTime.Parse("01/01/1900", dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
                }
                else
                {
                    iob.Chequedt = (DateTime.Parse(txtChequeDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
                }
                iob.Remarks = txtRemarks.Text;
                iob.Amount = Convert.ToDecimal(txtAmount.Text);
                iob.Inword = txtInwords.Text;
                iob.UserId = Convert.ToInt64(CookiesData["USERID"].ToString());
                dob.insertSingleVouch(iob);

                // Response.Write("<script>alert('Data has been Saved');</script>");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            ddlTransType.Text = Session["transtp"].ToString();
            txtTransDate.Text = Session["transdt"].ToString();
            txtAmount.Text = "";
            txtCheque.Text = "";
            txtChequeDate.Text = "";
            txtCredited.Text = "";
            txtDebited.Text = "";
            txtInwords.Text = "";
            txtRemarks.Text = "";
            txtTransYear.Text = "";
            txtVouchNo.Text = "";
            txtCNCreditNm.Text = "";
            txtCNDebitNm.Text = "";
            txtDebitNm.Text = "";
            txtJRCreditNm.Text = "";
            txtJRDebitNm.Text = "";
            txtCreditNm.Text = "";
            txtMpCreditNm.Text = "";
            txtMPDebitNM.Text = "";
            ddlTransFor.SelectedIndex = -1;
            txtCostPool.Text = "";

            DateTime TrnsDy = (DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal)); ;

            string mon = TrnsDy.Date.ToString("MMM").ToUpper();
            string year = TrnsDy.ToString("yy");
            txtTransYear.Text = mon + "-" + year;
            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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

            if (ddlTransType.Text == "MREC")
            {
                txtDebitNm.Focus();
            }
            else if (ddlTransType.Text == "MPAY")
            {
                txtMPDebitNM.Focus();
            }
            else if (ddlTransType.Text == "JOUR")
            {
                txtJRDebitNm.Focus();
            }
            else if (ddlTransType.Text == "CONT")
            {
                txtCNDebitNm.Focus();
            }
            lblCheque.Visible = false;
            lblChequeDT.Visible = false;
        }

        public void refresh()
        {
            ddlTransType.SelectedIndex = -1;
            //ddlDebitNM.SelectedIndex = -1;
            //ddlCreditNM.SelectedIndex = -1;
            ddlTransFor.SelectedIndex = -1;
            ddlTransMode.SelectedIndex = -1;
            //ddlCostPID.SelectedIndex = -1;
            txtCostPool.Text = "";
            // txtTransDate.Text = "";
            txtAmount.Text = "";
            txtCheque.Text = "";
            txtChequeDate.Text = "";
            txtCredited.Text = "";
            txtDebited.Text = "";
            txtInwords.Text = "";
            txtRemarks.Text = "";
            txtTransYear.Text = "";
            txtVouchNo.Text = "";
            txtCNCreditNm.Text = "";
            txtCNDebitNm.Text = "";
            txtDebitNm.Text = "";
            txtJRCreditNm.Text = "";
            txtJRDebitNm.Text = "";
            txtCreditNm.Text = "";
            txtMpCreditNm.Text = "";
            txtMPDebitNM.Text = "";

            DateTime today = DateTime.Today.Date;
            string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
            txtTransDate.Text = td;

            string mon = DateTime.Today.Date.ToString("MMM").ToUpper();
            string year = today.ToString("yy");
            txtTransYear.Text = mon + "-" + year;
            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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
        }

        public string prefixText { get; set; }

        public int count { get; set; }

        public string contextKey { get; set; }

        protected void txtCNDebitNm_TextChanged(object sender, EventArgs e)
        {
            if (txtCNDebitNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtCNDebitNm.Text + "'", txtDebited);
            }
            else
            {
                txtDebited.Text = "";
            }
            txtCNCreditNm.Focus();
        }

        protected void txtJRDebitNm_TextChanged(object sender, EventArgs e)
        {
            if (txtJRDebitNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtJRDebitNm.Text + "'", txtDebited);
            }
            else
            {
                txtDebited.Text = "";
            }
            txtJRCreditNm.Focus();
        }

        protected void txtMPDebitNM_TextChanged(object sender, EventArgs e)
        {
            if (txtMPDebitNM.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtMPDebitNM.Text + "'", txtDebited);
            }
            else
            {
                txtDebited.Text = "";
            }
            txtMpCreditNm.Focus();
        }

        protected void txtDebitNm_TextChanged(object sender, EventArgs e)
        {
            if (txtDebitNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtDebitNm.Text + "'", txtDebited);
            }
            else
            {
                txtDebited.Text = "";
            }
            txtCreditNm.Focus();
        }

        protected void txtCNCreditNm_TextChanged(object sender, EventArgs e)
        {
            if (txtCNCreditNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtCNCreditNm.Text + "'", txtCredited);
            }
            else
            {
                txtDebited.Text = "";
            }
            if (ddlTransMode.Text == "CASH CHEQUE" || ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {
                txtCheque.Focus();
            }
            else
            {
                txtRemarks.Focus();
            }
        }

        protected void txtJRCreditNm_TextChanged(object sender, EventArgs e)
        {
            if (txtJRCreditNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtJRCreditNm.Text + "'", txtCredited);
            }
            else
            {
                txtDebited.Text = "";
            }
            if (ddlTransMode.Text == "CASH CHEQUE" || ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {
                txtCheque.Focus();
            }
            else
            {
                txtRemarks.Focus();
            }
        }

        protected void txtMpCreditNm_TextChanged(object sender, EventArgs e)
        {
            if (txtMpCreditNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtMpCreditNm.Text + "'", txtCredited);
            }
            else
            {
                txtDebited.Text = "";
            }
            if (ddlTransMode.Text == "CASH CHEQUE" || ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {
                txtCheque.Focus();
            }
            else
            {
                txtRemarks.Focus();
            }
        }

        protected void txtCreditNm_TextChanged(object sender, EventArgs e)
        {
            if (txtCreditNm.Text != "")
            {
                dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD='P' and ACCOUNTNM = '" + txtCreditNm.Text + "'", txtCredited);
            }
            else
            {
                txtDebited.Text = "";
            }
            if (ddlTransMode.Text == "CASH CHEQUE" || ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {
                txtCheque.Focus();
            }
            else
            {
                txtRemarks.Focus();
            }
        }

        public void Save_Print()
        {
            Session["TransType"] = "";
            Session["TransDate"] = "";
            Session["VouchNo"] = "";
            Session["TransMode"] = "";
            Session["DebitCD"] = "";
            Session["CreditCD"] = "";
            Session["ChequeNo"] = "";
            Session["ChequeDT"] = "";
            Session["Remarks"] = "";
            Session["Amount"] = "";
            Session["Inword"] = "";
            string userName = CookiesData["USERNAME"].ToString();
            AccountDataAccess dob = new AccountDataAccess();
            AccountInterface iob = new AccountInterface();

            try
            {
                iob.Transtp = ddlTransType.Text;
                Session["TransType"] = ddlTransType.Text;
                iob.Transdt = (DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
                Session["TransDate"] = txtTransDate.Text;
                iob.Monyear = txtTransYear.Text;
                iob.Voucher = int.Parse(txtVouchNo.Text);
                Session["VouchNo"] = txtVouchNo.Text;
                iob.Transfor = ddlTransFor.Text;
                iob.Costpid = lblCostpoolID.Text;
                iob.Transmode = ddlTransMode.Text;
                Session["TransMode"] = ddlTransMode.Text;
                iob.Debitcd = txtDebited.Text;
                Session["DebitCD"] = txtDebited.Text;
                iob.Creditcd = txtCredited.Text;
                Session["CreditCD"] = txtCredited.Text;
                iob.Chequeno = txtCheque.Text;
                Session["ChequeNo"] = txtCheque.Text;
                if (txtChequeDate.Text == "")
                {
                    iob.Chequedt = DateTime.Parse("01/01/1900");
                }
                else
                {
                    iob.Chequedt = (DateTime.Parse(txtChequeDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
                }
                Session["ChequeDT"] = txtChequeDate.Text;
                iob.Remarks = txtRemarks.Text;
                Session["Remarks"] = txtRemarks.Text;
                iob.Amount = Convert.ToDecimal(txtAmount.Text);
                Session["Amount"] = txtAmount.Text;
                iob.Inword = txtInwords.Text;
                Session["Inword"] = txtInwords.Text;
                iob.Username = userName;
                iob.UserId = Convert.ToInt64(CookiesData["USERID"].ToString());
                dob.insertSingleVouch(iob);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            ddlTransType.Text = Session["TransType"].ToString();
            txtTransDate.Text = Session["TransDate"].ToString();
            txtAmount.Text = "";
            txtCheque.Text = "";
            txtChequeDate.Text = "";
            txtCredited.Text = "";
            txtDebited.Text = "";
            txtInwords.Text = "";
            txtRemarks.Text = "";
            txtTransYear.Text = "";
            txtVouchNo.Text = "";
            txtCNCreditNm.Text = "";
            txtCNDebitNm.Text = "";
            txtDebitNm.Text = "";
            txtJRCreditNm.Text = "";
            txtJRDebitNm.Text = "";
            txtCreditNm.Text = "";
            txtMpCreditNm.Text = "";
            txtMPDebitNM.Text = "";
            ddlTransFor.SelectedIndex = -1;
            txtCostPool.Text = "";

            DateTime TrnsDy = (DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal)); ;

            string mon = TrnsDy.Date.ToString("MMM").ToUpper();
            string year = TrnsDy.ToString("yy");
            txtTransYear.Text = mon + "-" + year;
            dbFunctions.lblAdd(@"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text + "' and TRANSTP = '" + ddlTransType.Text + "'", lblVCount);
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

            if (ddlTransType.Text == "MREC")
            {
                txtDebitNm.Focus();
            }
            else if (ddlTransType.Text == "MPAY")
            {
                txtMPDebitNM.Focus();
            }
            else if (ddlTransType.Text == "JOUR")
            {
                txtJRDebitNm.Focus();
            }
            else if (ddlTransType.Text == "CONT")
            {
                txtCNDebitNm.Focus();
            }
            lblCheque.Visible = false;
            lblChequeDT.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtDebited.Text == "")
            {
                Response.Write("<script>alert('Please Select Account Head.');</script>");
                if (ddlTransType.Text == "MREC")
                {
                    txtDebited.Focus();
                }
                else if (ddlTransType.Text == "MPAY")
                {
                    txtMPDebitNM.Focus();
                }
                else if (ddlTransType.Text == "JOUR")
                {
                    txtJRDebitNm.Focus();
                }
                else if (ddlTransType.Text == "CONT")
                {
                    txtCNDebitNm.Focus();
                }
            }
            else if (txtCredited.Text == "")
            {
                Response.Write("<script>alert('Please Select Account Head.');</script>");
                if (ddlTransType.Text == "MREC")
                {
                    txtCreditNm.Focus();
                }
                else if (ddlTransType.Text == "MPAY")
                {
                    txtMpCreditNm.Focus();
                }
                else if (ddlTransType.Text == "JOUR")
                {
                    txtJRCreditNm.Focus();
                }
                else if (ddlTransType.Text == "CONT")
                {
                    txtCNCreditNm.Focus();
                }
            }
            else if (txtAmount.Text == "")
            {
                Response.Write("<script>alert('Please Fill the Amount');</script>");
            }
            else if (ddlTransMode.Text == "CASH CHEQUE")
            {
                if (txtCheque.Text == "")
                {
                    lblChequeDT.Visible = false;
                    string msg = "Fill Cheque No.";
                    lblCheque.Visible = true;
                    lblCheque.Text = msg;
                    txtCheque.Focus();
                }
                else if (txtChequeDate.Text == "")
                {
                    lblCheque.Visible = false;
                    string msg = "Select Cheque Date.";
                    lblChequeDT.Visible = true;
                    lblChequeDT.Text = msg;
                    txtChequeDate.Focus();
                }
                else
                {
                    Save_Print();
                    Page.ClientScript.RegisterStartupScript(
                    this.GetType(), "OpenWindow", "window.open('../Report/Report/RptCreditVoucher.aspx','_newtab');", true);
                }

            }
            else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {

                if (txtCheque.Text == "")
                {
                    lblChequeDT.Visible = false;
                    string msg = "Fill Cheque No.";
                    lblCheque.Visible = true;
                    lblCheque.Text = msg;
                    txtCheque.Focus();
                }
                else if (txtChequeDate.Text == "")
                {
                    lblCheque.Visible = false;
                    string msg = "Select Cheque Date.";
                    lblChequeDT.Visible = true;
                    lblChequeDT.Text = msg;
                    txtChequeDate.Focus();
                }
                else
                {
                    Save_Print();
                    Page.ClientScript.RegisterStartupScript(
                    this.GetType(), "OpenWindow", "window.open('../Report/Report/RptCreditVoucher.aspx','_newtab');", true);
                }
            }
            else
            {
                Save_Print();
                Page.ClientScript.RegisterStartupScript(
                    this.GetType(), "OpenWindow", "window.open('../Report/Report/RptCreditVoucher.aspx','_newtab');", true);
            }


        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        protected void txtCheque_TextChanged(object sender, EventArgs e)
        {
            //txtChequeDate.Focus();
        }

        protected void txtChequeDate_TextChanged(object sender, EventArgs e)
        {
            //txtChequeDate.AutoPostBack = false;
            txtChequeDate.Focus();
        }

        protected void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            //txtAmount.Focus();
        }

        protected void txtInwords_TextChanged(object sender, EventArgs e)
        {
            if (txtInwords.Text == "")
                Response.Write("<script>alert('Please Fill the Amount');</script>");
            //ddlS.Focus();
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionListCostPool(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            // Try to use parameterized inline query/sp to protect sql injection
            SqlCommand cmd = new SqlCommand("SELECT COSTPNM FROM GL_COSTP WHERE COSTPNM LIKE '" + prefixText + "%' ", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["COSTPNM"].ToString());
            return CompletionSet.ToArray();
        }

        protected void txtCostPool_TextChanged(object sender, EventArgs e)
        {
            dbFunctions.lblAdd(@"Select COSTPID FROM GL_COSTP where COSTPNM ='" + txtCostPool.Text + "'", lblCostpoolID);
            ddlTransMode.Focus();
        }

    }
}