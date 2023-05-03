<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Job-Bill-Information.aspx.cs" Inherits="alchemySoft.CNF.UI.Job_Bill_Information" %>

<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>:: Job Receive ::</title>



    <%-- ReSharper disable once UseOfImplicitGlobalInFunctionScope --%>
    <script>
        $(document).ready(function () {
            BindControlEvents();
        });

        function BindControlEvents() {
            $("[id*=txtFwdDate],[id*=txtFwdDateEdit]").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });
            GetCompletionListJobYear();
            GetCompletionListExpensePerticularsWithExpId();
           <%-- $('.ui-autocomplete').click(function () {
                __doPostBack();
            });
            $("#<%=txtJobID.ClientID %>,[id*=txtExpense]").keydown(function (e) {
                if (e.which === 9 || e.which === 13)
                    window.__doPostBack();
            });--%>
        }
        function confMSG() {
            if (confirm("Are you Sure to Delete?")) { }
            else { return false; }
        }
        function GetCompletionListJobYear() {
            $("#<%=txtJobID.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListJobYear",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtJobID.ClientID %>").val() + "'}",
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
        function GetCompletionListExpensePerticularsWithExpId() {
            $("[id*=txtExpense]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListExpensePerticularsWithExpId",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("[id*=txtExpense]").val() + "'}",
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
                        <h1>Bill Information</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <%-- ReSharper disable once Html.IdDuplication --%>


                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-print" ID="btnPrint" runat="server" Text="Print"
                                        OnClick="btnPrint_Click"></asp:LinkButton></a>
                                </li>
                                <%--<li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-print" ID="btnPrintNSE" runat="server" Text="Print-NSE"
                                        OnClick="btnBillNSE_OnClick"></asp:LinkButton></a>
                                </li>
                                 <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-print" ID="btnPrintRC" runat="server" Text="Print-RC"
                                        OnClick="btnBillRC_OnClick"></asp:LinkButton></a>
                                </li>--%>
                            </ul>
                        </div>
                        <!-- end logout option -->


                    </div>
                    <!-- content header end -->
                    <div class="form-class text-center">
                        <asp:Label ID="lblErrMsgExist" runat="server" ForeColor="#990000" Visible="False"></asp:Label>
                        <asp:Label ID="lblErrmsg" runat="server" ForeColor="#990000" Visible="False"></asp:Label>
                        <asp:Label ID="lblChkInternalID" runat="server" ForeColor="#990000" Visible="False"></asp:Label>
                    </div>


                    <div class="content_wrapper">
                        <div class="col-md-12">
                            <%-- <div class="row form-class3px">
                            <div class="col-md-6"></div>
                            <div class="col-md-2">
                             <asp:Button runat="server" id="btnBillBOT" Text="Print By BTO" CssClass="form-control btn-warning input-sm" OnClick="btnPrint_Click"/>
                            </div>
                            <div class="col-md-2">
                              <asp:Button runat="server" id="btnBillNSE" Text="Print By NSE" OnClick="btnBillNSE_OnClick" CssClass="form-control btn-warning input-sm"/>
                            </div>
                            <div class="col-md-2">
                               <asp:Button runat="server" id="btnBillRC" Text="Print By RC" OnClick="btnBillRC_OnClick" CssClass="form-control btn-warning input-sm"/>
                            </div>
                            
                        </div>--%>

                            <div class="row form-class3px">
                                <div class="col-md-2">Job No & Year</div>
                                <div class="col-md-2">
                                    <asp:TextBox runat="server" ID="txtJobID" CssClass="form-control input-sm text-center"
                                        TabIndex="3" OnTextChanged="txtJobID_TextChanged" AutoPostBack="True"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox runat="server" ID="txtJobYear" CssClass="form-control input-sm text-center"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox runat="server" ID="txtJobType" CssClass="form-control input-sm text-center"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox runat="server" ID="txtReceiveDate" CssClass="form-control input-sm text-center" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox runat="server" ID="txtJobCommission" CssClass="form-control input-sm text-center"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-class3px">
                                <div class="col-md-2">Company ID</div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtCompanyNM" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtCompanyID" CssClass="form-control input-sm" ReadOnly="true"
                                        Visible="False"></asp:TextBox>
                                </div>
                                <div class="col-md-2">Party ID</div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtPartyNM" CssClass="form-control input-sm"
                                        ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtPartyID" CssClass="form-control input-sm" ReadOnly="true"
                                        Visible="False"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-class3px">
                                <div class="col-md-2">Ass. Value</div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtAssValue" CssClass="form-control input-sm" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2">USD Value</div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtUsdValue" CssClass="form-control input-sm" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                                <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True" BackColor="White" GridLines="Both"
                                    BorderStyle="None" CellPadding="4" CssClass="Grid" Font-Size="11px"
                                    OnRowDataBound="gvDetails_RowDataBound" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                                    OnRowCommand="gvDetails_RowCommand" OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing"
                                    OnRowUpdating="gvDetails_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Bill Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillDate" runat="server" Style="text-align: center" Text='<%# Eval("BILLD") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblBillDateEdit" runat="server" Style="text-align: center" Text='<%# Eval("BILLD") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtBillDate" Text='<%# Eval("BILLD") %>' CssClass="form-control input-sm"
                                                    TabIndex="11" ReadOnly="true"></asp:TextBox>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill No">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBillNo" Text='<%# Eval("BILLNO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtBillNoEdit" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("BILLNO") %>' Style="text-align: center"
                                                    TabIndex="121" ReadOnly="true"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("BILLNO") %>' Style="text-align: center"
                                                    TabIndex="120" ReadOnly="true"></asp:TextBox>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemStyle HorizontalAlign="Center" Width="4%" Font-Size="14px" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SL" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSl" Text='<%# Eval("EXPSL") %>'> </asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label runat="server" ID="lblSlEdit" Text='<%# Eval("EXPSL") %>'> </asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblSlItem" CssClass="form-control input-sm" Text='<%# Eval("EXPSL") %>'></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemStyle HorizontalAlign="Center" Width="4%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expense">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtExpenseEdit" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("EXPNM") %>' TabIndex="12" ReadOnly="true"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtExpense" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="13" AutoPostBack="True" Text='<%# Eval("EXPNM") %>'
                                                    OnTextChanged="txtExpense_TextChanged"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpense" runat="server" Style="text-align: center" Text='<%# Eval("EXPNM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="13%" />
                                            <ItemStyle HorizontalAlign="Left" Width="13%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expense ID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtExpenseIDEdit" runat="server" CssClass="form-control input-sm"
                                                    ReadOnly="true" Text='<%# Eval("EXPID") %>' Style="text-align: center"
                                                    TabIndex="55"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtExpenseID" runat="server" CssClass="form-control input-sm"
                                                    ReadOnly="true" Text='<%# Eval("EXPID") %>' Style="text-align: center"
                                                    TabIndex="55"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpenseID" runat="server" Text='<%# Eval("EXPID") %>'
                                                    CssClass="txtalign"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemStyle HorizontalAlign="Right" Width="4%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expense Amount" Visible="false">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtExpenseAmountEdit" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("EXPAMT") %>' Style="text-align: right"
                                                    TabIndex="55" ReadOnly="true"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtExpenseAmount" runat="server" CssClass="form-control input-sm" Enabled="false"
                                                    Text='0.00' Style="text-align: right" TabIndex="14"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpenseAmount" runat="server" Text='<%# Eval("EXPAMT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                            <ItemStyle HorizontalAlign="Right" Width="7%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Amount">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtBillAmountEdit" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("BILLAMT") %>' Style="text-align: right" TabIndex="30"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtBillAmount" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("BILLAMT") %>' Style="text-align: right" TabIndex="15"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillAmount" runat="server" Text='<%# Eval("BILLAMT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                            <ItemStyle HorizontalAlign="Right" Width="7%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRemarksEdit" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("REMARKS") %>' Style="text-align: left" TabIndex="31"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("REMARKS") %>' Style="text-align: left" TabIndex="16"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtFwdDateEdit" CssClass="form-control input-sm"
                                                    Text='<%# Eval("EXPPD") %>' TabIndex="32"
                                                    OnTextChanged="txtFwdDateEdit_TextChanged" AutoPostBack="True"></asp:TextBox>
                                            </EditItemTemplate>

                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtFwdDate" CssClass="form-control input-sm" TabIndex="17"
                                                    Text='<%# Eval("EXPPD") %>' AutoPostBack="True"
                                                    OnTextChanged="txtFwdDate_TextChanged"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblFwdDate" runat="server" Text='<%# Eval("EXPPD") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill SL">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtBillSlEdit" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("BILLSL") %>' Style="text-align: center"
                                                    TabIndex="33" AutoPostBack="true"
                                                    OnTextChanged="txtBillSlEdit_TextChanged"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtBillSl" runat="server" CssClass="form-control input-sm"
                                                    Text='<%# Eval("BILLSL") %>' Style="text-align: center"
                                                    TabIndex="18" AutoPostBack="true"
                                                    OnTextChanged="txtBillSl_TextChanged"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillSl" runat="server" Text='<%# Eval("BILLSL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemStyle HorizontalAlign="Center" Width="4%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <EditItemTemplate>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Job-Bill-Information.aspx", "UPDATER"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" Height="20px"
                                                    ImageUrl="~/Images/update.png" TabIndex="34" ToolTip="Update" Width="20px" />
                                                <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" Height="20px"
                                                    ImageUrl="~/Images/Cancel.png" TabIndex="35" ToolTip="Cancel" Width="20px" />
                                                <% } %>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Job-Bill-Information.aspx", "INSERTR"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon"
                                                    Height="30px" ImageUrl="~/Images/AddNewitem.png" TabIndex="19" ToolTip="Save &amp; Continue"
                                                    ValidationGroup="validaiton" Width="30px" />
                                                <% } %>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Job-Bill-Information.aspx", "UPDATER"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" Height="20px"
                                                    ImageUrl="~/Images/Edit.png" TabIndex="100" ToolTip="Edit" Width="20px" />
                                                <% } %>
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Job-Bill-Information.aspx", "DELETER"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" Height="20px"
                                                    ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()" TabIndex="101"
                                                    ToolTip="Delete" Width="21px" />
                                                <% } %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
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
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
            <%--<asp:PostBackTrigger ControlID="btnPrintNSE" />
            <asp:PostBackTrigger ControlID="btnPrintRC" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
