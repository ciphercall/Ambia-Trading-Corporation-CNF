<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="MenuRole.aspx.cs" Inherits="alchemySoft.Asl.UI.MenuRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>

    <script type="text/javascript">

        function pageLoad() {
            $('.select2').select();
            Search_Module();
            Search_CompanyName();
            $('#paneldown').hide();
            $('#panelbody').show();
            $("#<%=lblMsg.ClientID%>, #<%=lblMsg.ClientID%>").fadeOut(20000);
            //$('.ui-autocomplete').click(function () {
            //    __doPostBack();
            //});
            //$('.ui-autocomplete').select(function () {
            //    __doPostBack();
            //});
        }

        <%--function Search_Module() {
            $("#<%=txtUserName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListUserNameForMenuRole",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtUserName.ClientID %>").val() + "'}",
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

        function Search_CompanyName() {
            $("#<%=txtComapnyAdminName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListCompany",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtComapnyAdminName.ClientID %>").val() + "'}",
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
        }--%>
    </script>
    <style>
        span.crosshair {
            cursor: pointer;
        }

        .ui-autocomplete {
            max-width: 350px;
            max-height: 250px;
            overflow: auto;
        }
    </style>
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
                            </ul>
                        </div>
                        <!-- end logout option -->


                    </div>
                    <!-- content header end -->

                    <!-- Content Start From here -->
                    <div class="content_wrapper">
                        <div class="col-md-12">
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">
                                    <strong>
                                        <asp:Label runat="server" ID="lblUserlabel" Text="User Name :" Visible="False"></asp:Label>
                                        <asp:Label runat="server" ID="lblUserlabel1" Text="Comapny Name :" Visible="False"></asp:Label>
                                    </strong>
                                </div>
                                <div class="col-md-4">
                                    <%-- <asp:TextBox runat="server" ID="txtUserName" Width="100%" CssClass="form-control input-sm" TabIndex="1"
                                AutoPostBack="True" OnTextChanged="txtUserName_TextChanged"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtComapnyAdminName" Width="100%" CssClass="form-control input-sm" TabIndex="1"
                                AutoPostBack="True" OnTextChanged="txtComapnyAdminName_TextChanged"></asp:TextBox>--%>
                                    <asp:DropDownList runat="server" ID="ddlUserCompanyName" Width="100%" CssClass="form-control select2" TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2"><strong>Module Name :</strong></div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlModuleName" Width="100%" CssClass="form-control select2" TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2"><strong>Form Type :</strong></div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlMenuType" Width="100%" TabIndex="3" CssClass="form-control input-sm">
                                        <asp:ListItem Value="S">--Select Form Type--</asp:ListItem>
                                        <asp:ListItem Value="F">Form</asp:ListItem>
                                        <asp:ListItem Value="R">Report</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button runat="server" TabIndex="4" Width="100%" ID="btnSubmit" Text="Submit" CssClass="form-control input-sm btn-primary" OnClick="btnSubmit_Click"></asp:Button>
                                </div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-12 text-left">
                                    <asp:Label runat="server" Width="100%" ID="lblMsg" Visible="False"></asp:Label>
                                </div>
                            </div>

                            <div class="form-class">
                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        <div class="container-fluid">
                                            <ul class="nav navbar-nav navbar-left">
                                                <li>Menu Role List</li>
                                            </ul>
                                            <ul class="nav navbar-nav navbar-right">
                                                <li id="paneldown"><span class="glyphicon glyphicon-chevron-down crosshair"></span></li>
                                                <li id="panelup"><span class="glyphicon glyphicon-chevron-up crosshair"></span></li>
                                            </ul>
                                        </div>

                                    </div>
                                    <div class="panel-body" id="panelbody">
                                        <div class="table table-responsive table-hover gridRoot">
                                            <asp:GridView ID="gridViewAslRole" runat="server" AutoGenerateColumns="False" CssClass="Grid" AllowPaging="True" PageSize="10"
                                                OnSorting="gridViewAslRole_Sorting"
                                                OnPageIndexChanging="gridViewAslRole_PageIndexChanging"
                                                OnRowUpdating="gridViewAslRole_RowUpdating" ShowFooter="False" Width="100%"
                                                OnRowEditing="gridViewAslRole_RowEditing" OnRowCancelingEdit="gridViewAslRole_RowCancelingEdit">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="CompanyId" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCompanyId" runat="server" Text='<%#Eval("COMPID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblCompanyIdEdit" runat="server" Text='<%#Eval("COMPID") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="UserId" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("USERID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblUserIdEdit" runat="server" Text='<%#Eval("USERID") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ModuleId" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModuleId" runat="server" Text='<%#Eval("MODULEID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblModuleIdEdit" runat="server" Text='<%#Eval("MODULEID") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="MenuId" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMenuId" runat="server" Text='<%#Eval("MENUID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblMenuIdEdit" runat="server" Text='<%#Eval("MENUID") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Module Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModuleName" runat="server" Text='<%#Eval("MODULENM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblModuleNameEdit" runat="server" Text='<%#Eval("MODULENM") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtModuleName" runat="server" TabIndex="5" MaxLength="100" CssClass="form-control  input-sm" Font-Names="Calibri" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Menu Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMenuName" runat="server" Text='<%#Eval("MENUNM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblMenuNameEdit" runat="server" Text='<%#Eval("MENUNM") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtMenuName" runat="server" TabIndex="5" MaxLength="100" CssClass="form-control  input-sm" Font-Names="Calibri" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="32%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="32%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("STATUS") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlStatusEdit" CssClass="form-control input-sm">
                                                                <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                                <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtStatus" runat="server" TabIndex="11" MaxLength="100" CssClass="form-control  input-sm" Font-Names="Calibri" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Insert">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsert" runat="server" Text='<%#Eval("INSERTR") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlInsertEdit" TabIndex="12" CssClass="form-control input-sm">
                                                                <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                                <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtUpdate" runat="server" MaxLength="100" CssClass="form-control  input-sm" Font-Names="Calibri" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUpdate" runat="server" Text='<%#Eval("UPDATER") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlUpdateEdit" TabIndex="13" CssClass="form-control input-sm">
                                                                <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                                <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtUpdate" runat="server" TabIndex="6" MaxLength="100" CssClass="form-control  input-sm" Font-Names="Calibri" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDelete" runat="server" Text='<%#Eval("DELETER") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlDeleteEdit" TabIndex="14" CssClass="form-control input-sm">
                                                                <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                                <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtDelete" runat="server" TabIndex="6" MaxLength="100" CssClass="form-control  input-sm" Font-Names="Calibri" />
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <%--<FooterTemplate>
                                                <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon" Height="30px" ImageUrl="~/Images/AddNewitem.png" TabIndex="7" ToolTip="Save" Width="20px" />
                                            </FooterTemplate>--%>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnedit" runat="server" CommandName="Edit" Height="20px" ImageUrl="~/Images/Edit.png" OnClientClick="return confMSG()" TabIndex="110" ToolTip="Delete" Width="20px" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/Images/update.png"
                                                                ToolTip="Update" Height="20px" Width="20px" TabIndex="15" />
                                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/Cancel.png"
                                                                ToolTip="Cancel" Height="20px" Width="20px" TabIndex="16" />
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Content End From here -->

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label runat="server" ID="lblModuleId" Visible="False"></asp:Label>
    <asp:Label runat="server" ID="lblCompanyUserId" Visible="False"></asp:Label>
    <link href="../../MenuCssJs/css/select2.min.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/select2.min.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $('#paneldown').hide();
            $('#paneldown').click(function () {
                $('#panelbody').show(700);
                $('#paneldown').hide();
                $('#panelup').show();
            });
            $('#panelup').click(function () {
                $('#panelbody').hide(700);
                $('#panelup').hide();
                $('#paneldown').show();
            });
        }
    </script>

</asp:Content>
