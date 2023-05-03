<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="MaintenanceFeeProcess.aspx.cs" Inherits="alchemy.accounts.UI.MaintenanceFeeProcess" %>

<%@ Import Namespace="DynamicMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //BindControlEvents();
        });
        function pageLoad() {
            BindControlEvents();
        }
        function BindControlEvents() {
            $("#<%=txtDate.ClientID%>,#<%=txtEffectDate.ClientID%>").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>

            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Maintenance Bill Process</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

                    </div>
                    <!-- content header end -->
                    <!-- Content Start From here -->
                    <div class="form-class">

                        <div class="row form-class">
                            <div class="col-md-2">Date</div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="txtDate" CssClass="form-control input-sm  text-center"
                                    OnTextChanged="txtDate_OnTextChanged" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="txtMonthYear" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Transaction No</div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlTransno" CssClass="form-control input-sm" TabIndex="2" />
                            </div>
                        </div>
                        <hr />
                        <div class="row form-class">
                            <div class="col-md-2">Effect Date</div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="txtEffectDate" CssClass="form-control input-sm text-center"
                                    OnTextChanged="txtEffectDate_OnTextChanged" AutoPostBack="True" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="txtEffectMonthYear" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Effect Trans No</div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="txtEffectTransno" ReadOnly="True" CssClass="form-control input-sm  text-center"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList runat="server" ID="ddlTransType" CssClass="form-control input-sm">
                                    <asp:ListItem Value="JOUR">Journal</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row form-class red-color text-center">
                            <asp:Label runat="server" ID="lblMsg" Visible="False"></asp:Label>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2">
                                Remarks
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox runat="server" ID="txtRemarks" TabIndex="4" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <% if (dbFunctions.checkParmit("/Accounts/UI/MaintenanceFeeProcess.aspx", "INSERTR") == true)
                                    { %>
                                <asp:Button runat="server" ID="btnSubmit" TabIndex="10" CssClass="form-control input-sm btn-primary"
                                    Text="Submit" OnClick="btnSubmit_OnClick" />
                                <% } %>
                            </div>
                        </div>
                    </div>
                    <!-- Content End From here -->
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
