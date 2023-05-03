using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft;
using Microsoft.Ajax.Utilities;

namespace alchemySoft.Asl.UI
{
    public partial class MenuCreate : System.Web.UI.Page
    {
        IFormatProvider dateormat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        DataAccess.ASLDataAccess dob = new DataAccess.ASLDataAccess();
        Interface.ASLInterface iob = new Interface.ASLInterface();
        SqlConnection con = new SqlConnection(dbFunctions.Connection);
        protected void Page_Load(object sender, EventArgs e)
        {
            string TP = CookiesData["USERTYPE"].ToString();
            if (TP == "SUPERADMIN" || TP == "COMPADMIN")
            {
                if (!IsPostBack)
                {
                    if (CookiesData == null || !dbFunctions.permit())
                    {
                        Response.Redirect("~/login/ui/SignIn.aspx");
                    }
                    else if (CookiesData["USERTYPE"].ToString() == "SUPERADMIN")
                    {
                        txtModuleName.Focus(); 
                    }
                    else
                    {
                        //Response.Redirect("~/login/ui/signin.aspx");
                    }
                }
            }
            else
            {
                Response.Redirect("default");
            }
           
        }

        public string ModuleNameAdd()
        {
            string checkResult = "";
            if (txtModuleName.Text == "")
            {
                checkResult = "false"; 
                dbFunctions.popupAlert(Page, " Select Module Name.", "w");
            }
            else
            {
                string datacheck = dbFunctions.getData("SELECT MODULENM FROM ASL_MENUMST WHERE MODULENM='" + txtModuleName.Text + "'");
                if (datacheck == "")
                {
                    //IPHostEntry host = new IPHostEntry();
                  // string  host = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.ToUpper();
                    

                    iob.ipAddressInsert = dbFunctions.ipAddress();
                    iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
                    //iob.userPcInsert = dbFunctions.userPc();
                    iob.userPcInsert = dbFunctions.userPc();
                    iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);

                    iob.ModuleId = ModuleId();
                    iob.ModuleName = txtModuleName.Text.Trim();

                    dob.INSERT_ASL_MENUMST(iob);
                }
            }
            return checkResult;
        }

        public string ModuleId()
        {
            string moduleId = "";
            string maxDbModuleId = dbFunctions.getData("SELECT MAX(MODULEID) AS MODULEID FROM ASL_MENUMST");
            if (maxDbModuleId == "")
                moduleId = "01";
            else
            {
                int maxid = Convert.ToInt16(maxDbModuleId);
                maxid++;
                if (maxid < 10)
                    moduleId = "0" + maxid;
                else
                    moduleId = maxid.ToString();
            }
            return moduleId;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtModuleName.Text != "")
            {
                string checkModuleNmFromDb =
                    dbFunctions.getData("SELECT MODULENM FROM ASL_MENUMST WHERE MODULENM='" + txtModuleName.Text + "'");
                if (checkModuleNmFromDb == "")
                {
                    ModuleNameAdd(); 
                    dbFunctions.popupAlert(Page, " Module Create Succesfully.", "s");
                }
                else
                { 
                    dbFunctions.popupAlert(Page, "Module Already Created. Add menu Name.", "w");
                }
                Session["ModuleId"] =
                   dbFunctions.getData("SELECT MODULEID FROM ASL_MENUMST WHERE MODULENM='" + txtModuleName.Text + "'");

                GridShowForMenu();
            }
            else
            { 
                 
                dbFunctions.popupAlert(Page, " Write a Module Name.", "w");
                txtModuleName.Focus();
            }
        }



        public string MenuId(string moduleid)
        {
            string menuid = "";
            string maxMunuIdFromDb = dbFunctions.getData("SELECT MAX(MENUID) FROM ASL_MENU WHERE MODULEID='" + moduleid + "'");
            if (maxMunuIdFromDb == "")
                menuid = moduleid + "01";
            else
            {
                string menuidwithoutmodule = maxMunuIdFromDb.Substring(2, 2);
                int menuidconvert = Convert.ToInt16(menuidwithoutmodule);
                menuidconvert++;
                if (menuidconvert < 10)
                    menuid = moduleid + "0" + menuidconvert;
                else
                    menuid = moduleid + menuidconvert;
            }
            return menuid;
        }
        public void MenuNameAdd(string moduleid)
        {
            iob.ipAddressInsert = dbFunctions.ipAddress();
            iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
            iob.userPcInsert = dbFunctions.userPc();
            iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);

            iob.ModuleId = moduleid;
            iob.MenuId = MenuId(moduleid);
            //iob.MenuName = txtMenuName.Text.Trim();
            //iob.MenuType = ddlMenuType.SelectedValue;
            //iob.MenuLink = txtMenuLink.Text;

            string s = dob.INSERT_ASL_MENU(iob);
            if (s == "")
            {
                MenuAddInAslRoleForAllUser(iob.ModuleId, iob.MenuId, iob.MenuType);
                // MenuActiveForAllCompanyAdmin(iob.ModuleId, iob.MenuId, iob.MenuType);
            }
        }

        public void MenuAddInAslRoleForAllUser(string moduleId, string menuId, string menutp)
        {
            TextBox txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
            iob.LotiLengTudeInsert = txtLotiLongTude.Text;
            iob.ipAddressInsert = dbFunctions.ipAddress();
            iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
            iob.userPcInsert = dbFunctions.userPc();
            iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);
            iob.ModuleId = moduleId;
            iob.MenuId = menuId;
            iob.MenuType = menutp;
            iob.Status = "I";
            iob.InsertRole = "I";
            iob.UpdateRole = "I";
            iob.DeleteRole = "I";
            if (con.State != ConnectionState.Open) con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT SUBSTRING(CONVERT(NVARCHAR,USERID),1,3) AS CMPID, USERID 
FROM ASL_USERCO WHERE SUBSTRING(CONVERT(NVARCHAR,USERID),1,3) !='100' AND SUBSTRING(CONVERT(NVARCHAR,USERID),4,2)='01'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iob.CompanyId = Convert.ToInt16(dr["CMPID"].ToString());
                iob.CompanyUserId = Convert.ToInt16(dr["USERID"].ToString());
                dob.INSERT_ASL_ROLE(iob);
            }
            dr.Close();
            if (con.State != ConnectionState.Closed)con.Close();
        }

        public void MenuActiveForAllCompanyAdmin(string moduleId, string menuId, string menutp)
        {
            TextBox txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
            iob.LotiLengTudeUpdate = txtLotiLongTude.Text;
            iob.ipAddressUpdate = dbFunctions.ipAddress();
            iob.UserIdUpdate = Convert.ToInt64(CookiesData["USERID"].ToString());
            iob.InTimeUpdate = dbFunctions.timezone(DateTime.Now);
            iob.ModuleId = moduleId;
            iob.MenuId = menuId;
            iob.MenuType = menutp;
            iob.Status = "A";
            iob.InsertRole = "A";
            iob.UpdateRole = "A";
            iob.DeleteRole = "A";

            if (con.State != ConnectionState.Open) con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT COMPID, CONVERT(NVARCHAR,COMPID)+'01' AS USERID 
                FROM ASL_COMPANY WHERE COMPID!='100'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iob.CompanyId = Convert.ToInt16(dr["COMPID"].ToString());
                iob.CompanyUserId = Convert.ToInt16(dr["USERID"].ToString());
                dob.UPDATE_ASL_ROLE(iob);
            }
            dr.Close();
            if (con.State != ConnectionState.Closed)con.Close();
        }

        protected void txtModuleName_TextChanged(object sender, EventArgs e)
        {
            string checkModuleNmFromDb =
                   dbFunctions.getData("SELECT MODULENM FROM ASL_MENUMST WHERE MODULENM='" + txtModuleName.Text + "'");
            if (checkModuleNmFromDb == "")
            {  
                dbFunctions.popupAlert(Page, " Module name not present.", "w");
            }
            else
            {
                Session["ModuleId"] =
                   dbFunctions.getData("SELECT MODULEID FROM ASL_MENUMST WHERE MODULENM='" + txtModuleName.Text + "'"); 
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            iob.ipAddressInsert = dbFunctions.ipAddress();
            iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
            iob.userPcInsert = dbFunctions.userPc();
            iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);

            var ddlMenuType = (DropDownList)gvDetails.FooterRow.FindControl("ddlMenuType");
            var txtMenuName = (TextBox)gvDetails.FooterRow.FindControl("txtMenuName");
            var txtMenuLink = (TextBox)gvDetails.FooterRow.FindControl("txtMenuLink");
            var txtMenuSerial = (TextBox)gvDetails.FooterRow.FindControl("txtMenuSerial");

            if (e.CommandName.Equals("AddNew"))
            {
                if (ddlMenuType.SelectedValue == "SELECT")
                { 
                    dbFunctions.popupAlert(Page, " Select from type.", "w");
                    ddlMenuType.Focus();
                }
                else if (txtMenuName.Text == "")
                { 
                    dbFunctions.popupAlert(Page, " Insert menu name.", "w");
                    txtMenuName.Focus();
                }
                else if (txtMenuLink.Text == "")
                { 
                    dbFunctions.popupAlert(Page, " Insert menu link.", "w");
                    txtMenuLink.Focus();
                }
                else if (txtMenuSerial.Text == "")
                { 
                    dbFunctions.popupAlert(Page, " Insert menu Serial.", "w");
                    txtMenuSerial.Focus();
                }
                else
                { 
                    string moduleId = Session["ModuleId"].ToString();
                    iob.ModuleId = moduleId;
                    iob.MenuId = MenuId(iob.ModuleId);
                    iob.MenuName = txtMenuName.Text.Trim();
                    iob.MenuType = ddlMenuType.SelectedValue;
                    iob.MenuLink = txtMenuLink.Text;
                    iob.MenuSerial = Convert.ToInt16(txtMenuSerial.Text);
                    string s = dob.INSERT_ASL_MENU(iob);
                    if (s == "")
                    {
                        MenuAddInAslRoleForAllUser(iob.ModuleId, iob.MenuId, iob.MenuType);
                    }
                    GridShowForMenu();

                }
            }

        }
        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }


        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }
            else
            {
                gvDetails.EditIndex = e.NewEditIndex;
                GridShowForMenu();
                var lblModuleIdEdit = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblModuleIdEdit");
                var lblMenuIdEdit = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblMenuIdEdit");
                var ddlMenuTypeEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlMenuTypeEdit");

                ddlMenuTypeEdit.SelectedValue = dbFunctions.getData(@"SELECT MENUTP FROM ASL_MENU 
                WHERE MODULEID=" + lblModuleIdEdit.Text + " AND MENUID=" + lblMenuIdEdit.Text + "");
                ddlMenuTypeEdit.Focus();
            }
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }
            else
            {
                iob.UserIdUpdate = Convert.ToInt64(CookiesData["USERID"].ToString());
                iob.ipAddressUpdate =  CookiesData["ipAddress"].ToString();
                iob.InTimeUpdate = dbFunctions.timezone(DateTime.Now);

                var lblModuleIdEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblModuleIdEdit");
                var lblMenuIdEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblMenuIdEdit");

                var ddlMenuTypeEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlMenuTypeEdit");
                var txtMenuNameEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtMenuNameEdit");
                var txtMenuLinkEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtMenuLinkEdit");
                var txtMenuSerialEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtMenuSerialEdit");


                if (ddlMenuTypeEdit.SelectedValue == "SELECT")
                { 
                    dbFunctions.popupAlert(Page, " Select menu type.", "w");
                    ddlMenuTypeEdit.Focus();
                }
                else if (txtMenuNameEdit.Text == "")
                { 
                    dbFunctions.popupAlert(Page, "Insert menu name.", "w");
                    txtMenuNameEdit.Focus();
                }
                else if (txtMenuLinkEdit.Text == "")
                { 
                    dbFunctions.popupAlert(Page, "Insert menu link.", "w");
                    txtMenuLinkEdit.Focus();
                }
                else if (txtMenuSerialEdit.Text == "")
                { 
                    dbFunctions.popupAlert(Page, "Insert menu serial.", "w");
                    txtMenuSerialEdit.Focus();
                }
                else
                {
                    iob.ModuleId = lblModuleIdEdit.Text;
                    iob.MenuId = lblMenuIdEdit.Text;

                    iob.MenuType = ddlMenuTypeEdit.SelectedValue;
                    iob.MenuName = txtMenuNameEdit.Text;
                    iob.MenuLink = txtMenuLinkEdit.Text;
                    iob.MenuSerial = Convert.ToInt16(txtMenuSerialEdit.Text);

                    string s = dob.UPDATE_ASL_MENU(iob);
                    if (s == "")
                    { 
                        dbFunctions.popupAlert(Page, "Menu changed succesfully.", "s");
                        gvDetails.EditIndex = -1;
                        GridShowForMenu();
                    }
                    else
                    { 
                        dbFunctions.popupAlert(Page, "Failed for change. please check the required fields", "w");
                    }

                }
            }
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }
            else
            {
                gvDetails.EditIndex = -1;
                GridShowForMenu();
            }
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }
            else
            {
                var lblModuleId = (Label) gvDetails.Rows[e.RowIndex].FindControl("lblModuleId");
                var lblMenuId = (Label) gvDetails.Rows[e.RowIndex].FindControl("lblMenuId");

                string menuLeft =
                    dbFunctions.getData("SELECT COUNT(*) CNT FROM ASL_MENU WHERE MODULEID=" + lblModuleId.Text + "");
                iob.MenuId = lblMenuId.Text;
                iob.ModuleId = lblModuleId.Text;
                string s = "";
                if (menuLeft == "0")
                {
                    s = dob.DELETE_ASL_MENU(iob);
                    s = dob.DELETE_ASL_MENUMST(iob);
                }
                else
                {
                    s = dob.DELETE_ASL_MENU(iob);
                }
                if (s == "")
                {
                    gvDetails.EditIndex = -1;
                    GridShowForMenu(); 
                    dbFunctions.popupAlert(Page, "Succesfully Deleted .", "s");
                }
            }
        }

        protected void GridShowForMenu()
        {

            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            if (conn.State != ConnectionState.Open)conn.Open();
            string moduleId = Session["ModuleId"].ToString();
            SqlCommand cmd = new SqlCommand("SELECT MODULEID, MENUID, MENUTP, MENUNM, FLINK, MENUSL  FROM ASL_MENU WHERE MODULEID=" + moduleId + " ORDER BY MENUSL", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != ConnectionState.Closed)conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();

                var ddlMenuType = (DropDownList)gvDetails.FooterRow.FindControl("ddlMenuType");
                ddlMenuType.Focus();
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
                gvDetails.Rows[0].Cells[0].Text = "No menu found";
                var ddlMenuType = (DropDownList)gvDetails.FooterRow.FindControl("ddlMenuType");
                ddlMenuType.Focus();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            dbFunctions.popupAlert(Page,txtModuleName.Text,"w");
        }
    }
}
