using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AlchemyAccounting;
using alchemySoft;
using alchemy.accounts.Interface;

namespace alchemy.accounts.DataAccess
{
    public class AccountDataAccess
    {
        SqlConnection con;
        SqlCommand cmd;

        public AccountDataAccess()
        {
            con = new SqlConnection(dbFunctions.Connection);
            cmd = new SqlCommand("", con);
        }

        public string insertSingleVouch(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_STRANS(TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSFOR, COSTPID, TRANSMODE, " +
                                  "DEBITCD, CREDITCD, CHEQUENO, CHEQUEDT, AMOUNT, REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                                  "VALUES (@TRANSTP,@TRANSDT,@TRANSMY,@TRANSNO,@SERIALNO,@TRANSFOR,@COSTPID,@TRANSMODE,@DEBITCD," +
                                  "@CREDITCD,@CHEQUENO,@CHEQUEDT,@AMOUNT,@REMARKS,@USERPC,@USERID, @INTIME,@IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.SmallDateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.Voucher;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserId;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTM;

                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }



        string inc_Serial = "";
        int ser;
        string final_Serial = "";

        public string doProcess_MREC(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE,@CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_MPAY(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE, @CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_JOUR(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE, @CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_CONT(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE, @CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_STRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }
        public DataTable rptCreditVoucher(string Transtype, DateTime TransDate, int VoucherNo, string Mode)
        {
            DataTable table = new DataTable();
            string msg = "";
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec sp_rptCreditVoucher @TransType,@TransDate,@VouchNo,@Mode";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TransType", SqlDbType.NVarChar).Value = Transtype;
                cmd.Parameters.Add("@TransDate", SqlDbType.DateTime).Value = TransDate;
                cmd.Parameters.Add("@VouchNo", SqlDbType.Int).Value = VoucherNo;
                cmd.Parameters.Add("@Mode", SqlDbType.NVarChar).Value = Mode;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ob1)
            {
                msg = ob1.Message;
            }
            return table;

        }
        public DataTable CashBook(string DebitCD, DateTime From, DateTime To, string FilteredHead)
        {
            DataTable table = new DataTable();
            string msg = "";
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec sp_rptCashBook @DebitCD,@From,@To,@FilteredHead";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@DebitCD", SqlDbType.NVarChar).Value = DebitCD;
                cmd.Parameters.Add("@From", SqlDbType.DateTime).Value = From;
                cmd.Parameters.Add("@To", SqlDbType.DateTime).Value = To;
                cmd.Parameters.Add("@FilteredHead", SqlDbType.NVarChar).Value = FilteredHead;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ob1)
            {
                msg = ob1.Message;
            }
            return table;

        }

        public DataTable BankBook(string DebitCD, DateTime From, DateTime To, string FilteredHead)
        {
            DataTable table = new DataTable();
            string msg = "";
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec sp_rptBankBook @DebitCD,@From,@To,@FilteredHead";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@DebitCD", SqlDbType.NVarChar).Value = DebitCD;
                cmd.Parameters.Add("@From", SqlDbType.DateTime).Value = From;
                cmd.Parameters.Add("@To", SqlDbType.DateTime).Value = To;
                cmd.Parameters.Add("@FilteredHead", SqlDbType.NVarChar).Value = FilteredHead;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ob1)
            {
                msg = ob1.Message;
            }
            return table;

        }

        public DataTable LedgerBook(string debitCD, DateTime From, DateTime To, string searchHead)
        {
            DataTable table = new DataTable();
            string msg = "";
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec sp_rptLedgerBook @debitCD,@From,@To,@FilteredHead";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@debitCD", SqlDbType.NVarChar).Value = debitCD;
                cmd.Parameters.Add("@From", SqlDbType.DateTime).Value = From;
                cmd.Parameters.Add("@To", SqlDbType.DateTime).Value = To;
                cmd.Parameters.Add("@FilteredHead", SqlDbType.NVarChar).Value = searchHead;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ob1)
            {
                msg = ob1.Message;
            }
            return table;

        }
        public DataTable ChartAcc()
        {
            DataTable table = new DataTable();
            string msg = "";
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec sp_rptChartAccount";
                cmd.Parameters.Clear();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ob1)
            {
                msg = ob1.Message;
            }
            return table;

        }

        public string doProcess_BUY(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_BUY;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;

                // ob.Costpid = "";

                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.Serial_BUY;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                //if (ob.Costpid == "&nbsp;")
                //{
                //    ob.Costpid = "";
                //}
                //else
                //    ob.Costpid = ob.Costpid;
                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_BUY_Ret(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_BUY;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;

                // ob.Costpid = "";

                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.Serial_BUY;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                //if (ob.Costpid == "&nbsp;")
                //{
                //    ob.Costpid = "";
                //}
                //else
                //    ob.Costpid = ob.Costpid;
                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_SALE(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_SALE;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.Serial_SALE;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();


                if (ob.Allowance > 0)
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                      " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                      " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
                                      " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                    cmd.Parameters.Clear();

                    inc_Serial = ob.Serial_SALE;
                    ser = int.Parse(inc_Serial) + 2;
                    final_Serial = ser.ToString();
                    cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                    cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                    cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                    cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                    cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                    cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = "404010100002";
                    cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                    cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.AllowanceAmount;
                    cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                    cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                    cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                    cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                    cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                      " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                      " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
                                      " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                    cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                    cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                    cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                    inc_Serial = ob.Serial_SALE;
                    ser = int.Parse(inc_Serial) + 3;
                    final_Serial = ser.ToString();
                    cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                    cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = "404010100002";
                    cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value =  ob.Debitcd;
                    cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.AllowanceAmount;
                    cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                    cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                    cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                    cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                    cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }
        public string doProcess_WASTAGE(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_SALE;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.Serial_SALE;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }
        //public string doProcess_Receive(AccountInterface ob)
        //{
        //    string s = "";
        //    SqlTransaction tran = null;


        //    try
        //    {
        //        if (con.State != ConnectionState.Open)
        //             if (con.State != ConnectionState.Open)con.Open();
        //        tran = con.BeginTransaction();

        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
        //                          " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
        //                          " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
        //                          " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
        //        cmd.Parameters.Clear();

        //        cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
        //        cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
        //        cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
        //        cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
        //        cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_SALE;
        //        cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
        //        cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
        //        cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
        //        cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
        //        cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
        //        cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
        //        cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
        //        cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

        //        cmd.Transaction = tran;
        //        cmd.ExecuteNonQuery();

        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
        //                          " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
        //                          " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
        //                          " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
        //        cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
        //        cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
        //        cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
        //        inc_Serial = ob.Serial_SALE;
        //        ser = int.Parse(inc_Serial) + 1;
        //        final_Serial = ser.ToString();
        //        cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
        //        cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
        //        cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
        //        cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
        //        cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
        //        cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
        //        cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
        //        cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
        //        cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

        //        cmd.Transaction = tran;
        //        cmd.ExecuteNonQuery();

        //        tran.Commit();
        //        if (con.State != ConnectionState.Closed)
        //            if (con.State != ConnectionState.Closed)con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        s = ex.Message;
        //    }
        //    return s;
        //}
        //public string doProcess_Issue(AccountInterface ob)
        //{
        //    string s = "";
        //    SqlTransaction tran = null;


        //    try
        //    {
        //        if (con.State != ConnectionState.Open)
        //             if (con.State != ConnectionState.Open)con.Open();
        //        tran = con.BeginTransaction();

        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
        //                          " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
        //                          " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
        //                          " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
        //        cmd.Parameters.Clear();

        //        cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
        //        cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
        //        cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
        //        cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
        //        cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_SALE;
        //        cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
        //        cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
        //        cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
        //        cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
        //        cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
        //        cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
        //        cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
        //        cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

        //        cmd.Transaction = tran;
        //        cmd.ExecuteNonQuery();

        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
        //                          " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
        //                          " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
        //                          " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
        //        cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
        //        cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
        //        cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
        //        inc_Serial = ob.Serial_SALE;
        //        ser = int.Parse(inc_Serial) + 1;
        //        final_Serial = ser.ToString();
        //        cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
        //        cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
        //        cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
        //        cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
        //        cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
        //        cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
        //        cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
        //        cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
        //        cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

        //        cmd.Transaction = tran;
        //        cmd.ExecuteNonQuery();

        //        tran.Commit();
        //        if (con.State != ConnectionState.Closed)
        //            if (con.State != ConnectionState.Closed)con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        s = ex.Message;
        //    }
        //    return s;
        //}

        public string doProcess_SALE_Ret(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_SALE;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;

                // ob.Costpid = "";

                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.Serial_SALE;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                //if (ob.Costpid == "&nbsp;")
                //{
                //    ob.Costpid = "";
                //}
                //else
                //    ob.Costpid = ob.Costpid;
                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_SALE_DisCount(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Sl_Sale_dis;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;

                // ob.Costpid = "";

                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '',@CREDITCD, @DEBITCD,0,@AMOUNT, @REMARKS,'STK_TRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.Sl_Sale_dis;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                //if (ob.Costpid == "&nbsp;")
                //{
                //    ob.Costpid = "";
                //}
                //else
                //    ob.Costpid = ob.Costpid;
                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                //    ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                //cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_LC(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', '', '', '', @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO,@CHEQUEDT, @REMARKS,'LC_EXPENSE', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                //if (ob.Costpid == "&nbsp;")
                //{
                //    ob.Costpid = "";
                //}
                //else
                //    ob.Costpid = ob.Costpid;
                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', '', '', '', @CREDITCD, @DEBITCD,0, @AMOUNT, @CHEQUENO,@CHEQUEDT, @REMARKS,'LC_EXPENSE', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                //cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                //if (ob.Costpid == "&nbsp;")
                //{
                //    ob.Costpid = "";
                //}
                //else
                //    ob.Costpid = ob.Costpid;
                //cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                //cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                //if (ob.Chequeno == "&nbsp;")
                //{
                ob.Chequeno = "";
                //}
                //else
                //    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_MREC_Multiple(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;


            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();




                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE,@CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_MPAY_Multiple(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE, @CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_JOUR_Multiple(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE, @CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_CONT_Multiple(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'DEBIT', @TRANSFOR, @COSTPID, @TRANSMODE, @DEBITCD, @CREDITCD,@AMOUNT,0, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, TRANSDRCR, TRANSFOR, COSTPID, TRANSMODE, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " CHEQUENO, CHEQUEDT, REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO,'CREDIT', @TRANSFOR, @COSTPID, @TRANSMODE, @CREDITCD, @DEBITCD,0,@AMOUNT, @CHEQUENO, @CHEQUEDT, @REMARKS,'GL_MTRANS', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.Transfor;
                if (ob.Costpid == "&nbsp;")
                {
                    ob.Costpid = "";
                }
                else
                    ob.Costpid = ob.Costpid;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = ob.Costpid;
                cmd.Parameters.Add("@TRANSMODE", SqlDbType.NVarChar).Value = ob.Transmode;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                if (ob.Chequeno == "&nbsp;")
                {
                    ob.Chequeno = "";
                }
                else
                    ob.Chequeno = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUENO", SqlDbType.NVarChar).Value = ob.Chequeno;
                cmd.Parameters.Add("@CHEQUEDT", SqlDbType.DateTime).Value = ob.Chequedt;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Parse("01/01/1900");
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = "";

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string doProcess_MicroCreditCollection(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, COSTPID, TRANSDRCR, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO, @COSTPID, 'DEBIT', @DEBITCD, @CREDITCD, @AMOUNT, 0, @REMARKS,'MC_COLLECT', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER ( TRANSTP, TRANSDT, TRANSMY, TRANSNO, SERIALNO, COSTPID, TRANSDRCR, DEBITCD, CREDITCD, DEBITAMT, CREDITAMT, " +
                                  " REMARKS, TABLEID, USERPC, USERID, ACTDTI, IPADDRESS) " +
                                  " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSNO, @SERIALNO, @COSTPID, 'CREDIT', @CREDITCD, @DEBITCD, 0, @AMOUNT, @REMARKS,'MC_COLLECT', " +
                                  " @USERPC,@USERID, @ACTDTI, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                inc_Serial = ob.SerialNo_MREC;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = final_Serial;
                cmd.Parameters.Add("@COSTPID", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@ACTDTI", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        //public string doProcess_MicroCreditCollectionMember(AlchemyAccounting.multipurpose.InterFace.multipuposeInterface ob)
        //{
        //    string s = "";
        //    SqlTransaction tran = null;

        //    try
        //    {
        //        if (ob.SchTP=="DEPOSIT")
        //        {

        //            if (con.State != ConnectionState.Open)
        //                 if (con.State != ConnectionState.Open)con.Open();
        //            tran = con.BeginTransaction();
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "INSERT INTO MC_MLEDGER (TRANSTP, TRANSDT, TRANSMY, TRANSYY, TRANSNO, TRANSSL, MEMBER_ID, SCHEME_ID, INTERNALID, DEBITAMT, CREDITAMT, REMARKS, TABLEID, USERPC, USERID, IPADDRESS) " +
        //                              " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSYY, @TRANSNO, @TRANSSL, @MEMBER_ID, @SCHEME_ID, @INTERNALID, 0, @AMOUNT, @REMARKS,'MC_COLLECT', " +
        //                              " @USERPC, @USERID, @IPADDRESS)";
        //            cmd.Parameters.Clear();
        //            cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.TransTP;
        //            cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.TransDT;
        //            cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.TransMY;
        //            cmd.Parameters.Add("@TRANSYY", SqlDbType.Int).Value = ob.Year;
        //            cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.DocNo;
        //            //inc_Serial = ob.SerialNo_MREC;
        //            //ser = int.Parse(inc_Serial) + 1;
        //            //final_Serial = ser.ToString();
        //            cmd.Parameters.Add("@TRANSSL", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
        //            cmd.Parameters.Add("@MEMBER_ID", SqlDbType.NVarChar).Value = ob.MemberID;
        //            cmd.Parameters.Add("@SCHEME_ID", SqlDbType.NVarChar).Value = ob.SchemeID;
        //            cmd.Parameters.Add("@INTERNALID", SqlDbType.NVarChar).Value = ob.InternalID;
        //            cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
        //            cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
        //            cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.UserPc;
        //            cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserNm;
        //            cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ip;

        //            cmd.Transaction = tran;
        //            cmd.ExecuteNonQuery();
        //        }
        //        else
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "INSERT INTO MC_MLEDGER (TRANSTP, TRANSDT, TRANSMY, TRANSYY, TRANSNO, TRANSSL, MEMBER_ID, SCHEME_ID, INTERNALID, DEBITAMT, CREDITAMT, REMARKS, TABLEID, USERPC, USERID, IPADDRESS) " +
        //                              " Values (@TRANSTP, @TRANSDT, @TRANSMY, @TRANSYY, @TRANSNO, @TRANSSL, @MEMBER_ID, @SCHEME_ID, @INTERNALID, @AMOUNT, 0, @REMARKS,'MC_COLLECT', " +
        //                              " @USERPC,@USERID, @IPADDRESS)";
        //            cmd.Parameters.Clear();

        //            cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.TransTP;
        //            cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.TransDT;
        //            cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.TransMY;
        //            cmd.Parameters.Add("@TRANSYY", SqlDbType.Int).Value = ob.Year;
        //            cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.DocNo;
        //            cmd.Parameters.Add("@TRANSSL", SqlDbType.BigInt).Value = ob.SerialNo_MREC;
        //            cmd.Parameters.Add("@MEMBER_ID", SqlDbType.NVarChar).Value = ob.MemberID;
        //            cmd.Parameters.Add("@SCHEME_ID", SqlDbType.NVarChar).Value = ob.SchemeID;
        //            cmd.Parameters.Add("@INTERNALID", SqlDbType.NVarChar).Value = ob.InternalID;
        //            cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;
        //            cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
        //            cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.UserPc;
        //            cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserNm;
        //            cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ip;

        //            cmd.Transaction = tran;
        //            cmd.ExecuteNonQuery();
        //        }

        //        tran.Commit();
        //        if (con.State != ConnectionState.Closed)
        //            if (con.State != ConnectionState.Closed)con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        s = ex.Message;
        //    }
        //    return s;
        //}

        public string insert_GL_ACCCHART_DEPOT_DELAR_SHOPKEEPER_IMPORTER_Information(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO GL_ACCHART (ACCOUNTCD, ACCOUNTNM, OPENINGDT, LEVELCD, CONTROLCD, ACCOUNTTP, STATUSCD, USERPC, USERID, INTIME, IPADDRESS)
                        VALUES (@ACCOUNTCD, @ACCOUNTNM, @OPENINGDT, @LEVELCD, @CONTROLCD, @ACCOUNTTP, @STATUSCD, @USERPC, @USERID, @INTIME, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@ACCOUNTCD", SqlDbType.NVarChar).Value = ob.AccCD;
                cmd.Parameters.Add("@ACCOUNTNM", SqlDbType.NVarChar).Value = ob.AccNM;
                cmd.Parameters.Add("@OPENINGDT", SqlDbType.SmallDateTime).Value = ob.OpDT;
                cmd.Parameters.Add("@LEVELCD", SqlDbType.BigInt).Value = ob.LevelCD;
                cmd.Parameters.Add("@CONTROLCD", SqlDbType.NVarChar).Value = ob.ControlCD;
                cmd.Parameters.Add("@ACCOUNTTP", SqlDbType.Char).Value = ob.AccTP;
                cmd.Parameters.Add("@STATUSCD", SqlDbType.Char).Value = ob.StatusCD;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTM;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        public string update_GL_ACCCHART_DEPOT_DELAR_SHOPKEEPER_IMPORTER_Information(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE GL_ACCHART SET ACCOUNTNM =@ACCOUNTNM, UPDUSERPC =@UPDUSERPC, UPDUSERID =@UPDUSERID, UPDTIME =@UPDTIME, UPDIPADD =@UPDIPADD WHERE ACCOUNTCD =@ACCOUNTCD";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@ACCOUNTCD", SqlDbType.NVarChar).Value = ob.AccCD;
                cmd.Parameters.Add("@ACCOUNTNM", SqlDbType.NVarChar).Value = ob.AccNM;
                cmd.Parameters.Add("@UPDUSERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@UPDUSERID", SqlDbType.NVarChar).Value = ob.Username;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.SmallDateTime).Value = ob.InTM;
                cmd.Parameters.Add("@UPDIPADD", SqlDbType.NVarChar).Value = ob.Ipaddress;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }




        public string INSERT_ASL_BRANCH(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO ASL_BRANCH(COMPID,BRANCHCD,BRANCHID,BRANCHNM,ADDRESS,CONTACTNO,EMAILID,STATUS,USERPC,INSUSERID,INSTIME,INSIPNO,INSLTUDE)
 				Values 
				(@COMPID,@BRANCHCD,@BRANCHID,@BRANCHNM,@ADDRESS,@CONTACTNO,@EMAILID,@STATUS,@USERPC,@INSUSERID,@INSTIME,@INSIPNO,@INSLTUDE)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@COMPID", SqlDbType.BigInt).Value = ob.CompanyId;
                cmd.Parameters.Add("@BRANCHCD", SqlDbType.BigInt).Value = ob.BranchCode;
                cmd.Parameters.Add("@BRANCHID", SqlDbType.NVarChar).Value = ob.BranchId;
                cmd.Parameters.Add("@BRANCHNM", SqlDbType.NVarChar).Value = ob.BranchNm;
                cmd.Parameters.Add("@ADDRESS", SqlDbType.NVarChar).Value = ob.Address;
                cmd.Parameters.Add("@CONTACTNO", SqlDbType.NVarChar).Value = ob.ContactNo;
                cmd.Parameters.Add("@EMAILID", SqlDbType.NVarChar).Value = ob.EmailId;
                cmd.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = ob.Status;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.UserPcInsert;
                cmd.Parameters.Add("@INSIPNO", SqlDbType.NVarChar).Value = ob.IpAddressInsert;
                cmd.Parameters.Add("@INSLTUDE", SqlDbType.NVarChar).Value = ob.LotiLengTudeInsert;
                cmd.Parameters.Add("@INSTIME", SqlDbType.DateTime).Value = ob.InTimeInsert;
                cmd.Parameters.Add("@INSUSERID", SqlDbType.BigInt).Value = ob.UserIdInsert;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }


        public string DELETE_ASL_BRANCH(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"DELETE FROM ASL_BRANCH WHERE BRANCHCD=@BRANCHCD AND COMPID=@COMPID ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@BRANCHCD", SqlDbType.NVarChar).Value = ob.BranchCode;
                cmd.Parameters.Add("@COMPID", SqlDbType.BigInt).Value = ob.CompanyId;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }


        public string UPDATE_ASL_BRANCH(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE ASL_BRANCH SET BRANCHNM=@BRANCHNM,BRANCHID=@BRANCHID,CONTACTNO=@CONTACTNO,
                    ADDRESS=@ADDRESS,
                    EMAILID=@EMAILID,STATUS=@STATUS,UPDUSERID=@UPDUSERID,UPDTIME=@UPDTIME,UPDIPNO=@UPDIPNO,
                    UPDLTUDE=@UPDLTUDE WHERE BRANCHCD=@BRANCHCD AND COMPID=@COMPID ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@COMPID", SqlDbType.BigInt).Value = ob.CompanyId;
                cmd.Parameters.Add("@BRANCHCD", SqlDbType.NVarChar).Value = ob.BranchCode;

                cmd.Parameters.Add("@BRANCHNM", SqlDbType.NVarChar).Value = ob.BranchNm;
                cmd.Parameters.Add("@BRANCHID", SqlDbType.NVarChar).Value = ob.BranchId;
                cmd.Parameters.Add("@CONTACTNO", SqlDbType.NVarChar).Value = ob.ContactNo;
                cmd.Parameters.Add("@ADDRESS", SqlDbType.NVarChar).Value = ob.Address;
                cmd.Parameters.Add("@EMAILID", SqlDbType.NVarChar).Value = ob.EmailId;
                cmd.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = ob.Status;

                cmd.Parameters.Add("@UPDUSERID", SqlDbType.BigInt).Value = ob.UserIdUpdate;
                cmd.Parameters.Add("@UPDTIME", SqlDbType.DateTime).Value = ob.InTimeUpdate;
                cmd.Parameters.Add("@UPDIPNO", SqlDbType.NVarChar).Value = ob.IpAddressUpdate;
                cmd.Parameters.Add("@UPDLTUDE", SqlDbType.NVarChar).Value = ob.LotiLengTudeUpdate;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        internal string doProcess_DO(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD, DEBITAMT,CREDITAMT, REMARKS, USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP,@TRANSDT,@TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD, @DEBITAMT,@CREDITAMT, @REMARKS,  @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "DEBIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_Do;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                inc_Serial = ob.Serial_Do;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD,DEBITAMT,CREDITAMT, REMARKS, USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP,@TRANSDT,@TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD,@DEBITAMT,@CREDITAMT, @REMARKS,  @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "CREDIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = Convert.ToInt64(final_Serial);
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();


                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        internal string doProcess_MR(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {


                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD, DEBITAMT,CREDITAMT,REMARKS,  USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP, @TRANSDT,  @TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD, @DEBITAMT,@CREDITAMT, @REMARKS, @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "DEBIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_Mr;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                inc_Serial = ob.Serial_Mr;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD, DEBITAMT,CREDITAMT,REMARKS,  USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP, @TRANSDT,  @TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD, @DEBITAMT,@CREDITAMT,  @REMARKS, @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "CREDIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = Convert.ToInt64(final_Serial);
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }


        internal string doProcess_DNote(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD,DEBITAMT,CREDITAMT, REMARKS,   USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP, @TRANSDT,  @TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD,  @DEBITAMT,@CREDITAMT, @REMARKS,  @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "DEBIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_Dnote;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                inc_Serial = ob.Serial_Dnote;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD,DEBITAMT,CREDITAMT, REMARKS,   USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP, @TRANSDT,  @TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD, @DEBITAMT,@CREDITAMT,@REMARKS,  @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "CREDIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = Convert.ToInt64(final_Serial);
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                s = ex.Message;
            }
            return s;
        }

        internal string doProcess_CNote(AccountInterface ob)
        {
            string s = "";
            SqlTransaction tran = null;

            try
            {
                if (con.State != ConnectionState.Open)
                     if (con.State != ConnectionState.Open)con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD,DEBITAMT,CREDITAMT, REMARKS,  USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP, @TRANSDT,  @TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD, @DEBITAMT,@CREDITAMT, @REMARKS,  @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "DEBIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = ob.Serial_Cnote;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                inc_Serial = ob.Serial_Cnote;
                ser = int.Parse(inc_Serial) + 1;
                final_Serial = ser.ToString();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO GL_MASTER (TRANSDRCR,TABLEID,TRANSTP, TRANSDT,  TRANSMY, TRANSNO, SERIALNO, DEBITCD, CREDITCD, REMARKS,  USERPC, USERID, IPADDRESS) " +
                                  " Values (@TRANSDRCR,@TABLEID,@TRANSTP, @TRANSDT,  @TRANSMY, @TRANSNO, @SERIALNO, @DEBITCD, @CREDITCD,  @REMARKS,  @USERPC, @USERID, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDRCR", SqlDbType.NVarChar).Value = "CREDIT";
                cmd.Parameters.Add("@TABLEID", SqlDbType.NVarChar).Value = ob.tableid;
                cmd.Parameters.Add("@TRANSTP", SqlDbType.NVarChar).Value = ob.Transtp;
                cmd.Parameters.Add("@TRANSDT", SqlDbType.DateTime).Value = ob.Transdt;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.Monyear;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TransNo;
                cmd.Parameters.Add("@SERIALNO", SqlDbType.BigInt).Value = Convert.ToInt64(final_Serial);
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.Creditcd;
                cmd.Parameters.Add("@CREDITCD", SqlDbType.NVarChar).Value = ob.Debitcd;
                cmd.Parameters.Add("@DEBITAMT", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@CREDITAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.PcName;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.userName;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.ip;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
                if (con.State != ConnectionState.Closed)
                    if (con.State != ConnectionState.Closed)con.Close();
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