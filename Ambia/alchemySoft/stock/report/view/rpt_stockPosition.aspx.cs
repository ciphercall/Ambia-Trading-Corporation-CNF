using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.stock.report.view
{
    public partial class rpt_stockPosition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            dbFunctions.lblAdd(@"SELECT COMPNM FROM ASL_COMPANY WHERE COMPID ='101'", lblCompNM);
            dbFunctions.lblAdd(@"SELECT ADDRESS FROM ASL_COMPANY WHERE COMPID ='101'", lblAddress);

            string Fr = Session["FRDT_!"].ToString();
            string To = Session["TODT_!"].ToString();
            lblDate.Text = "FROM: " + Fr + " TO: " + To;
            string store = Session["STORENM_!"].ToString();
            string storeID = Session["STORENM_!"].ToString();
            string storeFr = "", storeTo = "";
            lblStoreNM.Text = store;
            if(store!="ALL")
            { 
                storeFr = " AND STOREFR = '"+ storeID + "' ";
                storeTo = " AND STORETO = '" + storeID + "'";
            }
            string FdT = dbFunctions.dateConvertYMD(Fr);
            string TdT = dbFunctions.dateConvertYMD(To);

            dbFunctions.gridViewAdd(GridView1, @"
SELECT STK_ITEM.ITEMNM,OPENING,RCVQTY,OPENING+RCVQTY STOCK,ISUQTY,(OPENING+RCVQTY)-ISUQTY BLNC FROM (
SELECT ITEMID,STORE,
(
SELECT B.CLQTY FROM (SELECT ITEMID, (SUM(isnull(RECQTY,0)) + SUM(isnull(BQTY,0))) - (SUM(isnull(ISUQTY,0)) + SUM(isnull(SQTY,0))) AS CLQTY  FROM (
SELECT ITEMID, SUM(isnull(QTY,0)) AS BQTY, SUM(isnull(AMOUNT,0)) AS BAMT, 0 AS SQTY, 0 AS SAMT, 0 AS RECQTY, 0 AS ISUQTY FROM STK_TRANS 
WHERE (TRANSDT <= '" + FdT + "') AND (TRANSTP = 'BUY') AND (STORETO = A.STORE)AND (ITEMID=A.ITEMID) " +
"GROUP BY ITEMID " +
"UNION " +
"SELECT ITEMID, 0 AS BQTY, 0 AS BAMT, SUM(isnull(QTY, 0)) AS SQTY, SUM(isnull(AMOUNT, 0)) AS SAMT, 0 AS RECQTY, 0 AS ISUQTY FROM STK_TRANS AS STK_TRANS_1 " +
"WHERE(TRANSDT <= '" + FdT + "') AND(TRANSTP = 'SALE') AND(STOREFR = A.STORE)AND ITEMID = A.ITEMID GROUP BY ITEMID " +
"UNION " +
"SELECT ITEMID, 0 AS BQTY, 0 AS BAMT, 0 AS SQTY, 0 AS SAMT, SUM(isnull(QTY, 0)) AS RECQTY, 0 AS ISUQTY FROM STK_TRANS AS STK_TRANS_2 " +
"WHERE(TRANSDT <= '" + FdT + "') AND(TRANSTP = 'IREC') AND(STORETO = A.STORE)AND(ITEMID = A.ITEMID) GROUP BY ITEMID " +
"UNION " +
"SELECT ITEMID, 0 AS BQTY, 0 AS BAMT, 0 AS SQTY, 0 AS SAMT, 0 AS RECQTY, SUM(isnull(QTY, 0)) AS ISUQTY FROM STK_TRANS AS STK_TRANS_1 " +
"WHERE(TRANSDT <= '" + FdT + "') AND(TRANSTP = 'IISS') AND(STOREFR = A.STORE)AND(ITEMID = A.ITEMID) GROUP BY ITEMID) AS A " +
"GROUP BY ITEMID) AS B INNER JOIN STK_ITEM ON B.ITEMID = STK_ITEM.ITEMID WHERE(B.CLQTY <> 0) " +
") OPENING,RCVQTY,ISUQTY FROM (" +
"SELECT ITEMID, STORETO STORE, ISNULL(SUM(QTY), 0) RCVQTY,0 ISUQTY FROM STK_TRANS WHERE TRANSDT BETWEEN '" + FdT + "' AND '" + TdT + "' " + storeTo + " " +
"GROUP BY ITEMID, STORETO " +
"UNION " +
"SELECT ITEMID, STOREFR STORE ,0 RCVQTY,ISNULL(SUM(QTY), 0) ISUQTY FROM STK_TRANS WHERE TRANSDT BETWEEN '"+ FdT + "' AND '"+ TdT + "' " + storeFr + "  " +
"GROUP BY ITEMID, STOREFR " +
") A ) B INNER JOIN STK_ITEM ON B.ITEMID = STK_ITEM.ITEMID");
        }
    }
}