<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptUserLogReport.aspx.cs" Inherits="alchemySoft.Accounts.Report.Report.rptUserLogReport" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="alchemySoft" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: Log History ::</title>
    <link href="../../../MenuCssJs/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/bootstrap.min.js"></script>
    <script src="../../../MenuCssJs/js/jspdf.js"></script>
    <script type="text/javascript">
        function ClosePrint() {
            var print = document.getElementById("print");
            print.style.visibility = "hidden";
            //            print.display = false;

            window.print();
        }
        $(document).ready(function () {
            var doc = new jsPDF();
            var specialElementHandlers = {
                '#editor': function (element, renderer) {
                    return true;
                }
            };

            $('#cmd').click(function () {
                doc.fromHTML($('#content').html(), 15, 15, {
                    'width': 170,
                    'elementHandlers': specialElementHandlers
                });
                doc.save('sample-file.pdf');
            });
        });
    </script>
    <style type="text/css">
        .panel-title {
            cursor: pointer;
        }

        .panel-body {
            padding-left: 0px;
            padding-right: 0px;
        }

        .panel-group {
            padding-left: 0px;
            padding-right: 0px;
        }

        .MyCssClass thead {
            display: table-header-group;
            border: 1px solid #000;
        }

        .container {
            /* to centre page on screen*/
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
        }

        body {
            background: #ffffff;
        }

        #btnPrint {
            font-weight: 700;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" id="content">

            <table style="width: 100%;">
                <tr>
                    <td style="width: 50%; font-size: 16px; text-align: left;">
                        <asp:Label ID="lblCompNM" runat="server" Style="font-size: 16px"></asp:Label>
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
                        <strong>USER LOG REPORT</strong>
                    </td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                        <strong>User Name : &nbsp;<asp:Label ID="lblusername" runat="server"></asp:Label>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;">
                        <strong>From :  &nbsp;<asp:Label ID="lblfdt" runat="server"></asp:Label></strong>
                        <strong>To : &nbsp;<asp:Label ID="lbltdt" runat="server"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;">
                        <%-- <button id="cmd">Save As PDF</button>--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; font-size: 14px; text-align: left; font-family: Calibri;"></td>
                    <td style="width: 50%; font-size: 14px; text-align: right; font-family: Calibri;"></td>
                </tr>
            </table>
            <br />
            <div>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    OnRowCreated="GridView1_RowCreated"
                    OnRowDataBound="GridView1_RowDataBound" Width="100%">
                    <Columns>
                        <asp:BoundField HeaderText="Log Date">
                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Log Type">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle Width="10%" CssClass="GridRowStyle" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Transaction Name">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="IP">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PC Name" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Data">
                            <HeaderStyle HorizontalAlign="Center" Width="50%" />
                            <ItemStyle HorizontalAlign="Left" Width="50%" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle Font-Names="Calibri" Font-Size="14px" />
                    <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                    <RowStyle Font-Size="12px" Font-Names="Calibri" />
                </asp:GridView>
            </div>
        </div>
        <div id="editor"></div>
    </form>
</body>
</html>
