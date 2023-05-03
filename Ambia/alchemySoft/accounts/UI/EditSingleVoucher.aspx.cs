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
    public partial class EditSingleVoucher : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        public string prefixText { get; set; }
        public int count { get; set; }
        public string contextKey { get; set; }
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    ddlEditTransType.AutoPostBack = true;
                    DateTime today = dbFunctions.timezone(DateTime.Now);
                    txtEdDate.Text = today.ToString("dd/MM/yyyy");
                    ddlEditTransType.Focus(); 
                }
            }
        }

        protected void ddlEditTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                Session["Transtype"] = "";
                if (ddlEditTransType.Text == "MREC")
                {
                    Session["Transtype"] = ddlEditTransType.Text;
                    gvDetails.Visible = false;
                }
                else if (ddlEditTransType.Text == "MPAY")
                {
                    Session["Transtype"] = ddlEditTransType.Text;
                    gvDetails.Visible = false;
                }
                else if (ddlEditTransType.Text == "JOUR")
                {
                    Session["Transtype"] = ddlEditTransType.Text;
                    gvDetails.Visible = false;
                }
                else if (ddlEditTransType.Text == "CONT")
                {
                    Session["Transtype"] = ddlEditTransType.Text;
                    gvDetails.Visible = false;
                }
                else
                {
                    return;
                }
                btnSearch.Focus();
            }

        }



        public void ShowGrid()
        {
            lblDebitCD.Text = "";
            lblCreditCD.Text = "";

            DateTime eddate = DateTime.Parse(txtEdDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string edate = eddate.ToString("yyyy/MM/dd");


            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT GL_STRANS.TRANSNO, GL_ACCHART.ACCOUNTNM AS DEBITCD, GL_STRANS.DEBITCD AS DRCD, GL_ACCHART_1.ACCOUNTNM AS CREDITCD, " +
                      " GL_STRANS.CREDITCD AS CRCD, GL_STRANS.CHEQUENO, CASE WHEN CONVERT(DATE, CHEQUEDT) = '01-01-1900' THEN '' ELSE CONVERT(nvarchar(10), CHEQUEDT, 103) END AS CHEQUEDT_CON, GL_STRANS.AMOUNT, GL_STRANS.REMARKS, GL_STRANS.TRANSFOR, GL_COSTP.COSTPNM, GL_STRANS.TRANSMODE, GL_STRANS.COSTPID " +
                      " FROM GL_STRANS LEFT JOIN GL_ACCHART ON GL_STRANS.DEBITCD = GL_ACCHART.ACCOUNTCD LEFT JOIN GL_ACCHART AS GL_ACCHART_1 ON GL_STRANS.CREDITCD = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_COSTP ON GL_STRANS.COSTPID = GL_COSTP.COSTPID " +
                      " WHERE (GL_STRANS.TRANSTP = '" + ddlEditTransType.Text + "') AND (GL_STRANS.TRANSDT = '" + edate + "')", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                gvDetails.Visible = true;

                dbFunctions.lblAdd(@"SELECT SUM(GL_STRANS.AMOUNT) AMT FROM GL_STRANS LEFT JOIN GL_ACCHART ON 
                    GL_STRANS.DEBITCD = GL_ACCHART.ACCOUNTCD LEFT JOIN GL_ACCHART AS GL_ACCHART_1 ON 
                    GL_STRANS.CREDITCD = GL_ACCHART_1.ACCOUNTCD LEFT OUTER JOIN GL_COSTP ON GL_STRANS.COSTPID = GL_COSTP.COSTPID  
                    WHERE (GL_STRANS.TRANSTP = '" + ddlEditTransType.Text + "') AND (GL_STRANS.TRANSDT = '" + edate + "')", lblTotalAmt);
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
                gvDetails.Rows[0].Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                if (txtEdDate.Text == "")
                {
                    dbFunctions.showMessage(Page,"Select a Date?");
                }
                else if (ddlEditTransType.Text == "--SELECT--")
                {
                    dbFunctions.showMessage(Page,"Select Transaction Type?");
                }
                else
                {
                    ShowGrid();
                }
            }
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            ShowGrid();
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow oItem = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            index = oItem.RowIndex;

            if (e.CommandName.Equals("print"))
            {
                if (CookiesData == null)
                {
                    Response.Redirect("~/login/ui/SignIn");
                }
                else
                {
                    Session["TransType"] = ddlEditTransType.Text;
                    Session["TransDate"] = txtEdDate.Text;
                    Label Voucher = (Label)gvDetails.Rows[index].Cells[1].FindControl("lblTransNo");
                    Session["VouchNo"] = Voucher.Text;
                    Label DBCD = (Label)gvDetails.Rows[index].Cells[2].FindControl("lblDRCD");

                    Session["DebitCD"] = DBCD.Text;
                    Label CRCD = (Label)gvDetails.Rows[index].Cells[4].FindControl("lblCreditCD");
                    Session["CreditCD"] = CRCD.Text;

                    //dbFunctions.showMessage(Page,"<script>");
                    //dbFunctions.showMessage(Page,"window.open('../Report/Report/RptCreditVoucherEdit.aspx','_blank')");
                    //dbFunctions.showMessage(Page,"</script>");
                }
            }
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            ShowGrid();

            DateTime eddate = DateTime.Parse(txtEdDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string edate = eddate.ToString("yyyy/MM/dd");

            DropDownList ddlTransFor = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlTransFor");
            DropDownList ddlTransMode = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlTransMode");
            Label lblTransNo = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblTransNo");
            dbFunctions.lblAdd("SELECT TRANSFOR FROM GL_STRANS WHERE TRANSDT ='" + edate + "' AND TRANSTP ='" + ddlEditTransType.Text + "' AND TRANSNO =" + lblTransNo.Text + "", lblTransFor);
            ddlTransFor.Text = lblTransFor.Text;
            dbFunctions.lblAdd("SELECT TRANSMODE FROM GL_STRANS WHERE TRANSDT ='" + edate + "' AND TRANSTP ='" + ddlEditTransType.Text + "' AND TRANSNO =" + lblTransNo.Text + "", lblTransMode);
            ddlTransMode.Text = lblTransMode.Text;

            DropDownList ddlCostPoolName = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlCostPoolName");
            dbFunctions.dropDown_Bind(ddlCostPoolName,"id","select", @"SELECT (GL_COSTP.COSTPNM + '|' + GL_COSTPMST.CATNM) nm, COSTPID id FROM GL_COSTP INNER JOIN GL_COSTPMST ON GL_COSTP.CATID = GL_COSTPMST.CATID ");

            Label lblCostPoolIDEdit = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblCostPoolIDEdit");
            if (lblCostPoolIDEdit.Text != "")
                ddlCostPoolName.SelectedIndex = ddlCostPoolName.Items.IndexOf(ddlCostPoolName.Items.FindByValue(lblCostPoolIDEdit.Text));

            ddlCostPoolName.Focus();
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
                SqlConnection conn = new SqlConnection(dbFunctions.Connection);
                string userName = CookiesData["USERID"].ToString();
                DateTime eddate = DateTime.Parse(txtEdDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string edate = eddate.ToString("yyyy/MM/dd");
                DropDownList ddlTransFor = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlTransFor");
                DropDownList ddlCostPoolName = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlCostPoolName");
                DropDownList ddlTransMode = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlTransMode");

                TextBox DebitCode = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtDbCd");
                TextBox CreditCode = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtCrCd");

                TextBox txtChq = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtChq");
                TextBox txtCqDt = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtCqDt");
                DateTime CQDT = new DateTime();
                string cqdt;
                if (txtChq.Text == "")
                {
                    CQDT = DateTime.Parse("01/01/1900 00:00:00", dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    cqdt = CQDT.ToString("yyyy/MM/dd");
                }
                else
                {
                    CQDT = DateTime.Parse(txtCqDt.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    cqdt = CQDT.ToString("yyyy/MM/dd");
                }
                TextBox txtAmount = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtAmount");
                TextBox txtRemarks = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarks");
                string Remarks = txtRemarks.Text;
                Label lblTransNo = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblTransNo");



                dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + DebitCode.Text + "'", lblDebitCD);
                dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + CreditCode.Text + "'", lblCreditCD);



                if (DebitCode.Text == "" || lblDebitCD.Text == "")
                {
                    lblErrMsg.Text = "Select Debit Head.";
                    lblErrMsg.Visible = true;
                    DebitCode.Focus();
                }
                else if (CreditCode.Text == "" || lblCreditCD.Text == "")
                {
                    lblErrMsg.Text = "Select Credit Head.";
                    lblErrMsg.Visible = true;
                    CreditCode.Focus();
                }
                else if (ddlCostPoolName.Text == "--SELECT--")
                {
                    lblErrMsg.Text = "Select Office Name.";
                    lblErrMsg.Visible = true;
                    ddlCostPoolName.Focus();
                }
                else
                {

                    string userIP = CookiesData["IpAddress"].ToString();
                    string userPC = CookiesData["PCName"].ToString();
                    DateTime uptime = dbFunctions.timezone(DateTime.Now);

                    try
                    {
                        // logdata add start //
                        string lotileng = CookiesData["Location"].ToString();
                        string ipAddress = CookiesData["IpAddress"].ToString();
                        string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  
                    '+ISNULL(CONVERT(NVARCHAR(50),SERIALNO,103),'(NULL)')+'  '+ISNULL(TRANSFOR,'(NULL)')+'  '+ISNULL(COSTPID,'(NULL)')+'  '+ISNULL(TRANSMODE,'(NULL)')+'  '+ISNULL(DEBITCD,
                    '(NULL)')+'  '+ISNULL(CREDITCD,'(NULL)')+'  '+ISNULL(CHEQUENO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),CHEQUEDT,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),AMOUNT,103),
                    '(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  '+ISNULL(UPUSERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  
                    '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+'  '+CONVERT(NVARCHAR(50),INTIME,103)+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+IPADDRESS+'  
                    '+ISNULL(UPDIPADD,'(NULL)') 
                    FROM GL_STRANS  where TRANSTP = '" + ddlEditTransType.Text + "' and TRANSDT = '" + edate + "' and TRANSNO = '" + lblTransNo.Text + "'");
                        string logid = "UPDATE";
                        string tableid = "GL_STRANS";
                        dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                        // logdata add end //
                    }
                    catch (Exception ex)
                    {
                        //ignore
                    }


                    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                    SqlCommand cmd = new SqlCommand(@"update GL_STRANS set TRANSFOR ='" + ddlTransFor.Text + "', COSTPID ='" + ddlCostPoolName.SelectedValue +
                        "', TRANSMODE ='" + ddlTransMode.Text + "', DEBITCD='" + lblDebitCD.Text + "', CREDITCD = '" + lblCreditCD.Text +
                        "', CHEQUENO= '" + txtChq.Text + "', CHEQUEDT ='" + cqdt + "', AMOUNT = '" + txtAmount.Text +
                        "' ,REMARKS = @Remarks, UPDATEUSERID=@UPDATEUSERID, UPUSERPC=@UPUSERPC, UPDATETIME=@UPDATETIME, UPDIPADD=@UPDIPADD" +
                                                    " where TRANSTP = '" + ddlEditTransType.Text + "' and TRANSDT = '" + edate + "' and TRANSNO = '" + lblTransNo.Text + "'", conn);
                    cmd.Parameters.AddWithValue("@Remarks", Remarks);
                    cmd.Parameters.AddWithValue("@UPDATEUSERID", userName);
                    cmd.Parameters.AddWithValue("@UPUSERPC", userPC);
                    cmd.Parameters.AddWithValue("@UPDATETIME", uptime);
                    cmd.Parameters.AddWithValue("@UPDIPADD", userIP);
                    cmd.ExecuteNonQuery();
                    if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                    gvDetails.EditIndex = -1;
                    lblErrMsg.Text = "Successfully Updated";
                    ShowGrid();
                    lblErrMsg.Visible = true;
                }
            }
        }

        protected void txtDbCd_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            //NamingContainer return the container that the control sits in
            TextBox otherDB = (TextBox)row.FindControl("txtDbCd");

            dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + otherDB.Text + "'", lblDebitCD);

            TextBox txtCrCd = (TextBox)row.FindControl("txtCrCd");
            txtCrCd.Focus();
        }

        protected void txtCrCd_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            //NamingContainer return the container that the control sits in
            TextBox otherCR = (TextBox)row.FindControl("txtCrCd");
            DropDownList ddlTransMode = (DropDownList)row.FindControl("ddlTransMode");
            TextBox txtChq = (TextBox)row.FindControl("txtChq");
            TextBox txtCqDt = (TextBox)row.FindControl("txtCqDt");
            TextBox txtAmount = (TextBox)row.FindControl("txtAmount");

            dbFunctions.lblAdd(@"Select ACCOUNTCD from GL_ACCHART where STATUSCD = 'P' and ACCOUNTNM = '" + otherCR.Text + "'", lblCreditCD);

            if (ddlTransMode.Text == "CASH CHEQUE")
            {
                txtChq.Enabled = true;
                txtCqDt.Enabled = true;
                txtChq.Focus();
            }
            else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
            {
                txtChq.Enabled = true;
                txtCqDt.Enabled = true;
                txtChq.Focus();
            }
            else
            {
                txtChq.Enabled = false;
                txtCqDt.Enabled = false;
                txtAmount.Focus();
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
                string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
                SqlConnection conn = new SqlConnection(dbFunctions.Connection);
                DateTime eddate = DateTime.Parse(txtEdDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string edate = eddate.ToString("yyyy/MM/dd");

                Label lblTransNo = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblTransNo");


                try
                {
                    // logdata add start //
                    string lotileng = CookiesData["Location"].ToString();
                    string ipAddress = CookiesData["IpAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT TRANSTP+'  '+ISNULL(CONVERT(NVARCHAR(50),TRANSDT,103),'(NULL)')+'  '+TRANSMY+'  '+CONVERT(NVARCHAR(50),TRANSNO,103)+'  
                    '+ISNULL(CONVERT(NVARCHAR(50),SERIALNO,103),'(NULL)')+'  '+ISNULL(TRANSFOR,'(NULL)')+'  '+ISNULL(COSTPID,'(NULL)')+'  '+ISNULL(TRANSMODE,'(NULL)')+'  '+ISNULL(DEBITCD,
                    '(NULL)')+'  '+ISNULL(CREDITCD,'(NULL)')+'  '+ISNULL(CHEQUENO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),CHEQUEDT,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),AMOUNT,103),
                    '(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  '+ISNULL(UPUSERPC,'(NULL)')+'  '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  
                    '+ISNULL(CONVERT(NVARCHAR(50),ACTDTI,103),'(NULL)')+'  '+CONVERT(NVARCHAR(50),INTIME,103)+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+IPADDRESS+'  
                    '+ISNULL(UPDIPADD,'(NULL)') 
                    FROM GL_STRANS WHERE TRANSTP = '" + ddlEditTransType.Text + "' and TRANSDT = '" + edate + "' and TRANSNO = '" + lblTransNo.Text + "'");
                    string logid = "DELETE";
                    string tableid = "GL_STRANS";
                    dbFunctions.insertLogData(lotileng, logid, tableid, logdata);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }


                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                SqlCommand cmd = new SqlCommand("delete from GL_STRANS where TRANSTP = '" + ddlEditTransType.Text + "' and TRANSDT = '" + edate + "' and TRANSNO = '" + lblTransNo.Text + "'", conn);
                cmd.ExecuteNonQuery();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                dbFunctions.showMessage(Page,"Successfully Deleted");
                ShowGrid();
            }
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        public int index { get; set; }

        protected void txtEdDate_TextChanged(object sender, EventArgs e)
        {
            ddlEditTransType.Focus();
        }

        protected void ddlTransFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            var ddlTransFor = (DropDownList)row.FindControl("ddlTransFor");
            var ddlCostPoolName = (DropDownList)row.FindControl("ddlCostPoolName");
            var ddlTransMode = (DropDownList)row.FindControl("ddlTransMode");
            if (ddlTransFor.Text == "OFFICIAL")
            {
                ddlTransMode.Focus();
                ddlCostPoolName.SelectedIndex = -1;
                ddlCostPoolName.Enabled = false;
            }
            else
            {
                ddlCostPoolName.Focus();
                ddlCostPoolName.Enabled = true;
            }
        }

        protected void ddlTransMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList ddlTransMode = (DropDownList)row.FindControl("ddlTransMode");
            TextBox txtChq = (TextBox)row.FindControl("txtChq");
            TextBox txtCqDt = (TextBox)row.FindControl("txtCqDt");
            TextBox txtDbCd = (TextBox)row.FindControl("txtDbCd");
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                if (ddlTransMode.Text == "CASH CHEQUE")
                {
                    txtChq.Enabled = true;
                    txtCqDt.Enabled = true;
                    txtDbCd.Focus();
                }
                else if (ddlTransMode.Text == "A/C PAYEE CHEQUE")
                {
                    txtChq.Enabled = true;
                    txtCqDt.Enabled = true;
                    txtDbCd.Focus();
                }
                else
                {
                    txtChq.Enabled = false;
                    txtCqDt.Enabled = false;
                    txtDbCd.Focus();
                }
            }
        }
    }
}