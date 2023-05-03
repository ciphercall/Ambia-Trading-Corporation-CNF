<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptSTransVoucherDrilldown.aspx.cs" Inherits="DynamicMenu.Accounts.Report.Report.rptSTransVoucherDrilldown" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Single Transaction</title>
    <link rel="shortcut icon" href="../../../Images/logo.png" />


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

        .style12 {
            width: 2px;
            font-weight: bold;
        }

        .style14 {
            width: 529px;
        }

        .style15 {
            width: 1051px;
        }

        .style16 {
            font-size: 18px;
            width: 182px;
            font-family: Calibri;
        }

        .style18 {
            width: 182px;
        }

        .style19 {
            width: 280px;
            font-family: Calibri;
        }

        .style22 {
            width: 5px;
            font-size: medium;
        }

        .style23 {
            width: 5px;
        }

        .style24 {
            width: 275px;
        }

        .style25 {
            width: 188px;
        }

        .style26 {
            width: 3px;
            font-weight: bold;
        }

        .style27 {
            width: 381px;
        }

        .style28 {
            width: 221px;
        }

        .style29 {
            width: 3px;
        }

        .style30 {
            width: 3px;
            font-size: medium;
        }

        .style31 {
            width: 180px;
            text-align: right;
        }

        .style32 {
            height: 12px;
        }

        .style33 {
            width: 105px;
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
        }

        .style42 {
            width: 283px;
            text-align: center;
        }

        .style43 {
            width: 296px;
            text-align: center;
        }

        .style44 {
            width: 297px;
            text-align: center;
        }

        .style46 {
            font-family: Calibri;
        }

        .style47 {
            width: 1051px;
            font-family: Calibri;
        }

        .style48 {
            font-family: Calibri;
        }

        .style50 {
            width: 211px;
            font-family: Calibri;
        }

        .style52 {
            font-family: Calibri;
        }

        .style53 {
            width: 572px;
        }

        .style55 {
            width: 300px;
        }

        .style56 {
            width: 480px;
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
        }

        .style62 {
            width: 186px;
        }

        .style63 {
            text-align: right;
            font-family: Calibri;
            width: 2px;
        }

        .style64 {
            height: 19px;
            text-align: right;
            font-family: Calibri;
            width: 2px;
        }

        .style65 {
            width: 2px;
        }

        .style66 {
            width: 480px;
        }

        .auto-style1 {
            text-align: right;
            font-family: Calibri;
            width: 126px;
        }

        .auto-style2 {
            height: 19px;
            text-align: right;
            font-family: Calibri;
            width: 126px;
        }

        .auto-style3 {
            width: 126px;
        }

        .auto-style5 {
            width: 431px;
            height: 19px;
        }

        .auto-style6 {
            width: 431px;
        }

        @font-face {
            font-family: "Dodgv2";
            src: url("../../../MenuCssJs/fonts/Dodgv2.ttf")format("truetype");
            font-weight: normal;
            font-style: normal;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main">
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 15%">
                            <div style="width: 120px; height: 80px;">
                                <img src="../../../Images/nmc.PNG" width="100%" height="100%;" alt="logo" />
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
                                    <td style="font-size: 10px; font-family: Calibri">Shahjadi Chamber (7th Floor), 1331/B, Sk. Mujib Road, Agrabad, Chittagong, Bangladesh.</td>
                                </tr>
                                <tr>
                                    <td style="font-size: 10px; font-family: Calibri">TEL: 880-31-724878, 717628, 717675, 2511239 FAX: 880-31-714736 E-mail: akdl_office@ambiagroup.com</td>
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

                <table style="width: 100%; font-size: 12px; font-family: Calibri">
                    <tr>
                        <td style="width: 35%">
                            <table>
                                <tr>
                                    <td style="width: 60%">Voucher No</td>
                                    <td style="text-align: left">
                                        <strong>:</strong></td>
                                    <td>
                                        <asp:Label ID="lblVNo" runat="server"
                                            CssClass="style61"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Voucher Date</td>
                                    <td style="text-align: right;">
                                        <strong>:</strong></td>
                                    <td>
                                        <asp:Label ID="lblTime" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 35%">
                            <div style="width: 100%; height: 30px;">
                                <table style="width: 100%;">

                                    <tr>
                                        <td colspan="3" style="font-weight: 700; font-size: 18px; text-align: center;">
                                            <asp:Label ID="lblVtype" runat="server" Style="font-family: Calibri"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width: 30%">
                            <table>
                                <tr>
                                    <td>Print Time</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblPrintTime" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>User Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblUserName" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>

                <table style="width: 100%; font-size: 12px; font-family: Calibri">
                    <tr>
                        <td style="width: 70%">
                            <table style="width: 100%; font-size: 12px; font-family: Calibri">
                                <tr>

                                    <td style="width: 24%; text-align: left">
                                        <asp:Label ID="lblReceiveCrBy" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 1%; text-align: center"><strong>:</strong></td>
                                    <td style="width: 75%; text-align: left">
                                        <asp:Label ID="lblReceivedBy" runat="server"></asp:Label>
                                        <asp:Label ID="lblMidDate" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 24%; text-align: left">
                                        <asp:Label ID="lblReceiveCrFrom" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 1%; text-align: center"><strong>:</strong></td>
                                    <td style="width: 75%; text-align: left">
                                        <asp:Label ID="lblReceivedFrom" runat="server"></asp:Label>
                                        <asp:Label ID="lblAmount" runat="server" Visible="False"></asp:Label>
                                    </td>

                                </tr>
                            </table>
                        </td>
                        <td style="width: 30%; font-family: Calibri;">
                            <table>
                                <tr>
                                    <td>Entry Time</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblInTime" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>User Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblEntryUserName" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </div>
            <div>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <table style="width: 100%; font-family: Calibri; border: 1px solid #000000">
                                <tr>
                                    <td style="text-align: center; font-weight: 700;"
                                        class="style16">Particulars</td>
                                    <td style="border-left: 1px solid #000000; text-align: center; font-weight: 700;"
                                        class="style16">Amount (Tk.)</td>
                                </tr>
                                <tr style="">
                                    <td style="border-top: 1px solid #000000; height: 40px; width: 80%">
                                        <asp:Label ID="lblParticulars" runat="server"></asp:Label>
                                    </td>
                                    <td style="border-top: 1px solid #000000; border-left: 1px solid #000000; text-align: right; height: 40px; width: 20%">
                                        <asp:Label ID="lblAmountComma" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr>
                        <td>
                            <table style="width: 100%; font-family: Calibri;font-size: 12px">
                                <tr>
                                    <td class="style19">
                                        <asp:Label ID="lblReceiveMode" runat="server"></asp:Label>
                                        Mode
                                    </td>
                                    <td class="style22">
                                        <strong>:</strong></td>
                                    <td class="style24">
                                        <asp:Label ID="lblRMode" runat="server"
                                            CssClass="style52"></asp:Label>
                                    </td>
                                    <td class="style25">
                                        <asp:Label ID="lblTransForName" runat="server" Text="Transaction For"></asp:Label></td>
                                    <td class="style26">
                                        <asp:Label ID="lblTransforSC" runat="server" Text=":"></asp:Label></td>
                                    <td class="style27">
                                        <asp:Label ID="lblTransFor" runat="server"></asp:Label></td>
                                    <td class="style28">&nbsp;</td>
                                    <td class="style29">&nbsp;</td>
                                    <td class="style31">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="style19">Cheque No</td>
                                    <td class="style22">
                                        <strong>:</strong></td>
                                    <td class="style24">
                                        <asp:Label ID="lblChequeNo" runat="server"
                                            CssClass="style52"></asp:Label>
                                    </td>
                                    <td class="style25" style="font-family: Calibri;">Cheque Date</td>
                                    <td class="style26">:</td>
                                    <td class="style27">
                                        <asp:Label ID="lblChequeDT" runat="server"></asp:Label>
                                    </td>
                                    <td class="style28"
                                        style="font-size: 15px; font-weight: 600; text-align: right; font-family: Calibri;">Total (Tk.)</td>
                                    <td class="style30">
                                        <strong>:</strong></td>
                                    <td class="style31">
                                        <asp:Label ID="lblTotAmount" runat="server"
                                            Style="font-size: 15px; text-align: right; font-weight: 600;"
                                            CssClass="style46"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div>

                <table width="100%">
                    <tr>
                        <td>
                            <table style="width: 100%; font-family: Calibri;font-size: 12px">
                                <tr>
                                    <td class="style33">In Words</td>
                                    <td class="style34">
                                        <strong>:</strong></td>
                                    <td>
                                        <div style="border-bottom: 1px solid #000000; width: 60%; font-family: Calibri;">
                                            <asp:Label Style="font-family: Calibri;" ID="lblInWords" runat="server"></asp:Label>
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

                        </td>
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
