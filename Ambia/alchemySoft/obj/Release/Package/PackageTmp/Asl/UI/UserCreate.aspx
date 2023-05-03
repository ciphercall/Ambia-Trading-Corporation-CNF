<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="UserCreate.aspx.cs" Inherits="alchemySoft.Asl.UI.UserCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link href="../../MenuCssJs/ui-lightness/jquery.ui.theme.css" rel="stylesheet" />
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <link href="../../MenuCssJs/bootstrap-clock-picker/src/clockpicker.css" rel="stylesheet" />

    <link href="../../MenuCssJs/bootstrap-clock-picker/src/standalone.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <script src="../../MenuCssJs/bootstrap-clock-picker/src/clockpicker.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            BindControlEvents();
        });

        function BindControlEvents() {
            Search_CompanyName();
            $("#<%=lblMsg.ClientID%>").fadeOut(10000);
            $('.ui-autocomplete').click(function () {
                __doPostBack();
            });
            $("#<%=txtCompanyName.ClientID %>").keydown(function (e) {
                if (e.which == 9 || e.which == 13)
                    window.__doPostBack();
            });
            $('.clockpicker').clockpicker({
                placement: 'top',
                align: 'left',
                donetext: 'Done'
            });
        }

        function Search_CompanyName() {
            $("#<%=txtCompanyName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListCompany",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtCompanyName.ClientID %>").val() + "'}",
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
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate> 
            <div class="col-md-10 pull-right " id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Create User</h1>

                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <li>
                                    <asp:LinkButton runat="server" ID="LinkBAdd" CssClass="fa fa-plus" Text="Add record" OnClick="LinkBAdd_Click" />
                                </li>
                                <li><%--<a href="#"><i class="fa fa-edit"></i>Edit record</a>--%>
                                    <asp:LinkButton runat="server" ID="linkBEdit" CssClass="fa fa-edit" Text="Edit record" OnClick="linkBEdit_Click" />
                                </li>
                            </ul>
                        </div>
                        <!-- end logout option --> 
                    </div>
                    <!-- content header end --> 
                    <!-- Content Write From Here-->
                    <div class="content_wrapper">
                        <div class="col-md-12"> 
                            <div class="row form-class">
                                <div class="col-md-2"><strong>Comapny Name</strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtCompanyName" Width="100%" AutoPostBack="True" MaxLength="100" CssClass="form-control auto-complete" ClientIDMode="Static" OnTextChanged="txtCompanyName_TextChanged"></asp:TextBox>
                                </div>
                                <div class="col-md-2">Branch</div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlBranch" Width="100%" AutoPostBack="True" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" />
                                </div>
                            </div> 
                            <div class="row form-class">
                                <div class="col-md-2 text-left"><strong>User Name<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtUserName" MaxLength="100" Width="100%" CssClass="form-control input-sm"></asp:TextBox>
                                    <asp:DropDownList runat="server" Visible="False" ID="ddlUserName" Width="100%" CssClass="form-control input-sm" AutoPostBack="True" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged" />
                                </div>
                                <div class="col-md-2 text-left"><strong>Department Name<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtDepartmentName" Width="100%" MaxLength="50" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2 text-left"><strong>Operation Type<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">

                                    <asp:DropDownList runat="server" ID="ddlOpType" Width="100%" CssClass="form-control input-sm">
                                        <asp:ListItem Value="COMPADMIN">Company Admin</asp:ListItem>
                                        <asp:ListItem Value="USERADMIN">User Admin</asp:ListItem>
                                        <asp:ListItem Value="USER">User</asp:ListItem>
                                    </asp:DropDownList>

                                </div>
                                <div class="col-md-2 text-left"><strong>User Address<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtUserAdd" Width="100%" MaxLength="100" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2 text-left"><strong>Mobile No.<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtMobileNo" Width="100%" MaxLength="11" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-left"><strong>Email Address<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtEmailId" Width="100%" MaxLength="50" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2 text-left"><strong>Login By<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlLogInBy" Width="100%" AutoPostBack="True" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlLogInBy_SelectedIndexChanged">
                                        <asp:ListItem Value="">--Select Login By--</asp:ListItem>
                                        <asp:ListItem Value="EMAIL">Email</asp:ListItem>
                                        <asp:ListItem Value="MOBNO">Mobile</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 text-left"><strong>Login ID<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtlogInId" Width="100%" MaxLength="50" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2 text-left"><strong>Password<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtPassword" Width="100%" MaxLength="20" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-left"><strong>Status<span class="red-color">*</span></strong></div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlStatus" Width="100%" CssClass="form-control input-sm">
                                        <asp:ListItem Value="">--Select Status--</asp:ListItem>
                                        <asp:ListItem Value="A">Active</asp:ListItem>
                                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2 text-left"><strong>Start Time</strong></div>
                                <div class="col-md-4">
                                    <div class="input-group input-group-sm clockpicker">
                                        <asp:TextBox ID="txtStartTime" class="form-control" Width="100%" value="10:00" MaxLength="10" aria-describedby="sizing-addon3" runat="server"></asp:TextBox>
                                        <span class="input-group-addon" id="sizing-addon3">
                                            <samp class="glyphicon glyphicon-time"></samp>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-2 text-left"><strong>End Time</strong></div>
                                <div class="col-md-4">
                                    <div class="input-group input-group-sm clockpicker">
                                        <asp:TextBox ID="txtEndTime" class="form-control" Width="100%" value="18:00" MaxLength="10" aria-describedby="sizing-addon4" runat="server"></asp:TextBox>
                                        <span class="input-group-addon" id="sizing-addon4">
                                            <samp class="glyphicon glyphicon-time"></samp>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row form-class" style="margin-top:5px">
                                <div class="col-md-9">
                                     <asp:Label runat="server" CssClass="alert alert-warning"  ID="lblMsg" Visible="False"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Button runat="server" Width="100%"  ID="btnSubmit" CssClass="form-control input-sm btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                                    <asp:Button runat="server" Width="100%"  ID="btnUpdate" Visible="False" CssClass="form-control input-sm btn-primary" Text="Update" OnClick="btnUpdate_Click" />
                                </div>
                            </div>
                        </div>
                        <!-- Content Write From Here-->
                    </div>
                </div>
                <!-- content box end here -->
            </div>
            <!-- main content end here -->
            <asp:Label runat="server" ID="lblCompanyId" Visible="False"></asp:Label>
            <asp:Label runat="server" ID="lblBranchCd" Visible="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
