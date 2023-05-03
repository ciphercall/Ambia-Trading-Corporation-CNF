<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt-costpool-wise-transaction.aspx.cs" Inherits="DynamicMenu.Accounts.Report.Report.rpt_costpool_wise_transaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CostPool Wise Transaction - Details</title>
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
            font-size: x-large;
            text-align: center;
            width: 908px;
            font-weight: 700;
            font-family: Calibri;
        }

        .style2 {
            font-size: medium;
            font-family: Calibri;
            font-weight: 700;
        }

        .SubTotalRowStyle {
            border: solid 2px Black;
            text-align: right;
        }

        .GrandTotalRowStyle {
            border: solid 1px Gray;
            color: #000000;
            font-weight: bold;
            font-size: 15px;
            text-align: right;
        }

        .GroupHeaderStyle {
            border: solid 1px Black;
            text-align: left;
            color: #000000;
            height: 30px;
            text-decoration: underline;
            font-weight: bold;
        }

        .GridRowStyle {
            padding-left: 3%;
        }

        .style8 {
            text-align: center;
            width: 908px;
        }

        .style10 {
            width: 142px;
        }

        .style11 {
            font-size: large;
            text-align: center;
            width: 908px;
            font-family: Calibri;
        }

        .GrandGrandTotalRowStyle {
            border: solid 1px Gray;
            color: #000000;
            font-weight: bold;
            font-size: 18px;
            text-align: right;
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
                            <strong>COSTPOOL WISE TRANSACTION - DETAILS</strong>
                        </td>

                        <td style="width: 50%; font-size: 14px; text-align: right; text-transform: uppercase; font-family: Calibri;"></td>
                    </tr>
                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                            <strong>OFFICE :&nbsp;
                                        <asp:Label ID="lblCostpool" runat="server"></asp:Label>
                            </strong>
                        </td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                            <strong>PERIOD  &nbsp;<asp:Label ID="lblFrom" runat="server"></asp:Label>
                                &nbsp;TO&nbsp; 
                    <asp:Label ID="lblTo" runat="server"></asp:Label></strong>
                        </td>
                    </tr>
                </table>

                <div class="MyCssClass" style="width: 100%; margin: 1% 0% 0% 0%;">

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        OnRowCreated="GridView1_RowCreated"
                        OnRowDataBound="GridView1_RowDataBound" Width="100%"
                        ShowHeaderWhenEmpty="True" ShowFooter="True">
                        <Columns>
                            <asp:BoundField HeaderText="Type">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="V. No">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit Name">
                                <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                <ItemStyle Width="25%" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Name">
                                <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                <ItemStyle Width="25%" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Remarks">
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                <ItemStyle Width="15%" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Amount">
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle Font-Bold="True" Font-Names="Calibri" Font-Size="14px" />
                        <HeaderStyle Font-Bold="True" Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Size="11px" Font-Names="Calibri" />
                    </asp:GridView>

                </div>

            </div>
        </div>
    </form>
</body>
</html>
