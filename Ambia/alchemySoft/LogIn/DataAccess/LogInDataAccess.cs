using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using alchemySoft;
using alchemySoft.LogIn.Interface;

namespace alchemySoft.LogIn.DataAccess
{
    public class LogInDataAccess 
    {
         SqlConnection con;
        SqlCommand cmd;

        public LogInDataAccess()
        {
            con = new SqlConnection(dbFunctions.Connection);
            cmd = new SqlCommand("", con);
        }

        public string UPDATE_ASL_PASSWORD(LogInInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE ASL_USERCO SET LOGINPW=@LOGINPW, UPDUSERID=@UPDUSERID, UPDTIME=@UPDTIME, 
                UPDIPNO=@UPDIPNO, UPDLTUDE=@UPDLTUDE WHERE USERID=@USERID";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@USERID", SqlDbType.BigInt).Value = ob.UserID;
                cmd.Parameters.Add("@LOGINPW", SqlDbType.NVarChar).Value = ob.Password;

                cmd.Parameters.Add("@UPDUSERID", SqlDbType.BigInt).Value = ob.UserIdUpdate;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.DateTime).Value = ob.InTimeUpdate;
                cmd.Parameters.Add("@UPDIPNO", SqlDbType.NVarChar).Value = ob.ipAddressUpdate;
                cmd.Parameters.Add("@UPDLTUDE", SqlDbType.NVarChar).Value = ob.LotiLengTudeUpdate;

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
        public string INSERT_ASL_LOG(LogInInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO ASL_LOG(COMPID,USERID,LOGTYPE,LOGSLNO,LOGDT,LOGTIME,LOGIPNO,LOGLTUDE,TABLEID,LOGDATA,userPc)
 				Values 
				(@COMPID,@USERID,@LOGTYPE,@LOGSLNO,@LOGDT,@LOGTIME,@LOGIPNO,@LOGLTUDE,@TABLEID,@LOGDATA,@userPc)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@COMPID", SqlDbType.BigInt).Value = ob.CompanyId;
                cmd.Parameters.Add("@USERID", SqlDbType.BigInt).Value = ob.CompanyUserId;
                cmd.Parameters.Add("@LOGTYPE", SqlDbType.NVarChar).Value = ob.LogType;
                cmd.Parameters.Add("@LOGSLNO", SqlDbType.BigInt).Value = ob.LogSlNo;
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.TableId;
                cmd.Parameters.Add("@LOGDATA", SqlDbType.NVarChar).Value = ob.LogDatA;

                cmd.Parameters.Add("@LOGDT", SqlDbType.DateTime).Value = ob.InTimeInsert;
                cmd.Parameters.Add("@LOGTIME", SqlDbType.DateTime).Value = ob.InTimeInsert;
                cmd.Parameters.Add("@LOGIPNO", SqlDbType.NVarChar).Value = ob.ipAddressInsert;
                cmd.Parameters.Add("@LOGLTUDE", SqlDbType.NVarChar).Value = ob.LotiLengTudeInsert;
                cmd.Parameters.Add("@userPc", SqlDbType.NVarChar).Value = ob.userPcInsert;

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
    }
}