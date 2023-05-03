using alchemySoft.stock.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace alchemySoft.stock.dataAccess
{
    public class data_Access
    {
        SqlConnection con;
        SqlCommand cmd;
        public data_Access()
        {
            con = new SqlConnection(dbFunctions.Connection);
            cmd = new SqlCommand("", con);
        }

        internal string INSERT_ASL_LOG(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO ASL_LOG(COMPID,USERID,LOGTYPE,LOGSLNO,LOGDT,LOGTIME,LOGIPNO,LOGLTUDE,TABLEID,LOGDATA,USERPC)
 				Values 
				(@COMPID,@USERID,@LOGTYPE,@LOGSLNO,@LOGDT,@LOGTIME,@LOGIPNO,@LOGLTUDE,@TABLEID,@LOGDATA,@USERPC)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@COMPID", SqlDbType.BigInt).Value = ob.CompanyId;
                cmd.Parameters.Add("@USERID", SqlDbType.BigInt).Value = ob.CompanyUserId;
                cmd.Parameters.Add("@LOGTYPE", SqlDbType.NVarChar).Value = ob.LogType;
                cmd.Parameters.Add("@LOGSLNO", SqlDbType.BigInt).Value = ob.LogSlNo;
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.TableId;
                cmd.Parameters.Add("@LOGDATA", SqlDbType.NVarChar).Value = ob.LogDatA;

                cmd.Parameters.Add("@LOGDT", SqlDbType.DateTime).Value = ob.InTimeInsert;
                cmd.Parameters.Add("@LOGTIME", SqlDbType.DateTime).Value = ob.InTimeInsert;
                cmd.Parameters.Add("@LOGIPNO", SqlDbType.NVarChar).Value = ob.IpAddressInsert;
                cmd.Parameters.Add("@LOGLTUDE", SqlDbType.NVarChar).Value = ob.LotiLengTudeInsert;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.UserPcInsert;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        internal string INSERT_ITEMMST(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO STK_ITEMMST(CATID, CATNM, USERPC, USERID, INTIME, IPADDRESS)
 				Values 
				(@CATID, @CATNM, @USERPC, @USERID, @INTIME, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar).Value = ob.catID;
                cmd.Parameters.Add("@CATNM", SqlDbType.NVarChar).Value = ob.catName;


                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.intime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return s;
        }

        public string INSERT_ITEM(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO STK_ITEM(CATID,ITEMID,ITEMNM,ITEMCD,COLOR,BUYRT,SALERT,MINSQTY,REMARKS,USERPC,USERID,INTIME,IPADDRESS)
 				Values 
				(@CATID,@ITEMID,@ITEMNM,@ITEMCD,@COLOR,@BUYRT,@SALERT,@MINSQTY,@REMARKS,@USERPC,@USERID,@INTIME,@IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar).Value = ob.catID;
                cmd.Parameters.Add("@ITEMID", SqlDbType.NVarChar).Value = ob.itemID;
                cmd.Parameters.Add("@ITEMNM", SqlDbType.NVarChar).Value = ob.itemNM;
                cmd.Parameters.Add("@ITEMCD", SqlDbType.NVarChar).Value = ob.itemCD;
                cmd.Parameters.Add("@COLOR", SqlDbType.NVarChar).Value = ob.color;
                cmd.Parameters.Add("@BUYRT", SqlDbType.Decimal).Value = ob.buyRT;
                cmd.Parameters.Add("@SALERT", SqlDbType.Decimal).Value = ob.saleRT;
                cmd.Parameters.Add("@MINSQTY", SqlDbType.Decimal).Value = ob.minStk;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return s;
        }
        public string UPDATE_ITEM(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE STK_ITEM SET ITEMNM=@ITEMNM,ITEMCD=@ITEMCD,COLOR=@COLOR,BUYRT=@BUYRT,SALERT=@SALERT,MINSQTY=@MINSQTY,REMARKS=@REMARKS,UPDUSERPC=@UPDUSERPC,UPDUSERID=@UPDUSERID,UPDTIME=@UPDTIME,UPDIPADDRESS=@UPDIPADDRESS WHERE ITEMID=@ITEMID ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@ITEMID", SqlDbType.NVarChar).Value = ob.itemID;
                cmd.Parameters.Add("@ITEMNM", SqlDbType.NVarChar).Value = ob.itemNM;
                cmd.Parameters.Add("@ITEMCD", SqlDbType.NVarChar).Value = ob.itemCD;
                cmd.Parameters.Add("@COLOR", SqlDbType.NVarChar).Value = ob.color;
                cmd.Parameters.Add("@BUYRT", SqlDbType.Decimal).Value = ob.buyRT;
                cmd.Parameters.Add("@SALERT", SqlDbType.Decimal).Value = ob.saleRT;
                cmd.Parameters.Add("@MINSQTY", SqlDbType.Decimal).Value = ob.minStk;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarks;

                cmd.Parameters.Add("@UPDUSERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@UPDUSERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@UPDIPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return s;
        }



        public string insertPS(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO STK_PS( PSTP, PSID, CITY, ADDRESS, CONTACTNO, EMAIL, WEBID, CPNM, CPNO, REMARKS, STATUS, USERPC, USERID, IPADDRESS,PS_ID) " +
                                  "VALUES (@PSTP,@PSID,@CITY,@ADDRESS,@CONTACTNO,@EMAIL,@WEBID,@CPNM,@CPNO,@REMARKS,@STATUS,@USERPC,@USERID,@IPADDRESS,@PS_ID)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@PSTP", SqlDbType.NVarChar).Value = ob.Pstp;
                cmd.Parameters.Add("@PSID", SqlDbType.NVarChar).Value = ob.Pscd;
                cmd.Parameters.Add("@CITY", SqlDbType.NVarChar).Value = ob.City;
                cmd.Parameters.Add("@ADDRESS", SqlDbType.NVarChar).Value = ob.Address;
                cmd.Parameters.Add("@CONTACTNO", SqlDbType.NVarChar).Value = ob.Contactno;
                cmd.Parameters.Add("@EMAIL", SqlDbType.NVarChar).Value = ob.Email;
                cmd.Parameters.Add("@WEBID", SqlDbType.NVarChar).Value = ob.Webid;
                cmd.Parameters.Add("@CPNM", SqlDbType.NVarChar).Value = ob.Cpnm;
                cmd.Parameters.Add("@CPNO", SqlDbType.NVarChar).Value = ob.Cpno;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarks;
                cmd.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = ob.Status;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@PS_ID", SqlDbType.NVarChar).Value = ob.Ps_ID;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        internal string INSERT_TRANS(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO STK_TRANS(TRANSTP,TRANSDT,TRANSMY,TRANSNO,STOREFR,STORETO,REMARKS,TRANSSL,ITEMID,UNITTP,PQTY,QTY,RATE,AMOUNT,USERPC,USERID,INTIME,IPADDRESS)
 				Values 
				(@TRANSTP,@TRANSDT,@TRANSMY,@TRANSNO,@STOREFR,@STORETO,@REMARKS,@TRANSSL,@ITEMID,@UNITTP,@PQTY,@QTY,@RATE,@AMOUNT,@USERPC,@USERID,@INTIME,@IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.transTP;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.transDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.transMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.transno;
                cmd.Parameters.Add("@STOREFR", SqlDbType.NVarChar).Value = ob.storeFrID;
                cmd.Parameters.Add("@STORETO", SqlDbType.NVarChar).Value = ob.storeToID;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarks;
                cmd.Parameters.Add("@ITEMID", SqlDbType.NVarChar).Value = ob.itemID;
                cmd.Parameters.Add("@TRANSSL", SqlDbType.NVarChar).Value = ob.transSL;
                cmd.Parameters.Add("@UNITTP", SqlDbType.NVarChar).Value = ob.unitTP;
                cmd.Parameters.Add("@PQTY", SqlDbType.Decimal).Value = ob.lotQty;
                cmd.Parameters.Add("@QTY", SqlDbType.Decimal).Value = ob.qty;
                cmd.Parameters.Add("@RATE", SqlDbType.Decimal).Value = ob.rate;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.amount;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        internal string INSERT_TRANSMST(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO STK_TRANSMST(TRANSMD,TRANSTP,TRANSDT,TRANSMY,TRANSNO,STOREFR,STORETO,LOT,BATCH,MACHINE,REMARKS,USERPC,USERID,INTIME,IPADDRESS,JOBNO)
 				Values 
				(@TRANSMD,@TRANSTP,@TRANSDT,@TRANSMY,@TRANSNO,@STOREFR,@STORETO,@LOT,@BATCH,@MACHINE,@REMARKS,@USERPC,@USERID,@INTIME,@IPADDRESS,@JOBNO)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSMD", SqlDbType.NVarChar).Value = ob.transMD;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.transTP;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.transDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.transMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.transno;
                cmd.Parameters.Add("@STOREFR", SqlDbType.NVarChar).Value = ob.storeFrID;
                cmd.Parameters.Add("@STORETO", SqlDbType.NVarChar).Value = ob.storeToID;
                cmd.Parameters.Add("@LOT", SqlDbType.NVarChar).Value = ob.lot;
                cmd.Parameters.Add("@BATCH", SqlDbType.NVarChar).Value = ob.batch;
                cmd.Parameters.Add("@MACHINE", SqlDbType.NVarChar).Value = ob.machine;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarksMst;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;
                cmd.Parameters.Add("@JOBNO", SqlDbType.NVarChar).Value = ob.jobNo;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();


            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }
        internal string UPDATE_TRANSMST(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE STK_TRANSMST SET STOREFR=@STOREFR,STORETO=@STORETO,REMARKS=@REMARKS,JOBNO=@JOBNO,
LOT=@LOT,BATCH=@BATCH,MACHINE=@MACHINE WHERE TRANSTP=@TRANSTP AND TRANSMY=@TRANSMY AND TRANSNO=@TRANSNO ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.transTP;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.transMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.transno;
                cmd.Parameters.Add("@STOREFR", SqlDbType.NVarChar).Value = ob.storeFrID;
                cmd.Parameters.Add("@STORETO", SqlDbType.NVarChar).Value = ob.storeToID;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarksMst;
                cmd.Parameters.Add("@JOBNO", SqlDbType.NVarChar).Value = ob.jobNo;
                cmd.Parameters.Add("@LOT", SqlDbType.NVarChar).Value = ob.lot;
                cmd.Parameters.Add("@BATCH", SqlDbType.NVarChar).Value = ob.batch;
                cmd.Parameters.Add("@MACHINE", SqlDbType.NVarChar).Value = ob.machine;
                cmd.Parameters.Add("@UPDUSERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@UPDUSERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@UPDIPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return s;

        }

        internal string UPDATE_TRANS(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE STK_TRANS SET REMARKS=@REMARKS,ITEMID=@ITEMID,UNITTP=@UNITTP,PQTY=@PQTY,
QTY=@QTY,RATE=@RATE,AMOUNT=@AMOUNT,UPDUSERPC=@UPDUSERPC,UPDUSERID=@UPDUSERID,UPDTIME=@UPDTIME,
UPDIPADDRESS=@UPDIPADDRESS WHERE SL=@SL";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SL", SqlDbType.NVarChar).Value = ob.sl;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarks;
                cmd.Parameters.Add("@ITEMID", SqlDbType.NVarChar).Value = ob.itemID;
                cmd.Parameters.Add("@UNITTP", SqlDbType.NVarChar).Value = ob.unitTP;
                cmd.Parameters.Add("@PQTY", SqlDbType.Decimal).Value = ob.lotQty;
                cmd.Parameters.Add("@QTY", SqlDbType.Decimal).Value = ob.qty;
                cmd.Parameters.Add("@RATE", SqlDbType.Decimal).Value = ob.rate;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.amount;
                cmd.Parameters.Add("@UPDUSERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@UPDUSERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@UPDIPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return s;

        }

        public string INSERT_TRANSMST_WORK(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO STK_TRANSMST(TRANSTP,TRANSDT,TRANSMY,TRANSNO,PSID,REMARKS,USERPC,USERID,INTIME,IPADDRESS,JOBNO)
 				Values 
				(@TRANSTP,@TRANSDT,@TRANSMY,@TRANSNO,@PSID,@REMARKS,@USERPC,@USERID,@INTIME,@IPADDRESS,@JOBNO)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.transTP;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.transDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.transMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.transno;
                cmd.Parameters.Add("@PSID", SqlDbType.NVarChar).Value = ob.psID;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarksMst;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;
                cmd.Parameters.Add("@JOBNO", SqlDbType.NVarChar).Value = ob.jobNo;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }
        public string INSERT_TRANS_WORK(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO STK_TRANS(TRANSSL,TRANSTP,TRANSDT,TRANSMY,TRANSNO,PSID,REMARKS,ITEMID,QTY,USERPC,USERID,INTIME,IPADDRESS,FABRIC,STYLENO,WPCNT,YARN,TTLPX)
 				Values 
				(@TRANSSL,@TRANSTP,@TRANSDT,@TRANSMY,@TRANSNO,@PSID,@REMARKS,@ITEMID,@QTY,@USERPC,@USERID,@INTIME,@IPADDRESS,@FABRIC,@STYLENO,@WPCNT,@YARN,@TTLPX)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSSL", SqlDbType.BigInt).Value = ob.transSL;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.transTP;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.transDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.transMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.transno;
                cmd.Parameters.Add("@PSID", SqlDbType.NVarChar).Value = ob.psID;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarks;
                cmd.Parameters.Add("@ITEMID", SqlDbType.NVarChar).Value = ob.itemID;
                cmd.Parameters.Add("@QTY", SqlDbType.Decimal).Value = ob.qty;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;
                cmd.Parameters.Add("@FABRIC", SqlDbType.NVarChar).Value = ob.fabric;
                cmd.Parameters.Add("@STYLENO", SqlDbType.NVarChar).Value = ob.style;
                cmd.Parameters.Add("@WPCNT", SqlDbType.Decimal).Value = ob.wpcnt;
                cmd.Parameters.Add("@YARN", SqlDbType.Decimal).Value = ob.yarn;
                cmd.Parameters.Add("@TTLPX", SqlDbType.Decimal).Value = ob.ttlPx;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }



        public string UPDATE_TRANS_WORK(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE STK_TRANS SET  REMARKS=@REMARKS, ITEMID=@ITEMID, QTY=@QTY,  
UPDUSERPC=@UPDUSERPC,UPDUSERID=@UPDUSERID,UPDTIME=@UPDTIME,
UPDIPADDRESS=@UPDIPADDRESS, FABRIC=@FABRIC,STYLENO=@STYLENO,WPCNT=@WPCNT,TTLPX=@TTLPX,
YARN=@YARN WHERE TRANSTP=@TRANSTP AND TRANSMY=@TRANSMY AND TRANSNO=@TRANSNO ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarks;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.transTP;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.transMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.transno;
                cmd.Parameters.Add("@PSID", SqlDbType.NVarChar).Value = ob.psID;
                cmd.Parameters.Add("@ITEMID", SqlDbType.NVarChar).Value = ob.itemID;
                cmd.Parameters.Add("@QTY", SqlDbType.Decimal).Value = ob.qty;

                cmd.Parameters.Add("@FABRIC", SqlDbType.NVarChar).Value = ob.fabric;
                cmd.Parameters.Add("@STYLENO", SqlDbType.NVarChar).Value = ob.style;
                cmd.Parameters.Add("@WPCNT", SqlDbType.Decimal).Value = ob.wpcnt;
                cmd.Parameters.Add("@YARN", SqlDbType.Decimal).Value = ob.yarn;
                cmd.Parameters.Add("@TTLPX", SqlDbType.Decimal).Value = ob.ttlPx;

                cmd.Parameters.Add("@UPDUSERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@UPDUSERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@UPDIPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }
        public string UPDATE_TRANSMST_WORK(models ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE STK_TRANSMST SET  REMARKS=@REMARKS, JOBNO=@JOBNO,PSID=@PSID, 
UPDUSERPC=@UPDUSERPC,UPDUSERID=@UPDUSERID,UPDTIME=@UPDTIME,
UPDIPADDRESS=@UPDIPADDRESS  WHERE TRANSTP=@TRANSTP AND TRANSMY=@TRANSMY AND TRANSNO=@TRANSNO ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.transTP;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.transMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.transno;
                cmd.Parameters.Add("@PSID", SqlDbType.NVarChar).Value = ob.psID;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.remarksMst;
                cmd.Parameters.Add("@JOBNO", SqlDbType.NVarChar).Value = ob.jobNo;

                cmd.Parameters.Add("@UPDUSERPC", SqlDbType.NVarChar).Value = ob.userPC;
                cmd.Parameters.Add("@UPDUSERID", SqlDbType.NVarChar).Value = ob.userID;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.DateTime).Value = ob.intime;
                cmd.Parameters.Add("@UPDIPADDRESS", SqlDbType.NVarChar).Value = ob.ipaddress;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                s = "true";
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

      
    }
}