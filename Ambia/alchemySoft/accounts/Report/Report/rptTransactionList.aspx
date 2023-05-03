<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptTransactionList.aspx.cs" Inherits="alchemySoft.accounts.Report.Report.rptTransactionList" %>

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
        .showHeader thead {
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
        }

        .style2 {
            font-size: medium;
        }

        .SubTotalRowStyle {
            border: solid 1px Black;
            font-weight: bold;
            text-align: right;
        }

        .GrandTotalRowStyle {
            border: solid 1px Gray;
            color: #000000;
            font-weight: bold;
            font-size: 18px;
            text-align: right;
            height: 35px;
        }

        .GroupHeaderStyle {
            border: solid 1px Black;
            text-align: left;
            color: #000000;
            font-weight: bold;
            height: 30px;
        }

        .GridRowStyle {
        }

        .style3 {
            font-family: Calibri;
        }

        .style8 {
            font-family: Calibri;
            font-size: medium;
        }

        #print {
            font-family: Calibri;
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
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                            <strong>TRANSACTION LISTING</strong>
                        </td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                            <strong>PERIOD  &nbsp;<asp:Label ID="lblFrom" runat="server"></asp:Label>
                                &nbsp;TO&nbsp; 
                    <asp:Label ID="lblTo" runat="server"></asp:Label></strong>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;"></td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                    </tr>
                </table>


                <div style="width: 100%; height: 1px; background: #000000;">
                </div>
                <table style="width: 100%; margin: 1% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblCreditHD" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="showHeader" style="width: 100%; margin: 0% 0% 0% 0%;">

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="GridView1_RowDataBound" Width="100%" ShowFooter="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle Width="10%" CssClass="GridRowStyle" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="V. No">
                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Branch">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Amount">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle Font-Names="Calibri" Font-Size="14px" />
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

                <table style="width: 100%; margin: 1% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblDebitHD" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="showHeader" style="width: 100%; margin: 0% 0% 0% 0%;">

                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="GridView2_RowDataBound" Width="100%" ShowFooter="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle Width="10%" CssClass="GridRowStyle" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="V. No">
                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Branch">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Amount">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle Font-Names="Calibri" Font-Size="14px" />
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

                <table style="width: 100%; margin: 1% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblJournalHD" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="showHeader" style="width: 100%; margin: 0% 0% 0% 0%;">

                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="GridView3_RowDataBound" Width="100%" ShowFooter="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle Width="10%" CssClass="GridRowStyle" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="V. No">
                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Branch">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Amount">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle Font-Names="Calibri" Font-Size="14px" />
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

                <table style="width: 100%; margin: 1% 0% 0% 0%;">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="lblContraHD" runat="server"
                                    Style="font-family: Calibri; font-size: 14px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div class="showHeader" style="width: 100%; margin: 0% 0% 0% 0%;">

                    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="GridView4_RowDataBound" Width="100%" ShowFooter="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle Width="10%" CssClass="GridRowStyle" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="V. No">
                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Branch">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Head">
                                <HeaderStyle HorizontalAlign="Center" Width="28%" />
                                <ItemStyle HorizontalAlign="Left" Width="28%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Amount">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle Font-Names="Calibri" Font-Size="14px" />
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="12px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
