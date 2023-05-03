using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace alchemySoft.LogIn.Interface
{
    public class LogInInterface
    {
        // Default Model Start //
        public Int64 UserIdInsert { get; set; }
        public string userPcInsert { get; set; }
        public DateTime InTimeInsert { get; set; }
        public string ipAddressInsert { get; set; }
        public string LotiLengTudeInsert { get; set; }
        public Int64 UserIdUpdate { get; set; }
        public string userPcUpdate { get; set; }
        public DateTime InTimeUpdate { get; set; }
        public string ipAddressUpdate { get; set; }
        public string LotiLengTudeUpdate { get; set; }

        // Default Model End //


        public Int64 UserID { get; set; }
        public string Password { get; set; }
        public object CompanyId { get; internal set; }
        public object CompanyUserId { get; internal set; }
        public object LogType { get; internal set; }
        public object LogSlNo { get; internal set; }
        public object TableId { get; internal set; }
        public object LogDatA { get; internal set; }
    }
}