using alchemySoft.LogIn;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.Asl.UI
{
    public partial class CompanyCreation : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
        DataAccess.ASLDataAccess dob = new DataAccess.ASLDataAccess();
        Interface.ASLInterface iob = new Interface.ASLInterface();
        SqlConnection con=new SqlConnection(dbFunctions.Connection);
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
                    else
                    {
                        txtComName.Focus();
                    }
                }
            }
            else
            {
                Response.Redirect("default");
            } 
        }

        public Int64 MaximumCompanyID()
        {
            Int64 maxCompanyId = 0;
            string maxId = dbFunctions.getData("SELECT MAX(COMPID) FROM ASL_COMPANY");
            maxCompanyId = Convert.ToInt64(maxId);
            maxCompanyId++;

            return maxCompanyId;
        }

        public string FieldCheck()
        {
            var txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
            string checkResult = "";
            if (txtComName.Text == "")
            {
                checkResult = "Please fill Company Name field.";
               
                txtComName.Focus();
            }
            else if (txtAddress.Text == "")
            {
                checkResult = "Please fill Address field.";
                txtAddress.Focus();
            }
            else if (txtContactNo.Text == "")
            {
                checkResult = "Please fill Contact no field.";
                txtContactNo.Focus();
            }
            else if (txtLotiLongTude.Text == "")
            {
                checkResult = "Location not Found.";
                dbFunctions.popupAlert(Page, checkResult, "w");
                Response.Redirect(Request.RawUrl); 
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
            HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
            if (FieldCheck() == "true")
            {
                TextBox txtLotiLongTude = (TextBox)Master.FindControl("txtLotiLongTude");
                TextBox txtIp = (TextBox)Master.FindControl("txtIp");
                iob.LotiLengTudeInsert = txtLotiLongTude.Text;
                iob.ipAddressInsert =dbFunctions.ipAddress();
                iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
                iob.userPcInsert =dbFunctions.userPc();
                iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);

                iob.CompanyId = MaximumCompanyID();
                iob.ComapanyName = txtComName.Text;
                iob.Address = txtAddress.Text;
                iob.ContactNo = txtContactNo.Text;
                iob.EmailId = txtEmailId.Text;
                iob.WebId = txtWebsiteId.Text;
                iob.Status = ddlStatus.SelectedValue;

                string s= dob.INSERT_ASL_COMPANY(iob);
                if(s=="")
                {
                    dbFunctions.popupAlert(Page, "Company Succesfully Created.", "s");
                    // logdata add start //
                    string lotileng = iob.LotiLengTudeInsert;
                    string logdata = @"Company Id: "+iob.CompanyId+", Company Name: "+iob.ComapanyName+", Address: "+
                        iob.Address+", Contact No: "+iob.ContactNo+", Email Id: "+iob.EmailId+", WebId: "+iob.WebId+
                        ", Status: "+iob.Status;
                    string logid = "INSERT";
                    string tableid = "ASL_COMPANY";
                   // LogData.InsertLogData(lotileng, logid, tableid, logdata, txtIp.Text);
                    LogData.InsertLogData(lotileng, logid, tableid, logdata, txtIp.Text);
                    // logdata add start //

                    Refresh(); 
                      
                    Session["CreateCompany"] = iob.CompanyId;
                    Response.Redirect("~/asl/ui/UserCreate.aspx");
                }
                else
                { 
                    dbFunctions.popupAlert(Page, "Company does not create.", "e"); 
                }
                
            }
            else
            {
                 FieldCheck(); 
            }
        }

        public void Refresh()
        {
            txtComName.Text = "";
            txtAddress.Text = "";
            txtEmailId.Text = "";
            txtContactNo.Text = "";
            txtWebsiteId.Text = "";
            ddlStatus.SelectedIndex = -1;
            txtComName.Focus();
        }
    }
}