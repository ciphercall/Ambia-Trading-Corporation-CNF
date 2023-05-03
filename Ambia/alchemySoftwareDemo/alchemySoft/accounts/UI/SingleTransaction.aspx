<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="SingleTransaction.aspx.cs" Inherits="alchemy.accounts.UI.SingleTransaction" %>

<%@ Import Namespace="DynamicMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>

    <script>
        $(document).ready(function () {
            //BindControlEvents();
        });
        function pageLoad() {
            BindControlEvents();
        }
        function BindControlEvents() {
            $("#<%=txtTransDate.ClientID %>").datepicker({
                onSelect: function () {
                    $("#<%=ddlCostPool.ClientID %>").focus();
                }, dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10"
            });
            $("#<%=txtChequeDate.ClientID %>").datepicker({
                onSelect: function () {
                    $("#<%=txtAmount.ClientID %>").focus();
                }, dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10"
            });

            GetCompletionListtDebitSingleVoucherNew();
            GetCompletionListCreditSingleVoucherNew();


        }
        function printclick() {
            var debit = $("#<%=txtCNDebitNm.ClientID %>").val();
            var credit = $("#<%=txtCNCreditNm.ClientID %>").val();
            var amount = $("#<%=txtAmount.ClientID %>").val();
            var trans = $("#<%=ddlTransMode.ClientID %>").val();
            var cheque = $("#<%=txtCheque.ClientID %>").val();
            var chequedate = $("#<%=txtChequeDate.ClientID %>").val();

            if (debit === "") {
                $("#<%=txtCNDebitNm.ClientID %>").focus();
                return false;
            }
            else if (credit === "") {
                $("#<%=txtCNCreditNm.ClientID %>").focus();
                return false;
            }
            else if (amount === "") {
                $("#<%=txtAmount.ClientID %>").focus();
                return false;
            }
            else if (trans === "CASH CHEQUE" && cheque === "") {
                $("#<%=txtCheque.ClientID %>").focus();
                return false;
            }
            else if (trans === "CASH CHEQUE" && chequedate === "") {
                $("#<%=txtChequeDate.ClientID %>").focus();
                    return false;
                }
                else {
                    setTimeout(function print() { window.open('../Report/Report/RptCreditVoucher.aspx', '_blank'); }, 1000);
                    return true;
                }
}


function GetCompletionListtDebitSingleVoucherNew() {
    $("#<%=txtCNDebitNm.ClientID %>").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "../../search.asmx/GetCompletionListtDebitSingleVoucherNew",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'txt' : '" + $("#<%=txtCNDebitNm.ClientID %>").val() + "','transtype' : '" + $("#<%=ddlTransType.ClientID %>").val() + "'}",
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
            $("#<%=txtDebited.ClientID %>").val(ui.item.x);
            $("#<%=txtCNCreditNm.ClientID %>").focus();
            return true;
        }
    });
}
function GetCompletionListCreditSingleVoucherNew() {
    $("#<%=txtCNCreditNm.ClientID %>").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "../../search.asmx/GetCompletionListCreditSingleVoucherNew",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'txt' : '" + $("#<%=txtCNCreditNm.ClientID %>").val() + "','transtype' : '" + $("#<%=ddlTransType.ClientID %>").val() + "'}",
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
            $("#<%=txtCredited.ClientID %>").val(ui.item.x);
            var trmode = $("#<%=ddlTransMode.ClientID %>").val();
            if (trmode === "CASH CHEQUE" || trmode === "A/C PAYEE CHEQUE")
                $("#<%=txtCheque.ClientID %>").focus();
            else
                $("#<%=txtAmount.ClientID %>").focus();
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Single Transaction Entry</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu"> 
                                <% if (dbFunctions.checkParmit("/Accounts/UI/SingleTransaction.aspx", "UPDATER") == true)
                                   { %>
                                <li><a href="EditSingleVoucher.aspx"><i class="fa fa-edit"></i>Edit</a>
                                </li>
                                <% } %> 
                            </ul>
                        </div>
                        <!-- end logout option -->


                    </div>
                    <!-- content header end -->


                    <!-- Content Start From here -->
                    <div class="form-class">
                     
                        <div class="row form-class">
                            <div class="col-md-2">Transaction Type :</div>
                            <div class="col-md-3">
                                <asp:DropDownList runat="server" ID="ddlTransType" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged" CssClass="form-control input-sm">
                                    <asp:ListItem Value="MPAY">PAYMENT</asp:ListItem>
                                    <asp:ListItem Value="MREC">RECEIPT</asp:ListItem>
                                    <asp:ListItem Value="JOUR">JOURNAL</asp:ListItem>
                                    <asp:ListItem Value="CONT">CONTRA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">Voucher No :</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtVouchNo" runat="server" ReadOnly="True" CssClass="form-control input-sm text-center"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">Transaction Date :</div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtTransDate" OnTextChanged="txtTransDate_TextChanged"
                                    AutoPostBack="True" CssClass="form-control input-sm text-center"></asp:TextBox>
                            </div>
                            <div class="col-md-3">Month-Year</div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="txtTransYear" ReadOnly="True" CssClass="form-control input-sm text-center"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <%--<asp:TextBox runat="server" ID="txtFiscalYear" ReadOnly="True" CssClass="form-control input-sm text-center"></asp:TextBox>--%>
                            </div>
                        </div>

                        <hr />

                        <div class="row form-class">
                            <div class="col-md-2">Unit Name : </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransFor" runat="server" OnSelectedIndexChanged="ddlTransFor_SelectedIndexChanged"
                                    CssClass="form-control input-sm" AutoPostBack="True" Visible="False">
                                    <%--<asp:ListItem>Select</asp:ListItem>--%>
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                    <asp:ListItem>OFFICIAL</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlCostPool" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-md-2">Transaction Mode :</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransMode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransMode_SelectedIndexChanged"
                                    CssClass="form-control input-sm">
                                    <asp:ListItem>CASH</asp:ListItem>
                                    <asp:ListItem>CASH CHEQUE</asp:ListItem>
                                    <asp:ListItem>A/C PAYEE CHEQUE</asp:ListItem>
                                    <asp:ListItem>ONLINE TRANSFER</asp:ListItem>
                                    <asp:ListItem>PAY ORDER</asp:ListItem>
                                    <asp:ListItem>ATM</asp:ListItem>
                                    <asp:ListItem>D.D.</asp:ListItem>
                                    <asp:ListItem>T.T.</asp:ListItem>
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div class="row form-class">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                                :
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCNDebitNm" runat="server" required="" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:TextBox ID="txtDebited" Style="max-width: 150px; display: none" runat="server"
                                    CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                                : 
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCNCreditNm" runat="server" required="" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:TextBox ID="txtCredited" Style="max-width: 150px; display: none" runat="server"
                                    CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>


                        <div class="row form-class">
                            <div class="col-md-2">
                                <asp:Label runat="server" ID="lblChequeNo" Text="Cheque No :" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCheque" runat="server" OnTextChanged="txtCheque_TextChanged"
                                    CssClass="form-control input-sm" Visible="False"></asp:TextBox>

                                <asp:Label ID="lblCheque" runat="server" ForeColor="#FF3300" Text="Label" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label runat="server" ID="lblChequeDate" Text="Cheque Date :" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtChequeDate" runat="server" Visible="False" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:Label ID="lblChequeDT" runat="server" ForeColor="#FF3300" Text="Label" Visible="False"></asp:Label>
                            </div>
                        </div>


                        <div class="row form-class">
                            <div class="col-md-2">Amount :</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAmount" runat="server" required="" OnTextChanged="txtAmount_TextChanged" AutoPostBack="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">In Words :</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtInwords" runat="server" TextMode="MultiLine"
                                    ReadOnly="True" OnTextChanged="txtInwords_TextChanged" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2">Remarks :</div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="80px" MaxLength="300"
                                    OnTextChanged="txtRemarks_TextChanged" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-12 text-center">
                                <asp:Label ID="lblErrMSg" runat="server" ForeColor="#CC0000"
                                    Text="lblErrMSg" Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-3"></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" Max-width="200px"
                                    OnClick="btnSave_Click" CssClass="form-control  btn-primary input-sm" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="Button1" runat="server" OnClientClick="printclick()"
                                    Text="Save &amp; Print" OnClick="Button1_Click" Max-width="200px"
                                    CssClass="form-control btn-primary input-sm" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnRefresh" Max-width="200px" OnClick="btnRefresh_Click" Text="Reset" CssClass="form-control  btn-primary input-sm" />
                            </div>
                            <div class="col-md-3"></div>
                        </div>

                        <!-- Content End From here -->


                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="Button1" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <asp:Label ID="lblVCount" runat="server" Visible="False" Text="Label"></asp:Label>
</asp:Content>
