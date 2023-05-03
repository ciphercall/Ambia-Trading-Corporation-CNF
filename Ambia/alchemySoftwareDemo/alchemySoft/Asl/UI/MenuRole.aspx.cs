using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using alchemySoft;
using System.Data.SqlClient;
using System.Web.Services;
using System.Text;
using System.Collections.Specialized;
using alchemySoft.Asl.DataAccess;
using alchemySoft.Asl.Interface;
using alchemySoft.LogIn;

namespace alchemySoft.Asl.UI
{
    public partial class MenuRole : System.Web.UI.Page
    {
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        SqlConnection con = new SqlConnection(dbFunctions.Connection);
        ASLDataAccess dob = new ASLDataAccess();
        ASLInterface iob = new ASLInterface();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CookiesData == null || !dbFunctions.permit())
                {
                    Response.Redirect("~/login/ui/signin.aspx");
                }
                else if (CookiesData["USERTYPE"].ToString() == "SUPERADMIN" || CookiesData["USERTYPE"].ToString() == "COMPADMIN")
                {
                    {
                        //Session["Companyidgrid"] = null;
                        string userId = CookiesData["USERID"].ToString();
                        ddlUserCompanyName.Focus(); 

                        if (CookiesData["USERTYPE"].ToString() == "SUPERADMIN")
                        {
                            lblUserlabel.Visible = false;
                            lblUserlabel1.Visible = true;
                            dbFunctions.dropDown_Bind(ddlModuleName,"id","select", @"SELECT MODULENM NM,MODULEID ID FROM ASL_MENUMST 
                        WHERE MODULEID NOT IN ('01','02') ORDER BY MODULENM");

                            dbFunctions.dropDown_Bind(ddlUserCompanyName, "id", "select", @"SELECT COMPNM NM,COMPID ID FROM ASL_COMPANY WHERE   COMPID!='100' ORDER BY COMPNM");
                        }
                        else
                        {
                            dbFunctions.dropDown_Bind(ddlModuleName, "id", "select", @"SELECT DISTINCT ASL_MENUMST.MODULENM NM,ASL_MENUMST.MODULEID ID
                    FROM ASL_USERCO INNER JOIN
                    ASL_ROLE ON ASL_USERCO.COMPID = ASL_ROLE.COMPID INNER JOIN
                    ASL_MENU ON ASL_ROLE.MODULEID = ASL_MENU.MODULEID INNER JOIN
                    ASL_MENUMST ON ASL_ROLE.MODULEID = ASL_MENUMST.MODULEID
                    WHERE ASL_ROLE.STATUS='A' AND ASL_ROLE.MODULEID NOT IN ('01','02')  AND ASL_ROLE.USERID='" + userId + "' ORDER BY ASL_MENUMST.MODULENM");
                            lblUserlabel.Visible = true;
                            lblUserlabel1.Visible = false;


                            dbFunctions.dropDown_Bind(ddlUserCompanyName, "id", "select", @"SELECT DISTINCT ASL_USERCO.USERNM NM,ASL_USERCO.USERID ID FROM ASL_USERCO INNER JOIN
                                        ASL_ROLE ON ASL_USERCO.COMPID = ASL_ROLE.COMPID AND ASL_USERCO.USERID = ASL_ROLE.USERID 
                                        WHERE ASL_USERCO.OPTP NOT IN ('SUPERADMIN','COMPADMIN')  ORDER BY ASL_USERCO.USERNM");
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/login/ui/signin.aspx");
                }
            }
        }

       
       

        public string FieldCheck()
        {
            string checkResult = "";
            if (ddlUserCompanyName.Text == "--SELECT--" )
            {
                checkResult = "Select module name.";
                ddlModuleName.SelectedIndex = -1;
                ddlModuleName.Focus();
            }
            else if (ddlMenuType.SelectedValue == "S")
            {
                checkResult = "Select form type.";
                ddlMenuType.SelectedIndex = -1;
                ddlMenuType.Focus();
            }
            else
            {
                checkResult = "true";
            }
            if(checkResult!="true")
                dbFunctions.popupAlert(Page, checkResult, "w");
            return checkResult;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn.aspx");
            }
            else
            {
                if (FieldCheck() == "true")
                {
                    ShowGridForAslRole(); 
                }
                else
                {
                   FieldCheck(); 
                }

            }
        }
        protected void ShowGridForAslRole()
        { 
            string id = ddlUserCompanyName.Text;
            if (id.Length > 3)
                id = id.Substring(0, 3); 

            string userid = ddlUserCompanyName.Text;
            if (userid.Length == 3)
                userid = userid + "01";
            string moduleid = ddlModuleName.Text;
            string menuTp = ddlMenuType.SelectedValue;

            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (conn.State != ConnectionState.Open)conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT ASL_MENU.MENUNM, ASL_MENUMST.MODULENM, 
                CASE(ASL_ROLE.STATUS) WHEN 'A' THEN 'ACTIVE' ELSE 'INACTIVE' END AS STATUS, 
                CASE(ASL_ROLE.INSERTR) WHEN 'A' THEN 'ACTIVE' ELSE 'INACTIVE' END AS INSERTR, 
                CASE(ASL_ROLE.UPDATER) WHEN 'A' THEN 'ACTIVE' ELSE 'INACTIVE' END AS UPDATER, 
                CASE(ASL_ROLE.DELETER) WHEN 'A' THEN 'ACTIVE' ELSE 'INACTIVE' END AS DELETER, 
                ASL_ROLE.COMPID, ASL_ROLE.USERID, ASL_ROLE.MODULEID, ASL_ROLE.MENUID
                FROM ASL_ROLE INNER JOIN
                ASL_MENUMST ON ASL_ROLE.MODULEID = ASL_MENUMST.MODULEID INNER JOIN
                ASL_MENU ON ASL_ROLE.MODULEID = ASL_MENU.MODULEID AND ASL_ROLE.MENUID = ASL_MENU.MENUID
                WHERE (ASL_ROLE.COMPID = @COMPID) AND (ASL_ROLE.USERID = @USERID AND ASL_ROLE.MENUTP=@MENUTP  
                AND ASL_ROLE.MODULEID=@MODULEID) ORDER BY ASL_MENU.MENUSL", conn);
            cmd.Parameters.AddWithValue("@COMPID", id);
            cmd.Parameters.AddWithValue("@USERID", userid);
            cmd.Parameters.AddWithValue("@MENUTP", menuTp);
            cmd.Parameters.AddWithValue("@MODULEID", moduleid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != ConnectionState.Closed)conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridViewAslRole.DataSource = ds;
                gridViewAslRole.DataBind();
                //TextBox txtZONENM = (TextBox)gridViewAslRole.FooterRow.FindControl("txtCountryNM");
                //txtZONENM.Focus();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gridViewAslRole.DataSource = ds;
                gridViewAslRole.DataBind();
                int columncount = gridViewAslRole.Rows[0].Cells.Count;
                gridViewAslRole.Rows[0].Cells.Clear();
                gridViewAslRole.Rows[0].Cells.Add(new TableCell());
                gridViewAslRole.Rows[0].Cells[0].ColumnSpan = columncount;
                gridViewAslRole.Rows[0].Cells[0].Text = "No Records Found";
                //TextBox txtZONENM = (TextBox)gridViewAslRole.FooterRow.FindControl("txtCountryNM");
                //txtZONENM.Focus();
            }
        }

        protected void gridViewAslRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn.aspx");
            }
            else
            {
                var lblCompanyIdEdit = (Label)gridViewAslRole.Rows[e.RowIndex].FindControl("lblCompanyIdEdit");
                var lblUserIdEdit = (Label)gridViewAslRole.Rows[e.RowIndex].FindControl("lblUserIdEdit");
                var lblModuleIdEdit = (Label)gridViewAslRole.Rows[e.RowIndex].FindControl("lblModuleIdEdit");
                var lblMenuIdEdit = (Label)gridViewAslRole.Rows[e.RowIndex].FindControl("lblMenuIdEdit");

                var ddlStatusEdit = (DropDownList)gridViewAslRole.Rows[e.RowIndex].FindControl("ddlStatusEdit");
                var ddlInsertEdit = (DropDownList)gridViewAslRole.Rows[e.RowIndex].FindControl("ddlInsertEdit");
                var ddlUpdateEdit = (DropDownList)gridViewAslRole.Rows[e.RowIndex].FindControl("ddlUpdateEdit");
                var ddlDeleteEdit = (DropDownList)gridViewAslRole.Rows[e.RowIndex].FindControl("ddlDeleteEdit");

                var txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
                TextBox txtIp = (TextBox)Master.FindControl("txtIp");
                iob.LotiLengTudeUpdate = txtLotiLongTude.Text;
                iob.ipAddressUpdate = dbFunctions.ipAddress();
                iob.UserIdUpdate = Convert.ToInt64(CookiesData["USERID"].ToString());
                iob.InTimeUpdate = dbFunctions.timezone(DateTime.Now);

                iob.LotiLengTudeInsert = txtLotiLongTude.Text;
                iob.ipAddressInsert = dbFunctions.ipAddress();
                iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
                iob.userPcInsert = dbFunctions.userPc();
                iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);

                iob.CompanyId = Convert.ToInt16(lblCompanyIdEdit.Text);
                iob.CompanyUserId = Convert.ToInt16(lblUserIdEdit.Text);
                iob.ModuleId = lblModuleIdEdit.Text;
                iob.MenuId = lblMenuIdEdit.Text;
                iob.MenuType = ddlMenuType.SelectedValue;

                iob.Status = ddlStatusEdit.SelectedValue;
                iob.InsertRole = ddlInsertEdit.SelectedValue;
                iob.UpdateRole = ddlUpdateEdit.SelectedValue;
                iob.DeleteRole = ddlDeleteEdit.SelectedValue;

                string s = dob.UPDATE_ASL_ROLE(iob);

                if (s == "")
                {
                    string cmpuserid = iob.CompanyUserId.ToString();
                    string cmpadminid = iob.CompanyId + "01";

                    if (cmpuserid == cmpadminid && CookiesData["USERTYPE"].ToString() == "SUPERADMIN" && iob.Status == "I")
                    {
                        iob.CompanyUserId = Convert.ToInt64(cmpadminid);
                        dob.DELETE_ASL_ROLE(iob);
                    }
                    else if (cmpuserid == cmpadminid && CookiesData["USERTYPE"].ToString() == "SUPERADMIN" && iob.Status == "A")
                    {
                        iob.Status = "I";
                        iob.InsertRole = "I";
                        iob.UpdateRole = "I";
                        iob.DeleteRole = "I";

                        if (con.State != ConnectionState.Open) con.Open();
                        
                        SqlCommand cmd = new SqlCommand(@"SELECT COMPID, USERID  FROM ASL_USERCO 
                        WHERE COMPID=@COMPID AND USERID!=@USERID", con);
                        cmd.Parameters.AddWithValue("@COMPID", iob.CompanyId);
                        cmd.Parameters.AddWithValue("@USERID", cmpadminid);
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            iob.CompanyId = Convert.ToInt16(dr["COMPID"].ToString());
                            iob.CompanyUserId = Convert.ToInt16(dr["USERID"].ToString());
                            dob.INSERT_ASL_ROLE(iob);
                        }
                        dr.Close();
                        if (con.State != ConnectionState.Closed)con.Close();
                    }
                    // logdata add start //
                    string lotileng = iob.LotiLengTudeUpdate;
                    string logdata = @"Company Id: " + iob.CompanyId + ", User Id: " + iob.CompanyUserId + ", Module Id: " +
                    iob.ModuleId + ", Menu Id: " + iob.MenuId + ", Menu Type: " + iob.MenuType + ", Status: " + iob.Status +
                    ", Insert Role: " + iob.InsertRole + ", Update Role: " + iob.UpdateRole + ", Delete Role: " + iob.DeleteRole;
                    string logid = "UPDATE";
                    string tableid = "ASL_ROLE";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, txtIp.Text);
                    // logdata add start //
                     
                    dbFunctions.popupAlert(Page, " Updated succesfully.", "s");
                }
                else
                { 
                    dbFunctions.popupAlert(Page, "Something went wrong", "e");
                }
                gridViewAslRole.EditIndex = -1;
                ShowGridForAslRole();
            }
        }

        protected void gridViewAslRole_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn.aspx");
            }
            else
            {
                gridViewAslRole.EditIndex = e.NewEditIndex;
                ShowGridForAslRole();
                var lblCompanyIdEdit = (Label)gridViewAslRole.Rows[e.NewEditIndex].FindControl("lblCompanyIdEdit");
                var lblUserIdEdit = (Label)gridViewAslRole.Rows[e.NewEditIndex].FindControl("lblUserIdEdit");
                var lblModuleIdEdit = (Label)gridViewAslRole.Rows[e.NewEditIndex].FindControl("lblModuleIdEdit");
                var lblMenuIdEdit = (Label)gridViewAslRole.Rows[e.NewEditIndex].FindControl("lblMenuIdEdit");

                var ddlStatusEdit = (DropDownList)gridViewAslRole.Rows[e.NewEditIndex].FindControl("ddlStatusEdit");
                var ddlInsertEdit = (DropDownList)gridViewAslRole.Rows[e.NewEditIndex].FindControl("ddlInsertEdit");
                var ddlUpdateEdit = (DropDownList)gridViewAslRole.Rows[e.NewEditIndex].FindControl("ddlUpdateEdit");
                var ddlDeleteEdit = (DropDownList)gridViewAslRole.Rows[e.NewEditIndex].FindControl("ddlDeleteEdit");

                string status = "";
                string insert = "";
                string update = "";
                string delete = "";
                if (con.State != ConnectionState.Open) con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT ASL_ROLE.STATUS, ASL_ROLE.INSERTR, ASL_ROLE.UPDATER, ASL_ROLE.DELETER
                FROM ASL_ROLE INNER JOIN ASL_MENUMST ON ASL_ROLE.MODULEID = ASL_MENUMST.MODULEID INNER JOIN
                ASL_MENU ON ASL_ROLE.MODULEID = ASL_MENU.MODULEID AND ASL_ROLE.MENUID = ASL_MENU.MENUID
                WHERE ASL_ROLE.COMPID=@COMPID AND  ASL_ROLE.USERID=@USERID AND ASL_ROLE.MODULEID=@MODULEID 
                AND ASL_ROLE.MENUID=@MENUID ", con);
                cmd.Parameters.AddWithValue("@COMPID", lblCompanyIdEdit.Text);
                cmd.Parameters.AddWithValue("@USERID", lblUserIdEdit.Text);
                cmd.Parameters.AddWithValue("@MODULEID", lblModuleIdEdit.Text);
                cmd.Parameters.AddWithValue("@MENUID", lblMenuIdEdit.Text);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    status = dr["STATUS"].ToString();
                    insert = dr["INSERTR"].ToString();
                    update = dr["UPDATER"].ToString();
                    delete = dr["DELETER"].ToString();
                }
                dr.Close();
                if (con.State != ConnectionState.Closed)con.Close();

                ddlStatusEdit.Text = status;
                ddlInsertEdit.Text = insert;
                ddlUpdateEdit.Text = update;
                ddlDeleteEdit.Text = delete; 
            }
        }
        protected void gridViewAslRole_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn.aspx");
            }
            else
            {
                gridViewAslRole.EditIndex = -1;
                ShowGridForAslRole(); 
            }
        }

        protected void gridViewAslRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn.aspx");
            }
            else
            {
                gridViewAslRole.PageIndex = e.NewPageIndex;
                gridViewAslRole.DataBind();
                ShowGridForAslRole(); 
            }
        }


        protected void gridViewAslRole_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn.aspx");
            }
            else
            {
                DataTable dataTable = gridViewAslRole.DataSource as DataTable;

                if (dataTable != null)
                {
                    DataView dataView = new DataView(dataTable);
                    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

                    gridViewAslRole.DataSource = dataView;
                    gridViewAslRole.DataBind();
                    ShowGridForAslRole();
                }
            }
        }
        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        } 
         
    }
}