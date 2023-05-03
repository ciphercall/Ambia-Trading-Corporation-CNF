<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="StoreEntry.aspx.cs" Inherits="DynamicMenu.Stock.UI.StoreEntry" %>

<%@ Import Namespace="alchemySoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) {
                //                alert("Clicked Yes");
            }
            else {
                //                alert("Clicked No");
                return false;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>STORE ENTRY</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


            </div> 
            <div class="content_wrapper  col-md-12">
                 <br />
            <br />
                <div class="row">
                    <asp:Label ID="lblMaxStID" runat="server"></asp:Label>
                    <asp:Label ID="lblSTID" runat="server"></asp:Label>
                </div>
                <%--<div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-4"></div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2"></div>
                    <div class="col-md-5"></div>
                </div>--%>

                <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                    <asp:GridView ID="gvDetails" runat="server"  CssClass="Grid"
                         Width="100%" AutoGenerateColumns="False" ShowFooter="True" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                        OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating"
                        OnRowCommand="gvDetails_RowCommand" OnRowDataBound="gvDetails_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Store ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblSTID" runat="server" Text='<%# Eval("STOREID") %>' Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSTID" runat="server" Text='<%# Eval("STOREID") %>' Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>

                                <FooterStyle Width="8%" HorizontalAlign="Center" />
                                <HeaderStyle Width="8%" HorizontalAlign="Center" />
                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Store Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblSTNM" runat="server" Text='<%# Eval("STORENM") %>' Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSTNMEdit" runat="server" Width="100%" Text='<%#Eval("STORENM") %>' CssClass="form-control input-sm"
                                        TabIndex="6" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSTNM" runat="server" Width="100%"  CssClass="form-control input-sm" TabIndex="1" />
                                </FooterTemplate>

                                <FooterStyle Width="30%" HorizontalAlign="Left" />
                                <HeaderStyle Width="30%" HorizontalAlign="Center" />
                                <ItemStyle Width="30%" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Address">
                                <ItemTemplate>
                                    <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("ADDRESS") %>' Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAddressEdit" runat="server"  Width="100%" Text='<%#Eval("ADDRESS") %>' CssClass="form-control input-sm"
                                        TabIndex="7" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAddress" runat="server" Width="100%"  CssClass="form-control input-sm" TabIndex="2" />
                                </FooterTemplate>

                                <FooterStyle HorizontalAlign="Left" Width="25%" />
                                <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact No">
                                <ItemTemplate>
                                    <asp:Label ID="lblContact" runat="server"  Text='<%#Eval("CONTACTNO") %>' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtContactEdit" runat="server"  Width="100%" Text='<%#Eval("CONTACTNO") %>' CssClass="form-control input-sm"
                                        TabIndex="8" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtContact" runat="server"  Width="100%" TabIndex="3" CssClass="form-control input-sm" />
                                </FooterTemplate>

                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("REMARKS") %>' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRemarksEdit" runat="server"  Width="100%" Text='<%#Eval("REMARKS") %>' CssClass="form-control input-sm"
                                        TabIndex="9" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%"  CssClass="form-control input-sm" TabIndex="4" />
                                </FooterTemplate>

                                <FooterStyle HorizontalAlign="Center" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/Images/update.png"
                                        ToolTip="Update" Height="20px" Width="20px" TabIndex="10" />
                                    <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/Cancel.png"
                                        ToolTip="Cancel" Height="20px" Width="20px" TabIndex="11" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <% if (dbFunctions.checkParmit("/stock/ui/storeEntry", "UPDATER") == true)
                                       { %>
                                    <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Images/Edit.png"
                                        ToolTip="Edit" Height="20px" Width="20px" TabIndex="101" />
                                    <% } %>
                                    <% if (dbFunctions.checkParmit("/stock/ui/storeEntry", "DELETER") == true)
                                       { %>
                                    <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" Text="Edit" runat="server"
                                        ImageUrl="~/Images/delete.png" ToolTip="Delete" Height="20px" Width="20px" OnClientClick="return confMSG()"
                                        TabIndex="111" />
                                    <% } %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (dbFunctions.checkParmit("/stock/ui/storeEntry", "INSERTR") == true)
                                       { %>
                                    <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Images/AddNewitem.png"
                                        CommandName="AddNew" Width="30px" Height="30px" ToolTip="Add new Record" ValidationGroup="validaiton"
                                        TabIndex="5" />
                                    <% } %>
                                </FooterTemplate>
                                <FooterStyle Width="5%" HorizontalAlign="Right" />
                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="text-center" BorderColor="Gray" BorderWidth="2px" HorizontalAlign="Center" Height="30px" Font-Size="13px" BackColor="#337AB7" ForeColor="White" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    </asp:GridView>
                </div>
            </div>
            <!-- Content End From here -->
        </div>
    </div>
</asp:Content>
