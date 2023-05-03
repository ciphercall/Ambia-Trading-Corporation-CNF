<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="RptNotesAcc.aspx.cs" Inherits="DynamicMenu.Accounts.Report.UI.RptNotesAcc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindControlEvents();
        });

        function BindControlEvents() {
            $("#<%=txtFrom.ClientID%>").datepicker({
                defaultDate: "",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                yearRange: "-10:+10",
                onClose: function (selectedDate) {
                    $("#txtTo").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#<%=txtTo.ClientID%>").datepicker({
                defaultDate: "",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                yearRange: "-10:+10",
                onClose: function (selectedDate) {
                    $("#txtFrom").datepicker("option", "maxDate", selectedDate);
                }
            });
            Search_GetCompletionListNotesAccount();
        }
        function Search_GetCompletionListNotesAccount() {
            $("#<%=txtHeadNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../../search.asmx/GetCompletionListNotesAccount",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtHeadNM.ClientID %>").val() + "','lableCode' : '" + $("#<%=ddlLevelID.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.label,
                                    value: item.label,
                                    x: item.value
                                };
                            }));
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    $("#<%=txtAccHeadCD.ClientID %>").val(ui.item.x);
                    $("#<%=txtFrom.ClientID %>").focus();
                    return true;
                }
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


    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Notes To The Accounts</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->




            </div>
            <!-- content header end -->


            <!-- Content Start From here -->
            <br />
          <div class="content_wrapper  col-md-12">
                <asp:UpdatePanel runat="server" ID="upd1">
                    <ContentTemplate>

                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">Type</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLevelID" runat="server" CssClass="form-control input-sm"
                                    OnSelectedIndexChanged="ddlLevelID_SelectedIndexChanged"
                                    AutoPostBack="True" TabIndex="1">
                                    <asp:ListItem>SELECT</asp:ListItem>
                                    <asp:ListItem Value="1">ASSET</asp:ListItem>
                                    <asp:ListItem Value="2">LIABILITY</asp:ListItem>
                                    <asp:ListItem Value="3">INCOME</asp:ListItem>
                                    <asp:ListItem Value="4">EXPENDETURE</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4"></div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">Head Name</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHeadNM" runat="server" CssClass="form-control input-sm" TabIndex="1"></asp:TextBox>
                                <asp:TextBox ID="txtAccHeadCD" runat="server" Style="display: none"></asp:TextBox>
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">From</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control input-sm" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">To</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control input-sm" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-4"></div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
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
