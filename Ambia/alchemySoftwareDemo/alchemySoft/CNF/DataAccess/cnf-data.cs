using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using alchemySoft.CNF.Interface;

namespace alchemySoft.CNF.DataAccess
{
    public class cnf_data
    {
        SqlConnection con;
        SqlCommand cmd;

        public cnf_data()
        {
            con = new SqlConnection(dbFunctions.Connection);
            cmd = new SqlCommand("", con);
        }

        public string save_cnf_job(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO CNF_JOB (JOBCDT, COMPID, REGID, JOBQUALITY, JOBYY, JOBTP, JOBNO,REFTP, PARTYID, CONSIGNEENM, CONSIGNEEADD, SUPPLIERNM, PKGS,MVESSEL,ROTNO,LINE_NO,CLRDON,FRWDT, CBM, GOODS, WTGROSS, WTNET, CNFV_USD, CNFV_ETP," +
                    " CNFV_ERT, CNFV_BDT, CRFV_USD, ASSV_BDT, COMM_AMT, CONTNO, DOCINVNO, DOCRCVDT, CRFNO,PKGSTP, CRFDT, BENO, BEDT, BLNO, BLDT, LCNO, LCDT, PERMITNO, PERMITDT, DELIVERYDT,  AWBNO, AWBDT, HBLNO, HBLDT, HAWBNO, HAWBDT, UNTKNO, " +
                    "COM_REMARKS, STATUS, USERPC, USERID, INTIME, IPADDRESS, VATNO) " +
                    " VALUES (@JOBCDT, @COMPID, @REGID, @JOBQUALITY, @JOBYY, @JOBTP, @JOBNO,@REFTP, @PARTYID, @CONSIGNEENM, @CONSIGNEEADD, @SUPPLIERNM, @PKGS,@MVESSEL,@ROTNO,@LINE_NO,@CLRDON,@FRWDT, @CBM, @GOODS, @WTGROSS, @WTNET, @CNFV_USD, @CNFV_ETP," +
                    " @CNFV_ERT, @CNFV_BDT, @CRFV_USD, @ASSV_BDT, @COMM_AMT, @CONTNO, @DOCINVNO, @DOCRCVDT, @CRFNO,@PKGSTP, @CRFDT, @BENO, @BEDT, @BLNO, @BLDT, @LCNO, @LCDT, @PERMITNO, @PERMITDT, @DELIVERYDT,@AWBNO, @AWBDT, @HBLNO, @HBLDT, @HAWBNO, " +
                    "@HAWBDT, @UNTKNO, @COM_REMARKS, @STATUS, @USERPC, @USERID, @INTIME, @IPADDRESS,@VATNO)";

                //WFDT,UNTKDT
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@JOBCDT", SqlDbType.Date).Value = ob.JobCrDT;
                cmd.Parameters.Add("@COMPID", SqlDbType.NVarChar).Value = ob.CompID;
                cmd.Parameters.Add("@REGID", SqlDbType.NVarChar).Value = ob.RegID;
                cmd.Parameters.Add("@JOBQUALITY", SqlDbType.NVarChar).Value = ob.JobQuality;
                cmd.Parameters.Add("@JOBYY", SqlDbType.Int).Value = ob.JobYear;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JobTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JobNo;
                cmd.Parameters.Add("@REFTP", SqlDbType.NVarChar).Value = ob.ReffereceType;
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PartyID;
                cmd.Parameters.Add("@CONSIGNEENM", SqlDbType.NVarChar).Value = ob.ConsigneeName;
                cmd.Parameters.Add("@CONSIGNEEADD", SqlDbType.NVarChar).Value = ob.ConsigneeAddress;
                cmd.Parameters.Add("@SUPPLIERNM", SqlDbType.NVarChar).Value = ob.SupplierNM;
                cmd.Parameters.Add("@PKGS", SqlDbType.Decimal).Value = ob.PkgDetails;
                cmd.Parameters.Add("@PKGSTP", SqlDbType.NVarChar).Value = ob.PackageType;

                cmd.Parameters.Add("@MVESSEL", SqlDbType.NVarChar).Value = ob.Vessel;
                cmd.Parameters.Add("@ROTNO", SqlDbType.NVarChar).Value = ob.RotNo;
                cmd.Parameters.Add("@LINE_NO", SqlDbType.NVarChar).Value = ob.LineNO;
                cmd.Parameters.Add("@CLRDON", SqlDbType.NVarChar).Value = ob.ClearedOn;
                cmd.Parameters.Add("@FRWDT", SqlDbType.Date).Value = ob.JobforWrdDT;

                cmd.Parameters.Add("@CBM", SqlDbType.Decimal).Value = ob.CBM;
                cmd.Parameters.Add("@GOODS", SqlDbType.NVarChar).Value = ob.GoodsDesc;
                cmd.Parameters.Add("@WTGROSS", SqlDbType.Decimal).Value = ob.GrossWeight;
                cmd.Parameters.Add("@WTNET", SqlDbType.Decimal).Value = ob.NetWeight;
                cmd.Parameters.Add("@CNFV_USD", SqlDbType.Decimal).Value = ob.CnfUSD;
                cmd.Parameters.Add("@CNFV_ETP", SqlDbType.NVarChar).Value = ob.ExTP;
                cmd.Parameters.Add("@CNFV_ERT", SqlDbType.Decimal).Value = ob.ExchangeRT;
                cmd.Parameters.Add("@CNFV_BDT", SqlDbType.Decimal).Value = ob.CnfBDT;
                cmd.Parameters.Add("@CRFV_USD", SqlDbType.Decimal).Value = ob.CnfUSD;
                cmd.Parameters.Add("@ASSV_BDT", SqlDbType.Decimal).Value = ob.AssessableAMT;
                cmd.Parameters.Add("@COMM_AMT", SqlDbType.Decimal).Value = ob.Commission;
                cmd.Parameters.Add("@CONTNO", SqlDbType.NVarChar).Value = ob.ContainerNo;
                cmd.Parameters.Add("@DOCINVNO", SqlDbType.NVarChar).Value = ob.InNO;
                cmd.Parameters.Add("@DOCRCVDT", SqlDbType.Date).Value = ob.InDT;
                cmd.Parameters.Add("@CRFNO", SqlDbType.NVarChar).Value = ob.CrfNO;
                cmd.Parameters.Add("@CRFDT", SqlDbType.Date).Value = ob.CrfDT;
                cmd.Parameters.Add("@BENO", SqlDbType.NVarChar).Value = ob.BeNO;
                cmd.Parameters.Add("@BEDT", SqlDbType.Date).Value = ob.BeDT;
                cmd.Parameters.Add("@BLNO", SqlDbType.NVarChar).Value = ob.BlNO;
                cmd.Parameters.Add("@BLDT", SqlDbType.Date).Value = ob.BlDT;
                cmd.Parameters.Add("@LCNO", SqlDbType.NVarChar).Value = ob.LcNO;
                cmd.Parameters.Add("@LCDT", SqlDbType.Date).Value = ob.LcDT;
                cmd.Parameters.Add("@PERMITNO", SqlDbType.NVarChar).Value = ob.PermitNO;
                cmd.Parameters.Add("@PERMITDT", SqlDbType.Date).Value = ob.PermitDT;
                cmd.Parameters.Add("@DELIVERYDT", SqlDbType.Date).Value = ob.DelDT;
                //cmd.Parameters.Add("@WFDT", SqlDbType.Date).Value = ob.WharfentDT;
                cmd.Parameters.Add("@UNTKNO", SqlDbType.NVarChar).Value = ob.UnderTakeNo;
                //cmd.Parameters.Add("@UNTKDT", SqlDbType.Date).Value = ob.UnderTakeDt;
                cmd.Parameters.Add("@COM_REMARKS", SqlDbType.NVarChar).Value = ob.ComRemarks;
                cmd.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = ob.Status;
                cmd.Parameters.Add("@AWBNO", SqlDbType.NVarChar).Value = ob.Awbno;
                cmd.Parameters.Add("@AWBDT", SqlDbType.DateTime).Value = ob.Awbdt;
                cmd.Parameters.Add("@HBLNO", SqlDbType.NVarChar).Value = ob.Hbl;
                cmd.Parameters.Add("@HBLDT", SqlDbType.DateTime).Value = ob.Hbldt;
                cmd.Parameters.Add("@HAWBNO", SqlDbType.NVarChar).Value = ob.Hawbno;
                cmd.Parameters.Add("@HAWBDT", SqlDbType.DateTime).Value = ob.Hawbdt;
                cmd.Parameters.Add("@VATNO", SqlDbType.NVarChar).Value = ob.VatNo;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserNM;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTM;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

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

        public string update_cnf_job(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE CNF_JOB SET JOBCDT =@JOBCDT, REGID =@REGID, JOBQUALITY =@JOBQUALITY,REFTP=@REFTP, PARTYID =@PARTYID, CONSIGNEENM =@CONSIGNEENM, CONSIGNEEADD =@CONSIGNEEADD, SUPPLIERNM =@SUPPLIERNM, PKGS =@PKGS,MVESSEL=@MVESSEL,ROTNO=@ROTNO,LINE_NO=@LINE_NO,CLRDON=@CLRDON,FRWDT=@FRWDT ,CBM=@CBM, GOODS =@GOODS, WTGROSS =@WTGROSS, " +
                      " WTNET =@WTNET, CNFV_USD =@CNFV_USD, CNFV_ETP =@CNFV_ETP, CNFV_ERT =@CNFV_ERT, CNFV_BDT =@CNFV_BDT, CRFV_USD =@CRFV_USD, ASSV_BDT =@ASSV_BDT, COMM_AMT =@COMM_AMT, CONTNO =@CONTNO, DOCINVNO =@DOCINVNO, DOCRCVDT =@DOCRCVDT, CRFNO =@CRFNO, CRFDT =@CRFDT, BENO =@BENO, BEDT =@BEDT, " +
                      " BLNO =@BLNO, BLDT =@BLDT, LCNO =@LCNO, LCDT =@LCDT, PERMITNO =@PERMITNO, PERMITDT =@PERMITDT, DELIVERYDT =@DELIVERYDT,  AWBNO =@AWBNO, AWBDT =@AWBDT, HBLNO =@HBLNO, HBLDT =@HBLDT, HAWBNO =@HAWBNO, HAWBDT =@HAWBDT," +
                      " UNTKNO =@UNTKNO,  COM_REMARKS =@COM_REMARKS,PKGSTP=@PKGSTP, VATNO=@VATNO, STATUS =@STATUS, USERPC =@USERPC, UPDATEUSERID =@UPDATEUSERID, UPDATETIME =@UPDATETIME, IPADDRESS =@IPADDRESS WHERE JOBYY =@JOBYY AND JOBTP =@JOBTP AND JOBNO =@JOBNO";
                //UNTKDT,WFDT
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@JOBCDT", SqlDbType.Date).Value = ob.JobCrDT;
                cmd.Parameters.Add("@REGID", SqlDbType.NVarChar).Value = ob.RegID;
                cmd.Parameters.Add("@JOBQUALITY", SqlDbType.NVarChar).Value = ob.JobQuality;
                cmd.Parameters.Add("@JOBYY", SqlDbType.Int).Value = ob.JobYear;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JobTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JobNo;
                cmd.Parameters.Add("@REFTP", SqlDbType.NVarChar).Value = ob.ReffereceType;
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PartyID;
                cmd.Parameters.Add("@CONSIGNEENM", SqlDbType.NVarChar).Value = ob.ConsigneeName;
                cmd.Parameters.Add("@CONSIGNEEADD", SqlDbType.NVarChar).Value = ob.ConsigneeAddress;
                cmd.Parameters.Add("@SUPPLIERNM", SqlDbType.NVarChar).Value = ob.SupplierNM;
                cmd.Parameters.Add("@PKGS", SqlDbType.Decimal).Value = ob.PkgDetails;
                cmd.Parameters.Add("@PKGSTP", SqlDbType.NVarChar).Value = ob.PackageType;

                cmd.Parameters.Add("@MVESSEL", SqlDbType.NVarChar).Value = ob.Vessel;
                cmd.Parameters.Add("@ROTNO", SqlDbType.NVarChar).Value = ob.RotNo;
                cmd.Parameters.Add("@LINE_NO", SqlDbType.NVarChar).Value = ob.LineNO;
                cmd.Parameters.Add("@CLRDON", SqlDbType.NVarChar).Value = ob.ClearedOn;
                cmd.Parameters.Add("@FRWDT", SqlDbType.Date).Value = ob.JobforWrdDT;

                cmd.Parameters.Add("@CBM", SqlDbType.Decimal).Value = ob.CBM;
                cmd.Parameters.Add("@GOODS", SqlDbType.NVarChar).Value = ob.GoodsDesc;
                cmd.Parameters.Add("@WTGROSS", SqlDbType.Decimal).Value = ob.GrossWeight;
                cmd.Parameters.Add("@WTNET", SqlDbType.Decimal).Value = ob.NetWeight;
                cmd.Parameters.Add("@CNFV_USD", SqlDbType.Decimal).Value = ob.CnfUSD;
                cmd.Parameters.Add("@CNFV_ETP", SqlDbType.NVarChar).Value = ob.ExTP;
                cmd.Parameters.Add("@CNFV_ERT", SqlDbType.Decimal).Value = ob.ExchangeRT;
                cmd.Parameters.Add("@CNFV_BDT", SqlDbType.Decimal).Value = ob.CnfBDT;
                cmd.Parameters.Add("@CRFV_USD", SqlDbType.Decimal).Value = ob.CnfUSD;
                cmd.Parameters.Add("@ASSV_BDT", SqlDbType.Decimal).Value = ob.AssessableAMT;
                cmd.Parameters.Add("@COMM_AMT", SqlDbType.Decimal).Value = ob.Commission;
                cmd.Parameters.Add("@CONTNO", SqlDbType.NVarChar).Value = ob.ContainerNo;
                cmd.Parameters.Add("@DOCINVNO", SqlDbType.NVarChar).Value = ob.InNO;
                cmd.Parameters.Add("@DOCRCVDT", SqlDbType.Date).Value = ob.InDT;
                cmd.Parameters.Add("@CRFNO", SqlDbType.NVarChar).Value = ob.CrfNO;
                cmd.Parameters.Add("@CRFDT", SqlDbType.Date).Value = ob.CrfDT;
                cmd.Parameters.Add("@BENO", SqlDbType.NVarChar).Value = ob.BeNO;
                cmd.Parameters.Add("@BEDT", SqlDbType.Date).Value = ob.BeDT;
                cmd.Parameters.Add("@BLNO", SqlDbType.NVarChar).Value = ob.BlNO;
                cmd.Parameters.Add("@BLDT", SqlDbType.Date).Value = ob.BlDT;
                cmd.Parameters.Add("@LCNO", SqlDbType.NVarChar).Value = ob.LcNO;
                cmd.Parameters.Add("@LCDT", SqlDbType.Date).Value = ob.LcDT;
                cmd.Parameters.Add("@PERMITNO", SqlDbType.NVarChar).Value = ob.PermitNO;
                cmd.Parameters.Add("@PERMITDT", SqlDbType.Date).Value = ob.PermitDT;
                cmd.Parameters.Add("@DELIVERYDT", SqlDbType.Date).Value = ob.DelDT;
                // cmd.Parameters.Add("@WFDT", SqlDbType.Date).Value = ob.WharfentDT;
                cmd.Parameters.Add("@AWBNO", SqlDbType.NVarChar).Value = ob.Awbno;
                cmd.Parameters.Add("@AWBDT", SqlDbType.DateTime).Value = ob.Awbdt;
                cmd.Parameters.Add("@HBLNO", SqlDbType.NVarChar).Value = ob.Hbl;
                cmd.Parameters.Add("@HBLDT", SqlDbType.DateTime).Value = ob.Hbldt;
                cmd.Parameters.Add("@HAWBNO", SqlDbType.NVarChar).Value = ob.Hawbno;
                cmd.Parameters.Add("@HAWBDT", SqlDbType.DateTime).Value = ob.Hawbdt;
                cmd.Parameters.Add("@UNTKNO", SqlDbType.NVarChar).Value = ob.UnderTakeNo;
                //cmd.Parameters.Add("@UNTKDT", SqlDbType.Date).Value = ob.UnderTakeDt;
                cmd.Parameters.Add("@COM_REMARKS", SqlDbType.NVarChar).Value = ob.ComRemarks;
                cmd.Parameters.Add("@VATNO", SqlDbType.NVarChar).Value = ob.VatNo;
                cmd.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = ob.Status;

                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpTM;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@UPDATEUSERID", SqlDbType.NVarChar).Value = ob.UpdateUser;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

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


        public string SaveJobReceive(cnf_Interface ob)
        {

            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "insert into CNF_JOBRCV (TRANSDT, TRANSMY, TRANSNO,TRANSFOR,COMPID,JOBYY,JOBTP,JOBNO,PARTYID,DEBITCD, REMARKS,AMOUNT, USERPC, INTIME, IPADDRESS)" +
                    "values(@TRANSDT, @TRANSMY, @TRANSNO,@TRANSFOR,@COMPID,@JOBYY,@JOBTP,@JOBNO,@PARTYID,@DEBITCD, @REMARKS,@AMOUNT, @USERPC, @INTIME, @IPADDRESS )";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDT", SqlDbType.SmallDateTime).Value = ob.TRANSDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.TRANSMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TRANSNO;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.TRANSFOR;

                cmd.Parameters.Add("@COMPID", SqlDbType.NVarChar).Value = ob.CompID;
                cmd.Parameters.Add("@JOBYY", SqlDbType.BigInt).Value = ob.JOBYY;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JOBTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JOBNO;
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.DEBITCD;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.REMARKS;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string UpdateJobReceive(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "update CNF_JOBRCV set TRANSFOR=@TRANSFOR, COMPID=@COMPID, JOBYY=@JOBYY, JOBTP=@JOBTP, JOBNO=@JOBNO, PARTYID=@PARTYID, DEBITCD=@DEBITCD, REMARKS=@REMARKS, AMOUNT=@AMOUNT, USERPC =@USERPC, UPDATETIME =@UPDATETIME, IPADDRESS =@IPADDRESS where TRANSDT=@TRANSDT and TRANSMY=@TRANSMY and TRANSNO=@TRANSNO";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDT", SqlDbType.SmallDateTime).Value = ob.TRANSDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.TRANSMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TRANSNO;
                cmd.Parameters.Add("@TRANSFOR", SqlDbType.NVarChar).Value = ob.TRANSFOR;

                cmd.Parameters.Add("@COMPID", SqlDbType.NVarChar).Value = ob.CompID;
                cmd.Parameters.Add("@JOBYY", SqlDbType.BigInt).Value = ob.JOBYY;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JOBTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JOBNO;
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;
                cmd.Parameters.Add("@DEBITCD", SqlDbType.NVarChar).Value = ob.DEBITCD;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.REMARKS;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.Decimal).Value = ob.Amount;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string DeleteJobReceive(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "DELETE FROM CNF_JOBRCV where TRANSMY=@TRANSMY and TRANSNO=@TRANSNO";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.TRANSMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.TRANSNO;

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


        public string INSERT_CNF_JOBEXP_EXTENDEDMST(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO CNF_JOBEXPMST(TRANSDT,TRANSMY,TRANSNO,EXPCD,REMARKS,USERPC,USERID,INTIME,IPADDRESS)
 				Values 
				(@TRANSDT,@TRANSMY,@TRANSNO,@EXPCD,@REMARKS,@USERPC,@USERID,@INTIME,@IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDT", SqlDbType.Date).Value = ob.ExDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.ExMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.InvoiceNO;
                cmd.Parameters.Add("@EXPCD", SqlDbType.NVarChar).Value = ob.ExpenseCD;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.RemarksTOP;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserNM;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.InTM;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

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

        public string save_cnf_job_expense_Extended(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO CNF_JOBEXP (TRANSDT, TRANSMY, TRANSNO, COMPID, JOBYY, JOBTP, JOBNO, EXPCD, SLNO, EXPID, EXPAMT, REMARKS, USERPC, USERID, INTIME, IPADDRESS) " +
                     " VALUES (@TRANSDT, @TRANSMY, @TRANSNO, @COMPID, @JOBYY, @JOBTP, @JOBNO, @EXPCD, @SLNO, @EXPID, @EXPAMT, @REMARKS, @USERPC, @USERID, @INTIME, @IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDT", SqlDbType.Date).Value = ob.ExDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.ExMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.InvoiceNO;
                cmd.Parameters.Add("@COMPID", SqlDbType.NVarChar).Value = ob.CompID;
                cmd.Parameters.Add("@JOBYY", SqlDbType.Int).Value = ob.JobYear;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JobTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JobNo;
                cmd.Parameters.Add("@EXPCD", SqlDbType.NVarChar).Value = ob.ExpenseCD;
                cmd.Parameters.Add("@SLNO", SqlDbType.BigInt).Value = ob.Sl;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.ExpensesID;
                cmd.Parameters.Add("@EXPAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.RemarksBOT;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserID;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.InTM;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

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

        public string UPDATE_CNF_JOBEXP_EXTENDED_MST(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE CNF_JOBEXPMST SET EXPCD=@EXPCD,REMARKS=@REMARKS WHERE TRANSMY=@TRANSMY AND TRANSNO=@TRANSNO ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.ExMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.InvoiceNO;
                cmd.Parameters.Add("@EXPCD", SqlDbType.NVarChar).Value = ob.ExpenseCD;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.RemarksTOP;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UpdateUser;
                cmd.Parameters.Add("@INTIME", SqlDbType.DateTime).Value = ob.UpTM;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

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
        public string UPDATE_CNF_JOBEXP_EXTENDED(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE CNF_JOBEXP SET TRANSDT=@TRANSDT,JOBYY=@JOBYY,JOBTP=@JOBTP,JOBNO=@JOBNO,EXPID=@EXPID,EXPAMT=@EXPAMT,REMARKS=@REMARKS,USERPC=@USERPC,
                UPDATEUSERID=@UPDATEUSERID,UPDATETIME=@UPDATETIME,IPADDRESS=@IPADDRESS WHERE TRANSMY=@TRANSMY AND TRANSNO=@TRANSNO AND SLNO=@SLNO ";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSDT", SqlDbType.Date).Value = ob.ExDT;
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.ExMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.InvoiceNO;
                cmd.Parameters.Add("@JOBYY", SqlDbType.Int).Value = ob.JobYear;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JobTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JobNo;
                cmd.Parameters.Add("@SLNO", SqlDbType.BigInt).Value = ob.Sl;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.ExpensesID;
                cmd.Parameters.Add("@EXPAMT", SqlDbType.Decimal).Value = ob.Amount;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.RemarksBOT;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@UPDATEUSERID", SqlDbType.NVarChar).Value = ob.UpdateUser;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.DateTime).Value = ob.UpTM;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;

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

        public string delete_cnf_job_expense(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM CNF_JOBEXP WHERE TRANSMY =@TRANSMY AND TRANSNO =@TRANSNO AND SLNO =@SLNO";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.ExMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.InvoiceNO;
                cmd.Parameters.Add("@SLNO", SqlDbType.BigInt).Value = ob.Sl;

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

        public string delete_cnf_job_expense_master(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM CNF_JOBEXPMST WHERE TRANSMY =@TRANSMY AND TRANSNO =@TRANSNO";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TRANSMY", SqlDbType.NVarChar).Value = ob.ExMY;
                cmd.Parameters.Add("@TRANSNO", SqlDbType.BigInt).Value = ob.InvoiceNO;

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


        public string SaveJobBillInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "insert into CNF_JOBBILL(COMPID, JOBYY, JOBTP,JOBNO,PARTYID,BILLDT,BILLNO,EXPSL,EXPID,EXPAMT, BILLAMT,EXPPDT,REMARKS,BILLSL, USERPC, USERID, INTIME, IPADDRESS)" +
                    "values(@COMPID, @JOBYY, @JOBTP,@JOBNO,@PARTYID,@BILLDT, @BILLNO, @EXPSL, @EXPID,@EXPAMT, @BILLAMT,@EXPPDT,@REMARKS,@BILLSL, @USERPC, @USERID, @INTIME, @IPADDRESS )";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@COMPID", SqlDbType.NVarChar).Value = ob.CompID;
                cmd.Parameters.Add("@JOBYY", SqlDbType.BigInt).Value = ob.JOBYY;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JOBTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JOBNO;

                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;
                cmd.Parameters.Add("@BILLDT", SqlDbType.SmallDateTime).Value = ob.BILLDT;
                cmd.Parameters.Add("@BILLNO", SqlDbType.BigInt).Value = ob.BILLNO;
                cmd.Parameters.Add("@EXPSL", SqlDbType.BigInt).Value = ob.EXPSL;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.EXPID;
                cmd.Parameters.Add("@EXPAMT", SqlDbType.Decimal).Value = ob.EXPAMT;
                cmd.Parameters.Add("@BILLAMT", SqlDbType.Decimal).Value = ob.BILLAMT;
                cmd.Parameters.Add("@EXPPDT", SqlDbType.SmallDateTime).Value = ob.EXPPDT;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.REMARKS;
                cmd.Parameters.Add("@BILLSL", SqlDbType.BigInt).Value = ob.BILLSL;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserID;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string UpdateJobBillInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "update CNF_JOBBILL set BILLAMT=@BILLAMT, REMARKS=@REMARKS, EXPPDT=@EXPPDT, BILLSL=@BILLSL, USERPC =@USERPC, UPDATEUSERID =@UPDATEUSERID, UPDATETIME =@UPDATETIME, IPADDRESS =@IPADDRESS where BILLDT=@BILLDT and BILLNO=@BILLNO and EXPSL=@EXPSL and JOBYY=@JOBYY and JOBTP=@JOBTP and JOBNO=@JOBNO and EXPID=@EXPID";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@COMPID", SqlDbType.NVarChar).Value = ob.CompID;
                cmd.Parameters.Add("@JOBYY", SqlDbType.BigInt).Value = ob.JOBYY;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JOBTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JOBNO;

                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;
                cmd.Parameters.Add("@BILLDT", SqlDbType.SmallDateTime).Value = ob.BILLDT;
                cmd.Parameters.Add("@BILLNO", SqlDbType.BigInt).Value = ob.BILLNO;
                cmd.Parameters.Add("@EXPSL", SqlDbType.BigInt).Value = ob.EXPSL;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.EXPID;
                cmd.Parameters.Add("@EXPAMT", SqlDbType.Decimal).Value = ob.EXPAMT;
                cmd.Parameters.Add("@BILLAMT", SqlDbType.Decimal).Value = ob.BILLAMT;
                cmd.Parameters.Add("@EXPPDT", SqlDbType.SmallDateTime).Value = ob.EXPPDT;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.REMARKS;
                cmd.Parameters.Add("@BILLSL", SqlDbType.BigInt).Value = ob.BILLSL;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@UPDATEUSERID", SqlDbType.NVarChar).Value = ob.UpdateUser;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string RemoveJobBillInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "delete from CNF_JOBBILL where BILLDT=@BILLDT and BILLNO=@BILLNO and EXPSL=@EXPSL and JOBYY=@JOBYY and JOBTP=@JOBTP and JOBNO=@JOBNO and EXPID=@EXPID";

                cmd.Parameters.Clear();

                cmd.Parameters.Add("@COMPID", SqlDbType.NVarChar).Value = ob.CompID;
                cmd.Parameters.Add("@JOBYY", SqlDbType.BigInt).Value = ob.JOBYY;
                cmd.Parameters.Add("@JOBTP", SqlDbType.NVarChar).Value = ob.JOBTP;
                cmd.Parameters.Add("@JOBNO", SqlDbType.BigInt).Value = ob.JOBNO;

                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;
                cmd.Parameters.Add("@BILLDT", SqlDbType.SmallDateTime).Value = ob.BILLDT;
                cmd.Parameters.Add("@BILLNO", SqlDbType.BigInt).Value = ob.BILLNO;
                cmd.Parameters.Add("@EXPSL", SqlDbType.BigInt).Value = ob.EXPSL;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.EXPID;
                cmd.Parameters.Add("@EXPAMT", SqlDbType.Decimal).Value = ob.EXPAMT;
                cmd.Parameters.Add("@BILLAMT", SqlDbType.Decimal).Value = ob.BILLAMT;
                cmd.Parameters.Add("@EXPPDT", SqlDbType.SmallDateTime).Value = ob.EXPPDT;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.REMARKS;
                cmd.Parameters.Add("@BILLSL", SqlDbType.BigInt).Value = ob.BILLSL;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        internal string EditExpenseInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update CNF_EXPENSE set EXPNM=@EXPNM, REMARKS=@REMARKS  where EXPCID=@EXPCID and EXPID=@EXPID ";

                cmd.Parameters.Clear();

                cmd.Parameters.Add("@EXPCID", SqlDbType.NVarChar).Value = ob.EXPCID;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.EXPID;

                cmd.Parameters.Add("@EXPNM", SqlDbType.NVarChar).Value = ob.EXPNM;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.REMARKS;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;



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

        internal string DeleteExpenseInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM CNF_EXPENSE WHERE EXPCID =@EXPCID AND EXPID =@EXPID";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@EXPCID", SqlDbType.NVarChar).Value = ob.EXPCID;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.EXPID;

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

        public string SaveExpenseInfo(cnf_Interface ob)
        {

            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "insert into CNF_EXPENSE (EXPCID, EXPID, EXPNM, REMARKS, USERPC, INTIME, UPDATETIME, IPADDRESS)" +
                    "values(@EXPCID, @EXPID,@EXPNM, @REMARKS,@USERPC, @INTIME , @UPDATETIME,@IPADDRESS  )";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@EXPCID", SqlDbType.NVarChar).Value = ob.EXPCID;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.EXPID;
                cmd.Parameters.Add("@EXPNM", SqlDbType.NVarChar).Value = ob.EXPNM;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.REMARKS;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        internal DataTable showExpenseInfo(string id)
        {
            DataTable table = new DataTable();
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select EXPCID , EXPCNM from CNF_EXPMST where EXPCID='" + id + "' ";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            catch { }
            return table;
        }
        public string MstInput(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "insert into CNF_EXPMST(EXPCID, EXPCNM, USERPC, INTIME, UPDATETIME, IPADDRESS)" +
                    "values(@EXPCID, @EXPCNM,@USERPC, @INTIME , @UPDATETIME,@IPADDRESS  )";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@EXPCID", SqlDbType.NVarChar).Value = ob.EXPCID;
                cmd.Parameters.Add("@EXPCNM", SqlDbType.NVarChar).Value = ob.EXPCNM;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string CreateParty(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "insert into CNF_PARTY (PARTYID, ADDRESS, CONTACTNO, EMAILID,WEBID,APNM,APNO,STATUS, USERPC, INTIME, UPDATETIME, IPADDRESS, LOGINID, LOGINPW)" +
                    "values(@PARTYID, @ADDRESS,@CONTACTNO, @EMAILID, @WEBID,@APNM,@APNO,@STATUS, @USERPC, @INTIME , @UPDATETIME,@IPADDRESS ,@LOGINID,@LOGINPW )";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PartyID;
                cmd.Parameters.Add("@ADDRESS", SqlDbType.NVarChar).Value = ob.Address;
                cmd.Parameters.Add("@CONTACTNO", SqlDbType.NVarChar).Value = ob.Contact;
                cmd.Parameters.Add("@EMAILID", SqlDbType.NVarChar).Value = ob.Email;
                cmd.Parameters.Add("@WEBID", SqlDbType.NVarChar).Value = ob.Web;
                cmd.Parameters.Add("@APNM", SqlDbType.NVarChar).Value = ob.APName;
                cmd.Parameters.Add("@APNO", SqlDbType.NVarChar).Value = ob.APContact;
                cmd.Parameters.Add("@STATUS", SqlDbType.Char).Value = ob.Status;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;
                cmd.Parameters.Add("@LOGINID", SqlDbType.NVarChar).Value = ob.Logid;
                cmd.Parameters.Add("@LOGINPW", SqlDbType.NVarChar).Value = ob.Logpw;

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
        public DataTable ShowpartyInfo(string id)
        {
            DataTable table = new DataTable();
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select PARTYID  from CNF_PARTY where PARTYID='" + id + "' ";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            catch { }
            return table;
        }
        public string UpdateParty(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"update CNF_PARTY set ADDRESS=@ADDRESS, CONTACTNO=@CONTACTNO, EMAILID=@EMAILID, 
                             WEBID=@WEBID, APNM=@APNM, APNO=@APNO, STATUS=@STATUS, LOGINID=@LOGINID,LOGINPW=@LOGINPW 
                                     where PARTYID=@PARTYID";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PartyID;
                cmd.Parameters.Add("@ADDRESS", SqlDbType.NVarChar).Value = ob.Address;
                cmd.Parameters.Add("@CONTACTNO", SqlDbType.NVarChar).Value = ob.Contact;
                cmd.Parameters.Add("@EMAILID", SqlDbType.NVarChar).Value = ob.Email;
                cmd.Parameters.Add("@WEBID", SqlDbType.NVarChar).Value = ob.Web;
                cmd.Parameters.Add("@APNM", SqlDbType.NVarChar).Value = ob.APName;
                cmd.Parameters.Add("@APNO", SqlDbType.NVarChar).Value = ob.APContact;
                cmd.Parameters.Add("@STATUS", SqlDbType.Char).Value = ob.Status;
                cmd.Parameters.Add("@LOGINID", SqlDbType.Char).Value = ob.Logid;
                cmd.Parameters.Add("@LOGINPW", SqlDbType.Char).Value = ob.Logpw;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string SaveCommissionInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "insert into CNF_COMMISSION (PARTYID, REGID,JOBQUALITY,COMMSL, EXCTP, VALUEFR,VALUETO,VALUETP, COMMAMT, USERPC, INTIME, UPDATETIME, IPADDRESS)" +
                    "values(@PARTYID,@REGID,@JOBQUALITY, @COMMSL,@EXCTP, @VALUEFR,@VALUETO, @VALUETP, @COMMAMT, @USERPC, @INTIME , @UPDATETIME,@IPADDRESS  )";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;

                cmd.Parameters.Add("@REGID", SqlDbType.NVarChar).Value = ob.RegID;
                cmd.Parameters.Add("@JOBQUALITY", SqlDbType.NVarChar).Value = ob.JobQuality;
                cmd.Parameters.Add("@COMMSL", SqlDbType.BigInt).Value = ob.COMMSL;
                cmd.Parameters.Add("@EXCTP", SqlDbType.NVarChar).Value = ob.EXCTP;
                cmd.Parameters.Add("@VALUEFR", SqlDbType.Decimal).Value = ob.VALUEFROM;
                cmd.Parameters.Add("@VALUETO", SqlDbType.Decimal).Value = ob.VALUETO;
                cmd.Parameters.Add("@VALUETP", SqlDbType.NVarChar).Value = ob.VALUETP;
                cmd.Parameters.Add("@COMMAMT", SqlDbType.Decimal).Value = ob.COMMAMT;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string DeleteCommissionInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM CNF_COMMISSION WHERE PARTYID =@PARTYID AND COMMSL =@COMMSL AND REGID=@REGID AND JOBQUALITY=@JOBQUALITY";
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;
                cmd.Parameters.Add("@REGID", SqlDbType.NVarChar).Value = ob.RegID;
                cmd.Parameters.Add("@JOBQUALITY", SqlDbType.NVarChar).Value = ob.JobQuality;
                cmd.Parameters.Add("@COMMSL", SqlDbType.BigInt).Value = ob.COMMSL;

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

        public string UpdateCommissionInfo(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "UPDATE CNF_COMMISSION set EXCTP=@EXCTP ,VALUEFR=@VALUEFR, VALUETO=@VALUETO, VALUETP=@VALUETP, COMMAMT=@COMMAMT where PARTYID=@PARTYID and COMMSL=@COMMSL AND REGID=@REGID AND JOBQUALITY=@JOBQUALITY";

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PARTYID;
                cmd.Parameters.Add("@REGID", SqlDbType.NVarChar).Value = ob.RegID;
                cmd.Parameters.Add("@JOBQUALITY", SqlDbType.NVarChar).Value = ob.JobQuality;
                cmd.Parameters.Add("@COMMSL", SqlDbType.BigInt).Value = ob.COMMSL;
                cmd.Parameters.Add("@EXCTP", SqlDbType.NVarChar).Value = ob.EXCTP;
                cmd.Parameters.Add("@VALUEFR", SqlDbType.Decimal).Value = ob.VALUEFROM;
                cmd.Parameters.Add("@VALUETO", SqlDbType.Decimal).Value = ob.VALUETO;
                cmd.Parameters.Add("@VALUETP", SqlDbType.NVarChar).Value = ob.VALUETP;
                cmd.Parameters.Add("@COMMAMT", SqlDbType.Decimal).Value = ob.COMMAMT;

                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@INTIME", SqlDbType.SmallDateTime).Value = ob.InTime;
                cmd.Parameters.Add("@UPDATETIME", SqlDbType.SmallDateTime).Value = ob.UpdateTime;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.IPAddress;

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

        public string Insert_Expese_RT(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO CNF_PARTYRT (PARTYID,EXPID,REGID,RATETP,JOBQUALITY,RATE,REMARKS,USERPC,USERID,IPADDRESS) values (@PARTYID,@EXPID,@REGID,@RATETP,@JOBQUALITY,@RATE,@REMARKS,@USERPC,@USERID,@IPADDRESS)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PartyID;
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.ExpensesID;
                cmd.Parameters.Add("@REGID", SqlDbType.NVarChar).Value = ob.RegID;
                cmd.Parameters.Add("@JOBQUALITY", SqlDbType.NVarChar).Value = ob.JobQuality;
                cmd.Parameters.Add("@RATETP", SqlDbType.NVarChar).Value = ob.RateTP;
                cmd.Parameters.Add("@RATE", SqlDbType.Decimal).Value = ob.Rate;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@USERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = ob.UserNM;
                cmd.Parameters.Add("@IPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;
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

        public string UPDATE_CNF_PARTYRT(cnf_Interface ob)
        {
            string s = "";
            SqlTransaction tran = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                string EXPID = HttpContext.Current.Session["EXPID"].ToString();
                string RATETP = HttpContext.Current.Session["RATETP"].ToString();
                tran = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE CNF_PARTYRT SET  EXPID=@EXPID,RATETP=@RATETP,RATE=@RATE,REMARKS=@REMARKS,UPDUSERPC=@UPDUSERPC,UPDUSERID=@UPDUSERID,UPDIPADDRESS=@UPDIPADDRESS WHERE PARTYID=@PARTYID AND JOBQUALITY=@JOBQUALITY AND EXPID='" + EXPID + "' and REGID=@REGID and RATETP='" + RATETP + "'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@EXPID", SqlDbType.NVarChar).Value = ob.ExpensesID;
                cmd.Parameters.Add("@RATETP", SqlDbType.NVarChar).Value = ob.RateTP;
                cmd.Parameters.Add("@JOBQUALITY", SqlDbType.NVarChar).Value = ob.JobQuality;
                cmd.Parameters.Add("@REGID", SqlDbType.NVarChar).Value = ob.RegID;
                cmd.Parameters.Add("@PARTYID", SqlDbType.NVarChar).Value = ob.PartyID;
                cmd.Parameters.Add("@RATE", SqlDbType.Decimal).Value = ob.Rate;
                cmd.Parameters.Add("@REMARKS", SqlDbType.NVarChar).Value = ob.Remarks;
                cmd.Parameters.Add("@UPDUSERPC", SqlDbType.NVarChar).Value = ob.Userpc;
                cmd.Parameters.Add("@UPDUSERID", SqlDbType.NVarChar).Value = ob.UserNM;
                cmd.Parameters.Add("@UPDIPADDRESS", SqlDbType.NVarChar).Value = ob.Ipaddress;
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