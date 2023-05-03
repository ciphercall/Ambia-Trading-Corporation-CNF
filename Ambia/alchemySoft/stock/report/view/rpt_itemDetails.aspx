<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_itemDetails.aspx.cs" Inherits="alchemySoft.stock.report.view.rpt_itemDetails" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report-Item Details</title>
    
    <link href="../../../css/ui-darkness/jquery.ui.theme.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/ui-darkness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js" type="text/javascript"></script>
     

    <script type ="text/javascript">
        function ClosePrint() {
            var print = document.getElementById("print");
            print.style.visibility = "hidden";
            //            print.display = false;

            window.print();
        }
    </script>

    <style media="print">
        .showHeader thead
         {
            display: table-header-group;
            border: 1px solid #000;
         }
    </style>
        <style type ="text/css">
        #main
        {
            float:left;
            border: 1px solid #cccccc;
            width: 100%;
            padding-bottom:40px;
        }
        #btnPrint
        {
            font-weight: 700;
        }
            .style1
            {
                font-size: small;
            }
                        
        .SubTotalRowStyle
        {
            border: solid 1px Black;
            
            font-weight: bold;
            text-align: right;
        }
        .GrandTotalRowStyle
        {
            border: solid 1px Gray;
            color: #000000;
            font-weight: bold;
            font-size: 18px;
            text-align: right;
            height: 35px;
        }
        .GroupHeaderStyle
        {
            border: solid 1px Black;
            text-align: left;
            color: #000000;
            font-weight: bold;
            height: 30px;
        }
        .GridRowStyle
        {
            padding-left:10%;
            
        }
            .style3
            {
                font-family: Calibri;
            }
      </style>

</head>

<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <table style="width: 100%;">
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td class="style1">
                    <asp:Label ID="lblCompNM" runat="server" 
                        style="font-family: Calibri; font-size: 20px; font-weight: 700"></asp:Label>
                </td>
                <td class="style5" style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right">
                    <input id="print" tabindex="1" type="button" value="Print" onclick = "ClosePrint()"/></td>
                <td style="text-align: right">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style7">
                    &nbsp;</td>
                <td class="style3">
                    <asp:Label ID="lblAddress" runat="server" 
                        style="font-family: Calibri; font-size: 9px"></asp:Label>
                </td>
                <td class="style5">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style7">
                    &nbsp;</td>
                <td class="style3">
                    <strong>Item Details</strong></td>
                <td class="style5">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style7">
                    &nbsp;</td>
                <td class="style8">
                    <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
                </td>
                <td class="style5">
                    &nbsp;</td>
                <td style="text-align: right">
                    <asp:Label ID="lblTime" runat="server" 
                        style="text-align: right; font-family: Calibri; font-size: medium;"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            </table>
            <div style = "width:96%; margin: 0% 2% 0% 2%; height: 1px; background: #000000;">
            </div>
            <div style = "width:96%; margin: 0% 2% 0% 2%;">

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    onrowcreated="GridView1_RowCreated" CssClass="Grid"
                    onrowdatabound="GridView1_RowDataBound" Width="100%">
                    <Columns>
                        <asp:BoundField HeaderText="Item Particulars" >
                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                        <ItemStyle Width="20%" CssClass="GridRowStyle" HorizontalAlign="Justify" />
                        </asp:BoundField> 
                        <asp:BoundField HeaderText="Item Code" DataField="ITEMCD" >
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Color"  DataField="COLOR">
                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Buy Rate"  DataField="BUYRT">
                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Sale Rate" DataField="SALERT">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle Font-Names="Calibri" Font-Size="14px" />
                    <RowStyle Font-Names="Calibri" Font-Size="12px" />
                </asp:GridView>

            </div>
        </div>
    </div>
    </form>
</body>
</html>
