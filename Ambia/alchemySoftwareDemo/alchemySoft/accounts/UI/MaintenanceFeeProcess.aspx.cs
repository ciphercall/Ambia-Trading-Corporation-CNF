using System;
using System.Web;
using AlchemyAccounting;
using alchemySoft;

namespace alchemy.accounts.UI
{
    public partial class MaintenanceFeeProcess : System.Web.UI.Page
    {
        readonly IFormatProvider _dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                string formLink = "/Accounts/UI/MaintenanceFeeProcess.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission)
                {
                    if (!Page.IsPostBack)
                    {
                        txtDate.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtEffectDate.Text = txtDate.Text;

                        DateTime datetrans = DateTime.Parse(txtDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        string mon = datetrans.ToString("MMM").ToUpper();
                        string yr = datetrans.ToString("yy");
                        txtMonthYear.Text = mon + "-" + yr;

                        var date = DateTime.Parse(txtDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        dbFunctions.dropDown_Bind(ddlTransno,"","select",
                            @"SELECT TRANSNO NM FROM GL_MTRANSMST WHERE TRANSTP='" + ddlTransType.SelectedValue + "' AND TRANSDT='" + date + "'");
                        ddlTransno.Focus();

                        DateTime transdate = DateTime.Parse(txtEffectDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        string month = transdate.ToString("MMM").ToUpper();
                        string years = transdate.ToString("yy");

                        txtEffectMonthYear.Text = month + "-" + years;
                        string maxtransno = dbFunctions.StringData(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtEffectMonthYear.Text +
                            "' and TRANSTP = '" + ddlTransType.Text + "'");
                        if (maxtransno == "")
                        {
                            txtEffectTransno.Text = "1";
                        }
                        else
                        {
                            int vNo = int.Parse(maxtransno);
                            int totVno = vNo + 1;
                            txtEffectTransno.Text = totVno.ToString();
                        }
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }
        protected void txtDate_OnTextChanged(object sender, EventArgs e)
        {
            DateTime datetrans = DateTime.Parse(txtDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string mon = datetrans.ToString("MMM").ToUpper();
            string yr = datetrans.ToString("yy");
            txtMonthYear.Text = mon + "-" + yr;

            var date = DateTime.Parse(txtDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            dbFunctions.dropDown_Bind(ddlTransno,"","select",
                @"SELECT TRANSNO nm FROM GL_MTRANSMST WHERE TRANSTP='" + ddlTransType.SelectedValue + "' AND TRANSDT='" + date + "'");
            ddlTransno.Focus();
        }

        protected void txtEffectDate_OnTextChanged(object sender, EventArgs e)
        {
            DateTime transdate = DateTime.Parse(txtEffectDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string month = transdate.ToString("MMM").ToUpper();
            string years = transdate.ToString("yy");

            txtEffectMonthYear.Text = month + "-" + years;
            string maxtransno = dbFunctions.StringData(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtEffectMonthYear.Text +
                "' and TRANSTP = '" + ddlTransType.Text + "'");
            if (maxtransno == "")
            {
                txtEffectTransno.Text = "1";
            }
            else
            {
                int vNo = int.Parse(maxtransno);
                int totVno = vNo + 1;
                txtEffectTransno.Text = totVno.ToString();
            }
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (ddlTransno.Text == "" || ddlTransno.Text == "--SELECT--")
            {
                lblMsg.Text = "Select transaction no.";
                lblMsg.Visible = true;
                ddlTransno.Focus();
            }
            else if (txtMonthYear.Text == "")
            {
                lblMsg.Text = "Select date.";
                lblMsg.Visible = true;
                txtDate.Focus();
            }
            else if (txtEffectMonthYear.Text == "")
            {
                lblMsg.Text = "Select date.";
                lblMsg.Visible = true;
                txtEffectDate.Focus();
            }
            else
            {
                string transdate = DateTime.Parse(txtDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal).ToString("yyyy-MM-dd");
                DateTime transdateeffect = DateTime.Parse(txtEffectDate.Text, _dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string month = transdateeffect.ToString("MMM").ToUpper();
                string years = transdateeffect.ToString("yy");
                txtEffectMonthYear.Text = month + "-" + years;

                string maxtransno = dbFunctions.StringData(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtEffectMonthYear.Text +
                "' and TRANSTP = '" + ddlTransType.Text + "'");
                if (maxtransno == "")
                {
                    txtEffectTransno.Text = "1";
                }
                else
                {
                    int vNo = int.Parse(maxtransno);
                    int totVno = vNo + 1;
                    txtEffectTransno.Text = totVno.ToString();
                }
                string userName = CookiesData["USERID"].ToString();
                string ipAddress = CookiesData["IpAddress"].ToString();
                string pcName = CookiesData["PCName"].ToString();
                string inTm = dbFunctions.timezone(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss tt");

                dbFunctions.execute(@"INSERT INTO GL_MTRANSMST(TRANSTP, TRANSDT, TRANSMY, TRANSNO, USERPC, USERID, INTIME, IPADDRESS) 
                VALUES('" + ddlTransType.SelectedValue + "', '" + transdateeffect + "', '" + txtEffectMonthYear.Text + "', '" + txtEffectTransno.Text +
                "', '" + pcName + "', '" + userName + "', '" + inTm + "', '" + ipAddress + "')");

                dbFunctions.execute(@"INSERT INTO GL_MTRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, REMARKS, USERPC, USERID,  INTIME, IPADDRESS)
                SELECT TRANSTP, '" + transdateeffect + "', '" + txtEffectMonthYear.Text + "', " + txtEffectTransno.Text + ", SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, '" + txtRemarks.Text + "', '" + pcName + "', '" + userName + "', '" + inTm + "', '" + ipAddress + 
                "' FROM            GL_MTRANS WHERE TRANSTP='" + ddlTransType.SelectedValue + "' AND TRANSDT='" + transdate + 
                "' AND TRANSMY='" + txtMonthYear.Text + "' AND TRANSNO='" + ddlTransno.Text + "'");

                lblMsg.Text = "Process Complete.";
                lblMsg.Visible = true;

                maxtransno = dbFunctions.StringData(@"Select max(TRANSNO) FROM GL_MTRANSMST where TRANSMY='" + txtEffectMonthYear.Text +
                "' and TRANSTP = '" + ddlTransType.Text + "'");
                if (maxtransno == "")
                {
                    txtEffectTransno.Text = "1";
                }
                else
                {
                    int vNo = int.Parse(maxtransno);
                    int totVno = vNo + 1;
                    txtEffectTransno.Text = totVno.ToString();
                }

                txtRemarks.Text = "";
                ddlTransno.SelectedIndex = -1;
                ddlTransno.Focus();
            }
        }

    }
}