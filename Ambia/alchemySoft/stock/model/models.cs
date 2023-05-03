using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace alchemySoft.stock.model
{
    public class models
    {
        internal string itemID;
        internal string itemNM;
        internal string color;
        internal decimal buyRT;
        internal decimal saleRT;
        internal int minStk;
        internal string itemCD;
        internal string remarks;
        internal string fabric;
        internal int qty;
        internal string style;
        internal decimal wpcnt;
        internal string yarn;
        internal decimal ttlPx;
        internal string transTP;
        internal DateTime transDT;
        internal string transMY;
        internal string transno;
        internal string jobNo;
        internal string psID;
        internal string remarksMst;
        internal string transSL;
        internal string storeFrID;
        internal string storeToID;
        internal decimal rate;
        internal decimal amount;
        internal string batch;
        internal string lot;
        internal int lotQty;
        internal string unitTP;
        internal string sl;
        internal string machine;
        internal string transMD;

        public string catID { get; internal set; }
        public string userID { get; internal set; }
        public long CompanyId { get; internal set; }
        public long CompanyUserId { get; internal set; }
        public string ImgUrl { get; internal set; }
        public object InTimeInsert { get; internal set; }
        public string IpAddressInsert { get; internal set; }
        public string LatiLongTudeInsert { get; internal set; }
        public object LogDatA { get; internal set; }
        public long LogSlNo { get; internal set; }
        public string LogType { get; internal set; }
        public string LotiLengTudeInsert { get; internal set; }
        public string TableId { get; internal set; }
        public long UserIdInsert { get; internal set; }
        public object UserPcInsert { get; internal set; }
        public string catName { get; internal set; }
        public string ipaddress { get; internal set; }
        public DateTime intime { get; internal set; }
        public string userPC { get; internal set; }
        public string Pstp { get; internal set; }
        public string Pscd { get; internal set; }
        public string City { get; internal set; }
        public string Address { get; internal set; }
        public string Contactno { get; internal set; }
        public string Email { get; internal set; }
        public string Webid { get; internal set; }
        public string Cpnm { get; internal set; }
        public string Cpno { get; internal set; }
        public string Status { get; internal set; }
        public string Username { get; internal set; }
        public string Ps_ID { get; internal set; }
    }
}