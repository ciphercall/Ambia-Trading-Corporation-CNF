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
    public partial class purchase_ : System.Web.UI.Page
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
                string formLink = "/stock/ui/purchase_";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        dbFunctions.dropDown_Bind(ddlStoreFr, "id", "select", "SELECT STORENM NM,STOREID ID FROM STK_STORE ORDER BY STORENM");
                        dbFunctions.dropDown_Bind(ddlStoreTo, "id", "select", "SELECT STORENM NM,STOREID ID FROM STK_STORE ORDER BY STORENM");
                        dbFunctions.dropDown_Bind(ddlJob, "", "select", "SELECT DISTINCT JOBNO NM FROM STK_TRANSMST WHERE TRANSTP='IORD' ORDER BY JOBNO");
                        txtDT.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        lblmYear.Text = dbFunctions.timezone(DateTime.Now).ToString("MMM-yy").ToUpper();
                        txtTransNo.Text = createTransno();
                        SHowData();
                        ddlStoreFr.Focus();
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
                transno = txtTransNo.Text;
            else
            {
                transno = ddlEditTransNo.Text;
                if (transno == "--SELECT--")
                    transno = "0";
            }
            SqlCommand cmd = new SqlCommand(@" SELECT        STK_ITEM.ITEMNM,STK_ITEM.COLOR, STK_TRANS.*
            FROM STK_ITEM INNER JOIN STK_TRANS ON STK_ITEM.ITEMID = STK_TRANS.ITEMID WHERE TRANSTP='BUY' AND TRANSNO='" + transno + "' AND TRANSMY='" + lblmYear.Text + "'", conn);
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
                TextBox txtLotFooter = (TextBox)gvDetails.FooterRow.FindControl("txtLotFooter");
                txtLotFooter.Text = txtTransNo.Text;
                dbFunctions.dropDown_Bind(ddlItemNMFooter, "id", "select", "SELECT ITEMNM NM,ITEMID ID FROM STK_ITEM ORDER BY ITEMNM");
                ddlItemNMFooter.Focus();
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
                gvDetails.Rows[0].Visible = false;
                TextBox txtLotFooter = (TextBox)gvDetails.FooterRow.FindControl("txtLotFooter");
                txtLotFooter.Text = txtTransNo.Text;
                DropDownList ddlItemNMFooter = (DropDownList)gvDetails.FooterRow.FindControl("ddlItemNMFooter");
                dbFunctions.dropDown_Bind(ddlItemNMFooter, "id", "select", "SELECT ITEMNM NM,ITEMID ID FROM STK_ITEM ORDER BY ITEMNM");
                ddlItemNMFooter.Focus();
            }
        }
        private string TransSL()
        {
            string trans = "";
            if (btnEdit.Text == "Edit ∓")
                trans = txtTransNo.Text;
            else
                trans = ddlEditTransNo.Text;
            string SL = dbFunctions.getData(@"SELECT MAX(TRANSSL) FROM STK_TRANS WHERE TRANSTP = 'BUY' and TRANSMY = '" + lblmYear.Text + "' AND TRANSNO='" + trans + "'");
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
            if (dbFunctions.checkParmit("/stock/ui/purchase_", "INSERTR") == true)
            {

                if (e.CommandName.Equals("AddNew"))
                {
                    TextBox txtFabricationFooter = (TextBox)gvDetails.FooterRow.FindControl("txtFabricationFooter");
                    DropDownList ddlItemNMFooter = (DropDownList)gvDetails.FooterRow.FindControl("ddlItemNMFooter");
                    TextBox txtLotFooter = (TextBox)gvDetails.FooterRow.FindControl("txtLotFooter");
                    TextBox txtQtyFooter = (TextBox)gvDetails.FooterRow.FindControl("txtQtyFooter");
                    TextBox txtRateFooter = (TextBox)gvDetails.FooterRow.FindControl("txtRateFooter");
                    TextBox txtAmountFooter = (TextBox)gvDetails.FooterRow.FindControl("txtAmountFooter");
                    TextBox txtRemarksFooter = (TextBox)gvDetails.FooterRow.FindControl("txtRemarksFooter");
                    if (txtDT.Text == "" || txtDT.Text.Length != 10)
                        dbFunctions.showMessage(lblErrorMaster, "Invalid Date.", "w", txtDT);
                    else if (ddlJob.Text == "--SELECT--")
                        dbFunctions.showMessage(lblErrorMaster, "Select Job No.", "w", ddlJob);
                    //else if (ddlStoreFr.Text == "--SELECT--")
                    //    dbFunctions.showMessage(lblErrorMaster, "Select department from.", "w", ddlStoreFr);
                    else if (ddlStoreTo.Text == "--SELECT--")
                        dbFunctions.showMessage(lblErrorMaster, "Select department to.", "w", ddlStoreTo);
                    else if (ddlItemNMFooter.Text == "--SELECT--")
                        dbFunctions.showMessage(lblErrorMaster, "Item name required.", "w", ddlItemNMFooter);
                    else if (dbFunctions.decimalConvert(txtQtyFooter.Text) < 1)
                        dbFunctions.showMessage(lblErrorMaster, "Qty can not be 0 or null.", "w", txtQtyFooter);
                    else if (dbFunctions.decimalConvert(txtRateFooter.Text) < 1)
                        dbFunctions.showMessage(lblErrorMaster, "Rate can not be 0 or null.", "w", txtRateFooter);
                    else
                    {
                        lblErrorMaster.Visible = false;
                        iob.userID = CookiesData["USERID"];
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        if (btnEdit.Text == "Edit ∓")
                            cmd = new SqlCommand("SELECT TRANSNO FROM STK_TRANSMST WHERE TRANSNO='" + txtTransNo.Text + "' and TRANSTP='BUY' AND TRANSMY='" + lblmYear.Text + "' AND USERID='" + iob.userID + "'", conn);
                        else
                            cmd = new SqlCommand("Select TRANSNO from STK_TRANSMST where TRANSNO='" + ddlEditTransNo.Text + "' and TRANSTP='BUY' AND TRANSMY='" + lblmYear.Text + "'", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        conn.Close();
                        iob.transMD = txtTransMd.Text;
                        iob.transTP = "BUY";
                        iob.transDT = dbFunctions.dateConvert(txtDT.Text);
                        iob.transMY = dbFunctions.dateConvert(txtDT.Text).ToString("MMM-yy").ToUpper();
                        iob.jobNo = ddlJob.Text;
                        iob.storeFrID = "";
                        iob.storeToID = ddlStoreTo.Text;
                        iob.lot = txtLot.Text;
                        iob.batch = txtBatch.Text;
                        iob.machine = txtMachine.Text;
                        iob.itemID = ddlItemNMFooter.Text;
                        iob.qty = dbFunctions.intConvert16(txtQtyFooter.Text);
                        iob.lotQty = dbFunctions.intConvert16(txtLotFooter.Text);
                        iob.unitTP = "";
                        iob.rate = dbFunctions.decimalConvert(txtRateFooter.Text);
                        iob.amount = dbFunctions.decimalConvert(txtAmountFooter.Text);
                        iob.remarks = txtRemarksFooter.Text;
                        iob.remarksMst = txtRemarks.Text;
                        iob.userPC = dbFunctions.userPc();
                        iob.ipaddress = dbFunctions.ipAddress();
                        iob.intime = dbFunctions.timezone(DateTime.Now);
                        iob.transSL = TransSL();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (btnEdit.Text == "Edit ∓")
                                iob.transno = txtTransNo.Text;
                            else
                                iob.transno = ddlEditTransNo.Text;
                            String s = dob.INSERT_TRANS(iob);
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

                            iob.transno = createTransno();

                            String r = dob.INSERT_TRANSMST(iob);
                            if (r == "true")
                            {
                                String s = dob.INSERT_TRANS(iob);
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
            if (dbFunctions.checkParmit("/stock/ui/purchase_", "UPDATER") == true)
            {
                gvDetails.EditIndex = e.NewEditIndex;
                SHowData();
                Label lblItemID = (Label)gvDetails.Rows[e.NewEditIndex].FindControl("lblItemID");
                DropDownList ddlItemNMEdit = (DropDownList)gvDetails.Rows[e.NewEditIndex].FindControl("ddlItemNMEdit");
                dbFunctions.dropDown_Bind(ddlItemNMEdit, "id", "select", "SELECT ITEMNM NM,ITEMID ID FROM STK_ITEM ORDER BY ITEMNM");
                ddlItemNMEdit.Text = lblItemID.Text;
                ddlItemNMEdit.Focus();
            }
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtFabricationEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtFabricationEdit");
            DropDownList ddlItemNMEdit = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("ddlItemNMEdit");
            TextBox txtLotEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtLotEdit");
            TextBox txtQtyEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtQtyEdit");
            TextBox txtRateEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRateEdit");
            TextBox txtAmountEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtAmountEdit");
            TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarksEdit");
            Label lblSLEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblSLEdit");
            if (txtDT.Text == "" || txtDT.Text.Length != 10)
                dbFunctions.showMessage(lblErrorMaster, "Invalid Date.", "w", txtDT);
            else if (ddlJob.Text == "--SELECT--")
                dbFunctions.showMessage(lblErrorMaster, "Select Job No.", "w", ddlJob);
            //else if (ddlStoreFr.Text == "--SELECT--")
            //    dbFunctions.showMessage(lblErrorMaster, "Select department from.", "w", ddlStoreFr);
            else if (ddlStoreTo.Text == "--SELECT--")
                dbFunctions.showMessage(lblErrorMaster, "Select department to.", "w", ddlStoreTo);
            else if (ddlItemNMEdit.Text == "--SELECT--")
                dbFunctions.showMessage(lblErrorMaster, "Item name required.", "w", ddlItemNMEdit);
            else if (dbFunctions.decimalConvert(txtQtyEdit.Text) < 1)
                dbFunctions.showMessage(lblErrorMaster, "Qty can not be 0 or null.", "w", txtQtyEdit);
            else if (dbFunctions.decimalConvert(txtRateEdit.Text) < 1)
                dbFunctions.showMessage(lblErrorMaster, "Rate can not be 0 or null.", "w", txtRateEdit);
            else
            {
                lblErrorMaster.Visible = false;
                lblErrorMSG.Visible = false;
                iob.sl = lblSLEdit.Text;
                iob.transMD = txtTransMd.Text;
                iob.transTP = "BUY";
                iob.transDT = dbFunctions.dateConvert(txtDT.Text);
                iob.transMY = lblmYear.Text;
                iob.jobNo = ddlJob.Text;
                iob.storeFrID = "";
                iob.storeToID = ddlStoreTo.Text;
                iob.lot = txtLot.Text;
                iob.batch = txtBatch.Text;
                iob.machine = txtMachine.Text;
                iob.itemID = ddlItemNMEdit.Text;
                iob.lotQty = dbFunctions.intConvert16(txtLotEdit.Text);
                iob.unitTP = "";
                iob.qty = dbFunctions.intConvert16(txtQtyEdit.Text);
                iob.rate = dbFunctions.decimalConvert(txtRateEdit.Text);
                iob.amount = dbFunctions.decimalConvert(txtAmountEdit.Text);
                iob.remarks = txtRemarksEdit.Text;
                iob.remarksMst = txtRemarks.Text;
                iob.userID = CookiesData["USERID"];
                iob.userPC = dbFunctions.userPc();
                iob.ipaddress = dbFunctions.ipAddress();
                iob.intime = dbFunctions.timezone(DateTime.Now);
                if (btnEdit.Text == "Edit ∓")
                    iob.transno = txtTransNo.Text;
                else
                    iob.transno = ddlEditTransNo.Text;
                String s = dob.UPDATE_TRANS(iob);
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
            lblErrorMaster.Visible = false;
            lblErrorMSG.Visible = false;
            if (dbFunctions.checkParmit("/stock/ui/purchase_", "DELETER") == true)
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
                        transno = txtTransNo.Text;
                    else
                        transno = ddlEditTransNo.Text;
                    string s = dbFunctions.execute("DELETE FROM STK_TRANS WHERE SL='" + lblSL.Text + "'");
                    string r = "";
                    if (s == "true")
                        r = dbFunctions.execute("DELETE FROM STK_TRANSMST WHERE TRANSTP='BUY' AND TRANSNO='" + transno + "' AND TRANSMY='" + lblmYear.Text + "'");
                    if (r == "true")
                    {
                        btnRefresh_Click(sender, e);
                        dbFunctions.showMessage(lblErrorMSG, "successfully deleted .", "s");
                    }
                    else
                        dbFunctions.showMessage(lblErrorMSG, "Something went wrong to delete execution", "e");
                    SHowData();
                }
            }
        }
        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblErrorMaster.Visible = false;
            lblErrorMSG.Visible = false;
            gvDetails.EditIndex = -1;
            SHowData();
        }
        private string createTransno()
        {
            string max = dbFunctions.getData("SELECT MAX(TRANSNO) FROM STK_TRANSMST WHERE TRANSTP='BUY' AND TRANSMY='" + lblmYear.Text + "'");
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
            lblErrorMaster.Visible = false;
            lblErrorMSG.Visible = false;
            if (btnEdit.Text == "Edit ∓")
            {
                lblmYear.Text = dbFunctions.dateConvert(txtDT.Text).ToString("MMM-yy");
                txtTransNo.Text = createTransno();
            }
            else
            {
                lblmYear.Text = dbFunctions.dateConvert(txtDT.Text).ToString("MMM-yy");
                dbFunctions.dropDown_Bind(ddlEditTransNo, "", "select", "SELECT DISTINCT TRANSNO NM FROM STK_TRANS WHERE TRANSDT ='" + dbFunctions.dateConvert(txtDT.Text) + "' AND TRANSMY='" + lblmYear.Text + "' and TRANSTP='SALE'");
                ddlEditTransNo.Focus();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblErrorMaster.Visible = false;
            lblErrorMSG.Visible = false;
            if (btnEdit.Text == "New ∓")
            {
                if (ddlEditTransNo.Text == "--SELECT--")
                    dbFunctions.showMessage(lblErrorMaster, "Select Invoice no.", "w", ddlEditTransNo);
                else if (ddlStoreTo.Text == "--SELECT--")
                    dbFunctions.showMessage(lblErrorMaster, "Select department to .", "w", ddlStoreTo);
                else if (ddlStoreFr.Text == "--SELECT--")
                    dbFunctions.showMessage(lblErrorMaster, "Select department from .", "w", ddlStoreFr);
                else if (ddlStoreTo.Text == "--SELECT--")
                    dbFunctions.showMessage(lblErrorMaster, "Select department to .", "w", ddlStoreTo);
                else
                {
                    iob.userID = CookiesData["USERID"];
                    iob.transTP = "BUY";
                    iob.transno = ddlEditTransNo.Text;
                    iob.transDT = dbFunctions.dateConvert(txtDT.Text);
                    iob.transMY = lblmYear.Text;
                    iob.jobNo = ddlJob.Text;
                    iob.storeFrID = ddlStoreFr.Text;
                    iob.storeToID = ddlStoreTo.Text;
                    iob.lot = txtLot.Text;
                    iob.batch = txtBatch.Text;
                    iob.transMD = txtTransMd.Text;
                    iob.machine = txtMachine.Text;
                    iob.remarksMst = txtRemarks.Text;
                    iob.userPC = dbFunctions.userPc();
                    iob.ipaddress = dbFunctions.ipAddress();
                    iob.intime = dbFunctions.timezone(DateTime.Now);
                    string s = dob.UPDATE_TRANSMST(iob);
                    if (s == "true")
                    {
                        string p = dbFunctions.execute("UPDATE STK_TRANS SET TRANSMD='" + txtTransMd.Text + "',STOREFR='" + ddlStoreFr.Text + "',STORETO='" + ddlStoreTo.Text + "' WHERE TRANSTP='BUY' AND TRANSNO='" + ddlEditTransNo.Text + "' AND TRANSMY='" + lblmYear.Text + "'");
                        dbFunctions.showMessage(lblErrorMaster, "Successfully updated in upper portion.", "S");
                    }
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ddlStoreFr.SelectedIndex = -1;
            ddlStoreTo.SelectedIndex = -1;
            txtRemarks.Text = "";
            ddlJob.SelectedIndex = -1;
            txtLot.Text = "";
            txtBatch.Text = "";
            txtMachine.Text = "";
            lblErrorMaster.Visible = false;
            lblErrorMSG.Visible = false;
            txtTransNo.Text = createTransno();
            SHowData();
            if (btnEdit.Text == "Edit ∓")
                ddlStoreFr.Focus();
            else
                ddlEditTransNo.Focus();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit ∓")
            {
                btnUpdate.Enabled = true;
                txtTransNo.Visible = false;
                btnEdit.Text = "New ∓";
                ddlEditTransNo.Visible = true;
                dbFunctions.dropDown_Bind(ddlEditTransNo, "", "select", "SELECT DISTINCT TRANSNO NM FROM STK_TRANSMST WHERE TRANSDT ='" + dbFunctions.dateConvertYMD(txtDT.Text) + "' AND TRANSMY='" + lblmYear.Text + "' and TRANSTP='BUY'");
                btnRefresh_Click(sender, e);
                SHowData();

            }
            else
            {
                btnUpdate.Enabled = false;
                txtTransNo.Visible = true;
                btnEdit.Text = "Edit ∓";
                ddlEditTransNo.Visible = false;
                btnRefresh_Click(sender, e);
                SHowData();
            }
        }

        protected void ddlEditOrderNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEditTransNo.Text != "--SELECT--")
                {
                    SqlConnection conn = new SqlConnection(dbFunctions.Connection);
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string script = "SELECT * FROM STK_TRANSMST WHERE TRANSNO='" + ddlEditTransNo.Text + "' AND TRANSMY='" + lblmYear.Text + "' AND TRANSTP='BUY'";
                    SqlCommand cmd = new SqlCommand(script, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ddlJob.Text = dr["JOBNO"].ToString();
                        ddlStoreFr.Text = dr["STOREFR"].ToString();
                        ddlStoreTo.Text = dr["STORETO"].ToString();
                        txtRemarks.Text = dr["REMARKS"].ToString();
                        txtLot.Text = dr["LOT"].ToString();
                        txtBatch.Text = dr["BATCH"].ToString();
                        txtMachine.Text = dr["MACHINE"].ToString(); ;
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

        protected void txtQtyFooter_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtQtyFooter = (TextBox)row.FindControl("txtQtyFooter");
            TextBox txtRateFooter = (TextBox)row.FindControl("txtRateFooter");
            TextBox txtAmountFooter = (TextBox)row.FindControl("txtAmountFooter");
            TextBox txtRemarksFooter = (TextBox)row.FindControl("txtRemarksFooter");
            decimal qty = dbFunctions.decimalConvert(txtQtyFooter.Text);
            decimal rate = dbFunctions.decimalConvert(txtRateFooter.Text);
            decimal amount = qty * rate;
            txtAmountFooter.Text = amount.ToString();
            txtRemarksFooter.Focus();
        }
        protected void txtQtyEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtQtyEdit = (TextBox)row.FindControl("txtQtyEdit");
            TextBox txtRateEdit = (TextBox)row.FindControl("txtRateEdit");
            TextBox txtAmountEdit = (TextBox)row.FindControl("txtAmountEdit");
            TextBox txtRemarksEdit = (TextBox)row.FindControl("txtRemarksEdit");
            decimal qty = dbFunctions.decimalConvert(txtQtyEdit.Text);
            decimal rate = dbFunctions.decimalConvert(txtQtyEdit.Text);
            decimal amount = qty * rate;
            txtAmountEdit.Text = amount.ToString();
            txtRemarksEdit.Focus();
        }
        protected void txtRateFooter_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtQtyFooter = (TextBox)row.FindControl("txtQtyFooter");
            TextBox txtRateFooter = (TextBox)row.FindControl("txtRateFooter");
            TextBox txtAmountFooter = (TextBox)row.FindControl("txtAmountFooter");
            TextBox txtRemarksFooter = (TextBox)row.FindControl("txtRemarksFooter");
            decimal qty = dbFunctions.decimalConvert(txtQtyFooter.Text);
            decimal rate = dbFunctions.decimalConvert(txtRateFooter.Text);
            decimal amount = qty * rate;
            txtAmountFooter.Text = amount.ToString();
            txtRemarksFooter.Focus();
        }
        protected void txtRateEdit_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtQtyEdit = (TextBox)row.FindControl("txtQtyEdit");
            TextBox txtRateEdit = (TextBox)row.FindControl("txtRateEdit");
            TextBox txtAmountEdit = (TextBox)row.FindControl("txtAmountEdit");
            TextBox txtRemarksEdit = (TextBox)row.FindControl("txtRemarksEdit");
            decimal qty = dbFunctions.decimalConvert(txtQtyEdit.Text);
            decimal rate = dbFunctions.decimalConvert(txtQtyEdit.Text);
            decimal amount = qty * rate;
            txtAmountEdit.Text = amount.ToString();
            txtRemarksEdit.Focus();
        }
    }
}