<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_stockPosition.aspx.cs" Inherits="alchemySoft.stock.report.view.rpt_stockPosition" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../../../css/ui-darkness/jquery.ui.theme.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/ui-darkness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js" type="text/javascript"></script> 
    <script src="../../../MenuCssJs/js/jquery-ui.js"  type="text/javascript"></script>

    <script type="text/javascript">
        function ClosePrint() {
            var print = document.getElementById("print");
            print.style.visibility = "hidden";
            //            print.display = false;

            window.print();
        }
    </script>

    <style type="text/css">
        .auto-style2 {
            width: 100%;
        }

        .auto-style3 {
            text-align: center;
        }

        .auto-style4 {
            font-size: medium;
        }
    </style>

</head>

<body>
    <form id="form1" runat="server">
        <div>
            <div>

                <table class="auto-style2">
                    <tr>
                        <td class="auto-style3">
                            <div style="float: right; display: inline-block; margin-left: -35px">
                                <input id="print" tabindex="1" type="button" value="Print" onclick="ClosePrint()" />
                            </div>
                            <div style="display: inline-block">
                                <asp:Label ID="lblCompNM" runat="server"
                                    Style="font-family: Calibri; font-size: 20px; font-weight: 700"></asp:Label>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="lblAddress" runat="server"
                                Style="font-family: Calibri; font-size: 10px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <strong>S</strong><span class="auto-style4"><strong>TOCK POSITION-REPORT</strong></span></td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            
                            <div style="display: inline-block;" >
                                <asp:Label ID="lblDate" runat="server" CssClass="style2"></asp:Label>
                            </div>
                            
                        </td>
                    </tr>
                </table>
                <div style="float: right; display: inline-block; margin-left: -35px">
                                <asp:Label ID="lblTime" runat="server"
                                    Style="text-align: right; font-family: Calibri; font-size: medium;"></asp:Label>
                            </div>
                &nbsp;<table style="width: 100%;">
                    <tr>
                        <td style="font-family: Calibri; font-size: medium">
                            <strong>
                                STORE NAME : &nbsp;<asp:Label ID="lblStoreNM" runat="server"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                </table>
                <div style="width: 100%;">

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="Item " DataField="ITEMNM">
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                <ItemStyle Width="15%" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Per Stock" DataField="OPENING">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Total Receive" DataField="RCVQTY">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle Width="8%" HorizontalAlign="Center" />

                            </asp:BoundField>
                            <asp:BoundField HeaderText="Total Stock" DataField="STOCK">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Issue" DataField="ISUQTY">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Balance" DataField="BLNC">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle Font-Names="Calibri" Font-Size="16px" />
                        <RowStyle Font-Names="Calibri" Font-Size="14px" />
                    </asp:GridView>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
