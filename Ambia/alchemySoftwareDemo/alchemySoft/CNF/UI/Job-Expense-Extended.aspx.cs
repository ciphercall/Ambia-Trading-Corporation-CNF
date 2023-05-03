using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft.CNF.DataAccess;
using alchemySoft.CNF.Interface;
using alchemySoft.LogIn;

namespace alchemySoft.CNF.UI
{
    public partial class Job_Expense_Extended : System.Web.UI.Page
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
                DateTime dateTime = Convert.ToDateTime("2018 JUN 26");
                string formLink = "/CNF/UI/Job-Expense-Extended.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (dbFunctions.timezone(DateTime.Now) >= dateTime)
                    Response.Redirect("~/Servererror.html");
                else if (permission)
                {
                    if (!IsPostBack)
                    {
                        StartUp();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        public void StartUp()
        {
            DateTime td = dbFunctions.timezone(DateTime.Now);
            txtExDT.Text = td.ToString("dd/MM/yyyy");
            string mon = td.ToString("MMM").ToUpper();
            string year = td.ToString("yy");
            lblMY.Text = "";
            lblMY.Text = mon + "-" + year;
            check_Invoice_No();
            GridShow();
            txtExpenseNM.Focus();
        }

        public void check_Invoice_No()
        {
            lblInvoiceNo.Text = "";
            dbFunctions.lblAdd("SELECT MAX(TRANSNO) AS TRANSNO FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "'", lblInvoiceNo);
            if (lblInvoiceNo.Text == "")
            {
                txtNo.Text = "1";
            }
            else
            {
                Int64 trns, ftrns = 0;
                trns = Convert.ToInt64(lblInvoiceNo.Text);
                ftrns = trns + 1;
                txtNo.Text = ftrns.ToString();
            }
        }
        private string CkeckTransNO(string TransMY, Int64 TransNO)
        {
            string result = "false";
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            conn.Open();
            string script = "SELECT * FROM CNF_JOBEXPMST WHERE TRANSMY ='" + TransMY + "' AND TRANSNO =" + TransNO + "";
            SqlCommand cmd = new SqlCommand(script, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count < 1)
            {
                result = "true";
            }
            return result;
        }
        protected void txtExDT_TextChanged(object sender, EventArgs e)
        {
            if (txtExDT.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select a date.";
                dbFunctions.popupAlert(Page, "Select a date.", "w");
                txtExDT.Focus();
            }
            else
            {
                // lblError.Visible = false;

                DateTime exDT = DateTime.Parse(txtExDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string mon = exDT.ToString("MMM").ToUpper();
                string year = exDT.ToString("yy");
                lblMY.Text = "";
                lblMY.Text = mon + "-" + year;
                if (btnEdit.Text == "Edit")
                {
                    check_Invoice_No();
                }
                else
                {
                    string uTp = CookiesData["USERTYPE"].ToString();
                    string brCD = CookiesData["COMPANYID"].ToString();
                    if (uTp == "COMPADMIN")
                    {
                        dbFunctions.dropDown_Bind(ddlInvoice, "", "SELECT", "SELECT TRANSNO AS NM FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "'");
                    }
                    else
                    {
                        dbFunctions.dropDown_Bind(ddlInvoice, "", "SELECT", "SELECT DISTINCT TRANSNO FROM CNF_JOBEXP WHERE TRANSMY ='" + lblMY.Text + "'");
                    }
                    ddlInvoice.Focus();
                }
            }
        }


        protected void txtExpenseNM_TextChanged(object sender, EventArgs e)
        {
            if (txtExpenseNM.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select expense";
                dbFunctions.popupAlert(Page, "Select expense.", "w");
                txtExpenseNM.Focus();
            }
            else
            {
                //lblError.Visible = false;
                txtExCD.Text = "";
                dbFunctions.txtAdd("SELECT ACCOUNTCD FROM GL_ACCHART WHERE ACCOUNTNM ='" + txtExpenseNM.Text + "' AND STATUSCD = 'P'", txtExCD);
                txtRemarks.Focus();
            }
        }

        protected void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            TextBox txtJobNo = (TextBox)gvDetails.FooterRow.FindControl("txtJobNo");
            txtJobNo.Focus();
        }

        private void GridShow()
        {

            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string TransNO = "";
            if (btnEdit.Text == "Edit")
                TransNO = txtNo.Text;
            else
                TransNO = ddlInvoice.Text;
            if (TransNO == "--SELECT--")
            {
                TransNO = "0";
            }
            conn.Open();
            SqlCommand cmdd = new SqlCommand(@"SELECT     CNF_JOBEXP.SLNO, CNF_JOBEXP.JOBNO, CNF_JOBEXP.JOBYY, CNF_JOBEXP.JOBTP, CNF_JOBEXP.EXPID, CNF_EXPENSE.EXPNM, CNF_JOBEXP.EXPAMT, CNF_JOBEXP.REMARKS, 
GL_ACCHART.ACCOUNTNM, ASL_BRANCH.BRANCHID,CNF_JOB.REFTP
FROM CNF_JOBEXP INNER JOIN
CNF_EXPENSE ON CNF_JOBEXP.EXPID = CNF_EXPENSE.EXPID INNER JOIN
CNF_JOB ON CNF_JOBEXP.JOBNO = CNF_JOB.JOBNO AND CNF_JOBEXP.JOBTP = CNF_JOB.JOBTP AND CNF_JOBEXP.JOBYY = CNF_JOB.JOBYY INNER JOIN
GL_ACCHART ON CNF_JOB.PARTYID = GL_ACCHART.ACCOUNTCD  INNER JOIN
ASL_BRANCH ON CNF_JOBEXP.COMPID = ASL_BRANCH.BRANCHCD 
WHERE TRANSMY='" + lblMY.Text + "' AND TRANSNO='" + TransNO + "' ORDER BY CNF_JOBEXP.SLNO", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmdd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();

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
                        Label Per = (Label)grid.Cells[6].FindControl("lblAmount");
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
                        txtTotalAmount.Text = a.ToString();
                    }
                    a += totAmt;
                }
                TextBox txtJobNo = (TextBox)gvDetails.FooterRow.FindControl("txtJobNo");
                txtJobNo.Focus();
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
                TextBox txtJobNo = (TextBox)gvDetails.FooterRow.FindControl("txtJobNo");
                txtJobNo.Focus();
            }
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (btnEdit.Text == "Edit")
                {
                    if (txtNo.Text == "")
                        iob.InvoiceNO = 0;
                    else
                        iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                }
                else
                {
                    if (ddlInvoice.Text == "--SELECT--" || ddlInvoice.Text == "")
                    {
                        iob.InvoiceNO = 0;
                    }
                    else
                        iob.InvoiceNO = Convert.ToInt64(ddlInvoice.Text);
                }

                dbFunctions.lblAdd("SELECT MAX(SLNO) AS SLNO FROM CNF_JOBEXP WHERE TRANSMY ='" + lblMY.Text + "' AND TRANSNO ='" + iob.InvoiceNO + "'", lblExsl);

                int sl, fSl = 0;

                if (lblExsl.Text == "")
                {
                    e.Row.Cells[0].Text = "1";
                }
                else
                {
                    sl = Convert.ToInt16(lblExsl.Text);
                    fSl = sl + 1;

                    e.Row.Cells[0].Text = fSl.ToString();
                }
            }
        }

        protected void txtParticulars_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtParticulars = (TextBox)row.FindControl("txtParticulars");
            TextBox txtCode = (TextBox)row.FindControl("txtCode");
            TextBox txtAmount = (TextBox)row.FindControl("txtAmount");

            if (txtParticulars.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select particulars.";
                dbFunctions.popupAlert(Page, "Select particulars.", "w");
                txtParticulars.Focus();
            }
            else
            {
                txtCode.Text = "";
                dbFunctions.txtAdd("SELECT EXPID FROM CNF_EXPENSE WHERE EXPNM ='" + txtParticulars.Text + "'", txtCode);
                if (txtCode.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select particulars.";
                    dbFunctions.popupAlert(Page, "Select particulars.", "w");
                    txtParticulars.Text = "";
                    txtParticulars.Focus();
                }
                else
                    txtAmount.Focus();
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (CookiesData["USERNAME"] == null)
                Response.Redirect("~/Login/UI/SignIn.aspx");
            else
            {
                try
                {
                    string serial = gvDetails.FooterRow.Cells[0].Text;
                    TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
                    TextBox txtCode = (TextBox)gvDetails.FooterRow.FindControl("txtCode");
                    TextBox txtAmount = (TextBox)gvDetails.FooterRow.FindControl("txtAmount");
                    TextBox txtRemarksG = (TextBox)gvDetails.FooterRow.FindControl("txtRemarksG");
                    TextBox txtJobNo = (TextBox)gvDetails.FooterRow.FindControl("txtJobNo");
                    Label txtJobYear = (Label)gvDetails.FooterRow.FindControl("txtJobYear");
                    Label txtJobTP = (Label)gvDetails.FooterRow.FindControl("txtJobTP");
                    if (lblMY.Text == "")
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "Select date.";
                        dbFunctions.popupAlert(Page, "Select date.", "w");
                        txtExDT.Focus();
                    }
                    else if (txtExCD.Text == "")
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "Select expense.";
                        dbFunctions.popupAlert(Page, "Select expense.", "w");
                        txtExpenseNM.Focus();
                    }
                    else if (txtJobNo.Text == "")
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "Select job no.";
                        dbFunctions.popupAlert(Page, "Select job no.", "w");
                        txtJobNo.Focus();
                    }
                    else if (txtCode.Text == "")
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "Select particulars.";
                        dbFunctions.popupAlert(Page, "Select particulars.", "w");
                        txtParticulars.Focus();
                    }
                    else if (txtAmount.Text == "" || txtAmount.Text == ".00" || txtAmount.Text == "0")
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "Type amount.";
                        dbFunctions.popupAlert(Page, "Type amount.", "w");
                        txtAmount.Focus();
                    }
                    else
                    {
                        //lblError.Visible = false;
                        iob.ExDT = DateTime.Parse(txtExDT.Text, dateformat,
                            System.Globalization.DateTimeStyles.AssumeLocal);
                        iob.ExMY = lblMY.Text;
                        iob.Sl = Convert.ToInt64(serial);
                        //check_Invoice_No();
                        if (btnEdit.Text == "Edit")
                        {
                            dbFunctions.lblAdd(
                                "SELECT COUNT(*) FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "' AND TRANSNO='" +
                                txtNo.Text + "'", lbltransNomst);

                            if (Convert.ToInt16(lbltransNomst.Text) == 0)
                            {
                                iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                            }
                            else
                            {
                                dbFunctions.lblAdd(
                                    "SELECT COUNT(*) FROM CNF_JOBEXP WHERE TRANSMY ='" + lblMY.Text + "' AND TRANSNO='" +
                                    txtNo.Text + "'", lbltransSL);
                                if (Convert.ToInt16(lbltransSL.Text) == 0)
                                {
                                    check_Invoice_No();
                                    iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                                }
                                else
                                {
                                    if (iob.Sl == 1)
                                    {
                                        check_Invoice_No();
                                        iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                                    }
                                    else
                                    {
                                        iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                                    }


                                }

                            }
                        }
                        else
                        {
                            iob.InvoiceNO = Convert.ToInt64(ddlInvoice.Text);
                        }



                        iob.JobNo = Convert.ToInt64(txtJobNo.Text);
                        iob.JobYear = Convert.ToInt16(txtJobYear.Text);
                        iob.JobTP = txtJobTP.Text;
                        iob.CompID = lblCmpID.Text;
                        iob.ExpenseCD = txtExCD.Text;
                        iob.RemarksTOP = txtRemarks.Text;

                        iob.ExpensesID = txtCode.Text;
                        iob.Amount = Convert.ToDecimal(txtAmount.Text);
                        iob.RemarksBOT = txtRemarksG.Text;
                        iob.Ipaddress = CookiesData["ipAddress"].ToString();
                        iob.Userpc = CookiesData["PCName"].ToString();
                        iob.InTM = dbFunctions.timezone(DateTime.Now);
                        iob.UserNM = CookiesData["USERNAME"].ToString();
                        iob.UserID = CookiesData["USERID"].ToString();
                        if (e.CommandName.Equals("SaveCon"))
                        {
                            string Check = CkeckTransNO(lblMY.Text, iob.InvoiceNO);
                            if (Check == "true")
                            {
                                check_Invoice_No();
                                iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                                dob.INSERT_CNF_JOBEXP_EXTENDEDMST(iob);
                                dob.save_cnf_job_expense_Extended(iob);
                            }
                            else
                                dob.save_cnf_job_expense_Extended(iob);
                            GridShow();
                            txtJobNo.Focus();
                        }

                        else if (e.CommandName.Equals("Complete"))
                        {

                            string Check = CkeckTransNO(lblMY.Text, iob.InvoiceNO);
                            if (Check == "true")
                            {
                                check_Invoice_No();
                                iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                                dob.INSERT_CNF_JOBEXP_EXTENDEDMST(iob);
                                dob.save_cnf_job_expense_Extended(iob);
                            }
                            else
                                dob.save_cnf_job_expense_Extended(iob);
                            GridShow();
                            Refresh();
                            txtExpenseNM.Focus();
                        }

                        else if (e.CommandName.Equals("SavePrint"))
                        {

                            RefreshSession();
                            Session["Jobdate"] = txtExDT.Text;
                            Session["transmy"] = iob.ExMY.ToString();
                            if (btnEdit.Text == "Edit")
                                Session["InvoiceNo"] = iob.InvoiceNO;
                            else
                                Session["InvoiceNo"] = iob.InvoiceNO;
                            Session["JobNo"] = iob.JobNo;
                            Session["Jobyear"] = iob.JobYear;
                            Session["JobType"] = iob.JobTP;
                            Session["expenseCD"] = iob.ExpenseCD;
                            Session["remarksT"] = iob.RemarksTOP;


                            string Check = CkeckTransNO(lblMY.Text, iob.InvoiceNO);
                            if (Check == "true")
                            {
                                check_Invoice_No();
                                iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                                dob.INSERT_CNF_JOBEXP_EXTENDEDMST(iob);
                                dob.save_cnf_job_expense_Extended(iob);
                            }
                            else
                                dob.save_cnf_job_expense_Extended(iob);
                            Refresh();
                            txtExpenseNM.Focus();
                             ScriptManager.RegisterStartupScript(this,
                                this.GetType(), "OpenWindow",
                                "window.open('../report/vis-rep/RptExpense_VR.aspx','_newtab');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
            }
        }

        public void RefreshSession()
        {
            Session["Jobdate"] = "";
            Session["transmy"] = "";
            Session["InvoiceNo"] = "";
            Session["JobNo"] = "";
            Session["Jobyear"] = "";
            Session["JobType"] = "";
            Session["Compid"] = "";
            Session["expenseCD"] = "";
            Session["remarksT"] = "";
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

            TextBox txtJobNoEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtJobNoEdit");
            txtJobNoEdit.Focus();
        }



        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (CookiesData["USERNAME"] == null)
            {
                Response.Redirect("~/Login/UI/signin.aspx");
            }
            else
            {
                Label lblSlEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSlEdit");
                TextBox txtParticularsEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtParticularsEdit");
                TextBox txtCodeEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtCodeEdit");
                TextBox txtJobNoEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtJobNoEdit");
                Label txtJobYearEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("txtJobYearEdit");
                Label txtJobTPEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("txtJobTPEdit");
                TextBox txtAmountEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtAmountEdit");
                TextBox txtRemarksGEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarksGEdit");

                if (lblMY.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select date.";
                    dbFunctions.popupAlert(Page, "Select date.", "w");
                    txtExDT.Focus();
                }
                else if (txtExCD.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select expense.";
                    dbFunctions.popupAlert(Page, "Select expense.", "w");
                    txtExpenseNM.Focus();
                }
                else if (txtJobNoEdit.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select job no.";
                    dbFunctions.popupAlert(Page, "Select job no.", "w");
                    txtJobNoEdit.Focus();
                }
                else if (txtCodeEdit.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select particulars.";
                    dbFunctions.popupAlert(Page, "Select particulars.", "w");
                    txtParticularsEdit.Focus();
                }
                else if (txtAmountEdit.Text == "" || txtAmountEdit.Text == ".00" || txtAmountEdit.Text == "0")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Type amount.";
                    dbFunctions.popupAlert(Page, "Type amount.", "w");
                    txtAmountEdit.Focus();
                }
                else
                {
                    iob.ExDT = DateTime.Parse(txtExDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.ExMY = lblMY.Text;
                    if (btnEdit.Text == "Edit")
                        iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                    else
                        iob.InvoiceNO = Convert.ToInt64(ddlInvoice.Text);
                    iob.JobNo = Convert.ToInt64(txtJobNoEdit.Text);
                    iob.JobYear = Convert.ToInt16(txtJobYearEdit.Text);
                    iob.JobTP = txtJobTPEdit.Text;
                    iob.CompID = lblCmpID.Text;
                    iob.ExpenseCD = txtExCD.Text;
                    iob.RemarksTOP = txtRemarks.Text;
                    iob.Sl = Convert.ToInt64(lblSlEdit.Text);
                    iob.ExpensesID = txtCodeEdit.Text;
                    iob.Amount = Convert.ToDecimal(txtAmountEdit.Text);
                    iob.RemarksBOT = txtRemarksGEdit.Text;

                    iob.Ipaddress = CookiesData["ipAddress"].ToString();
                    iob.Userpc = CookiesData["PCName"].ToString();
                    iob.UpdateUser = CookiesData["USERNAME"].ToString();
                    iob.UpTM = dbFunctions.timezone(DateTime.Now);

                    //try
                    //{
                    //    // logdata add start //
                    //    string lotileng = HttpContext.Current.Session["Location"].ToString();
                    //    string ipAddress = HttpContext.Current.Session["IpAddress"].ToString();
                    //    string logdata = dbFunctions.StringData(@"SELECT CONVERT(NVARCHAR(50),TRANSDT,103)+'  '+TRANSMY+'  '+
                    //    CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+ISNULL(COMPID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),JOBYY,103),'(NULL)')+'  '+
                    //    ISNULL(JOBTP,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),JOBNO,103),'(NULL)')+'  '+EXPCD+'  '+ISNULL(REMARKS,'(NULL)')+'  '+
                    //    ISNULL(USERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+'  '+
                    //    CONVERT(NVARCHAR(50),INTIME,103)+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+IPADDRESS FROM CNF_JOBEXPMST 
                    //    WHERE TRANSMY='" + iob.ExMY + "' AND TRANSNO='" + iob.InvoiceNO + "'");
                    //    string logid = "UPDATE";
                    //    string tableid = "CNF_JOBEXPMST";
                    //    dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                    //    // logdata add end //
                    //}
                    //catch (Exception ex)
                    //{
                    //    //ignore
                    //}

                    try
                    {
                        // logdata add start //
                        string lotileng = CookiesData["LOCATION"].ToString();
                        string ipAddress = CookiesData["ipAddress"].ToString();
                        string logdata = dbFunctions.StringData(@"SELECT 'TRANSDT : ' + CONVERT(NVARCHAR(50),TRANSDT,103)+' | '+'TRANSMY : ' + TRANSMY+' | '+'TRANSNO :
' + CONVERT(NVARCHAR(50),TRANSNO,103)+' | '+'COMPID : ' + COMPID+' | '+'JOBYY : ' + CONVERT(NVARCHAR(50),JOBYY,103)+' | '+'JOBTP : ' + JOBTP+' | '+'JOBNO : 
' + CONVERT(NVARCHAR(50),JOBNO,103)+' | '+'EXPCD : ' + EXPCD+' | '+'SLNO : ' + CONVERT(NVARCHAR(50),SLNO,103)+' | '+'EXPID : ' + EXPID+' | '+'EXPAMT :
' + CONVERT(NVARCHAR(50),EXPAMT,103)+' | '+'REMARKS : ' + ISNULL(REMARKS,'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'USERID :
' + ISNULL(USERID,'(NULL)')+' | '+'UPDATEUSERID : ' + ISNULL(UPDATEUSERID,'(NULL)')+' | '+'ACTDTI : ' + ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+' | '+'INTIME :
' + CONVERT(NVARCHAR(50),INTIME,103)+' | '+'UPDATETIME : ' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + IPADDRESS+' | ' FROM CNF_JOBEXP
 WHERE TRANSMY='" + iob.ExMY + "' AND TRANSNO='" + iob.InvoiceNO + "' AND SLNO='" + iob.Sl + "'");

                        /*SELECT CONVERT(NVARCHAR(50),TRANSDT,103)+'  '+TRANSMY+'  '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+
                        COMPID+'  '+CONVERT(NVARCHAR(50),JOBYY,103)+'  '+JOBTP+'  '+CONVERT(NVARCHAR(50),JOBNO,103)+'  '+EXPCD+'  '+
                        CONVERT(NVARCHAR(50),SLNO,103)+'  '+EXPID+'  '+CONVERT(NVARCHAR(50),EXPAMT,103)+'  '+ISNULL(REMARKS,'(NULL)')+'  '+
                        ISNULL(USERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+
                        ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+'  '+CONVERT(NVARCHAR(50),INTIME,103)+'  '+
                        ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+IPADDRESS FROM CNF_JOBEXP */
                        string logid = "UPDATE";
                        string tableid = "CNF_JOBEXP";
                        LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                        // logdata add end //
                    }
                    catch (Exception ex)
                    {
                        //ignore
                    }

                    //dob.UPDATE_CNF_JOBEXP_EXTENDED_MST(iob);
                    dob.UPDATE_CNF_JOBEXP_EXTENDED(iob);




                    gvDetails.EditIndex = -1;
                    GridShow();

                    TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
                    txtParticulars.Focus();
                }
            }
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (CookiesData["USERNAME"] == null)
            {
                Response.Redirect("~/Login/UI/signin.aspx");
            }
            else
            {
                Label lblSL = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSL");

                if (lblMY.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select date.";
                    dbFunctions.popupAlert(Page, "Select date.", "w");
                    txtExDT.Focus();
                }
                else if (txtExCD.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select expense.";
                    dbFunctions.popupAlert(Page, "Select expense.", "w");
                    txtExCD.Focus();
                }
                else
                {
                    iob.InTM = DateTime.Now;
                    iob.ExDT = DateTime.Parse(txtExDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.ExMY = lblMY.Text;
                    if (btnEdit.Text == "Edit")
                        iob.InvoiceNO = Convert.ToInt64(txtNo.Text);
                    else
                        iob.InvoiceNO = Convert.ToInt64(ddlInvoice.Text);
                    iob.ExpenseCD = txtExCD.Text;
                    iob.Sl = Convert.ToInt64(lblSL.Text);

                    SqlConnection conn = new SqlConnection(dbFunctions.Connection);
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand("SELECT * FROM CNF_JOBEXP where TRANSMY='" + iob.ExMY + "' AND TRANSNO = " + iob.InvoiceNO + "", conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1);
                    conn.Close();
                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        try
                        {
                            // logdata add start //
                            string lotileng = CookiesData["LOCATION"].ToString();
                            string ipAddress = CookiesData["ipAddress"].ToString();
                            string logdata = dbFunctions.StringData(@"SELECT 'TRANSDT : ' + CONVERT(NVARCHAR(50),TRANSDT,103)+' | '+'TRANSMY : ' + TRANSMY+' | '+'TRANSNO :
' + CONVERT(NVARCHAR(50),TRANSNO,103)+' | '+'COMPID : ' + COMPID+' | '+'JOBYY : ' + CONVERT(NVARCHAR(50),JOBYY,103)+' | '+'JOBTP : ' + JOBTP+' | '+'JOBNO : 
' + CONVERT(NVARCHAR(50),JOBNO,103)+' | '+'EXPCD : ' + EXPCD+' | '+'SLNO : ' + CONVERT(NVARCHAR(50),SLNO,103)+' | '+'EXPID : ' + EXPID+' | '+'EXPAMT :
' + CONVERT(NVARCHAR(50),EXPAMT,103)+' | '+'REMARKS : ' + ISNULL(REMARKS,'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'USERID :
' + ISNULL(USERID,'(NULL)')+' | '+'UPDATEUSERID : ' + ISNULL(UPDATEUSERID,'(NULL)')+' | '+'ACTDTI : ' + ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+' | '+'INTIME :
' + CONVERT(NVARCHAR(50),INTIME,103)+' | '+'UPDATETIME : ' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + IPADDRESS+' | ' FROM CNF_JOBEXP
                        WHERE TRANSMY='" + iob.ExMY + "' AND TRANSNO='" + iob.InvoiceNO + "' AND SLNO='" + iob.Sl + "'");
                            string logid = "DELETE";
                            string tableid = "CNF_JOBEXP";
                            LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                            // logdata add end //
                        }
                        catch (Exception ex)
                        {
                            //ignore
                        }
                        dob.delete_cnf_job_expense(iob);
                        GridShow();
                    }
                    else
                    {
                        try
                        {
                            // logdata add start //
                            string lotileng = Session["LOCATION"].ToString();
                            string ipAddress = CookiesData["ipAddress"].ToString();
                            string logdata = dbFunctions.StringData(@"SELECT 'TRANSDT : ' + CONVERT(NVARCHAR(50),TRANSDT,103)+' | '+'TRANSMY : ' + TRANSMY+' | '+'TRANSNO :
' + CONVERT(NVARCHAR(50),TRANSNO,103)+' | '+'COMPID : ' + ISNULL(COMPID,'(NULL)')+' | '+'JOBYY : ' + ISNULL(CONVERT(NVARCHAR(50),JOBYY,103),'(NULL)')+' | '+'JOBTP :
' + ISNULL(JOBTP,'(NULL)')+' | '+'JOBNO : ' + ISNULL(CONVERT(NVARCHAR(50),JOBNO,103),'(NULL)')+' | '+'EXPCD : ' + EXPCD+' | '+'REMARKS :
' + ISNULL(REMARKS,'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'USERID : ' + ISNULL(USERID,'(NULL)')+' | '+'UPDATEUSERID :
' + ISNULL(UPDATEUSERID,'(NULL)')+' | '+'ACTDTI : ' + ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+' | '+'INTIME :
' + CONVERT(NVARCHAR(50),INTIME,103)+' | '+'UPDATETIME : ' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + IPADDRESS+' | ' FROM CNF_JOBEXPMST
WHERE TRANSMY='" + iob.ExMY + "' AND TRANSNO='" + iob.InvoiceNO + "'");
                            string logid = "DELETE";
                            string tableid = "CNF_JOBEXPMST";
                            LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                            // logdata add end //
                        }
                        catch (Exception ex)
                        {
                            //ignore
                        }

                        try
                        {
                            // logdata add start //
                            string lotileng = CookiesData["LOCATION"].ToString();
                            string ipAddress = CookiesData["ipAddress"].ToString();
                            string logdata = dbFunctions.StringData(@"SELECT 'TRANSDT : ' + CONVERT(NVARCHAR(50),TRANSDT,103)+' | '+'TRANSMY : ' + TRANSMY+' | '+'TRANSNO :
' + CONVERT(NVARCHAR(50),TRANSNO,103)+' | '+'COMPID : ' + COMPID+' | '+'JOBYY : ' + CONVERT(NVARCHAR(50),JOBYY,103)+' | '+'JOBTP : ' + JOBTP+' | '+'JOBNO : 
' + CONVERT(NVARCHAR(50),JOBNO,103)+' | '+'EXPCD : ' + EXPCD+' | '+'SLNO : ' + CONVERT(NVARCHAR(50),SLNO,103)+' | '+'EXPID : ' + EXPID+' | '+'EXPAMT :
' + CONVERT(NVARCHAR(50),EXPAMT,103)+' | '+'REMARKS : ' + ISNULL(REMARKS,'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'USERID :
' + ISNULL(USERID,'(NULL)')+' | '+'UPDATEUSERID : ' + ISNULL(UPDATEUSERID,'(NULL)')+' | '+'ACTDTI : ' + ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+' | '+'INTIME :
' + CONVERT(NVARCHAR(50),INTIME,103)+' | '+'UPDATETIME : ' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + IPADDRESS+' | ' FROM CNF_JOBEXP                     
  WHERE TRANSMY='" + iob.ExMY + "' AND TRANSNO='" + iob.InvoiceNO + "' AND SLNO='" + iob.Sl + "'");
                            string logid = "DELETE";
                            string tableid = "CNF_JOBEXP";
                            LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                            // logdata add end //
                        }
                        catch (Exception ex)
                        {
                            //ignore
                        }


                        dob.delete_cnf_job_expense(iob); // delete delete_cnf_job_expense
                        dob.delete_cnf_job_expense_master(iob); // Delete delete_cnf_job_expense_master table
                        if (btnEdit.Text == "Edit")
                        {
                            Refresh();
                        }
                        else
                        {
                            dbFunctions.dropDown_Bind(ddlInvoice, "", "SELECT", "SELECT TRANSNO AS NM FROM CNF_JOBEXPMST WHERE TRANSMY='" + iob.ExMY + "'  ORDER BY TRANSNO");
                            ddlInvoice.SelectedIndex = -1;
                            Refresh();
                        }
                        GridShow();
                        txtExpenseNM.Focus();
                    }
                }
            }
        }

        private void Refresh()
        {
            //lblError.Visible = false;
            txtExCD.Text = "";
            txtExpenseNM.Text = "";
            txtRemarks.Text = "";
            txtTotalAmount.Text = ".00";
            if (btnEdit.Text == "Edit")
                check_Invoice_No();
            else
                ddlInvoice.SelectedIndex = -1;
            GridShow();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                //lblError.Visible = false;
                btnEdit.Text = "New";
                btnPrint.Visible = true;
                btnUpdate.Visible = true;
                txtNo.Visible = false;
                ddlInvoice.Visible = true;
                Refresh();
                StartUp();

                string uTp = CookiesData["USERTYPE"].ToString();
                string brCD = CookiesData["COMPANYID"].ToString();

                if (uTp == "COMPADMIN")
                {
                    dbFunctions.dropDown_Bind(ddlInvoice, "", "SELECT", "SELECT TRANSNO AS NM FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "'");
                }
                else
                {
                    dbFunctions.dropDown_Bind(ddlInvoice, "", "SELECT", "SELECT DISTINCT TRANSNO AS NM FROM CNF_JOBEXP WHERE TRANSMY ='" + lblMY.Text + "'");
                }
                ddlInvoice.Focus();
            }
            else
            {

                //lblError.Visible = false;
                btnEdit.Text = "Edit";
                btnPrint.Visible = false;
                btnUpdate.Visible = false;
                txtNo.Visible = true;
                ddlInvoice.Visible = false;
                Refresh();
                StartUp();
            }
        }

        protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInvoice.Text == "--SELECT--")
            {
                Refresh();
                //lblError.Visible = true;
                //lblError.Text = "Select invoice";
                dbFunctions.popupAlert(Page, "Select Invoice.", "w");
                ddlInvoice.Focus();
            }
            else
            {
                //lblError.Visible = false;
                // dbFunctions.TxtAdd("SELECT CONVERT(NVARCHAR(20),TRANSDT,103)AS TRANSDT FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "' AND TRANSNO =" + ddlInvoice.Text + "", txtExDT);

                dbFunctions.txtAdd("SELECT EXPCD FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "' AND TRANSNO =" + ddlInvoice.Text + "", txtExCD);
                dbFunctions.txtAdd("SELECT GL_ACCHART.ACCOUNTNM FROM CNF_JOBEXPMST INNER JOIN GL_ACCHART ON CNF_JOBEXPMST.EXPCD = GL_ACCHART.ACCOUNTCD " +
                    " WHERE CNF_JOBEXPMST.TRANSMY ='" + lblMY.Text + "' AND CNF_JOBEXPMST.TRANSNO =" + ddlInvoice.Text + " AND GL_ACCHART.STATUSCD = 'P'", txtExpenseNM);
                dbFunctions.txtAdd("SELECT REMARKS FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "' AND TRANSNO =" + ddlInvoice.Text + "", txtRemarks);
                dbFunctions.txtAdd("SELECT CONVERT(NVARCHAR(20),TRANSDT, 103) TRANSDT FROM CNF_JOBEXPMST WHERE TRANSMY ='" + lblMY.Text + "' AND TRANSNO =" + ddlInvoice.Text + "", txtExDT);
                GridShow();

            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            RefreshSession();

            if (lblMY.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select date.";
                dbFunctions.popupAlert(Page, "Select date.", "w");
                txtExDT.Focus();
            }
            else if (txtExCD.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select expense.";
                dbFunctions.popupAlert(Page, "Select expense.", "w");
                txtExCD.Focus();
            }
            //else if (ddlInvoice.Text == "--SELECT--" || ddlInvoice.Text == "")
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Select expense.";
            //    ddlInvoice.Focus();
            //}
            else
            {
                Session["Jobdate"] = txtExDT.Text;
                Session["transmy"] = lblMY.Text;
                if (btnEdit.Text == "Edit")
                    Session["InvoiceNo"] = txtNo.Text;
                else
                    Session["InvoiceNo"] = ddlInvoice.Text;
                Session["expenseCD"] = txtExCD.Text;
                Session["remarksT"] = txtRemarks.Text;

                 ScriptManager.RegisterStartupScript(this,
                this.GetType(), "OpenWindow", "window.open('../Report/Report/RptExpense_VR.aspx','_newtab');", true);
            }
        }

        protected void txtJobNo_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtJobNo = (TextBox)row.FindControl("txtJobNo");
            Label txtJobYear = (Label)row.FindControl("txtJobYear");
            Label txtJobTP = (Label)row.FindControl("txtJobTP");
            Label lblRemarksFoot = (Label)row.FindControl("lblRemarksFoot");
            Label txtBranch = (Label)row.FindControl("txtBranch");
            Label lblREFTP = (Label)row.FindControl("lblREFTP");
            TextBox txtParticulars = (TextBox)row.FindControl("txtParticulars");
            if (txtJobNo.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select job no.";
                dbFunctions.popupAlert(Page, "Select job no.", "w");
                txtJobNo.Focus();
            }
            else
            {
                //lblError.Visible = false;
                string jobno = "";
                string jobyear = "";
                string jobtp = "";
                string compid = "";
                string searchPar = txtJobNo.Text;

                int splitter = searchPar.IndexOf("|");
                if (splitter != -1)
                {
                    string[] lineSplit = searchPar.Split('|');

                    jobno = lineSplit[0];
                    jobyear = lineSplit[1];
                    jobtp = lineSplit[2];
                    compid = lineSplit[3];

                    txtJobNo.Text = jobno.Trim();
                    txtJobYear.Text = jobyear.Trim();
                    txtJobTP.Text = jobtp.Trim();
                    lblCmpID.Text = compid;
                    dbFunctions.lblAdd(@"SELECT GL_ACCHART.ACCOUNTNM FROM CNF_JOB INNER JOIN
                    GL_ACCHART ON CNF_JOB.PARTYID = GL_ACCHART.ACCOUNTCD
                    WHERE JOBNO='" + txtJobNo.Text + "' AND JOBTP='" + txtJobTP.Text + "' AND JOBYY='" + txtJobYear.Text + "'", lblRemarksFoot);

                    dbFunctions.lblAdd(@"SELECT REFTP FROM CNF_JOB 
                    WHERE JOBNO='" + txtJobNo.Text + "' AND JOBTP='" + txtJobTP.Text + "' AND JOBYY='" + txtJobYear.Text + "'", lblREFTP);

                    dbFunctions.lblAdd(@"SELECT ASL_BRANCH.BRANCHNM FROM CNF_JOB INNER JOIN ASL_BRANCH ON CNF_JOB.COMPID=ASL_BRANCH.BRANCHCD
                    WHERE JOBNO='" + txtJobNo.Text + "' AND JOBTP='" + txtJobTP.Text + "' AND JOBYY='" + txtJobYear.Text + "'", txtBranch);
                    txtParticulars.Focus();
                }
                else
                {
                    txtJobNo.Text = "";
                    txtJobYear.Text = "";
                    txtJobTP.Text = "";
                }
            }
        }
        protected void txtParticularsEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtParticularsEdit = (TextBox)row.FindControl("txtParticularsEdit");
            TextBox txtCodeEdit = (TextBox)row.FindControl("txtCodeEdit");
            TextBox txtAmountEdit = (TextBox)row.FindControl("txtAmountEdit");

            if (txtParticularsEdit.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select particulars.";
                dbFunctions.popupAlert(Page, "Select particulars.", "w");
                txtParticularsEdit.Focus();
            }
            else
            {
                txtCodeEdit.Text = "";
                dbFunctions.txtAdd("SELECT EXPID FROM CNF_EXPENSE WHERE EXPNM ='" + txtParticularsEdit.Text + "'", txtCodeEdit);
                if (txtCodeEdit.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select particulars.";
                    dbFunctions.popupAlert(Page, "Select particulars.", "w");
                    txtParticularsEdit.Text = "";
                    txtParticularsEdit.Focus();
                }
                else
                    txtAmountEdit.Focus();
            }
        }

        protected void txtJobNoEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtJobNoEdit = (TextBox)row.FindControl("txtJobNoEdit");
            Label txtJobYearEdit = (Label)row.FindControl("txtJobYearEdit");
            Label txtJobTPEdit = (Label)row.FindControl("txtJobTPEdit");
            Label lblRemarksFoot = (Label)row.FindControl("lblRemarksEdit");
            TextBox txtParticularsEdit = (TextBox)row.FindControl("txtParticularsEdit");
            Label txtBranch = (Label)row.FindControl("txtBranchEdit");
            if (txtJobNoEdit.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select job no.";
                dbFunctions.popupAlert(Page, "Select job no.", "w");
                txtJobNoEdit.Focus();
            }
            else
            {
                //lblError.Visible = false;
                string jobno = "";
                string jobyear = "";
                string jobtp = "";
                string searchPar = txtJobNoEdit.Text;

                int splitter = searchPar.IndexOf("|");
                if (splitter != -1)
                {
                    string[] lineSplit = searchPar.Split('|');

                    jobno = lineSplit[0];
                    jobyear = lineSplit[1];
                    jobtp = lineSplit[2];

                    txtJobNoEdit.Text = jobno.Trim();
                    txtJobYearEdit.Text = jobyear.Trim();
                    txtJobTPEdit.Text = jobtp.Trim();

                    dbFunctions.lblAdd(@"SELECT GL_ACCHART.ACCOUNTNM FROM CNF_JOB INNER JOIN
                    GL_ACCHART ON CNF_JOB.PARTYID = GL_ACCHART.ACCOUNTCD
                    WHERE JOBNO='" + txtJobNoEdit.Text + "' AND JOBTP='" + txtJobTPEdit.Text + "' AND JOBYY='" + txtJobTPEdit.Text + "'", lblRemarksFoot);

                    dbFunctions.lblAdd(@"SELECT ASL_BRANCH.BRANCHID FROM CNF_JOB INNER JOIN
                    ASL_BRANCH ON CNF_JOB.COMPID = ASL_BRANCH.COMPID
                    WHERE JOBNO='" + txtJobNoEdit.Text + "' AND JOBTP='" + txtJobTPEdit.Text + "' AND JOBYY='" + txtJobTPEdit.Text + "'", txtBranch);
                    txtParticularsEdit.Focus();
                }
                else
                {
                    txtJobNoEdit.Text = "";
                    txtJobYearEdit.Text = "";
                    txtJobTPEdit.Text = "";
                }
            }
        }

        protected void btnUpdate_OnClick(object sender, EventArgs e)
        {
            if (ddlInvoice.SelectedValue != "" || ddlInvoice.SelectedValue != "--SELECT--")
            {
                iob.ExMY = lblMY.Text;
                iob.ExpenseCD = txtExCD.Text;
                iob.REMARKS = txtRemarks.Text;
                iob.InvoiceNO = Convert.ToInt64(ddlInvoice.Text);
                try
                {
                    // logdata add start //
                    string lotileng = CookiesData["LOCATION"].ToString();
                    string ipAddress = CookiesData["ipAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT 'TRANSDT : ' + CONVERT(NVARCHAR(50),TRANSDT,103)+' | '+'TRANSMY : ' + TRANSMY+' | '+'TRANSNO :
' + CONVERT(NVARCHAR(50),TRANSNO,103)+' | '+'COMPID : ' + ISNULL(COMPID,'(NULL)')+' | '+'JOBYY : ' + ISNULL(CONVERT(NVARCHAR(50),JOBYY,103),'(NULL)')+' | '+'JOBTP :
' + ISNULL(JOBTP,'(NULL)')+' | '+'JOBNO : ' + ISNULL(CONVERT(NVARCHAR(50),JOBNO,103),'(NULL)')+' | '+'EXPCD : ' + EXPCD+' | '+'REMARKS :
' + ISNULL(REMARKS,'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'USERID : ' + ISNULL(USERID,'(NULL)')+' | '+'UPDATEUSERID :
' + ISNULL(UPDATEUSERID,'(NULL)')+' | '+'ACTDTI : ' + ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+' | '+'INTIME :
' + CONVERT(NVARCHAR(50),INTIME,103)+' | '+'UPDATETIME : ' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + IPADDRESS+' | ' FROM CNF_JOBEXPMST
WHERE TRANSMY='" + iob.ExMY + "' AND TRANSNO='" + iob.InvoiceNO + "'");

                    /*SELECT CONVERT(NVARCHAR(50),TRANSDT,103)+'  '+TRANSMY+'  '+
                        CONVERT(NVARCHAR(50),TRANSNO,103)+'  '+ISNULL(COMPID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),JOBYY,103),'(NULL)')+'  '+
                        ISNULL(JOBTP,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),JOBNO,103),'(NULL)')+'  '+EXPCD+'  '+ISNULL(REMARKS,'(NULL)')+'  '+
                        ISNULL(USERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+'  '+
                        CONVERT(NVARCHAR(50),INTIME,103)+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+IPADDRESS FROM CNF_JOBEXPMST
                   */
                    string logid = "UPDATE";
                    string tableid = "CNF_JOBEXPMST";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }

                dbFunctions.execute($@"UPDATE CNF_JOBEXP SET EXPCD='{iob.ExpenseCD}' WHERE TRANSMY='{iob.ExMY}' AND TRANSNO='{iob.InvoiceNO}'");
                dbFunctions.ExecuteQuery($@"UPDATE CNF_JOBEXPMST SET EXPCD='{iob.ExpenseCD}',REMARKS='{iob.REMARKS}' WHERE TRANSMY='{iob.ExMY}' AND TRANSNO='{iob.InvoiceNO}'");
                GridShow();
            }
        }
    }
}