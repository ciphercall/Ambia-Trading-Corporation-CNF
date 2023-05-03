using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using alchemySoft.CNF.DataAccess;
using alchemySoft.CNF.Interface;

namespace alchemySoft.CNF.UI
{
    public partial class cnf_job_info : System.Web.UI.Page
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
                string formLink = "/CNF/UI/cnf-job-info.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (dbFunctions.timezone(DateTime.Now) >= dateTime)
                    Response.Redirect("~/Servererror.html");
                else if (permission)
                {
                    if (!IsPostBack)
                    {
                        DateTime td = dbFunctions.timezone(DateTime.Now);
                        txtCrDt.Text = td.ToString("dd/MM/yyyy");
                        txtJobYear.Text = td.ToString("yyyy");
                        string brCD = CookiesData["COMPANYID"].ToString();
                        dbFunctions.txtAdd($@"SELECT BRANCHID+'-'+COMPNM FROM ASL_COMPANY INNER JOIN ASL_BRANCH ON ASL_COMPANY.COMPID=ASL_BRANCH.COMPID WHERE ASL_COMPANY.COMPID='{brCD}'", txtCompNM);
                        txtCompID.Text = "10101";
                        check_Job_No();
                        txtCompNM.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/default");
                }
            }
        }

        public void check_Job_No()
        {
            lblJobNo.Text = "";
            dbFunctions.lblAdd("SELECT MAX(JOBNO) AS JOBNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "'", lblJobNo);
            if (lblJobNo.Text == "")
            {
                txtNo.Text = "1";
            }
            else
            {
                Int64 trns, ftrns = 0;
                trns = Convert.ToInt64(lblJobNo.Text);
                ftrns = trns + 1;
                txtNo.Text = ftrns.ToString();
            }
        }
        public void load()
        {
            DateTime td = dbFunctions.timezone(DateTime.Now);
            txtCrDt.Text = td.ToString("dd/MM/yyyy");
            txtJobYear.Text = td.ToString("yyyy");
            check_Job_No();
            //dbFunctions.dropDown_Bind(txtConsigneeNM, "", "SELECT", "SELECT DISTINCT CONSIGNEENM AS NM FROM CNF_JOB");
            //dbFunctions.dropDown_Bind(txtSuppNM, "", "SELECT", "SELECT DISTINCT SUPPLIERNM AS NM FROM CNF_JOB");
            //dbFunctions.dropDown_Bind(ddlExTP, "", "SELECT", "SELECT DISTINCT CNFV_ETP AS NM FROM CNF_JOB");

        }

        protected void txtCompNM_TextChanged(object sender, EventArgs e)
        {
            if (txtCompNM.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select Company.";
                dbFunctions.popupAlert(Page, "Select Company.", "w");
                txtCompNM.Focus();
            }
            else
            {
                string compadd = "";
                string compnm = "";
                string searchPar = txtCompNM.Text;

                int splitter = searchPar.IndexOf("-");
                if (splitter != -1)
                {
                    string[] lineSplit = searchPar.Split('-');

                    compadd = lineSplit[0];
                    compnm = lineSplit[1];
                }

                txtCompID.Text = "";
                dbFunctions.txtAdd("SELECT COMPID FROM ASL_COMPANY WHERE BRANCHID ='" + compadd + "'", txtCompID);
                if (txtCompID.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select Company.";
                    dbFunctions.popupAlert(Page, "Select Company.", "w");
                    txtCompNM.Text = "";
                    txtCompNM.Focus();
                }
                else
                {
                    lblError.Visible = false;
                    ddlJobTp.Focus();
                }
            }
        }
        private void Refresh()
        {

            lblError.Visible = false;
            // txtCompNM.Text = "";
            //ddlJobTp.SelectedIndex = -1;
            //txtCrDt.Text = td.ToString("dd/MM/yyyy");
            //txtJobYear.Text = td.ToString("yyyy");
            // ddlRegID.SelectedIndex = -1;
            //ddlJobQuality.SelectedIndex = -1;
            txtCompID.Text = "";
            txtFORWRDDT.Text = "";
            DateTime td = dbFunctions.timezone(DateTime.Now);

            if (btnEdit.Text == "Edit")
            {
                check_Job_No();
            }
            else
            {
                ddlJobNo.SelectedIndex = -1;
            }

            txtPartyNM.Text = "";
            txtPartyID.Text = "";
            txtConsigneeNM.Text = "";
            txtConsigneeAdd.Text = "";
            txtGoodsDesc.Text = "";
            txtPkgDet.Text = "";
            txtPackageType.Text = "";
            txtVessel.Text = "";
            txtRotNO.Text = "";
            txtClearedOn.Text = "";
            txtLineNo.Text = "";
            txtCBM.Text = "";
            txtContainerNo.Text = "";
            txtSuppNM.Text = "";
            txtCNFVUSD.Text = ".00";
            txtCRFVUSD.Text = ".00";
            txtChangeRT.Text = ".00";
            txtExTP.Text = "";
            txtCNFVBDT.Text = ".00";
            txtGrossWeight.Text = ".00";
            txtNetWeight.Text = ".00";
            txtCRFNO.Text = "";
            txtCRFDT.Text = "";
            txtInvoiceNo.Text = "";
            txtInvoiceDT.Text = "";
            txtBENO.Text = "";
            txtBEDT.Text = "";
            txtBLNO.Text = "";
            txtBLDT.Text = "";
            txtLCNO.Text = "";
            txtLCDT.Text = "";
            txtPermitNO.Text = "";
            txtPermitDT.Text = "";
            // txtWhDT.Text = "";
            txtDelDT.Text = "";
            txtAssessableVal.Text = ".00";
            txtCommission.Text = ".00";
            txtAwbNo.Text = "";
            txtAwbDT.Text = "";
            txtHBlNo.Text = "";
            txtHblDT.Text = "";
            txtHawbDT.Text = "";
            txtUnderTakeNo.Text = "";
            // txtUnderTakeDt.Text = "";
            txtContainerNo.Text = "";
            //txtVatNo.Text = "";
            ddlStatus.SelectedIndex = -1;
            txtComRemarks.Text = "";
            txtCrDt.Text = td.ToString("dd/MM/yyyy");
            txtJobYear.Text = td.ToString("yyyy");
            string brCD = CookiesData["COMPANYID"].ToString();
            dbFunctions.txtAdd("SELECT BRANCHID+'-'+COMPNM FROM ASL_COMPANY WHERE COMPID='" + brCD + "'", txtCompNM);
            txtCompID.Text = brCD;
            check_Job_No();
            txtCompNM.Focus();
        }

        protected void ddlJobNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlJobNo.Text == "Select")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select Job No.";
                dbFunctions.popupAlert(Page, "Select Job No.", "w");
                ddlJobNo.Focus();
            }
            else
            {
                lblError.Visible = false;

                dbFunctions.txtAdd("SELECT COMPID FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCompID);
                dbFunctions.txtAdd("SELECT ASL_COMPANY.BRANCHID + '-' + ASL_COMPANY.COMPNM FROM ASL_COMPANY INNER JOIN CNF_JOB ON ASL_COMPANY.COMPID = CNF_JOB.COMPID " +
                    " WHERE ASL_COMPANY.COMPID='" + txtCompID.Text + "'  AND CNF_JOB.JOBYY =" + txtJobYear.Text + " AND CNF_JOB.JOBTP ='" + ddlJobTp.Text + "' AND CNF_JOB.JOBNO =" + ddlJobNo.Text + "", txtCompNM);
                dbFunctions.lblAdd("SELECT JOBTP FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", lblJobTP);
                ddlJobTp.Text = lblJobTP.Text;
                dbFunctions.txtAdd("SELECT CONVERT(NVARCHAR(20),JOBCDT,103) AS JOBCDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCrDt);
                dbFunctions.txtAdd("SELECT JOBYY FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtJobYear);
                dbFunctions.lblAdd("SELECT REGID FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", lblJobReg);
                ddlRegID.Text = lblJobReg.Text;
                dbFunctions.lblAdd("SELECT JOBQUALITY FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", lblJobQuality);
                ddlJobQuality.Text = lblJobQuality.Text;
                dbFunctions.txtAdd("SELECT PARTYID FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtPartyID);
                dbFunctions.txtAdd("SELECT GL_ACCHART.ACCOUNTNM FROM GL_ACCHART INNER JOIN CNF_JOB ON GL_ACCHART.ACCOUNTCD = CNF_JOB.PARTYID WHERE CNF_JOB.JOBYY =" + txtJobYear.Text + " AND CNF_JOB.JOBTP ='" + ddlJobTp.Text + "' AND CNF_JOB.JOBNO =" + ddlJobNo.Text + "", txtPartyNM);
                dbFunctions.txtAdd("SELECT CONSIGNEENM FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtConsigneeNM);


                dbFunctions.txtAdd("SELECT CONSIGNEEADD FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtConsigneeAdd);
                dbFunctions.txtAdd("SELECT GOODS FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtGoodsDesc);
                dbFunctions.txtAdd("SELECT PKGS FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtPkgDet);
                dbFunctions.txtAdd("SELECT PKGSTP FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtPackageType);
                dbFunctions.txtAdd("SELECT MVESSEL FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtVessel);
                dbFunctions.txtAdd("SELECT ROTNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtRotNO);
                dbFunctions.txtAdd("SELECT LINE_NO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtLineNo);
                dbFunctions.txtAdd("SELECT CLRDON FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtClearedOn);
                dbFunctions.txtAdd("SELECT (CASE WHEN FRWDT ='1990-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),FRWDT,103) END) AS FRWDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtFORWRDDT);

                dbFunctions.txtAdd("SELECT CBM FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCBM);
                dbFunctions.txtAdd("SELECT CONTNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtContainerNo);

                dbFunctions.txtAdd("SELECT SUPPLIERNM FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtSuppNM);


                dbFunctions.txtAdd("SELECT CNFV_USD FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCNFVUSD);
                dbFunctions.txtAdd("SELECT CRFV_USD FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCRFVUSD);
                dbFunctions.txtAdd("SELECT CNFV_ERT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtChangeRT);
                dbFunctions.txtAdd("SELECT CNFV_ETP FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtExTP);


                dbFunctions.txtAdd("SELECT CNFV_BDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCNFVBDT);
                dbFunctions.txtAdd("SELECT WTGROSS FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtGrossWeight);
                dbFunctions.txtAdd("SELECT WTNET FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtNetWeight);
                dbFunctions.txtAdd("SELECT CRFNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCRFNO);
                dbFunctions.txtAdd("SELECT (CASE WHEN CRFDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),CRFDT,103) END) AS CRFDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCRFDT);
                dbFunctions.txtAdd("SELECT DOCINVNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtInvoiceNo);
                dbFunctions.txtAdd("SELECT (CASE WHEN DOCRCVDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),DOCRCVDT,103) END) AS DOCRCVDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtInvoiceDT);
                dbFunctions.txtAdd("SELECT BENO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtBENO);
                dbFunctions.txtAdd("SELECT (CASE WHEN BEDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),BEDT,103) END) AS BEDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtBEDT);
                dbFunctions.txtAdd("SELECT BLNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtBLNO);
                dbFunctions.txtAdd("SELECT (CASE WHEN BLDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),BLDT,103) END) AS BLDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtBLDT);
                dbFunctions.txtAdd("SELECT LCNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtLCNO);
                dbFunctions.txtAdd("SELECT (CASE WHEN LCDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),LCDT,103) END) AS LCDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtLCDT);
                dbFunctions.txtAdd("SELECT PERMITNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtPermitNO);
                dbFunctions.txtAdd("SELECT (CASE WHEN PERMITDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),PERMITDT,103) END) AS PERMITDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtPermitDT);
                //dbFunctions.txtAdd("SELECT (CASE WHEN WFDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),WFDT,103) END) AS WFDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtWhDT);


                dbFunctions.txtAdd("SELECT (CASE WHEN DELIVERYDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),DELIVERYDT,103) END) AS DELIVERYDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtDelDT);
                dbFunctions.txtAdd("SELECT ASSV_BDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtAssessableVal);
                dbFunctions.txtAdd("SELECT COMM_AMT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtCommission);
                dbFunctions.txtAdd("SELECT AWBNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtAwbNo);
                dbFunctions.txtAdd("SELECT (CASE WHEN AWBDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),AWBDT,103) END) AS AWBDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtAwbDT);
                dbFunctions.txtAdd("SELECT HBLNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtHBlNo);
                dbFunctions.txtAdd("SELECT (CASE WHEN HBLDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),HBLDT,103) END) AS AWBDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtHblDT);
                dbFunctions.txtAdd("SELECT HAWBNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtHawbNo);
                dbFunctions.txtAdd("SELECT (CASE WHEN HAWBDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),HAWBDT,103) END) AS AWBDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtHawbDT);
                dbFunctions.txtAdd("SELECT UNTKNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtUnderTakeNo);
                //dbFunctions.txtAdd("SELECT (CASE WHEN UNTKDT ='1900-01-01' THEN NULL ELSE CONVERT(NVARCHAR(20),UNTKDT,103) END) AS UNTKDT FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtUnderTakeDt);


                dbFunctions.txtAdd("SELECT COM_REMARKS FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtComRemarks);
                dbFunctions.lblAdd("SELECT STATUS FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", lblStatus);
                dbFunctions.lblAdd("SELECT REFTP FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", lblRefference);
                dbFunctions.txtAdd("SELECT VATNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND JOBNO =" + ddlJobNo.Text + "", txtVatNo);
                if (lblRefference.Text != "")
                    ddlRefferenceType.SelectedValue = lblRefference.Text;
                else ddlRefferenceType.SelectedIndex = -1;

                txtCompNM.Focus();
            }
        }

        protected void ddlRegID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnEdit.Text != "Edit")
                txtAssessableVal_TextChanged(sender, e);
            ddlJobQuality.Focus();

        }

        protected void txtAssessableVal_TextChanged(object sender, EventArgs e)
        {
            if (txtCNFVUSD.Text == "")
                txtCNFVUSD.Text = "0";
            else if (txtChangeRT.Text == "")
                txtChangeRT.Text = "0";

            if (txtPartyID.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select Party Name.";
                dbFunctions.popupAlert(Page, "Select Party Name.", "w");
                txtChangeRT.Text = ".00";
                txtAssessableVal.Text = ".00";
                txtPartyNM.Focus();
            }
            else
            {
                con = new SqlConnection(dbFunctions.Connection);
                if (con.State != ConnectionState.Open) con.Open();

                cmd = new SqlCommand("SELECT EXCTP, VALUETP, COMMAMT FROM CNF_COMMISSION WHERE (PARTYID = '" + txtPartyID.Text + "') AND " + txtAssessableVal.Text + " BETWEEN VALUEFR AND VALUETO AND EXCTP='BDT' AND REGID ='" + ddlRegID.Text + "' AND JOBQUALITY ='" + ddlJobQuality.Text + "'" +
                        " UNION ALL " +
                        " SELECT EXCTP, VALUETP, COMMAMT FROM CNF_COMMISSION WHERE (PARTYID = '" + txtPartyID.Text + "') AND " + txtCNFVUSD.Text + " BETWEEN VALUEFR AND VALUETO AND EXCTP='USD' AND REGID ='" + ddlRegID.Text + "' AND JOBQUALITY ='" + ddlJobQuality.Text + "'", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (con.State != ConnectionState.Closed) con.Close();

                if (ds.Tables[0].Rows.Count > 1)
                {
                    //lblError.Visible = true;
                    //lblError.Text = "An error occured. Please check party commission form.";
                    dbFunctions.popupAlert(Page, "An error occured. Please check party commission form.", "w");
                    txtChangeRT.Text = ".00";
                    txtCommission.Text = ".00";
                    txtAssessableVal.Text = ".00";
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    GridView1.Visible = false;
                    txtChangeRT.Focus();
                }
                else if (ds.Tables[0].Rows.Count > 0)
                {
                    lblError.Visible = false;
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    GridView1.Visible = false;

                    foreach (GridViewRow grid in GridView1.Rows)
                    {
                        try
                        {
                            string comMainVal = "0";
                            if (grid.Cells[0].Text == "BDT")
                                comMainVal = txtAssessableVal.Text;
                            else
                                comMainVal = txtAssessableVal.Text;

                            if (grid.Cells[2].Text == "")
                                grid.Cells[2].Text = ".00";

                            if (grid.Cells[1].Text == "AMT")
                            {
                                txtCommission.Text = grid.Cells[2].Text;
                            }
                            else
                            {
                                txtCommission.Text = (Convert.ToDecimal(comMainVal) * ((Convert.ToDecimal(grid.Cells[2].Text)) / 100)).ToString("F");
                            }

                            txtCRFNO.Focus();
                        }
                        catch (Exception ex)
                        {
                            lblError.Visible = true;
                            lblError.Text = ex.Message;
                        }
                    }

                }
                else
                {
                    lblError.Visible = false;
                    txtCommission.Text = ".00";
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    GridView1.Visible = false;
                    txtCRFNO.Focus();
                }
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
            txtCompNM.Text = "";
            ddlJobTp.SelectedIndex = -1;
            txtCrDt.Text = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
            txtJobYear.Text = dbFunctions.timezone(DateTime.Now).ToString("yyyy");
            ddlRegID.SelectedIndex = -1;
            ddlJobQuality.SelectedIndex = -1;
        }
        protected void ddlJobQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnEdit.Text != "Edit")
                txtAssessableVal_TextChanged(sender, e);

            txtPartyNM.Focus();
        }

        protected void txtPartyNM_TextChanged(object sender, EventArgs e)
        {
            if (txtPartyNM.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select Party.";
                dbFunctions.popupAlert(Page, "Select Party.", "w");
                txtPartyNM.Focus();
            }
            else
            {
                txtPartyID.Text = "";
                dbFunctions.txtAdd("SELECT ACCOUNTCD FROM GL_ACCHART WHERE ACCOUNTNM ='" + txtPartyNM.Text + "' AND STATUSCD ='P'", txtPartyID);
                if (txtPartyID.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select Party.";
                    dbFunctions.popupAlert(Page, "Select Party.", "w");
                    txtPartyNM.Text = "";
                    txtPartyNM.Focus();
                }
                else
                {
                    if (btnEdit.Text != "Edit")
                        txtAssessableVal_TextChanged(sender, e);

                    lblError.Visible = false;
                    txtConsigneeNM.Focus();
                }
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "New";
                txtNo.Visible = false;
                ddlJobNo.Visible = true;
                btnSave.Text = "Update";
                Refresh();
                string uTp = CookiesData["USERTYPE"].ToString();
                string brCD = CookiesData["COMPANYID"].ToString();

                if (uTp == "ADMIN")
                {
                    dbFunctions.dropDown_Bind(ddlJobNo, "", "", "SELECT JOBNO AS NM FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "'");
                }
                else
                {
                    dbFunctions.dropDown_Bind(ddlJobNo, "", "SELECT", "SELECT JOBNO AS NM FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND COMPID like '" + brCD + "%'");
                }
                ddlJobNo.Focus();
            }
            else
            {
                btnEdit.Text = "Edit";
                txtNo.Visible = true;
                ddlJobNo.Visible = false;
                check_Job_No();
                Refresh();
                btnSave.Text = "Save";
                txtCompNM.Focus();
            }
        }

        protected void ddlJobTp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["COMPANYID"].ToString();

            if (btnEdit.Text == "Edit")
            {
                check_Job_No();
            }
            else
            {
                if (uTp == "ADMIN")
                {
                    dbFunctions.dropDown_Bind(ddlJobNo, "", "", "SELECT JOBNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "'");
                }
                else
                {
                    dbFunctions.dropDown_Bind(ddlJobNo, "", "", "SELECT JOBNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND COMPID ='" + brCD + "'");
                }
            }
            txtCrDt.Focus();
        }
        protected void txtJobYear_TextChanged(object sender, EventArgs e)
        {
            string uTp = CookiesData["USERTYPE"].ToString();
            string brCD = CookiesData["COMPANYID"].ToString();

            if (btnEdit.Text == "Edit")
            {
                check_Job_No();
            }
            else
            {
                if (uTp == "ADMIN")
                {
                    dbFunctions.dropDown_Bind(ddlJobNo, "", "", "SELECT JOBNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "'");
                }
                else
                {
                    dbFunctions.dropDown_Bind(ddlJobNo, "", "", "SELECT JOBNO FROM CNF_JOB WHERE JOBYY =" + txtJobYear.Text + " AND JOBTP ='" + ddlJobTp.Text + "' AND COMPID ='" + brCD + "'");
                }
            }
            ddlRegID.Focus();
        }
        protected void txtCrDt_TextChanged(object sender, EventArgs e)
        {
            DateTime ctd = DateTime.Parse(txtCrDt.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            txtCrDt.Text = ctd.ToString("dd/MM/yyyy");
            txtJobYear.Text = ctd.ToString("yyyy");
            check_Job_No();
            txtJobYear.Focus();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Session["CompanyName"] = txtCompNM.Text;
            Session["CompanyId"] = txtCompID.Text;
            Session["JobType"] = ddlJobTp.SelectedItem.Text;
            Session["JobYear"] = txtJobYear.Text;

            if (btnEdit.Text == "Edit")
            {
                Session["JobNo"] = txtNo.Text;
            }
            else
            {
                Session["JobNo"] = ddlJobNo.SelectedItem.Text;
            }
             ScriptManager.RegisterStartupScript(this,
                      this.GetType(), "OpenWindow", "window.open('../report/vis-rep/rpt-Job-Create.aspx','_newtab');", true);
        }

        protected void ddlConsigneeNM_OnTextChanged(object sender, EventArgs e)
        {
            if (txtConsigneeNM.Text == "SELECT")
            {
                txtConsigneeAdd.Text = "";
            }
            else
            {
                txtConsigneeAdd.Text = "";
                dbFunctions.txtAdd("SELECT CONSIGNEEADD FROM CNF_JOB WHERE CONSIGNEENM ='" + txtConsigneeNM.Text + "' ", txtConsigneeAdd);
                if (txtConsigneeAdd.Text == "")
                {
                    txtConsigneeAdd.Focus();
                }
                else
                {
                    txtSuppNM.Focus();
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CookiesData["USERNAME"] == null)
                Response.Redirect("~/Login/UI/SignIn.aspx");
            else
            {
                if (txtCompID.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select Company";
                    dbFunctions.popupAlert(Page, "Select Company.", "w");
                    txtCompNM.Focus();
                }
                else if (txtCrDt.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select Job Creattion Date.";
                    dbFunctions.popupAlert(Page, "Select Job Creattion Date.", "w");
                    txtCrDt.Focus();
                }
                else if (txtJobYear.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Type Job Year.";
                    dbFunctions.popupAlert(Page, "Type Job Year.", "w");
                    txtJobYear.Focus();
                }
                else if (txtPartyID.Text == "")
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Select Party.";
                    dbFunctions.popupAlert(Page, "Select Party.", "w");
                    txtPartyNM.Focus();
                }
                else
                {
                    iob.CompID = txtCompID.Text;
                    iob.JobTP = ddlJobTp.Text;
                    iob.JobCrDT = DateTime.Parse(txtCrDt.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.JobYear = Convert.ToInt16(txtJobYear.Text);
                    if (btnEdit.Text == "Edit")
                    {
                        check_Job_No();
                        iob.JobNo = Convert.ToInt64(txtNo.Text);
                    }
                    else
                    {
                        if (ddlJobNo.Text == "Select")
                        {
                            //lblError.Visible = true;
                            //lblError.Text = "Select Job No.";
                            dbFunctions.popupAlert(Page, "Select Job No.", "w");
                            ddlJobNo.Focus();
                        }
                        else
                            iob.JobNo = Convert.ToInt64(ddlJobNo.Text);
                    }
                    iob.RegID = ddlRegID.Text;
                    iob.JobQuality = ddlJobQuality.Text;
                    iob.PartyID = txtPartyID.Text;
                    iob.ConsigneeName = txtConsigneeNM.Text;
                    iob.ConsigneeAddress = txtConsigneeAdd.Text;
                    iob.GoodsDesc = txtGoodsDesc.Text;
                    if (txtPkgDet.Text == ".00" || txtPkgDet.Text == "")
                        txtPkgDet.Text = "0";
                    iob.PkgDetails = Convert.ToDecimal(txtPkgDet.Text);
                    iob.Vessel = txtVessel.Text;
                    iob.RotNo = txtRotNO.Text;
                    iob.LineNO = txtLineNo.Text;
                    iob.ClearedOn = txtClearedOn.Text;
                    if (txtCBM.Text == ".00" || txtCBM.Text == "")
                        txtCBM.Text = "0";
                    if (txtFORWRDDT.Text == "")
                        iob.JobforWrdDT = DateTime.Parse("01/01/1990", dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    else
                        iob.JobforWrdDT = DateTime.Parse(txtFORWRDDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.CBM = Convert.ToDecimal(txtCBM.Text);
                    iob.ContainerNo = txtContainerNo.Text;
                    iob.SupplierNM = txtSuppNM.Text;
                    iob.CnfUSD = Convert.ToDecimal(txtCNFVUSD.Text);
                    iob.CrfUSD = Convert.ToDecimal(txtCNFVUSD.Text);
                    iob.ExchangeRT = Convert.ToDecimal(txtChangeRT.Text);
                    iob.ExTP = txtExTP.Text;
                    iob.CnfBDT = Convert.ToDecimal(txtCNFVBDT.Text);
                    if (txtGrossWeight.Text == ".00" || txtGrossWeight.Text == "")
                        txtGrossWeight.Text = "0";
                    iob.GrossWeight = Convert.ToDecimal(txtGrossWeight.Text);
                    if (txtNetWeight.Text == ".00" || txtNetWeight.Text == "")
                        txtNetWeight.Text = "0";
                    iob.NetWeight = Convert.ToDecimal(txtNetWeight.Text);
                    iob.BeNO = txtBENO.Text;
                    if (txtBEDT.Text == "")
                        txtBEDT.Text = "1900-01-01";
                    iob.BeDT = DateTime.Parse(txtBEDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    //if (txtWhDT.Text == "")
                    //    txtWhDT.Text = "1900-01-01";
                    //iob.WharfentDT = DateTime.Parse(txtWhDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    if (txtDelDT.Text == "")
                        txtDelDT.Text = "1900-01-01";
                    iob.DelDT = DateTime.Parse(txtDelDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.AssessableAMT = Convert.ToDecimal(txtAssessableVal.Text);
                    iob.Commission = Math.Round(Convert.ToDecimal(txtCommission.Text));
                    iob.CrfNO = txtCRFNO.Text;
                    if (txtCRFDT.Text == "")
                        txtCRFDT.Text = "1900-01-01";
                    iob.CrfDT = DateTime.Parse(txtCRFDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.InNO = txtInvoiceNo.Text;
                    if (txtInvoiceDT.Text == "")
                        txtInvoiceDT.Text = "1900-01-01";
                    iob.InDT = DateTime.Parse(txtInvoiceDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.BlNO = txtBLNO.Text;
                    if (txtBLDT.Text == "")
                        txtBLDT.Text = "1900-01-01";
                    iob.BlDT = DateTime.Parse(txtBLDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.LcNO = txtLCNO.Text;
                    if (txtLCDT.Text == "")
                        txtLCDT.Text = "1900-01-01";
                    iob.LcDT = DateTime.Parse(txtLCDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.PermitNO = txtPermitNO.Text;
                    if (txtPermitDT.Text == "")
                        txtPermitDT.Text = "1900-01-01";
                    iob.PermitDT = DateTime.Parse(txtPermitDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.Awbno = txtAwbNo.Text;
                    if (txtAwbDT.Text == "")
                        txtAwbDT.Text = "1900-01-01";
                    iob.Awbdt = DateTime.Parse(txtAwbDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.Hbl = txtHBlNo.Text;
                    if (txtHblDT.Text == "")
                        txtHblDT.Text = "1900-01-01";
                    iob.Hbldt = DateTime.Parse(txtHblDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.Hawbno = txtHawbNo.Text;
                    if (txtHawbDT.Text == "")
                        txtHawbDT.Text = "1900-01-01";
                    iob.Hawbdt = DateTime.Parse(txtHawbDT.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.UnderTakeNo = txtUnderTakeNo.Text;
                    //if (txtUnderTakeDt.Text == "")
                    //    txtUnderTakeDt.Text = "1900-01-01";
                    //iob.UnderTakeDt = DateTime.Parse(txtUnderTakeDt.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.ComRemarks = txtComRemarks.Text;
                    iob.Status = ddlStatus.Text;
                    iob.PackageType = txtPackageType.Text;
                    iob.ReffereceType = ddlRefferenceType.SelectedValue;
                    iob.VatNo = txtVatNo.Text;
                    iob.InTM = dbFunctions.timezone(DateTime.Now);
                    iob.UpTM = dbFunctions.timezone(DateTime.Now);
                    if (btnEdit.Text == "Edit")
                        iob.UserNM = CookiesData["USERNAME"].ToString();
                    else
                        iob.UpdateUser = CookiesData["USERNAME"].ToString();

                    iob.Ipaddress = CookiesData["IpAddress"].ToString();
                    iob.Userpc = CookiesData["PCName"].ToString();

                    if (btnEdit.Text == "Edit")
                        dob.save_cnf_job(iob);
                    else
                    {
                        try
                        {

                            string alldata = dbFunctions.StringData(@"SELECT CONVERT(NVARCHAR(50),JOBCDT,103)+'  '+COMPID+'  '+ISNULL(REGID,'(NULL)')+'  
                        '+ISNULL(JOBQUALITY,'(NULL)')+'  '+CONVERT(NVARCHAR(50),JOBYY,103)+'  '+JOBTP+'  '+CONVERT(NVARCHAR(50),JOBNO,103)+'  
                        '+ISNULL(PARTYID,'(NULL)')+'  '+ISNULL(CONSIGNEENM,'(NULL)')+'  '+ISNULL(CONSIGNEEADD,'(NULL)')+'  '+ISNULL(SUPPLIERNM,'(NULL)')+'  
                        '+CONVERT(NVARCHAR(50),PKGS,103)+'  '+ISNULL(GOODS,'(NULL)')+'  '+CONVERT(NVARCHAR(50),WTGROSS,103)+'  '+CONVERT(NVARCHAR(50),WTNET,103)+'  
                        '+CONVERT(NVARCHAR(50),CBM,103)+'  '+CONVERT(NVARCHAR(50),CNFV_USD,103)+'  '+ISNULL(CNFV_ETP,'(NULL)')+'  
                        '+CONVERT(NVARCHAR(50),CNFV_ERT,103)+'  '+CONVERT(NVARCHAR(50),CNFV_BDT,103)+'  '+CONVERT(NVARCHAR(50),CRFV_USD,103)+'  
                        '+CONVERT(NVARCHAR(50),ASSV_BDT,103)+'  '+CONVERT(NVARCHAR(50),COMM_AMT,103)+'  '+ISNULL(CONTNO,'(NULL)')+'  
                        '+ISNULL(DOCINVNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),DOCRCVDT,103),'(NULL)')+'  '+ISNULL(CRFNO,'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),CRFDT,103),'(NULL)')+'  '+ISNULL(BENO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),BEDT,103),'(NULL)')+'  
                        '+ISNULL(BLNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),BLDT,103),'(NULL)')+'  '+ISNULL(BTNO,'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),BTDT,103),'(NULL)')+'  '+ISNULL(LCNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),LCDT,103),'(NULL)')+'  
                        '+ISNULL(LCANO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),LCADT,103),'(NULL)')+'  '+ISNULL(PERMITNO,'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),PERMITDT,103),'(NULL)')+'  '+ISNULL(BILLNOM,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),BILLDTM,103),'(NULL)')+'  
                        '+ISNULL(MVESSEL,'(NULL)')+'  '+ISNULL(FVESSEL,'(NULL)')+'  '+ISNULL(MARKSNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ETB,103),'(NULL)')+'  
                        '+ISNULL(BERTHSNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),ARRIVEDT,103),'(NULL)')+'  '+ISNULL(ROTNO,'(NULL)')+'  
                        '+ISNULL(LINE_NO,'(NULL)')+'  '+ISNULL(CLRDON,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),FRWDT,103),'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),DELIVERYDT,103),'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),CLDT,103),'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),WFDT,103),'(NULL)')+'  '+ISNULL(AWBNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),AWBDT,103),'(NULL)')+'  
                        '+ISNULL(HBLNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),HBLDT,103),'(NULL)')+'  '+ISNULL(HAWBNO,'(NULL)')+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),HAWBDT,103),'(NULL)')+'  '+ISNULL(UNTKNO,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),UNTKDT,103),'(NULL)')+'  
                        '+ISNULL(COM_REMARKS,'(NULL)')+'  '+ISNULL(CONVERT(NVARCHAR(50),STATUS,103),'(NULL)')+'  '+ISNULL(USERPC,'(NULL)')+'  
                        '+ISNULL(USERID,'(NULL)')+'  '+ISNULL(UPDATEUSERID,'(NULL)')+'  '+CONVERT(NVARCHAR(50),INTIME,103)+'  
                        '+ISNULL(CONVERT(NVARCHAR(50),UPDATETIME,103),'(NULL)')+'  '+IPADDRESS+'  '+ISNULL(FRWNO,'(NULL)')+'  '+ISNULL(PKGSTP,'(NULL)')+'  
                        '+ISNULL(REFTP,'(NULL)')+'  '+ISNULL(VATNO,'(NULL)') FROM CNF_JOB 
                        WHERE JOBYY =" + iob.JobYear + " AND JOBTP ='" + iob.JobTP + "' AND JOBNO =" + iob.JobNo + "");
                            SqlConnection con = new SqlConnection(dbFunctions.Connection);
                            SqlCommand cmdinsert = new SqlCommand("insert into ASL_DLT values('CNF_JOB-UPDATE',@DESCRP,@USERPC,@USERID,@INTIME,@IPADD)", con);
                            cmdinsert.Parameters.AddWithValue("@DESCRP", alldata);
                            cmdinsert.Parameters.AddWithValue("@USERPC", iob.Userpc);
                            cmdinsert.Parameters.AddWithValue("@USERID", iob.UpdateUser);
                            cmdinsert.Parameters.AddWithValue("@INTIME", iob.InTM);
                            cmdinsert.Parameters.AddWithValue("@IPADD", iob.Ipaddress);
                            con.Open();
                            cmdinsert.ExecuteNonQuery();
                            con.Close();

                        }
                        catch (Exception)
                        {

                            throw;
                        }

                        dob.update_cnf_job(iob);
                    }
                    Refresh();

                    if (btnEdit.Text == "Edit")
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "Job No " + iob.JobNo + " Informations are Successfully Saved.";

                        dbFunctions.popupAlert(Page, "Job No " + iob.JobNo + " Informations are Successfully Saved", "w");
                    }
                    else
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "Job No " + iob.JobNo + " Informations are Successfully Updated.";

                        dbFunctions.popupAlert(Page, "Job No " + iob.JobNo + " Informations are Successfully Updated", "w");
                    }
                }
            }
        }
        protected void ddlSuppNM_OnTextChanged(object sender, EventArgs e)
        {
            txtGoodsDesc.Focus();
        }
        protected void txtCNFVUSD_TextChanged(object sender, EventArgs e)
        {
            txtChangeRT.Text = ".00";
            txtChangeRT.Focus();
        }

        protected void txtChangeRT_TextChanged(object sender, EventArgs e)
        {
            if (txtCNFVUSD.Text == "")
                txtCNFVUSD.Text = "0";
            else if (txtChangeRT.Text == "")
                txtChangeRT.Text = "0";

            if (txtPartyID.Text == "")
            {
                //lblError.Visible = true;
                //lblError.Text = "Select Party Name.";
                dbFunctions.popupAlert(Page, "Select Party Name.", "w");
                txtChangeRT.Text = ".00";
                txtPartyNM.Focus();
            }
            else
            {
                txtCNFVBDT.Text = (Convert.ToDecimal(txtCNFVUSD.Text) * Convert.ToDecimal(txtChangeRT.Text)).ToString();
                txtAssessableVal.Text = txtCNFVBDT.Text;

                con = new SqlConnection(dbFunctions.Connection);
                if (con.State != ConnectionState.Open) con.Open();

                cmd = new SqlCommand("SELECT EXCTP, VALUETP, COMMAMT FROM CNF_COMMISSION WHERE (PARTYID = '" + txtPartyID.Text + "') AND " + txtAssessableVal.Text + " BETWEEN VALUEFR AND VALUETO AND EXCTP='BDT' AND REGID ='" + ddlRegID.Text + "' AND JOBQUALITY ='" + ddlJobQuality.Text + "'" +
                        " UNION ALL " +
                        " SELECT EXCTP, VALUETP, COMMAMT FROM CNF_COMMISSION WHERE (PARTYID = '" + txtPartyID.Text + "') AND " + txtCNFVUSD.Text + " BETWEEN VALUEFR AND VALUETO AND EXCTP='USD' AND REGID ='" + ddlRegID.Text + "' AND JOBQUALITY ='" + ddlJobQuality.Text + "'", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (con.State != ConnectionState.Closed) con.Close();

                if (ds.Tables[0].Rows.Count > 1)
                {
                    //lblError.Visible = true;
                    //lblError.Text = "An error occured. Please check party commission form.";
                    dbFunctions.popupAlert(Page, "An error occured. Please check party commission form.", "w");
                    txtChangeRT.Text = ".00";
                    txtCommission.Text = ".00";
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    GridView1.Visible = false;
                    txtChangeRT.Focus();
                }
                else if (ds.Tables[0].Rows.Count > 0)
                {
                    lblError.Visible = false;
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    GridView1.Visible = false;

                    foreach (GridViewRow grid in GridView1.Rows)
                    {
                        try
                        {
                            string comMainVal = "0";
                            if (grid.Cells[0].Text == "BDT")
                                comMainVal = txtAssessableVal.Text;
                            else
                                comMainVal = txtAssessableVal.Text;

                            if (grid.Cells[2].Text == "")
                                grid.Cells[2].Text = ".00";

                            if (grid.Cells[1].Text == "AMT")
                            {
                                txtCommission.Text = Math.Round(Convert.ToDecimal(grid.Cells[2].Text)).ToString(CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                txtCommission.Text = Math.Round(Convert.ToDecimal(comMainVal) * ((Convert.ToDecimal(grid.Cells[2].Text)) / 100)).ToString("F");
                            }

                            txtExTP.Focus();
                        }
                        catch (Exception ex)
                        {
                            lblError.Visible = true;
                            lblError.Text = ex.Message;
                        }
                    }
                }
                else
                {
                    lblError.Visible = false;
                    txtCommission.Text = ".00";
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    GridView1.Visible = false;
                    txtExTP.Focus();
                }
            }
        }
        protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
            con = new SqlConnection(dbFunctions.Connection);

            if (con.State != ConnectionState.Open) con.Open();
            cmd = new SqlCommand("SELECT DOCINVNO FROM CNF_JOB WHERE DOCINVNO =@DOCINVNO", con);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@DOCINVNO", txtInvoiceNo.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (con.State != ConnectionState.Closed) con.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //lblError.Visible = true;
                //lblError.Text = "Invoice no used in other job no.";
                dbFunctions.popupAlert(Page, "Invoice no used in other job no.", "w");
                txtInvoiceNo.Text = "";
                txtInvoiceDT.Text = "";
                txtInvoiceNo.Focus();
            }
            else
            {
                lblError.Visible = false;
                txtInvoiceDT.Focus();
            }
        }
        protected void txtInvoiceDT_TextChanged(object sender, EventArgs e)
        {
            txtBENO.Focus();
        }
        protected void txtLCDT_TextChanged(object sender, EventArgs e)
        {
            txtPermitNO.Focus();
        }
        protected void txtBLDT_TextChanged(object sender, EventArgs e)
        {
            txtLCNO.Focus();
        }
        protected void txtPermitNO_TextChanged(object sender, EventArgs e)
        {
            con = new SqlConnection(dbFunctions.Connection);

            if (con.State != ConnectionState.Open) con.Open();
            cmd = new SqlCommand("SELECT PERMITNO FROM CNF_JOB WHERE PERMITNO =@PERMITNO", con);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@PERMITNO", txtPermitNO.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (con.State != ConnectionState.Closed) con.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //lblError.Visible = true;
                //lblError.Text = "Permit no used in other job no.";
                dbFunctions.popupAlert(Page, "Permit no used in other job no.", "w");
                txtPermitNO.Text = "";
                txtPermitDT.Text = "";
                txtPermitNO.Focus();
            }
            else
            {
                lblError.Visible = false;
                txtPermitDT.Focus();
            }
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Focus();
        }
        protected void txtUnderTakeDt_TextChanged(object sender, EventArgs e)
        {
            txtContainerNo.Focus();
        }
        protected void txtHawbDT_TextChanged(object sender, EventArgs e)
        {
            txtUnderTakeNo.Focus();
        }
        protected void txtAwbDT_TextChanged(object sender, EventArgs e)
        {
            txtHBlNo.Focus();
        }
        protected void txtHblDT_TextChanged(object sender, EventArgs e)
        {
            txtHawbNo.Focus();
        }
        protected void txtPermitDT_TextChanged(object sender, EventArgs e)
        {
            ddlStatus.Focus();
        }
        protected void txtDelDT_TextChanged(object sender, EventArgs e)
        {
            txtAssessableVal.Focus();
        }
        protected void txtBEDT_TextChanged(object sender, EventArgs e)
        {
            txtBLNO.Focus();
        }
        protected void txtCRFDT_TextChanged(object sender, EventArgs e)
        {
            txtInvoiceNo.Focus();
        }
        protected void txtFORWRDDT_TextChanged(object sender, EventArgs e)
        {
            txtAssessableVal.Focus();
        }
    }
}