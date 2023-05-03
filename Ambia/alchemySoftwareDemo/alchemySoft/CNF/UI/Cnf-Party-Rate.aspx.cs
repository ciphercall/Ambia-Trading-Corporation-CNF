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

namespace alchemySoft.CNF.UI
{
    public partial class Cnf_Party_Rate : System.Web.UI.Page
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
                string formLink = "/CNF/UI/Cnf-Party-Rate.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");

                if (permission)
                {
                    if (!IsPostBack)
                    {
                        txtPartNM.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        protected void gv_Party_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            TextBox txtEXPNMFooter = (TextBox)gv_Party.FooterRow.FindControl("txtEXPNMFooter");
            Label lblEXPIDFooter = (Label)gv_Party.FooterRow.FindControl("lblEXPIDFooter");
            TextBox txtRATEFooter = (TextBox)gv_Party.FooterRow.FindControl("txtRATEFooter");
            TextBox txtREMARKSFooter = (TextBox)gv_Party.FooterRow.FindControl("txtREMARKSFooter");
            DropDownList ddlRATETPFooter = (DropDownList)gv_Party.FooterRow.FindControl("ddlRATETPFooter");
            if (txtPartyID.Text == "")
                txtPartNM.Focus();
            else if (lblEXPIDFooter.Text == "")
                txtEXPNMFooter.Focus();
            else if (txtRATEFooter.Text == "")
                txtRATEFooter.Focus();
            else
            {
                iob.Ipaddress = CookiesData["ipAddress"].ToString();
                iob.Userpc = CookiesData["PCName"].ToString();
                iob.UserNM = CookiesData["UserName"].ToString();
                iob.PartyID = lblPartyID.Text;
                iob.ExpensesID = lblEXPIDFooter.Text;
                iob.RegID = ddlRegID.Text;
                iob.JobQuality = ddlJobQuality.Text;
                iob.RateTP = ddlRATETPFooter.Text;
                iob.Rate = Convert.ToDecimal(txtRATEFooter.Text);
                iob.Remarks = txtREMARKSFooter.Text;
                if (e.CommandName.Equals("SaveCon"))
                {
                    dob.Insert_Expese_RT(iob);
                    GridShow();
                }
                else if (e.CommandName.Equals("Complete"))
                {
                    dob.Insert_Expese_RT(iob);
                    txtPartNM.Text = "";
                    lblPartyID.Text = "";
                    GridShow();
                    txtPartNM.Focus();
                }
            }

        }
        private void GridShow()
        {
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (conn.State != ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT     CNF_EXPENSE.EXPNM, CNF_PARTYRT.EXPID, CNF_PARTYRT.RATETP, CNF_PARTYRT.RATE, CNF_PARTYRT.REMARKS
                        FROM CNF_PARTYRT INNER JOIN CNF_EXPENSE ON CNF_PARTYRT.EXPID = CNF_EXPENSE.EXPID WHERE PARTYID='" + lblPartyID.Text + "' AND JOBQUALITY='" + ddlJobQuality.Text + "' AND REGID='" + ddlRegID.Text + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != ConnectionState.Closed) conn.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                gv_Party.DataSource = ds;
                gv_Party.DataBind();
                gv_Party.Visible = true;
                TextBox txtEXPNMFooter = (TextBox)gv_Party.FooterRow.FindControl("txtEXPNMFooter");
                txtEXPNMFooter.Focus();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gv_Party.DataSource = ds;
                gv_Party.DataBind();
                int columncount = gv_Party.Rows[0].Cells.Count;
                gv_Party.Rows[0].Cells.Clear();
                gv_Party.Rows[0].Cells.Add(new TableCell());
                gv_Party.Rows[0].Cells[0].ColumnSpan = columncount;
                gv_Party.Rows[0].Cells[0].Text = "No Records Found";
                gv_Party.Rows[0].Visible = false;
                TextBox txtEXPNMFooter = (TextBox)gv_Party.FooterRow.FindControl("txtEXPNMFooter");
                txtEXPNMFooter.Focus();
            }
        }

        protected void txtPartNM_TextChanged(object sender, EventArgs e)
        {
            if (txtPartNM.Text != "")
            {
                dbFunctions.lblAdd("SELECT ACCOUNTCD FROM GL_ACCHART WHERE ACCOUNTNM='" + txtPartNM.Text + "'", lblPartyID);
                GridShow();
                ddlRegID.Focus();
            }
        }

        protected void txtEXPNMFooter_TextChanged(object sender, EventArgs e)
        {
            TextBox txtEXPNMFooter = (TextBox)gv_Party.FooterRow.FindControl("txtEXPNMFooter");
            TextBox txtRATEFooter = (TextBox)gv_Party.FooterRow.FindControl("txtRATEFooter");
            DropDownList ddlRATETPFooter = (DropDownList)gv_Party.FooterRow.FindControl("ddlRATETPFooter");
            Label lblEXPIDFooter = (Label)gv_Party.FooterRow.FindControl("lblEXPIDFooter");
            if (txtEXPNMFooter.Text == "")
                txtEXPNMFooter.Focus();
            else
            {
                dbFunctions.lblAdd("SELECT EXPID FROM CNF_EXPENSE WHERE EXPNM='" + txtEXPNMFooter.Text + "'", lblEXPIDFooter);
                ddlRATETPFooter.Focus();
            }
        }

        protected void txtEXPNMEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtEXPNMEdit = (TextBox)row.FindControl("txtEXPNMEdit");
            TextBox txtRATEEdit = (TextBox)row.FindControl("txtRATEEdit");
            Label lblEXPIDEdit = (Label)row.FindControl("lblEXPIDEdit");
            DropDownList ddlRATETPEdit = (DropDownList)row.FindControl("ddlRATETPEdit");
            if (txtEXPNMEdit.Text == "")
                txtEXPNMEdit.Focus();
            else
            {
                dbFunctions.lblAdd("SELECT EXPID FROM CNF_EXPENSE WHERE EXPNM='" + txtEXPNMEdit.Text + "'", lblEXPIDEdit);
                ddlRATETPEdit.Focus();
            }
        }

        protected void gv_Party_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gv_Party.EditIndex = e.NewEditIndex;
            GridShow();
            Label lblEXPIDEdit = (Label)gv_Party.Rows[e.NewEditIndex].FindControl("lblEXPIDEdit");
            DropDownList ddlRATETPEdit = (DropDownList)gv_Party.Rows[e.NewEditIndex].FindControl("ddlRATETPEdit");
            TextBox txtEXPNMEdit = (TextBox)gv_Party.Rows[e.NewEditIndex].FindControl("txtEXPNMEdit");
            Session["EXPID"] = lblEXPIDEdit.Text;
            Session["RATETP"] = ddlRATETPEdit.Text;
            txtEXPNMEdit.Focus();

        }

        protected void gv_Party_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtEXPNMEdit = (TextBox)gv_Party.Rows[e.RowIndex].FindControl("txtEXPNMEdit");
            Label lblEXPIDEdit = (Label)gv_Party.Rows[e.RowIndex].FindControl("lblEXPIDEdit");
            TextBox txtRATEEdit = (TextBox)gv_Party.Rows[e.RowIndex].FindControl("txtRATEEdit");
            TextBox txtREMARKSEdit = (TextBox)gv_Party.Rows[e.RowIndex].FindControl("txtREMARKSEdit");
            DropDownList ddlRATETPEdit = (DropDownList)gv_Party.Rows[e.RowIndex].FindControl("ddlRATETPEdit");
            if (txtPartyID.Text == "")
                txtPartNM.Focus();
            else if (lblEXPIDEdit.Text == "")
                txtEXPNMEdit.Focus();
            else if (txtRATEEdit.Text == "")
                txtRATEEdit.Focus();
            else
            {
                iob.Ipaddress = CookiesData["ipAddress"].ToString();
                iob.Userpc = CookiesData["PCName"].ToString();
                iob.UserNM = CookiesData["UserName"].ToString();
                iob.PartyID = lblPartyID.Text;
                iob.ExpensesID = lblEXPIDEdit.Text;
                iob.Rate = Convert.ToDecimal(txtRATEEdit.Text);
                iob.RegID = ddlRegID.Text;
                iob.RateTP = ddlRATETPEdit.Text;
                iob.RegID = ddlRegID.Text;
                iob.JobQuality = ddlJobQuality.Text;
                iob.Remarks = txtREMARKSEdit.Text;
                dob.UPDATE_CNF_PARTYRT(iob);
                gv_Party.EditIndex = -1;
                GridShow();
            }
        }

        protected void gv_Party_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblEXPID = (Label)gv_Party.Rows[e.RowIndex].FindControl("lblEXPID");
            Label lblRATETP = (Label)gv_Party.Rows[e.RowIndex].FindControl("lblRATETP");
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (conn.State != ConnectionState.Open) conn.Open();
            string Script = "DELETE FROM CNF_PARTYRT WHERE PARTYID='" + lblPartyID.Text + "' AND EXPID='" + lblEXPID.Text + "' AND JOBQUALITY='" + ddlJobQuality.Text + "' AND REGID='" + ddlRegID.Text + "' AND RATETP='" + lblRATETP.Text + "'";
            SqlCommand cmd = new SqlCommand(Script, conn);
            cmd.ExecuteNonQuery();
            GridShow();
        }

        protected void ddlRATETPFooter_SelectedIndexChanged(object sender, EventArgs e)
        {

            TextBox txtRATEFooter = (TextBox)gv_Party.FooterRow.FindControl("txtRATEFooter");
            txtRATEFooter.Focus();
        }

        protected void ddlRATETPEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            TextBox txtRATEEdit = (TextBox)row.FindControl("txtRATEEdit");
            txtRATEEdit.Focus();
        }

        protected void gv_Party_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_Party.EditIndex = -1;
            GridShow();
        }

        protected void ddlRegID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblPartyID.Text == "")
                txtPartNM.Focus();
            else
            {
                GridShow();
                ddlJobQuality.Focus();

            }
        }

        protected void ddlJobQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblPartyID.Text == "")
                txtPartNM.Focus();
            else
                GridShow();
        }

        protected void txtPartNM_OnTextChanged(object sender, EventArgs e)
        {
            GridShow();
        }
    }
}