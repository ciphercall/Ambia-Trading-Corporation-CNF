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

    public partial class CostPool : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string formLink = "/Accounts/UI/CostPool.aspx";
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

        //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        //public static string[] GetCompletionList(string prefixText, int count, string contextKey)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(dbFunctions.Connection);
        //    SqlCommand cmd = new SqlCommand("SELECT CATNM FROM GL_COSTPMST WHERE CATNM like '" + prefixText + "%'", conn);
        //    SqlDataReader oReader;
        //    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
        //    List<String> CompletionSet = new List<string>();
        //    oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    while (oReader.Read())
        //        CompletionSet.Add(oReader["CATNM"].ToString());
        //    return CompletionSet.ToArray();
        //}

        protected void txtCategoryNM_TextChanged(object sender, EventArgs e)
        {
            lblCatID.Text = "";
            lblMaxCatID.Text = "";
            dbFunctions.lblAdd(@"select CATID from GL_COSTPMST where CATNM='" + txtCategoryNM.Text + "'", lblCatID);
            dbFunctions.lblAdd(@"select max(CATID) from GL_COSTPMST", lblMaxCatID);
            if (lblCatID.Text == "")
            {
                if (lblMaxCatID.Text == "")
                {
                    lblCatID.Text = "C01";
                }
                else
                {
                    string MaxCatId = lblMaxCatID.Text;
                    string CatId = MaxCatId.Substring(1, 2);
                    string mid, C_ID;
                    int ID = int.Parse(CatId);
                    int CID = ID + 1;
                    if (CID < 10)
                    {
                        mid = CID.ToString();
                        C_ID = "0" + mid;
                    }
                    else
                        C_ID = CID.ToString();
                    string FID = "C" + C_ID.ToString();
                    lblCatID.Text = FID;
                }
            }
            else
            {

            }
            Search.Focus();
            //BindEmployeeDetails();
        }

        protected void ShowCostPoolDesc()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT CATID, COSTPID, COSTPNM, REMARKS, USERID, USERPC, INTIME, IPADDRSS FROM GL_COSTP WHERE CATID='" + lblCatID.Text + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                TextBox txtCOSTPNM = (TextBox)gvDetails.FooterRow.FindControl("txtCOSTPNM");
                txtCOSTPNM.Focus();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
                //int columncount = gvDetails.Rows[0].Cells.Count;
                //gvDetails.Rows[0].Cells.Clear();
                //gvDetails.Rows[0].Cells.Add(new TableCell());
                //gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                //gvDetails.Rows[0].Cells[0].Text = "No Records Found";
                gvDetails.Rows[0].Visible = false;
                TextBox txtCOSTPNM = (TextBox)gvDetails.FooterRow.FindControl("txtCOSTPNM");
                txtCOSTPNM.Focus();
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("Select CATID from GL_COSTPMST  where CATID='" + lblCatID.Text + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ShowCostPoolDesc();
            }
            else
            {
                string userName = CookiesData["USERID"].ToString();

                string query = "";
                SqlCommand comm = new SqlCommand(query, conn);

                query = ("insert into GL_COSTPMST (CATID, CATNM, USERPC, USERID,IPADDRESS) " +
                               "values('" + lblCatID.Text + "','" + txtCategoryNM.Text + "','',@USERID,'')");

                comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@USERID", userName);

                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                int result = comm.ExecuteNonQuery();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                ShowCostPoolDesc();
            }
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                dbFunctions.lblAdd(@"select MAX(COSTPID) from GL_COSTP where CATID = '" + lblCatID.Text + "'", lblIMaxItemID);
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
                    OItemCD = mxCD.Substring(3, 4);
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
                e.Row.Cells[0].Text = lblCatID.Text;
                e.Row.Cells[1].Text = ItemCD;
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string userName = CookiesData["USERID"].ToString();
            string ipAddress = CookiesData["IpAddress"].ToString();
            string PCName = CookiesData["PCName"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string query = "";
            SqlCommand comm = new SqlCommand(query, conn);



            if (e.CommandName.Equals("AddNew"))
            {
                string CatID, CostPID;
                CatID = gvDetails.FooterRow.Cells[0].Text;
                CostPID = gvDetails.FooterRow.Cells[1].Text;
                TextBox txtCOSTPNM = (TextBox)gvDetails.FooterRow.FindControl("txtCOSTPNM");
                TextBox txtREMARKS = (TextBox)gvDetails.FooterRow.FindControl("txtREMARKS");

                query = ("insert into GL_COSTP ( CATID, COSTPID, COSTPNM, REMARKS, USERID, USERPC, IPADDRSS) " +
                         "values(@CatID,@CostPID,'" + txtCOSTPNM.Text + "','" + txtREMARKS.Text + "',@USERID,'" + PCName + "','" + ipAddress + "')");

                comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@CatID", CatID);
                comm.Parameters.AddWithValue("@CostPID", CostPID);
                comm.Parameters.AddWithValue("@USERID", userName);

                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                int result = comm.ExecuteNonQuery();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                ShowCostPoolDesc();
            }
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            ShowCostPoolDesc();
            TextBox txtCOSTPNMEdit = (TextBox)gvDetails.Rows[e.NewEditIndex].FindControl("txtCOSTPNMEdit");
            txtCOSTPNMEdit.Focus();
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            string userName = CookiesData["USERID"].ToString();
            string ipAddress = CookiesData["IpAddress"].ToString();
            string PCName = CookiesData["PCName"].ToString();

            Label lblCatGIDEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblCatGIDEdit");
            Label lblCOSTPIDEdit = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblCOSTPIDEdit");
            TextBox txtCOSTPNMEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtCOSTPNMEdit");

            TextBox txtREMARKSEdit = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtREMARKSEdit");

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand("update GL_COSTP set COSTPNM='" + txtCOSTPNMEdit.Text + "', REMARKS = '" + txtREMARKSEdit.Text + "', USERID = '" + userName + "' where CATID = '" + lblCatGIDEdit.Text + "' and COSTPID = '" + lblCOSTPIDEdit.Text + "'", conn);
            cmd.ExecuteNonQuery();
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            gvDetails.EditIndex = -1;
            ShowCostPoolDesc();
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            Label lblCatGID = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblCatGID");
            Label lblCOSTPID = (Label)gvDetails.Rows[e.RowIndex].FindControl("lblCOSTPID");

            dbFunctions.lblAdd(@"select COSTPID from GL_MTRANS where COSTPID = '" + lblCOSTPID.Text + "'", lblChkItemID);

            int result = 0;

            if (lblChkItemID.Text == "")
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                SqlCommand cmd = new SqlCommand("delete FROM GL_COSTP where CATID = '" + lblCatGID.Text + "' and COSTPID = '" + lblCOSTPID.Text + "'", conn);
                result = cmd.ExecuteNonQuery();
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            }

            else
            {
                dbFunctions.showMessage(Page,"This Item has a Transaction.");
            }

            if (result == 1)
            {
                ShowCostPoolDesc();
            }
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            ShowCostPoolDesc();
        }
    }
}