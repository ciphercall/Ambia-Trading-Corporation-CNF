<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="ProjectExpenseStatement.aspx.cs" Inherits="DynamicMenu.Accounts.Report.UI.ProjectExpenseStatement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

        });

        function BindControlEvents() {
            $("#<%=txtFrom.ClientID %>,#<%=txtTo.ClientID %>").datepicker({dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10"
            });
            GetCompletionListCostPool();
        }
        function GetCompletionListCostPool() {
            $("#<%=txtProjectNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../../search.asmx/GetCompletionListCostPool",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtProjectNm.ClientID %>").val() + "'}",
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
                    $("#<%=txtProjectCD.ClientID %>").val(ui.item.x);
                    $("#<%=ddlType.ClientID %>").focus();
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
                <h1>OFFICE WISE TRANSACTION STATEMENT</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div>
            <!-- content header end -->
            <!-- Content Start From here -->
            <div class="form-class">

                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">Office</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtProjectNm" runat="server" TabIndex="1" CssClass="form-control input-sm"></asp:TextBox>
                        <asp:TextBox ID="txtProjectCD" runat="server" Style="display: none"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">Type</div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlType" CssClass="form-control input-sm">
                            <asp:ListItem>--SELECT--</asp:ListItem>
                            <asp:ListItem Value="MREC">RECEIPT</asp:ListItem>
                            <asp:ListItem Value="MPAY">PAYMENT</asp:ListItem>
                            <asp:ListItem Value="JOUR">JOURNAL</asp:ListItem>
                            <asp:ListItem Value="CONT">CONTRA</asp:ListItem>
                        </asp:DropDownList>
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
                    <div class="col-md-2">to</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTo" runat="server" CssClass="form-control input-sm" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            TabIndex="3" CssClass="form-control input-sm btn-primary" />
                    </div>
                    <div class="col-md-5"></div>

                </div>

            </div>
            <!-- Content End From here -->
        </div>
    </div>

</asp:Content>
