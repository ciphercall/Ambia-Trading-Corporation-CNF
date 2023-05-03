<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Expense-Information.aspx.cs" Inherits="alchemySoft.CNF.UI.Expense_Information" %>

<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>:: Job Receive ::</title>
    <%-- <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>--%>


    <%-- ReSharper disable once UseOfImplicitGlobalInFunctionScope --%>
    <script>
        $(document).ready(function () {
            BindControlEvents();
        });
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) { }
            else { return false; }
        }
        function BindControlEvents() {
            GetCompletionListExpensePerticularHead();
        }
        function GetCompletionListExpensePerticularHead() {
            $("#<%=txtcat.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListExpensePerticularHead",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtcat.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    value: item
                                };
                            }));
                        },
                        error: function () {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    $("#<%=btnSubmit.ClientID %>").focus();
                    return true;
                }
            });
            }
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46) {
                    return false;
                }
                return true;
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
                        <h1>Expense Information Entry</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <%-- ReSharper disable once Html.IdDuplication --%>
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-refresh" ID="btnRefresh" OnClick="btnRefresh_Click" runat="server" Text="Refresh"></asp:LinkButton></a>
                                </li>
                            </ul>
                        </div>
                        <!-- end logout option -->

                    </div>
                    <br />
                    <!-- content header end -->
                    <div class="form-class text-center">
                        <asp:Label runat="server" ID="lblErrMsgExist" Visible="False" ForeColor="#990000" Style="font-weight: 700"></asp:Label>
                        <asp:Label runat="server" ID="lblErrMsg" Visible="False" ForeColor="#990000" Style="font-weight: 700"></asp:Label>
                    </div>
                    <asp:Label runat="server" ID="lblcid" Visible="false"></asp:Label>
                    <asp:Label ID="Label1" runat="server" ForeColor="#990000" Visible="False"></asp:Label>
                    <asp:Label ID="lblChkInternalID" runat="server" ForeColor="#990000" Visible="False"></asp:Label>

                    <div class="content_wrapper">
                        <div class="col-md-12">
                            <div class="row form-class3px">
                                <div class="col-md-4 text-right">Category ID</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtcat" runat="server" TabIndex="1" Width="90%" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtcatID" runat="server" AutoPostBack="True" TabIndex="2" OnTextChanged="txtcatID_TextChanged" CssClass="form-control input-sm text-center"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button runat="server" Width="200%" ID="btnSubmit" CssClass="form-control input-sm btn-primary" OnClick="txtcat_TextChanged" Text="Submit" />
                                </div>
                            </div>
                            <div class="panel panel-default" style="border-color: #5cb85c; padding-left: 4px; padding-right: 4px; padding-bottom: 2px">
                                <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True" BackColor="White" GridLines="Both"
                                    BorderStyle="None" CellPadding="4" CssClass="Grid" Font-Size="11px" Width="100%"
                                    OnRowDataBound="gvDetails_RowDataBound" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                                    OnRowCommand="gvDetails_RowCommand" OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing"
                                    OnRowUpdating="gvDetails_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Style="text-align: center" Text='<%# Eval("EXPCID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblIDEdit" runat="server" Style="text-align: center" Text='<%# Eval("EXPCID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                            <FooterStyle CssClass="txtalign" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expense ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpense" runat="server" Style="text-align: center" Text='<%# Eval("EXPID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label runat="server" ID="lblExpenseEdit" Style="text-align: center" Text='<%# Eval("EXPID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                            <ItemStyle HorizontalAlign="Center" Width="7%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Particulars">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtParticularsEdit" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="62" Text='<%# Eval("EXPNM") %>' Style="text-align: left"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtParticulars" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="32"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblParticulars" runat="server" Text='<%# Eval("EXPNM") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="45%" />
                                            <ItemStyle HorizontalAlign="Center" Width="45%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRemarksEdit" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="63" Text='<%# Eval("REMARKS") %>' Style="text-align: left"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="33" Style="text-align: left"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("REMARKS") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                            <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <EditItemTemplate>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Expense-Information.aspx", "UPDATER"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" Height="20px"
                                                    ImageUrl="~/Images/update.png" TabIndex="64" ToolTip="Update" Width="20px" />
                                                <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" Height="20px"
                                                    ImageUrl="~/Images/Cancel.png" TabIndex="65" ToolTip="Cancel" Width="20px" />
                                                <% } %>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Expense-Information.aspx", "INSERTR"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon"
                                                    Height="25px" ImageUrl="~/Images/AddNewitem.png" TabIndex="34" ToolTip="Save &amp; Continue"
                                                    ValidationGroup="validaiton" Width="25px" />
                                                <% } %>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Expense-Information.aspx", "UPDATER"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" Height="20px"
                                                    ImageUrl="~/Images/Edit.png" TabIndex="100" ToolTip="Edit" Width="20px" />
                                                <% } %>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Expense-Information.aspx", "DELETER"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" Height="20px"
                                                    ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()" TabIndex="101"
                                                    ToolTip="Delete" Width="21px" />
                                                <% } %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle Font-Size="14px" />
                                    <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="16px" BackColor="#D9EDF7" />
                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnSave_Print" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
