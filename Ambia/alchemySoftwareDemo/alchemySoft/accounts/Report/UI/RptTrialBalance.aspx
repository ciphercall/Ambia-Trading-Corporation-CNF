<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="RptTrialBalance.aspx.cs" Inherits="DynamicMenu.Accounts.Report.UI.RptTrialBalance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+10" });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Trial balance</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


            </div>
            <!-- content header end -->


            <!-- Content Start From here -->
            <div class="form-class">
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">AS AT</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control input-sm"
                            ClientIDMode="Static" TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search"
                            CssClass="form-control input-sm btn-primary" OnClick="btnSearch_Click" TabIndex="3" />
                    </div>
                    <div class="col-md-5"></div>

                </div>
            </div>
            <!-- Content End From here -->
        </div>
    </div>
</asp:Content>
