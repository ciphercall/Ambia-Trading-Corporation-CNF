<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorList.aspx.cs" Inherits="DynamicMenu.Error.ErrorList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="css/jquery-2.1.3.js"></script>
    <script src="css/jquery-ui.js"></script> 
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $("#<%=txtFR.ClientID%>,#<%=txtTO.ClientID%>").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            color: #800000;
            font-size: large;
            text-decoration: underline;
        }

        .Grid {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
            font-size: 9pt;
        }

            .Grid td {
                padding: 2px;
                border: solid 1px #c1c1c1;
            }

            .Grid tr:hover {
                background: #cbf3dd;
                color: #b22929;
            }

            .Grid th {
                padding: 4px 2px;
                color: #fff;
                background: #525252;
                border-left: solid 1px #525252;
                font-size: 0.9em;
                font-size: 9pt;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">

            <span class="auto-style1"><strong>Error List<br />
            </strong></span>
            <br />

            From :
        <asp:TextBox ID="txtFR" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp; To:
        <asp:TextBox ID="txtTO" runat="server"></asp:TextBox>

            &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSearch" runat="server" Text="Search" Height="22px" OnClick="btnSearch_Click" /><hr />

        </div>
        <div>

            <asp:GridView ID="GridView1" CssClass="Grid" Width="100%" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="SL" DataField="SL">
                        <HeaderStyle HorizontalAlign="Center" Width="4%" />
                        <ItemStyle HorizontalAlign="Left" Width="4%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Form Name" DataField="FORMNM">
                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Error Message" DataField="ERROR">
                        <HeaderStyle HorizontalAlign="Center" Width="50%" />
                        <ItemStyle HorizontalAlign="Left" Width="50%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Action From" DataField="ACTIONFR">
                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="User" DataField="USERNM">
                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="In Time" DataField="INTIME">
                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>
