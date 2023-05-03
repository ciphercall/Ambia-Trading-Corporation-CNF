using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft.CNF.DataAccess;
using alchemySoft.CNF.Interface;
using alchemySoft.LogIn;

namespace alchemySoft.CNF.UI
{
    public partial class Job_Bill_Information : System.Web.UI.Page
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
                string formLink = "/CNF/UI/Job-Bill-Information.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                //if (dbFunctions.timezone(DateTime.Now) >= dateTime)
                //    Response.Redirect("~/Servererror.html");
                if (permission)
                {
                    if (!IsPostBack)
                    {
                        txtJobID.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        private void GridShow()
        {
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            conn.Open();

            SqlCommand cmdd = new SqlCommand("SELECT CONVERT(NVARCHAR(20), dbo.CNF_JOBBILL.BILLDT, 103) AS BILLD, dbo.CNF_JOBBILL.BILLNO, dbo.CNF_JOBBILL.EXPSL, dbo.CNF_JOBBILL.EXPID, " +
                    " dbo.CNF_JOBBILL.EXPAMT, dbo.CNF_JOBBILL.BILLAMT, (CASE WHEN EXPPDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),dbo.CNF_JOBBILL.EXPPDT, 103) END) AS EXPPD, dbo.CNF_JOBBILL.REMARKS, dbo.CNF_JOBBILL.BILLSL, dbo.CNF_EXPENSE.EXPNM " +
                    " FROM dbo.CNF_JOBBILL INNER JOIN dbo.CNF_EXPENSE ON dbo.CNF_JOBBILL.EXPID = dbo.CNF_EXPENSE.EXPID WHERE (dbo.CNF_JOBBILL.JOBNO = " + txtJobID.Text + ") AND (dbo.CNF_JOBBILL.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOBBILL.JOBYY = '" + txtJobYear.Text + "') AND (dbo.CNF_JOBBILL.COMPID ='" + txtCompanyID.Text + "')" +
                    " ORDER BY CNF_JOBBILL.EXPSL", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmdd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();


            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                gvDetails.Visible = true;

                TextBox txtBillDate = (TextBox)gvDetails.FooterRow.FindControl("txtBillDate");

                dbFunctions.txtAdd("SELECT DISTINCT CONVERT(NVARCHAR(20), dbo.CNF_JOBBILL.BILLDT, 103) AS BILLD " +
                   " FROM dbo.CNF_JOBBILL INNER JOIN dbo.CNF_EXPENSE ON dbo.CNF_JOBBILL.EXPID = dbo.CNF_EXPENSE.EXPID WHERE (dbo.CNF_JOBBILL.JOBNO = " + txtJobID.Text + ") AND (dbo.CNF_JOBBILL.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOBBILL.JOBYY = '" + txtJobYear.Text + "') AND (dbo.CNF_JOBBILL.COMPID ='" + txtCompanyID.Text + "') ", txtBillDate);

                if (txtBillDate.Text == "" || txtBillDate.Text == "01/01/1900")
                {
                    txtBillDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                }

                TextBox txtBillNo = (TextBox)gvDetails.FooterRow.FindControl("txtBillNo");
                txtBillNo.Text = txtJobID.Text;

                TextBox txtExpense = (TextBox)gvDetails.FooterRow.FindControl("txtExpense");
                txtExpense.Focus();

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
                TextBox txtBillDate = (TextBox)gvDetails.FooterRow.FindControl("txtBillDate");
                txtBillDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                TextBox txtBillNo = (TextBox)gvDetails.FooterRow.FindControl("txtBillNo");
                txtBillNo.Text = txtJobID.Text;
                gvDetails.FooterRow.Visible = true;
            }
        }

        protected void txtJobID_TextChanged(object sender, EventArgs e)
        {
            if (txtJobID.Text == "")
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Select job no.";
                txtJobID.Focus();
            }
            else
            {
                lblErrmsg.Visible = false;

                string jobno = "";
                string jobyear = "";
                string jobtp = "";
                string searchPar = txtJobID.Text;

                int splitter = searchPar.IndexOf("|");
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
                    dbFunctions.txtAdd("SELECT (BRANCHID + '-' + BRANCHNM) FROM ASL_BRANCH WHERE BRANCHCD = '" + txtCompanyID.Text + "' ", txtCompanyNM);

                    dbFunctions.txtAdd("SELECT PARTYID FROM CNF_JOB WHERE JOBNO =" + jobno + " AND JOBYY =" + jobyear + " AND JOBTP ='" + jobtp + "'", txtPartyID);
                    dbFunctions.txtAdd("SELECT ACCOUNTNM FROM GL_ACCHART WHERE ACCOUNTCD = '" + txtPartyID.Text + "' ", txtPartyNM);

                    dbFunctions.txtAdd("SELECT CONVERT(NVARCHAR(20),JOBCDT,103) AS TRANSD FROM CNF_JOB WHERE JOBNO =" + jobno + " AND JOBYY =" + jobyear + " AND JOBTP ='" + jobtp + "'", txtReceiveDate);
                    dbFunctions.txtAdd("SELECT COMM_AMT FROM CNF_JOB WHERE JOBNO =" + jobno + " AND JOBYY =" + jobyear + " AND JOBTP ='" + jobtp + "'", txtJobCommission);
                    dbFunctions.txtAdd("SELECT ASSV_BDT FROM CNF_JOB WHERE JOBNO =" + jobno + " AND JOBYY =" + jobyear + " AND JOBTP ='" + jobtp + "'", txtAssValue);
                    dbFunctions.txtAdd("SELECT CNFV_USD FROM CNF_JOB WHERE JOBNO =" + jobno + " AND JOBYY =" + jobyear + " AND JOBTP ='" + jobtp + "'", txtUsdValue);
                    GridShow();
                }
                else
                {
                    txtJobID.Text = "";
                    txtJobYear.Text = "";
                    txtJobType.Text = "";
                    txtCompanyID.Text = "";
                    lblErrmsg.Visible = true;
                    lblErrmsg.Text = "Select job no.";
                    txtJobID.Focus();
                }
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                TextBox txtBillDate = (TextBox)gvDetails.FooterRow.FindControl("txtBillDate");
                Label lblSlItem = (Label)gvDetails.FooterRow.FindControl("lblSlItem");
                TextBox txtFwdDate = (TextBox)gvDetails.FooterRow.FindControl("txtFwdDate");
                TextBox txtBillNo = (TextBox)gvDetails.FooterRow.FindControl("txtBillNo");

                TextBox txtExpenseID = (TextBox)gvDetails.FooterRow.FindControl("txtExpenseID");
                TextBox txtExpense = (TextBox)gvDetails.FooterRow.FindControl("txtExpense");
                TextBox txtExpenseAmount = (TextBox)gvDetails.FooterRow.FindControl("txtExpenseAmount");
                TextBox txtBillAmount = (TextBox)gvDetails.FooterRow.FindControl("txtBillAmount");
                TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");
                TextBox txtBillSl = (TextBox)gvDetails.FooterRow.FindControl("txtBillSl");

                if (e.CommandName.Equals("SaveCon"))
                {
                    if (txtCompanyID.Text == "")
                    {
                        lblErrMsgExist.Visible = true;
                        lblErrMsgExist.Text = "particular input missing";
                        txtCompanyID.Focus();
                    }
                    else if (txtExpenseID.Text == "")
                    {
                        lblErrMsgExist.Visible = true;
                        lblErrMsgExist.Text = "particular input missing";
                        txtExpense.Focus();
                    }
                    else if (txtExpense.Text == "")
                    {
                        lblErrMsgExist.Visible = true;
                        lblErrMsgExist.Text = "particular input missing";
                        txtExpense.Focus();
                    }
                    else if (txtExpenseAmount.Text == "")
                    {
                        lblErrMsgExist.Visible = true;
                        lblErrMsgExist.Text = "particular input missing";
                        txtExpenseAmount.Focus();
                    }
                    else
                    {
                        lblErrmsg.Visible = false;

                        iob.CompID = txtCompanyID.Text;
                        iob.JOBYY = Convert.ToInt64(txtJobYear.Text);
                        iob.JOBNO = Convert.ToInt64(txtJobID.Text);
                        iob.JOBTP = txtJobType.Text;
                        iob.PARTYID = txtPartyID.Text;
                        iob.BILLDT = DateTime.Parse(txtBillDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal); ;
                        iob.BILLNO = Convert.ToInt64(txtBillNo.Text);

                        DateTime FRDT = DateTime.Parse(txtBillDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        string FDT = FRDT.ToString("yyyy-MM-dd");

                        dbFunctions.lblAdd("select MAX (EXPSL) from CNF_JOBBILL where BILLDT='" + FDT + "' and BILLNO='" + txtBillNo.Text + "' ", lblChkInternalID);

                        if (lblChkInternalID.Text == "")
                        {
                            string cid = "1";
                            lblSlItem.Text = cid;
                            iob.EXPSL = Convert.ToInt64(cid);
                        }

                        else
                        {
                            var id = Int64.Parse(lblChkInternalID.Text) + 1;
                            lblSlItem.Text = id.ToString();
                            iob.EXPSL = Convert.ToInt64(lblSlItem.Text);
                        }

                        iob.EXPID = txtExpenseID.Text;
                        iob.EXPAMT = Convert.ToDecimal(txtExpenseAmount.Text);
                        if (txtBillAmount.Text == "")
                            txtBillAmount.Text = "0";
                        iob.BILLAMT = Convert.ToDecimal(txtBillAmount.Text);
                        if (txtFwdDate.Text == "")
                            txtFwdDate.Text = "1900-01-01";
                        iob.EXPPDT = DateTime.Parse(txtFwdDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        iob.REMARKS = txtRemarks.Text;
                        if (txtBillSl.Text == "")
                            txtBillSl.Text = "0";
                        iob.BILLSL = Convert.ToInt64(txtBillSl.Text);
                        iob.InTime = dbFunctions.timezone(DateTime.Now);
                        //iob.UpdateTime = DateTime.Now;
                        iob.Userpc = CookiesData["PCName"].ToString();
                        iob.UserID = CookiesData["USERNAME"].ToString();
                        iob.IPAddress = CookiesData["ipAddress"].ToString();

                        dob.SaveJobBillInfo(iob);

                        txtExpense.Focus();
                        GridShow();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        public void Refresh()
        {
            txtJobID.Text = "";
            txtJobType.Text = "";
            txtJobYear.Text = "";
            txtPartyID.Text = "";
            txtPartyNM.Text = "";
            txtCompanyID.Text = "";
            txtCompanyNM.Text = "";

            lblErrmsg.Text = "";
            lblErrMsgExist.Text = "";
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtBillDate = (TextBox)e.Row.FindControl("txtBillDate");
                Label lblSlItem = (Label)e.Row.FindControl("lblSlItem");
                TextBox txtFwdDate = (TextBox)e.Row.FindControl("txtFwdDate");
                TextBox txtBillNo = (TextBox)e.Row.FindControl("txtBillNo");

                TextBox txtExpenseID = (TextBox)e.Row.FindControl("txtExpenseID");
                TextBox txtExpense = (TextBox)e.Row.FindControl("txtExpense");
                TextBox txtExpenseAmount = (TextBox)e.Row.FindControl("txtExpenseAmount");
                TextBox txtBillAmount = (TextBox)e.Row.FindControl("txtBillAmount");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                TextBox txtBillSl = (TextBox)e.Row.FindControl("txtBillSl");



                //DateTime today = DateTime.Now;
                //string td = dbFunctions.Dayformat(today);
                //txtBillDate.Text = td;

                //txtFwdDate.Text = td;

                //string mon = today.ToString("MMM").ToUpper();
                //string year = today.ToString("yyyy");
                if (txtReceiveDate.Text == "")
                {
                    txtBillDate.Text = "1900-01-01";
                }
                else
                    txtBillDate.Text = txtReceiveDate.Text;

                DateTime FRDT = DateTime.Parse(txtBillDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string FDT = FRDT.ToString("yyyy-MM-dd");

                dbFunctions.lblAdd("select MAX (EXPSL) from CNF_JOBBILL where BILLDT='" + FDT + "' and BILLNO='" + txtBillNo.Text + "' ", lblChkInternalID);

                if (lblChkInternalID.Text == "")
                {
                    string cid = "1";
                    lblSlItem.Text = cid;
                }

                else
                {
                    var id = Int64.Parse(lblChkInternalID.Text) + 1;
                    lblSlItem.Text = id.ToString();
                }

                dbFunctions.txtAdd("SELECT dbo.CNF_JOBBILL.BILLNO FROM dbo.CNF_JOBBILL INNER JOIN dbo.CNF_EXPENSE ON dbo.CNF_JOBBILL.EXPID = dbo.CNF_EXPENSE.EXPID " +
                    " WHERE (dbo.CNF_JOBBILL.JOBNO = " + txtJobID.Text + ") AND (dbo.CNF_JOBBILL.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOBBILL.JOBYY = '" + txtJobYear.Text + "') AND (dbo.CNF_JOBBILL.COMPID ='" + txtCompanyID.Text + "')", txtBillNo);

                txtExpense.Focus();
            }
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            GridShow();
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            GridShow();

            TextBox txtBillAmountEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtBillAmountEdit");
            txtBillAmountEdit.Focus();
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Label lblBillDateEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblBillDateEdit");
                Label lblSlEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSlEdit");
                TextBox txtFwdDateEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtFwdDateEdit");
                TextBox txtBillNoEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtBillNoEdit");

                TextBox txtExpenseIDEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtExpenseIDEdit");
                TextBox txtExpenseEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtExpenseEdit");
                TextBox txtExpenseAmountEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtExpenseAmountEdit");
                TextBox txtBillAmountEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtBillAmountEdit");
                TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarksEdit");
                TextBox txtBillSlEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtBillSlEdit");


                iob.CompID = txtCompanyID.Text;
                iob.JOBYY = Convert.ToInt64(txtJobYear.Text);
                iob.JOBNO = Convert.ToInt64(txtJobID.Text);
                iob.JOBTP = txtJobType.Text;
                iob.PARTYID = txtPartyID.Text;

                iob.BILLDT = DateTime.Parse(lblBillDateEdit.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                iob.BILLNO = Convert.ToInt64(txtBillNoEdit.Text);

                iob.EXPSL = Convert.ToInt64(lblSlEdit.Text);

                iob.EXPID = txtExpenseIDEdit.Text;
                iob.EXPAMT = Convert.ToDecimal(txtExpenseAmountEdit.Text);
                iob.BILLAMT = Convert.ToDecimal(txtBillAmountEdit.Text);
                if (txtFwdDateEdit.Text == "")
                    txtFwdDateEdit.Text = "1900-01-01";
                iob.EXPPDT = DateTime.Parse(txtFwdDateEdit.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                iob.REMARKS = txtRemarksEdit.Text;
                if (txtBillSlEdit.Text == "")
                    txtBillSlEdit.Text = "0";
                iob.BILLSL = Convert.ToInt64(txtBillSlEdit.Text);
                //iob.InTime = DateTime.Now;
                iob.UpdateTime = DateTime.Now;
                iob.Userpc = CookiesData["PCName"].ToString();
                iob.UpdateUser = CookiesData["UserName"].ToString();
                iob.IPAddress = CookiesData["ipAddress"].ToString();

                lblErrmsg.Visible = false;

                try
                {
                    // logdata add start //
                    string lotileng = HttpContext.Current.Session["Location"].ToString();
                    string ipAddress = HttpContext.Current.Session["IpAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT 'PROCESSDT : ' + ISNULL(CONVERT(NVARCHAR(50),PROCESSDT,103),'(NULL)')+' | '+'COMPID :
' + COMPID+' | '+'JOBYY : ' + CONVERT(NVARCHAR(50),JOBYY,103)+' | '+'JOBTP : ' + JOBTP+' | '+'JOBNO : ' + CONVERT(NVARCHAR(50),JOBNO,103)+' | '+'PARTYID :
' + ISNULL(PARTYID,'(NULL)')+' | '+'BILLDT : ' + ISNULL(CONVERT(NVARCHAR(50),BILLDT,103),'(NULL)')+' | '+'BILLNO :
' + ISNULL(CONVERT(NVARCHAR(50),BILLNO,103),'(NULL)')+' | '+'EXPSL : ' + ISNULL(CONVERT(NVARCHAR(50),EXPSL,103),'(NULL)')+' | '+'EXPID : ' + EXPID+' | '+'EXPAMT :
' + CONVERT(NVARCHAR(50),EXPAMT,103)+' | '+'BILLAMT : ' + CONVERT(NVARCHAR(50),BILLAMT,103)+' | '+'EXPPDT :
' + ISNULL(CONVERT(NVARCHAR(50),EXPPDT,103),'(NULL)')+' | '+'REMARKS : ' + ISNULL(REMARKS,'(NULL)')+' | '+'BILLSL :
' + ISNULL(CONVERT(NVARCHAR(50),BILLSL,103),'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'USERID :
' + ISNULL(USERID,'(NULL)')+' | '+'UPDATEUSERID : ' + ISNULL(UPDATEUSERID,'(NULL)')+' | '+'INTIME : ' + ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+' | '+'UPDATETIME :
' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + ISNULL(IPADDRESS,'(NULL)')+' | ' FROM CNF_JOBBILL
                    WHERE BILLDT='" + iob.BILLDT + "' and BILLNO='" + iob.BILLNO + "' and EXPSL='" + iob.EXPSL + "' and JOBYY='" + iob.JOBYY +
                    "' and JOBTP='" + iob.JOBTP + "' and JOBNO='" + iob.JOBNO + "' and EXPID='" + iob.EXPID + "'");



                    /*SELECT ISNULL(CONVERT(NVARCHAR(50),PROCESSDT,103),'(NULL)')+'  '+COMPID+'  '+
                    CONVERT(NVARCHAR(50),JOBYY,103)+'  '+JOBTP+'  '+CONVERT(NVARCHAR(50),JOBNO,103)+'  '+ISNULL(PARTYID,'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),BILLDT,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),BILLNO,103),'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),EXPSL,103),'(NULL)')+'  '+EXPID+'  '+CONVERT(NVARCHAR(50),EXPAMT,103)+'  '+
                    CONVERT(NVARCHAR(50),BILLAMT,103)+'  '+ISNULL(CONVERT(NVARCHAR(50),EXPPDT,103),'(NULL)')+'  '+
                    ISNULL(REMARKS,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),BILLSL,103),'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  '+
                    ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+ISNULL(IPADDRESS,'(NULL)') FROM CNF_JOBBILL */
                    string logid = "UPDATE";
                    string tableid = "CNF_JOBBILL";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }


                dob.UpdateJobBillInfo(iob);

                gvDetails.EditIndex = -1;

                GridShow();

                TextBox txtExpense = (TextBox)gvDetails.FooterRow.FindControl("txtExpense");
                txtExpense.Focus();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Label lblBillDate = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblBillDate");
                Label lblSl = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSl");
                Label lblFwdDate = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblFwdDate");
                Label lblBillNo = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblBillNo");

                Label lblExpenseID = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblExpenseID");
                Label lblExpense = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblExpense");
                Label lblExpenseAmount = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblExpenseAmount");
                Label lblBillAmount = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblBillAmount");
                Label lblRemarks = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblRemarks");
                Label lblBillSl = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblBillSl");


                iob.CompID = txtCompanyID.Text;
                iob.JOBYY = Convert.ToInt64(txtJobYear.Text);
                iob.JOBNO = Convert.ToInt64(txtJobID.Text);
                iob.JOBTP = txtJobType.Text;
                iob.PARTYID = txtPartyID.Text;

                iob.BILLDT = DateTime.Parse(lblBillDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                iob.BILLNO = Convert.ToInt64(lblBillNo.Text);

                iob.EXPSL = Convert.ToInt64(lblSl.Text);

                iob.EXPID = lblExpenseID.Text;
                iob.EXPAMT = Convert.ToDecimal(lblExpenseAmount.Text);
                iob.BILLAMT = Convert.ToDecimal(lblBillAmount.Text);
                if (lblFwdDate.Text == "")
                    lblFwdDate.Text = "1900-01-01";
                iob.EXPPDT = DateTime.Parse(lblFwdDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                iob.REMARKS = lblRemarks.Text;
                if (lblBillSl.Text == "")
                    lblBillSl.Text = "0";
                iob.BILLSL = Convert.ToInt64(lblBillSl.Text);
                iob.InTime = DateTime.Now;
                iob.UpdateTime = DateTime.Now;
                iob.Userpc = CookiesData["PCName"].ToString();
                iob.IPAddress = CookiesData["ipAddress"].ToString();

                lblErrmsg.Visible = false;
                try
                {
                    // logdata add start //
                    string lotileng = HttpContext.Current.Session["Location"].ToString();
                    string ipAddress = HttpContext.Current.Session["IpAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT 'PROCESSDT : ' + ISNULL(CONVERT(NVARCHAR(50),PROCESSDT,103),'(NULL)')+' | '+'COMPID :
' + COMPID+' | '+'JOBYY : ' + CONVERT(NVARCHAR(50),JOBYY,103)+' | '+'JOBTP : ' + JOBTP+' | '+'JOBNO : ' + CONVERT(NVARCHAR(50),JOBNO,103)+' | '+'PARTYID :
' + ISNULL(PARTYID,'(NULL)')+' | '+'BILLDT : ' + ISNULL(CONVERT(NVARCHAR(50),BILLDT,103),'(NULL)')+' | '+'BILLNO :
' + ISNULL(CONVERT(NVARCHAR(50),BILLNO,103),'(NULL)')+' | '+'EXPSL : ' + ISNULL(CONVERT(NVARCHAR(50),EXPSL,103),'(NULL)')+' | '+'EXPID : ' + EXPID+' | '+'EXPAMT :
' + CONVERT(NVARCHAR(50),EXPAMT,103)+' | '+'BILLAMT : ' + CONVERT(NVARCHAR(50),BILLAMT,103)+' | '+'EXPPDT :
' + ISNULL(CONVERT(NVARCHAR(50),EXPPDT,103),'(NULL)')+' | '+'REMARKS : ' + ISNULL(REMARKS,'(NULL)')+' | '+'BILLSL :
' + ISNULL(CONVERT(NVARCHAR(50),BILLSL,103),'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'USERID :
' + ISNULL(USERID,'(NULL)')+' | '+'UPDATEUSERID : ' + ISNULL(UPDATEUSERID,'(NULL)')+' | '+'INTIME : ' + ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+' | '+'UPDATETIME :
' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + ISNULL(IPADDRESS,'(NULL)')+' | ' FROM CNF_JOBBILL
                    WHERE BILLDT='" + iob.BILLDT + "' and BILLNO='" + iob.BILLNO + "' and EXPSL='" + iob.EXPSL + "' and JOBYY='" + iob.JOBYY +
                    "' and JOBTP='" + iob.JOBTP + "' and JOBNO='" + iob.JOBNO + "' and EXPID='" + iob.EXPID + "'");
                    string logid = "DELETE";
                    string tableid = "CNF_JOBBILL";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }

                dob.RemoveJobBillInfo(iob);

                gvDetails.EditIndex = -1;

                GridShow();

                TextBox txtExpense = (TextBox)gvDetails.FooterRow.FindControl("txtExpense");
                txtExpense.Focus();


            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }
        protected void txtExpense_TextChanged(object sender, EventArgs e)
        {
            TextBox txtExpenseID = (TextBox)gvDetails.FooterRow.FindControl("txtExpenseID");
            TextBox txtExpense = (TextBox)gvDetails.FooterRow.FindControl("txtExpense");
            TextBox txtBillAmount = (TextBox)gvDetails.FooterRow.FindControl("txtBillAmount");
            TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");
            TextBox txtFwdDate = (TextBox)gvDetails.FooterRow.FindControl("txtFwdDate");

            if (txtExpense.Text == "")
            {
                lblErrMsgExist.Visible = true;
                lblErrMsgExist.Text = "Select Expense Name.";
                txtExpense.Focus();
            }
            else
            {
                lblErrMsgExist.Visible = false;

                string expnm = "";
                string expID = "";

                string searchPar = txtExpense.Text;

                int splitter = searchPar.IndexOf("|");
                if (splitter != -1)
                {
                    string[] lineSplit = searchPar.Split('|');

                    expnm = lineSplit[0];
                    expID = lineSplit[1];


                    txtExpense.Text = expnm.Trim();
                    txtExpenseID.Text = expID.Trim();
                    Label Amount = new Label();
                    dbFunctions.txtAdd(@"SELECT dbo.CNF_PARTYRT.REMARKS FROM dbo.CNF_PARTYRT INNER JOIN dbo.CNF_JOB ON dbo.CNF_PARTYRT.REGID = dbo.CNF_JOB.REGID AND dbo.CNF_PARTYRT.JOBQUALITY = dbo.CNF_JOB.JOBQUALITY
                    WHERE     (dbo.CNF_PARTYRT.PARTYID = '" + txtPartyID.Text + "') AND (dbo.CNF_PARTYRT.EXPID = '" + expID + "') AND (dbo.CNF_JOB.JOBNO = " + txtJobID.Text + ") AND (dbo.CNF_JOB.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOB.JOBYY = " + txtJobYear.Text + ")", txtRemarks);

                    dbFunctions.lblAdd(@"SELECT DISTINCT dbo.CNF_PARTYRT.RATE
                    FROM  dbo.CNF_JOB INNER JOIN
                    dbo.CNF_PARTYRT ON dbo.CNF_JOB.REGID = dbo.CNF_PARTYRT.REGID AND dbo.CNF_JOB.JOBQUALITY = dbo.CNF_PARTYRT.JOBQUALITY AND 
                    dbo.CNF_JOB.PARTYID = dbo.CNF_PARTYRT.PARTYID
                    WHERE     (SUBSTRING(dbo.CNF_PARTYRT.EXPID, 0, 3) = 'I3') AND (dbo.CNF_JOB.PARTYID = '" + txtPartyID.Text + "') AND (dbo.CNF_PARTYRT.EXPID = '" + expID + "') AND (dbo.CNF_JOB.JOBNO = " + txtJobID.Text + ") AND " +
                      "(dbo.CNF_JOB.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOB.JOBYY = '" + txtJobYear.Text + "')", Amount);
                    string a = expID.Substring(0, 2);
                    if (a == "I3")
                    {
                        txtBillAmount.Text = Amount.Text;
                        if (txtRemarks.Text == "")
                            txtRemarks.Focus();
                        else
                            txtFwdDate.Focus();
                    }
                    else
                    {
                        Label rtTp = new Label();
                        Label billamt = new Label();
                        Label parJob = new Label();
                        //                        dbFunctions.lblAdd(@"SELECT   CNF_PARTYRT.RATETP FROM   CNF_PARTYRT INNER JOIN  
                        //                        CNF_JOBBILL ON CNF_JOBBILL.PARTYID = CNF_PARTYRT.PARTYID WHERE PARTYID='" + txtPartyID.Text + "' AND EXPID='" + expID + "'", rtTp);
                        dbFunctions.lblAdd(@"SELECT     dbo.CNF_PARTYRT.RATETP
                        FROM    dbo.CNF_PARTYRT INNER JOIN
                      dbo.CNF_JOB ON dbo.CNF_PARTYRT.REGID = dbo.CNF_JOB.REGID AND dbo.CNF_PARTYRT.PARTYID = dbo.CNF_JOB.PARTYID AND 
                      dbo.CNF_PARTYRT.JOBQUALITY = dbo.CNF_JOB.JOBQUALITY
                       WHERE (dbo.CNF_PARTYRT.PARTYID = '" + txtPartyID.Text + "') AND (dbo.CNF_PARTYRT.EXPID = '" + expID + "') AND (dbo.CNF_JOB.JOBNO = " + txtJobID.Text + ") AND  " +
                      "(dbo.CNF_JOB.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOB.JOBYY = " + txtJobYear.Text + ")", rtTp);

                        if (rtTp.Text == "FIXED")
                        {
                            dbFunctions.lblAdd(@"SELECT   dbo.CNF_PARTYRT.RATE  FROM    dbo.CNF_PARTYRT INNER JOIN
                      dbo.CNF_JOB ON dbo.CNF_PARTYRT.REGID = dbo.CNF_JOB.REGID AND dbo.CNF_PARTYRT.PARTYID = dbo.CNF_JOB.PARTYID AND 
                      dbo.CNF_PARTYRT.JOBQUALITY = dbo.CNF_JOB.JOBQUALITY
                       WHERE (dbo.CNF_PARTYRT.PARTYID = '" + txtPartyID.Text + "') AND (dbo.CNF_PARTYRT.EXPID = '" + expID + "') AND (dbo.CNF_JOB.JOBNO = " + txtJobID.Text + ") AND  " +
                      "(dbo.CNF_JOB.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOB.JOBYY = " + txtJobYear.Text + ")", billamt);
                            if (billamt.Text == "")
                                billamt.Text = "0";
                            txtBillAmount.Text = billamt.Text;
                            if (txtRemarks.Text == "")
                                txtRemarks.Focus();
                            else
                                txtFwdDate.Focus();
                        }
                        else if (rtTp.Text == "KGS")
                        {
                            dbFunctions.lblAdd(@"SELECT CNF_PARTYRT.RATE FROM    dbo.CNF_PARTYRT INNER JOIN
                      dbo.CNF_JOB ON dbo.CNF_PARTYRT.REGID = dbo.CNF_JOB.REGID AND dbo.CNF_PARTYRT.PARTYID = dbo.CNF_JOB.PARTYID AND 
                      dbo.CNF_PARTYRT.JOBQUALITY = dbo.CNF_JOB.JOBQUALITY
                       WHERE (dbo.CNF_PARTYRT.PARTYID = '" + txtPartyID.Text + "') AND (dbo.CNF_PARTYRT.EXPID = '" + expID + "') AND (dbo.CNF_JOB.JOBNO = " + txtJobID.Text + ") AND  " +
                      "(dbo.CNF_JOB.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOB.JOBYY = " + txtJobYear.Text + ")", billamt);
                            if (billamt.Text == "")
                                billamt.Text = "0";
                            dbFunctions.lblAdd(@"SELECT WTNET FROM CNF_JOB WHERE JOBNO=" + txtJobID.Text + " AND JOBTP='" + txtJobType.Text + "' AND PARTYID='" + txtPartyID.Text + "'", parJob);
                            if (parJob.Text == "")
                                parJob.Text = "0";
                            txtBillAmount.Text = (Convert.ToDecimal(billamt.Text) * Convert.ToDecimal(parJob.Text)).ToString("F");
                            if (txtRemarks.Text == "")
                                txtRemarks.Focus();
                            else
                                txtFwdDate.Focus();
                        }
                        else if (rtTp.Text == "PKTS")
                        {
                            dbFunctions.lblAdd(@"SELECT CNF_PARTYRT.RATE FROM    dbo.CNF_PARTYRT INNER JOIN
                      dbo.CNF_JOB ON dbo.CNF_PARTYRT.REGID = dbo.CNF_JOB.REGID AND dbo.CNF_PARTYRT.PARTYID = dbo.CNF_JOB.PARTYID AND 
                      dbo.CNF_PARTYRT.JOBQUALITY = dbo.CNF_JOB.JOBQUALITY
                       WHERE (dbo.CNF_PARTYRT.PARTYID = '" + txtPartyID.Text + "') AND (dbo.CNF_PARTYRT.EXPID = '" + expID + "') AND (dbo.CNF_JOB.JOBNO = " + txtJobID.Text + ") AND  " +
                      "(dbo.CNF_JOB.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOB.JOBYY = " + txtJobYear.Text + ")", billamt);
                            if (billamt.Text == "")
                                billamt.Text = "0";
                            dbFunctions.lblAdd(@"SELECT PKGS FROM CNF_JOB WHERE JOBNO=" + txtJobID.Text + " AND JOBTP='" + txtJobType.Text + "' AND PARTYID='" + txtPartyID.Text + "'", parJob);
                            if (parJob.Text == "")
                                parJob.Text = "0";
                            txtBillAmount.Text = (Convert.ToDecimal(billamt.Text) * Convert.ToDecimal(parJob.Text)).ToString("F");
                            if (txtRemarks.Text == "")
                                txtRemarks.Focus();
                            else
                                txtFwdDate.Focus();
                        }
                        else if (rtTp.Text == "CBM")
                        {
                            dbFunctions.lblAdd(@"SELECT CNF_PARTYRT.RATE FROM    dbo.CNF_PARTYRT INNER JOIN
                      dbo.CNF_JOB ON dbo.CNF_PARTYRT.REGID = dbo.CNF_JOB.REGID AND dbo.CNF_PARTYRT.PARTYID = dbo.CNF_JOB.PARTYID AND 
                      dbo.CNF_PARTYRT.JOBQUALITY = dbo.CNF_JOB.JOBQUALITY
                       WHERE (dbo.CNF_PARTYRT.PARTYID = '" + txtPartyID.Text + "') AND (dbo.CNF_PARTYRT.EXPID = '" + expID + "') AND (dbo.CNF_JOB.JOBNO = " + txtJobID.Text + ") AND  " +
                      "(dbo.CNF_JOB.JOBTP = '" + txtJobType.Text + "') AND (dbo.CNF_JOB.JOBYY = " + txtJobYear.Text + ")", billamt);
                            if (billamt.Text == "")
                                billamt.Text = "0";
                            dbFunctions.lblAdd(@"SELECT CBM FROM CNF_JOB WHERE JOBNO=" + txtJobID.Text + " AND JOBTP='" + txtJobType.Text + "' AND PARTYID='" + txtPartyID.Text + "'", parJob);
                            if (parJob.Text == "")
                                parJob.Text = "0";
                            txtBillAmount.Text = (Convert.ToDecimal(billamt.Text) * Convert.ToDecimal(parJob.Text)).ToString("F");
                            if (txtRemarks.Text == "")
                                txtRemarks.Focus();
                            else
                                txtFwdDate.Focus();
                        }
                        else
                        {
                            txtBillAmount.Focus();
                        }
                    }
                    if (txtBillAmount.Text == "")
                        txtBillAmount.Focus();
                }
                else
                {
                    txtExpenseID.Text = "";
                    txtExpense.Text = "";
                    lblErrmsg.Visible = true;
                    lblErrmsg.Text = "Select expense.";
                    txtExpense.Focus();
                }
            }
        }
        protected void txtFwdDateEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtBillSlEdit = (TextBox)row.FindControl("txtBillSlEdit");
            txtBillSlEdit.Focus();
        }

        protected void txtFwdDate_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtBillSl = (TextBox)row.FindControl("txtBillSl");
            txtBillSl.Focus();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (txtJobID.Text == "")
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Select job no.";
                txtJobID.Focus();
            }
            else
            {
                Session["jobno"] = null;
                Session["jobtp"] = null;
                Session["jobyear"] = null;
                Session["compid"] = null;
                Session["partyid"] = null;
                Session["jobdt"] = null;

                Session["jobno"] = txtJobID.Text;
                Session["jobtp"] = txtJobType.Text;
                Session["jobyear"] = txtJobYear.Text;
                Session["compid"] = txtCompanyID.Text;
                Session["partyid"] = txtPartyID.Text;
                Session["jobdt"] = txtReceiveDate.Text;

                 ScriptManager.RegisterStartupScript(this,this.GetType(), "OpenWindow", "window.open('../Report/Report/rpt-bill-details.aspx','_newtab');", true);



            }
        }





        protected void txtBillSlEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            ImageButton imgbtnUpdate = (ImageButton)row.FindControl("imgbtnUpdate");
            imgbtnUpdate.Focus();
        }

        protected void txtBillSl_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            ImageButton imgbtnAdd = (ImageButton)row.FindControl("imgbtnAdd");
            imgbtnAdd.Focus();
        }

        protected void btnBillNSE_OnClick(object sender, EventArgs e)
        {
            if (txtJobID.Text == "")
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Select job no.";
                txtJobID.Focus();
            }
            else
            {
                Session["jobno"] = null;
                Session["jobtp"] = null;
                Session["jobyear"] = null;
                Session["compid"] = null;
                Session["partyid"] = null;
                Session["jobdt"] = null;

                Session["jobno"] = txtJobID.Text;
                Session["jobtp"] = txtJobType.Text;
                Session["jobyear"] = txtJobYear.Text;
                Session["compid"] = txtCompanyID.Text;
                Session["partyid"] = txtPartyID.Text;
                Session["jobdt"] = txtReceiveDate.Text;


                 ScriptManager.RegisterStartupScript(this,this.GetType(), "OpenWindow", "window.open('../Report/Report/rpt-bill-details-NSE.aspx','_newtab');", true);
            }
        }

        protected void btnBillRC_OnClick(object sender, EventArgs e)
        {
            if (txtJobID.Text == "")
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Select job no.";
                txtJobID.Focus();
            }
            else
            {
                Session["jobno"] = null;
                Session["jobtp"] = null;
                Session["jobyear"] = null;
                Session["compid"] = null;
                Session["partyid"] = null;
                Session["jobdt"] = null;

                Session["jobno"] = txtJobID.Text;
                Session["jobtp"] = txtJobType.Text;
                Session["jobyear"] = txtJobYear.Text;
                Session["compid"] = txtCompanyID.Text;
                Session["partyid"] = txtPartyID.Text;
                Session["jobdt"] = txtReceiveDate.Text;


                 ScriptManager.RegisterStartupScript(this,this.GetType(), "OpenWindow", "window.open('../Report/Report/rpt-bill-details-rc.aspx','_newtab');", true);
            }
        }
    }
}