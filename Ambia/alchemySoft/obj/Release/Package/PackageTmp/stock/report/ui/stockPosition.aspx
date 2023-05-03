<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="stockPosition.aspx.cs" Inherits="alchemySoft.stock.report.ui.stockPosition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Stock Position-Report</title>
      <link href="../../../MenuCssJs/ui-lightness/jquery.ui.theme.css" rel="stylesheet" />
    <link href="../../../MenuCssJs/ui-lightness/jquery-ui.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.9.0.js"></script>
    <script src="../../../Scripts/jquery-ui.js"></script>
    <link href="../../../Autocompletion.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtDateFr,#txtDateTo").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+100" });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Stock Position-Report</h1> 
            </div>
            <!-- content header end -->
            <!-- Content Start From here -->
            <div class="content_wrapper  col-md-12">
                <!-- Main Content -->
                <br />
                <div class="row form-class3px">
                    <div class="col-md-2"> 
                    </div>
                    <div class="col-md-2">
                        Store :
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlStore" CssClass="form-control" runat="server" Width="100%"> 
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form-class3px">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                        From Date :
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtDateFr" runat="server" CssClass="form-control"
                            ClientIDMode="Static"  Width="100%"  ></asp:TextBox>
                    </div>

                </div>
                <div class="row form-class3px">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2">
                        To Date :
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"
                            ClientIDMode="Static"  Width="100%" ></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button CssClass="form-control" ID="btnSearch" runat="server" Text="Search" Font-Bold="True"
                            Font-Italic="False" Width="100%" OnClick="btnSearch_Click"   />
                    </div>
                </div>

                <div class="row form-class3px">
                    <div class="col-md-4">
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblGridMsg" runat="server" Visible="False" ForeColor="#CC0000"
                            Style="font-weight: 700"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

