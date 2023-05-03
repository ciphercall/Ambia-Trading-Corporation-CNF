using System;
using alchemySoft;

namespace alchemySoft.Asl.Report.Report
{
    public partial class AllUserInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null)
                {
                    Response.Redirect("~/login/ui/signin.aspx");
                }
                else
                {
                    if (Session["USERTYPE"].ToString() == "SUPERADMIN")
                    {
                        Session["CompanyIdForReport"] = null;
                        dbFunctions.dropDown_Bind(txtCompanyName,"","select", "SELECT COMPNM NM FROM ASL_COMPANY WHERE COMPID!='100'");
                    }
                    else if (Session["USERTYPE"].ToString() == "COMPADMIN")
                    {
                        lblMsg.Visible = false;
                        if (Session["COMPANYID"] != null)
                        {
                            string companyid = Session["COMPANYID"].ToString();
                            txtCompanyName.Text= dbFunctions.getData("SELECT COMPNM FROM ASL_COMPANY WHERE COMPID='" + companyid + "'");
                            Session["CompanyIdForReport"] = companyid;
                            lblMsg.Visible = false;
                            txtCompanyName.Visible = false;
                            lblComLabel.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect("/Default.aspx");
                    }

                }
            }
        }

        protected void txtCompanyName_TextChanged(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("~/login/ui/signin.aspx");
            }
            else
            {
                Session["CompanyIdForReport"] = null;
                if (txtCompanyName.Text == "--SELECT--")
                {
                    lblMsg.Text = "Select Company Name";
                    lblMsg.Visible = true;
                }
                else
                {
                    string companyId =
                        dbFunctions.getData("SELECT COMPID FROM ASL_COMPANY WHERE COMPNM='" + txtCompanyName.Text +
                                               "'");
                    if (companyId == "")
                    {
                        txtCompanyName.SelectedIndex = -1;
                        lblMsg.Text = "Select Company Name";
                        lblMsg.Visible = true;
                    }
                    else
                    {
                        Session["CompanyIdForReport"] = companyId;
                        ShowData();
                        lblMsg.Visible = false;
                    }
                }
            }
        }
        private void ShowData()
        {
            string companyid = Session["CompanyIdForReport"].ToString();
            dbFunctions.gridViewAdd(gv_User, @"SELECT ASL_COMPANY.COMPNM, ASL_USERCO.USERNM, ASL_USERCO.DEPTNM, 
                            CASE(ASL_USERCO.OPTP) WHEN 'COMPADMIN' THEN 'COMPANY ADMIN' WHEN 'USERADMIN' THEN 'USER ADMIN' 
                            WHEN 'SUPERADMIN' THEN 'SUPER ADMIN' 
                            WHEN 'USER' THEN 'USER' ELSE ASL_USERCO.OPTP END AS OPTP,
                            ASL_USERCO.ADDRESS, ASL_USERCO.MOBNO, ASL_USERCO.EMAILID, 
                            CASE(ASL_USERCO.LOGINBY) WHEN 'MOBNO' THEN 'MOBILE' ELSE ASL_USERCO.LOGINBY END AS LOGINBY, 
                            ASL_USERCO.LOGINID, ASL_USERCO.LOGINPW, CONVERT(varchar(15),CAST(ASL_USERCO.TIMEFR AS TIME),100) AS TIMEFR,
                            CONVERT(varchar(15),CAST(ASL_USERCO.TIMETO AS TIME),100) AS TIMETO, 
                            CASE(ASL_USERCO.STATUS) WHEN 'A' THEN 'ACTIVE' ELSE 'INACTIVE' END AS STATUS
                            FROM ASL_USERCO INNER JOIN
                            ASL_COMPANY ON ASL_USERCO.COMPID = ASL_COMPANY.COMPID
                            WHERE ASL_COMPANY.COMPID='" + companyid + "'" +
             " ORDER BY ASL_COMPANY.COMPNM, ASL_USERCO.USERNM");
        }
    }
}