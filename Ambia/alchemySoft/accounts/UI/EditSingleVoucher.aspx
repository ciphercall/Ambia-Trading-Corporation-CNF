<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="EditSingleVoucher.aspx.cs" Inherits="alchemy.accounts.UI.EditSingleVoucher" %>

<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>

    <script>
        
        function pageLoad() {
            BindControlEvents();
        }
        function print() {
            window.open('../Report/Report/RptCreditVoucherEdit.aspx', '_blank');
        }
        function BindControlEvents() {
            $("#<%=txtEdDate.ClientID%>, [id*=txtCqDt]").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });

            GetCompletionListtDebitSingleVoucherNew();
            GetCompletionListCreditSingleVoucherNew();
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) { }
            else { return false; }
        }
        function GetCompletionListtDebitSingleVoucherNew() {
            $("[id*=txtDbCd]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListtDebitSingleVoucherNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtDbCd]").val() + "','transtype' : '" + $("#<%=ddlEditTransType.ClientID %>").val() + "'}",
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
                    $("[id*=lblDRCD]").val(ui.item.x);
                    $("[id*=txtCrCd]").focus();
                    return true;
                }
            });
        }
        function GetCompletionListCreditSingleVoucherNew() {
            $("[id*=txtCrCd]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListCreditSingleVoucherNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtCrCd]").val() + "','transtype' : '" + $("#<%=ddlEditTransType.ClientID %>").val() + "'}",
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
                    $("[id*=lblCreditCD]").val(ui.item.x);
                    $("[id*=txtChq]").focus();
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
                        <h1>Edit Single Voucher</h1> 
                    </div> 
                    <div class="content_wrapper  col-md-12">
                        <br />  
                        <div class="row form-class"> 
                            <div class="col-md-1">Date:</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtEdDate" runat="server" ClientIDMode="Static" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Transaction Type:</div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlEditTransType" runat="server" Width="100%" OnSelectedIndexChanged="ddlEditTransType_SelectedIndexChanged"
                                    TabIndex="2" AutoPostBack="True" CssClass="form-control input-sm">
                                    <asp:ListItem>--SELECT--</asp:ListItem>
                                    <asp:ListItem Value="MREC">RECEIPT</asp:ListItem>
                                    <asp:ListItem Value="MPAY">PAYMENT</asp:ListItem>
                                    <asp:ListItem Value="JOUR">JOURNAL</asp:ListItem>
                                    <asp:ListItem Value="CONT">CONTRA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSearch" Width="100%" runat="server" OnClick="btnSearch_Click"
                                    Text="Search" CssClass="form-control btn-primary input-sm" />
                            </div>
                            <div class="col-md-1"></div>
                        </div>
                    </div> 
                    <hr /> 
                    <div style="text-align: center">
                        <asp:Label ID="lblDebitCD" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTransTP" runat="server"></asp:Label>
                        <asp:Label ID="lblCreditCD" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTransMode" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTransFor" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblCatNM" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblErrMsg" runat="server" ForeColor="#FF3300" Visible="False"></asp:Label>
                    </div>
                    <%Session["UPDATER_singleEdit"]= dbFunctions.checkParmit("/Accounts/UI/SingleTransaction.aspx", "UPDATER");
                        Session["DELETER_singleEdit"]= dbFunctions.checkParmit("/Accounts/UI/SingleTransaction.aspx", "DELETER");  %>
                    <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                        <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="Both"
                            BorderStyle="None" CellPadding="4" CssClass="Gridview text-center" Width="100%" Font-Names="Calibri" Font-Size="11px"
                            OnRowCancelingEdit="gvDetails_RowCancelingEdit" OnRowDeleting="gvDetails_RowDeleting"
                            OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating" OnRowCommand="gvDetails_RowCommand"
                            OnRowDataBound="gvDetails_RowDataBound">
                            <Columns>

                                <asp:TemplateField HeaderText="Print">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnPrint" ClientIDMode="Static" runat="server" CommandName="print" ToolTip="Print" ImageUrl="../../Images/print.png"
                                            CssClass="glyphicon glyphicon-print" Height="20px" Width="20px" OnClientClick="setTimeout(function print() {window.open('../Report/Report/RptCreditVoucherEdit.aspx', '_blank');}, 1000);"/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="btnPrint" ClientIDMode="Static" runat="server" CssClass="glyphicon glyphicon-print" ToolTip="Print"
                                            ImageUrl="../../Images/print.png" Height="20px" Width="20px" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                    <ItemStyle HorizontalAlign="Center" Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Trans No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransNo" runat="server" Text='<%#Eval("TRANSNO") %>' Width="98%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblTransNo" runat="server" Text='<%#Eval("TRANSNO") %>' CssClass="form-control input-sm"
                                            Width="98%" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                    <ItemStyle HorizontalAlign="Center" Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transaction For" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransFor" runat="server" Text='<%#Eval("TRANSFOR") %>' Width="98%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTransFor" runat="server" Width="98%" TabIndex="5" CssClass="form-control input-sm" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlTransFor_SelectedIndexChanged">
                                            <asp:ListItem>OFFICIAL</asp:ListItem>
                                            <asp:ListItem>OTHERS</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Office Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostPoolNM" runat="server" Text='<%#Eval("COSTPNM") %>' Width="98%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList runat="server" TabIndex="6" ID="ddlCostPoolName" CssClass="form-control input-sm" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostPoolID" runat="server" Text='<%# Eval("COSTPID") %>' Visible="true"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblCostPoolIDEdit" runat="server" Text='<%# Eval("COSTPID") %>' Visible="true"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mode">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransMode" runat="server" Text='<%#Eval("TRANSMODE") %>' Width="98%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTransMode" runat="server" TabIndex="7" OnSelectedIndexChanged="ddlTransMode_SelectedIndexChanged"
                                            AutoPostBack="true" CssClass="form-control input-sm" Width="98%">
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
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Debit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDBCD" runat="server" Text='<%#Eval("DEBITCD") %>' Width="98%" />
                                        <asp:Label ID="lblDRCD" runat="server" Text='<%# Eval("DRCD") %>' Style="display: none;"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDbCd" runat="server" Text='<%#Eval("DEBITCD") %>' Width="98%"
                                            TabIndex="8" AutoPostBack="True" OnTextChanged="txtDbCd_TextChanged" CssClass="form-control input-sm" />
                                        <asp:TextBox ID="lblDRCD" runat="server" Text='<%# Eval("DRCD") %>' Style="display: none;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:TemplateField> 
                                <asp:TemplateField Visible="False"></asp:TemplateField> 
                                <asp:TemplateField HeaderText="Credit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCRCD" runat="server" Text='<%#Eval("CREDITCD") %>' Width="98%" />
                                        <asp:Label ID="lblCreditCD" runat="server" Text='<%#Eval("CRCD") %>' Style="display: none;"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCrCd" runat="server" Text='<%#Eval("CREDITCD") %>' Width="98%"
                                            TabIndex="9" AutoPostBack="True" OnTextChanged="txtCrCd_TextChanged" CssClass="form-control input-sm" />
                                        <asp:TextBox ID="lblCreditCD" runat="server" Text='<%# Eval("CRCD") %>' Style="display: none;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField> 
                                <asp:TemplateField Visible="False"></asp:TemplateField> 
                                <asp:TemplateField HeaderText="Cheque No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCHEQUE" runat="server" Text='<%#Eval("CHEQUENO") %>' Width="98%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtChq" runat="server" Text='<%#Eval("CHEQUENO") %>' CssClass="form-control input-sm"
                                            TabIndex="10" Width="98%" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cheque Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCQDT" runat="server" Text='<%#Eval("CHEQUEDT_CON") %>' Width="98%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCqDt" runat="server" Text='<%#Eval("CHEQUEDT_CON") %>' CssClass="form-control input-sm"
                                            TabIndex="11" Width="98%" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("AMOUNT") %>' Width="98%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("AMOUNT") %>' Style="text-align: right"
                                            TabIndex="12" CssClass="form-control input-sm" Width="98%" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>' Width="98%"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("REMARKS") %>' CssClass="form-control input-sm"
                                            Width="98%" TabIndex="13" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/Images/update.png"
                                            TabIndex="14" ToolTip="Update" Height="20px" Width="20px" />
                                        <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/Cancel.png"
                                            TabIndex="15" ToolTip="Cancel" Height="20px" Width="20px" />
                                    </EditItemTemplate>
                                    
                                    <ItemTemplate>
                                        <% if (Session["UPDATER_singleEdit"].ToString() == "True")
                                           {%> 
                                        <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Images/Edit.png"
                                            ToolTip="Edit" Height="20px" Width="20px" TabIndex="101" />
                                        <% } %>
                                       <% if (Session["DELETER_singleEdit"].ToString() == "True")
                                            { %>
                                        <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" runat="server" ImageUrl="~/Images/delete.png"
                                            ToolTip="Delete" Height="20px" Width="20px" OnClientClick="return confMSG()"
                                            TabIndex="102" />
                                        <% } %>
                                    </ItemTemplate>

                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle Font-Names="Calibri" Font-Size="14px" />
                            <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="13px" BackColor="#D9EDF7" />
                            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        </asp:GridView> 
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 75%; text-align: right; font-weight: 700">Total : </td>
                                <td style="width: 9%; text-align: right; font-weight: 700">
                                    <asp:Label runat="server" ID="lblTotalAmt" Text="0.00"></asp:Label>
                                </td>
                                <td style="width: 16%"></td>
                            </tr>
                        </table> 
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
