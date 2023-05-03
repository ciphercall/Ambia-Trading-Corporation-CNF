<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptChartofAcc.aspx.cs" Inherits="alchemySoft.accounts.Report.Report.rptChartofAcc" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" href="../../../Images/favicon.ico" />
    
    
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js" type="text/javascript"></script>

    <script type ="text/javascript">
        function ClosePrint() {
            var print = document.getElementById("print");
            print.style.visibility = "hidden";
//            print.display = false;

            window.print();
        }
    </script>

    <style media="print">

        .showHeader thead
         {
            display: table-header-group;
            border: 1px solid #000;
         }

    </style>

    <style type ="text/css">
        #main
        {
            float:left;
            border: 1px solid #cccccc;
        }
        .style1
        {
            font-size: 20px;
            font-family: Calibri;
            text-align: left;
        }
        .style4
        {
            width: 2510px;
        }
        #Button1
        {
            text-align: left;
        }
        .style5
        {
            width: 399px;
        }
        .style6
        {
            font-size: x-large;
            width: 27px;
        }
        .style7
        {
            width: 27px;
        }
        #btnPrint
        {
            font-weight: 700;
            font-style: italic;
        }
        .style8
        {
            text-align: center;
        }
        .style9
        {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
        <table style="width: 100%;">
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td class="style1" colspan="2">
                    <asp:Label ID="lblCompNM" runat="server" style="font-weight: 700"></asp:Label>
                </td>
                <td class="style5" style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right">
                    <input id="print" tabindex="1" type="button" value="Print" onclick = "ClosePrint()"/></td>
                <td style="text-align: right">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style7">
                    &nbsp;</td>
                <td colspan="2">
                    <asp:Label ID="lblAddress" runat="server" 
                        style="font-family: Calibri; font-size: 11px"></asp:Label>
                </td>
                <td class="style5">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style7">
                    &nbsp;</td>
                <td class="style9" colspan="2">
                    <strong style="text-align: left; font-family: Calibri; font-size: 16px;">CHART OF ACCOUNTS</strong></td>
                <td class="style5">
                    &nbsp;</td>
                <td style="text-align: right">
                    <asp:Label ID="lblTime" runat="server" Text="Label" 
                        style="font-family: Calibri; font-size: 16px"></asp:Label>
                </td>
                <td style="text-align: right">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style7">
                    &nbsp;</td>
                <td class="style8">
                    &nbsp;</td>
                <td class="style4">
                    &nbsp;</td>
                <td class="style5">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        <div class = "showHeader">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" 
                onrowdatabound="GridView1_RowDataBound" ShowHeaderWhenEmpty="True">
                <Columns>
                    <asp:BoundField HeaderText="ACCOUNT CODE">
                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ACCOUNT NAME">
                    <HeaderStyle HorizontalAlign="Center" Width="60%" />
                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="STATUS">
                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="CONTROL CODE">
                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle Font-Names="Calibri" Font-Size="16px" />
                <RowStyle Font-Names="Calibri" Font-Size="14px" />
            </asp:GridView>
         </div>
<%--            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:Alchemy_Acc%>" 
                SelectCommand="SELECT ACCOUNTCD AS [ACCOUNT CODE] , ACCOUNTNM AS [ACCOUNT NAME], STATUSCD + '       ' + CONVERT (NVARCHAR(10), LEVELCD, 103) AS STATUS, CONTROLCD AS [CONTROL CODE]   FROM GL_ACCHART">
            </asp:SqlDataSource>--%>
        </div>
    </div>
    </form>
</body>
</html>
