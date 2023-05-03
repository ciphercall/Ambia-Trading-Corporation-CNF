<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Cnf-Party-Rate.aspx.cs" Inherits="alchemySoft.CNF.UI.Cnf_Party_Rate" %>

<%@ Import Namespace="alchemySoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>:: Cnf Party Rate ::</title>
    <%--<link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
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
            GetCompletionListPartyName();
            GetCompletionListExpensePerticulars();
        }

        function GetCompletionListPartyName() {
            $("#<%=txtPartNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListPartyName",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("#<%=txtPartNM.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Request.Cookies["USERTYPE"]%>','brCD' : '<%=HttpContext.Current.Request.Cookies["COMPANYID"]%>'}",
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
                        error: function () {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    $("#<%=txtPartyID.ClientID %>").val(ui.item.x);
                    $("#<%=ddlRegID.ClientID %>").focus();
                    return true;
                }
            });
        }

        function GetCompletionListExpensePerticulars() {
            $("[id*=txtEXPNMFooter],[id*=txtEXPNMEdit]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListExpensePerticulars",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtEXPNMFooter],[id*=txtEXPNMEdit]").val() + "'}",
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
                minLength: 1
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
                        <h1>Party Wise Quotation Rate</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <%-- ReSharper disable once Html.IdDuplication --%>
                        <%--     <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-print" ID="btnPartyReport" runat="server" OnClick="btnPartyReport_Click" Text="All Party Information"></asp:LinkButton></a>
                                </li>
                            </ul>
                        </div>--%>
                        <!-- end logout option -->


                    </div>

                    <div class="content_wrapper col-md-12">
                        <div class="row form-class3px">
                            <div class="col-md-1" style="width: 104px">Party Name</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtPartNM" runat="server" CssClass="form-control input-sm" AutoPostBack="True" OnTextChanged="txtPartNM_OnTextChanged"></asp:TextBox>
                                <asp:TextBox ID="txtPartyID" runat="server" Style="display: none" Width="53px"></asp:TextBox>
                                <asp:Label ID="lblPartyID" runat="server" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-1" style="width: 104px">Register ID</div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlRegID" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem>CHITTAGONG</asp:ListItem>
                                    <asp:ListItem>COMILLA</asp:ListItem>
                                    <asp:ListItem>BENAPOLE</asp:ListItem>
                                    <asp:ListItem>DEPZ</asp:ListItem>
                                    <asp:ListItem>ICD</asp:ListItem>
                                    <asp:ListItem>AEPZ</asp:ListItem>
                                    <asp:ListItem>AIRPORT</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-1" style="width: 104px">Job Quality</div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlJobQuality" runat="server" CssClass="form-control input-sm" TabIndex="3" Width="130px">
                                    <asp:ListItem>FABRICS</asp:ListItem>
                                    <asp:ListItem>RAW MATERIALS</asp:ListItem>
                                    <asp:ListItem>ACCESSORIES</asp:ListItem>
                                    <asp:ListItem>DYES & CHEMICALS</asp:ListItem>
                                    <asp:ListItem>GENERAL BONDS</asp:ListItem>
                                    <asp:ListItem>MACHINERY</asp:ListItem>
                                    <asp:ListItem>GARMENTS</asp:ListItem>
                                    <asp:ListItem>COURIER ABOVE</asp:ListItem>
                                    <asp:ListItem>COURIER BELOW</asp:ListItem>
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />


                        <div class="panel panel-default" style="border-color: #5cb85c; padding-left: 4px; padding-right: 4px; padding-bottom: 2px">

                            <asp:GridView ID="gv_Party" runat="server" BackColor="White" BorderColor="White"
                                BorderStyle="Ridge" BorderWidth="2px" CssClass="Grid" Width="100%" AutoGenerateColumns="False" ShowFooter="True" Style="text-align: left"
                                OnRowCommand="gv_Party_RowCommand" OnRowDeleting="gv_Party_RowDeleting" OnRowEditing="gv_Party_RowEditing"
                                OnRowUpdating="gv_Party_RowUpdating" OnRowCancelingEdit="gv_Party_RowCancelingEdit">
                                <Columns>
                                    <asp:TemplateField HeaderText="Expense Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEXPNM" runat="server" Text='<%# Eval("EXPNM") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEXPNMEdit" runat="server" CssClass="form-control input-sm" Width="100%" Text='<%# Eval("EXPNM") %>' AutoPostBack="True" OnTextChanged="txtEXPNMEdit_TextChanged"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEXPNMFooter" runat="server" Width="100%" CssClass="form-control input-sm" AutoPostBack="True" OnTextChanged="txtEXPNMFooter_TextChanged"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" Font-Size="14px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expence ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEXPID" runat="server" Text='<%# Eval("EXPID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEXPIDEdit" runat="server" Text='<%# Eval("EXPID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblEXPIDFooter" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlRATETPEdit" CssClass="form-control input-sm" AutoPostBack="true" runat="server" Width="100%" Text='<%# Eval("RATETP") %>' OnSelectedIndexChanged="ddlRATETPEdit_SelectedIndexChanged">
                                                <asp:ListItem>FIXED</asp:ListItem>
                                                <asp:ListItem>KGS</asp:ListItem>
                                                <asp:ListItem>PKTS</asp:ListItem>
                                                <asp:ListItem>CBM</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlRATETPFooter" CssClass="form-control input-sm" AutoPostBack="true" runat="server" Width="100%" OnSelectedIndexChanged="ddlRATETPFooter_SelectedIndexChanged">
                                                <asp:ListItem>FIXED</asp:ListItem>
                                                <asp:ListItem>KGS</asp:ListItem>
                                                <asp:ListItem>PKTS</asp:ListItem>
                                                <asp:ListItem>CBM</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRATETP" runat="server" Text='<%# Eval("RATETP") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="14px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRATEEdit" runat="server" CssClass="form-control input-sm" Text='<%# Eval("RATE") %>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtRATEFooter" runat="server" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRATE" runat="server" Text='<%# Eval("RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="14px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remarks">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtREMARKSEdit" runat="server" CssClass="form-control input-sm" Text='<%# Eval("REMARKS") %>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtREMARKSFooter" runat="server" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblREMARKS" runat="server" Text='<%# Eval("REMARKS") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="14px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Cnf-Party-Rate.aspx", "UPDATER"))
                                                { %>
                                            <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" Height="20px"
                                                ImageUrl="~/Images/update.png" TabIndex="34" ToolTip="Update" Width="20px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" Height="20px"
                                                ImageUrl="~/Images/Cancel.png" TabIndex="35" ToolTip="Cancel" Width="20px" />
                                            <% } %>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Cnf-Party-Rate.aspx", "INSERTR"))
                                                { %>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon"
                                                Height="25px" ImageUrl="~/Images/AddNewitem.png" TabIndex="15" ToolTip="Save &amp; Continue"
                                                ValidationGroup="validaiton" Width="25px" />
                                            <% } %>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Cnf-Party-Rate.aspx", "UPDATER"))
                                                { %>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" Height="20px"
                                                ImageUrl="~/Images/Edit.png" TabIndex="100" ToolTip="Edit" Width="20px" />
                                            <% } %>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Cnf-Party-Rate.aspx", "DELETER"))
                                                { %>
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" Height="20px"
                                                ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()" TabIndex="101"
                                                ToolTip="Delete" Width="21px" />
                                            <% } %>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                        <ItemStyle HorizontalAlign="Center" Width="6%" />
                                    </asp:TemplateField>

                                </Columns>
                                <EditRowStyle Font-Size="14px" />
                                <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="13px" BackColor="#D9EDF7" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
