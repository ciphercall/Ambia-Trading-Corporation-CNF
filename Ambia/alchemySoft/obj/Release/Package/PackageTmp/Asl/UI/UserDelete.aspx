<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="UserDelete.aspx.cs" Inherits="alchemySoft.Asl.UI.UserDelete" %>

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
            Search_Store_fr();
            $("#<%=lblMsg.ClientID%>").fadeOut(10000);

            $('.ui-autocomplete').click(function () {
                __doPostBack();
            });
            $("#<%=txtCompanyName.ClientID %>").keydown(function (e) {
                if (e.which == 9 || e.which == 13)
                    window.__doPostBack();
            });
        });

        function Search_Store_fr() {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Create User</h1>

                <div class="btn-group pull-right" id="editOption">
                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                        <i class="fa fa-cog"></i>
                    </button>
                    <ul class="dropdown-menu pull-right" style="" role="menu">
                        <li><a href="UserCreate.aspx"><i class="fa fa-plus"></i>Add record</a>
                        </li>
                        <li><a href="#"><i class="fa fa-edit"></i>Edit record</a>
                        </li>
                        <li><a href="UserDelete.aspx"><i class="fa fa-times"></i>Delete record</a>
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
                        <div class="col-md-2"></div>
                        <div class="col-md-2">Comapny Name</div>
                        <div class="col-md-5">
                            <asp:TextBox runat="server" ID="txtCompanyName" AutoPostBack="True" MaxLength="100" CssClass="form-control auto-complete" ClientIDMode="Static" OnTextChanged="txtCompanyName_TextChanged"></asp:TextBox>
                        </div>
                        <div class="col-md-3"></div>
                    </div>
                    <div class="text-center">
                        <strong>
                            <asp:Label runat="server" ForeColor="red" ID="lblMsg" Visible="False"></asp:Label></strong>
                    </div>

                    <div class="row form-class">
                        <div class="col-md-2"></div>
                        <div class="col-md-2">User Name</div>
                        <div class="col-md-5">
                            <asp:TextBox runat="server" ID="txtUserName" AutoPostBack="True" MaxLength="100" CssClass="form-control auto-complete" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="col-md-3"></div>
                    </div>

                    <div class="row form-class">
                        <div class="col-md-4"></div>
                        <div class="col-md-4 text-center">
                            <asp:Button runat="server" ID="btnSubmit" Text="Delete" CssClass="form-control input-sm btn-primary" Width="200px" />
                        </div>
                        <div class="col-md-4"></div>
                    </div>

                </div>
                <!-- Content Write From Here-->
            </div>
        </div>s
        <!-- content box end here -->
    </div>
    <!-- main content end here -->
    <asp:Label runat="server" ID="lblCompanyId" Visible="False"></asp:Label>


</asp:Content>
