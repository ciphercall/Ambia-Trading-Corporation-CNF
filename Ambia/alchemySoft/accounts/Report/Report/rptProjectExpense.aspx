<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptProjectExpense.aspx.cs" Inherits="alchemySoft.accounts.Report.Report.rptProjectExpense" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            width: 765px;
            font-family: Calibri;
        }

        .style12 {
            width: 273px;
        }

        .style14 {
            text-align: center;
            width: 765px;
            font-family: Calibri;
            font-size: 20px;
            font-weight: 700;
        }

        .style15 {
            font-size: 15px;
        }

        .style16 {
            width: 279px;
        }

        .style17 {
            width: 10px;
        }

        .style18 {
            font-family: Calibri;
            font-size: 15px;
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
                            <strong>TRANSACTION STATEMENT</strong>
                        </td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                            <strong>PERIOD  &nbsp;<asp:Label ID="lblFrom" runat="server"></asp:Label>
                                &nbsp;TO&nbsp; 
                    <asp:Label ID="lblTo" runat="server"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                            <strong>OFFICE :&nbsp;
                                        <asp:Label ID="lblProjectName" runat="server"></asp:Label>
                            </strong>
                        </td>
                        <td style="width: 50%; font-size: 14px; text-align: right; text-transform: uppercase; font-family: Calibri;">
                            <strong>TRANSACTION TYPE :&nbsp;
                                    <asp:Label runat="server" ID="lblType"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>

                <div style="width: 100%; margin: 1% 0% 0% 0%;">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        Font-Size="12px" OnRowDataBound="GridView1_RowDataBound" ShowFooter="True"
                        ShowHeaderWhenEmpty="True" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="Debit Head Particulars">
                                <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit Head Particulars">
                                <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Amount (Tk.)">
                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle Font-Names="Calibri"
                            Font-Size="14px" Font-Bold="True" />
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Names="Calibri" Font-Size="12px" />
                    </asp:GridView>

                </div>

            </div>
        </div>
    </form>
</body>
</html>
