<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="script_.aspx.cs" Inherits="alchemySoft.Asl.UI.script_" %>

<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <%--<script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>--%>


    <script type="text/javascript">
        function pageLoad() {
            Search_Module();
            Search_MenuName();
            $("#<%=lblMsg.ClientID%>, #<%=lblMsgMenu.ClientID%>,#<%=lblGridMSG.ClientID%>").fadeOut(20000);
            $("#<%=lblMsg.ClientID%>, #<%=lblMsgMenu.ClientID%>,#<%=lblGridMSG.ClientID%>").text = "";

        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) { }
            else { return false; }
        }
        function Search_Module() {
            $("#<%=txtModuleName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListModuleName",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtModuleName.ClientID %>").val() + "'}",
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
        function Search_MenuName() {
            $("[id*=txtMenuName],[id*=txtMenuNameEdit]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListMenuName",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtMenuName],[id*=txtMenuNameEdit]").val() + "'}",
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
    <%-- <style>
        .ui-autocomplete {
            max-width: 350px;
            max-height: 250px;
            overflow: auto;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>

            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Menu Information</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <li><a href="#"><i class="fa fa-plus"></i>Add Menu</a>
                                </li>
                                <li><a href="#"><i class="fa fa-edit"></i>Edit Menu</a>
                                </li>
                                <li><a href="#"><i class="fa fa-times"></i>Delete Menu</a>
                                </li>

                            </ul>
                        </div>
                        <!-- end logout option -->


                    </div>
                    <!-- content header end -->


                    <div class="content_wrapper">
                        <div class="col-md-12">
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">
                                    <strong>Generate Script :</strong>
                                </div> 
                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnSubmit" Width="100%" TabIndex="2" Text="Submit" CssClass="form-control input-sm btn-primary" OnClick="btnSubmit_Click" />

                                </div>


                                <div class="row form-class">
                                    <div class="col-md-12 text-left">
                                        <strong>
                                            <asp:Label runat="server" Width="100%" ID="lblMsg" Visible="False"></asp:Label></strong>
                                    </div>
                                </div>
                                <div class="row text-center">
                                    <asp:Label runat="server" ID="lblMsgMenu" ForeColor="red"></asp:Label>
                                </div>
                                <div class="table table-responsive table-hover gridRoot">
                                   
                                </div>
                                <div class="text-left">
                                    <asp:Label ID="lblGridMSG" Width="100%" runat="server" ForeColor="#CC0000" CssClass="alert alert-success" Visible="False"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
