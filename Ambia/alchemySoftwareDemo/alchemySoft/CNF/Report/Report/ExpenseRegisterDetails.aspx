<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpenseRegisterDetails.aspx.cs"
    Inherits="alchemySoft.CNF.report.vis_rep.ExpenseRegisterDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="shortcut icon" href="../../../Images/logo.png" />
    <link href="../../../css/ui-darkness/jquery.ui.theme.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/ui-darkness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.9.0.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ClosePrint() {
            var print = document.getElementById("print");
            print.style.visibility = "hidden";
            //            print.display = false;

            window.print();
        }
    </script>
    <style media="print" type="text/css">
        .ShowHeader thead
        {
            display: table-header-group;
            border: 1px solid #000;
        }
        
        .line-separator
        {
            height: 1px;
            background: #717171;
            border-bottom: 1px solid #313030;
        }
    </style>
    <style type="text/css">
        .style1
        {
            width: 15%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%">
            <tr>
                <td style="width: 10%" rowspan="3">
                </td>
                <td style="width: 80%; text-align: center">
                </td>
                <td style="width: 10%">
                    <input id="print" tabindex="1" type="button" value="Print" onclick="ClosePrint()"
                        style="font-family: Calibri; font-size: 15px; font-weight: bold; font-style: inherit" />
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 2%; text-align: left">
                </td>
                <td style="width: 98%; text-align: left; font-size: 19PX; font-family:Calibri" >
                    EXPENSE REGISTER DETAILS
                </td>
                
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 2%; text-align: left">
                &nbsp;
                </td>
                <td style="width:4%; text-align:left; font-family:Calibri">
                    From
                </td>
                <td style="width: 1%; text-align: center">
                    :
                </td>
                <td style="width: 5%">
                    <asp:Label runat="server" ID="lblFromdate" Font-Names="Calibri"></asp:Label>
                </td>
                <td style="width: 3%; text-align: right; font-family:Calibri" >
                    To
                </td>
                <td style="width: 1%; text-align: center">
                    :
                </td>
                <td style="width: 6%">
                    <asp:Label ID="lblTodate" runat="server" Font-Names="Calibri"></asp:Label>
                </td>
                <td style="width: 44%; text-align: right; font-family:Calibri" >
                    Print date
                </td>
                <td style="width: 1%; text-align: center">
                    :
                </td>
                <td style="width: 33%">
                    <asp:Label ID="lblPrintDate" runat="server" Font-Names="Calibri"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 2%; text-align: left">
                </td>
                <td style="width: 15%; text-align: left; font-family:Calibri">
                    Expense Head 
                </td>
                <td style="width: 1%; text-align: center">
                    : 
                </td>
                <td style="width: 82%">
                    <asp:Label runat="server" ID="lblExpenseHD" Font-Names="Calibri"></asp:Label>
                  
                </td>
              
            </tr>
        </table>
       <%-- <table style="width: 100%">
            <tr>
                <td style="width: 2%; text-align: left">
                </td>
                <td style="width: 12%; text-align: left">
                    
                </td>
                <td style="text-align: left" class="style1">
                    &nbsp;<asp:Label ID="lblExpenseNM" runat="server" Font-Names="Calibri"> </asp:Label>
                </td>
                <td style="width: 1%; text-align: center">
                    &nbsp;
                </td>
                <td style="width: 15%">
                    <asp:Label ID="lblExpenseID" runat="server" Visible="false" Font-Names="Calibri"></asp:Label>
                </td>
                <td style="width: 10%">
                    &nbsp;
                </td>
                <td style="width: 15%">
                </td>
                <td style="width: 5%">
                </td>
                <td style="width: 16%; text-align: right">
                </td>
                <td style="width: 12%">
                </td>
            </tr>
        </table>--%>
        <div style="width: 96%; margin: 1% 2% 0% 2%;">
            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" Font-Names="Calibri"
                DataKeyNames="JOBNO,JOBYY,JOBTP " OnRowDataBound="gvReport_RowDataBound" ShowFooter="True"
                Width="100%" OnRowCreated="gvReport_RowCreated">
                <%--<Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:GridView ID="gvSubReport" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">--%>
                <Columns>
                    <asp:BoundField HeaderText="Date" DataField="TD">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Invoice No" DataField="TRANSNO">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Expense Name" DataField="EXPNM">
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Amount" DataField="EXPAMT">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Remarks" DataField="REMARKS">
                        <HeaderStyle HorizontalAlign="Center" Width="30%" />
                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                    </asp:BoundField>
                    
                </Columns>
                <FooterStyle Font-Size="14px" />
                <HeaderStyle Font-Size="14px" />
                <RowStyle Font-Size="12px" />
            </asp:GridView>
        </div>
        <table style="width: 100%">
            <tr>
                <td style="width: 2%; text-align: left">
                </td>
                <td style="width: 5%">
                </td>
                <td style="width: 1%">
                </td>
                <td style="width: 84%">
                </td>
            </tr>
            <tr>
                <td style="width: 2%; text-align: left">
                </td>
                <td style="width: 5%; text-align: right; font-family: Calibri; font-size: 14px; font-weight: bold">
                    &nbsp;
                </td>
                <td style="width: 1%; text-align: center; font-family: Calibri; font-size: 14px;
                    font-weight: bold">
                    &nbsp;
                </td>
                <td style="width: 84%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 2%; text-align: left">
                </td>
                <td style="width: 5%; text-align: left; font-family: Calibri; font-size: 14px; font-weight: bold">
                </td>
                <td style="width: 1%; text-align: center; font-family: Calibri; font-size: 14px;
                    font-weight: bold">
                </td>
                <td style="width: 84%">
                    <asp:Label ID="lblInWords" runat="server" Font-Names="Calibri" Font-Size="15px" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
