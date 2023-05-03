<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptNotesAccount.aspx.cs" Inherits="alchemySoft.accounts.Report.Report.rptNotesAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notes to the Accounts</title>
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
            font-size: 16px;
            font-family: Calibri;
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
            font-size: 16px;
            text-align: right;
            height: 25px;
            font-family: Calibri;
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

        .style9 {
            font-family: Calibri;
            font-size: 9px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="main">
            <table style="width: 100%; margin: 1% 2% 0% 2%;">
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
                        <strong>
                            <asp:Label ID="lblHeadNM" runat="server"></asp:Label></strong>
                    </td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                </tr>
                <tr>
                    <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                        <strong>PERIOD  &nbsp;<asp:Label ID="lblFrom" runat="server"></asp:Label>
                            &nbsp;TO&nbsp; 
                    <asp:Label ID="lblTo" runat="server"></asp:Label></strong>
                    </td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                        <strong>NOTES TO THE ACCOUNTS</strong>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;"></td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                </tr>
            </table>

            <div class="showHeader" style="width: 100%; margin: 1% 2% 0% 2%;">

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    OnRowDataBound="GridView1_RowDataBound" Width="100%" ShowFooter="True">
                    <Columns>
                        <asp:BoundField HeaderText="SL">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Account Code">
                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Head Particulars">
                            <HeaderStyle HorizontalAlign="Center" Width="65%" />
                            <ItemStyle HorizontalAlign="Left" Width="65%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Amount">
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Right" Width="15%" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle CssClass="GrandTotalRowStyle" />
                    <HeaderStyle Font-Bold="True" Font-Names="Calibri" Font-Size="14px" />
                    <RowStyle Font-Size="12px" Font-Names="Calibri" />
                </asp:GridView>

            </div>
        </div>
    </form>
</body>
</html>
