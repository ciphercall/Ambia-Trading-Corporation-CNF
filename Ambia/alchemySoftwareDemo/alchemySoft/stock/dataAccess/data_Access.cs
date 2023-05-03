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
    }
}