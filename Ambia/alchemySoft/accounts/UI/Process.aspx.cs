using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using DynamicMenu;
using System.Data.SqlClient;
using alchemy.accounts.DataAccess;
using alchemy.accounts.Interface;

namespace alchemySoft.accounts.ui
{
    public partial class Process : System.Web.UI.Page
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data
        AccountDataAccess dob = new AccountDataAccess();
        AccountInterface iob = new AccountInterface();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CookiesData == null)
            {
                Response.Redirect("~/Login/UI/Login.aspx");
            }
            else
            {
                string formLink = "/Accounts/UI/Process.aspx";
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
                    Response.Redirect("/Default.aspx");
                }
            }
        }

        public void ShowGrid()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, convert(nvarchar(20),CHEQUEDT,103) as CHEQUEDT, AMOUNT, REMARKS, USERPC, USERID, ACTDTI, INTIME, IPADDRESS " +
                                            " FROM dbo.GL_STRANS where TRANSDT = '" + p_Date + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, CHEQUENO, convert(nvarchar(20),CHEQUEDT,103) as CHEQUEDT, AMOUNT, REMARKS, USERPC, USERID, ACTDTI, INTIME, IPADDRESS " +
                                            " FROM GL_MTRANS where TRANSDT = '" + p_Date + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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

        public void ShowGrid_Purchase()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT " +
                                            " from STK_TRANS where TRANSDT='" + p_Date + "' and LCTP='LOCAL' and TRANSTP='BUY' and TRANSDT <> '2013-04-30 00:00:00' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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
        public void ShowGrid_Purchase_IMPORT()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT " +
                                            " from STK_TRANS where TRANSDT='" + p_Date + "' and LCTP='IMPORT' and TRANSTP='BUY' and TRANSDT <> '2013-04-30 00:00:00' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView22.DataSource = ds;
                GridView22.DataBind();
                GridView22.Visible = false;
            }
            else
            {
                GridView22.DataSource = ds;
                GridView22.DataBind();
                GridView22.Visible = false;
            }
        }
        public void ShowGrid_Purchase_Ret()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT " +
                                            " from STK_TRANS where TRANSDT='" + p_Date + "' and TRANSTP='IRTB' and TRANSDT <> '2013-04-30 00:00:00' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(AMOUNT)as AMOUNT from STK_TRANS where TRANSDT='" + p_Date + "' and TRANSTP='SALE' AND TRANSDT <='2014-06-30'" +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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

        public void ShowGrid_Sale_Discount()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT * FROM " +
                                                " (SELECT A.TRANSTP, A.TRANSDT, A.TRANSMY, A.TRANSNO, A.STOREFR, A.PSID, A.REMARKS, (A.DISCAMT+STK_TRANSMST.DISAMT) AS DISAMT " +
                                                " FROM  (SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, INVREFNO, STOREFR, STORETO, PSID, SUM(DISCAMT) AS DISCAMT, REMARKS " +
                                                        " FROM          STK_TRANS " +
                                                        " WHERE (TRANSTP = 'SALE') AND (TRANSDT = '" + p_Date + "') AND (TRANSDT <='2014-06-30')" +
                                                        " GROUP BY TRANSTP, TRANSDT, TRANSMY, TRANSNO, INVREFNO, STOREFR, STORETO, PSID,REMARKS) AS A INNER JOIN " +
                                            " STK_TRANSMST ON A.TRANSTP = STK_TRANSMST.TRANSTP AND A.TRANSDT = STK_TRANSMST.TRANSDT AND A.TRANSMY = STK_TRANSMST.TRANSMY AND " +
                                            " A.TRANSNO = STK_TRANSMST.TRANSNO AND A.INVREFNO = STK_TRANSMST.INVREFNO AND A.STOREFR = STK_TRANSMST.STOREFR AND " +
                                            " A.STORETO = STK_TRANSMST.STORETO AND A.PSID = STK_TRANSMST.PSID) AS B WHERE B.DISAMT<>0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView4.DataSource = ds;
                GridView4.DataBind();
                GridView4.Visible = false;
            }
            else
            {
                GridView4.DataSource = ds;
                GridView4.DataBind();
                GridView4.Visible = false;
            }
        }

        public void ShowGrid_Sale_New()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, TOTNET AS AMOUNT " +
                       " from STK_TRANSMST where TRANSDT='" + p_Date + "' and TRANSTP='SALE' AND (TRANSDT >'2014-06-30')", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSaleNew.DataSource = ds;
                gvSaleNew.DataBind();
                gvSaleNew.Visible = false;
            }
            else
            {
                gvSaleNew.DataSource = ds;
                gvSaleNew.DataBind();
                gvSaleNew.Visible = false;
            }
        }

        //public void ShowGrid_Sale_New_LtCost()
        //{

        //    DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
        //    string p_Date = Pdate.ToString("yyyy/MM/dd");

        //    
        //    SqlConnection conn = new SqlConnection(dbFunctions.Connection);

        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, LTCOST AS AMOUNT " +
        //               " from STK_TRANSMST where TRANSDT='" + p_Date + "' and TRANSTP='SALE'", conn);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    conn.Close();
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        gvSaleLtCost.DataSource = ds;
        //        gvSaleLtCost.DataBind();
        //        gvSaleLtCost.Visible = false;
        //    }
        //    else
        //    {
        //        gvSaleLtCost.DataSource = ds;
        //        gvSaleLtCost.DataBind();
        //        gvSaleLtCost.Visible = false;
        //    }
        //}

        public void ShowGrid_Sale_Ret()
        {

            DateTime Pdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string p_Date = Pdate.ToString("yyyy/MM/dd");

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS, sum(TOTNET)as AMOUNT from STK_TRANSMST where TRANSDT='" + p_Date + "' and TRANSTP='IRTS' " +
                                            " group by TRANSTP, TRANSDT, TRANSMY, TRANSNO, STOREFR, STORETO, PSID, LCTP, LCID, LCDATE, REMARKS", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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

            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(" SELECT TRANSTP, TRANSDT, TRANSMY, TRANSNO, LCCD, CNBCD, AMOUNT, (CASE WHEN LCINVNO = '' THEN REMARKS WHEN REMARKS = '' THEN LCINVNO ELSE (REMARKS + ' - ' + LCINVNO)END)AS REMARKS FROM LC_EXPENSE " +
                                            " WHERE (TRANSDT = '" + p_Date + "')", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
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

        public void Showgrid_Check_Incomplete_Process()
        {
            
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);
            DateTime processdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string pdt = processdate.ToString("yyyy/MM/dd");

            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT TRANSTP, TRANSNO FROM STK_TRANSMST WHERE TRANSDT='" + pdt + "' AND ISNULL(TOTNET,0)=0", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                IncompleteGrid.DataSource = ds;
                IncompleteGrid.DataBind();
                IncompleteGrid.Visible = true;
                lblMSG_Incomplete.Visible = true;
                lblMSG_Incomplete.Text = "At first check & complete below transaction(s), then execute process again..";
            }
            else
            {
                lblMSG_Incomplete.Visible = false;
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                IncompleteGrid.DataSource = ds;
                IncompleteGrid.DataBind();
                int columncount = IncompleteGrid.Rows[0].Cells.Count;
                IncompleteGrid.Rows[0].Cells.Clear();
                IncompleteGrid.Rows[0].Cells.Add(new TableCell());
                IncompleteGrid.Rows[0].Cells[0].ColumnSpan = columncount;
                IncompleteGrid.Rows[0].Visible = false;
            }
        }

        protected void IncompleteGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TRANSTP = DataBinder.Eval(e.Row.DataItem, "TRANSTP").ToString();
                if (TRANSTP == "SALE")
                {
                    TRANSTP = "SALE";
                }
                //else if (TRANSTP == "BUY")
                //{
                //    TRANSTP = "BUY";
                //}
                //else if (TRANSTP == "ITRF")
                //{
                //    TRANSTP = "TRANSFER";
                //}
                else if (TRANSTP == "IRTS")
                {
                    TRANSTP = "SALE RETURN";
                }
                else if (TRANSTP == "IRTB")
                {
                    TRANSTP = "PURCHASE RETURN";
                }
                else
                {
                    TRANSTP = TRANSTP;
                }
                e.Row.Cells[0].Text = "&nbsp;" + TRANSTP;

                string TRANSNO = DataBinder.Eval(e.Row.DataItem, "TRANSNO").ToString();
                e.Row.Cells[1].Text = "&nbsp;" + TRANSNO;
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {

            Label lblcheckprocess = new Label();
            DateTime processdate = DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
            string pdt = processdate.ToString("yyyy/MM/dd");

            //dbFunctions.lblAdd(@"SELECT COUNT(TOTNET) AS TOTNET FROM STK_TRANSMST WHERE TRANSTP IN ('SALE','BUY','ITRF','IRTS','IRTB') AND TRANSDT='" + pdt + "' AND TOTNET IN ('0.00','0')", lblcheckprocess);
            dbFunctions.lblAdd(@"SELECT COUNT(TOTNET) AS TOTNET FROM STK_TRANSMST WHERE TRANSTP IN ('SALE','IRTS','IRTB') AND TRANSDT='" + pdt + "' AND TOTNET IN ('0.00','0')", lblcheckprocess);

            if (txtDate.Text == "")
            {
                Response.Write("<script>alert('Select a Date want to process?');</script>");
            }
            else if (lblcheckprocess.Text != "0")
            {
                Showgrid_Check_Incomplete_Process();
            }
            else
            {
                string userName = CookiesData["USERID"].ToString();
                string serialNo = "";
                int sl, serial;

                ShowGrid();
                //ShowGrid_Purchase();
                //ShowGrid_Purchase_IMPORT();
                //ShowGrid_Purchase_Ret();
                //ShowGrid_Sale();
                ///ShowGrid_Sale_Discount();
                //ShowGrid_Sale_Ret();
               // ShowGridLC();
                ShowGrid_Multiple();
                //ShowGrid_Sale_New();
                //ShowGrid_Sale_New_LtCost();

                iob.Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
                string trans_DT = iob.Transdt.ToString("yyyy/MM/dd");
                iob.Username = userName;



                
                SqlConnection conn = new SqlConnection(dbFunctions.Connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'GL_STRANS' and TRANSTP <> 'OPEN'", conn);
                cmd.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'GL_MTRANS' and TRANSTP <> 'OPEN'", conn);
                cmd3.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'STK_TRANS' and TRANSTP='JOUR'", conn);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("Delete from GL_MASTER where TRANSDT='" + trans_DT + "' and TABLEID = 'LC_EXPENSE' and TRANSTP='MPAY'", conn);
                cmd2.ExecuteNonQuery();

                conn.Close();
                if (iob.Transdt < DateTime.Parse("2099-04-22", dateformat, System.Globalization.DateTimeStyles.AssumeLocal))
                {
                    foreach (GridViewRow grid in GridView1.Rows)
                    {
                        try
                        {
                            if (grid.Cells[0].Text == "MREC")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MREC' ", lblSerial_Mrec);
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
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MPAY' ", lblSerial_Mpay);
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
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' ", lblSerial_Jour);
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
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'CONT' ", lblSerial_Cont);
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
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MREC' ", lblSerial_Mrec);
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



                                dob.doProcess_MREC_Multiple(iob);
                            }
                            else if (grid.Cells[0].Text == "MPAY")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MPAY' ", lblSerial_Mpay);
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

                                dob.doProcess_MPAY_Multiple(iob);
                            }
                            else if (grid.Cells[0].Text == "JOUR")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' ", lblSerial_Jour);
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

                                dob.doProcess_JOUR_Multiple(iob);
                            }
                            else if (grid.Cells[0].Text == "CONT")
                            {
                                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'CONT' ", lblSerial_Cont);
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


                                dob.doProcess_CONT_Multiple(iob);
                            }


                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                        }
                    }

                    Buy_process();
                    Buy_process_Import();
                    Buy_process_Ret();
                    Sale_process();
                    Sale_Discount_process();
                    Sale_process_Ret();
                    LC_Process();
                    Sale_process_New();
                    //Sale_process_New_LtCost();

                    Response.Write("<script>alert('Process Completed.');</script>");
                }
            }
        }

        public void Buy_process()
        {
            string userName = CookiesData["USERID"].ToString(); 
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in GridView2.Rows)
            {
                try
                {
                    if (grid.Cells[0].Text == "BUY")
                    {
                        dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '11%' ", lblSerial_BUY);
                        if (lblSerial_BUY.Text == "")
                        {
                            serialNo = "11000";
                            iob.Serial_BUY = serialNo;
                        }
                        else
                        {
                            sl = int.Parse(lblSerial_BUY.Text);
                            serial = sl + 1;

                            iob.Serial_BUY = serial.ToString();
                        }

                        iob.Transtp = "JOUR";
                        iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                        string month = iob.Transdt.ToString("MMM").ToUpper();
                        string years = iob.Transdt.ToString("yy");
                        //iob.Monyear = grid.Cells[2].Text;
                        iob.Monyear = month + "-" + years;
                        iob.TransNo = grid.Cells[3].Text;
                        iob.StoreTo = grid.Cells[5].Text;

                        iob.Debitcd = "102060100001";
                        iob.Creditcd = grid.Cells[6].Text;
                        iob.Amount = Convert.ToDecimal(grid.Cells[11].Text);
                        string Remarks = grid.Cells[10].Text;
                        if (Remarks == "&nbsp;")
                        {
                            iob.Remarks = "";
                        }
                        else
                            iob.Remarks = Remarks;



                        dob.doProcess_BUY(iob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

        }
        public void Buy_process_Import()
        {
            string userName = CookiesData["USERID"].ToString();
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in GridView22.Rows)
            {
                try
                {
                    if (grid.Cells[0].Text == "BUY")
                    {
                        dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '11%' ", lblSerial_BUY);
                        if (lblSerial_BUY.Text == "")
                        {
                            serialNo = "91000";
                            iob.Serial_BUY = serialNo;
                        }
                        else
                        {
                            sl = int.Parse(lblSerial_BUY.Text);
                            serial = sl + 1;

                            iob.Serial_BUY = serial.ToString();
                        }

                        iob.Transtp = "JOUR";
                        iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                        string month = iob.Transdt.ToString("MMM").ToUpper();
                        string years = iob.Transdt.ToString("yy");
                        //iob.Monyear = grid.Cells[2].Text;
                        iob.Monyear = month + "-" + years;
                        iob.TransNo = grid.Cells[3].Text;
                        iob.StoreTo = grid.Cells[5].Text;

                        iob.Debitcd = "102060100001";
                        iob.Creditcd = grid.Cells[8].Text;
                        iob.Amount = Convert.ToDecimal(grid.Cells[11].Text);
                        string Remarks = grid.Cells[10].Text;
                        if (Remarks == "&nbsp;")
                        {
                            iob.Remarks = "";
                        }
                        else
                            iob.Remarks = Remarks;



                        dob.doProcess_BUY(iob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

        }
        public void Buy_process_Ret()
        {
            string userName = CookiesData["USERID"].ToString();
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in gridPurchase_Ret.Rows)
            {
                try
                {
                    if (grid.Cells[0].Text == "IRTB")
                    {
                        dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '14%' ", lblSerial_BUY);
                        if (lblSerial_BUY.Text == "")
                        {
                            serialNo = "14000";
                            iob.Serial_BUY = serialNo;
                        }
                        else
                        {
                            sl = int.Parse(lblSerial_BUY.Text);
                            serial = sl + 1;

                            iob.Serial_BUY = serial.ToString();
                        }

                        iob.Transtp = "JOUR";
                        iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                        string month = iob.Transdt.ToString("MMM").ToUpper();
                        string years = iob.Transdt.ToString("yy");
                        //iob.Monyear = grid.Cells[2].Text;
                        iob.Monyear = month + "-" + years;
                        iob.TransNo = grid.Cells[3].Text;
                        iob.StoreFrom = grid.Cells[5].Text;

                        iob.Debitcd = grid.Cells[6].Text;
                        iob.Creditcd = "401020100002";
                        iob.Amount = Convert.ToDecimal(grid.Cells[11].Text);
                        string Remarks = grid.Cells[10].Text;
                        if (Remarks == "&nbsp;")
                        {
                            iob.Remarks = "";
                        }
                        else
                            iob.Remarks = Remarks;



                        dob.doProcess_BUY_Ret(iob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

        }

        public void Sale_process()
        {
            string userName = CookiesData["USERID"].ToString();
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in GridView3.Rows)
            {
                try
                {
                    if (grid.Cells[0].Text == "SALE")
                    {
                        dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '12%'", lblSerial_SALE);
                        if (lblSerial_SALE.Text == "")
                        {
                            serialNo = "12000";
                            iob.Serial_SALE = serialNo;
                        }
                        else
                        {
                            sl = int.Parse(lblSerial_SALE.Text);
                            serial = sl + 1;

                            iob.Serial_SALE = serial.ToString();
                        }

                        iob.Transtp = "JOUR";
                        iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                        string month = iob.Transdt.ToString("MMM").ToUpper();
                        string years = iob.Transdt.ToString("yy");
                        //iob.Monyear = grid.Cells[2].Text;
                        iob.Monyear = month + "-" + years;
                        iob.TransNo = grid.Cells[3].Text;
                        iob.StoreFrom = grid.Cells[4].Text;

                        iob.Debitcd = grid.Cells[6].Text;
                        iob.Creditcd = "301010100001";
                        iob.Amount = Convert.ToDecimal(grid.Cells[11].Text);
                        string Remarks = grid.Cells[10].Text;
                        if (Remarks == "&nbsp;")
                        {
                            iob.Remarks = "";
                        }
                        else
                            iob.Remarks = Remarks;



                        dob.doProcess_SALE(iob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        public void Sale_process_New()
        {
            string userName = CookiesData["USERID"].ToString();
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in gvSaleNew.Rows)
            {
                try
                {
                    if (grid.Cells[0].Text == "SALE")
                    {
                        dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '12%'", lblSerial_SALE);
                        if (lblSerial_SALE.Text == "")
                        {
                            serialNo = "12000";
                            iob.Serial_SALE = serialNo;
                        }
                        else
                        {
                            sl = int.Parse(lblSerial_SALE.Text);
                            serial = sl + 1;

                            iob.Serial_SALE = serial.ToString();
                        }

                        iob.Transtp = "JOUR";
                        iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                        iob.Monyear = grid.Cells[2].Text;
                        iob.TransNo = grid.Cells[3].Text;
                        iob.StoreFrom = grid.Cells[4].Text;

                        iob.Debitcd = grid.Cells[6].Text;
                        iob.Creditcd = "301010100001";
                        iob.Amount = Convert.ToDecimal(grid.Cells[11].Text);
                        string Remarks = grid.Cells[10].Text;
                        if (Remarks == "&nbsp;")
                        {
                            iob.Remarks = "";
                        }
                        else
                            iob.Remarks = Remarks;

                        dob.doProcess_SALE(iob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        //public void Sale_process_New_LtCost()
        //{
        //    string userName = HttpContext.Current.Session["UserName"].ToString();
        //    AlchemyAccounting.Accounts.DataAccess.SingleVoucher dob = new DataAccess.SingleVoucher();
        //    AlchemyAccounting.Accounts.Interface.SingleVoucher iob = new Interface.SingleVoucher();
        //    string serialNo = "";
        //    int sl, serial;

        //    iob.Username = userName;

        //    DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
        //    string trans_DT = Transdt.ToString("yyyy/MM/dd");

        //    foreach (GridViewRow grid in gvSaleLtCost.Rows)
        //    {
        //        try
        //        {
        //            if (grid.Cells[0].Text == "SALE")
        //            {
        //                dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '16%'", lblSerial_SALE);
        //                if (lblSerial_SALE.Text == "")
        //                {
        //                    serialNo = "16000";
        //                    iob.Serial_SALE = serialNo;
        //                }
        //                else
        //                {
        //                    sl = int.Parse(lblSerial_SALE.Text);
        //                    serial = sl + 1;

        //                    iob.Serial_SALE = serial.ToString();
        //                }

        //                iob.Transtp = "JOUR";
        //                iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
        //                iob.Monyear = grid.Cells[2].Text;
        //                iob.TransNo = grid.Cells[3].Text;
        //                iob.StoreFrom = grid.Cells[4].Text;

        //                iob.Debitcd = grid.Cells[6].Text;
        //                iob.Creditcd = "202030100001";
        //                iob.Amount = Convert.ToDecimal(grid.Cells[11].Text);
        //                string Remarks = grid.Cells[10].Text;
        //                if (Remarks == "&nbsp;")
        //                {
        //                    iob.Remarks = "";
        //                }
        //                else
        //                    iob.Remarks = Remarks;

        //                dob.doProcess_SALE_LtCost(iob);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Write(ex.Message);
        //        }
        //    }
        //}

        public void Sale_Discount_process()
        {
            string userName = CookiesData["USERID"].ToString();
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in GridView4.Rows)
            {
                try
                {
                    if (grid.Cells[0].Text == "SALE")
                    {
                        dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'JOUR' and SERIALNO like '13%'", lblSlSale_Dis);
                        if (lblSlSale_Dis.Text == "")
                        {
                            serialNo = "13000";
                            iob.Sl_Sale_dis = serialNo;
                        }
                        else
                        {
                            sl = int.Parse(lblSlSale_Dis.Text);
                            serial = sl + 1;

                            iob.Sl_Sale_dis = serial.ToString();
                        }

                        iob.Transtp = "JOUR";
                        iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                        string month = iob.Transdt.ToString("MMM").ToUpper();
                        string years = iob.Transdt.ToString("yy");
                        //iob.Monyear = grid.Cells[2].Text;
                        iob.Monyear = month + "-" + years;
                        iob.TransNo = grid.Cells[3].Text;
                        iob.StoreFrom = grid.Cells[4].Text;

                        iob.Debitcd = "401030100001";
                        iob.Creditcd = grid.Cells[5].Text;
                        iob.Amount = Convert.ToDecimal(grid.Cells[7].Text);
                        string Remarks = grid.Cells[6].Text;
                        if (Remarks == "&nbsp;")
                        {
                            iob.Remarks = "";
                        }
                        else
                            iob.Remarks = Remarks;



                        dob.doProcess_SALE_DisCount(iob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        public void Sale_process_Ret()
        {
            string userName = CookiesData["USERID"].ToString();
            string serialNo = "";
            int sl, serial;

            iob.Username = userName;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");
            string mon = Transdt.ToString("MMM").ToUpper();
            string yr = Transdt.ToString("yy");
            string mnyr = mon + "-" + yr;

            foreach (GridViewRow grid in gridSale_Ret.Rows)
            {
                try
                {
                    if (grid.Cells[0].Text == "IRTS")
                    {
                        dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSMY = '" + mnyr + "' and TRANSTP = 'JOUR' and SERIALNO like '15%'", lblSerial_SALE);
                        if (lblSerial_SALE.Text == "")
                        {
                            serialNo = "15000";
                            iob.Serial_SALE = serialNo;
                        }
                        else
                        {
                            sl = int.Parse(lblSerial_SALE.Text);
                            serial = sl + 1;

                            iob.Serial_SALE = serial.ToString();
                        }

                        iob.Transtp = "JOUR";
                        iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                        string month = iob.Transdt.ToString("MMM").ToUpper();
                        string years = iob.Transdt.ToString("yy");
                        //iob.Monyear = grid.Cells[2].Text;
                        iob.Monyear = month + "-" + years;
                        iob.TransNo = grid.Cells[3].Text;
                        iob.StoreFrom = grid.Cells[4].Text;

                        iob.Debitcd = "301010100002";
                        iob.Creditcd = grid.Cells[6].Text;
                        iob.Amount = Convert.ToDecimal(grid.Cells[11].Text);
                        string Remarks = grid.Cells[10].Text;
                        if (Remarks == "&nbsp;")
                        {
                            iob.Remarks = "";
                        }
                        else
                            iob.Remarks = Remarks;



                        dob.doProcess_SALE_Ret(iob);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        public void LC_Process()
        {
            string userName = CookiesData["USERID"].ToString();
            string ipAddress = dbFunctions.ipAddress();
            string PCName = dbFunctions.userPc(); 
            SqlConnection conn = new SqlConnection(dbFunctions.Connection);

            string serialNo = "";
            int sl, serial;

            iob.Username = userName;
            iob.Userpc = PCName;
            iob.Ipaddress = ipAddress;

            DateTime Transdt = (DateTime.Parse(txtDate.Text, dateformat, System.Globalization.DateTimeStyles.AssumeLocal));
            string trans_DT = Transdt.ToString("yyyy/MM/dd");

            foreach (GridViewRow grid in gridLC.Rows)
            {
                try
                {
                    dbFunctions.lblAdd(@"Select max(SERIALNO) FROM  GL_MASTER where TRANSDT = '" + trans_DT + "' and TRANSTP = 'MPAY' ", lblSerial_LC);

                    if (lblSerial_LC.Text == "")
                    {
                        serialNo = "20000";
                        iob.SerialNo_MREC = serialNo;
                    }
                    else
                    {
                        sl = int.Parse(lblSerial_LC.Text);
                        serial = sl + 1;
                        iob.SerialNo_MREC = serial.ToString();
                    }
                    iob.Transtp = grid.Cells[0].Text;
                    iob.Transdt = DateTime.Parse(grid.Cells[1].Text);
                    iob.Monyear = grid.Cells[2].Text;
                    iob.TransNo = grid.Cells[3].Text;
                    iob.Debitcd = grid.Cells[4].Text;
                    iob.Creditcd = grid.Cells[5].Text;
                    iob.Amount = Convert.ToDecimal(grid.Cells[6].Text);
                    string Remarks = grid.Cells[7].Text;
                    if (Remarks == "&nbsp;")
                    {
                        iob.Remarks = "";
                    }
                    else
                        iob.Remarks = Remarks;

                    dob.doProcess_LC(iob);

                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
             ShowGrid();
            //ShowGrid_Purchase();
            //ShowGrid_Purchase_Ret();
            //ShowGrid_Sale();
            //ShowGrid_Sale_Discount();
            //ShowGrid_Sale_Ret();
            //ShowGridLC();
            ShowGrid_Multiple();
            //ShowGrid_Sale_New();
            //ShowGrid_Sale_New_LtCost();
            btnProcess.Focus();
        }
    }
}