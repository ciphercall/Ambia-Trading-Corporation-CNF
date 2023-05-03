<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="SingleTransaction.aspx.cs" Inherits="alchemy.accounts.UI.SingleTransaction" %>

<%@ Import Namespace="alchemySoft" %>

<%@ Import Namespace="DynamicMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>

    <script>
        $(document).ready(function () {
            BindControlEvents();
        });
        function BindControlEvents() {
            $("#txtTransDate,#txtChequeDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });

            Search_GetCompletionListCostPool();
            Search_GetCompletionListConD();
            Search_GetCompletionListMrecC();
            Search_GetCompletionListConC();
            Search_GetCompletionListJourC();
            Search_GetCompletionListMpayC();
            Search_GetCompletionListMpayD();
            Search_GetCompletionListMrecD();
            Search_GetCompletionListJourD();
            $('.ui-autocomplete').click(function () {
                __doPostBack();
            });
            $('.ui-autocomplete').select(function () {
                __doPostBack();
            });
        };


        function Search_GetCompletionListCostPool() {
            $("#<%=txtCostPool.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/search.asmx/GetCompletionListCostPool",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtCostPool.ClientID %>").val() + "'}",
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

        function Search_GetCompletionListConD() {
            $("#<%=txtCNDebitNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListConD",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtCNDebitNm.ClientID %>").val() + "'}",
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
        function Search_GetCompletionListJourD() {
            $("#<%=txtJRDebitNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListJourD",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtJRDebitNm.ClientID %>").val() + "'}",
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
        function Search_GetCompletionListMrecD() {
            $("#<%=txtDebitNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListMrecD",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtDebitNm.ClientID %>").val() + "'}",
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
        function Search_GetCompletionListMpayD() {
            $("#<%=txtMPDebitNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListMpayD",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtMPDebitNM.ClientID %>").val() + "'}",
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
        function Search_GetCompletionListMpayC() {
            $("#<%=txtMpCreditNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListMpayC",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtMpCreditNm.ClientID %>").val() + "'}",
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
        function Search_GetCompletionListJourC() {
            $("#<%=txtJRCreditNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListJourC",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtJRCreditNm.ClientID %>").val() + "'}",
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
        function Search_GetCompletionListConC() {
            $("#<%=txtCNCreditNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListConC",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtCNCreditNm.ClientID %>").val() + "'}",
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
        function Search_GetCompletionListMrecC() {
            $("#<%=txtCreditNm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListMrecC",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtCreditNm.ClientID %>").val() + "'}",
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
    <asp:UpdatePanel ID="Up_TabContaner" runat="server">
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
                                <%--<% if (UserPermissionChecker.checkParmit("/accounts/ui/SingleTransaction.aspx", "INSERTR") == true)
                                   { %>
                                <li><a href="SingleTransaction.aspx"><i class="fa fa-plus"></i>Create</a>
                                </li>
                                <% } %>--%>

                                <% if (dbFunctions.checkParmit("/accounts/ui/SingleTransaction.aspx", "UPDATER") == true)
                                   { %>
                                <li><a href="EditSingleVoucher.aspx"><i class="fa fa-edit"></i>Edit</a>
                                </li>
                                <% } %>

                                <% if (dbFunctions.checkParmit("/accounts/ui/SingleTransaction.aspx", "DELETER") == true)
                                   { %>
                                <li><a href="EditSingleVoucher.aspx"><i class="fa fa-edit"></i>Delete</a>
                                </li>
                                <% } %>
                            </ul>
                        </div>
                        <!-- end logout option -->
                        <asp:Label ID="lblVouch" runat="server" Visible="False"></asp:Label>

                    </div>
                    <!-- content header end -->


                    <!-- Content Start From here -->
                     <div class="content_wrapper  col-md-12">
                         <br />
                        <div class="row form-class">
                            <div class="col-md-2">Transaction Type :</div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlTransType" TabIndex="1" Width="100%" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged" CssClass="form-control input-sm">
                                    <asp:ListItem Value="MPAY">PAYMENT</asp:ListItem>
                                    <asp:ListItem Value="MREC">RECEIPT</asp:ListItem>
                                    <asp:ListItem Value="JOUR">JOURNAL</asp:ListItem>
                                    <asp:ListItem Value="CONT">CONTRA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">Voucher No:</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtVouchNo" runat="server" Width="100%"  ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <%--<div class="col-md-2">
                        <a onclick="return showInFrameDialog(this,'EditSingleVoucher.aspx','Edit Single Voucher');" href="" style="font-family: Helvetica Neue,Lucida Grande,Segoe UI,Arial,Helvetica,Verdana,sans-serif; font-size: 1.2em; font-weight: bold; font-style: italic; text-decoration: none; color: #000000;">Edit</a>
                    </div>--%>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2">Transaction Date:</div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtTransDate" Width="100%"  TabIndex="2" ClientIDMode="Static" OnTextChanged="txtTransDate_TextChanged" AutoPostBack="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="txtTransYear" Width="100%"  TabIndex="3" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>

                        <hr />
                        <asp:Label ID="lblErrorMSG" runat="server" ForeColor="#FF3300" Visible="False"></asp:Label>
                        <div class="row form-class">
                            <div class="col-md-2">Transaction For: </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlTransFor" runat="server" Width="100%"  OnSelectedIndexChanged="ddlTransFor_SelectedIndexChanged"
                                    CssClass="form-control input-sm" AutoPostBack="True" Visible="true" TabIndex="4">
                                    <asp:ListItem>OFFICIAL</asp:ListItem>
                                    <asp:ListItem>OTHERS</asp:ListItem>

                                </asp:DropDownList>

                                <asp:Label ID="lblCostpoolID" runat="server" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtCostPool" runat="server" Width="100%"  OnTextChanged="txtCostPool_TextChanged" ClientIDMode="Static"
                                    CssClass="form-control input-sm" AutoPostBack="True" TabIndex="5"></asp:TextBox>
                               <%-- <asp:DropDownList ID="ddlCostPID" runat="server" CssClass="form-control input-sm" TabIndex="5">
                                </asp:DropDownList>--%>
                            </div>
                            <div class="col-md-2">Transaction Mode:</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransMode" runat="server" Width="100%"  AutoPostBack="True" OnSelectedIndexChanged="ddlTransMode_SelectedIndexChanged"
                                    CssClass="form-control input-sm" TabIndex="6">
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
                                <asp:Label ID="Label1" runat="server" Text="Label">:</asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCNDebitNm" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtCNDebitNm_TextChanged" CssClass="form-control input-sm" TabIndex="7"></asp:TextBox>
                                <asp:TextBox ID="txtJRDebitNm" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtJRDebitNm_TextChanged" CssClass="form-control input-sm" TabIndex="8"></asp:TextBox>
                                <asp:TextBox ID="txtMPDebitNM" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtMPDebitNM_TextChanged" CssClass="form-control input-sm" TabIndex="9"></asp:TextBox>
                                <asp:TextBox ID="txtDebitNm" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtDebitNm_TextChanged" CssClass="form-control input-sm" TabIndex="10"></asp:TextBox>
                                <asp:TextBox ID="txtDebited" Width="100%"  Style="max-width: 150px;" Visible="False" runat="server" ReadOnly="True"
                                    CssClass="form-control input-sm" TabIndex="11"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>:
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCNCreditNm" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtCNCreditNm_TextChanged" CssClass="form-control input-sm" TabIndex="12"></asp:TextBox>
                                <asp:TextBox ID="txtJRCreditNm" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtJRCreditNm_TextChanged" CssClass="form-control input-sm" TabIndex="13"></asp:TextBox>
                                <asp:TextBox ID="txtMpCreditNm" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtMpCreditNm_TextChanged" CssClass="form-control input-sm" TabIndex="14"></asp:TextBox>
                                <asp:TextBox ID="txtCreditNm" runat="server" Width="100%"  AutoPostBack="True" ClientIDMode="Static"
                                    OnTextChanged="txtCreditNm_TextChanged" CssClass="form-control input-sm" TabIndex="15"></asp:TextBox>
                                <asp:TextBox ID="txtCredited" Width="100%"  Style="max-width: 150px;" Visible="False" runat="server" ReadOnly="True"
                                    CssClass="form-control input-sm" TabIndex="16"></asp:TextBox>
                            </div>
                        </div>


                        <div class="row form-class">
                            <div class="col-md-2">
                                <%--<asp:Label runat="server" ID="lblChequeNo" Text="Cheque No:" Visible="True"></asp:Label>--%>
                        Cheque No :
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCheque" runat="server" Width="100%"  OnTextChanged="txtCheque_TextChanged" TabIndex="18"
                                    CssClass="form-control input-sm"></asp:TextBox>

                                <asp:Label ID="lblCheque" runat="server" ForeColor="#FF3300" Text="Label" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <%--<asp:Label runat="server" ID="lblChequeDate" Text="Cheque Date:" Visible="False"></asp:Label>--%>
                        Cheque Date :
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtChequeDate" runat="server" Width="100%"  TabIndex="19" ClientIDMode="Static"
                                    OnTextChanged="txtChequeDate_TextChanged" CssClass="form-control input-sm"></asp:TextBox>

                                <asp:Label ID="lblChequeDT" runat="server" ForeColor="#FF3300" Text="Label" Visible="False"></asp:Label>
                            </div>
                        </div>


                        <div class="row form-class">
                            <div class="col-md-2">Remarks:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" 
                                    OnTextChanged="txtRemarks_TextChanged" CssClass="form-control input-sm" TabIndex="20" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Amount:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAmount" runat="server" Width="100%"  OnTextChanged="txtAmount_TextChanged" TabIndex="21"
                                    ClientIDMode="Static" AutoPostBack="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2">In Words:</div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtInwords" runat="server" Width="100%"  TextMode="MultiLine"
                                    ReadOnly="True" OnTextChanged="txtInwords_TextChanged" TabIndex="22" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-12 text-center">
                                <asp:Label ID="lblErrMSg" runat="server" Width="100%"  ForeColor="#CC0000"
                                    Text="lblErrMSg" Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSave" runat="server" Width="100%"  Text="Save"   TabIndex="23"
                                    OnClick="btnSave_Click" CssClass="form-control  btn-primary input-sm" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="Button1" runat="server" Width="100%" 
                                    Text="Save &amp; Print" OnClick="Button1_Click" 
                                    CssClass="form-control btn-primary input-sm" TabIndex="24" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnRefresh"  Width="100%"  OnClick="btnRefresh_Click" Text="Reset" CssClass="form-control  btn-primary input-sm" TabIndex="25" />
                            </div>
                            <div class="col-md-3"></div>
                        </div>

                        <!-- Content End From here -->

                    </div>
                </div>
            </div>

            <asp:Label ID="lblVCount" runat="server" Visible="False" Text="Label"></asp:Label>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="Button1"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnRefresh"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
