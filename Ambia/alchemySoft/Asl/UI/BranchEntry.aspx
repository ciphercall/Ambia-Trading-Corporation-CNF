<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="BranchEntry.aspx.cs" Inherits="alchemySoft.Asl.UI.BranchEntry" %>

<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>--%>

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
                        <br />

                    </div>
                    <!-- content header end -->

                    <br />
                    <br />
                    <!-- Content Start From here -->
                    <div class="form-class content_wrapper">
                        <div class="col-md-12">
                            
                            <asp:DropDownList runat="server" ID="ddlCompanyName" AutoPostBack="true"  CssClass="form-control select2" TabIndex="2" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <div class="table table-responsive table-hover gridRoot">
                                <asp:GridView ID="gridViewForBranch" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                    CssClass="Grid" OnRowCancelingEdit="gridViewForBranch_RowCancelingEdit"
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
                                                <asp:TextBox ID="txtBranchNameEdit" runat="server" Text='<%#Eval("BRANCHNM") %>' Width="100%"
                                                    TabIndex="6" CssClass="form-control input-sm" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtBranchName" runat="server" TabIndex="7" Width="100%"
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
                                                <asp:TextBox ID="txtBranchIdEdit" runat="server" Text='<%#Eval("BRANCHID") %>' Width="100%"
                                                    TabIndex="6" CssClass="form-control input-sm" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtBranchId" runat="server" TabIndex="7" Width="100%"
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
                                                <asp:TextBox ID="txtAddressEdit" runat="server" Text='<%#Eval("ADDRESS") %>' Width="100%"
                                                    TabIndex="6" CssClass="form-control input-sm" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtAddress" runat="server" TabIndex="7" Width="100%"
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
                                                <asp:TextBox ID="txtContactNoEdit" runat="server" Text='<%#Eval("CONTACTNO") %>' Width="100%"
                                                    TabIndex="6" CssClass="form-control input-sm" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtContactNo" runat="server" TabIndex="7" Width="100%"
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
                                                <asp:TextBox ID="txtEmailEdit" runat="server" Text='<%#Eval("EMAILID") %>' TabIndex="15" Width="100%"
                                                    CssClass="form-control input-sm" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtEmail" runat="server" TabIndex="8" Width="100%"
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
                                                <asp:DropDownList runat="server" ID="ddlStatusEdit" Width="100%" CssClass="form-control input-sm" TabIndex="16">
                                                    <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                    <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList runat="server" ID="ddlStatus" Width="100%" CssClass="form-control input-sm" TabIndex="9">
                                                    <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                                    <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:TemplateField>


                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit"
                                                    Height="30px" ImageUrl="~/Images/Edit.png" TabIndex="30" ToolTip="Edit"
                                                    Width="30px" />
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete"
                                                    Height="30px" ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()"
                                                    TabIndex="31" Text="Edit" ToolTip="Delete" Width="30px" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update"
                                                    Height="30px" ImageUrl="~/Images/update.png" TabIndex="17" ToolTip="Update"
                                                    Width="30px" />
                                                <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel"
                                                    Height="30px" ImageUrl="~/Images/Cancel.png" TabIndex="18" ToolTip="Cancel"
                                                    Width="30px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon"
                                                    Height="30px" ImageUrl="~/Images/AddNewitem.png" TabIndex="10"
                                                    ToolTip="Add new Record" ValidationGroup="validaiton" Width="30px" />
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            <FooterStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <!-- Content End From here -->
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
