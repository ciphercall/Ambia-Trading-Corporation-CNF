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
    public partial class Party_Commission : System.Web.UI.Page
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
                string formLink = "/CNF/UI/Party-Information.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");

                if (permission)
                {
                    if (!IsPostBack)
                    {
                        
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        public void GridShow()
        {
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            conn.Open();

            SqlCommand cmdd = new SqlCommand("SELECT COMMSL, EXCTP, VALUEFR, VALUETO, VALUETP, COMMAMT FROM  CNF_COMMISSION WHERE PARTYID='" + txtPartyID.Text + "' AND REGID='" + ddlRegID.Text + "' AND JOBQUALITY='" + ddlJobQuality.Text + "'", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmdd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();


            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                gvDetails.Visible = true;

                DropDownList ddlExctype = (DropDownList)gvDetails.FooterRow.FindControl("ddlExctype");
                ddlExctype.Focus();

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
                DropDownList ddlExctype = (DropDownList)gvDetails.FooterRow.FindControl("ddlExctype");
                ddlExctype.Focus();
            }
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            GridShow();
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Label lblSerial = (Label)gvDetails.FooterRow.FindControl("lblSerial");
            DropDownList ddlExctype = (DropDownList)gvDetails.FooterRow.FindControl("ddlExctype");
            TextBox txtfrom = (TextBox)gvDetails.FooterRow.FindControl("txtfrom");
            TextBox txtTO = (TextBox)gvDetails.FooterRow.FindControl("txtTO");
            DropDownList ddlvalueTp = (DropDownList)gvDetails.FooterRow.FindControl("ddlvalueTp");
            TextBox txtAmount = (TextBox)gvDetails.FooterRow.FindControl("txtAmount");


            if (e.CommandName.Equals("SaveCon"))
            {

                if (txtPartyID.Text == "")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "particular input missing";
                    txtParty.Focus();
                }
                else if (txtfrom.Text == "" || txtfrom.Text == "0" || txtfrom.Text == ".00" || txtfrom.Text == "0.00")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "particular input missing";
                    txtfrom.Focus();
                }
                else if (txtTO.Text == "" || txtTO.Text == "0" || txtTO.Text == ".00" || txtTO.Text == "0.00")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "particular input missing";
                    txtTO.Focus();
                }
                else if (txtAmount.Text == "" || txtAmount.Text == "0" || txtAmount.Text == ".00" || txtAmount.Text == "0.00")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "particular input missing";
                    txtAmount.Focus();
                }
                else
                {
                    lblErrMsg.Visible = false;

                    iob.PARTYID = txtPartyID.Text;
                    iob.RegID = ddlRegID.Text;
                    iob.JobQuality = ddlJobQuality.Text;
                    iob.COMMSL = Convert.ToInt64(gvDetails.FooterRow.Cells[0].Text);
                    iob.EXCTP = ddlExctype.Text;
                    iob.VALUEFROM = Convert.ToDecimal(txtfrom.Text);
                    iob.VALUETO = Convert.ToDecimal(txtTO.Text);
                    iob.VALUETP = ddlvalueTp.Text;
                    iob.COMMAMT = Convert.ToDecimal(txtAmount.Text);
                    iob.InTime = DateTime.Now;
                    iob.UpdateTime = DateTime.Now;
                    iob.Userpc = CookiesData["PCName"].ToString();
                    iob.IPAddress = CookiesData["ipAddress"].ToString();

                    dob.SaveCommissionInfo(iob);

                    GridShow();

                }
            }
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                dbFunctions.lblAdd(" select MAX (COMMSL) from CNF_COMMISSION where PARTYID='" + txtPartyID.Text + "' ", lblChkInternalID);

                if (lblChkInternalID.Text == "")
                {
                    string cid = "01";
                    e.Row.Cells[0].Text = cid;
                }

                else
                {
                    var id = Int32.Parse(lblChkInternalID.Text) + 1;
                    e.Row.Cells[0].Text = id.ToString();
                }
            }
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblSerial = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSerial");

            iob.PARTYID = txtPartyID.Text;
            iob.COMMSL = Convert.ToInt64(lblSerial.Text);
            iob.RegID = ddlRegID.Text;
            iob.JobQuality = ddlJobQuality.Text;
            try
            {
                // logdata add start //
                string lotileng = CookiesData["LOCATION"].ToString();
                string ipAddress = CookiesData["ipAddress"].ToString();
                string logdata = dbFunctions.StringData(@"SELECT 'PARTYID : ' + PARTYID+' | '+'REGID : ' + REGID+' | '+'JOBQUALITY : 
' + JOBQUALITY+' | '+'COMMSL : ' + CONVERT(NVARCHAR(50),COMMSL,103)+' | '+'EXCTP : ' + ISNULL(EXCTP,'(NULL)')+' | '+'VALUEFR : 
' + ISNULL(CONVERT(NVARCHAR(50),VALUEFR,103),'(NULL)')+' | '+'VALUETO : ' + ISNULL(CONVERT(NVARCHAR(50),VALUETO,103),'(NULL)')+' | '+'VALUETP : 
' + ISNULL(CONVERT(NVARCHAR(50),VALUETP,103),'(NULL)')+' | '+'COMMAMT : ' + ISNULL(CONVERT(NVARCHAR(50),COMMAMT,103),'(NULL)')+' | '+'USERPC : 
' + ISNULL(USERPC,'(NULL)')+' | '+'INTIME : ' + ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+' | '+'UPDATETIME :
' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + ISNULL(IPADDRESS,'(NULL)')+' | ' FROM CNF_COMMISSION
where PARTYID='" + iob.PARTYID + "' and COMMSL='" + iob.COMMSL +
                "' AND REGID='" + iob.RegID + "' AND JOBQUALITY='" + iob.JobQuality + "'");
                string logid = "DELETE";
                string tableid = "CNF_COMMISSION";
                LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                // logdata add end //
            }
            catch (Exception ex)
            {
                //ignore
            }

            dob.DeleteCommissionInfo(iob);

            gvDetails.EditIndex = -1;
            GridShow();

            DropDownList ddlExctype = (DropDownList)gvDetails.FooterRow.FindControl("ddlExctype");
            ddlExctype.Focus();

        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            GridShow();

            Label lblSerialEdit = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblSerialEdit");
            DropDownList ddlExctypeEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlExctypeEdit");
            dbFunctions.lblAdd("SELECT EXCTP FROM CNF_COMMISSION WHERE PARTYID ='" + txtPartyID.Text + "' AND COMMSL =" + lblSerialEdit.Text + "", lblValTP);
            ddlExctypeEdit.Text = lblValTP.Text;
            DropDownList ddlvalueTpEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlvalueTpEdit");
            dbFunctions.lblAdd("SELECT VALUETP FROM CNF_COMMISSION WHERE PARTYID ='" + txtPartyID.Text + "' AND COMMSL =" + lblSerialEdit.Text + "", lblValCommPer);
            ddlvalueTpEdit.Text = lblValCommPer.Text;
            ddlExctypeEdit.Focus();

        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label lblSerialEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSerialEdit");
            DropDownList ddlExctypeEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlExctypeEdit");
            TextBox txtfromEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtfromEdit");
            TextBox txtToEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtToEdit");
            DropDownList ddlvalueTpEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlvalueTpEdit");
            TextBox txtAmountEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtAmountEdit");

            if (txtPartyID.Text == "")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "particular input missing";
                txtParty.Focus();
            }
            else if (txtfromEdit.Text == "" || txtfromEdit.Text == "0" || txtfromEdit.Text == ".00" || txtfromEdit.Text == "0.00")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "particular input missing";
                txtfromEdit.Focus();
            }
            else if (txtToEdit.Text == "" || txtToEdit.Text == "0" || txtToEdit.Text == ".00" || txtToEdit.Text == "0.00")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "particular input missing";
                txtToEdit.Focus();
            }
            else if (txtAmountEdit.Text == "" || txtAmountEdit.Text == "0" || txtAmountEdit.Text == ".00" || txtAmountEdit.Text == "0.00")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "particular input missing";
                txtAmountEdit.Focus();
            }

            else
            {
                lblErrMsg.Visible = false;

                iob.PARTYID = txtPartyID.Text;
                iob.RegID = ddlRegID.Text;
                iob.JobQuality = ddlJobQuality.Text;
                iob.COMMSL = Convert.ToInt64(lblSerialEdit.Text);
                iob.EXCTP = ddlExctypeEdit.Text;
                iob.VALUEFROM = Convert.ToDecimal(txtfromEdit.Text);
                iob.VALUETO = Convert.ToDecimal(txtToEdit.Text);
                iob.VALUETP = ddlvalueTpEdit.Text;
                iob.COMMAMT = Convert.ToDecimal(txtAmountEdit.Text);
                iob.InTime = DateTime.Now;
                iob.UpdateTime = DateTime.Now;
                iob.Userpc = CookiesData["PCName"].ToString();
                iob.IPAddress = CookiesData["ipAddress"].ToString();

                try
                {
                    // logdata add start //
                    string lotileng = CookiesData["LOCATION"].ToString();
                    string ipAddress = CookiesData["ipAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT 'PARTYID : ' + PARTYID+' | '+'REGID : ' + REGID+' | '+'JOBQUALITY : 
' + JOBQUALITY+' | '+'COMMSL : ' + CONVERT(NVARCHAR(50),COMMSL,103)+' | '+'EXCTP : ' + ISNULL(EXCTP,'(NULL)')+' | '+'VALUEFR : 
' + ISNULL(CONVERT(NVARCHAR(50),VALUEFR,103),'(NULL)')+' | '+'VALUETO : ' + ISNULL(CONVERT(NVARCHAR(50),VALUETO,103),'(NULL)')+' | '+'VALUETP : 
' + ISNULL(CONVERT(NVARCHAR(50),VALUETP,103),'(NULL)')+' | '+'COMMAMT : ' + ISNULL(CONVERT(NVARCHAR(50),COMMAMT,103),'(NULL)')+' | '+'USERPC : 
' + ISNULL(USERPC,'(NULL)')+' | '+'INTIME : ' + ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+' | '+'UPDATETIME :
' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + ISNULL(IPADDRESS,'(NULL)')+' | ' FROM CNF_COMMISSION
where PARTYID='" + iob.PARTYID + "' and COMMSL='" + iob.COMMSL + "' AND REGID='" + iob.RegID + "' AND JOBQUALITY='" + iob.JobQuality + "'");

                    /*SELECT PARTYID+'  '+REGID+'  '+JOBQUALITY+'  '+CONVERT(NVARCHAR(50),
                    COMMSL,103)+'  '+ISNULL(EXCTP,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),VALUEFR,103),'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),VALUETO,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),VALUETP,103),'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),COMMAMT,103),'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  '+
                    ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+
                    ISNULL(IPADDRESS,'(NULL)') FROM CNF_COMMISSION */
                    string logid = "UPDATE";
                    string tableid = "CNF_COMMISSION";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }

                dob.UpdateCommissionInfo(iob);

                gvDetails.EditIndex = -1;
                GridShow();

                DropDownList ddlExctype = (DropDownList)gvDetails.FooterRow.FindControl("ddlExctype");
                ddlExctype.Focus();

            }
        }

        protected void ddlExctype_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList ddlExctype = (DropDownList)row.FindControl("ddlExctype");
            TextBox txtfrom = (TextBox)row.FindControl("txtfrom");
            txtfrom.Focus();
        }

        protected void ddlExctypeEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            TextBox txtfromEdit = (TextBox)row.FindControl("txtfromEdit");
            txtfromEdit.Focus();
        }

        protected void ddlvalueTp_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
            txtAmount.Focus();
        }

        protected void ddlvalueTpEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            TextBox txtAmountEdit = (TextBox)row.FindControl("txtAmountEdit");
            txtAmountEdit.Focus();
        }

        protected void txtPartNM_TextChanged(object sender, EventArgs e)
        {
            if (txtParty.Text == "")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "Select party name.";
                txtParty.Focus();
            }
            else
            {
                lblErrMsg.Visible = false;
                txtPartyID.Text = "";
                dbFunctions.txtAdd("SELECT ACCOUNTCD FROM GL_ACCHART WHERE ACCOUNTNM='" + txtParty.Text + "' AND STATUSCD ='P'", txtPartyID);
                if (txtPartyID.Text == "")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Select party name.";
                    txtParty.Text = "";
                    txtPartyID.Text = "";
                    txtParty.Focus();
                }
                else
                {
                    GridShow();
                    ddlRegID.Focus();
                }
            }
        }

        protected void ddlJobQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridShow();
        }

        protected void ddlRegID_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridShow();
            ddlJobQuality.Focus();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            GridShow();
        }
    }
}