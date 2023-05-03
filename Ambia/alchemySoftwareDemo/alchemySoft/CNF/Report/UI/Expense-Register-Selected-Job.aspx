<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Expense-Register-Selected-Job.aspx.cs" Inherits="alchemySoft.CNF.Report.UI.Expense_Register_Selected_Job" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <script type="text/javascript">
        $(document).ready(function () {
            BindControlEvents();
        });
        function BindControlEvents() {
            GetCompletionListJobYear();
        }
        function GetCompletionListJobYear() {
            $("#<%=txtJobID.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../../search.asmx/GetCompletionListJobYear",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtJobID.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                var str = item;
                                var res = str.split("|");
                                return {
                                    label: item,
                                    value: res[0]
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
                    var str = ui.item.label;
                    var res = str.split("|");
                    $("#<%=txtJobID.ClientID %>").val(res[0]);
                    $("#<%=txtJobYear.ClientID %>").val(res[1]);
                    $("#<%=txtJobType.ClientID %>").val(res[2]);
                    $("#<%=btnSubmit.ClientID %>").focus();
                    return true;
                }
            });
        }
    </script>
    <style>
        .ui-autocomplete {
            width: 250px;
            max-height: 250px;
            overflow: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Expense Register</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div>
            <!-- content header end -->
            <!-- Content Start From here -->
            <div class="content_wrapper col-md-12">

                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2 text-right"><strong>Job No & Year	</strong></div>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" ID="txtJobID" CssClass="form-control input-sm text-center" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <asp:TextBox runat="server" ID="txtJobYear" CssClass="form-control input-sm text-center" oNfocus="blur()"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" ID="txtJobType" CssClass="form-control input-sm text-center" oNfocus="blur()"></asp:TextBox>
                    </div>
                    <div class="col-md-3"></div>
                </div>
                <div class="row form-class text-center text-danger">
                    <strong>
                        <asp:Label runat="server" ID="lblErrmsg"></asp:Label></strong>
                </div>
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="form-control input-sm btn-warning"
                             OnClick="btnSubmit_Click" TabIndex="2"/>
                    </div>
                    <div class="col-md-5"></div>

                </div>

            </div>
            <!-- Content End From here -->
        </div>
    </div>
</asp:Content>
