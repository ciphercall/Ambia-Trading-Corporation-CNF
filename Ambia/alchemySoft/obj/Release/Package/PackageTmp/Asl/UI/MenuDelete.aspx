<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="MenuDelete.aspx.cs" Inherits="alchemySoft.Asl.UI.MenuDelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <script type="text/javascript">
        $(document).ready(function () {
            Search_Module();
            Search_MenuName();
            $("#<%=lblMsg.ClientID%>").fadeOut(20000);
            $('.ui-autocomplete').click(function () {
                __doPostBack();
            });
            $("#<%=txtModuleName.ClientID %>,#<%=txtMenuName.ClientID %>").keydown(function (e) {
                if (e.which == 9 || e.which == 13)
                    window.__doPostBack();
            });
        });

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
                select: function () {
                    __doPostBack('#txtMenuName');
                    return true;
                }
            });
        }
        function Search_MenuName() {
            $("#<%=txtMenuName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListMenuNameByType",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtMenuName.ClientID %>").val() + "'}",
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
                select: function () {
                    __doPostBack('#txtMenuLink');
                    return true;
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
                        <li><a href="MenuCreate.aspx"><i class="fa fa-plus"></i>Add Menu</a>
                        </li>
                        <li><a href="MenuUpdate.aspx"><i class="fa fa-edit"></i>Edit Menu</a>
                        </li>
                        <li><a href="MenuDelete.aspx"><i class="fa fa-times"></i>Delete Menu</a>
                        </li>

                    </ul>
                </div>
                <!-- end logout option -->
            </div>
            <!-- content header end -->


            <div class="content_wrapper">
                <div class="col-md-12">
                    <div class="panel panel-primary ">
                        <!-- Default panel contents -->
                        <div class="panel-heading">Delete Menu</div>
                        <br />
                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <strong>Module Name :</strong>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtModuleName" CssClass="form-control input-sm" AutoPostBack="True" OnTextChanged="txtModuleName_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-4"></div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <strong>Menu Type :</strong>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlMenuType" CssClass="form-control input-sm" AutoPostBack="True" OnSelectedIndexChanged="ddlMenuType_SelectedIndexChanged">
                                    <asp:ListItem Value="S">--Select Menu Type--</asp:ListItem>
                                    <asp:ListItem Value="F">Form</asp:ListItem>
                                    <asp:ListItem Value="R">Report</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <strong>Menu Name :</strong>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtMenuName" CssClass="form-control input-sm" AutoPostBack="True" OnTextChanged="txtMenuName_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-4"></div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-12 text-center">
                                <strong>
                                    <asp:Label runat="server" ID="lblMsg" ForeColor="red" Visible="False"></asp:Label></strong>
                            </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-5"></div>
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnDeleteMenu" Text="Delete" CssClass="form-control input-sm btn-primary" OnClick="btnDeleteMenu_Click" />
                            </div>
                            <div class="col-md-5"></div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>
    <asp:Label runat="server" ID="lblModuleID" Visible="False"></asp:Label>
    <asp:Label runat="server" ID="lblMenuID" Visible="False"></asp:Label>

</asp:Content>
