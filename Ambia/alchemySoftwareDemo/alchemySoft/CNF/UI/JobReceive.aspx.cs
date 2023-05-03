using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft.CNF.DataAccess;
using alchemySoft.CNF.Interface;

namespace alchemySoft.CNF.UI
{
    public partial class JobReceive : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;

        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        cnf_Interface iob = new cnf_Interface();
        cnf_data dob = new cnf_data();
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }
            else
            {
                DateTime dateTime = Convert.ToDateTime("2018 AUG 05");
                string formLink = "/CNF/UI/JobReceive.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");

                if (permission)
                {
                    if (!IsPostBack)
                    {
                        DateTime td = dbFunctions.timezone(DateTime.Now);
                        txtReceiveDate.Text = td.ToString("dd/MM/yyyy");
                        string mon = td.ToString("MMM").ToUpper();
                        string year = td.ToString("yy");
                        lblMY.Text = mon + "-" + year;
                        txtTransMY.Text = lblMY.Text;

                        Session["dis"] = null;
                        Session["dis"] = ddlRcvType.Text;

                        txtReceiveDate.Focus();

                        dbFunctions.lblAdd("select MAX(TRANSNO) from CNF_JOBRCV where TRANSMY='" + txtTransMY.Text + "' ", lblSL);
                        if (lblSL.Text == "")
                        {
                            txtVoucher.Text = "1";
                        }
                        else
                        {
                            var id = Int64.Parse(lblSL.Text) + 1;
                            txtVoucher.Text = id.ToString();

                        }
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        protected void txtReceiveDate_TextChanged(object sender, EventArgs e)
        {
            DateTime transdate = (DateTime.Parse(txtReceiveDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string month = transdate.ToString("MMM").ToUpper();
            string years = transdate.ToString("yy");

            txtTransMY.Text = month + "-" + years;

            dbFunctions.lblAdd("select MAX(TRANSNO) from CNF_JOBRCV where TRANSMY='" + txtTransMY.Text + "' ", lblSL);
            if (lblSL.Text == "")
            {
                txtVoucher.Text = "1";
            }
            else
            {
                var id = Int64.Parse(lblSL.Text) + 1;
                txtVoucher.Text = id.ToString();
            }
            ddlRcvType.Focus();
        }

        protected void ddlTransMy_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbFunctions.dropDown_Bind(ddlVouchNo, "", "SELECT", "Select TRANSNO AS NM from CNF_JOBRCV where TRANSMY='" + ddlTransMy.Text + "' ");

            txtCompanyID.Text = "";
            txtCompanyNM.Text = "";
            txtJobID.Text = "";
            txtJobType.Text = "";
            txtJobYear.Text = "";
            txtPartyID.Text = "";
            txtPartyNM.Text = "";
            txtAmount.Text = "";
            txtRemarks.Text = "";
            txtInwords.Text = "";
            txtCashBankID.Text = "";
            txtCashBankNM.Text = "";
            ddlVouchNo.Focus();

        }

        protected void ddlVouchNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbFunctions.txtAdd(" SELECT COMPID FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtCompanyID);
            dbFunctions.txtAdd("SELECT COMPNM FROM ASL_COMPANY WHERE COMPID = '" + txtCompanyID.Text + "' ", txtCompanyNM);

            dbFunctions.txtAdd(" SELECT JOBYY FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtJobYear);
            dbFunctions.txtAdd(" SELECT JOBTP FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtJobType);
            dbFunctions.txtAdd(" SELECT JOBNO FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtJobID);


            dbFunctions.txtAdd(" SELECT PARTYID FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtPartyID);
            dbFunctions.txtAdd("SELECT ACCOUNTNM FROM GL_ACCHART WHERE ACCOUNTCD = '" + txtPartyID.Text + "' AND STATUSCD ='P'", txtPartyNM);

            dbFunctions.txtAdd(" SELECT DEBITCD FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtCashBankID);
            dbFunctions.txtAdd("SELECT  ACCOUNTNM FROM GL_ACCHART WHERE ACCOUNTCD = '" + txtCashBankID.Text + "' ", txtCashBankNM);


            dbFunctions.txtAdd(" SELECT AMOUNT FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtAmount);

            dbFunctions.txtAdd(" SELECT REMARKS FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtRemarks);

            dbFunctions.txtAdd(" SELECT CONVERT(NVARCHAR(20),TRANSDT,103) AS TRANSD FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", txtReceiveDate);

            dbFunctions.lblAdd(" SELECT TRANSFOR FROM CNF_JOBRCV WHERE TRANSNO='" + ddlVouchNo.Text + "' AND TRANSMY='" + ddlTransMy.Text + "' ", lbltransfor);
            ddlRcvType.Text = lbltransfor.Text;

            Session["dis"] = null;
            Session["dis"] = ddlRcvType.Text;

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

            string AmtConv = dbFunctions.SpellAmount.MoneyConvFn(txtAmount.Text.ToString().Trim());
            //string amntComma = SpellAmount.comma(Convert.ToDecimal(txtAmount.Text));
            //Label3.Text = amntComma;

            txtInwords.Text = AmtConv.Trim();
            txtInwords.ForeColor = System.Drawing.Color.Green;

        }

        protected void ddlRcvType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["dis"] = null;
            Session["dis"] = ddlRcvType.Text;
            txtJobID.Focus();
        }

        protected void txtCashBankNM_TextChanged(object sender, EventArgs e)
        {
            if (txtCashBankNM.Text == "")
            {
                //lblErrmsg.Visible = true;
                //lblErrmsg.Text = "Select cash/bank id.";
                dbFunctions.popupAlert(Page, "Select cash/bank id.", "w");
                txtCashBankNM.Focus();
            }
            else
            {
                dbFunctions.txtAdd("SELECT ACCOUNTCD FROM GL_ACCHART WHERE ACCOUNTNM = '" + txtCashBankNM.Text + "' AND STATUSCD ='P'", txtCashBankID);
                if (txtCashBankID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select cash/bank id.";
                    dbFunctions.popupAlert(Page, "Select cash/bank id.", "w");
                    txtCashBankNM.Text = "";
                    txtCashBankNM.Focus();
                }
                else
                {
                    lblErrmsg.Visible = false;
                    txtRemarks.Focus();
                }
            }
        }
        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
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
                x1 = txtAmount.Text.ToString().Trim().Substring(0, txtAmount.Text.Trim().IndexOf(".", StringComparison.Ordinal));
                x2 = txtAmount.Text.ToString().Trim().Substring(txtAmount.Text.Trim().IndexOf(".", StringComparison.Ordinal) + 1);
            }
            else
            {
                x1 = txtAmount.Text.Trim();
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
                txtAmount.Text = Math.Round(Convert.ToDouble(txtAmount.Text), 2).ToString(CultureInfo.InvariantCulture).Trim();
            }

            string amtConv = dbFunctions.SpellAmount.MoneyConvFn(txtAmount.Text.ToString().Trim());
            //string amntComma = SpellAmount.comma(Convert.ToDecimal(txtAmount.Text));
            //Label3.Text = amntComma;

            txtInwords.Text = amtConv.Trim();
            txtInwords.ForeColor = System.Drawing.Color.Green;

            btnSave.Focus();
        }
        protected void txtJobID_TextChanged(object sender, EventArgs e)
        {
            if (txtJobID.Text == "")
            {
                //lblErrmsg.Visible = true;
                //lblErrmsg.Text = "Select job no.";
                dbFunctions.popupAlert(Page, "Select job no", "w");
                txtJobID.Focus();
            }
            else
            {
                // lblErrmsg.Visible = false;

                string jobno = "";
                string jobyear = "";
                string jobtp = "";
                string searchPar = txtJobID.Text;

                int splitter = searchPar.IndexOf("|", StringComparison.Ordinal);
                if (splitter != -1)
                {
                    string[] lineSplit = searchPar.Split('|');

                    jobno = lineSplit[0];
                    jobyear = lineSplit[1];
                    jobtp = lineSplit[2];

                    txtJobID.Text = jobno.Trim();
                    txtJobYear.Text = jobyear.Trim();
                    txtJobType.Text = jobtp.Trim();
                    txtCompanyID.Text = "";

                    dbFunctions.txtAdd("SELECT COMPID FROM CNF_JOB WHERE JOBNO =" + jobno + " AND JOBYY =" + jobyear + " AND JOBTP ='" + jobtp + "'", txtCompanyID);
                    dbFunctions.txtAdd("SELECT (BRANCHID + '-' + COMPNM) AS COMPNM FROM ASL_COMPANY WHERE COMPID = '" + txtCompanyID.Text + "' ", txtCompanyNM);

                    dbFunctions.txtAdd("SELECT PARTYID FROM CNF_JOB WHERE JOBNO =" + jobno + " AND JOBYY =" + jobyear + " AND JOBTP ='" + jobtp + "'", txtPartyID);
                    dbFunctions.txtAdd("SELECT ACCOUNTNM FROM GL_ACCHART WHERE ACCOUNTCD = '" + txtPartyID.Text + "' ", txtPartyNM);

                    txtCashBankNM.Focus();
                }
                else
                {
                    txtJobID.Text = "";
                    txtJobYear.Text = "";
                    txtJobType.Text = "";
                    txtCompanyID.Text = "";
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblErrmsg.Visible = false;
            txtTransMY.Visible = true;
            txtVoucher.Visible = true;

            txtReceiveDate.Enabled = true;
            ddlTransMy.Visible = false;
            ddlVouchNo.Visible = false;

            btnSave.Text = "Save";

            btnUpdate.Visible = true;
            btnCancel.Visible = false;

            Refresh();

            DateTime td = dbFunctions.timezone(DateTime.Now);
            txtReceiveDate.Text = td.ToString("dd/MM/yyyy");
            string mon = td.ToString("MMM").ToUpper();
            string year = td.ToString("yy");
            lblMY.Text = mon + "-" + year;
            txtTransMY.Text = lblMY.Text;
            btnDelete.Visible = false;
            txtJobID.Focus();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblErrmsg.Visible = false;
            txtTransMY.Visible = false;
            txtVoucher.Visible = false;

            txtReceiveDate.Enabled = false;
            ddlTransMy.Visible = true;
            ddlVouchNo.Visible = true;

            btnSave.Text = "Update";

            btnUpdate.Visible = false;
            btnCancel.Visible = true;
            btnDelete.Visible = true;
            //btnUpdate.Text = "Cancel";

            dbFunctions.dropDown_Bind(ddlTransMy, "", "SELECT", "Select distinct TRANSMY AS NM from CNF_JOBRCV");

            dbFunctions.dropDown_Bind(ddlVouchNo, "", "SELECT", "Select TRANSNO AS NM from CNF_JOBRCV where TRANSMY='" + ddlTransMy.Text + "' ");
            ddlTransMy.Focus();



            //dbFunctions.dropDownAddWithSelect(ddlVouchNo, "Select TRANSNO from CNF_JOBRCV where TRANSMY='" + ddlTransMy.Text + "' ");


        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ddlTransMy.Text == "Select")
            {
                //lblErrmsg.Visible = true;
                //lblErrmsg.Text = "Select month.";
                dbFunctions.popupAlert(Page, "Select month", "w");
                ddlTransMy.Focus();
            }
            else if (ddlVouchNo.Text == "Select")
            {
                //lblErrmsg.Visible = true;
                //lblErrmsg.Text = "Select Voucher no.";
                dbFunctions.popupAlert(Page, "Select voucher no.", "w");
                ddlVouchNo.Focus();
            }
            else
            {

                string TRANSDT = "";
                string TRANSMY = "";
                string TRANSNO = "";
                string TRANSFOR = "";
                string COMPID = "";
                string JOBYY = "";
                string JOBTP = "";
                string JOBNO = "";
                string PARTYID = "";
                string DEBITCD = "";
                string REMARKS = "";
                string AMOUNT = "";
                string USERPC = "";
                string USERID = "";
                string UPDATEUSERID = "";
                string INTIME = "";
                string UPDATETIME = "";
                string IPADDRESS = "";

                iob.TRANSMY = ddlTransMy.Text;
                iob.TRANSNO = Convert.ToInt64(ddlVouchNo.Text);

                string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);
                string userName = HttpContext.Current.Session["UserName"].ToString();

                if (conn.State != ConnectionState.Open) conn.Open();

                SqlCommand cmdselectdata = new SqlCommand("Select * FROM CNF_JOBRCV where TRANSMY=@TRANSMY and TRANSNO=@TRANSNO", conn);
                cmdselectdata.Parameters.Clear();
                cmdselectdata.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = iob.TRANSMY;
                cmdselectdata.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = iob.TRANSNO;

                SqlDataReader dr = cmdselectdata.ExecuteReader();
                while (dr.Read())
                {
                    TRANSDT = dr["TRANSDT"].ToString();
                    TRANSMY = dr["TRANSMY"].ToString();
                    TRANSNO = dr["TRANSNO"].ToString();
                    TRANSFOR = dr["TRANSFOR"].ToString();
                    COMPID = dr["COMPID"].ToString();
                    JOBYY = dr["JOBYY"].ToString();
                    JOBTP = dr["JOBTP"].ToString();
                    JOBNO = dr["JOBNO"].ToString();
                    PARTYID = dr["PARTYID"].ToString();
                    DEBITCD = dr["DEBITCD"].ToString();
                    AMOUNT = dr["AMOUNT"].ToString();
                    REMARKS = dr["REMARKS"].ToString();
                    USERPC = dr["USERPC"].ToString();
                    USERID = dr["USERID"].ToString();
                    UPDATEUSERID = dr["UPDATEUSERID"].ToString();
                    INTIME = dr["INTIME"].ToString();
                    UPDATETIME = dr["UPDATETIME"].ToString();
                    IPADDRESS = dr["IPADDRESS"].ToString();

                }
                dr.Close();

                string alldata = TRANSDT + ", " + TRANSMY + ", " + TRANSNO + ", " + TRANSFOR
                    + ", " + COMPID + ", " + JOBYY + ", " + JOBTP + ", " + JOBNO + ", " + PARTYID + ", " + DEBITCD + ", " + DEBITCD + ", " + AMOUNT
                    + ", " + REMARKS + ", " + USERPC + ", " + USERID + ", " + UPDATEUSERID + ", " + INTIME + ", " + UPDATETIME + ", " + IPADDRESS;

                iob.InTM = dbFunctions.timezone(DateTime.Now);
                string userpc = HttpContext.Current.Session["PCName"].ToString(); ;
                string ipadd = HttpContext.Current.Session["IpAddress"].ToString();


                SqlCommand cmdinsert = new SqlCommand("insert into ASL_DLT values('CNF_JOBRCV',@DESCRP,@USERPC,@USERID,@INTIME,@IPADD)", conn);
                cmdinsert.Parameters.AddWithValue("@DESCRP", alldata);
                cmdinsert.Parameters.AddWithValue("@USERPC", userpc);
                cmdinsert.Parameters.AddWithValue("@USERID", userName);
                cmdinsert.Parameters.AddWithValue("@INTIME", iob.InTM);
                cmdinsert.Parameters.AddWithValue("@IPADD", ipadd);

                cmdinsert.ExecuteNonQuery();





                lblErrmsg.Visible = false;


                dob.DeleteJobReceive(iob);
                Refresh();

                dbFunctions.dropDown_Bind(ddlVouchNo, "", "SELECT", "Select TRANSNO AS NM from CNF_JOBRCV where TRANSMY='" + ddlTransMy.Text + "' ");

                //lblErrmsg.Visible = true;
                //lblErrmsg.Text = "Data deleted.";
                dbFunctions.popupAlert(Page, "Data deleted.!", "w");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtCompanyID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no", "w");
                    txtJobID.Focus();
                }
                else if (txtJobYear.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no", "w");
                    txtJobID.Focus();
                }
                else if (txtCashBankID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select Bank Id.";
                    dbFunctions.popupAlert(Page, "Select Bank Id.", "w");
                    txtCashBankNM.Text = "";
                    txtCashBankNM.Focus();
                }
                else if (txtJobType.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no.", "w");
                    txtJobID.Focus();
                }
                else if (txtPartyID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no.", "w");
                    txtJobID.Focus();
                }
                else if (txtAmount.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Type amount.";
                    dbFunctions.popupAlert(Page, "Type amount.", "w");
                    txtAmount.Focus();
                }
                else
                {

                    iob.TRANSDT = DateTime.Parse(txtReceiveDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);

                    //iob.TRANSNO = Convert.ToInt64(txtVoucher.Text);
                    iob.TRANSFOR = ddlRcvType.Text;
                    iob.CompID = txtCompanyID.Text;
                    iob.JOBNO = Convert.ToInt64(txtJobID.Text);
                    iob.JOBYY = Convert.ToInt64(txtJobYear.Text);
                    iob.JOBTP = txtJobType.Text;
                    iob.PARTYID = txtPartyID.Text;
                    iob.DEBITCD = txtCashBankID.Text;
                    iob.REMARKS = txtRemarks.Text;
                    iob.Amount = Convert.ToDecimal(txtAmount.Text);
                    iob.UserID = CookiesData["USERID"].ToString();
                    iob.Userpc = CookiesData["PCName"].ToString();
                    iob.IPAddress = CookiesData["IpAddress"].ToString();
                    iob.InTime = dbFunctions.timezone(DateTime.Now);
                    iob.UpdateTime = dbFunctions.timezone(DateTime.Now);

                    if (btnSave.Text == "Save")
                    {
                        if (txtTransMY.Text == "")
                        {
                            //lblErrmsg.Visible = true;
                            //lblErrmsg.Text = "Select date.";
                            dbFunctions.popupAlert(Page, "Select date.", "w");
                            txtReceiveDate.Focus();
                        }
                        else
                        {
                            dbFunctions.lblAdd("select MAX(TRANSNO) from CNF_JOBRCV where TRANSMY='" + txtTransMY.Text + "' ", lblSL);
                            if (lblSL.Text == "")
                            {
                                txtVoucher.Text = "1";
                                iob.TRANSNO = Convert.ToInt64(txtVoucher.Text);
                            }

                            else
                            {
                                var id = Int64.Parse(lblSL.Text) + 1;
                                txtVoucher.Text = id.ToString();
                                iob.TRANSNO = Convert.ToInt64(txtVoucher.Text);
                            }
                            iob.TRANSMY = txtTransMY.Text;
                            dob.SaveJobReceive(iob);
                            Refresh();
                            txtJobID.Focus();
                        }
                    }

                    else if (btnSave.Text == "Update")
                    {
                        if (ddlTransMy.Text == "Select")
                        {
                            //lblErrmsg.Visible = true;
                            //lblErrmsg.Text = "Select month year.";
                            dbFunctions.popupAlert(Page, "Select month year.", "w");
                            ddlTransMy.Focus();
                        }
                        else if (ddlVouchNo.Text == "")
                        {
                            //lblErrmsg.Visible = true;
                            //lblErrmsg.Text = "Select voucher no.";
                            dbFunctions.popupAlert(Page, "Select voucher no.", "w");
                            ddlVouchNo.Focus();
                        }
                        else
                        {
                            iob.TRANSNO = Convert.ToInt64(ddlVouchNo.Text);
                            iob.TRANSMY = ddlTransMy.Text;
                            try
                            {

                                string alldata = dbFunctions.StringData(@"SELECT ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+ISNULL(TRANSFOR,'(NULL)')+'  '+ISNULL(COMPID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),JOBYY,103),'(NULL)')+'  '+ISNULL(JOBTP,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),JOBNO,103),'(NULL)')+'  '+ISNULL(PARTYID,'(NULL)')+'  '+ISNULL(DEBITCD,'(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+CONVERT(NVARCHAR(50),AMOUNT,103)+'  '+ISNULL(USERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+ISNULL(IPADDRESS,'(NULL)') FROM CNF_JOBRCV
                                 where TRANSDT='" + iob.TRANSDT + "' and TRANSMY='" + iob.TRANSMY + "' and TRANSNO=" + iob.TRANSNO + "");
                                SqlConnection con = new SqlConnection(dbFunctions.Connection);
                                SqlCommand cmdinsert = new SqlCommand("insert into ASL_DLT values('CNF_JOBRCV-UPDATE',@DESCRP,@USERPC,@USERID,@INTIME,@IPADD)", con);
                                cmdinsert.Parameters.AddWithValue("@DESCRP", alldata);
                                cmdinsert.Parameters.AddWithValue("@USERPC", iob.Userpc);
                                cmdinsert.Parameters.AddWithValue("@USERID", iob.UserID);
                                cmdinsert.Parameters.AddWithValue("@INTIME", iob.UpdateTime);
                                cmdinsert.Parameters.AddWithValue("@IPADD", iob.IPAddress);
                                con.Open();
                                cmdinsert.ExecuteNonQuery();
                                con.Close();

                            }
                            catch (Exception ex)
                            {
                                Session["ERROR"] = ex.ToString();
                                dbFunctions.ErrorLog(Page, ex, sender);
                                throw;
                            }

                            dob.UpdateJobReceive(iob);
                            Refresh();
                            ddlVouchNo.Focus();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Response.Write(ex);
                //Session["ERROR"] = ex.ToString();
                //dbFunctions.ErrorLog(Page, ex, sender);
                //HttpContext.Current.Response.Redirect("/Error/error.aspx");
            }
        }

        public void Save_Print()
        {
            Session["ReceiveTp"] = "";
            Session["CompID"] = "";
            Session["Jobno"] = "";
            Session["JobYear"] = "";
            Session["JobTp"] = "";
            Session["PartyID"] = "";
            Session["DebitCD"] = "";
            Session["Remarks"] = "";
            Session["Amount"] = "";
            Session["Inwords"] = "";
            Session["voucherNo"] = "";

            try
            {
                if (txtCompanyID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no.", "w");
                    txtJobID.Focus();
                }
                else if (txtJobYear.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no.", "w");
                    txtJobID.Focus();
                }
                else if (txtCashBankID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select Bank Id.";
                    dbFunctions.popupAlert(Page, "Select Bank id.", "w");
                    txtCashBankNM.Text = "";
                    txtCashBankNM.Focus();
                }
                else if (txtJobType.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no", "w");
                    txtJobID.Focus();
                }
                else if (txtPartyID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no.", "w");
                    txtJobID.Focus();
                }
                else if (txtCashBankID.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Select cash/bank id.";
                    dbFunctions.popupAlert(Page, "Select cash/bank id.", "w");
                    txtCashBankNM.Focus();
                }
                else if (txtAmount.Text == "")
                {
                    //lblErrmsg.Visible = true;
                    //lblErrmsg.Text = "Type amount.";
                    dbFunctions.popupAlert(Page, "Type amount.", "w");
                    txtAmount.Focus();
                }
                else
                {
                    iob.TRANSDT = DateTime.Parse(txtReceiveDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);

                    //iob.TRANSNO = Convert.ToInt64(txtVoucher.Text);


                    iob.TRANSFOR = ddlRcvType.Text;
                    Session["ReceiveTp"] = ddlRcvType.Text;

                    iob.CompID = txtCompanyID.Text;
                    Session["CompID"] = txtCompanyID.Text;

                    iob.JOBNO = Convert.ToInt64(txtJobID.Text);
                    Session["Jobno"] = Convert.ToInt64(txtJobID.Text);

                    iob.JOBYY = Convert.ToInt64(txtJobYear.Text);
                    Session["JobYear"] = Convert.ToInt64(txtJobYear.Text);

                    iob.JOBTP = txtJobType.Text;
                    Session["JobTp"] = txtJobType.Text;

                    iob.PARTYID = txtPartyID.Text;
                    Session["PartyID"] = txtPartyID.Text;

                    iob.DEBITCD = txtCashBankID.Text;
                    Session["DebitCD"] = txtCashBankID.Text;

                    iob.REMARKS = txtRemarks.Text;
                    Session["Remarks"] = txtRemarks.Text;

                    iob.Amount = Convert.ToDecimal(txtAmount.Text);
                    Session["Amount"] = Convert.ToDecimal(txtAmount.Text);


                    Session["Inwords"] = txtInwords.Text;

                    iob.Userpc = CookiesData["PCName"].ToString();
                    iob.IPAddress = CookiesData["IpAddress"].ToString();
                    iob.InTime = dbFunctions.timezone(DateTime.Now);
                    iob.UpdateTime = dbFunctions.timezone(DateTime.Now);


                    if (btnSave.Text == "Save")
                    {
                        iob.TRANSMY = txtTransMY.Text;
                        iob.TRANSNO = Convert.ToInt64(txtVoucher.Text);
                        Session["voucherNo"] = Convert.ToInt64(txtVoucher.Text);
                        dob.SaveJobReceive(iob);
                    }
                    else
                    {
                        iob.TRANSMY = ddlTransMy.Text;
                        iob.TRANSNO = Convert.ToInt64(ddlVouchNo.Text);
                        Session["voucherNo"] = Convert.ToInt64(ddlVouchNo.Text);
                        dob.UpdateJobReceive(iob);
                    }

                    Refresh();
                    txtJobID.Focus();
                }
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        protected void btnSave_Print_Click(object sender, EventArgs e)
        {
            Save_Print();

             ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "OpenWindow", "window.open('../report/vis-rep/RptReceive_VR.aspx','_newtab');", true);
        }

        public void Refresh()
        {
            ddlRcvType.SelectedIndex = -1;
            txtCompanyID.Text = "";
            txtCompanyNM.Text = "";
            txtJobID.Text = "";
            txtJobType.Text = "";
            txtJobYear.Text = "";
            txtPartyID.Text = "";
            txtPartyNM.Text = "";
            txtAmount.Text = "";
            txtRemarks.Text = "";
            txtInwords.Text = "";
            txtCashBankID.Text = "";
            txtCashBankNM.Text = "";
            ddlVouchNo.SelectedIndex = -1;


            DateTime transdate = (DateTime.Parse(txtReceiveDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string month = transdate.ToString("MMM").ToUpper();
            string years = transdate.ToString("yy");

            txtTransMY.Text = month + "-" + years;

            dbFunctions.lblAdd("select MAX(TRANSNO) from CNF_JOBRCV where TRANSMY='" + lblMY.Text + "' ", lblSL);
            if (lblSL.Text == "")
            {
                txtVoucher.Text = "1";
            }
            else
            {
                var id = Int64.Parse(lblSL.Text) + 1;
                txtVoucher.Text = id.ToString();

            }

        }
    }
}