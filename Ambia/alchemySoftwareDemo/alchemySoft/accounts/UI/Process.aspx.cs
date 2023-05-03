using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using DynamicMenu;
using System.Data.SqlClient;
using System.Web.Services;
using System.Text;
using System.Collections.Specialized;
using AlchemyAccounting;
using System.Net;
using alchemy.accounts.DataAccess;
using alchemy.accounts.Interface;
using alchemySoft;

namespace alchemy.accounts.UI
{
    public partial class Process : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        AccountDataAccess dob = new AccountDataAccess();
        AccountInterface iob = new AccountInterface();
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                const string formLink = "/Accounts/ui/Process.aspx";
                bool permission = dbFunctions.checkParmit(formLink, "STATUS");
                if (permission == true)
                {
                    if (!IsPostBack)
                    {
                        lblSerial_Mrec.Visible = false;
                        lblSerial_Mpay.Visible = false;
                        lblSerial_Jour.Visible = false;
                        lblSerial_Cont.Visible = false;
                        DateTime today = DateTime.Today.Date;
                        string td = dbFunctions.timezone(DateTime.Now).ToString("dd/MM/yyyy");
                        txtDate.Text = td;
                        btnProcess.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/permission.aspx");
                }
            }
        }
        public void frf_Do_ShowGrid()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@" SELECT  CONVERT(NVARCHAR(10),DODT,103) DODT,DOYY, DONO, CNFAGID, ISNULL(DOAMT,0) DOAMT, ISNULL(VATAMT,0) VATAMT, 'HB/L: '+BLNO+' '+REMARKS REMARKS
FROM   FRF_DO WHERE  DODT = '" + p_Date + "' ORDER BY DODT, DONO", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView5_frf_Do.DataSource = ds;
                GridView5_frf_Do.DataBind();
                GridView5_frf_Do.Visible = false;
            }
            else
            {
                GridView5_frf_Do.DataSource = ds;
                GridView5_frf_Do.DataBind();
                GridView5_frf_Do.Visible = false;
                //Response.Write("<script>alert('No Data Found');</script>");
                //GridView1.Visible = false;
            }
        }
        public void frf_Mr_ShowGrid()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT  CONVERT(NVARCHAR(10),A.MRDT,103) MRDT ,A.MRYY, A.MRNO, A.CNFAGID, A.CBID, SUM(ISNULL(AMOUNT,0)) MRAMT, 'HB/L: '+A.BLNO+' '+A.REMARKS REMARKS
FROM   FRF_MRMST A INNER JOIN FRF_MR B ON A.MRDT = B.MRDT AND A.MRNO = B.MRNO WHERE A.MRDT ='" + p_Date + "' AND A.CNFAGID<>''  AND  A.CBID<>'' " +
"GROUP BY A.MRDT,A.MRYY, A.MRNO, A.CNFAGID, A.CBID, A.BLNO, A.REMARKS ORDER BY A.MRDT, A.MRNO", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView6_frf_Mr.DataSource = ds;
                GridView6_frf_Mr.DataBind();
                GridView6_frf_Mr.Visible = false;
            }
            else
            {
                GridView6_frf_Mr.DataSource = ds;
                GridView6_frf_Mr.DataBind();
                GridView6_frf_Mr.Visible = false;
                //Response.Write("<script>alert('No Data Found');</script>");
                //GridView1.Visible = false;
            }
        }
        public void frf_DNote_ShowGrid()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT CONVERT(NVARCHAR(10),A.TRANSDT,103) TRANSDT,A.TRANSYY, A.TRANSNO, A.TRANSTP, A.PSID, SUM(ISNULL(BDTAMT,0)) AMOUNT, 
A.NOTETP+' NOTE'+' HB/L: '+A.HBLNO+' INV.NO: '+A.TRANSNO+' JOB NO: '+A.JOBNO+' '+A.REMARKS REMARKS
FROM   FRF_DCNMST A INNER JOIN FRF_DCNOTE B ON A.NOTETP = B.NOTETP AND A.TRANSTP = B.TRANSTP AND A.TRANSDT = B.TRANSDT AND A.TRANSNO = B.TRANSNO
WHERE  A.NOTETP = 'DEBIT' AND A.TRANSDT =  '" + p_Date + "' " +
"GROUP BY A.TRANSDT,A.TRANSYY, A.TRANSNO, A.TRANSTP, A.PSID, A.HBLNO, A.JOBNO, A.REMARKS, A.NOTETP", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView7_frf_DNote.DataSource = ds;
                GridView7_frf_DNote.DataBind();
                GridView7_frf_DNote.Visible = false;
            }
            else
            {
                GridView7_frf_DNote.DataSource = ds;
                GridView7_frf_DNote.DataBind();
                GridView7_frf_DNote.Visible = false;
                //Response.Write("<script>alert('No Data Found');</script>");
                //GridView1.Visible = false;
            }
        }
        public void frf_CNote_ShowGrid()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT CONVERT(NVARCHAR(10),A.TRANSDT,103) TRANSDT, A.TRANSNO, A.TRANSTP, A.PSID, SUM(ISNULL(BDTAMT,0)) AMOUNT, 
A.NOTETP+' NOTE'+' HB/L: '+A.HBLNO+' INV.NO: '+A.TRANSNO+' JOB NO: '+A.JOBNO+' '+A.REMARKS REMARKS
FROM   FRF_DCNMST A INNER JOIN FRF_DCNOTE B ON A.NOTETP = B.NOTETP AND A.TRANSTP = B.TRANSTP AND A.TRANSDT = B.TRANSDT AND A.TRANSNO = B.TRANSNO
WHERE  A.NOTETP = 'CREDIT' AND A.TRANSDT = '" + p_Date + "' " +
"GROUP BY A.TRANSDT, A.TRANSNO, A.TRANSTP, A.PSID, A.HBLNO, A.JOBNO, A.REMARKS, A.NOTETP", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView7_frf_CNote.DataSource = ds;
                GridView7_frf_CNote.DataBind();
                GridView7_frf_CNote.Visible = false;
            }
            else
            {
                GridView7_frf_CNote.DataSource = ds;
                GridView7_frf_CNote.DataBind();
                GridView7_frf_CNote.Visible = false;
                //Response.Write("<script>alert('No Data Found');</script>");
                //GridView1.Visible = false;
            }
        }
        public void ShowGrid()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, convert(nvarchar(20),CHEQUEDT,103) as CHEQUEDT, AMOUNT, REMARKS, USERPC, USERID, ACTDTI, INTIME, IPADDRESS " +
                                            " FROM dbo.GL_STRANS where TRANSDT = '" + p_Date + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = false;
            }
            else
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = false;
                //Response.Write("<script>alert('No Data Found');</script>");
                //GridView1.Visible = false;
            }
        }
        public void ShowGrid_Multiple()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, convert(nvarchar(20),CHEQUEDT,103) as CHEQUEDT, AMOUNT, REMARKS, USERPC, USERID, ACTDTI, INTIME, IPADDRESS " +
                                            " FROM GL_MTRANS where TRANSDT = '" + p_Date + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridMultiple.DataSource = ds;
                gridMultiple.DataBind();
                gridMultiple.Visible = false;
            }
            else
            {
                gridMultiple.DataSource = ds;
                gridMultiple.DataBind();
                gridMultiple.Visible = false;
                //Response.Write("<script>alert('No Data Found');</script>");
                //GridView1.Visible = false;
            }
        }
        public void UserServerInfo()
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            iob.ip = ipAddress.ToString();
            iob.PcName = strHostName.ToString();
            iob.inTime = dbFunctions.timezone(DateTime.Now);
            iob.userName = CookiesData["UserName"].ToString();
        }
        public void ShowGrid_Purchase()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT " +
                                            " from STK_TRANS where TRANSDT='" + p_Date + "' and LCTP='LOCAL' and TRANSTP='BUY' and TRANSDT <> '2015-04-07 00:00:00' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView2.DataSource = ds;
                GridView2.DataBind();
                GridView2.Visible = false;
            }
            else
            {
                GridView2.DataSource = ds;
                GridView2.DataBind();
                GridView2.Visible = false;
            }
        }
        public void ShowGrid_Purchase_Ret()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT " +
                                            " from STK_TRANS where TRANSDT='" + p_Date + "' and TRANSTP='IRTB' and TRANSDT <> '2015-04-07 00:00:00' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridPurchase_Ret.DataSource = ds;
                gridPurchase_Ret.DataBind();
                gridPurchase_Ret.Visible = false;
            }
            else
            {
                gridPurchase_Ret.DataSource = ds;
                gridPurchase_Ret.DataBind();
                gridPurchase_Ret.Visible = false;
            }
        }

        public void ShowGrid_Sale()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT STK_TRANSMST.TRANSMY, STK_TRANSMST.TRANSNO, STK_TRANSMST.STOREFR, STK_TRANSMST.PSID, STK_TRANSMST.TOTNET, STK_TRANSMST.CASHAMT, STK_TRANSMST.REMARKS, STK_STORE.COMPID " +
                    " FROM STK_STORE INNER JOIN STK_TRANSMST ON STK_STORE.STOREID = STK_TRANSMST.STOREFR WHERE STK_TRANSMST.TRANSDT='" + p_Date + "' AND STK_TRANSMST.TRANSTP = 'SALE' ORDER BY STK_TRANSMST.TRANSNO ", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView3.DataSource = ds;
                GridView3.DataBind();
                GridView3.Visible = false;
            }
            else
            {
                GridView3.DataSource = ds;
                GridView3.DataBind();
                GridView3.Visible = false;
            }
        }

        //public void ShowGrid_Sale_Discount()
        //{

        //    DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
        //    string p_Date = Pdate.ToString("yyyy/MM/dd");

        //    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(connectionString);

        //    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
        //    SqlCommand cmd = new SqlCommand(" SELECT * FROM " +
        //                                        " (SELECT A.TRANSTP, A.TRANSDT, A.TRANSMY, A.TRANSNO, A.STOREFR, A.PSID, A.REMARKS, (A.DISCAMT+STK_TRANSMST.DISAMT) AS DISAMT " +
        //                                        " FROM  (SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, INVREFNO, STOREFR, STORETO, PSID, SUM(DISCAMT) AS DISCAMT, REMARKS " +
        //                                                " FROM          STK_TRANS " +
        //                                                " WHERE (TRANSTP = 'SALE') AND (TRANSDT = '" + p_Date + "') " +
        //                                                " GROUP BY TRANSTP, TRANSDT, TRANSMY, TRANSNO, INVREFNO, STOREFR, STORETO, PSID,REMARKS) AS A INNER JOIN " +
        //                                    " STK_TRANSMST ON A.TRANSTP = STK_TRANSMST.TRANSTP AND A.TRANSDT = STK_TRANSMST.TRANSDT AND A.TRANSMY = STK_TRANSMST.TRANSMY AND " +
        //                                    " A.TRANSNO = STK_TRANSMST.TRANSNO AND A.INVREFNO = STK_TRANSMST.INVREFNO AND A.STOREFR = STK_TRANSMST.STOREFR AND " +
        //                                    " A.STORETO = STK_TRANSMST.STORETO AND A.PSID = STK_TRANSMST.PSID) AS B WHERE B.DISAMT<>0", conn);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        GridView4.DataSource = ds;
        //        GridView4.DataBind();
        //        GridView4.Visible = false;
        //    }
        //    else
        //    {
        //        GridView4.DataSource = ds;
        //        GridView4.DataBind();
        //        GridView4.Visible = false;
        //    }
        //}

        public void ShowGrid_Sale_Ret()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT from STK_TRANS where TRANSDT='" + p_Date + "' and TRANSTP='IRTS' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridSale_Ret.DataSource = ds;
                gridSale_Ret.DataBind();
                gridSale_Ret.Visible = false;
            }
            else
            {
                gridSale_Ret.DataSource = ds;
                gridSale_Ret.DataBind();
                gridSale_Ret.Visible = false;
            }
        }
        public void ShowGridLC()
        {
            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, LCCD, CNBCD, AMOUNT, (CASE WHEN LCINVNO = '' THEN REMARKS WHEN REMARKS = '' THEN LCINVNO ELSE (REMARKS + ' - ' + LCINVNO)END)AS REMARKS FROM LC_EXPENSE " +
                                            " WHERE (TRANSDT = '" + p_Date + "')", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridLC.DataSource = ds;
                gridLC.DataBind();
                gridLC.Visible = false;
            }
            else
            {
                gridLC.DataSource = ds;
                gridLC.DataBind();
                gridLC.Visible = false;
            }
        }
        public void ShowGrid_Purchase_Import()
        {
            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT " +
                                            " from STK_TRANS where TRANSDT='" + p_Date + "' and LCTP='IMPORT' and TRANSTP='BUY' and TRANSDT <> '2015-04-07 00:00:00' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBuyImport.DataSource = ds;
                gvBuyImport.DataBind();
                gvBuyImport.Visible = false;
            }
            else
            {
                gvBuyImport.DataSource = ds;
                gvBuyImport.DataBind();
                gvBuyImport.Visible = false;
            }
        }
        int i = 1;
        public void frf_Do_Insert()
        {
            string userName = CookiesData["USERNAME"].ToString(); ;
            string serialNo = "";
            int sl, serial;
            iob.Username = userName;
            DateTime Transdt = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string trans_DT = Transdt.ToString("yyyy/MM/dd");
            if (i == 1 || i == 2)
            {
                foreach (GridViewRow grid in GridView5_frf_Do.Rows)
                {
                    try
                    {
                        iob.Transtp = "JOUR";
                        DateTime Transdt1 = DateTime.Parse(grid.Cells[0].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                        iob.Transdt = Transdt1;
                        string mY = Transdt1.ToString("MMM-yy").ToUpper();
                        iob.Monyear = mY;
                        iob.tableid = "FRF_DO";
                        iob.TransNo = grid.Cells[2].Text;
                        iob.Debitcd = grid.Cells[3].Text;
                        if (i == 1)
                        {
                            dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '31%' ", lblSerial_Do);
                            if (lblSerial_Do.Text == "")
                            {
                                serialNo = "31000";
                                iob.Serial_Do = serialNo;
                            }
                            else
                            {
                                sl = int.Parse(lblSerial_Do.Text);
                                serial = sl + 1;
                                iob.Serial_Do = serial.ToString();
                            }
                            iob.Creditcd = "301010100001";
                            iob.Amount = Convert.ToDecimal(grid.Cells[4].Text);
                        }
                        else
                        {
                            dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '32%' ", lblSerial_Do);
                            if (lblSerial_Do.Text == "")
                            {
                                serialNo = "32000";
                                iob.Serial_Do = serialNo;
                            }
                            else
                            {
                                sl = int.Parse(lblSerial_Do.Text);
                                serial = sl + 1;
                                iob.Serial_Do = serial.ToString();
                            }
                            iob.Creditcd = "202030100001";
                            iob.Amount = Convert.ToDecimal(grid.Cells[5].Text);
                        }
                        iob.Remarks = grid.Cells[6].Text;
                        dob.doProcess_DO(iob);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }
                }
                i++;
                frf_Do_Insert();
            }


        }
        public void frf_Mr_Insert()
        {
            string userName = CookiesData["USERNAME"].ToString(); ;
            string serialNo = "";
            int sl, serial;
            iob.Username = userName;
            DateTime Transdt = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string trans_DT = Transdt.ToString("yyyy/MM/dd");
            foreach (GridViewRow grid in GridView6_frf_Mr.Rows)
            {
                try
                {
                    dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MREC' and SERIALNO like '33%' ", lblSerial_Mrec);
                    if (lblSerial_Mrec.Text == "")
                    {
                        serialNo = "33000";
                        iob.Serial_Mr = serialNo;
                    }
                    else
                    {
                        sl = int.Parse(lblSerial_Mrec.Text);
                        serial = sl + 1;
                        iob.Serial_Mr = serial.ToString();
                    }
                    iob.Transtp = "MREC";
                    DateTime Transdt1 = DateTime.Parse(grid.Cells[0].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.Transdt = Transdt1;
                    string mY = Transdt1.ToString("MMM-yy").ToUpper();
                    iob.Monyear = mY;
                    iob.tableid = "FRF_MR";
                    iob.TransNo = grid.Cells[2].Text;
                    iob.Creditcd = grid.Cells[3].Text;
                    iob.Debitcd = grid.Cells[4].Text;
                    iob.Amount = Convert.ToDecimal(grid.Cells[5].Text);
                    iob.Remarks = grid.Cells[6].Text;
                    dob.doProcess_MR(iob);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

        }
        public void frf_DNote_Insert()
        {
            string userName = CookiesData["USERNAME"].ToString(); ;
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in GridView7_frf_DNote.Rows)
            {
                try
                {
                    dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '34%' ", lblSerial_DNote);
                    if (lblSerial_DNote.Text == "")
                    {
                        serialNo = "34000";
                        iob.Serial_Dnote = serialNo;
                    }
                    else
                    {
                        sl = int.Parse(lblSerial_DNote.Text);
                        serial = sl + 1;
                        iob.Serial_Dnote = serial.ToString();
                    }

                    iob.Transtp = "JOUR";
                    DateTime Transdt1 = DateTime.Parse(grid.Cells[0].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.Transdt = Transdt1;
                    string mY = Transdt1.ToString("MMM-yy").ToUpper();
                    iob.Monyear = mY;
                    iob.TransNo = dbFunctions.getData("SELECT isnull(MAX(TRANSNO),0)+1 FROM GL_MASTER WHERE TRANSMY='" + iob.Monyear + "' AND TRANSTP = 'JOUR' and SERIALNO like '34%' ");
                    //iob.Transtp = grid.Cells[3].Text;
                    iob.tableid = "FRF_DCNOTE";
                    iob.Debitcd = grid.Cells[4].Text;
                    iob.Creditcd = "301010100001";
                    iob.Amount = Convert.ToDecimal(grid.Cells[5].Text);
                    iob.Remarks = grid.Cells[6].Text;
                    dob.doProcess_DNote(iob);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

        }
        public void frf_CNote_Insert()
        {
            string userName = CookiesData["USERNAME"].ToString(); ;
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in GridView7_frf_CNote.Rows)
            {
                try
                {
                    dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '35%' ", lblSerial_CNote);
                    if (lblSerial_CNote.Text == "")
                    {
                        serialNo = "35000";
                        iob.Serial_Cnote = serialNo;
                    }
                    else
                    {
                        sl = int.Parse(lblSerial_CNote.Text);
                        serial = sl + 1;
                        iob.Serial_CNote = serial.ToString();
                    }

                    iob.Transtp = "JOUR";
                    DateTime Transdt1 = DateTime.Parse(grid.Cells[0].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                    iob.Transdt = Transdt1;
                    string mY = Transdt1.ToString("MMM-yy").ToUpper();
                    iob.Monyear = mY;
                    iob.TransNo = dbFunctions.getData("SELECT isnull(MAX(TRANSNO),0)+1 FROM GL_MASTER WHERE TRANSMY='" + iob.Monyear + "' AND TRANSTP = 'JOUR' and SERIALNO like '35%' ");
                    //iob.Transtp = grid.Cells[3].Text;
                    iob.tableid = "FRF_DCNOTE";
                    iob.Debitcd = "401010100001";
                    iob.Creditcd = grid.Cells[3].Text;
                    iob.Amount = Convert.ToDecimal(grid.Cells[4].Text);
                    iob.Remarks = grid.Cells[5].Text;
                    dob.doProcess_CNote(iob);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

        }
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            UserServerInfo();
            if (CookiesData == null)
            {
                Response.Redirect("~/login/ui/SignIn");
            }
            else
            {
                if (txtDate.Text == "")
                {
                    Response.Write("<script>alert('Select a Date want to process?');</script>");
                }
                else
                {
                    string userName = CookiesData["USERNAME"].ToString(); ;
                    string serialNo = "";
                    int sl, serial;

                    ShowGrid();
                    ShowGrid_Multiple();



                    iob.Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
                    string trans_DT = iob.Transdt.ToString("yyyy/MM/dd");
                    iob.Username = userName;

                    string connectionString = ConfigurationManager.ConnectionStrings["Alchemy_Acc"].ConnectionString;
                    SqlConnection conn = new SqlConnection(connectionString);
                    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                    SqlCommand cmd = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'GL_STRANS' and TRANSTP <> 'OPEN'", conn);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'GL_MTRANS' and TRANSTP <> 'OPEN'", conn);
                    cmd3.CommandTimeout = 0;
                    cmd3.ExecuteNonQuery();

                    SqlCommand cmd1 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'STK_TRANS' and TRANSTP IN ('JOUR','MREC')", conn);
                    cmd1.CommandTimeout = 0;
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'LC_EXPENSE' and TRANSTP='MPAY'", conn);
                    cmd2.CommandTimeout = 0;
                    cmd2.ExecuteNonQuery();
                    //////
                    SqlCommand cmd10 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'FRF_MR' and TRANSTP='MREC'", conn);
                    cmd10.CommandTimeout = 0;
                    cmd10.ExecuteNonQuery();
                    SqlCommand cmd11 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'FRF_DO' and TRANSTP='JOUR'", conn);
                    cmd11.CommandTimeout = 0;
                    cmd11.ExecuteNonQuery();
                    SqlCommand cmd12 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'FRF_DCNOTE' and TRANSTP='JOUR'", conn);
                    cmd12.CommandTimeout = 0;
                    cmd12.ExecuteNonQuery();
                    SqlCommand cmd13 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'FRF_DCNOTE' and TRANSTP='JOUR'", conn);
                    cmd13.CommandTimeout = 0;
                    cmd13.ExecuteNonQuery();
                    ///////////////////
                    frf_Do_ShowGrid();
                    frf_Mr_ShowGrid();
                    frf_DNote_ShowGrid();
                    frf_CNote_ShowGrid();
                    frf_Do_Insert();
                    frf_Mr_Insert();
                    frf_DNote_Insert();
                    frf_CNote_Insert();
                    //SqlCommand cmd4 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'MC_COLLECT' and TRANSTP='MREC'", conn);
                    //cmd4.ExecuteNonQuery();

                    //SqlCommand cmd5 = new SqlCommand("Delete from MC_MLEDGER where TRANSDT='" + trans_DT + "' and TABLEID = 'MC_COLLECT' and TRANSTP='MREC'", conn);
                    //cmd5.ExecuteNonQuery();

                    if (conn.State != System.Data.ConnectionState.Closed) conn.Close();

                    foreach (GridViewRow grid in GridView1.Rows)
                    {
                        try
                        {
                            if (grid.Cells[0].Text == "MREC")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MREC' AND TABLEID='GL_STRANS' ", lblSerial_Mrec);
                                if (lblSerial_Mrec.Text == "")
                                {
                                    serialNo = "1000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Mrec.Text);
                                    serial = sl + 1;

                                    iob.SerialNo_MREC = serial.ToString();
                                }

                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                //if (grid.Cells[10].Text == "&nbsp;")
                                //{
                                //    iob.Chequeno = null;
                                //}
                                //else
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;



                                dob.doProcess_MREC(iob);
                            }
                            else if (grid.Cells[0].Text == "MPAY")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MPAY'  AND TABLEID='GL_STRANS'", lblSerial_Mpay);
                                if (lblSerial_Mpay.Text == "")
                                {
                                    serialNo = "2000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Mpay.Text);
                                    serial = sl + 1;
                                    iob.SerialNo_MREC = serial.ToString();
                                }
                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;

                                dob.doProcess_MPAY(iob);
                            }
                            else if (grid.Cells[0].Text == "JOUR")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR'  AND TABLEID='GL_STRANS'", lblSerial_Jour);
                                if (lblSerial_Jour.Text == "")
                                {
                                    serialNo = "3000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Jour.Text);
                                    serial = sl + 1;
                                    iob.SerialNo_MREC = serial.ToString();
                                }
                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;

                                dob.doProcess_JOUR(iob);
                            }
                            else if (grid.Cells[0].Text == "CONT")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'CONT'  AND TABLEID='GL_STRANS'", lblSerial_Cont);
                                if (lblSerial_Cont.Text == "")
                                {
                                    serialNo = "4000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Cont.Text);
                                    serial = sl + 1;
                                    iob.SerialNo_MREC = serial.ToString();
                                }
                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;


                                dob.doProcess_CONT(iob);
                            }


                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                        }
                    }

                    foreach (GridViewRow grid in gridMultiple.Rows)
                    {
                        try
                        {
                            if (grid.Cells[0].Text == "MREC")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MREC'  AND TABLEID='GL_MTRANS'", lblSerial_Mrec);
                                if (lblSerial_Mrec.Text == "")
                                {
                                    serialNo = "5000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Mrec.Text);
                                    serial = sl + 1;

                                    iob.SerialNo_MREC = serial.ToString();
                                }

                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                //if (grid.Cells[10].Text == "&nbsp;")
                                //{
                                //    iob.Chequeno = null;
                                //}
                                //else
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;



                                dob.doProcess_MREC_Multiple(iob);
                            }
                            else if (grid.Cells[0].Text == "MPAY")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MPAY'   AND TABLEID='GL_MTRANS'", lblSerial_Mpay);
                                if (lblSerial_Mpay.Text == "")
                                {
                                    serialNo = "6000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Mpay.Text);
                                    serial = sl + 1;
                                    iob.SerialNo_MREC = serial.ToString();
                                }
                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;

                                dob.doProcess_MPAY_Multiple(iob);
                            }
                            else if (grid.Cells[0].Text == "JOUR")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR'   AND TABLEID='GL_MTRANS'", lblSerial_Jour);
                                if (lblSerial_Jour.Text == "")
                                {
                                    serialNo = "7000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Jour.Text);
                                    serial = sl + 1;
                                    iob.SerialNo_MREC = serial.ToString();
                                }
                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;

                                dob.doProcess_JOUR_Multiple(iob);
                            }
                            else if (grid.Cells[0].Text == "CONT")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'CONT'   AND TABLEID='GL_MTRANS'", lblSerial_Cont);
                                if (lblSerial_Cont.Text == "")
                                {
                                    serialNo = "8000";
                                    iob.SerialNo_MREC = serialNo;
                                }
                                else
                                {
                                    sl = int.Parse(lblSerial_Cont.Text);
                                    serial = sl + 1;
                                    iob.SerialNo_MREC = serial.ToString();
                                }
                                iob.Transtp = grid.Cells[0].Text;
                                iob.Monyear = grid.Cells[2].Text;
                                iob.TransNo = grid.Cells[3].Text;
                                iob.Transfor = grid.Cells[5].Text;
                                iob.Costpid = grid.Cells[6].Text;
                                iob.Transmode = grid.Cells[7].Text;
                                iob.Debitcd = grid.Cells[8].Text;
                                iob.Creditcd = grid.Cells[9].Text;
                                iob.Chequeno = grid.Cells[10].Text;
                                iob.Chequedt = DateTime.Parse(grid.Cells[11].Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                                iob.Amount = Convert.ToDecimal(grid.Cells[12].Text);
                                string Remarks = grid.Cells[13].Text;
                                if (Remarks == "&nbsp;")
                                {
                                    iob.Remarks = "";
                                }
                                else
                                    iob.Remarks = Remarks;

                                dob.doProcess_CONT_Multiple(iob);
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                        }
                    }

                    //MicroCreditCollection_Process();
                    //MicroCreditCollectionMember_Process();

                    Response.Write("<script>alert('Process Completed.');</script>");
                }
            }
        }
    }
}