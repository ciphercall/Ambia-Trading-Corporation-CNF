using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlchemyAccounting;
using DynamicMenu.Accounts.DataAccess;
using DynamicMenu.Accounts.Interface;

namespace alchemy.accounts.UI
{
    public partial class BranchEntry : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        AccountDataAccess dob = new AccountDataAccess();
        AccountInterface iob = new AccountInterface();
        SqlConnection con = new SqlConnection(DbFunctions.Connection);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/Login/UI/Login.aspx");
            }
            else
            {
                string formLink = "/Accounts/UI/BranchEntry.aspx";
                bool permission = UserPermissionChecker.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        ShowGridForBrunch();
                    }
                }
                else
                {
                    Response.Redirect("../../DeshBoard/UI/Default.aspx");
                }
            }
        }

        public string BranchCodeGenarate()
        {
            string maximumId = "";
            string companyId = CookiesData["COMPANYID"].ToString();
            string chekcMaxId = DbFunctions.StringData(" SELECT MAX(BRANCHCD) AS BRANCHCD FROM ASL_BRANCH WHERE COMPID='" + companyId + "'");
            if (chekcMaxId == "")
            {
                maximumId = companyId + "01";
            }
            else
            {
                int userid = Convert.ToInt16(chekcMaxId.Substring(3, 2));
                userid++;
                if (userid < 10)
                {
                    maximumId = companyId + "0" + userid;
                }
                else
                {
                    maximumId = companyId + userid;
                }
            }

            return maximumId;
        }
        protected void ShowGridForBrunch()
        {
            string companyID = "";
            if (CookiesData["COMPANYID"] != null)
                companyID = CookiesData["COMPANYID"].ToString();

            SqlConnection conn = new SqlConnection(DbFunctions.Connection);
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT COMPID, BRANCHCD, BRANCHNM, BRANCHID, ADDRESS,CONTACTNO, EMAILID, 
                        CASE(STATUS) WHEN 'A' THEN 'ACTIVE' ELSE 'INACTIVE' END AS STATUS 
                        FROM ASL_BRANCH WHERE COMPID=@COMPID", conn);
            cmd.Parameters.AddWithValue("@COMPID", companyID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridViewForBranch.DataSource = ds;
                gridViewForBranch.DataBind();
                //TextBox txtBrunchName = (TextBox)gridViewForBranch.FooterRow.FindControl("txtBranchName");
                //txtBrunchName.Focus();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gridViewForBranch.DataSource = ds;
                gridViewForBranch.DataBind();
                int columncount = gridViewForBranch.Rows[0].Cells.Count;
                gridViewForBranch.Rows[0].Cells.Clear();
                gridViewForBranch.Rows[0].Cells.Add(new TableCell());
                gridViewForBranch.Rows[0].Cells[0].ColumnSpan = columncount;
                gridViewForBranch.Rows[0].Cells[0].Text = "No Records Found";
                //TextBox txtBrunchName = (TextBox)gridViewForBranch.FooterRow.FindControl("txtBrunchName");
                //txtBrunchName.Focus();
            }
        }

        protected void gridViewForBranch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/LogIn/UI/LogIn.aspx");
            }
            else
            {
                var txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
                iob.LotiLengTudeUpdate = txtLotiLongTude.Text;
                iob.IpAddressUpdate = DbFunctions.IpAddress();
                iob.UserIdUpdate = Convert.ToInt64(CookiesData["USERID"].ToString());
                iob.InTimeUpdate = DbFunctions.Timezone(DateTime.Now);

                var lblCompanyIdEdit = (Label)gridViewForBranch.Rows[e.RowIndex].FindControl("lblCompanyIdEdit");
                var lblBranchCdEdit = (Label)gridViewForBranch.Rows[e.RowIndex].FindControl("lblBranchCdEdit");

                var txtBranchNameEdit = (TextBox)gridViewForBranch.Rows[e.RowIndex].FindControl("txtBranchNameEdit");
                var txtBranchIdEdit = (TextBox)gridViewForBranch.Rows[e.RowIndex].FindControl("txtBranchIdEdit");
                var txtAddressEdit = (TextBox)gridViewForBranch.Rows[e.RowIndex].FindControl("txtAddressEdit");
                var txtContactNoEdit = (TextBox)gridViewForBranch.Rows[e.RowIndex].FindControl("txtContactNoEdit");
                var txtEmailEdit = (TextBox)gridViewForBranch.Rows[e.RowIndex].FindControl("txtEmailEdit");
                var ddlStatusEdit = (DropDownList)gridViewForBranch.Rows[e.RowIndex].FindControl("ddlStatusEdit");

                var field = new string[] { txtBranchNameEdit.Text, ddlStatusEdit.Text };

                if (DbFunctions.FieldCheck(field) == true)
                {
                    iob.CompanyId = Convert.ToInt64(lblCompanyIdEdit.Text);
                    iob.BranchCode = lblBranchCdEdit.Text;
                    iob.BranchNm = txtBranchNameEdit.Text;
                    iob.BranchId = txtBranchIdEdit.Text;
                    iob.Address = txtAddressEdit.Text;
                    iob.ContactNo = txtContactNoEdit.Text;
                    iob.EmailId = txtEmailEdit.Text;
                    iob.Status = ddlStatusEdit.SelectedValue;
                    string s = dob.UPDATE_ASL_BRANCH(iob);
                    if (s == "")
                    {
                        lblMsg.Text = "Updated successfully.";
                        lblMsg.Visible = true;
                    }
                    else
                    {
                        lblMsg.Text = "Updated not successfull.";
                        lblMsg.Visible = true;
                    }
                }
                else
                {
                    lblMsg.Text = "Mendatory field is empty.";
                    lblMsg.Visible = true;
                }

                gridViewForBranch.EditIndex = -1;
                ShowGridForBrunch();
            }
        }

        protected void gridViewForBranch_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/LogIn/UI/LogIn.aspx");
            }
            else
            {
                gridViewForBranch.EditIndex = e.NewEditIndex;
                ShowGridForBrunch();
                Label lblCompanyIdEdit = (Label)gridViewForBranch.Rows[e.NewEditIndex].FindControl("lblCompanyIdEdit");
                Label lblBranchCdEdit = (Label)gridViewForBranch.Rows[e.NewEditIndex].FindControl("lblBranchCdEdit");

                DropDownList ddlStatusEdit =
                    (DropDownList)gridViewForBranch.Rows[e.NewEditIndex].FindControl("ddlStatusEdit");

                string status = "";
                status = DbFunctions.StringData(@"SELECT CASE(STATUS) WHEN 'A' THEN 'ACTIVE' ELSE 'INACTIVE' END AS STATUS 
                FROM ASL_BRANCH WHERE COMPID='" + lblCompanyIdEdit + "' AND BRANCHCD='" + lblBranchCdEdit + "'");

                ddlStatusEdit.Text = status;
                lblMsg.Visible = false;
            }
        }

        protected void gridViewForBranch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/LogIn/UI/LogIn.aspx");
            }
            else
            {
                gridViewForBranch.EditIndex = -1;
                ShowGridForBrunch();
                lblMsg.Visible = false;
            }
        }

        protected void gridViewForBranch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/LogIn/UI/LogIn.aspx");
            }
            else
            {
                if (e.CommandName.Equals("SaveCon"))
                {
                    var txtBranchName = (TextBox)gridViewForBranch.FooterRow.FindControl("txtBranchName");
                    var txtBranchId = (TextBox)gridViewForBranch.FooterRow.FindControl("txtBranchId");
                    var txtAddress = (TextBox)gridViewForBranch.FooterRow.FindControl("txtAddress");
                    var txtContactNo = (TextBox)gridViewForBranch.FooterRow.FindControl("txtContactNo");
                    var txtEmail = (TextBox)gridViewForBranch.FooterRow.FindControl("txtEmail");
                    var ddlStatus = (DropDownList)gridViewForBranch.FooterRow.FindControl("ddlStatus");

                    var txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
                    iob.IpAddressInsert = DbFunctions.IpAddress();
                    iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
                    iob.UserPcInsert = DbFunctions.UserPc();
                    iob.LotiLengTudeInsert = txtLotiLongTude.Text;
                    iob.InTimeInsert = DbFunctions.Timezone(DateTime.Now);

                    var field = new string[] { txtBranchName.Text, ddlStatus.Text, txtContactNo.Text };

                    if (DbFunctions.FieldCheck(field) == true)
                    {

                        iob.CompanyId = Convert.ToInt64(CookiesData["COMPANYID"].ToString());
                        iob.BranchCode = BranchCodeGenarate();
                        iob.BranchNm = txtBranchName.Text;
                        iob.BranchId = txtBranchId.Text;
                        iob.Address = txtAddress.Text;
                        iob.ContactNo = txtContactNo.Text;
                        iob.EmailId = txtEmail.Text;
                        iob.Status = ddlStatus.SelectedValue;

                        string s = dob.INSERT_ASL_BRANCH(iob);

                        if (s == "")
                        {
                            lblMsg.Visible = false;
                        }
                        else
                        {
                            lblMsg.Text = "Branch not create, please try angain.";
                            lblMsg.Visible = true;
                        }
                        ShowGridForBrunch();
                    }
                    else
                    {
                        lblMsg.Text = "Mendatory field is empty.";
                        lblMsg.Visible = true;
                    }
                }
            }
        }

        protected void gridViewForBranch_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/LogIn/UI/LogIn.aspx");
            }
            else
            {
                var lblCompanyId = (Label)gridViewForBranch.Rows[e.RowIndex].FindControl("lblCompanyId");
                var lblBranchCd = (Label)gridViewForBranch.Rows[e.RowIndex].FindControl("lblBranchCd");

                iob.CompanyId = Convert.ToInt64(lblCompanyId.Text);
                iob.BranchCode = lblBranchCd.Text;
                string s = dob.DELETE_ASL_BRANCH(iob);
                if (s == "")
                {
                    lblMsg.Text = "Successfully deleted branch.";
                    lblMsg.Visible = true;
                }
                else
                {
                    lblMsg.Text = "Branch not delete, please try again.";
                    lblMsg.Visible = true;
                }
                ShowGridForBrunch();
            }
        }

    }
}