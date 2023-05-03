<%@ Page  Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="purchase_.aspx.cs" Inherits="alchemySoft.stock.ui.purchase_" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Receive Information</title>
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
                        <h1>PURCHASE INFORMATION</h1>
                    </div>
                    <!-- content header end -->
                    <!-- Content Start From here -->
                    <div class="content_wrapper  col-md-12">
                        <br />
                        <br />
                        <div class="col-md-7">
                            <div class="col-md-12">
                                <div class="row form-class3px">
                                    <div class="col-md-2">
                                        Date :
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtDT" runat="server" AutoPostBack="True" Width="100%"
                                            CssClass="form-control input-sm" OnTextChanged="txtDT_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1"></div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnEdit" Width="100%" runat="server" CssClass="form-control input-sm btn-primary" Font-Bold="True" OnClick="btnEdit_Click"
                                            Text="Edit ∓" />
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">
                                        Invoice No :
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTransNo" runat="server" Width="100%" CssClass="form-control input-sm" ReadOnly="True"></asp:TextBox>
                                        <asp:DropDownList ID="ddlEditTransNo" runat="server" Width="100%" AutoPostBack="True"
                                            CssClass="form-control input-sm" Visible="False" OnSelectedIndexChanged="ddlEditOrderNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        Job:
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlJob" runat="server" Width="100%"
                                            CssClass="select">
                                        </asp:DropDownList>
                                    </div>
                                </div> 
                                <div class="row form-class3px">
                                    <div class="col-md-2">
                                        Dept. To :  
                                    </div>
                                    <div class="col-md-8" style="margin-bottom: 2px">
                                        <asp:DropDownList ID="ddlStoreFr" runat="server" Visible="false" CssClass="select" Width="100%"></asp:DropDownList>
                                        <asp:DropDownList ID="ddlStoreTo" runat="server" CssClass="select" Width="100%"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row form-class3px">
                                    <div class="col-md-2">
                                        Remarks : 
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtRemarks" runat="server" Width="100%"  CssClass="form-control input-sm"></asp:TextBox>
                                    </div> 
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="col-md-12">
                                <br /><br /> 
                                <div class="row form-class3px">
                                    <div class="col-md-4">  
                                        Transmode :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="txtTransMd" runat="server" Width="100%"  
                                            CssClass="form-control">
                                            <asp:ListItem Value="RM">RAW MATERIALS</asp:ListItem>
                                            <asp:ListItem Value="FG">FINISHED GOODS</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                 <asp:TextBox ID="txtLot" runat="server" Width="100%" Visible="false"
                                            CssClass="form-control"></asp:TextBox>
                                        <asp:TextBox ID="txtBatch" runat="server" Width="100%"  Visible="false"
                                            CssClass="form-control"></asp:TextBox>
                                 <div class="row form-class3px">
                                    <div class="col-md-4">
                                        Machine Name :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtMachine" runat="server" Width="100%"
                                            CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <br />
                                <div class="col-md-1"></div> 
                                <div class="col-md-1">
                                    <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Width="100%" CssClass="form-control input-sm btn-primary"
                                        Text="Print ⏢" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnUpdate" runat="server"  Font-Bold="True" Width="100%" CssClass="form-control input-sm btn-primary"
                                        Text="Update ⤤⤦" OnClick="btnUpdate_Click" Enabled="False" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnRefresh" runat="server"  Font-Bold="True" Width="100%" CssClass="btn btn-info"
                                        Text="Refresh ✓" OnClick="btnRefresh_Click" />
                                </div>
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
                                    <asp:TemplateField HeaderText="Item">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemNM" runat="server" Text='<%#Eval("ITEMNM") %>' Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlItemNMEdit" runat="server" CssClass="select" Width="100%" Height="30px"></asp:DropDownList>
                                            <asp:Label ID="lblItemID" runat="server" Text='<%#Eval("ITEMID") %>' Visible="false" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlItemNMFooter" runat="server" CssClass="select" Width="100%" Height="30px"></asp:DropDownList>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Color">
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
                                    </asp:TemplateField>--%>
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
                                    <asp:TemplateField HeaderText="Lot/Batch">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLot" runat="server" Text='<%# Eval("PQTY") %>' Width="100%"
                                                Style="text-align: left" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtLotEdit" runat="server" Text='<%#Eval("PQTY") %>' CssClass="form-control input-sm" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLotFooter" runat="server" CssClass="form-control input-sm" Width="100%"  Enabled="false"></asp:TextBox>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="10%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Qty">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQtyEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("QTY") %>' AutoPostBack="true" OnTextChanged="txtQtyEdit_TextChanged">0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtQtyFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%" AutoPostBack="true" OnTextChanged="txtQtyFooter_TextChanged">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("QTY") %>' AutoPostBack="true" OnTextChanged="txtQtyFooter_TextChanged"></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRateEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("RATE") %>' AutoPostBack="true" OnTextChanged="txtRateEdit_TextChanged">0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtRateFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%" AutoPostBack="true" OnTextChanged="txtRateFooter_TextChanged">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAmountEdit" runat="server" ReadOnly="true" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("AMOUNT") %>'>0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAmountFooter" ReadOnly="true" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
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
