<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="CostPool.aspx.cs" Inherits="alchemy.accounts.UI.CostPool" %>

<%@ Import Namespace="DynamicMenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
           // BindControlEvents();
            $("#<%=txtCategoryNM.ClientID %>").keydown(function (e) {
                //if (e.which === 9 || e.which === 13)
                //    window.__doPostBack();
            });
        });
        function pageLoad() {
            BindControlEvents();
        }
        function BindControlEvents() {
            Search_GetCompletionListCostPoolName();
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) {
            }
            else {
                return false;
            }
        }

        function Search_GetCompletionListCostPoolName() {
            $("#<%=txtCategoryNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListCostPoolName",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtCategoryNM.ClientID %>").val() + "'}",
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
                select: function (event, ui) {
                    $("#<%=Search.ClientID %>").focus();
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
                        <h1>Cost Pool Entry</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->



                    </div>
                    <!-- content header end -->


                    <!-- Content Start From here -->
                    <div class="form-class">
                        <div class="row form-class">
                            <div class="col-md-3"></div>
                            <div class="col-md-1">Category:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCategoryNM" runat="server" TabIndex="1"
                                    OnTextChanged="txtCategoryNM_TextChanged" CssClass="form-control input-sm"
                                    AutoPostBack="True"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="Search" runat="server" CssClass="form-control input-sm btn-primary"
                                    Text="Search" OnClick="Search_Click" />
                            </div>
                            <div class="col-md-2"></div>
                        </div>



                        <div>
                            <asp:Label ID="lblCatID" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblMaxCatID" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblChkItemID" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblIMaxItemID" runat="server" Visible="False"></asp:Label>
                        </div>
                        <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                            <asp:GridView ID="gvDetails" runat="server" BackColor="White" AutoGenerateColumns="False" ShowFooter="True" GridLines="Both"
                                BorderStyle="None" CssClass="Gridview text-center" CellPadding="3" CellSpacing="1" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                                OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating"
                                OnRowCommand="gvDetails_RowCommand" OnRowDataBound="gvDetails_RowDataBound" Width="100%"
                                Font-Names="Calibri">
                                <Columns>
                                    <asp:TemplateField HeaderText="Cat ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCatGID" runat="server" Text='<%# Eval("CATID") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblCatGIDEdit" runat="server" Text='<%#Eval("CATID") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                        <ItemStyle HorizontalAlign="Center" Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Pool ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCOSTPID" runat="server" Text='<%# Eval("COSTPID") %>' Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblCOSTPIDEdit" runat="server" Text='<%#Eval("COSTPID") %>' Style="text-align: center" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Pool Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCOSTPNM" runat="server" Text='<%# Eval("COSTPNM") %>' Style="text-align: left" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCOSTPNMEdit" runat="server" Text='<%#Eval("COSTPNM") %>' Width="98%"
                                                TabIndex="10" Font-Names="Calibri" CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtCOSTPNM" runat="server" Width="98%" TabIndex="3" Font-Names="Calibri" CssClass="form-control input-sm" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="29%" />
                                        <ItemStyle HorizontalAlign="Left" Width="29%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lblREMARKS" runat="server" Text='<%#Eval("REMARKS") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtREMARKSEdit" runat="server" Text='<%#Eval("REMARKS") %>' TabIndex="15"
                                                Width="98%" Font-Names="Calibri" CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtREMARKS" runat="server" TabIndex="8"
                                                Width="98%" Font-Names="Calibri" CssClass="form-control input-sm" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle Width="20%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <% if (dbFunctions.checkParmit("/Accounts/UI/CostPool.aspx", "UPDATER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit"
                                                Height="20px" ImageUrl="~/Images/Edit.png" TabIndex="30" ToolTip="Edit"
                                                Width="20px" />
                                            <% } %>
                                            <% if (dbFunctions.checkParmit("/Accounts/UI/CostPool.aspx", "DELETER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete"
                                                Height="20px" ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()"
                                                TabIndex="31" Text="Edit" ToolTip="Delete" Width="20px" />
                                            <% } %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update"
                                                Height="20px" ImageUrl="~/Images/update.png" TabIndex="16" ToolTip="Update"
                                                Width="20px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel"
                                                Height="20px" ImageUrl="~/Images/Cancel.png" TabIndex="17" ToolTip="Cancel"
                                                Width="20px" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <% if (dbFunctions.checkParmit("/Accounts/UI/CostPool.aspx", "INSERTR") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="AddNew"
                                                Height="20px" ImageUrl="~/Images/AddNewitem.png" TabIndex="9"
                                                ToolTip="Add new Record" ValidationGroup="validaiton" Width="20px" />
                                            <% } %>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle Font-Names="Calibri" Font-Size="14px" />
                                <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="13px" BackColor="#D9EDF7" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            </asp:GridView>

                        </div>


                    </div>
                    <!-- Content End From here -->
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
