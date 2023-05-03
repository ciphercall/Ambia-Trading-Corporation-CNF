<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="itemInformation.aspx.cs" Inherits="alchemySoft.stock.ui.itemInformation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Item Information</title>
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script>
        function pageLoad() {
            Search_GetCompletionListItemMasterCategoryName();
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function Search_GetCompletionListItemMasterCategoryName() {
            $("#<%=txtCategoryNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListItemMasterCategoryName",
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
            });
        }
    </script>
    <style>
        .ui-autocomplete {
            max-width: 350px;
            max-height: 250px;
            overflow: auto;
        }

        .file-upload {
            position: relative;
            overflow: hidden;
            margin: 10px;
        }

            .file-upload input.file-input {
                position: absolute;
                top: 0;
                right: 0;
                margin: 0;
                padding: 0;
                font-size: 20px;
                cursor: pointer;
                opacity: 0;
                filter: alpha(opacity=0);
            }

        .img1 {
            /*float: right;
            position: absolute;
            right: 153px;
            top: 200px;
            z-index: 1000;*/
            width: 50px;
            height: 41px;
        }
        /*hover image*/


        .image {
            width: 100%;
            height: 100%;
        }

        .img1 {
            -webkit-transition: all 1s ease !important; /* Safari and Chrome */
            -moz-transition: all 1s ease !important; /* Firefox */
            -ms-transition: all 1s ease !important; /* IE 9 */
            -o-transition: all 1s ease !important; /* Opera */
            transition: all 1s ease !important;
        }

            .img1:hover {
                z-index: 10;
                -webkit-transform: scale(3.2) !important; /* Safari and Chrome */
                -moz-transform: scale(3.2) !important; /* Firefox */
                -ms-transform: scale(3.2) !important; /* IE 9 */
                -o-transform: scale(3.2) !important; /* Opera */
                transform: scale(3.2) !important;
                cursor: pointer !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="asd" runat="server">
        <ContentTemplate>
            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">

                    <div id="contentHeaderBox">
                        <h1>ITEM INFORMATION</h1>


                    </div>
                    <!-- content header end -->

                    <asp:Label ID="lblCatID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblMaxCatID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblChkItemID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblChkCatID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblIMaxItemID" runat="server" Visible="False"></asp:Label>

                    <!-- Content Start From here -->
                    <div class="content_wrapper  col-md-12">
                        <br />
                        <br />
                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <strong>Category Name :</strong>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCategoryNM" runat="server" CssClass="form-control input-sm" AutoPostBack="true" OnTextChanged="txtCategoryNM_TextChanged" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="Search" runat="server" Font-Bold="True" Font-Italic="True" CssClass="btn btn-info"
                                    Text="Search" OnClick="Search_Click" />
                            </div>

                        </div>
                        <br />
                        <div class="table table-responsive table-hover gridRoot">
                            <asp:GridView ID="gvDetails" runat="server" CssClass="Grid" AutoGenerateColumns="False" ShowFooter="True"
                                OnRowCancelingEdit="gvDetails_RowCancelingEdit" OnRowDeleting="gvDetails_RowDeleting"
                                OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating"
                                OnRowCommand="gvDetails_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ITEMID") %>' Width="100%"
                                                Style="text-align: center" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblItemID" runat="server" Text='<%#Eval("ITEMID") %>' Width="100%"
                                                Style="text-align: center" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemNM" runat="server" Text='<%# Eval("ITEMNM") %>' Width="100%"
                                                Style="text-align: left" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtItemNMEdit" runat="server" Text='<%#Eval("ITEMNM") %>' CssClass="form-control input-sm" TabIndex="10" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtItemNMFooter" runat="server" CssClass="form-control input-sm" Width="100%" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="20%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemCD" runat="server" Text='<%#Eval("ITEMCD") %>' Style="text-align: center" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtItemCDEdit" runat="server" Width="100%" Text='<%#Eval("ITEMCD") %>' Style="text-align: right" CssClass="form-control input-sm" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtItemCDFooter" runat="server" Style="text-align: left" Width="100%" CssClass="form-control input-sm" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="10%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColor" runat="server" Text='<%#Eval("COLOR") %>' Style="text-align: center" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtColorEdit" runat="server" Text='<%#Eval("COLOR") %>' Style="text-align: right" Width="100%" CssClass="form-control input-sm" />
                                            <%-- <asp:AutoCompleteExtender
                                                ID="txtBrandEditPFooterAutoCompleteExtender" runat="server"
                                                CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                CompletionInterval="10" CompletionSetCount="3" Enabled="True"
                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionCOLORList" 
                                                TargetControlID="txtBrandEdit" UseContextKey="True">
                                            </asp:AutoCompleteExtender>--%>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtColorFooter" runat="server" Style="text-align: left" CssClass="form-control input-sm" />
                                            <%--<asp:AutoCompleteExtender
                                                ID="txtBrandAutoCompleteExtender" runat="server"
                                                CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                CompletionInterval="10" CompletionSetCount="3" Enabled="True"
                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionCOLORList" 
                                                TargetControlID="txtBrand" UseContextKey="True">
                                            </asp:AutoCompleteExtender>--%>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" Width="10%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Buy Rate">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtBuyRTEdit" runat="server" Style="text-align: right" Text='<%#Eval("BUYRT") %>' Width="100%"
                                                CssClass="form-control input-sm">0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtBuyRT" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuyRT" runat="server" Width="100%" Style="text-align: right" Text='<%#Eval("BUYRT") %>'>0</asp:Label>
                                        </ItemTemplate>

                                        <FooterStyle HorizontalAlign="Right" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sale Rate">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSaleRTEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right"
                                                Text='<%#Eval("SALERT") %>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtSaleRT" runat="server" CssClass="form-control input-sm" Style="text-align: right"
                                                Width="100%"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaleRT" runat="server" Style="text-align: right" Text='<%#Eval("SALERT") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Min. Qty">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMinsQtyEdit" runat="server" CssClass="form-control input-sm" Style="text-align: right" Width="100%"
                                                Text='<%#Eval("MINSQTY") %>'>0</asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtMinsQtyFooter" runat="server" Style="text-align: right" CssClass="form-control input-sm" Width="100%">0</asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblMisQty" runat="server" Style="text-align: right" Width="100%" Text='<%#Eval("MINSQTY") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
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
                                                ToolTip="Update" Height="20px" Width="20px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Images/Cancel.png"
                                                ToolTip="Cancel" Height="20px" Width="20px" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Images/Edit.png"
                                                ToolTip="Edit" Height="20px" Width="20px" />
                                            <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" Text="Edit" runat="server"
                                                ImageUrl="~/Images/delete.png" ToolTip="Delete" Height="20px" Width="20px" OnClientClick="return confMSG()" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Images/AddNewitem.png"
                                                CommandName="AddNew" Width="35px" Height="35px" ToolTip="Add new Record" ValidationGroup="validaiton" />
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
                                <asp:Label ID="lblErrorMSG" runat="server" width="100%" Visible="False"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <!-- Content End From here -->
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
