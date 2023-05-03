<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptReceiptPaymentState.aspx.cs" Inherits="alchemySoft.accounts.Report.Report.rptReceiptPaymentState" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" href="../../../Images/favicon.ico" />


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

        .style1 {
            font-size: small;
            text-align: center;
            width: 908px;
        }

        .style2 {
            font-size: medium;
            font-family: Calibri;
        }

        .SubTotalRowStyle {
            border: solid 1px Black;
            font-weight: bold;
            text-align: right;
            font-family: Calibri;
            font-size: 14px;
            height: 25px;
        }

        .GrandTotalRowStyle {
            border: solid 1px Gray;
            color: #000000;
            font-weight: bold;
            font-size: 16px;
            text-align: right;
            height: 28px;
        }

        .GroupHeaderStyle {
            border: solid 1px Black;
            text-align: left;
            color: #000000;
            height: 28px;
            text-decoration: underline;
            font-size: 15px;
            font-weight: bold;
        }

        .GridRowStyle {
            padding-left: 10%;
        }

        .style8 {
            text-align: center;
            width: 908px;
        }

        .style10 {
            width: 142px;
        }

        .style11 {
            font-size: medium;
            text-align: center;
            width: 908px;
            font-family: Calibri;
        }

        .GrandGrandTotalRowStyle {
            border: solid 1px Gray;
            color: #000000;
            font-weight: bold;
            font-size: 16px;
            text-align: right;
            height: 30px;
            font-family: Calibri;
        }

        .style12 {
            font-family: Calibri;
        }

        .style13 {
            font-size: medium;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="main">
            <div>
                <table style="width: 100%; margin: 1% 0% 0% 0%;">
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
                        <td colspan="2" style="width: 100%; font-size: 14px; text-align: left; font-family: Calibri;">
                            <strong>RECEIPTS & PAYMENT STATEMENT - CONSOLIDATED</strong>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                            <strong>PERIOD  &nbsp;<asp:Label ID="lblFrom" runat="server"></asp:Label>
                                &nbsp;TO&nbsp; 
                    <asp:Label ID="lblTo" runat="server"></asp:Label></strong>
                        </td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                            <strong></strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;"></td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                    </tr>
                </table>


                <table style="width: 100%; margin: 0% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblOpening" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="MyCssClass" style="width: 100%; margin: 0% 0% 1% 0%;">

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        OnRowCreated="GridView1_RowCreated"
                        OnRowDataBound="GridView1_RowDataBound" Width="100%"
                        ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Particulars">
                                <HeaderStyle HorizontalAlign="Center" Width="60%" />
                                <ItemStyle Width="60%" HorizontalAlign="Left" CssClass="GridRowStyle" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

                <table style="width: 100%; margin: 0% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblReceive" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="MyCssClass" style="width: 100%; margin: 0% 0% 1% 0%;">

                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                        OnRowCreated="GridView2_RowCreated"
                        OnRowDataBound="GridView2_RowDataBound" Width="100%"
                        ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Particulars">
                                <HeaderStyle HorizontalAlign="Center" Width="60%" />
                                <ItemStyle Width="60%" CssClass="GridRowStyle" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

                <table style="width: 100%; margin: 0% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblPayment" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="MyCssClass" style="width: 100%; margin: 0% 0% 1% 0%;">

                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"
                        OnRowCreated="GridView3_RowCreated"
                        OnRowDataBound="GridView3_RowDataBound" Width="100%"
                        ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Particulars">
                                <HeaderStyle HorizontalAlign="Center" Width="60%" />
                                <ItemStyle Width="60%" CssClass="GridRowStyle" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

                <table style="width: 100%; margin: 0% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblClosing" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="MyCssClass" style="width: 100%; margin: 0% 0% 1% 0%;">

                    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False"
                        OnRowCreated="GridView4_RowCreated"
                        OnRowDataBound="GridView4_RowDataBound" Width="100%"
                        ShowHeaderWhenEmpty="True" ShowFooter="True">
                        <Columns>
                            <asp:BoundField HeaderText="Particulars">
                                <HeaderStyle HorizontalAlign="Center" Width="60%" />
                                <ItemStyle Width="60%" CssClass="GridRowStyle" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle Width="35px" CssClass="GrandGrandTotalRowStyle" />
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

            </div>
        </div>
    </form>
</body>
</html>
