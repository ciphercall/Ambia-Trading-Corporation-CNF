<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="JobReceive.aspx.cs" Inherits="alchemySoft.CNF.UI.JobReceive" %>

<%@ Import Namespace="alchemySoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Job Receive</title>



    <script>
        function pageLoad() {
            GetCompletionListJobNoYearType();
            GetCompletionListCash_Bank();
        }

        function GetCompletionListJobNoYearType() {
            $("#<%=txtJobID.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/search.asmx/GetCompletionListJobNoYearType",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("#<%=txtJobID.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Request.Cookies["UserInfo"]%>'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.JobNoAndTp,
                                    value: item.JobNoAndTp,
                                    PartyId: item.PartyId,
                                    CompanyId: item.CompanyId,
                                    CompanyName: item.CompanyName,
                                    AccountName: item.AccountName
                                };
                            }));
                        },
                        error: function (result) {
                            alert(result);
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    var jobnotype = ui.item.label;
                    var res = jobnotype.split('|');
                    $("#<%=txtJobYear.ClientID %>").val(res[1]);
                    $("#<%=txtJobType.ClientID %>").val(res[2]);

                    $("#<%=txtPartyID.ClientID %>").val(ui.item.PartyId);
                    $("#<%=txtCompanyID.ClientID %>").val(ui.item.CompanyId);
                    $("#<%=txtCompanyNM.ClientID %>").val(ui.item.CompanyName);
                    $("#<%=txtPartyNM.ClientID %>").val(ui.item.AccountName);

                    //setTimeout(jobnotypesubstring, 500);
                    $("#<%=txtCashBankNM.ClientID %>").focus();
                    return true;
                }
            });
        }


        <%--function GetCompletionListCash_Bank() {
            $("#<%=txtCashBankNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/search.asmx/GetCompletionListCash_Bank",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtCashBankNM.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Request.Cookies["UserInfo"]%>','rcvTp' : '" + $("#<%=ddlRcvType.ClientID %>").val() + "', 'brCD' : '" + $("#<%=txtCompanyID.ClientID %>").val() + "'}",
                        
                        data: "{ 'txt' : '" + $("#<%=txtCashBankNM.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Session["USERTYPE"]%>','brCD' : '<%=HttpContext.Current.Session["BrCD"]%>','rcvTp' : '" + $("#<%=ddlRcvType.ClientID %>").val() + "'}",

        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Label,
                                    value: item.Label,
                                    accountcode: item.Value
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
                    $("#<%=txtCashBankID.ClientID %>").val(ui.item.accountcode);
                    $("#<%=txtRemarks.ClientID %>").focus();
                    return true;
                }
            });
        }--%>


        function GetCompletionListCash_Bank() {
            $("#<%=txtCashBankNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/search.asmx/GetCompletionListCash_Bank",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        <%--data: "{ 'txt' : '" + $("#<%=txtCashBankNM.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Session["USERTYPE"]%>','brCD' : '<%=HttpContext.Current.Session["BrCD"]%>','rcvTp' : '" + $("#<%=ddlRcvType.ClientID %>").val() + "'}",--%>

                        data: "{ 'term' : '" + $("#<%=txtCashBankNM.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Request.Cookies["UserInfo"]%>','rcvTp' : '" + $("#<%=ddlRcvType.ClientID %>").val() + "', 'brCD' : '" + $("#<%=txtCompanyID.ClientID %>").val() + "'}",

                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.label,
                                    value: item.label,
                                    accountcode: item.value
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
                    $("#<%=txtCashBankID.ClientID %>").val(ui.item.accountcode);
                    $("#<%=txtRemarks.ClientID %>").focus();
                    return true;
                }
            });
        }
        //function isNumberKey(evt) {
        //    var charCode = (evt.which) ? evt.which : event.keyCode;
        //    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46) {
        //        return false;
        //    }
        //    return true;
        //}


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1 style="font-style: italic"><strong>Receive Against job (Only Bill) Information</strong></h1>

                        <!-- logout option button -->
                        <%-- ReSharper disable once Html.IdDuplication --%>
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <% if (dbFunctions.checkParmit("/CNF/UI/cnf-job-info.aspx", "UPDATER"))
                                    { %>
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-edit" ID="btnEditss" runat="server" Text="Edit"></asp:LinkButton>
                                </a>
                                </li>
                                <% } %>
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-refresh" ID="btnRefreshss" runat="server" Text="Refresh"></asp:LinkButton></a>
                                </li>
                            </ul>
                        </div>
                        <!-- end logout option -->
                    </div>

                    <%--<div class="panel panel-default" style="border-color: #5cb85c">
                        <div class="panel-heading"></div>
                        <div class="panel-body">Panel Content</div>
                    </div>--%>


                    <div class="content_wrapper">
                        <div class="col-md-12">
                            <asp:Label runat="server" ID="lblMY" Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="lblSL" Visible="false"></asp:Label>

                            <div class="panel panel-default" style="border-color: #5cb85c; padding-left: 4px; padding-right: 4px; padding-bottom: 2px">
                                <div class="panel-heading">
                                    <div class="row form-class3px">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-1">
                                            <% if (dbFunctions.checkParmit("/CNF/UI/JobReceive.aspx", "UPDATER"))
                                                {%>
                                            <asp:Button ID="btnUpdate" Text="Edit" Width="140%" runat="server" CssClass="form-control btn-success" OnClick="btnUpdate_Click"></asp:Button>
                                            <% }  %>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnCancel" Text="Cancel" Width="140%" runat="server" Visible="False" CssClass="form-control btn-info" OnClick="btnCancel_Click"></asp:Button>
                                        </div>
                                        <div class="col-md-1">
                                            <% if (dbFunctions.checkParmit("/CNF/UI/JobReceive.aspx", "DELETER"))
                                                { %>
                                            <asp:Button ID="btnDelete" Text="Delete" Width="140%" runat="server" Visible="False" CssClass="form-control btn-danger" OnClick="btnDelete_Click"></asp:Button>
                                            <% } %>
                                        </div>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Receive Date</div>
                                    <div class="col-md-2">
                                        <asp:TextBox runat="server" ID="txtReceiveDate" CssClass="form-control input-sm" Width="100%" AutoPostBack="True" OnTextChanged="txtReceiveDate_TextChanged" TabIndex="1"></asp:TextBox>

                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox runat="server" ID="txtTransMY" CssClass="form-control input-sm" Width="100%" ReadOnly="true"></asp:TextBox>
                                        <asp:DropDownList runat="server" Width="170%" ID="ddlTransMy" OnSelectedIndexChanged="ddlTransMy_SelectedIndexChanged" AutoPostBack="True" Visible="false" CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Voucher No</div>
                                    <div class="col-md-2">
                                        <asp:TextBox runat="server" ID="txtVoucher" CssClass="form-control input-sm" Width="100%"
                                            ReadOnly="true"></asp:TextBox>
                                        <asp:DropDownList runat="server" ID="ddlVouchNo" OnSelectedIndexChanged="ddlVouchNo_SelectedIndexChanged" AutoPostBack="True" Visible="false" CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                        <%----%>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Receive Type</div>
                                    <div class="col-md-2">
                                        <asp:DropDownList runat="server" ID="ddlRcvType" CssClass="form-control input-sm" Width="100%"
                                            TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="ddlRcvType_SelectedIndexChanged">
                                            <asp:ListItem Value="Normal">Normal</asp:ListItem>
                                            <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                            <asp:ListItem>Discount</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lbltransfor" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Company ID</div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtCompanyNM" CssClass="form-control input-sm" Width="100%" onFocus="blur()"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtCompanyID" CssClass="form-control input-sm" Width="100%" Visible="False"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Job No & Year</div>
                                    <div class="col-md-1">
                                        <asp:TextBox runat="server" ID="txtJobID" CssClass="form-control input-sm" Width="140%" TabIndex="3" OnTextChanged="txtJobID_TextChanged" AutoPostBack="True"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox runat="server" ID="txtJobYear" CssClass="form-control input-sm" Width="140%" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox runat="server" ID="txtJobType" CssClass="form-control input-sm" Width="140%" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Party ID</div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtPartyNM" CssClass="form-control input-sm" Width="100%" ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtPartyID" CssClass="form-control input-sm" Width="100%" ReadOnly="true"
                                            Visible="False"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Cash/Bank ID</div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtCashBankNM" CssClass="form-control input-sm" Width="100%" OnTextChanged="txtCashBankNM_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtCashBankID" CssClass="form-control input-sm" Width="100%" Visible="False"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Remarks</div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">Amount</div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtAmount" CssClass="form-control input-sm" Width="100%" AutoPostBack="True" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">In Words</div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtInwords" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">
                                        <% if (dbFunctions.checkParmit("/CNF/UI/JobReceive.aspx", "INSERTR"))
                                            { %>
                                        <asp:Button runat="server" Text="Save" ID="btnSave" CssClass="form-control btn-primary" Width="100%" OnClick="btnSave_Click"></asp:Button>
                                        <% } %>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button runat="server" Text="Print" ID="btnSave_Print" CssClass="form-control btn-default" Width="100%" OnClick="btnSave_Print_Click"></asp:Button>
                                    </div>
                                    <asp:Label runat="server" ID="lblErrmsg" Visible="False" ForeColor="#990000" Style="font-weight: 700"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave_Print" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
