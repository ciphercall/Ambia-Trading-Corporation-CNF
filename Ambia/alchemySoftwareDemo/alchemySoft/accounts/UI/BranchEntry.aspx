<%@ Page Title="" Language="C#" MasterPageFile="~/Dynamic.Master" AutoEventWireup="true" CodeBehind="BranchEntry.aspx.cs" Inherits="alchemy.accounts.UI.BranchEntry" %>

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
            ;
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) {
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>


            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Office Entry</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->



                    </div>
                    <!-- content header end -->


                    <!-- Content Start From here -->
                    <div class="form-class">
                        <div class="form-class text-center">
                            <asp:Label runat="server" ID="lblMsg" Visible="False" ForeColor="red"></asp:Label>
                        </div>
                        <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">

                            <asp:GridView ID="gridViewForBranch" runat="server" BackColor="White" AutoGenerateColumns="False" ShowFooter="True" GridLines="Both"
                                BorderStyle="None" CssClass="Gridview text-center" CellPadding="3" CellSpacing="1" OnRowCancelingEdit="gridViewForBranch_RowCancelingEdit"
                                OnRowEditing="gridViewForBranch_RowEditing" OnRowUpdating="gridViewForBranch_RowUpdating"
                                Width="100%" OnRowCommand="gridViewForBranch_RowCommand" OnRowDeleting="gridViewForBranch_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="COMPANYID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyId" runat="server" Text='<%# Eval("COMPID") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblCompanyIdEdit" runat="server" Text='<%#Eval("COMPID") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BRANCHCD" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchCd" runat="server" Text='<%# Eval("BRANCHCD") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblBranchCdEdit" runat="server" Text='<%#Eval("BRANCHCD") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Office Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BRANCHNM") %>' Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtBranchNameEdit" runat="server" Text='<%#Eval("BRANCHNM") %>' Width="98%"
                                                TabIndex="6" CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtBranchName" runat="server" TabIndex="7"
                                                CssClass="form-control input-sm"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Short Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchId" runat="server" Text='<%# Eval("BRANCHID") %>' Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtBranchIdEdit" runat="server" Text='<%#Eval("BRANCHID") %>' Width="98%"
                                                TabIndex="6" CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtBranchId" runat="server" TabIndex="7"
                                                CssClass="form-control input-sm"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ADDRESS") %>' Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAddressEdit" runat="server" Text='<%#Eval("ADDRESS") %>' Width="98%"
                                                TabIndex="6" CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAddress" runat="server" TabIndex="7"
                                                CssClass="form-control input-sm"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Contact No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContactNo" runat="server" Text='<%# Eval("CONTACTNO") %>' Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtContactNoEdit" runat="server" Text='<%#Eval("CONTACTNO") %>' Width="98%"
                                                TabIndex="6" CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtContactNo" runat="server" TabIndex="7"
                                                CssClass="form-control input-sm"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Email">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("EMAILID") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEmailEdit" runat="server" Text='<%#Eval("EMAILID") %>' TabIndex="15"
                                                CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEmail" runat="server" TabIndex="8"
                                                CssClass="form-control input-sm" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("STATUS") %>' Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlStatusEdit" CssClass="form-control input-sm" TabIndex="16">
                                                <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control input-sm" TabIndex="9">
                                                <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:TemplateField>


                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <% if (UserPermissionChecker.checkParmit("/Accounts/UI/BranchEntry.aspx", "UPDATER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit"
                                                Height="20px" ImageUrl="~/Images/Edit.png" TabIndex="30" ToolTip="Edit"
                                                Width="20px" />
                                            <% } %>
                                            <% if (UserPermissionChecker.checkParmit("/Accounts/UI/BranchEntry.aspx", "DELETER") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete"
                                                Height="20px" ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()"
                                                TabIndex="31" Text="Edit" ToolTip="Delete" Width="20px" />
                                            <% } %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update"
                                                Height="20px" ImageUrl="~/Images/update.png" TabIndex="17" ToolTip="Update"
                                                Width="20px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel"
                                                Height="20px" ImageUrl="~/Images/Cancel.png" TabIndex="18" ToolTip="Cancel"
                                                Width="20px" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <% if (UserPermissionChecker.checkParmit("/Accounts/UI/BranchEntry.aspx", "INSERTR") == true)
                                                { %>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon"
                                                Height="20px" ImageUrl="~/Images/AddNewitem.png" TabIndex="10"
                                                ToolTip="Add new Record" ValidationGroup="validaiton" Width="20px" />
                                            <% } %>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
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
