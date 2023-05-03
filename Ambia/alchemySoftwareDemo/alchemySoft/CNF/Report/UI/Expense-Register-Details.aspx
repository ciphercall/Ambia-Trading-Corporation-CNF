<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Expense-Register-Details.aspx.cs" Inherits="DynamicMenu.CNF.Report.UI.Expense_Register_Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            BindControlEvents();
        });
        function BindControlEvents() {
            GetCompletionListAccountHeadWithId();
            $(function () {
                $("#<%=txtFromDate.ClientID%>").datepicker({
                    defaultDate: "",
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "-10:+10",
                    onClose: function (selectedDate) {
                        $("#<%=txtToDate.ClientID%>").datepicker("option", "minDate", selectedDate);
                    }
                });
                $("#<%=txtToDate.ClientID%>").datepicker({
                    defaultDate: "",
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "-10:+10",
                    onClose: function (selectedDate) {
                        $("#<%=txtFromDate.ClientID%>").datepicker("option", "maxDate", selectedDate);
                    }
                });
            });
        }
        function GetCompletionListAccountHeadWithId() {
            $("#<%=txtExpenseNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../../search.asmx/GetCompletionListAccountHeadWithId",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtExpenseNM.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.label,
                                    value: item.label,
                                    compid: item.value
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
                    $("#<%=txtExpenseID.ClientID%>").val(ui.item.compid);
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
                <h1>Expense Register Details</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div>
            <!-- content header end -->
            <!-- Content Start From here -->
            <div class="content_wrapper col-md-12">

                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2 text-right"><strong>Expense Head	</strong></div>
                    <div class="col-md-5">
                        <asp:TextBox runat="server" ID="txtExpenseNM" CssClass="form-control input-sm"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtExpenseID" Style="display: none"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2 text-right"><strong>From</strong></div>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-md-1"><strong>To</strong></div>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class text-center text-danger">
                    <strong>
                        <asp:Label runat="server" ID="lblErrmsg"></asp:Label></strong>
                </div>
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="form-control input-sm btn-warning" OnClick="btnSubmit_Click" />
                    </div>
                    <div class="col-md-5"></div>

                </div>

            </div>
            <!-- Content End From here -->
        </div>
    </div>
</asp:Content>
