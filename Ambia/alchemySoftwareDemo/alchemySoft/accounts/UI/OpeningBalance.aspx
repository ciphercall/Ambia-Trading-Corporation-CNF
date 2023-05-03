<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="OpeningBalance.aspx.cs" Inherits="alchemy.accounts.UI.OpeningBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //BindControlEvents();
        });
        function pageLoad() {
            BindControlEvents();
        }
        function BindControlEvents() {
            $("#<%=txtDate.ClientID%>").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });
            GetCompletionListOpeningBalanceEntryAccountNM();
            GetCompletionListOpeningBalanceEntryAccountNMEdit();
        }
        function GetCompletionListOpeningBalanceEntryAccountNM() {
            $("[id*=txtDebitCD]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListOpeningBalanceEntryAccountNM",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("[id*=txtDebitCD]").val() + "'}",
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
                    $("[id*=txtDebitCDCode]").val(ui.item.x);
                    $("[id*=txtDbAmt]").focus();
                    return true;
                }
            });
        } function GetCompletionListOpeningBalanceEntryAccountNMEdit() {
            $("[id*=txtDebitCDEdit]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListOpeningBalanceEntryAccountNM",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("[id*=txtDebitCDEdit]").val() + "'}",
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
                    $("[id*=txtDebitCDCodeEdit]").val(ui.item.x);
                    $("[id*=txtDbAmtEdit]").focus();
                    return true;
                }
            });
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) { }
            else { return false; }
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
                        <h1>Opening Balance Entry</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

                    </div>
                    <!-- content header end -->

                    <asp:Label ID="lblTotCount" runat="server"></asp:Label>
                    <asp:Label ID="lblVCount" runat="server"></asp:Label>

                    <!-- Content Start From here -->
                    <div class="form-class">


                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">Date</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control input-sm"
                                    OnTextChanged="txtDate_TextChanged" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                                <asp:Label ID="lblMY" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-12 text-center">
                                <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">

                            <asp:GridView ID="gvDetails" runat="server" BackColor="White" BorderStyle="None" CssClass="Gridview text-center" CellPadding="3"
                                CellSpacing="1" GridLines="Both" Width="100%" AutoGenerateColumns="False" ShowFooter="True"
                                OnRowCancelingEdit="gvDetails_RowCancelingEdit" OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing"
                                OnRowUpdating="gvDetails_RowUpdating" OnRowCommand="gvDetails_RowCommand" OnRowDataBound="gvDetails_RowDataBound">

                                <Columns>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("TRANSDT") %>' />
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("TRANSDT") %>' />
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="15%" HorizontalAlign="Center" />
                                        <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVouchNo" runat="server" Text='<%# Eval("TRANSNO") %>'
                                                Style="text-align: center" />
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <asp:Label ID="lblVouchNo" runat="server" Text='<%#Eval("TRANSNO") %>'
                                                Style="text-align: center" />
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="5%" HorizontalAlign="Center" />
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Account Head">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccHdNM" runat="server" Text='<%# Eval("ACCOUNTNM") %>'
                                                Style="text-align: left" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDebitCDEdit" runat="server" CssClass="form-control input-sm" Text='<%#Eval("ACCOUNTNM") %>' TabIndex="12" />
                                            <asp:TextBox ID="txtDebitCDCodeEdit" style="display: none" runat="server" CssClass="form-control input-sm" Text='<%#Eval("ACCOUNTNM") %>' TabIndex="12" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDebitCD" runat="server" CssClass="form-control input-sm" TabIndex="2" />
                                            <asp:TextBox ID="txtDebitCDCode" style="display: none"  runat="server" CssClass="form-control input-sm" TabIndex="2" />
                                        </FooterTemplate>
                                        <FooterStyle Width="40%" />
                                        <ItemStyle Width="40%" />
                                        <HeaderStyle Width="40%" HorizontalAlign="Center" />

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Debit Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebitAmt" runat="server" Text='<%#Eval("DEBITAMT") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDbAmtEdit" CssClass="form-control input-sm" runat="server" Text='<%#Eval("DEBITAMT") %>'
                                                Style="text-align: right" TabIndex="13" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDbAmt" CssClass="form-control input-sm" runat="server" TabIndex="3"
                                                Style="text-align: right" />
                                        </FooterTemplate>

                                        <FooterStyle Width="15%" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Credit Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCrAmt" runat="server" Text='<%#Eval("CREDITAMT") %>'></asp:Label>
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCrAmtEdit" CssClass="form-control input-sm" runat="server" Text='<%#Eval("CREDITAMT") %>'
                                                Style="text-align: right" TabIndex="14" />
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox ID="txtCrAmt" CssClass="form-control input-sm" runat="server" TabIndex="4"
                                                Style="text-align: right" />
                                        </FooterTemplate>
                                        <FooterStyle Width="15%" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server"
                                                ImageUrl="~/Images/update.png" ToolTip="Update" Height="20px" Width="20px"
                                                TabIndex="15" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel"
                                                ImageUrl="~/Images/Cancel.png" ToolTip="Cancel" Height="20px" Width="20px"
                                                TabIndex="15" />

                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <% if (DynamicMenu.dbFunctions.checkParmit("/Accounts/UI/OpeningBalance.aspx", "UPDATER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server"
                                                ImageUrl="~/Images/Edit.png" ToolTip="Edit" Height="20px" Width="20px"
                                                TabIndex="10" />
                                            <%} %>
                                            <% if (DynamicMenu.dbFunctions.checkParmit("/Accounts/UI/OpeningBalance.aspx", "DELETER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" Text="Edit" runat="server"
                                                ImageUrl="~/Images/delete.png" ToolTip="Delete" Height="20px" Width="20px" OnClientClick="return confMSG()"
                                                TabIndex="11" />
                                            <%} %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <% if (DynamicMenu.dbFunctions.checkParmit("/Accounts/UI/OpeningBalance.aspx", "INSERTR") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Images/AddNewitem.png"
                                                CommandName="AddNew" Width="30px" Height="30px" ToolTip="Add new Record"
                                                ValidationGroup="validaiton" TabIndex="5" />
                                            <%} %>
                                        </FooterTemplate>

                                        <FooterStyle Width="10%" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>

                                </Columns>
                                <EditRowStyle Font-Names="Calibri" Font-Size="14px" />
                                <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="13px" BackColor="#D9EDF7" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            </asp:GridView>

                        </div>

                        <div class="row form-class">
                            <div class="col-md-6"></div>
                            <div class="col-md-6">
                                <table class="">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lbltotal" Text="Total : "></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtTotDebit" runat="server" ReadOnly="True" Font-Bold="True"
                                                Style="text-align: right" TabIndex="10" CssClass="form-control input-sm"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTotCredit" runat="server" ReadOnly="True"
                                                Font-Bold="True" Style="text-align: right" TabIndex="11" CssClass="form-control input-sm"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                    </div>


                </div>
                <!-- Content End From here -->
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
