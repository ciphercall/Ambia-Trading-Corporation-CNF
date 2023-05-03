using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft.CNF.DataAccess;
using alchemySoft.CNF.Interface;
using alchemySoft.LogIn;

namespace alchemySoft.CNF.UI
{
    public partial class Expense_Information : System.Web.UI.Page
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
                        txtcat.Focus();

                        dbFunctions.lblAdd("select MAX(EXPCID) from CNF_EXPMST ", lblcid);
                        if (lblcid.Text == "")
                        {
                            txtcatID.Text = "I1";
                        }
                        else
                        {
                            var resultString = Regex.Match(lblcid.Text, @"\d+").Value;
                            var id = Int32.Parse(resultString) + 1;
                            txtcatID.Text = "I" + id;
                        }

                        GridShow();
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

            SqlCommand cmdd = new SqlCommand("SELECT CNF_EXPENSE.EXPNM, CNF_EXPENSE.EXPCID, CNF_EXPENSE.EXPID, CNF_EXPENSE.REMARKS" +
                     " FROM CNF_EXPENSE INNER JOIN" +
                      " CNF_EXPMST ON CNF_EXPENSE.EXPCID = CNF_EXPMST.EXPCID " +
                      " where CNF_EXPENSE.EXPCID = '" + txtcatID.Text + "' ", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmdd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();


            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                gvDetails.Visible = true;

                TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
                txtParticulars.Focus();

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
        }

        protected void txtcat_TextChanged(object sender, EventArgs e)
        {

            dbFunctions.lblAdd("select MAX(EXPCID) from CNF_EXPMST ", lblcid);

            if (lblcid.Text == "")
            {
                txtcatID.Text = "I1";
            }

            else
            {
                var resultString = Regex.Match(lblcid.Text, @"\d+").Value;
                var id = Int32.Parse(resultString) + 1;

                txtcatID.Text = "I" + id;
            }

            dbFunctions.txtAdd("SELECT EXPCID FROM CNF_EXPMST WHERE EXPCNM= '" + txtcat.Text + "'", txtcatID);
            GridShow();

            TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
            txtParticulars.Focus();

        }

        protected void txtcatID_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = txtcatID.Text;

                dbFunctions.lblAdd("select MAX(EXPID) from CNF_EXPENSE where EXPCID='" + txtcatID.Text + "' ", lblChkInternalID);

                if (lblChkInternalID.Text == "")
                {
                    string cid = txtcatID.Text + "01";
                    e.Row.Cells[1].Text = cid;

                }

                else
                {
                    //var resultString = Regex.Match(lblChkInternalID.Text, @"\d+").Value;
                    //var id = Int32.Parse(resultString) + 1;
                    var resultString = lblChkInternalID.Text.Substring(2, 2);
                    var id = Int64.Parse(resultString) + 1;
                    if (id > 99)
                        e.Row.Visible = false;
                    else
                    {
                        if (id < 10)
                            e.Row.Cells[1].Text = txtcatID.Text + "0" + id;
                        else
                            e.Row.Cells[1].Text = txtcatID.Text + id;
                    }

                }
            }

        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            GridShow();
        }

        private bool Previousdata(string id)
        {
            bool bflag = false;
            DataTable table = new DataTable();

            try
            {
                table = dob.showExpenseInfo(id);
                DataSet userDS = new DataSet();
            }
            catch (Exception ex)
            {
                table = null;
                Response.Write(ex.Message);
            }
            if (table != null)
            {
                if (table.Rows.Count > 0)
                    bflag = true;
            }
            return bflag;
        }


        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Label lblID = (Label)gvDetails.FooterRow.FindControl("lblID");
            Label lblExpense = (Label)gvDetails.FooterRow.FindControl("lblExpense");
            TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
            TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");



            if (e.CommandName.Equals("SaveCon"))
            {
                if (Previousdata(gvDetails.FooterRow.Cells[0].Text) == false)
                {
                    iob.EXPCNM = txtcat.Text;
                    iob.EXPCID = gvDetails.FooterRow.Cells[0].Text;
                    iob.InTime = dbFunctions.timezone(DateTime.Now);
                    iob.UpdateTime = dbFunctions.timezone(DateTime.Now);
                    iob.Userpc = CookiesData["PCName"].ToString();
                    iob.IPAddress = CookiesData["ipAddress"].ToString();

                    dob.MstInput(iob);
                }


                iob.EXPID = gvDetails.FooterRow.Cells[1].Text;

                if (txtParticulars.Text == "")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "particular input missing";
                    txtParticulars.Focus();
                }
                else if (iob.EXPID.Length > 4)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "You will not add no more expense id in this category.";
                    txtParticulars.Focus();
                }
                else
                {
                    iob.EXPCID = gvDetails.FooterRow.Cells[0].Text;
                    iob.EXPID = gvDetails.FooterRow.Cells[1].Text;
                    iob.EXPNM = txtParticulars.Text;
                    iob.REMARKS = txtRemarks.Text;
                    iob.InTime = dbFunctions.timezone(DateTime.Now);
                    iob.UpdateTime = dbFunctions.timezone(DateTime.Now);
                    iob.Userpc = CookiesData["PCName"].ToString();
                    iob.IPAddress = CookiesData["ipAddress"].ToString();

                    dob.SaveExpenseInfo(iob);
                }
                GridShow();
            }

        }
        public void Refresh()
        {
            //Label lblID = (Label)gvDetails.FooterRow.FindControl("lblID");
            //Label lblExpense = (Label)gvDetails.FooterRow.FindControl("lblExpense");
            //TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
            //TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");

            //txtParticulars.Text = "";
            //txtRemarks.Text = "";
            txtcat.Text = "";

            dbFunctions.lblAdd("select MAX(EXPCID) from CNF_EXPMST ", lblcid);
            if (lblcid.Text == "")
            {
                txtcatID.Text = "I1";
            }
            else
            {
                var resultString = Regex.Match(lblcid.Text, @"\d+").Value;
                var id = Int32.Parse(resultString) + 1;

                txtcatID.Text = "I" + id;
            }


            GridShow();
            txtcat.Focus();

        }


        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblID = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblID");
            Label lblExpense = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblExpense");
            iob.EXPCID = lblID.Text;
            iob.EXPID = lblExpense.Text;

            if (CheckExpIdData(iob.EXPID))
            {
                try
                {
                    // logdata add start //
                    string lotileng = CookiesData["LOCATION"].ToString();
                    string ipAddress = CookiesData["IpAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT 'EXPCID : ' + EXPCID+' | '+'EXPID : ' + EXPID+' | '+'EXPNM : 
' + ISNULL(EXPNM,'(NULL)')+' | '+'REMARKS : ' + ISNULL(REMARKS,'(NULL)')+' | '+'USERID : ' + ISNULL(USERID,'(NULL)')+' | '+'UPDUSERID : 
' + ISNULL(UPDUSERID,'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'UPDUSERPC : ' + ISNULL(UPDUSERPC,'(NULL)')+' | '+'INTIME :
' + ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+' | '+'UPDATETIME : ' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : 
' + ISNULL(IPADDRESS,'(NULL)')+' | '+'UPDIPADD : ' + ISNULL(UPDIPADD,'(NULL)')+' | ' FROM CNF_EXPENSE
  where EXPCID='" + iob.EXPCID + "' and EXPID='" + iob.EXPID + "'");

                    /*SELECT EXPCID+'  '+EXPID+'  '+ISNULL(EXPNM,'(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+
                    ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDUSERID,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  '+ISNULL(UPDUSERPC,'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+
                    ISNULL(IPADDRESS,'(NULL)')+'  '+ISNULL(UPDIPADD,'(NULL)') FROM CNF_EXPENSE */
                    string logid = "DELETE";
                    string tableid = "CNF_EXPENSE";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }

                dob.DeleteExpenseInfo(iob);

                gvDetails.EditIndex = -1;
                GridShow();



            }
            else
            {
                lblErrMsgExist.Text = "This Expense Head have Child Data.";
                lblErrMsgExist.Visible = true;
            }
            TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
            TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");
            txtParticulars.Focus();


        }

        public static bool CheckExpIdData(string expid)
        {
            string countdDataJobBill = dbFunctions.StringData("SELECT COUNT(*) CNT FROM [dbo].[CNF_JOBBILL] WHERE EXPID='" + expid + "'");
            string countdDataExpense = dbFunctions.StringData("SELECT COUNT(*) CNT FROM [dbo].[CNF_JOBEXP] WHERE EXPID='" + expid + "'");
            if (countdDataJobBill == "0" && countdDataExpense == "0")
                return true;
            else return false;
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            GridShow();

            TextBox txtParticularsEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtParticularsEdit");
            txtParticularsEdit.Focus();
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label lblIDEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblIDEdit");
            Label lblExpenseEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblExpenseEdit");
            TextBox txtParticularsEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtParticularsEdit");
            TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarksEdit");


            Label lblID = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblID");
            Label lblExpense = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblExpense");

            TextBox txtParticulars = (TextBox)gvDetails.FooterRow.FindControl("txtParticulars");
            TextBox txtRemarks = (TextBox)gvDetails.FooterRow.FindControl("txtRemarks");


            if (txtParticularsEdit.Text == "")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "particular input missing";
                txtParticularsEdit.Focus();
            }

            else
            {
                iob.EXPCID = lblIDEdit.Text;
                iob.EXPID = lblExpenseEdit.Text;

                iob.EXPNM = txtParticularsEdit.Text;
                iob.REMARKS = txtRemarksEdit.Text;
                iob.InTime = dbFunctions.timezone(DateTime.Now);
                iob.UpdateTime = dbFunctions.timezone(DateTime.Now);
                iob.Userpc = CookiesData["PCName"].ToString();
                iob.IPAddress = CookiesData["IpAddress"].ToString();


                try
                {
                    // logdata add start //
                    string lotileng = CookiesData["Location"].ToString();
                    string ipAddress = CookiesData["IpAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT 'EXPCID : ' + EXPCID+' | '+'EXPID : ' + EXPID+' | '+'EXPNM : 
' + ISNULL(EXPNM,'(NULL)')+' | '+'REMARKS : ' + ISNULL(REMARKS,'(NULL)')+' | '+'USERID : ' + ISNULL(USERID,'(NULL)')+' | '+'UPDUSERID : 
' + ISNULL(UPDUSERID,'(NULL)')+' | '+'USERPC : ' + ISNULL(USERPC,'(NULL)')+' | '+'UPDUSERPC : ' + ISNULL(UPDUSERPC,'(NULL)')+' | '+'INTIME :
' + ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+' | '+'UPDATETIME : ' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : 
' + ISNULL(IPADDRESS,'(NULL)')+' | '+'UPDIPADD : ' + ISNULL(UPDIPADD,'(NULL)')+' | ' FROM CNF_EXPENSE
 where EXPCID='" + iob.EXPCID + "' and EXPID='" + iob.EXPID + "'");

                    /*SELECT EXPCID+'  '+EXPID+'  '+ISNULL(EXPNM,'(NULL)')+'  '+ISNULL(REMARKS,'(NULL)')+'  '+
                    ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDUSERID,'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  '+ISNULL(UPDUSERPC,'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+
                    ISNULL(IPADDRESS,'(NULL)')+'  '+ISNULL(UPDIPADD,'(NULL)') FROM CNF_EXPENSE */
                    string logid = "UPDATE";
                    string tableid = "CNF_EXPENSE";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }

                dob.EditExpenseInfo(iob);

                gvDetails.EditIndex = -1;
                GridShow();
                txtParticulars.Focus();

            }
        }
    }
}