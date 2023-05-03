<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptMultipleVoucherEdit.aspx.cs" Inherits="alchemySoft.accounts.Report.Report.rptMultipleVoucherEdit" %>

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
        #main {
            width: 620px;
            border: 2px solid #000;
            /* to centre page on screen*/
            margin-left: auto;
            margin-right: auto;
        }

        #btnPrint {
            font-weight: 700;
            font-style: italic;
        }

        .style1 {
            font-size: smaller;
        }

        .style2 {
            width: 862px;
        }

        .style3 {
            font-size: 11px;
            width: 862px;
        }

        .style5 {
            font-size: smaller;
            width: 190px;
        }

        .style29 {
            width: 3px;
        }

        .style30 {
            width: 3px;
            font-size: medium;
        }

        .style33 {
            width: 88px;
        }

        .style34 {
            width: 1px;
        }

        .style38 {
            width: 54px;
        }

        .style39 {
            width: 277px;
            text-align: center;
            font-size: medium;
        }

        .style42 {
            width: 283px;
            text-align: center;
            font-size: medium;
        }

        .style43 {
            width: 296px;
            font-size: medium;
            text-align: center;
        }

        .style44 {
            width: 297px;
            font-size: medium;
            text-align: center;
        }

        .style56 {
            width: 473px;
            height: 19px;
        }

        .style58 {
            height: 19px;
        }

        .style59 {
            height: 19px;
            text-align: right;
            font-family: Calibri;
            width: 186px;
        }

        .style60 {
            text-align: right;
            font-family: Calibri;
            width: 186px;
        }

        .style61 {
            font-family: Calibri;
            font-size: small;
        }

        .style62 {
            width: 186px;
            font-family: Calibri;
            text-align: right;
        }

        .style63 {
            text-align: center;
            font-family: Calibri;
            width: 315px;
            font-size: medium;
        }

        .style64 {
            height: 19px;
            text-align: right;
            font-family: Calibri;
            width: 315px;
        }

        .style66 {
            width: 473px;
        }

        .style67 {
            width: 315px;
        }

        @font-face {
            font-family: "Dodgv2";
            src: url("../../../MenuCssJs/fonts/Dodgv2.ttf")format("truetype");
            font-weight: normal;
            font-style: normal;
        }
    </style>
</head>
<body style="font-size: small">
    <form id="form1" runat="server">
        <div id="main">
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 15%">
                            <div style="width: 120px; height: 80px;">
                                <img src="../../../Images/logo.png" width="100%" height="100%;" alt="logo" />
                            </div>
                        </td>

                        <td style="width: 70%">
                            <table style="width: 100%; text-align: center">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCompNM" runat="server" Style="font-family: Dodgv2; font-size: 21px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size: 10px; font-family: Calibri">Chittagong 	: SHAHAJADI CHAMBER (7TH FLOOR), 1331/B, SK. MUJIB ROAD, AGRABAD, CHITTAGONG-4100, BANGLADESH-4000.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size: 10px; font-family: Calibri">Contact    	: Mobile: 01678060322, E-mail: akdl_office@ambiagroup.com
                                    </td>
                                </tr>
                            </table>
                        </td>

                        <td style="width: 15%; font-size: 11px; text-align: right">
                            <input id="print" tabindex="1" type="button" value="Print" onclick="ClosePrint()" />
                        </td>
                    </tr>
                </table>
            </div>

            <div>
                <table style="width: 100%; font-size: 12px">
                    <tr style="text-align: center">
                        <td style="width: 30%">
                            <table>
                                <tr>
                                    <td style="text-align: left">Voucher No</td>
                                    <td><strong>:</strong></td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblVNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">Voucher Date</td>
                                    <td><strong>:</strong></td>
                                    <td>
                                        <asp:Label ID="lblTime" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 40%; text-align: center">
                            <strong style="font-size: 20px">
                                <asp:Label ID="lblVoucherName" runat="server"></asp:Label>
                                VOUCHER</strong>
                        </td>
                        <td style="width: 30%">
                            <table style="width: 100%; font-size: 12px">
                                <tr>
                                    <td>Print Time</td>
                                    <td>:</td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblPrintTime" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>User Name</td>
                                    <td>:</td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Entry Time</td>
                                    <td>:</td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblEntryTime" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>User Name</td>
                                    <td>:</td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblEntryUserNm" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </div>
            <br />
            <div>

                <asp:GridView ID="gvdetails" runat="server" AutoGenerateColumns="False"
                    OnRowDataBound="gvdetails_RowDataBound" ShowFooter="True"
                    Style="font-family: Calibri" Width="100%">
                    <Columns>
                        <asp:BoundField HeaderText="SL">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Debited To">
                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Credited To">
                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Reamarks">
                            <HeaderStyle HorizontalAlign="Center" Width="35%" />
                            <ItemStyle HorizontalAlign="Left" Width="35%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                            <ItemStyle HorizontalAlign="Right" Width="20%" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle Font-Names="Calibri" Font-Size="14px" />
                    <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                    <RowStyle Font-Names="Calibri" Font-Size="12px" />
                </asp:GridView>

            </div>

            <div style="margin-top: 1%;">

                <table style="width: 100%; font-family: Calibri;">
                    <tr>
                        <td class="style33" style="font-size: 10pt">In Words</td>
                        <td class="style34" style="font-size: medium">
                            <strong>:</strong></td>
                        <td>
                            <div style="border-bottom: 1px solid #000000; width: 60%;">
                                <asp:Label ID="lblInWords" runat="server" Style="font-size: 10pt;"></asp:Label>
                                <div style="border-bottom: 1px solid #000000; width: 100%;"></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="style33" style="font-size: medium">&nbsp;</td>
                        <td class="style34" style="font-size: medium">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>

            </div>
            <div style="width: 100%">
                <table style="width: 100%; font-family: Calibri; font-size: 13px">
                    <tr>
                        <td style="width: 33%; text-align: center">&nbsp;</td>
                        <td style="width: 34%; text-align: center">&nbsp;</td>
                        <td style="width: 33%; text-align: center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 33%; text-align: center">Prepared By</td>
                        <td style="width: 34%; text-align: center">Received By</td>
                        <td style="width: 33%; text-align: center">Authorized By</td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
