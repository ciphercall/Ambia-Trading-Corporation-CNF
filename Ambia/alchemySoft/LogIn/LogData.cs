using alchemySoft.LogIn.DataAccess;
using alchemySoft.LogIn.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace alchemySoft.LogIn
{
    public class LogData
    { 
        public static void InsertLogData(string lotiLengtude, string logType, string tableId, string logData, string ipAddress)
        {
            try
            {
                HttpCookie CookiesData = HttpContext.Current.Request.Cookies["UserInfo"];//Output Data 
                LogInDataAccess dob = new LogInDataAccess();
                LogInInterface iob = new LogInInterface();
                iob.LotiLengTudeInsert = lotiLengtude;
                iob.ipAddressInsert = dbFunctions.ipAddress();
                iob.UserIdInsert = Convert.ToInt64(CookiesData["USERID"].ToString());
                iob.userPcInsert = dbFunctions.userPc();
                iob.InTimeInsert = dbFunctions.timezone(DateTime.Now);

                string logSl = dbFunctions.getData("SELECT ISNULL(MAX(LOGSLNO+1),1) AS MAXLOGSLNO FROM ASL_LOG WHERE USERID= " + iob.UserIdInsert + "");
                iob.LogSlNo = Convert.ToInt64(logSl);
                iob.LogType = logType;
                iob.CompanyId = Convert.ToInt64(CookiesData["COMPANYID"].ToString());
                iob.CompanyUserId = Convert.ToInt64(CookiesData["USERID"].ToString());
                iob.TableId = tableId;
                iob.LogDatA = logData;
                dob.INSERT_ASL_LOG(iob);
            }
            catch (Exception)
            {

            }

        }
        
    }
}