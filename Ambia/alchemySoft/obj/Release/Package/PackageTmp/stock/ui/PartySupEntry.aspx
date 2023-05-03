<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="PartySupEntry.aspx.cs" Inherits="DynamicMenu.Stock.UI.PartySupEntry" %>
<%@ Import Namespace="alchemySoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
     <link rel="shortcut icon" href="icon.icns" />

    <script type="text/javascript">
        $(document).ready(function () {
            BindControlEvents();
        });
        function BindControlEvents() {
            $("#txtDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });
            Search_GetCompletionListParty();
            Search_GetCompletionListSuppliar();
            //$('.ui-autocomplete').click(function () {
            //    __doPostBack();
            //});
            //$('.ui-autocomplete').select(function () {
            //    __doPostBack();
            //});

            $('.ui-autocomplete').click(function () {
                __doPostBack();
            });
            $("#<%=txtPNM.ClientID %>,#<%=txtSNM.ClientID %>").keydown(function (e) {
                if (e.which == 9 || e.which == 13)
                    window.__doPostBack();
            });
        };

        function confMSG() {
            if (confirm("Are you Sure to Delete?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function Search_GetCompletionListParty() {
            $("#<%=txtPNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListParty",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtPNM.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    value: item
                                };
                            }));
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
            });
        }
        function Search_GetCompletionListSuppliar() {
            $("#<%=txtSNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListSuppliar",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtSNM.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    value: item
                                };
                            }));
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
            });
        }
    </script>
    <style>
        .ui-autocomplete {
            max-width: 350px;
            max-height: 250px;
            overflow: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="Up_TabContaner" runat="server">
        <ContentTemplate>
            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>PARTY & SUPPLIER ENTRY</h1> 

                    </div> 
                    
                     
                    <div class="content_wrapper  col-md-12">
                        <br />
                       <br />
                        <asp:Label ID="lblPS_ID" runat="server" Visible="False"></asp:Label>
                        <div class="row"></div>
                        <div class="row form-class">
                            <div class="col-md-2">
                                <b>PS Type :</b>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlPSTP" runat="server" TabIndex="1" CssClass="form-control input-sm"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlPSTP_SelectedIndexChanged">
                                    <asp:ListItem Value="P">PARTY</asp:ListItem>
                                    <asp:ListItem Value="S">SUPPLIER</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <%--<a onclick="return showInFrameDialog(this,'EditPSEntry.aspx','Edit Party Suppliar');"
                            href="" 
                            style="font-family: Helvetica Neue,Lucida Grande,Segoe UI,Arial,Helvetica,Verdana,sans-serif; font-size: 1.2em; font-weight: bold; font-style: italic; text-decoration: none; color: #000000; text-align: center; ">Edit</a>--%>
                            </div>
                            <div class="col-md-2">
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">
                                <b>PS Name :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPNM" runat="server" TabIndex="2" Width="100%" CssClass="form-control input-sm"
                                    AutoPostBack="True" OnTextChanged="txtPNM_TextChanged"></asp:TextBox>
                                <asp:TextBox ID="txtSNM" runat="server" TabIndex="2" Width="100%" CssClass="form-control input-sm"
                                    AutoPostBack="True" OnTextChanged="txtSNM_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPSCD" runat="server" Width="100%" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">
                                <b>City :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCity" runat="server"  Width="100%" TabIndex="3" CssClass="form-control input-sm"
                                    OnTextChanged="txtCity_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <b>Address :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAddress" runat="server" Width="100%" TabIndex="4" TextMode="MultiLine"
                                    CssClass="form-control input-sm" OnTextChanged="txtAddress_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">
                                <b>Contact No :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtContact" runat="server" Width="100%" TabIndex="5" CssClass="form-control input-sm"
                                    OnTextChanged="txtContact_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <b>Email :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" TabIndex="6" CssClass="form-control input-sm"
                                    OnTextChanged="txtEmail_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">
                                <b>Web ID :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtWebID" runat="server" Width="100%" TabIndex="7" CssClass="form-control input-sm"
                                    OnTextChanged="txtWebID_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <b>C.P Name :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCPNM" runat="server" Width="100%" TabIndex="8" CssClass="form-control input-sm"
                                    OnTextChanged="txtCPNM_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">
                                <b>C.P No :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCPNO" runat="server" Width="100%" TabIndex="9" CssClass="form-control input-sm"
                                    OnTextChanged="txtCPNO_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <b>Remarks :</b>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TabIndex="10" TextMode="MultiLine"
                                    CssClass="form-control input-sm" OnTextChanged="txtRemarks_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">
                                <b>Status :</b>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" TabIndex="11" CssClass="form-control input-sm">
                                    <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                    <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-4"></div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">
                                <%--<strong>Are you Sure to</strong>--%>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSave" runat="server" Width="100%" Font-Bold="True" Font-Italic="True"
                                    TabIndex="12" Text="Save" CssClass="form-control btn-primary" OnClick="btnSave_Click" />
                            </div>
                            <div class="col-md-2">
                                <%--<asp:Button ID="btnSavePrint" runat="server" Font-Bold="True" CssClass="form-control btn-primary"
                                    Font-Italic="True" Text="Save &amp; Print" TabIndex="13" />--%>
                                <asp:Button ID="btnRefresh" runat="server" Width="100%" Font-Bold="True" Font-Italic="True" CssClass="form-control btn-primary"
                                    Text="Refresh" OnClick="btnRefresh_Click" />
                            </div>
                            <div class="col-md-3"></div>
                        </div>

                        <%--<div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2"></div>
                    <div class="col-md-5"></div>
                </div>--%>
                    </div>
                    <!-- Content End From here -->
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnSavePrint"></asp:PostBackTrigger>--%>
            <asp:PostBackTrigger ControlID="btnSave"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnRefresh"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
