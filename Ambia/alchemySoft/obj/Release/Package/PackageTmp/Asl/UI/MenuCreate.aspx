<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="MenuCreate.aspx.cs" Inherits="alchemySoft.Asl.UI.MenuCreate" %>

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
                                    <strong>Module Name :</strong>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtModuleName" TabIndex="1"
                                        CssClass="form-control input-sm" Width="100%" AutoPostBack="True" OnTextChanged="txtModuleName_TextChanged"></asp:TextBox>
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
                                    <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="Grid" Width="100%"
                                        OnRowCommand="gvDetails_RowCommand" OnRowDataBound="gvDetails_RowDataBound" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                                        OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating" OnRowDeleting="gvDetails_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Module Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModuleId" runat="server" Text='<%# Eval("MODULEID") %>' Width="100%"
                                                        Style="text-align: center" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblModuleIdEdit" runat="server" Text='<%#Eval("MODULEID") %>' Width="100%"
                                                        Style="text-align: center" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Menu Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMenuId" runat="server" Text='<%# Eval("MENUID") %>' Width="100%"
                                                        Style="text-align: center" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblMenuIdEdit" runat="server" Text='<%#Eval("MENUID") %>' Width="100%"
                                                        Style="text-align: center" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Menu Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMenuType" runat="server" Text='<%# Eval("MENUTP") %>' Width="100%" Style="text-align: left" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlMenuTypeEdit" Width="100%" TabIndex="21" CssClass="form-control input-sm">
                                                        <asp:ListItem Value="SELECT">--SELECT--</asp:ListItem>
                                                        <asp:ListItem Value="F">FORM</asp:ListItem>
                                                        <asp:ListItem Value="R">REPORT</asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList runat="server" Width="100%" TabIndex="11" ID="ddlMenuType" CssClass="form-control input-sm">
                                                        <asp:ListItem Value="SELECT">--SELECT--</asp:ListItem>
                                                        <asp:ListItem Value="F">FORM</asp:ListItem>
                                                        <asp:ListItem Value="R">REPORT</asp:ListItem>
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Menu Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMenuName" runat="server" Text='<%# Eval("MENUNM") %>' Width="100%" Style="text-align: left" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtMenuNameEdit" Width="100%" TabIndex="22" runat="server" Text='<%#Eval("MENUNM") %>' ClientIDMode="Static" CssClass="form-control input-sm" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtMenuName" Width="100%" TabIndex="12" runat="server" CssClass="form-control input-sm" ClientIDMode="Static" />
                                                </FooterTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Menu Link">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMenuLink" runat="server" Text='<%# Eval("FLINK") %>' Width="100%" Style="text-align: left" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtMenuLinkEdit" Width="100%" runat="server" TabIndex="23" Text='<%#Eval("FLINK") %>' CssClass="form-control input-sm" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtMenuLink" Width="100%" runat="server" CssClass="form-control input-sm" TabIndex="13" />
                                                </FooterTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Menu Serial">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMenuSerial" runat="server" Text='<%# Eval("MENUSL") %>' Width="100%"
                                                        Style="text-align: left" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtMenuSerialEdit" Width="100%" TabIndex="24" runat="server" Text='<%#Eval("MENUSL") %>' CssClass="form-control input-sm" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtMenuSerial" Width="100%" runat="server" CssClass="form-control input-sm" TabIndex="14" />
                                                </FooterTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Execution">
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/Images/update.png"
                                                        ToolTip="Update" Height="20px" Width="20px" TabIndex="25" />
                                                    <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/Cancel.png"
                                                        ToolTip="Cancel" Height="20px" Width="20px" TabIndex="26" />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Images/Edit.png"
                                                        ToolTip="Edit" Height="20px" Width="20px" TabIndex="40" />
                                                    <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" Text="Edit" runat="server"
                                                        ImageUrl="~/Images/delete.png" ToolTip="Delete" Height="20px" Width="20px" OnClientClick="return confMSG()"
                                                        TabIndex="41" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Images/AddNewitem.png"
                                                        CommandName="AddNew" Width="30px" Height="32px" ToolTip="Add new Record" ValidationGroup="validaiton"
                                                        TabIndex="15" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" Width="8%" />
                                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                <ItemStyle HorizontalAlign="Center" Width="8%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
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
