<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt-Money-receipts.aspx.cs" Inherits="DynamicMenu.Accounts.Report.Report.rpt_Money_receipts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Money Receipts Report</title>


    <script src="../../../MenuCssJs/js/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ClosePrint() {
            var print = document.getElementById("print");
            print.style.visibility = "hidden";
            //            print.display = false;

            window.print();
        }
    </script>

    <style type="text/css">
        .MyCssClass thead {
            display: table-header-group;
            border: 1px solid #000;
        }

        #main {
            width: 620px;
            /* to centre page on screen*/
            margin-left: auto;
            margin-right: auto;
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

        .GrandGrandTotalRowStyle {
            border: solid 1px Gray;
            color: #000000;
            font-weight: bold;
            font-size: 16px;
            font-family: Calibri;
            text-align: right;
            height: 30px;
        }

        .style1 {
            text-align: center;
            width: 922px;
            font-family: Calibri;
        }

        .style2 {
            font-size: 16px;
        }

        .style12 {
            width: 109px;
        }

        .style14 {
            text-align: center;
            width: 922px;
            font-family: Calibri;
            font-size: 20px;
            font-weight: 700;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="main">
            <div>
                <table style="width: 100%; margin: 0% 0% 0% 0%;">
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
                            <strong>MONEY RECEIPTS</strong>
                        </td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                            <strong>FROM : &nbsp; 
                                <asp:Label ID="lblfdt" runat="server"></asp:Label>
                             &nbsp;  &nbsp;  &nbsp;  &nbsp; TO : &nbsp; 
                                <asp:Label ID="lbltdt" runat="server"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;"></td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                    </tr>
                </table>



                <div class="MyCssClass" style="width: 100%; margin: 0% 0% 0% 0%;">

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" OnRowDataBound="GridView1_RowDataBound" ShowFooter="True"
                        ShowHeaderWhenEmpty="True" Width="100%" OnRowCreated="GridView1_RowCreated">
                        <Columns>
                            <asp:BoundField HeaderText="PARTICULARS">
                                <HeaderStyle HorizontalAlign="Center" Width="1.4%" />
                                <ItemStyle HorizontalAlign="Left" Width="1.4%" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="RECEIVE TYPE">
                                <HeaderStyle HorizontalAlign="Center" Width="1%" />
                                <ItemStyle HorizontalAlign="Center" Width="1%" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="REMARKS">
                                <HeaderStyle HorizontalAlign="Center" Width="0.8%" />
                                <ItemStyle HorizontalAlign="Left" Width="0.8%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="AMOUNT">
                                <HeaderStyle HorizontalAlign="Center" Width="0.1%" />
                                <ItemStyle HorizontalAlign="Right" Width="0.1%" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle CssClass="GrandGrandTotalRowStyle" Font-Names="Calibri" Font-Size="14px" />
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Names="Calibri" Font-Size="12px" />
                    </asp:GridView>

                </div>

            </div>
        </div>
    </form>
</body>
</html>
