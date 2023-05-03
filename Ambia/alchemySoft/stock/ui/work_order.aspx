<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="work_order.aspx.cs" Inherits="alchemySoft.stock.ui.work_order" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Item Information</title>
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />

    <script>

        function confMSG() {
            if (confirm("Are you Sure to Delete?")) {
                return true;
            }
            else {
                return false;
            }
        }

    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="asd" runat="server">
        <ContentTemplate>
            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">

                    <div id="contentHeaderBox">
                        <h1>WORK ORDER INFORMATION</h1>


                    </div>
                    <!-- content header end -->

                    <!-- Content Start From here -->
                    <div class="content_wrapper  col-md-12">
                        <br />
                        <br />
                        <div class="row form-class3px">
                            <div class="col-md-2">
                                Date :
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtDT" runat="server" AutoPostBack="True" Width="100%"
                                    CssClass="form-control input-sm" OnTextChanged="txtDT_TextChanged"></asp:TextBox>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnEdit" Width="100%" runat="server" CssClass="form-control input-sm btn-primary" Font-Bold="True" OnClick="btnEdit_Click"
                                    Text="Edit ∓" />
                            </div>
                        </div>

                        <div class="row form-class3px">
                            <div class="col-md-2">
                                Order No :
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtOrderNo" runat="server" Width="100%" CssClass="form-control input-sm" ReadOnly="True"></asp:TextBox>
                                <asp:DropDownList ID="ddlEditOrderNo" runat="server" Width="100%" AutoPostBack="True"
                                    CssClass="form-control input-sm" Visible="False" OnSelectedIndexChanged="ddlEditOrderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                Job No :
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtJobNo" Width="100%" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class3px">
                            <div class="col-md-2">
                                Party Name : 
                            </div>
                            <div class="col-md-6" style="margin-bottom: 2px">
                                <asp:DropDownList ID="ddlPartyNM" runat="server" CssClass="select" Width="100%"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row form-class3px">
                            <div class="col-md-2">
                                Remarks : 
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TabIndex="100" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-1"></div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnPrint" runat="server" TabIndex="203" Font-Bold="True" Width="100%" CssClass="form-control input-sm btn-primary"
                                    Text="Print ⏢" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnUpdate" runat="server" TabIndex="203" Font-Bold="True" Width="100%" CssClass="form-control input-sm btn-primary"
                                    Text="Update ⤤⤦" OnClick="btnUpdate_Click" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnRefresh" runat="server" TabIndex="203" Font-Bold="True" Width="100%" CssClass="btn btn-info"
                                    Text="Refresh ✓" OnClick="btnRefresh_Click" />
                            </div>
                        </div>
                        <br />
                        <div class="col-md-12">
                            <asp:Label ID="lblErrorMaster" runat="server" Width="100%" Visible="False"></asp:Label>
                            <asp:Label ID="lblmYear" runat="server" Width="100%" Visible="False"></asp:Label>
                        </div>
                        <br />
                        <div class="table table-responsive table-hover gridRoot">
                            <asp:GridView ID="gvDetails" runat="server" CssClass="Grid" AutoGenerateColumns="False" ShowFooter="True"
                                OnRowCancelingEdit="gvDetails_RowCancelingEdit" OnRowDeleting="gvDetails_RowDeleting" Width="100%"
                                OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating"
                                OnRowCommand="gvDetails_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSL" runat="server" Text='<%# Eval("SL") %>' Width="100%"
                                                Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblSLEdit" runat="server" Text='<%#Eval("SL") %>' Width="100%"
                                                Style="text-align: center" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                     
                                    <asp:TemplateField HeaderText="Fabrication">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFabrication" runat="server" Text='<%# Eval("FABRIC") %>' Width="100%"
                                                Style="text-align: left" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFabricationEdit" runat="server" Text='<%#Eval("FABRIC") %>' CssClass="form-control input-sm"  Width="100%"  />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFabricationFooter" runat="server" CssClass="form-control input-sm" Width="100%" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="12%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemNM" runat="server" Text='<%#Eval("ITEMNM") %>' Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlItemNMEdit"  AutoPostBack="true" OnSelectedIndexChanged="ddlItemNMEdit_SelectedIndexChanged" runat="server" CssClass="select" Width="100%" Height="30px"></asp:DropDownList>
                                            <asp:Label ID="lblItemID" runat="server" Text='<%#Eval("ITEMID") %>' Visible="false" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlItemNMFooter" AutoPostBack="true" OnSelectedIndexChanged="ddlItemNMFooter_SelectedIndexChanged" runat="server" CssClass="select" Width="100%" Height="30px"></asp:DropDownList>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColor" runat="server" Text='<%#Eval("COLOR") %>' Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblColorEdit" Height="30px" runat="server" Text='<%#Eval("COLOR") %>' Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblColorFooter" Height="30px" CssClass="form-control" Style="margin-bottom: -6px;" runat="server" Width="100%" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="10%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Panton">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPantonEdit" runat="server" Text='<%#Eval("PANTON") %>' Width="100%"
                                                CssClass="form-control input-sm">0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPantonFooter" runat="server" CssClass="form-control input-sm"  Width="100%"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPanton" runat="server" Width="100%" Text='<%#Eval("PANTON") %>'></asp:Label>
                                        </ItemTemplate> 
                                        <FooterStyle HorizontalAlign="Right" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>--%>
                                    <%--<asp:TemplateField HeaderText="Work Type">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtWorkTPEdit" runat="server" CssClass="form-control input-sm"  Text='<%#Eval("WORKTP") %>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtWorkTPFooter" runat="server" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkTP" runat="server" Style="text-align: right" Text='<%#Eval("WORKTP") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Style">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtStyleEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("STYLENO") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtStyleFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStyle" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("STYLENO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="6%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                        <ItemStyle HorizontalAlign="Left" Width="6%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Finish Qty">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFinishQtyEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("QTY") %>'>0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFinishQtyFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinishQty" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("QTY") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Y %">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtWpcntEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("WPCNT") %>' AutoPostBack="true" OnTextChanged="txtWpcntEdit_TextChanged">0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtWpcntFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%"
                                                 AutoPostBack="true" OnTextChanged="txtWpcntFooter_TextChanged">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblWpcnt" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("WPCNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="YARN">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtYarnEdit" runat="server" ReadOnly="true" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("YARN") %>'>0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtYarnFooter" ReadOnly="true" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblYarn" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("YARN") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total PX">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTtlPxEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("TTLPX") %>'>0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtTtlPxFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTtlPx" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("TTLPX") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="6%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                        <ItemStyle HorizontalAlign="Left" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRemarksEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("REMARKS") %>'>0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtRemarksFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/Images/update.png"
                                                ToolTip="Update" Height="30px" Width="30px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/Cancel.png"
                                                ToolTip="Cancel" Height="30px" Width="30px" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Images/Edit.png"
                                                ToolTip="Edit" Height="30px" Width="30px" />
                                            <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" Text="Edit" runat="server"
                                                ImageUrl="~/Images/delete.png" ToolTip="Delete" Height="30px" Width="30px" OnClientClick="return confMSG()" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Images/AddNewitem.png"
                                                CommandName="AddNew" Width="30px" Height="30px" ToolTip="Add new Record" />
                                        </FooterTemplate>
                                        <FooterStyle Width="5%" HorizontalAlign="Center" />
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-12">
                                <asp:Label ID="lblErrorMSG" runat="server" Width="100%" Visible="False"></asp:Label>
                            </div>
                        </div>
                        <!-- Content End From here -->
                    </div>
                </div>
                <script src="../../MenuCssJs/js/select2.full.min.js"></script>
                <link href="../../MenuCssJs/css/select2.min.css" rel="stylesheet" />
                <script>
                    function pageLoad() {
                        $(<%=txtDT.ClientID%>).datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });
            $(".select").select2();
        }
                </script>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
