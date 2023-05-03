<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="DrillDownReport.aspx.cs" Inherits="DynamicMenu.Accounts.Report.UI.BalanceSheetDrillDownReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindControlEvents();
        });

        function BindControlEvents() {
            $("#txtFrom").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });

            Search_GetCompletionListLedgerBook();
          <%--  $('.ui-autocomplete').click(function () {
                __doPostBack();
            });
            $("#<%=txtHeadNM.ClientID %>").keydown(function (e) {
                if (e.which == 9 || e.which == 13)
                    window.__doPostBack();
            });--%>
        }


        function Search_GetCompletionListLedgerBook() {
            $("#<%=txtHeadNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../../search.asmx/GetCompletionListLedgerBookGeneral",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("#<%=txtHeadNM.ClientID %>").val() + "'}",
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
                    $("#<%=txtacccd.ClientID%>").val(ui.item.x);
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
                <h1>LEDGER HISTORY YEARLY</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->




            </div>
            <!-- content header end -->


            <!-- Content Start From here -->
            <div class="form-class">

                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">Head Name</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtHeadNM" runat="server" CssClass="form-control input-sm" TabIndex="1"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtacccd" style="display: none"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">Date</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control input-sm"
                            ClientIDMode="Static" TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>

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
