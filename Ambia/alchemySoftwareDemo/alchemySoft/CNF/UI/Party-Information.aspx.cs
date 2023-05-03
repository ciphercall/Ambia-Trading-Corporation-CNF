using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft.CNF.DataAccess;
using alchemySoft.CNF.Interface;
using alchemySoft.LogIn;

namespace alchemySoft.CNF.UI
{
    public partial class Party_Information : System.Web.UI.Page
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
                        txtPartynm.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        private bool Previousdata(string id)
        {
            bool bflag = false;
            DataTable table = new DataTable();

            try
            {
                table = dob.ShowpartyInfo(id);
            }
            catch (Exception ex)
            {
                table = null;
                Response.Write(ex.Message);
            }
            if (table != null)
            {
                if (table.Rows.Count > 0)
                    bflag = true;
            }
            return bflag;
        }


        protected void txtParty_TextChanged(object sender, EventArgs e)
        {

            lblerrmsg.Visible = false;

            dbFunctions.txtAdd("SELECT ACCOUNTCD FROM GL_ACCHART WHERE ACCOUNTNM='" + txtPartynm.Text + "' AND STATUSCD ='P'",
                txtPartyID);

            if (Previousdata(txtPartyID.Text))
            {
                //dbFunctions.txtAdd("SELECT ADDRESS FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtaddress);
                //dbFunctions.txtAdd("SELECT CONTACTNO FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtcontact);
                //dbFunctions.txtAdd("SELECT EMAILID FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtEmail);
                //dbFunctions.txtAdd("SELECT WEBID FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtwebadd);
                //dbFunctions.txtAdd("SELECT APNM FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtAPname);
                //dbFunctions.txtAdd("SELECT APNO FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtapcontact);
                //dbFunctions.txtAdd("SELECT LOGINID FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtlogmail);
                //dbFunctions.txtAdd("SELECT LOGINPW FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "' ", txtpassword);

                SqlConnection con = new SqlConnection(dbFunctions.Connection);
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM CNF_PARTY WHERE PARTYID='" + txtPartyID.Text + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtaddress.Text = dr["ADDRESS"].ToString();
                    txtcontact.Text = dr["CONTACTNO"].ToString();
                    txtEmail.Text = dr["EMAILID"].ToString();
                    txtwebadd.Text = dr["WEBID"].ToString();
                    txtAPname.Text = dr["APNM"].ToString();
                    txtapcontact.Text = dr["APNO"].ToString();
                    txtlogmail.Text = dr["LOGINID"].ToString();
                    txtpassword.Text = dr["LOGINPW"].ToString();
                    ddlstatus.SelectedValue = dr["STATUS"].ToString();
                }
                dr.Close();
                con.Close();
            }
            else
            {
                txtaddress.Text = "";
                txtcontact.Text = "";
                txtEmail.Text = "";
                txtwebadd.Text = "";
                txtAPname.Text = "";
                txtapcontact.Text = "";
                txtlogmail.Text = "";
                txtpassword.Text = "";
                lblerrmsg.Visible = false;

                txtaddress.Focus();
            }
        }


        protected void txtPartyID_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {

            if (Previousdata(txtPartyID.Text))
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Party Name Already Exist";
                txtPartynm.Focus();
            }
            else
            {
                try
                {
                    //iob.PartyNanme = txtPartynm.Text;
                    iob.PartyID = txtPartyID.Text;
                    iob.Address = txtaddress.Text;
                    iob.Contact = txtcontact.Text;
                    iob.Email = txtEmail.Text;
                    iob.Web = txtwebadd.Text;
                    iob.APName = txtAPname.Text;
                    iob.APContact = txtapcontact.Text;
                    iob.Status = ddlstatus.Text;
                    iob.Logid = txtlogmail.Text.Trim();
                    iob.Logpw = txtpassword.Text.Trim();

                    iob.InTime = DateTime.Now;
                    iob.UpdateTime = DateTime.Now;
                    iob.Userpc = CookiesData["PCName"].ToString();
                    iob.IPAddress = CookiesData["ipAddress"].ToString();

                    lblerrmsg.Text = "";

                    dob.CreateParty(iob);


                    Refresh();

                    lblerrmsg.Text = "Added your record succesfully.";
                    lblerrmsg.Visible = true;
                    lblerrmsg.ForeColor = Color.Green;
                    txtPartynm.Focus();
                    //}


                }

                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

        }


        public void Refresh()
        {
            txtPartynm.Text = "";
            txtPartyID.Text = "";
            txtaddress.Text = "";
            txtcontact.Text = "";
            txtEmail.Text = "";
            txtwebadd.Text = "";
            txtAPname.Text = "";
            txtapcontact.Text = "";
            txtlogmail.Text = "";
            txtpassword.Text = "";
            ddlstatus.SelectedIndex = -1;

            lblerrmsg.Visible = false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //iob.PartyNanme = txtPartynm.Text;
                iob.PartyID = txtPartyID.Text;
                iob.Address = txtaddress.Text;
                iob.Contact = txtcontact.Text;
                iob.Email = txtEmail.Text;
                iob.Web = txtwebadd.Text;
                iob.APName = txtAPname.Text;
                iob.APContact = txtapcontact.Text;
                iob.Status = ddlstatus.Text;
                iob.Logid = txtlogmail.Text;
                iob.Logpw = txtpassword.Text;

                iob.InTime = DateTime.Now;
                iob.UpdateTime = DateTime.Now;
                iob.Userpc = CookiesData["PCName"].ToString();
                iob.IPAddress = CookiesData["ipAddress"].ToString();


                try
                {
                    // logdata add start //
                    string lotileng = CookiesData["LOCATION"].ToString();
                    string ipAddress = CookiesData["ipAddress"].ToString();
                    string logdata = dbFunctions.StringData(@"SELECT 'PARTYID : ' + PARTYID+' | '+'ADDRESS : ' + ISNULL(ADDRESS,'(NULL)')+' | '+'CONTACTNO :
' + ISNULL(CONTACTNO,'(NULL)')+' | '+'EMAILID : ' + ISNULL(EMAILID,'(NULL)')+' | '+'WEBID : ' + ISNULL(WEBID,'(NULL)')+' | '+'APNM :
' + ISNULL(APNM,'(NULL)')+' | '+'APNO : ' + ISNULL(APNO,'(NULL)')+' | '+'STATUS : ' + ISNULL(CONVERT(NVARCHAR(50),STATUS,103),'(NULL)')+' | '+'USERPC : 
' + ISNULL(USERPC,'(NULL)')+' | '+'INTIME : ' + ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+' | '+'UPDATETIME :
' + ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+' | '+'IPADDRESS : ' + ISNULL(IPADDRESS,'(NULL)')+' | '+'LOGINID : ' + ISNULL(LOGINID,'(NULL)')+' | '+'LOGINPW :
' + ISNULL(LOGINPW,'(NULL)')+' | ' FROM CNF_PARTY where PARTYID='" + iob.PartyID + "'");
                    /*SELECT PARTYID+'  '+ISNULL(ADDRESS,'(NULL)')+'  '+ISNULL(CONTACTNO,'(NULL)')+'  '+
                        ISNULL(EMAILID,'(NULL)')+'  '+ISNULL(WEBID,'(NULL)')+'  '+ISNULL(APNM,'(NULL)')+'  '+ISNULL(APNO,'(NULL)')+'  '+
                        ISNULL(CONVERT(NVARCHAR(50),STATUS,103),'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  '+
                        ISNULL(CONVERT(NVARCHAR(50),INTIME,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+
                        ISNULL(IPADDRESS,'(NULL)')+'  '+ISNULL(LOGINID,'(NULL)')+'  '+ISNULL(LOGINPW,'(NULL)') FROM CNF_PARTY */
                    string logid = "UPDATE";
                    string tableid = "CNF_PARTY";
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, ipAddress);
                    // logdata add end //
                }
                catch (Exception ex)
                {
                    //ignore
                }

                dob.UpdateParty(iob);

                Refresh();
                lblerrmsg.Text = "Updated your record successfully.";
                lblerrmsg.Visible = true;
                lblerrmsg.ForeColor = Color.Green;
                txtPartynm.Focus();
                //}
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            // }
        }

        protected void btnPartyReport_Click(object sender, EventArgs e)
        {
            if (CookiesData["USERNAME"] == null)
                Response.Redirect("~/Login/UI/signin.aspx");
            else
            {
                 ScriptManager.RegisterStartupScript(this,
                    GetType(), "OpenWindow",
                    "window.open('../Report/Report/AllPartyInformation.aspx','_newtab');", true);
            }
        }

        protected bool UniqueLoginId(string loginId)
        {
            string dbLoginId = dbFunctions.StringData("SELECT LOGINID FROM CNF_PARTY WHERE LOGINID='" + loginId + "'");
            return loginId == dbLoginId;
        }

        protected bool UniqueVatNo(string VatNo)
        {
            string dbVatNo = dbFunctions.StringData("SELECT VATNO FROM CNF_PARTY WHERE VATNO='" + VatNo + "'");
            return VatNo == dbVatNo;
        }
    }
}