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
                Response.Redirect("~/login/ui/SignIn");
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
                        txtCheque.Enabled = false;
                        txtChequeDate.Enabled = false;
                        lblVCount.Visible = false;
                        ddlTransType.AutoPostBack = true;
                        ddlTransMode.AutoPostBack = true;
                        txtChequeDate.AutoPostBack = true;

                        if (ddlTransType.Text == "MPAY")
                        {
                            DateTime today = dbFunctions.timezone(DateTime.Now);
                            txtTransDate.Text = today.ToString("dd/MM/yyyy");

                            string mon = dbFunctions.timezone(DateTime.Today.Date).ToString("MMM").ToUpper();
                            string year = today.ToString("yy");
                            txtTransYear.Text = mon + "-" + year;
                            dbFunctions.lblAdd(
                                @"Select max(TRANSNO) FROM GL_STRANS where TRANSMY='" + txtTransYear.Text +
                                "' and TRANSTP = '" +
                                ddlTransType.Text + "'", lblVCount);
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
                            //txtFiscalYear.Text = DbFunctions.GetFinancialYear(today);
                        }
                        dbFunctions.dropDown_Bind(ddlCostPool,"id","select", @"SELECT COSTPNM nm, COSTPID id FROM GL_COSTP ");
                        ddlTransType.Focus();
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
                if (ddlTransType.Text == "MREC")
                {
                    txtTransDate.Text = "";
                    txtDebited.Text = "";
                    txtCredited.Text = "";
                    Label1.Visible = true;
                    Label2.Visible = true;
                    Label1.Text = "Received To";
                    Label2.Text = "Received From";

                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
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

                    //GetCompletionListMrecD(prefixText, count, contextKey);
                    //GetCompletionListMrecC(prefixText, count, contextKey);
                    ddlTransType.Focus();

                }
                else if (ddlTransType.Text == "MPAY")
                {
                    txtTransDate.Text = "";
                    txtDebited.Text = "";
                    txtCredited.Text = "";
                    Label1.Visible = true;
                    Label2.Visible = true;
                    Label1.Text = "Payment To";
                    Label2.Text = "Payment From";

                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
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

                    //GetCompletionListMpayD(prefixText, count, contextKey);
                    //GetCompletionListMpayC(prefixText, count, contextKey);
                    ddlTransType.Focus();
                }
                else if (ddlTransType.Text == "JOUR")
                {
                    txtTransDate.Text = "";
                    txtDebited.Text = "";
                    txtCredited.Text = "";
                    Label1.Visible = true;
                    Label2.Visible = true;
                    Label1.Text = "Debited To";
                    Label2.Text = "Credited To";

                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
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

                    //GetCompletionListJourD(prefixText, count, contextKey);
                    //GetCompletionListJourC(prefixText, count, contextKey);
                    ddlTransType.Focus();
                }
                else if (ddlTransType.Text == "CONT")
                {
                    txtTransDate.Text = "";
                    txtCNDebitNm.Text = "";
                    txtCNCreditNm.Text = "";
                    txtDebited.Text = "";
                    txtCredited.Text = "";
                    Label1.Visible = true;
                    Label2.Visible = true;
                    Label1.Text = "Debited To";
                    Label2.Text = "Credited To";

                    txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                    DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
                    string mon = today.ToString("MMM").ToUpper();
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

                    //GetCompletionListConD(prefixText, count, contextKey);
                    //GetCompletionListConC(prefixText, count, contextKey);
                    ddlTransType.Focus();
                }
                else
                {
                    Label1.Visible = false;
                    Label2.Visible = false;


                }
            }
        }

        protected void ddlTransMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                if (ddlTransMode.Text == "CASH CHEQUE")
                {
                    txtCheque.Enabled = true;
                    txtChequeDate.Enabled = true;

                    lblChequeDate.Visible = true;
                    lblChequeNo.Visible = true;
                    txtCheque.Visible = true;
                    txtChequeDate.Visible = true;
                }
                else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
                {
                    txtCheque.Enabled = true;
                    txtChequeDate.Enabled = true;
                    lblChequeDate.Visible = true;
                    lblChequeNo.Visible = true;
                    txtCheque.Visible = true;
                    txtChequeDate.Visible = true;
                }
                else
                {
                    txtCheque.Enabled = false;
                    txtChequeDate.Enabled = false;
                    lblChequeDate.Visible = false;
                    lblChequeNo.Visible = false;
                    txtCheque.Visible = false;
                    txtChequeDate.Visible = false;
                }

                txtCNDebitNm.Focus();
            }
        }
        protected void txtTransDate_TextChanged(object sender, EventArgs e)
        {
            DateTime transdate = DateTime.Parse(txtTransDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string month = transdate.ToString("MMM").ToUpper();
            string years = transdate.ToString("yy");
            //txtFiscalYear.Text = DbFunctions.GetFinancialYear(transdate);

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
            ddlCostPool.Focus();
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            txtInwords.Text = "";
            decimal dec;
            Boolean validInput = Decimal.TryParse(txtAmount.Text, out dec);
            if (!validInput)
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
                x1 = txtAmount.Text.Trim().Substring(0, txtAmount.Text.Trim().IndexOf("."));
                x2 = txtAmount.Text.Trim().Substring(txtAmount.Text.Trim().IndexOf(".") + 1);
            }
            else
            {
                x1 = txtAmount.Text.ToString().Trim();
                x2 = "00";
            }

            x1 = x1.Trim() != "" ? Convert.ToInt64(x1.Trim()).ToString().Trim() : "0";

            txtAmount.Text = x1 + "." + x2;

            if (x2.Length > 2)
            {
                txtAmount.Text = Math.Round(Convert.ToDouble(txtAmount.Text), 2).ToString(CultureInfo.InvariantCulture).Trim();
            }

            string amtConv =dbFunctions.SpellAmount.MoneyConvFn(txtAmount.Text.Trim());

            txtInwords.Text = amtConv.Trim();
            txtInwords.ForeColor = System.Drawing.Color.Green;
            txtRemarks.Focus();
        }

        protected void ddlTransFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransFor.Text == "OTHERS")
            {
                ddlCostPool.Focus();
                ddlCostPool.Enabled = true;
            }
            else
            {
                ddlTransMode.Focus();
                ddlCostPool.Enabled = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else if (ddlCostPool.Text == "")
            {
                lblErrMSg.Visible = true;
                lblErrMSg.Text = "Select Branch Name.";
                ddlCostPool.Focus();
            }
            else
            {
                txtTransDate_TextChanged(sender, e);
                if (txtDebited.Text == "")
                {
                    Response.Write("<script>alert('Please Select Account Head.');</script>");
                    txtDebited.Focus();

                }
                else if (txtCredited.Text == "")
                {
                    Response.Write("<script>alert('Please Select Account Head.');</script>");
                    txtCNCreditNm.Focus();
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

        }

        public void Save()
        {
            string userName = CookiesData["USERID"].ToString();
            string userIp = CookiesData["IpAddress"].ToString();
            string userPc = CookiesData["PCName"].ToString();

            try
            {

                Session["transtp"] = ddlTransType.Text;
                Session["transdt"] = txtTransDate.Text;
                iob.Transtp = ddlTransType.Text;
                //iob.FiscalYear = txtFiscalYear.Text;
                iob.Transdt = (DateTime.Parse(txtTransDate.Text, dateformat, DateTimeStyles.AssumeLocal));
                iob.Monyear = txtTransYear.Text;
                iob.Voucher = int.Parse(txtVouchNo.Text);
                iob.Transfor = ddlTransFor.Text;
                iob.Costpid = ddlCostPool.SelectedValue;
                iob.Transmode = ddlTransMode.Text;
                iob.Debitcd = txtDebited.Text;
                iob.Creditcd = txtCredited.Text;




                iob.Chequeno = txtCheque.Text;
                iob.Chequedt = txtChequeDate.Text == "" ? (DateTime.Parse("01/01/1900", dateformat, DateTimeStyles.AssumeLocal)) : (DateTime.Parse(txtChequeDate.Text, dateformat, DateTimeStyles.AssumeLocal));
                iob.Remarks = txtRemarks.Text;
                iob.Amount = Convert.ToDecimal(txtAmount.Text);
                iob.Inword = txtInwords.Text;
                iob.UserId = Convert.ToInt64(userName);
                iob.Userpc = userPc;
                iob.Ipaddress = userIp;
                iob.InTM = dbFunctions.timezone(DateTime.Now);
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
            ddlTransFor.SelectedIndex = -1;
            ddlCostPool.SelectedIndex = -1;

            DateTime trnsDy = (DateTime.Parse(txtTransDate.Text, dateformat, DateTimeStyles.AssumeLocal));

            string mon = trnsDy.Date.ToString("MMM").ToUpper();
            string year = trnsDy.ToString("yy");
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
            txtCNDebitNm.Focus();
            ddlCostPool.Focus();
            lblCheque.Visible = false;
            lblChequeDT.Visible = false;
        }



        public void Refresh()
        {
            ddlTransType.SelectedIndex = -1;
            //ddlDebitNM.SelectedIndex = -1;
            //ddlCreditNM.SelectedIndex = -1;
            ddlTransFor.SelectedIndex = -1;
            ddlTransMode.SelectedIndex = -1;
            //ddlCostPID.SelectedIndex = -1;
            ddlCostPool.SelectedIndex = -1;
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

            txtTransDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
            DateTime today = dbFunctions.dateConvert(txtTransDate.Text);
            string mon = today.ToString("MMM").ToUpper();
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

        public string PrefixText { get; set; }

        public int Count { get; set; }

        public string ContextKey { get; set; }

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
                txtAmount.Focus();
            }
        }

        public void Save_Print()
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
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
                string userName = CookiesData["USERID"].ToString();
                string userPc = CookiesData["PCName"].ToString();
                string userIp = CookiesData["IpAddress"].ToString();


                try
                {
                    iob.Transtp = ddlTransType.Text;
                    Session["TransType"] = ddlTransType.Text;
                    iob.Transdt = (DateTime.Parse(txtTransDate.Text, dateformat, DateTimeStyles.AssumeLocal));
                    Session["TransDate"] = txtTransDate.Text;
                    Session["TransMonthYear"] = txtTransYear.Text;
                    iob.Monyear = txtTransYear.Text;
                    iob.Voucher = int.Parse(txtVouchNo.Text);
                    Session["VouchNo"] = txtVouchNo.Text;
                    iob.Transfor = ddlTransFor.Text;
                    iob.Costpid = ddlCostPool.SelectedValue;
                    iob.Transmode = ddlTransMode.Text;
                    Session["TransMode"] = ddlTransMode.Text;
                    iob.Debitcd = txtDebited.Text;
                    Session["DebitCD"] = txtDebited.Text;
                    iob.Creditcd = txtCredited.Text;
                    Session["CreditCD"] = txtCredited.Text;
                    iob.Chequeno = txtCheque.Text;
                    Session["ChequeNo"] = txtCheque.Text;
                    iob.Chequedt = txtChequeDate.Text == "" ? DateTime.Parse("01/01/1900") : (DateTime.Parse(txtChequeDate.Text, dateformat, DateTimeStyles.AssumeLocal));
                    Session["ChequeDT"] = txtChequeDate.Text;
                    iob.Remarks = txtRemarks.Text;
                    Session["Remarks"] = txtRemarks.Text;
                    iob.Amount = Convert.ToDecimal(txtAmount.Text);
                    Session["Amount"] = txtAmount.Text;
                    iob.Inword = txtInwords.Text;
                    Session["Inword"] = txtInwords.Text;
                    iob.UserId = Convert.ToInt64(userName);
                    iob.InTM = dbFunctions.timezone(DateTime.Now);
                    iob.Userpc = userPc;
                    iob.Ipaddress = userIp;
                    //iob.FiscalYear = txtFiscalYear.Text;

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
                ddlTransFor.SelectedIndex = -1;
                ddlCostPool.SelectedIndex = -1;

                DateTime trnsDy = (DateTime.Parse(txtTransDate.Text, dateformat, DateTimeStyles.AssumeLocal));

                string mon = trnsDy.Date.ToString("MMM").ToUpper();
                string year = trnsDy.ToString("yy");
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
                txtCNDebitNm.Focus();
                ddlCostPool.Focus();
                lblCheque.Visible = false;
                lblChequeDT.Visible = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else if (ddlCostPool.Text == "")
            {
                lblErrMSg.Visible = true;
                lblErrMSg.Text = "Select Branch Name.";
                ddlCostPool.Focus();
            }
            else
            {
                txtTransDate_TextChanged(sender, e);
                if (txtDebited.Text == "")
                {
                    //Response.Write("<script>alert('Please Select Account Head.');</script>");

                    txtDebited.Focus();
                }
                else if (txtCredited.Text == "")
                {
                    //Response.Write("<script>alert('Please Select Account Head.');</script>");
                    txtCNCreditNm.Focus();

                }
                else if (txtAmount.Text == "")
                {
                    //Response.Write("<script>alert('Please Fill the Amount');</script>");
                    txtAmount.Focus();
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
                        // ScriptManager.RegisterStartupScript(this,
                        //       GetType(), "OpenWindow", "window.open('../Report/Report/RptCreditVoucher.aspx','_newtab');", true);
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
                        // ScriptManager.RegisterStartupScript(this,
                        //       GetType(), "OpenWindow", "window.open('../Report/Report/RptCreditVoucher.aspx','_newtab');", true);
                    }
                }
                else
                {
                    Save_Print();
                    // ScriptManager.RegisterStartupScript(this,
                    //           GetType(), "OpenWindow", "window.open('../Report/Report/RptCreditVoucher.aspx','_newtab');", true);
                }

            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
            lblErrMSg.Text = "";
            lblErrMSg.Visible = false;
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
            btnSave.Focus();
        }

        protected void txtInwords_TextChanged(object sender, EventArgs e)
        {
            if (txtInwords.Text == "")
                Response.Write("<script>alert('Please Fill the Amount');</script>");
            //ddlS.Focus();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
             ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "OpenWindow", "window.open('/Accounts/UI/EditSingleVoucher.aspx','_newtab');", true);
        }
    }
}