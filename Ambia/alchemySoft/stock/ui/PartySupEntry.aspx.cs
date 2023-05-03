using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Configuration;
using DynamicMenu;
using System.Data.SqlClient; 
using alchemySoft;
using alchemySoft.stock.dataAccess;
using alchemySoft.stock.model;

namespace DynamicMenu.Stock.UI
{
    public partial class PartySupEntry : System.Web.UI.Page
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
                string formLink = "/stock/ui/PartySupEntry";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!Page.IsPostBack)
                    {
                        ddlPSTP.Focus();
                        txtSNM.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            } 
        }

        protected void ddlPSTP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPSTP.Text == "P")
            {
                txtPNM.Visible = true;
                txtSNM.Visible = false;
                txtPNM.Focus();
            }
            else if (ddlPSTP.Text == "S")
            {
                txtPNM.Visible = false;
                txtSNM.Visible = true;
                txtSNM.Focus();
            }
        }

        //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        //public static string[] GetCompletionListParty(string prefixText, int count, string contextKey)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    // Try to use parameterized inline query/sp to protect sql injection
        //    SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) IN ('10202') and ACCOUNTNM LIKE '" + prefixText + "%' AND STATUSCD = 'P'", conn);
        //    SqlDataReader oReader;
        //    conn.Open();
        //    List<String> CompletionSet = new List<string>();
        //    oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    while (oReader.Read())
        //        CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
        //    return CompletionSet.ToArray();
        //}
        //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        //public static string[] GetCompletionListSuppliar(string prefixText, int count, string contextKey)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    // Try to use parameterized inline query/sp to protect sql injection
        //    SqlCommand cmd = new SqlCommand("SELECT ACCOUNTNM FROM GL_ACCHART WHERE substring(ACCOUNTCD,1,5) in ('20202') and ACCOUNTNM LIKE '" + prefixText + "%' AND STATUSCD = 'P'", conn);
        //    SqlDataReader oReader;
        //    conn.Open();
        //    List<String> CompletionSet = new List<string>();
        //    oReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    while (oReader.Read())
        //        CompletionSet.Add(oReader["ACCOUNTNM"].ToString());
        //    return CompletionSet.ToArray();
        //}

        protected void txtPNM_TextChanged(object sender, EventArgs e)
        {
            dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where ACCOUNTNM = '" + txtPNM.Text + "'", txtPSCD);
            txtCity.Focus();
        }

        protected void txtSNM_TextChanged(object sender, EventArgs e)
        {
            dbFunctions.txtAdd(@"Select ACCOUNTCD from GL_ACCHART where ACCOUNTNM = '" + txtSNM.Text + "'", txtPSCD);
            txtCity.Focus();
        }

        protected void txtCity_TextChanged(object sender, EventArgs e)
        {
            txtAddress.Focus();
        }

        protected void txtAddress_TextChanged(object sender, EventArgs e)
        {
            txtContact.Focus();
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtWebID.Focus();
        }

        protected void txtContact_TextChanged(object sender, EventArgs e)
        {
            txtEmail.Focus();
        }

        protected void txtWebID_TextChanged(object sender, EventArgs e)
        {
            txtCPNM.Focus();
        }

        protected void txtCPNM_TextChanged(object sender, EventArgs e)
        {
            txtCPNO.Focus();
        }

        protected void txtCPNO_TextChanged(object sender, EventArgs e)
        {
            txtRemarks.Focus();
        }

        protected void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            ddlStatus.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string userName = HttpContext.Current.Session["UserName"].ToString();
            if (txtPSCD.Text == "")
            {
                Response.Write("<script>alert('Please Select Party or Suppliar.');</script>");
            }
            else
            {
                try
                {
                    iob.Pstp = ddlPSTP.Text;
                    iob.Pscd = txtPSCD.Text;
                    iob.City = txtCity.Text;
                    iob.Address = txtAddress.Text;
                    iob.Contactno = txtContact.Text;
                    iob.Email = txtEmail.Text;
                    iob.Webid = txtWebID.Text;
                    iob.Cpnm = txtCPNM.Text;
                    iob.Cpno = txtCPNO.Text;
                    iob.remarks = txtRemarks.Text;
                    iob.Status = ddlPSTP.Text;
                    iob.Username = userName;
                    dbFunctions.lblAdd(@"select max(PS_ID) from  STK_PS where PSTP = '" + ddlPSTP.Text + "'", lblPS_ID);
                    Int64 ps;
                    if (lblPS_ID.Text == "")
                    {
                        ps = 1;
                    }
                    else
                    {
                        Int64 ps_id = Convert.ToInt64(lblPS_ID.Text);
                        ps = ps_id + 1;
                    }
                    iob.Ps_ID = ps.ToString();
                    dob.insertPS(iob);

                    Refresh();
                    ddlPSTP.Focus();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        public void Refresh()
        {
            txtPNM.Text = "";
            txtSNM.Text = "";
            txtPSCD.Text = "";
            txtCity.Text = "";
            txtAddress.Text = "";
            txtContact.Text = "";
            txtCPNM.Text = "";
            txtCPNO.Text = "";
            txtEmail.Text = "";
            txtRemarks.Text = "";
            txtWebID.Text = "";
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}