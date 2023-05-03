using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft;

namespace alchemySoft.LogIn.UI
{
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(dbFunctions.Connection);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null)
                {
                    Response.Redirect("~/LogIn/UI/signin.aspx");
                }
                else
                {
                    string userId = Session["USERID"].ToString();
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT ASL_USERCO.USERNM, ASL_USERCO.DEPTNM, ASL_USERCO.ADDRESS, ASL_USERCO.MOBNO, 
                    ASL_USERCO.EMAILID, ASL_USERCO.TIMEFR, ASL_USERCO.TIMETO, ASL_COMPANY.COMPNM FROM ASL_USERCO 
                    INNER JOIN ASL_COMPANY ON ASL_USERCO.COMPID = ASL_COMPANY.COMPID WHERE (ASL_USERCO.USERID = '" + userId + "')", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        lblUserName.Text = dr["USERNM"].ToString();
                        lblCompanyName.Text = dr["COMPNM"].ToString();
                        //lblBranch.Text = dr["BRANCHNM"].ToString();
                        lblAddress.Text = dr["ADDRESS"].ToString();
                        lblDepartment.Text = dr["DEPTNM"].ToString();
                        lblMobile.Text = dr["MOBNO"].ToString();
                        lblEmail.Text = dr["EMAILID"].ToString();
                        lblTimeFrom.Text = dr["TIMEFR"].ToString();
                        lbltimeTo.Text = dr["TIMETO"].ToString();
                    }
                    dr.Close();
                    con.Close();

                    lblTimeFrom.Text = DateTime.ParseExact(lblTimeFrom.Text, "HH:mm", null).ToString("hh:mm tt");
                    lbltimeTo.Text = DateTime.ParseExact(lbltimeTo.Text, "HH:mm", null).ToString("hh:mm tt");
                }
            }
        }
    }
}