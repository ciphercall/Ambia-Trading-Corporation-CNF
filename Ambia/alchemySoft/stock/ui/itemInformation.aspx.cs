using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft.stock.dataAccess;
using alchemySoft.stock.model;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

namespace alchemySoft.stock.ui
{
    public partial class itemInformation : System.Web.UI.Page
    {
        data_Access dob = new data_Access();
        models iob = new models();
        SqlConnection con = new SqlConnection(dbFunctions.Connection);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null || !dbFunctions.permit())
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string formLink = "/stock/ui/itemInformation";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        txtCategoryNM.Focus();
                        lblCatID.Text = "";
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionCOLORList(string prefixText, int count, string contextKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT TOP 10 COLOR FROM STK_ITEM WHERE COLOR like '" + prefixText + "%' ORDER BY COLOR", conn);
            SqlDataReader oReader;
            conn.Open();
            List<String> CompletionSet = new List<string>();
            oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (oReader.Read())
                CompletionSet.Add(oReader["COLOR"].ToString());
            conn.Close();
            return CompletionSet.ToArray();
        }

        protected void txtCategoryNM_TextChanged(object sender, EventArgs e)
        {
            string check = dbFunctions.getDataParemeter("SELECT CATID FROM STK_ITEMMST WHERE CATNM=@TXT", txtCategoryNM.Text);
            lblCatID.Text = check;
            if (check == "")
                Search.Text = "Create";
            else
                Search.Text = "Search";

        }
        private string itemIdCreate()
        {
            dbFunctions.lblAdd(@"select MAX(ITEMID) from STK_ITEM where CATID = '" + lblCatID.Text + "'", lblIMaxItemID);
            string ItemCD;
            string mxCD, OItemCD, mid, subItemCD;
            int subCD, incrItCD;
            if (lblIMaxItemID.Text == "")
            {
                ItemCD = lblCatID.Text + "0001";
            }
            else
            {
                mxCD = lblIMaxItemID.Text;
                OItemCD = mxCD.Substring(4, 4);
                subCD = int.Parse(OItemCD);
                incrItCD = subCD + 1;
                if (incrItCD < 10)
                {
                    mid = incrItCD.ToString();
                    subItemCD = "000" + mid;
                }
                else if (incrItCD < 100)
                {
                    mid = incrItCD.ToString();
                    subItemCD = "00" + mid;
                }
                else if (incrItCD < 1000)
                {
                    mid = incrItCD.ToString();
                    subItemCD = "0" + mid;
                }
                else
                    subItemCD = incrItCD.ToString();

                ItemCD = lblCatID.Text + subItemCD;
            }
            return ItemCD;
        }
        private string catIDcreate()
        {
            string catID = "", maxID = "";
            string check = dbFunctions.getData("SELECT MAX(CATID) FROM STK_ITEMMST");
            if (check == "")
            {
                catID = "001"; 
            }
            else
            {
                maxID = check;
                int CID = int.Parse(maxID.Substring(1, 3)) + 1;
                if (CID < 10)
                    catID = "00" + CID;
                else if (CID < 100)
                    catID = "0" + CID;
                else
                    catID = CID.ToString();
            }
            return "I" + catID.ToString(); ;
        }
        protected void BindEmployeeDetails()
        {
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT CATID, ITEMID, COLOR,ITEMCD, ITEMNM, BUYRT, SALERT,MINSQTY,REMARKS 
            FROM dbo.STK_ITEM  WHERE CATID='" + lblCatID.Text + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                TextBox txtItemNMFooter = (TextBox)gvDetails.FooterRow.FindControl("txtItemNMFooter");
                txtItemNMFooter.Focus();
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
                TextBox txtItemNMFooter = (TextBox)gvDetails.FooterRow.FindControl("txtItemNMFooter");
                txtItemNMFooter.Focus();
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            string s = dbFunctions.getDataParemeter("SELECT CATID FROM STK_ITEMMST WHERE CATNM=@TXT",txtCategoryNM.Text);
            if (s == "")
            {
                lblCatID.Text = catIDcreate();
                string USERID = CookiesData["USERID"].ToString();
                iob.userID = USERID;
                iob.catID = lblCatID.Text;
                iob.catName = txtCategoryNM.Text;
                iob.ipaddress = dbFunctions.ipAddress();
                iob.intime = dbFunctions.timezone(DateTime.Now);
                iob.userPC = dbFunctions.userPc();
                string x = dob.INSERT_ITEMMST(iob);
                if (x == "true")
                    BindEmployeeDetails();
            }else
                BindEmployeeDetails();


        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (dbFunctions.checkParmit("/stock/ui/itemInformation", "INSERTR") == true)
            {

                TextBox txtItemNMFooter = (TextBox)gvDetails.FooterRow.FindControl("txtItemNMFooter");
                TextBox txtBuyRT = (TextBox)gvDetails.FooterRow.FindControl("txtBuyRT");
                TextBox txtSaleRT = (TextBox)gvDetails.FooterRow.FindControl("txtSaleRT");
                TextBox txtItemCDFooter = (TextBox)gvDetails.FooterRow.FindControl("txtItemCDFooter");
                TextBox txtColorFooter = (TextBox)gvDetails.FooterRow.FindControl("txtColorFooter");
                TextBox txtMinsQtyFooter = (TextBox)gvDetails.FooterRow.FindControl("txtMinsQtyFooter");
                TextBox txtRemarksFooter = (TextBox)gvDetails.FooterRow.FindControl("txtRemarksFooter");
                if (e.CommandName.Equals("AddNew"))
                {
                    iob.itemID = itemIdCreate();
                    iob.itemNM = txtItemNMFooter.Text;
                    iob.itemCD = txtItemCDFooter.Text;
                    iob.catID = lblCatID.Text;
                    iob.color = txtColorFooter.Text;
                    iob.buyRT = dbFunctions.decimalConvert(txtBuyRT.Text);
                    iob.saleRT = dbFunctions.decimalConvert(txtSaleRT.Text);
                    iob.minStk = dbFunctions.intConvert16(txtMinsQtyFooter.Text);
                    iob.remarks = txtRemarksFooter.Text;
                    iob.userID = CookiesData["USERID"];
                    iob.userPC = dbFunctions.userPc();
                    iob.ipaddress = dbFunctions.ipAddress();
                    iob.intime = dbFunctions.timezone(DateTime.Now);
                    String s = dob.INSERT_ITEM(iob);
                    if (s == "true")
                    {
                        dbFunctions.showMessage(lblErrorMSG, "Successfully inserted.", "s");
                        BindEmployeeDetails();
                    }
                    else
                    {
                        dbFunctions.showMessage(lblErrorMSG, "Somthing went wrong", "e");
                    }

                }
            }
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (dbFunctions.checkParmit("/stock/ui/itemInformation", "UPDATER") == true)
            {
                gvDetails.EditIndex = e.NewEditIndex;
                BindEmployeeDetails();
                TextBox txtItemNMEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtItemNMEdit");
                txtItemNMEdit.Focus();
            }
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label lblItemID = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblItemID");
            TextBox txtItemNMEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtItemNMEdit");
            TextBox txtItemCDEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtItemCDEdit");
            TextBox txtBuyRTEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtBuyRTEdit");
            TextBox txtSaleRTEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtSaleRTEdit");
            TextBox txtColorEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtColorEdit");
            TextBox txtMinsQtyEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtMinsQtyEdit");
            TextBox txtRemarksEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtRemarksEdit");

            iob.itemID = lblItemID.Text;
            iob.itemNM = txtItemNMEdit.Text;
            iob.itemCD = txtItemCDEdit.Text;
            iob.catID = lblCatID.Text;
            iob.color = txtColorEdit.Text;
            iob.buyRT = dbFunctions.decimalConvert(txtBuyRTEdit.Text);
            iob.saleRT = dbFunctions.decimalConvert(txtSaleRTEdit.Text);
            iob.minStk = dbFunctions.intConvert16(txtMinsQtyEdit.Text);
            iob.remarks = txtRemarksEdit.Text;
            iob.userID = CookiesData["USERID"];
            iob.userPC = dbFunctions.userPc();
            iob.ipaddress = dbFunctions.ipAddress();
            iob.intime = dbFunctions.timezone(DateTime.Now);

            // logdata add start //
            //Label lblLogData = new Label();
            //string lotileng = iob.LatiLongTudeInsert;
            //dbFunctions.lblAdd(@"SELECT CATID+' '+ITEMID+' '+PARTNO+' '+ITEMNM+' '+ISNULL(BRAND,'NULL')+' '+ISNULL(IMGURL,'NULL')+' '+UNIT+' '+ISNULL(CONVERT(NVARCHAR(20),BUYRT),'NULL')
            //                    +' '+ISNULL(CONVERT(NVARCHAR(20),SALERT),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),NETWT),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),GROSSWT),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),MINSQTY),'NULL')
            //                    +' '+ISNULL(USERPC,'NULL')+' '+ISNULL(USERID,'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),ACTDTI),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),INTIME),'NULL')
            //                    +' '+ISNULL(IPADDRESS,'NULL')+' '+ISNULL(INSLTUDE,'NULL')+' '+ISNULL(UPDUSERPC,'NULL')+' '+ISNULL(UPDUSERID,'NULL')
            //                    +' '+ISNULL(CONVERT(NVARCHAR(20),UPDTIME),'NULL')+' '+ISNULL(UPDIPADDRESS,'NULL')+' '+ISNULL(UPDLTUDE,'NULL')
            //                    FROM STK_ITEM WHERE CATID = '" + lblCatID.Text + "' and ITEMID = '" + lblItemID.Text + "'", lblLogData);
            //string logid = "UPDATE";
            //string tableid = "STK_ITEM";
            //dbFunctions.insertLogData(lotileng, logid, tableid, lblLogData.Text);

            string s = dob.UPDATE_ITEM(iob);
            if (s == "true")
            {
                dbFunctions.showMessage(lblErrorMSG, "Successfully updated.", "s");
                gvDetails.EditIndex = -1;
                BindEmployeeDetails();
            }
            else
            {
                dbFunctions.showMessage(lblErrorMSG, "Somthing went wrong", "e");
            }
        }
        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dbFunctions.checkParmit("/stock/ui/itemInformation", "DELETER") == true)
            {
                Label lblItemID = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblItemID");
                dbFunctions.lblAdd(@"SELECT ITEMID from STK_TRANS where ITEMID = '" + lblItemID.Text + "'", lblChkItemID);
                if (lblChkItemID.Text == "")
                {
                    lblErrorMSG.Visible = false;
                    // logdata add start //
                    //Label lblLogData = new Label();
                    //string lotileng = iob.LatiLongTudeInsert;
                    //dbFunctions.lblAdd(@"SELECT CATID+' '+ITEMID+' '+PARTNO+' '+ITEMNM+' '+ISNULL(BRAND,'NULL')+' '+ISNULL(IMGURL,'NULL')+' '+UNIT+' '+ISNULL(CONVERT(NVARCHAR(20),BUYRT),'NULL')
                    //            +' '+ISNULL(CONVERT(NVARCHAR(20),SALERT),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),NETWT),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),GROSSWT),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),MINSQTY),'NULL')
                    //            +' '+ISNULL(USERPC,'NULL')+' '+ISNULL(USERID,'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),ACTDTI),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),INTIME),'NULL')
                    //            +' '+ISNULL(IPADDRESS,'NULL')+' '+ISNULL(INSLTUDE,'NULL')+' '+ISNULL(UPDUSERPC,'NULL')+' '+ISNULL(UPDUSERID,'NULL')
                    //            +' '+ISNULL(CONVERT(NVARCHAR(20),UPDTIME),'NULL')+' '+ISNULL(UPDIPADDRESS,'NULL')+' '+ISNULL(UPDLTUDE,'NULL')
                    //            FROM STK_ITEM WHERE CATID = '" + lblCatID.Text + "' and ITEMID = '" + lblItemID.Text + "'", lblLogData);
                    //string logid = "DELETE";
                    //string tableid = "STK_ITEM";
                    //dbFunctions.insertLogData(lotileng, logid, tableid, lblLogData.Text);
                    // logdata add start //

                    if (gvDetails.Rows.Count > 1)
                    {
                        string s = dbFunctions.execute("DELETE FROM STK_ITEM WHERE ITEMID='" + lblItemID.Text + "'");
                        if (s == "true")
                        {
                            dbFunctions.showMessage(lblErrorMSG, "Successfully deleted.", "s");
                            BindEmployeeDetails();
                        }
                        else
                        {
                            dbFunctions.showMessage(lblErrorMSG, "Somthing went wrong", "e");
                        }
                    }
                    else
                    {
                        lblChkCatID.Text = "";


                        string s = dbFunctions.execute("DELETE FROM STK_ITEM WHERE ITEMID='" + lblItemID.Text + "'");

                        // logdata add start //
                        //Label lblLogData2 = new Label();
                        //string lotileng2 = iob.LatiLongTudeInsert;
                        //dbFunctions.lblAdd(@"SELECT CATID+' '+CATNM+' '+ISNULL(USERPC,'NULL')+' '+ISNULL(USERID,'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),ACTDTI),'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),INTIME),'NULL')
                        //                +' '+ISNULL(IPADDRESS,'NULL')+' '+ISNULL(UPDUSERPC,'NULL')+' '+ISNULL(CONVERT(NVARCHAR(20),UPDTIME),'NULL')+' '+ISNULL(UPDIPADDRESS,'NULL')
                        //                FROM STK_ITEMMST WHERE CATID = '" + lblCatID.Text + "'", lblLogData2);
                        //string logid2 = "DELETE";
                        //string tableid2 = "STK_ITEMMST";
                        //dbFunctions.insertLogData(lotileng2, logid2, tableid2, lblLogData2.Text);
                        // logdata add start //

                        string r = "";
                        if (s == "true")
                            r = dbFunctions.execute("DELETE FROM STK_ITEMMST WHERE CATID='" + lblCatID.Text + "'");
                        if (r == "true")
                        {
                            dbFunctions.showMessage(lblErrorMSG, "Successfully deleted.", "s");
                        }
                        else
                            dbFunctions.showMessage(lblErrorMSG, "Somthing went wrong", "e");
                        txtCategoryNM.Text = "";
                        txtCategoryNM.Focus();
                        BindEmployeeDetails();
                    }
                }
                else
                    dbFunctions.showMessage(lblErrorMSG, "This Item has a Transaction..", "w");
            }
        }
        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            BindEmployeeDetails();
        }
        protected void txtItemNMFooter_TextChanged(object sender, EventArgs e)
        {
            lblErrorMSG.Visible = false;
            TextBox txtItemCDFooter = (TextBox)gvDetails.FooterRow.FindControl("txtItemCDFooter");
            txtItemCDFooter.Focus();
        }


    }
}