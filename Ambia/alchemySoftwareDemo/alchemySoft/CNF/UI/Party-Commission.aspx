<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Party-Commission.aspx.cs" Inherits="alchemySoft.CNF.UI.Party_Commission" %>

<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>:: Party Commission Entry ::</title>
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
        }

        function GetCompletionListPartyName() {
            $("#<%=txtParty.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListPartyName",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("#<%=txtParty.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Session["USERTYPE"]%>','brCD' : '<%=HttpContext.Current.Session["BrCD"]%>'}",
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
                        <h1>Party Commission Entry</h1>
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
                    <!-- content header end -->
                    <div class="form-class text-center">
                        <asp:Label runat="server" ID="lblErrMsg" Visible="False" ForeColor="#990000" Style="font-weight: 700"></asp:Label>
                        <asp:Label ID="lblErrMsgExist" runat="server" ForeColor="#990000" Visible="False"></asp:Label>
                        <asp:Label ID="lblChkInternalID" runat="server" ForeColor="#990000" Visible="False"></asp:Label>
                    </div>
                    <asp:Label ID="lblValCommPer" runat="server" ForeColor="#990000"
                        Visible="False"></asp:Label>
                    <asp:Label ID="lblValTP" runat="server" ForeColor="#990000" Visible="False"></asp:Label>

                    <div class="content_wrapper col-md-12">
                        <div class="row form-class3px">
                            <div class="col-md-1" style="width: 104px">Party Name</div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtParty" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:TextBox ID="txtPartyID" runat="server" Style="display: none" Width="53px"></asp:TextBox> 
                            </div>
                            <div class="col-md-1" style="width: 104px">Register ID</div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlRegID" runat="server" AutoPostBack="True" CssClass="form-control input-sm"
                                    OnSelectedIndexChanged="ddlRegID_SelectedIndexChanged">
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
                                <asp:DropDownList ID="ddlJobQuality" runat="server" AutoPostBack="True" CssClass="form-control input-sm"
                                    OnSelectedIndexChanged="ddlJobQuality_SelectedIndexChanged" TabIndex="3" Width="130px">
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
                            <div class="col-md-1">
                                <asp:Button runat="server" ID="btnSubmit" CssClass="form-control input-sm btn-primary" OnClick="btnSubmit_OnClick" Text="Submit" />
                            </div>
                        </div><br/>
                        <div class="panel panel-default" style="border-color: #5cb85c; padding-left: 4px; padding-right: 4px; padding-bottom: 2px">
                            
                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True" BackColor="White" GridLines="Both"
                                BorderStyle="None" CellPadding="4" CssClass="Grid" Width="100%" Font-Size="11px"
                                OnRowDataBound="gvDetails_RowDataBound" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                                OnRowCommand="gvDetails_RowCommand" OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing"
                                OnRowUpdating="gvDetails_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="Serial">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerial" runat="server" Style="text-align: center" Text='<%# Eval("COMMSL") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblSerialEdit" runat="server" Style="text-align: center" Text='<%# Eval("COMMSL") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <%--<asp:TextBox ID="txtID" runat="server" Style="text-align: center" Text='<%# Eval("EXPCID") %>' ReadOnly="true" > </asp:TextBox>--%>
                                        </FooterTemplate>
                                        <FooterStyle />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExctype" Text='<%# Eval("EXCTP") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlExctypeEdit" CssClass="form-control input-sm"
                                                TabIndex="20" AutoPostBack="True" OnSelectedIndexChanged="ddlExctypeEdit_SelectedIndexChanged">
                                                <asp:ListItem Value="BDT">BDT</asp:ListItem>
                                                <asp:ListItem Value="USD">USD</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlExctype" CssClass="form-control input-sm"
                                                TabIndex="10" AutoPostBack="True" OnSelectedIndexChanged="ddlExctype_SelectedIndexChanged">
                                                <asp:ListItem Value="BDT">BDT</asp:ListItem>
                                                <asp:ListItem Value="USD">USD</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                        <ItemStyle HorizontalAlign="Left" Width="7%" Font-Size="14px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FROM">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtfromEdit" runat="server" CssClass="form-control input-sm "
                                                TabIndex="21" Text='<%# Eval("VALUEFR") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtfrom" runat="server" CssClass="form-control input-sm "
                                                TabIndex="11"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblfrom" runat="server" Text='<%# Eval("VALUEFR") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="14px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TO">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtToEdit" runat="server" CssClass="form-control input-sm "
                                                TabIndex="22" Text='<%# Eval("VALUETO") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtTO" runat="server" CssClass="form-control input-sm "
                                                TabIndex="12" Width="100%"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTo" runat="server" Text='<%# Eval("VALUETO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="14px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <EditItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlvalueTpEdit" Width="100%" CssClass="form-control input-sm txtalign" Height="28px"
                                                TabIndex="23" AutoPostBack="True" OnSelectedIndexChanged="ddlvalueTpEdit_SelectedIndexChanged">
                                                <asp:ListItem Value="PCNT">Percent</asp:ListItem>
                                                <asp:ListItem Value="AMT">Amount</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ddlvalueTp" Width="100%" CssClass="form-control input-sm txtalign" Height="28px"
                                                TabIndex="13" AutoPostBack="True" OnSelectedIndexChanged="ddlvalueTp_SelectedIndexChanged">
                                                <asp:ListItem Value="PCNT">Percent</asp:ListItem>
                                                <asp:ListItem Value="AMT">Amount</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblvalueTp" runat="server" Text='<%# Eval("VALUETP") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="14px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAmountEdit" runat="server"
                                                TabIndex="24" Text='<%# Eval("COMMAMT") %>' Width="100%" CssClass="form-control input-sm "></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAmount" runat="server" TabIndex="14"
                                                Width="100%" CssClass="form-control input-sm "></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("COMMAMT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="14px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Party-Commission.aspx", "UPDATER"))
                                                { %>
                                            <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" Height="20px"
                                                ImageUrl="~/Images/update.png" TabIndex="34" ToolTip="Update" Width="20px" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" Height="20px"
                                                ImageUrl="~/Images/Cancel.png" TabIndex="35" ToolTip="Cancel" Width="20px" />
                                            <% } %>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Party-Commission.aspx", "INSERTR"))
                                                { %>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon"
                                                Height="25px" ImageUrl="~/Images/AddNewitem.png" TabIndex="15" ToolTip="Save &amp; Continue"
                                                ValidationGroup="validaiton" Width="25px" />
                                            <% } %>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Party-Commission.aspx", "UPDATER"))
                                                { %>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" Height="20px"
                                                ImageUrl="~/Images/Edit.png" TabIndex="100" ToolTip="Edit" Width="20px" />
                                            <% } %>
                                            <% if (dbFunctions.checkParmit("/CNF/UI/Party-Commission.aspx", "DELETER"))
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
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
