<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="BalanceSheet.aspx.cs" Inherits="DynamicMenu.Accounts.Report.UI.BasicInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Balance Sheet</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div>
            <!-- content header end -->
            <!-- Content Start From here -->
            <br />
          <div class="content_wrapper  col-md-12">
                
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2"> <strong>Date</strong></div>
                    <div class="col-md-4"> <asp:TextBox ID="txtDate" runat="server"  
                        CssClass="form-control input-sm" ClientIDMode="Static" ></asp:TextBox></div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2"><asp:Button ID="btnSearch" runat="server" Text="Search" 
                        onclick="btnSearch_Click" CssClass="form-control input-sm btn-primary"/></div>
                    <div class="col-md-5"></div>
                    
                </div>
                
            </div>
            <!-- Content End From here -->
        </div>
    </div>


</asp:Content>
