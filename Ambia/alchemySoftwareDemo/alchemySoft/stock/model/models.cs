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
    }
}