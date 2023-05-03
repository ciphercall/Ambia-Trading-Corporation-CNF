<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="MultipleVoucher.aspx.cs" Inherits="alchemy.accounts.UI.MultipleVoucher" %>

<%@ Import Namespace="DynamicMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
           // BindControlEvents();
        });
        function pageLoad() {
            BindControlEvents();
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) { }
            else { return false; }
        }
        function BindControlEvents() {
            $("#<%=txtTransDate.ClientID%>, [id*=txtChequeDate], [id*=txtChequeDateEdit]").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });
            GetCompletionListtDebitSingleVoucherNew();
            GetCompletionListtDebitSingleVoucherNewEdit();
            GetCompletionListCreditSingleVoucherNew();
            GetCompletionListCreditSingleVoucherNewEdit();
            //$('.ui-autocomplete').click(function () {
            //    __doPostBack();
            //});
            //$("[id*=txtDebited],[id*=txtDebitedEdit],[id*=txtCredited],[id*=txtCreditedEdit]").keydown(function (e) {
            //    if (e.which == 9 || e.which == 13)
            //        window.__doPostBack();
            //});
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) {
            }
            else {
                return false;
            }
        }
        function GetCompletionListtDebitSingleVoucherNew() {
            $("[id*=txtDebited]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListtDebitSingleVoucherNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtDebited]").val() + "','transtype' : '" + $("#<%=ddlTransType.ClientID %>").val() + "'}",
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
                    $("[id*=txtDebitCD]").val(ui.item.x);
                    $("[id*=txtCredited]").focus();
                    return true;
                }
            });
        }
        function GetCompletionListCreditSingleVoucherNew() {
            $("[id*=txtCredited]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListCreditSingleVoucherNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtCredited]").val() + "','transtype' : '" + $("#<%=ddlTransType.ClientID %>").val() + "'}",
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
                    $("[id*=txtCreditCD]").val(ui.item.x);
                    $("[id*=txtCostPNM]").focus();
                    return true;
                }
            });
        }
        function GetCompletionListtDebitSingleVoucherNewEdit() {
            $("[id*=txtDebitedEdit]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListtDebitSingleVoucherNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtDebitedEdit]").val() + "','transtype' : '" + $("#<%=ddlTransType.ClientID %>").val() + "'}",
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
                    $("[id*=txtDebitCD]").val(ui.item.x);
                    $("[id*=txtCreditedEdit]").focus();
                    return true;
                }
            });
        }
        function GetCompletionListCreditSingleVoucherNewEdit() {
            $("[id*=txtCreditedEdit]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListCreditSingleVoucherNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtCreditedEdit]").val() + "','transtype' : '" + $("#<%=ddlTransType.ClientID %>").val() + "'}",
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
                    $("[id*=txtCreditCD]").val(ui.item.x);
                    $("[id*=txtCostPNMEdit]").focus();
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
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>

            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Multiple Transaction</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <%--<div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <% if (dbFunctions.checkParmit("/Accounts/UI/MultipleVoucher.aspx", "INSERTR") == true)
                                    { %>
                                <li><a href="MultipleVoucher.aspx"><i class="fa fa-plus"></i>Create</a>
                                </li>
                                <% } %>

                                <% if (dbFunctions.checkParmit("/Accounts/UI/MultipleVoucher.aspx", "UPDATER") == true)
                                    { %>
                                <li><a href="MultipleVoucher.aspx"><i class="fa fa-edit"></i>Edit</a>
                                </li>
                                <% } %>

                                <% if (dbFunctions.checkParmit("/Accounts/UI/MultipleVoucher.aspx", "DELETER") == true)
                                    { %>
                                <li><a href="MultipleVoucher.aspx"><i class="fa fa-edit"></i>Delete</a>
                                </li>
                                <% } %>
                            </ul>
                        </div>--%>
                        <!-- end logout option -->


                    </div>
                    <!-- content header end -->


                    <!-- Content Start From here -->
                    <div class="form-class">

                        <div class="row form-class">
                            <asp:TextBox ID="txtDebitCD" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="txtCreditCD" runat="server" Style="display: none"></asp:TextBox>
                            <asp:Label ID="lblTransforEdit" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblTransmodeEdit" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblVCount" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblSLCount" runat="server" Visible="False"></asp:Label>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2">Voucher No</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtVouchNo" runat="server" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:DropDownList ID="ddlVouch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVouch_SelectedIndexChanged"
                                    CssClass="form-control " Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <% if (dbFunctions.checkParmit("/Accounts/UI/MultipleVoucher.aspx", "UPDATER") == true)
                                    { %>
                                <asp:Button ID="btnEdit" runat="server" CssClass="form-control btn-primary input-sm"
                                    Text="EDIT" OnClick="btnEdit_Click" />
                                <% } %>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnPrint" runat="server" CssClass="form-control btn-primary input-sm"
                                    Text="PRINT"  OnClick="btnPrint_Click" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Voucher Type:</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransType" runat="server" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged"
                                    TabIndex="1" AutoPostBack="True" CssClass="form-control input-sm">
                                    <asp:ListItem Value="MPAY">PAYMENT</asp:ListItem>
                                    <asp:ListItem Value="MREC">RECEIPT</asp:ListItem>
                                    <asp:ListItem Value="JOUR">JOURNAL</asp:ListItem>
                                    <asp:ListItem Value="CONT">CONTRA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">Voucher Date:</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtTransDate" runat="server" 
                                    OnTextChanged="txtTransDate_TextChanged" AutoPostBack="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtTransYear" runat="server" CssClass="form-control input-sm" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>



                        <div class="text-center">
                            <asp:Label ID="lblError" runat="server" ForeColor="#CC0000" Visible="False"></asp:Label>
                        </div>

                        <hr />
                        <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                            <asp:GridView ID="gvDetails" runat="server" BackColor="White" BorderStyle="None" CssClass="Gridview text-center" CellPadding="3"
                                CellSpacing="1" GridLines="Both" Width="100%" AutoGenerateColumns="False" ShowFooter="True"
                                OnRowDataBound="gvDetails_RowDataBound" OnRowCommand="gvDetails_RowCommand" OnRowEditing="gvDetails_RowEditing"
                                OnRowCancelingEdit="gvDetails_RowCancelingEdit" OnRowUpdating="gvDetails_RowUpdating" OnRowDeleting="gvDetails_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblSLEdit" runat="server" Text='<%# Eval("SERIALNO") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSL" runat="server" Text='<%# Eval("SERIALNO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="2%" />
                                        <ItemStyle HorizontalAlign="Center" Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDebit" runat="server"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebit" runat="server" Text='<%# Eval("DEBITNM") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDebitedEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                TabIndex="30" Width="98%" Text='<%# Eval("DEBITNM") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDebited" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                TabIndex="4" Width="98%"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCredit" runat="server"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit" runat="server" Text='<%# Eval("CREDITNM") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCreditedEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                TabIndex="31" Width="98%" Text='<%# Eval("CREDITNM") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtCredited" runat="server" Font-Names="Calibri"
                                                TabIndex="5" Width="98%" CssClass="form-control input-sm"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trans For" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransFor" runat="server" Text='<%# Eval("TRANSFOR") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlTransforEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                TabIndex="32" Width="98%" OnSelectedIndexChanged="ddlTransforEdit_SelectedIndexChanged"
                                                AutoPostBack="True">
                                                <asp:ListItem>PROJECT</asp:ListItem>
                                                <asp:ListItem>OFFICIAL</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlTransfor" runat="server" Font-Names="Calibri"
                                                TabIndex="6" Width="98%" AutoPostBack="True" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlTransfor_SelectedIndexChanged">
                                                <asp:ListItem>PROJECT</asp:ListItem>
                                                <asp:ListItem>OFFICIAL</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Left" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Office">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProject" runat="server" Text='<%# Eval("COSTPNM") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="txtCostPNMEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                Font-Size="10px" TabIndex="33" Width="98%">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="txtCostPNM" runat="server" Font-Names="Calibri"
                                                TabIndex="7" Width="98%" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transaction Mode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransMode" runat="server" Text='<%# Eval("TRANSMODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlTransModeEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlTransModeEdit_SelectedIndexChanged"
                                                TabIndex="34" Width="98%" AutoPostBack="True">
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
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlTransMode" runat="server" Font-Names="Calibri"
                                                OnSelectedIndexChanged="ddlTransMode_SelectedIndexChanged" TabIndex="8" Width="98%"
                                                AutoPostBack="True" CssClass="form-control input-sm">
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
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                        <ItemStyle HorizontalAlign="Left" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cheque No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChequeNo" runat="server" Text='<%# Eval("CHEQUENO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtChequeEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                Font-Size="10px" TabIndex="35" Width="98%" Text='<%# Eval("CHEQUENO") %>' Enabled="False"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtCheque" runat="server" Font-Names="Calibri"
                                                TabIndex="9" Width="98%" CssClass="form-control input-sm" Enabled="False"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cheque Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChequeDT" runat="server" Text='<%# Eval("CHEQUEDT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <strong>
                                                <asp:TextBox ID="txtChequeDateEdit" runat="server" CssClass="form-control input-sm" Font-Names="Calibri"
                                                    Font-Size="8px" TabIndex="36" Width="98%" Text='<%# Eval("CHEQUEDT") %>' Enabled="False"
                                                    AutoPostBack="True" OnTextChanged="txtChequeDateEdit_TextChanged"></asp:TextBox>
                                            </strong>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <strong>
                                                <asp:TextBox ID="txtChequeDate" runat="server" Font-Names="Calibri"
                                                    TabIndex="10" Width="98%" CssClass="form-control input-sm" AutoPostBack="True" 
                                                    OnTextChanged="txtChequeDate_TextChanged" Enabled="False"></asp:TextBox>

                                            </strong>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                        <ItemStyle HorizontalAlign="Left" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("REMARKS") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRemarksEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                Font-Size="10px" TabIndex="37" Width="98%" Text='<%# Eval("REMARKS") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtRemarks" runat="server" Font-Names="Calibri"
                                                TabIndex="11" Width="98%" CssClass="form-control input-sm"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAmountEdit" runat="server" Font-Names="Calibri" CssClass="form-control input-sm"
                                                Font-Size="10px" TabIndex="38" Width="98%" Text='<%# Eval("AMOUNT") %>' Style="text-align: right"
                                                AutoPostBack="True" OnTextChanged="txtAmountEdit_TextChanged"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAmount" runat="server" Font-Names="Calibri"
                                                TabIndex="12" Width="98%" CssClass="form-control input-sm" Style="text-align: right"
                                                AutoPostBack="True" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <% if (dbFunctions.checkParmit("/Accounts/UI/MultipleVoucher.aspx", "UPDATER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" Height="20px"
                                                ImageUrl="~/Images/Edit.png" TabIndex="20" ToolTip="Edit" Width="20px" />
                                            <% } %>
                                            <% if (dbFunctions.checkParmit("/Accounts/UI/MultipleVoucher.aspx", "DELETER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" Height="20px"
                                                ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()" TabIndex="21"
                                                ToolTip="Delete" Width="21px" />
                                            <% } %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" Height="25px"
                                                ImageUrl="~/Images/update.png" TabIndex="39" ToolTip="Update" Width="25px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" Height="25px"
                                                ImageUrl="~/Images/Cancel.png" TabIndex="40" ToolTip="Cancel" Width="25px" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <% if (dbFunctions.checkParmit("/Accounts/UI/MultipleVoucher.aspx", "INSERTR") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon" CssClass=""
                                                Height="25px" Width="25" ImageUrl="~/Images/AddNewitem.png" TabIndex="13" ToolTip="Save &amp; Continue"
                                                ValidationGroup="validaiton" />
                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Complete" CssClass=""
                                                Height="25px" Width="25" ImageUrl="~/Images/checkmark.png" TabIndex="14" ToolTip="Complete"
                                                ValidationGroup="validaiton" />
                                            <asp:ImageButton ID="ImagebtnPPrint" runat="server" CommandName="SavePrint" CssClass=""
                                                Height="25px" Width="25" ImageUrl="~/Images/print.png" TabIndex="15" ToolTip="Save &amp; Print"
                                                ValidationGroup="validaiton" />
                                            <% } %>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle Font-Names="Calibri" Font-Size="14px" />
                                <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="13px" BackColor="#D9EDF7" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            </asp:GridView>
                        </div>


                        <div class="row form-class">
                            <div class="col-md-2">In Words :</div>
                            <div class="col-md-7">
                                <asp:TextBox ID="txtCumInWords" runat="server" ReadOnly="True" CssClass="form-control input-sm" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div class="col-md-3"></div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-2">In Words (Total) :</div>
                            <div class="col-md-7">
                                <asp:TextBox ID="txtTotInWords" runat="server" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtTotAmount" runat="server" ReadOnly="True" CssClass="form-control input-sm" Style="text-align: right;"
                                    Font-Bold="True"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
             
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
