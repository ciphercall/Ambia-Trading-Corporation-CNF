<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportBankBook.aspx.cs" Inherits="alchemySoft.accounts.Report.Report.ReportBankBook" %>

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
            font-style: italic;
        }

        .style8 {
            text-align: left;
            font-family: Calibri;
            font-size: 16px;
            width: 836px;
        }

        .style9 {
            text-align: left;
            width: 836px;
        }

        .style13 {
            width: 2px;
            font-weight: 700;
        }

        .style16 {
            width: 709px;
            font-weight: bold;
        }

        .style18 {
            width: 188px;
            font-weight: bold;
        }

        .style19 {
            width: 183px;
        }

        .style23 {
            font-size: 20px;
            font-weight: 700;
            font-family: Calibri;
            width: 836px;
        }

        .style24 {
            font-family: Calibri;
            font-size: 16px;
        }

        .style25 {
            width: 19px;
        }

        .style26 {
            width: 384px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="main">
            <div>
                 <table style="width: 100%">
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
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                            <strong>BANK BOOK</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                            <strong>PERIOD  &nbsp;<asp:Label ID="lblFrom" runat="server"></asp:Label>
                                &nbsp;TO&nbsp; 
                    <asp:Label ID="lblTo" runat="server"></asp:Label></strong>
                        </td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                            <strong>Opening Balance (Tk.):&nbsp;&nbsp;
                                <asp:Label ID="lblOpenBal" runat="server" CssClass="style22"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;"></td>
                        <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                    </tr>
                </table>

                <div style="margin-top: 0px;">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="GridView1_RowDataBound"
                        ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Doc. No">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Particulars">
                                <HeaderStyle HorizontalAlign="Center" Width="35%" />
                                <ItemStyle HorizontalAlign="Left" Width="35%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Debit (Tk.)">
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Credit (Tk.)">
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Balance (Tk.)">
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                        <RowStyle Font-Names="Calibri" Font-Size="12px" />
                    </asp:GridView>
                </div>


                 <div>
                    <br />
                    <table style="width: 100%; font-family: Calibri; font-size: 14px">
                        <tr>
                            <td style="width: 55%; text-align: right;">
                                <strong>Periodic Total :</strong>
                            </td>
                            <td style="width: 15%; text-align: right; border-top: 1px solid #000;">
                                <strong>
                                    <asp:Label ID="lblPeriodicDB" runat="server"></asp:Label>
                                </strong>
                            </td>
                            <td style="width: 15%; text-align: right; border-top: 1px solid #000;">
                                <strong>
                                    <asp:Label ID="lblPeriodicCR" runat="server"></asp:Label>
                                </strong>
                            </td>
                            <td style="width: 15%"></td>
                        </tr>
                        <tr>
                            <td style="width: 55%; text-align: right; font-size: 14px">
                                <strong>Periodic Balance :</strong>
                            </td>
                            <td style="width: 15%; text-align: right; border-top: 1px solid #000;">
                                <strong>
                                    <asp:Label ID="lblPeriodicBalance" runat="server"></asp:Label></strong>
                            </td>
                            <td style="width: 15%; text-align: right;">
                                <strong></strong>
                            </td>
                            <td style="width: 15%"></td>
                        </tr>
                    </table>
                    <br />
                </div>
                
                 <table style="width: 100%; border: 2px solid #000; font-family: Calibri; font-size: 14px">
                    <tr>
                        <td style="width: 55%; text-align: right; border-right: 2px solid;">
                            <strong>
                              
                                Total :
                            </strong>
                        </td>
                        <td style="width: 15%; text-align: right; border-right: 2px solid;">
                            <strong>
                                <asp:Label ID="lblTotBalance" runat="server" ></asp:Label></strong>
                        </td>
                        <td style="width: 15%; text-align: right; border-right: 2px solid;">
                            <strong> <asp:Label ID="lblTotCR" runat="server"></asp:Label></strong>
                        </td>
                        <td style="width: 15%; text-align: right">
                            <strong>
                                 <asp:Label ID="lblLastCumBalC" runat="server" Visible="False"></asp:Label>  
                            <asp:Label ID="lblLastCumBalance" runat="server"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
