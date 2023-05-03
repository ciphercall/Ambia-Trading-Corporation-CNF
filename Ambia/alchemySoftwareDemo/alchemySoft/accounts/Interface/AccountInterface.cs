using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace alchemy.accounts.Interface
{
    public class AccountInterface
    {
        private string transtp;

        public string Transtp
        {
            get { return transtp; }
            set { transtp = value; }
        }
        private DateTime transdt;

        public DateTime Transdt
        {
            get { return transdt; }
            set { transdt = value; }
        }
        private string monyear;

        public string Monyear
        {
            get { return monyear; }
            set { monyear = value; }
        }
        private int voucher;

        public int Voucher
        {
            get { return voucher; }
            set { voucher = value; }
        }
        private string transfor;

        public string Transfor
        {
            get { return transfor; }
            set { transfor = value; }
        }
        private string costpid;

        public string Costpid
        {
            get { return costpid; }
            set { costpid = value; }
        }
        private string transmode;

        public string Transmode
        {
            get { return transmode; }
            set { transmode = value; }
        }
        private string debitcd;

        public string Debitcd
        {
            get { return debitcd; }
            set { debitcd = value; }
        }
        private string creditcd;

        public string Creditcd
        {
            get { return creditcd; }
            set { creditcd = value; }
        }
        private string chequeno;

        public string Chequeno
        {
            get { return chequeno; }
            set { chequeno = value; }
        }
        private DateTime chequedt;

        public DateTime Chequedt
        {
            get { return chequedt; }
            set { chequedt = value; }
        }
        private string remarks;

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private string inword;

        public string Inword
        {
            get { return inword; }
            set { inword = value; }
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private decimal debitAmt;

        public decimal DebitAmt
        {
            get { return debitAmt; }
            set { debitAmt = value; }
        }
        private decimal creditAmt;

        public decimal CreditAmt
        {
            get { return creditAmt; }
            set { creditAmt = value; }
        }

        private string serialNo_MREC;

        public string SerialNo_MREC
        {
            get { return serialNo_MREC; }
            set { serialNo_MREC = value; }
        }

        private string serialNo_MPAY;

        public string SerialNo_MPAY
        {
            get { return serialNo_MPAY; }
            set { serialNo_MPAY = value; }
        }
        private string serialNo_JOUR;

        public string SerialNo_JOUR
        {
            get { return serialNo_JOUR; }
            set { serialNo_JOUR = value; }
        }

        private string serial_Cont;

        public string Serial_Cont
        {
            get { return serial_Cont; }
            set { serial_Cont = value; }
        }
        string transNo;

        public string TransNo
        {
            get { return transNo; }
            set { transNo = value; }
        }

        private string storeTo;

        public string StoreTo
        {
            get { return storeTo; }
            set { storeTo = value; }
        }

        private string storeFrom;

        public string StoreFrom
        {
            get { return storeFrom; }
            set { storeFrom = value; }
        }

        private string psID;

        public string PsID
        {
            get { return psID; }
            set { psID = value; }
        }

        private string serial_BUY;

        public string Serial_BUY
        {
            get { return serial_BUY; }
            set { serial_BUY = value; }
        }

        private string serial_SALE;

        public string Serial_SALE
        {
            get { return serial_SALE; }
            set { serial_SALE = value; }
        }

        private string sl_Sale_dis;

        public string Sl_Sale_dis
        {
            get { return sl_Sale_dis; }
            set { sl_Sale_dis = value; }
        }

        private string userpc;
        
        public string Userpc
        {
            get { return userpc; }
            set { userpc = value; }
        }

        private string ipaddress;

        public string Ipaddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }
        private DateTime inTM;

        public DateTime InTM
        {
            get { return inTM; }
            set { inTM = value; }
        }
        private string accCD;

        public string AccCD
        {
            get { return accCD; }
            set { accCD = value; }
        }
        private string accNM;

        public string AccNM
        {
            get { return accNM; }
            set { accNM = value; }
        }

        private DateTime opDT;

        public DateTime OpDT
        {
            get { return opDT; }
            set { opDT = value; }
        }

        private Int64 levelCD;

        public Int64 LevelCD
        {
            get { return levelCD; }
            set { levelCD = value; }
        }
        private string controlCD;

        public string ControlCD
        {
            get { return controlCD; }
            set { controlCD = value; }
        }

        private string accTP;

        public string AccTP
        {
            get { return accTP; }
            set { accTP = value; }
        }

        private string branchCD;

        public string BranchCD
        {
            get { return branchCD; }
            set { branchCD = value; }
        }

        private string statusCD;

        public string StatusCD
        {
            get { return statusCD; }
            set { statusCD = value; }
        }

        private DateTime insTime;

        public DateTime InsTime
        {
            get { return insTime; }
            set { insTime = value; }
        }

        private string apTitle;

        public string ApTitle
        {
            get { return apTitle; }
            set { apTitle = value; }
        }

        private string attnTo; 

        public string AttnTo
        {
            get { return attnTo; }
            set { attnTo = value; }
        }


        // Branch Entry

        public Int64 CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string BranchId { get; set; }
        public string BranchNm { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string Status { get; set; }

        // Branch Entry

        public Int64 UserId { get; set; }
      

        // Default Model Start //
        public Int64 UserIdInsert { get; set; }
        public string UserPcInsert { get; set; }
        public DateTime InTimeInsert { get; set; }
        public string IpAddressInsert { get; set; }
        public string LotiLengTudeInsert { get; set; }
        public Int64 UserIdUpdate { get; set; }
        public string UserPcUpdate { get; set; }
        public DateTime InTimeUpdate { get; set; }
        public string IpAddressUpdate { get; set; }
        public string LotiLengTudeUpdate { get; set; }
        public decimal Allowance { get; set; }
        public decimal AllowanceAmount { get; set; }

        // Default Model End //

        public string ip { get; set; }

        public string PcName { get; set; }

        public DateTime inTime { get; set; }

        public string userName { get; set; }

        public string tableid { get; set; }

        public string Serial_Do { get; set; }

        public string Serial_Mr { get; set; }

        public string Serial_Dnote { get; set; }

        public string Serial_Cnote { get; set; }

        public string Serial_CNote { get; set; }
    }
}