using alchemySoft.stock.dataAccess;
using alchemySoft.stock.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.stock.ui
{
    public partial class work_order : System.Web.UI.Page
    {
        data_Access dob = new data_Access();
        models iob = new models();
        SqlConnection conn = new SqlConnection(dbFunctions.Connection);

        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string formLink = "/stock/ui/work_order";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        dbFunctions.dropDown_Bind(ddlPartyNM, "id", "select", "SELECT ACCOUNTNM NM,ACCOUNTCD ID FROM GL_ACCHART WHERE SUBSTRING(ACCOUNTCD,1,5) IN ('10202','20202') AND STATUSCD = 'P'");
                        txtDT.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        lblmYear.Text= dbFunctions.timezone(DateTime.Now).ToString("MMM-yy");
                        txtOrderNo.Text = createOrder();
                        SHowData();
                        ddlPartyNM.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }
        protected void SHowData()
        {
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            conn.Open();
            string transno = "";
            if (btnEdit.Text == "Edit ∓")
                transno = txtOrderNo.Text;
            else
            {
                transno = ddlEditOrderNo.Text;
                if (transno == "--SELECT--")
                    transno = "0";
            }
            SqlCommand cmd = new SqlCommand(@" SELECT        STK_ITEM.ITEMNM,STK_ITEM.COLOR, STK_TRANS.*
FROM        STK_ITEM INNER JOIN STK_TRANS ON STK_ITEM.ITEMID = STK_TRANS.ITEMID WHERE TRANSTP='IORD' AND TRANSNO='" + transno+"' AND TRANSMY='"+lblmYear.Text+"'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind(); 
                //NamingContainer return the container that the control sits in 
                DropDownList ddlItemNMFooter = (DropDownList)gvDetails.FooterRow.FindControl("ddlItemNMFooter");
                dbFunctions.dropDown_Bind(ddlItemNMFooter,"id","select","SELECT ITEMNM+' | '+COLOR NM,ITEMID ID FROM STK_ITEM ORDER BY ITEMNM");
                TextBox txtFabricationFooter = (TextBox)gvDetails.FooterRow.FindControl("txtFabricationFooter");
                txtFabricationFooter.Focus();
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
                gvDetails.Rows[0].Visible=false;
                DropDownList ddlItemNMFooter = (DropDownList)gvDetails.FooterRow.FindControl("ddlItemNMFooter");
                dbFunctions.dropDown_Bind(ddlItemNMFooter, "id", "select", "SELECT ITEMNM+' | '+COLOR NM,ITEMID ID FROM STK_ITEM ORDER BY ITEMNM");
                TextBox txtFabricationFooter = (TextBox)gvDetails.FooterRow.FindControl("txtFabricationFooter");
                txtFabricationFooter.Focus();
            }
        }
        private string TransSL()
        {
            string trans = "";
            if (btnEdit.Text == "Edit ∓")
                trans = txtOrderNo.Text;
            else
                trans = ddlEditOrderNo.Text;
            string SL = dbFunctions.getData(@"SELECT MAX(TRANSSL) FROM STK_TRANS WHERE TRANSTP = 'IORD' and TRANSMY = '" + lblmYear.Text + "' AND TRANSNO='" + trans + "'");
            if (SL == "")
                SL = "1";
            else
            {
                SL = (int.Parse(SL) + 1).ToString();
            }
            return SL;
        }
        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (dbFunctions.checkParmit("/stock/ui/work_order", "INSERTR") == true)
            {

                if (e.CommandName.Equals("AddNew"))
                {
                    TextBox txtFabricationFooter = (TextBox)gvDetails.FooterRow.FindControl("txtFabricationFooter");
                    DropDownList ddlItemNMFooter = (DropDownList)gvDetails.FooterRow.FindControl("ddlItemNMFooter");
                    TextBox txtStyleFooter = (TextBox)gvDetails.FooterRow.FindControl("txtStyleFooter");
                    TextBox txtFinishQtyFooter = (TextBox)gvDetails.FooterRow.FindControl("txtFinishQtyFooter");
                    TextBox txtWpcntFooter = (TextBox)gvDetails.FooterRow.FindControl("txtWpcntFooter");
                    TextBox txtYarnFooter = (TextBox)gvDetails.FooterRow.FindControl("txtYarnFooter");
                    TextBox txtTtlPxFooter = (TextBox)gvDetails.FooterRow.FindControl("txtTtlPxFooter");
                    TextBox txtRemarksFooter = (TextBox)gvDetails.FooterRow.FindControl("txtRemarksFooter"); 
                    if (ddlItemNMFooter.Text == "--SELECT--")
                        dbFunctions.showMessage(lblErrorMaster, "Item name required.", "w", ddlItemNMFooter);
                    else if (dbFunctions.decimalConvert(txtFinishQtyFooter.Text) < 1)
                        dbFunctions.showMessage(lblErrorMaster, "Finish qty can not be 0 or null.", "w", txtFinishQtyFooter);
                    else
                    {

                        iob.userID = CookiesData["USERID"];
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        if (btnEdit.Text == "Edit ∓")
                            cmd = new SqlCommand("SELECT TRANSNO FROM STK_TRANSMST WHERE TRANSNO='" + txtOrderNo.Text + "' and TRANSTP='IORD' AND TRANSMY='" + lblmYear.Text + "' AND USERID='" + iob.userID + "'", conn);
                        else
                            cmd = new SqlCommand("Select TRANSNO from STK_TRANSMST where TRANSNO='" + ddlEditOrderNo.Text + "' and TRANSTP='IORD' AND TRANSMY='" + lblmYear.Text + "'", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        conn.Close();
                        iob.transMD = "";
                        iob.transTP = "IORD";
                        iob.transDT = dbFunctions.dateConvert(txtDT.Text);
                        iob.transMY = dbFunctions.dateConvert(txtDT.Text).ToString("MMM-yy").ToUpper();
                        iob.jobNo = txtJobNo.Text;
                        iob.psID = ddlPartyNM.Text;
                        iob.fabric = txtFabricationFooter.Text;
                        iob.itemID = ddlItemNMFooter.Text; 
                        iob.style = txtStyleFooter.Text;
                        iob.qty = dbFunctions.intConvert16(txtWpcntFooter.Text);
                        iob.wpcnt = dbFunctions.decimalConvert(txtWpcntFooter.Text);
                        iob.yarn = txtYarnFooter.Text;
                        iob.ttlPx = dbFunctions.decimalConvert(txtTtlPxFooter.Text);
                        iob.remarks = txtRemarksFooter.Text;
                        iob.remarksMst = txtRemarks.Text;
                        iob.userPC = dbFunctions.userPc();
                        iob.ipaddress = dbFunctions.ipAddress();
                        iob.intime = dbFunctions.timezone(DateTime.Now);
                        iob.transSL = TransSL();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (btnEdit.Text == "Edit ∓")
                                iob.transno = txtOrderNo.Text;
                            else
                                iob.transno =ddlEditOrderNo.Text;
                            String s = dob.INSERT_TRANS_WORK(iob);
                            if (s == "true")
                            {
                                dbFunctions.showMessage(lblErrorMSG, "Successfully inserted.", "s");
                                SHowData();
                            }
                            else
                            {
                                dbFunctions.showMessage(lblErrorMSG, "Something went wrong to insert execution", "e");
                            }
                        }
                        else
                        {

                            iob.transno = createOrder();

                            String r = dob.INSERT_TRANSMST_WORK(iob);
                            if (r == "true")
                            {
                                String s = dob.INSERT_TRANS_WORK(iob);
                                if (s == "true")
                                {
                                    dbFunctions.showMessage(lblErrorMSG, "Successfully inserted.", "s");
                                    SHowData();
                                }
                                else
                                    dbFunctions.showMessage(lblErrorMSG, "Something went wrong to insert execution", "e");
                            }
                        }
                    }
                }
            }
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (dbFunctions.checkParmit("/stock/ui/work_order", "UPDATER") == true)
            {
                gvDetails.EditIndex = e.NewEditIndex;
                SHowData();
                Label lblItemID = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblItemID");
                DropDownList ddlItemNMEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlItemNMEdit");
                dbFunctions.dropDown_Bind(ddlItemNMEdit, "id", "select", "SELECT ITEMNM+' | '+COLOR NM,ITEMID ID FROM STK_ITEM ORDER BY ITEMNM");
                ddlItemNMEdit.Text = lblItemID.Text; 
                TextBox txtFabricationEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtFabricationEdit");
                txtFabricationEdit.Focus();
            }
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtFabricationEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtFabricationEdit");
            DropDownList ddlItemNMEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlItemNMEdit");
            TextBox txtStyleEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtStyleEdit");
            TextBox txtFinishQtyEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtFinishQtyEdit");
            TextBox txtWpcntEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtWpcntEdit");
            TextBox txtYarnEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtYarnEdit");
            TextBox txtTtlPxEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtTtlPxEdit");
            TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarksEdit");
            if (ddlItemNMEdit.Text == "--SELECT--")
                dbFunctions.showMessage(lblErrorMaster, "Item name required.", "w", ddlItemNMEdit);
            else if (dbFunctions.decimalConvert(txtFinishQtyEdit.Text) < 1)
                dbFunctions.showMessage(lblErrorMaster, "Finish qty can not be 0 or null.", "w", txtFinishQtyEdit);
            else
            { 
                iob.userID = CookiesData["USERID"];
                iob.transTP = "IORD";
                iob.transMD = "";
                iob.transDT = dbFunctions.dateConvert(txtDT.Text);
                iob.transMY = lblmYear.Text;
                iob.jobNo = txtJobNo.Text;
                iob.psID = ddlPartyNM.Text;
                iob.fabric = txtFabricationEdit.Text;
                iob.itemID = ddlItemNMEdit.Text;
                iob.style = txtStyleEdit.Text;
                iob.qty = dbFunctions.intConvert16(txtWpcntEdit.Text);
                iob.wpcnt = dbFunctions.decimalConvert(txtWpcntEdit.Text);
                iob.yarn = txtYarnEdit.Text;
                iob.ttlPx = dbFunctions.decimalConvert(txtTtlPxEdit.Text);
                iob.remarks = txtRemarksEdit.Text;
                iob.remarksMst = txtRemarks.Text;
                iob.userPC = dbFunctions.userPc();
                iob.ipaddress = dbFunctions.ipAddress();
                iob.intime = dbFunctions.timezone(DateTime.Now);
                if (btnEdit.Text == "Edit ∓")
                    iob.transno = Convert.ToInt16(txtOrderNo.Text).ToString();
                else
                    iob.transno = Convert.ToInt16(ddlEditOrderNo.Text).ToString();
                String s = dob.UPDATE_TRANS_WORK(iob);
                if (s == "true")
                {
                    dbFunctions.showMessage(lblErrorMSG, "Successfully Updated.", "s");
                    gvDetails.EditIndex = -1;
                    SHowData();
                }
                else
                    dbFunctions.showMessage(lblErrorMSG, "Something went wrong to update execution", "e");
            }
        }
        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dbFunctions.checkParmit("/stock/ui/work_order", "DELETER") == true)
            {
                Label lblSL = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSL"); 
                lblErrorMSG.Visible = false; 
                if (gvDetails.Rows.Count > 1)
                {
                    string s = dbFunctions.execute("DELETE FROM STK_TRANS WHERE SL='" + lblSL.Text + "'");
                    if (s == "true")
                    {
                        dbFunctions.showMessage(lblErrorMSG, "One row deleted.", "s");
                        SHowData();
                    }
                    else
                    {
                        dbFunctions.showMessage(lblErrorMSG, "Something went wrong to delete execution", "e");
                    }
                }
                else
                {
                    string transno = "";
                    if (btnEdit.Text == "Edit ∓")
                        transno = txtOrderNo.Text;
                    else
                        transno = ddlEditOrderNo.Text;
                    string s = dbFunctions.execute("DELETE FROM STK_TRANS WHERE SL='" + lblSL.Text + "'");
                    string r = "";
                    if (s == "true")
                        r = dbFunctions.execute("DELETE FROM STK_TRANSMST WHERE TRANSTP='IORD' AND TRANSNO='"+ transno + "' AND TRANSMY='"+lblmYear.Text+"'");
                    if (r == "true")
                    {
                        dbFunctions.showMessage(lblErrorMSG, "Order successfully deleted .", "s");
                        btnRefresh_Click(sender,e);
                    }
                    else
                        dbFunctions.showMessage(lblErrorMSG, "Something went wrong to delete execution", "e");
                    SHowData();
                }
            }
        }
        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            SHowData();
        }
        private  string createOrder()
        {
            string max = dbFunctions.getData("SELECT MAX(TRANSNO) FROM STK_TRANSMST WHERE TRANSTP='IORD' AND TRANSMY='" + lblmYear.Text + "'");
            if (max == "")
                max = "1";
            else
            {
                int transno = Convert.ToInt16(max) + 1;
                max = transno.ToString();
            }
            return max;
        }
        protected void txtDT_TextChanged(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit ∓")
            {
                lblmYear.Text = dbFunctions.dateConvert(txtDT.Text).ToString("MMM-yy");
                txtOrderNo.Text = createOrder();
            }
            else
            {
                lblmYear.Text = dbFunctions.dateConvert(txtDT.Text).ToString("MMM-yy");
                dbFunctions.dropDown_Bind(ddlEditOrderNo, "", "select", "SELECT DISTINCT TRANSNO NM FROM STK_TRANS WHERE TRANSDT ='" + dbFunctions.dateConvert(txtDT.Text) + "' AND TRANSMY='" + lblmYear.Text + "' and TRANSTP='SALE'");
                ddlEditOrderNo.Focus();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "New ∓")
            {
                if (ddlEditOrderNo.Text == "--SELECT--")
                    dbFunctions.showMessage(lblErrorMaster, "Select Order no.", "w", ddlEditOrderNo);
                else if (ddlPartyNM.Text == "--SELECT--")
                    dbFunctions.showMessage(lblErrorMaster, "Select Party .", "w", ddlPartyNM);
                else
                {
                    iob.userID = CookiesData["USERID"];
                    iob.transTP = "IORD";
                    iob.transno = ddlEditOrderNo.Text;
                    iob.transDT = dbFunctions.dateConvert(txtDT.Text);
                    iob.transMY = lblmYear.Text;
                    iob.jobNo = txtJobNo.Text;
                    iob.psID = ddlPartyNM.Text;
                    iob.remarksMst = txtRemarks.Text;
                    iob.userPC = dbFunctions.userPc();
                    iob.ipaddress = dbFunctions.ipAddress();
                    iob.intime = dbFunctions.timezone(DateTime.Now);
                    string s = dob.UPDATE_TRANSMST_WORK(iob);
                    if (s == "true")
                    {
                        string p = dbFunctions.execute("UPDATE STK_TRANS SET PSID='" + ddlPartyNM.Text + "' WHERE TRANSTP='IORD' AND TRANSNO='" + ddlEditOrderNo.Text + "' AND TRANSMY='" + lblmYear.Text + "'");
                        dbFunctions.showMessage(lblErrorMaster, "Successfully updated in upper portion.", "S");
                    }
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ddlPartyNM.SelectedIndex = -1;
            txtRemarks.Text = "";
            txtJobNo.Text = "";
            lblErrorMaster.Visible = false;
            lblErrorMSG.Visible = false;
            txtOrderNo.Text = createOrder();
            SHowData();
            if (btnEdit.Text == "Edit ∓")
                ddlPartyNM.Focus();
            else
                ddlEditOrderNo.Focus();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit ∓")
            {
                txtOrderNo.Visible = false;
                btnEdit.Text = "New ∓"; 
                ddlEditOrderNo.Visible = true;
                dbFunctions.dropDown_Bind(ddlEditOrderNo,"","select" ,"SELECT DISTINCT TRANSNO NM FROM STK_TRANSMST WHERE TRANSDT ='" + dbFunctions.dateConvertYMD(txtDT.Text) + "' AND TRANSMY='" + lblmYear.Text + "' and TRANSTP='IORD'");
                btnRefresh_Click(sender,e); 
                SHowData();

            }
            else
            {
                txtOrderNo.Visible = true;
                btnEdit.Text = "Edit ∓";
                ddlEditOrderNo.Visible = false;
                btnRefresh_Click(sender, e);
                SHowData();
            }
        }

        protected void ddlEditOrderNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEditOrderNo.Text != "--SELECT--")
                {
                    SqlConnection conn = new SqlConnection(dbFunctions.Connection);
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string script = "SELECT * FROM STK_TRANSMST WHERE TRANSNO='"+ddlEditOrderNo.Text+"' AND TRANSMY='"+lblmYear.Text+"' AND TRANSTP='IORD'";
                    SqlCommand cmd = new SqlCommand(script, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtJobNo.Text = dr["JOBNO"].ToString();
                        ddlPartyNM.Text = dr["PSID"].ToString();
                        txtRemarks.Text = dr["REMARKS"].ToString();
                        SHowData();
                    }
                    dr.Close();
                    if (conn.State != ConnectionState.Closed) conn.Close();
                }
            }
            catch
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
            }
        }

        protected void ddlItemNMFooter_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            //NamingContainer return the container that the control sits in
            DropDownList ddlItemNMFooter = (DropDownList)row.FindControl("ddlItemNMFooter");
            TextBox txtStyleFooter = (TextBox)row.FindControl("txtStyleFooter");
            if (ddlItemNMFooter.Text != "--SELECT--")
            {
                Label lblColorFooter = (Label)row.FindControl("lblColorFooter");
                string color = dbFunctions.splitText(ddlItemNMFooter.SelectedItem.ToString(), '|', 1).TrimStart();
                lblColorFooter.Text = color;
                txtStyleFooter.Focus();
            } 
        }

        protected void ddlItemNMEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            //NamingContainer return the container that the control sits in
            TextBox ddlItemNMEdit = (TextBox)row.FindControl("ddlItemNMEdit");
            TextBox txtStyleEdit = (TextBox)row.FindControl("txtStyleEdit");
            if (ddlItemNMEdit.Text != "--SELECT--")
            {
                Label lblColorEdit = (Label)row.FindControl("lblColorEdit");
                string color = dbFunctions.splitText(ddlItemNMEdit.Text, '|', 1).TrimStart();
                lblColorEdit.Text = color;
                txtStyleEdit.Focus();
            }
        }

        protected void txtWpcntEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtWpcntEdit = (TextBox)row.FindControl("txtWpcntEdit");
            TextBox txtYarnEdit = (TextBox)row.FindControl("txtYarnEdit");
            TextBox txtFinishQtyEdit = (TextBox)row.FindControl("txtFinishQtyEdit");
            int qty = dbFunctions.intConvert16(txtFinishQtyEdit.Text);
            int wpcnt = dbFunctions.intConvert16(txtWpcntEdit.Text);
            txtYarnEdit.Text = (((qty * wpcnt) / 100) + qty).ToString();
        }

        protected void txtWpcntFooter_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtWpcntFooter = (TextBox)row.FindControl("txtWpcntFooter");
            TextBox txtYarnFooter = (TextBox)row.FindControl("txtYarnFooter");
            TextBox txtFinishQtyFooter = (TextBox)row.FindControl("txtFinishQtyFooter");
            int qty = dbFunctions.intConvert16(txtFinishQtyFooter.Text);
            int wpcnt = dbFunctions.intConvert16(txtWpcntFooter.Text);
            int yarn = dbFunctions.intConvert16(txtYarnFooter.Text);
            txtYarnFooter.Text = (((qty * wpcnt) / 100) + qty).ToString();
        }
    }
}