<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDdaFromAnother.aspx.cs" Inherits="DynamicMenu.Accounts.Report.Report.ReportDdaFromAnother" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="AlchemyAccounting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: Ledger History Yearly ::</title>
    <link href="../../../MenuCssJs/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        function ClosePrint() {
            var print = document.getElementById("print");
            print.style.visibility = "hidden";
            //            print.display = false;

            window.print();
        }
    </script>
    <style type="text/css">
        .panel-title {
            cursor: pointer;
        }

        .panel-body {
            padding-left: 0px;
            padding-right: 0px;
        }

        .panel-group {
            padding-left: 0px;
            padding-right: 0px;
        }

        .MyCssClass thead {
            display: table-header-group;
            border: 1px solid #000;
        }

        .container {
            /* to centre page on screen*/
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
        }

        @font-face {
            font-family: "Dodgv2";
            src: url("../../../MenuCssJs/fonts/Dodgv2.ttf")format("truetype");
            font-weight: normal;
            font-style: normal;
        }

        #btnPrint {
            font-weight: 700;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">




            <%  
                //string accountnm = Session["AccNM"].ToString();
                IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
                //string accountcd = Session["AccCode"].ToString();
                //string date = Session["From"].ToString();

                string accountnm = HttpContext.Current.Server.UrlDecode(Request.QueryString["AccNM"]);
                string accountcd = HttpContext.Current.Server.UrlDecode(Request.QueryString["AccCode"]);
                string date = HttpContext.Current.Server.UrlDecode(Request.QueryString["From"]);

                DateTime dateTime = DateTime.Parse(date, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
                string datesql = dateTime.ToString("yyyy/MM/dd");
                string yearSql = dateTime.ToString("yyyy");
            %>


            <table style="width: 100%;">
                <tr>
                    <td style="width: 50%; font-size: 16px; text-align: left;">
                        <asp:Label ID="lblCompNM" runat="server" Style="font-family: Dodgv2; font-size: 16px"></asp:Label>
                    </td>
                    <td style="width: 50%; font-size: 14px; text-align: right;">
                        <input id="print" tabindex="1" type="button" value="Print" onclick="ClosePrint()" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; font-size: 10px; text-align: left; font-family: Calibri;">
                        <asp:Label ID="lblAddress" runat="server"
                            Style="font-family: Calibri; font-size: 10px"></asp:Label>
                    </td>
                    <td style="width: 50%; font-size: 10px; text-align: right; font-family: Calibri;">
                        <asp:Label ID="lblTime" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                        <strong>Ledger History Yearly(<%=yearSql %>) Of <%=accountnm %></strong>
                    </td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                        <strong>AS ON DATE : &nbsp;<asp:Label ID="lblFrom" runat="server"></asp:Label>
                        </strong>
                    </td>
                </tr>

                <tr>
                    <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;"></td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                </tr>
            </table>
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 40%;">Month</td>
                                <td style="width: 20%; text-align: right">Opening</td>
                                <td style="width: 13%; text-align: right">Debit Tk.</td>
                                <td style="width: 13%; text-align: right">Credit Tk.</td>
                                <td style="width: 14%; text-align: right">Closing</td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="panel-body" style="padding: 0px">






                    <%


                        SqlConnection con = new SqlConnection(DbFunctions.Connection);
                        string query1 = string.Format(@"SELECT DISTINCT C.DEBITCD, FIRSTDT, LASTDT, YR, LASTMY, ISNULL(SUM(D.DEBITAMT)-SUM(D.CREDITAMT),0) OPAMT, C.DRAMT, C.CRAMT, C.AMT
                        FROM (
                        SELECT DISTINCT B.DEBITCD, DATEADD(month, DATEDIFF(month, 0,LASTDT), 0) FIRSTDT, LASTDT,  UPPER(CONVERT(varchar(3),LASTDT,7))+'-'+CONVERT(varchar(2),LASTDT,2) YR,
                        UPPER(CONVERT(varchar(3),LASTDT,7))+'-'+CONVERT(varchar(2),LASTDT,2) LASTMY, A.DEBITAMT DRAMT, A.CREDITAMT CRAMT, 
                        SUM(B.DEBITAMT)-SUM(B.CREDITAMT) AMT
                        FROM (
                        SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,TRANSDT)+1,0)) LASTDT, DEBITCD, SUM(DEBITAMT) DEBITAMT, SUM(CREDITAMT) CREDITAMT
                        FROM GL_MASTER 
                        WHERE DEBITCD='{0}' AND right(CONVERT(varchar(11),TRANSDT),4) = '{2}' AND TRANSDT <= '{1}'
                        GROUP BY DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,TRANSDT)+1,0)), DEBITCD
                        ) A, GL_MASTER AS B
                        WHERE B.TRANSDT <= A.LASTDT AND B.DEBITCD = '{0}' AND B.TRANSDT <= '{1}'
                        GROUP BY B.DEBITCD, LASTDT, UPPER(CONVERT(varchar(3),LASTDT,7))+CONVERT(varchar(2),LASTDT,2), 
                        UPPER(CONVERT(varchar(3),LASTDT,7))+'-'+CONVERT(varchar(2),LASTDT,2), A.DEBITAMT, A.CREDITAMT
                        )  C LEFT OUTER JOIN GL_MASTER AS D
                        ON D.TRANSDT < C.FIRSTDT AND C.DEBITCD = D.DEBITCD AND C.DEBITCD = '{0}' AND D.TRANSDT <= '{1}'
                        GROUP BY C.DEBITCD, FIRSTDT, LASTDT, YR, LASTMY, C.DRAMT, C.CRAMT, C.AMT", accountcd, datesql, yearSql);
                         if (con.State != System.Data.ConnectionState.Open)con.Open();
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        SqlDataReader dr1 = cmd1.ExecuteReader();

                        decimal debitTotalAmt = 0;
                        decimal creditTotalAmt = 0;
                        var openingbalancing = "0.00";
                        int i = 0;
                        while (dr1.Read())
                        {
                            var month = dr1["YR"].ToString();
                            var openingbl = dr1["OPAMT"].ToString();
                            var debitBalance = dr1["DRAMT"].ToString();
                            var creditBalance = dr1["CRAMT"].ToString();
                            var amount = dr1["AMT"].ToString();

                            debitTotalAmt += Convert.ToDecimal(debitBalance);
                            creditTotalAmt += Convert.ToDecimal(creditBalance);

                            var opening = dbFunctions.StringData(@"SELECT ISNULL(SUM(DEBITAMT)-SUM(CREDITAMT),0) OPAMT
                                        FROM   GL_MASTER
                                        WHERE  DEBITCD = '" + accountcd + "' AND TRANSDT < '01-'+'" + month + "'");

                            if (i == 0)
                                openingbalancing = opening;

                    %>






                    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true" style="padding-top: 10px; margin-bottom: 5px">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading<%=i %>">
                                <h4 class="panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse<%=i %>" aria-expanded="true" aria-controls="collapse<%=i %>">
                                    <a style="text-decoration: none">

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 40%; text-align: left"><%=month %></td>
                                                <td style="width: 20%; text-align: right;"><%=SpellAmount.comma(Convert.ToDecimal(openingbl)) %></td>
                                                <td style="width: 13%; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(debitBalance)) %></td>
                                                <td style="width: 13%; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(creditBalance)) %></td>
                                                <td style="width: 14%; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(amount)) %></td>
                                            </tr>
                                        </table>

                                    </a>
                                </h4>
                            </div>
                            <div id="collapse<%=i %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading<%=i %>">
                                <div class="panel-body">
                                    <table style="width: 100%" class="table table-hover">
                                        <tr>
                                            <td style="width: 10%; background: #819FF7">Date</td>
                                            <td style="width: 10%; background: #819FF7">Voucher No</td>
                                            <td style="width: 40%; background: #819FF7">Particulars</td>
                                            <td style="width: 13%; background: #819FF7; text-align: right">Debit(Tk.)</td>
                                            <td style="width: 13%; background: #819FF7; text-align: right">Credit(Tk.)</td>
                                            <td style="width: 14%; background: #819FF7; text-align: right">Balance</td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="background: #E0E6F8"></td>
                                            <td colspan="2" style="background: #E0E6F8; text-align: right">Opening Balance</td>
                                            <td style="background: #E0E6F8; width: 14%; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(opening)) %></td>
                                        </tr>

                                        <%
                                            SqlCommand cmd2 = new SqlCommand(@"SELECT A.TRANSNO, REPLACE(CONVERT(VARCHAR(11),TRANSDT,106), ' ','-') TRANSDT, TRANSTP, 
                                                CASE WHEN A.TABLEID='GL_STRANS' THEN (CASE WHEN TRANSTP = 'MREC' 
                                                THEN 'SRV' WHEN TRANSTP = 'MPAY' THEN 'SPV' WHEN TRANSTP = 'CONT' THEN 'SCV' WHEN TRANSTP = 'JOUR' THEN 'SJV'  ELSE '' END) 
                                                WHEN A.TABLEID='GL_MTRANS' THEN (CASE WHEN TRANSTP = 'MREC' 
                                                THEN 'MRV' WHEN TRANSTP = 'MPAY' THEN 'MPV' WHEN TRANSTP = 'CONT' THEN 'MCV' WHEN TRANSTP = 'JOUR' THEN 'MJV'  ELSE '' END) 
                                                ELSE 
                                                (CASE WHEN TRANSTP = 'MREC' 
                                                THEN 'ARV' WHEN TRANSTP = 'MPAY' THEN 'APV' WHEN TRANSTP = 'CONT' THEN 'ACV' WHEN TRANSTP = 'JOUR' THEN 'AJV' ELSE '' END) END AS
                                                TP,
                                                CREDITNM, ISNULL(DEBITAMT,0) DEBITAMT, ISNULL(CREDITAMT,0) CREDITAMT, ISNULL(BALAMT,0) BALAMT
                                                FROM(
                                                SELECT TABLEID, TRANSNO, TRANSDT, TRANSTP, ACCOUNTNM CREDITNM, ISNULL(DEBITAMT,0) DEBITAMT, 
                                                ISNULL(CREDITAMT,0) CREDITAMT, (ISNULL(DEBITAMT,0) - ISNULL(CREDITAMT,0)) BALAMT
                                                FROM GL_MASTER AS A INNER JOIN GL_ACCHART AS B ON A.CREDITCD = B.ACCOUNTCD
                                                WHERE A.DEBITCD = '" + accountcd + "' AND UPPER(TRANSMY)='" + month + "' ) A ORDER BY TRANSDT, DEBITAMT DESC, TRANSTP DESC, TRANSNO", con);

                                            SqlDataReader dr2 = cmd2.ExecuteReader();

                                            decimal totaldr = 0;
                                            decimal totalcr = 0;
                                            decimal totalamt = 0;
                                            int j = 0;
                                            decimal balancingAmt = 0;
                                            while (dr2.Read())
                                            {
                                                string transdt = dr2["TRANSDT"].ToString();
                                                string transno = dr2["TRANSNO"].ToString();
                                                string transtp = dr2["TP"].ToString();
                                                string transtyperpt = dr2["TRANSTP"].ToString();
                                                string perticular = dr2["CREDITNM"].ToString();
                                                string drbalance = dr2["DEBITAMT"].ToString();
                                                string crbalance = dr2["CREDITAMT"].ToString();
                                                string amt = dr2["BALAMT"].ToString();

                                                totaldr += Convert.ToDecimal(drbalance);
                                                totalcr += Convert.ToDecimal(crbalance);
                                                totalamt += Convert.ToDecimal(amt);

                                                if (j == 0)
                                                {
                                                    balancingAmt = (Convert.ToDecimal(opening) + Convert.ToDecimal(drbalance) - Convert.ToDecimal(crbalance));
                                                }
                                                else
                                                {
                                                    balancingAmt = (Convert.ToDecimal(balancingAmt) + Convert.ToDecimal(drbalance) - Convert.ToDecimal(crbalance));
                                                }
                                                j++;
                                        %>

                                        <tr>
                                            <td style="width: 10%"><%=transdt %></td>

                                            <%if (transtp.Substring(0, 1) == "S")
                                                { %>
                                            <td style="width: 10%; text-decoration: none">
                                                <a href="rptSTransVoucherDrilldown.aspx?transmy=<%=month %>&transtp=<%=transtyperpt %>&voucherno=<%=transno %>&date=<%=transdt %>" target="_blank">
                                                    <%=transtp %>-<%=transno %>
                                                </a>
                                            </td>
                                            <%} %>

                                            <%else if (transtp.Substring(0, 1) == "M")
                                                { %>
                                            <td style="width: 10%; text-decoration: none">
                                                <a href="rptMTransVoucherDrilldown.aspx?transmy=<%=month %>&transtp=<%=transtyperpt %>&voucherno=<%=transno %>&date=<%=transdt %>" target="_blank">
                                                    <%=transtp %>-<%=transno %>
                                                </a>
                                            </td>
                                            <%} %>

                                            <%else
                                                { %>
                                            <td style="width: 10%; text-decoration: none">
                                                <%=transtp %>-<%=transno %>
                                            </td>
                                            <%} %>

                                            <td style="width: 40%"><%=perticular %></td>
                                            <td style="width: 13%; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(drbalance)) %></td>
                                            <td style="width: 13%; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(crbalance)) %></td>
                                            <td style="width: 14%; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(balancingAmt)) %></td>
                                        </tr>

                                        <% }
                                            dr2.Close();
                                        %>

                                        <tr>
                                            <td colspan="3" style="background: #ddd; text-align: right">Total :</td>
                                            <td style="width: 13%; background: #ddd; text-align: right"><%=SpellAmount.comma(totaldr) %></td>
                                            <td style="width: 13%; background: #ddd; text-align: right"><%=SpellAmount.comma(totalcr) %></td>
                                            <td style="width: 14%; background: #ddd; text-align: right"><%=SpellAmount.comma(Convert.ToDecimal(opening)+Convert.ToDecimal(totaldr)-Convert.ToDecimal(totalcr)) %></td>
                                        </tr>

                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>


                    <%
                            i++;
                        }
                        dr1.Close();
                        if (con.State != System.Data.ConnectionState.Closed)con.Close();
                    %>






                    <table style="width: 100%; font-size: 16px" class="table table-hover">
                        <tr>
                            <td style="width: 60%; background: #E6E6E6; text-align: right; font-weight: 700;">Total :</td>
                            <td style="width: 13%; background: #E6E6E6; text-align: right; font-weight: 700; padding-right: 6px;"><%=SpellAmount.comma(debitTotalAmt) %></td>
                            <td style="width: 13%; background: #E6E6E6; text-align: right; font-weight: 700; padding-right: 11px;"><%=SpellAmount.comma(creditTotalAmt) %></td>
                            <td style="width: 14%; background: #E6E6E6; text-align: right; font-weight: 700; padding-right: 16px;"><%=SpellAmount.comma(Convert.ToDecimal(openingbalancing)+Convert.ToDecimal(debitTotalAmt)-Convert.ToDecimal(creditTotalAmt)) %></td>
                        </tr>
                    </table>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
