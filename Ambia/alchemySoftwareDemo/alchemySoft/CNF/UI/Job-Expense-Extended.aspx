<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Job-Expense-Extended.aspx.cs" Inherits="alchemySoft.CNF.UI.Job_Expense_Extended" %>

<%@ Import Namespace="alchemySoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>:: Job Expense ::</title>


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
            $("#<%=txtExDT.ClientID%>").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });

            GetCompletionListPartyNameJobExpense();
            GetCompletionRemarksJobExpns();
            GetCompletionListJobYear();
            GetCompletionListExpensePerticulars();
          <%--  $("[id*=txtJobNo],[id*=txtJobNoEdit],[id*=txtParticulars],[id*=txtParticularsEdit],#<%=txtExpenseNM.ClientID %>").keydown(function (e) {
                if (e.which === 9 || e.which === 13)
                    window.__doPostBack();
            });--%>
        }
        function GetCompletionListPartyNameJobExpense() {
            $("#<%=txtExpenseNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListPartyNameJobExpense",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtExpenseNM.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.label,
                                    value: item.label,
                                    accountcode: item.value
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
                    $("#<%=txtExCD.ClientID %>").val(ui.item.accountcode);
                    $("#<%=txtRemarks.ClientID %>").focus();
                    return true;
                }
            });
        }




        function GetCompletionRemarksJobExpns() {
            $("#<%=txtRemarks.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionRemarksJobExpns",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtRemarks.ClientID %>").val() + "'}",
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
                minLength: 1,
                select: function (event, ui) {
                    $("#<%=txtRemarks.ClientID %>").focus();
                    return true;
                }
            });
            }


            function GetCompletionListJobYear() {
                $("[id*=txtJobNo],[id*=txtJobNoEdit]").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "../../search.asmx/GetCompletionListJobYear",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: "{ 'txt' : '" + $("[id*=txtJobNo],[id*=txtJobNoEdit]").val() + "'}",
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



            function GetCompletionListExpensePerticulars() {
                $("[id*=txtParticulars],[id*=txtParticularsEdit]").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "../../search.asmx/GetCompletionListExpensePerticulars",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: "{ 'txt' : '" + $("[id*=txtParticulars],[id*=txtParticularsEdit]").val() + "'}",
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

            $(function () {
                $('#txtExpenseNM').on('keyup', function () {
                    if ($(this).val() == '') {
                        alert('Name can\'t be blank');
                    }
                });
            });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Job Expense Extended</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <%-- ReSharper disable once Html.IdDuplication --%>
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <% if (dbFunctions.checkParmit("/CNF/UI/Job-Expense-Extended.aspx", "UPDATER"))
                                    { %>
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-edit" ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit"></asp:LinkButton>
                                </a>
                                </li>
                                <% } %>
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-refresh" ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click"></asp:LinkButton></a>
                                </li>
                            </ul>
                        </div>
                        <!-- end logout option -->


                    </div>
                    <!-- content header end -->
                    <div class="form-class text-center">
                        <%--<asp:Label ID="lblError" runat="server" ForeColor="#CC0000" Visible="False"></asp:Label>--%>
                    </div>

                    <%-- <div class="alert alert-warning">
                        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                        <strong>Warning!<asp:Label ID="lblError" runat="server" ForeColor="#CC0000" Visible="False"></asp:Label></strong>
                    </div>--%>

                    <%--<div class="alert alert-warning">
                        <strong>Warning!</strong><asp:Label ID="lblError" runat="server" ForeColor="#CC0000" Visible="False"></asp:Label>
                    </div>--%>

                    <asp:Label ID="lblCmpID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblMY" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblInvoiceNo" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblExsl" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lbltransNomst" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lbltransSL" runat="server" Visible="False"></asp:Label>

                    <div class="content_wrapper">
                        <div class="col-md-12">


                            <div class="row form-class3px">
                                <div class="col-md-2">Expense Data & No</div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtExDT" runat="server" AutoPostBack="True" CssClass="form-control input-sm text-center"
                                        OnTextChanged="txtExDT_TextChanged" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtNo" runat="server" ReadOnly="True" CssClass="form-control input-sm" TabIndex="100"></asp:TextBox>
                                    <asp:DropDownList ID="ddlInvoice" runat="server" AutoPostBack="True" TabIndex="1"
                                        Visible="False" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlInvoice_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">Expensed By</div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtExpenseNM" runat="server" CssClass="form-control input-sm" TabIndex="3"></asp:TextBox>
                                    <asp:TextBox ID="txtExCD" Style="display: none" CssClass="form-control input-sm" runat="server" TabIndex="300"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-class3px">
                                <div class="col-md-2">Remarks</div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control input-sm" TabIndex="4" TextMode="MultiLine" OnTextChanged="txtRemarks_TextChanged"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" TabIndex="50" CssClass="form-control input-sm btn-primary"
                                        OnClick="btnPrint_Click" Visible="True" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" TabIndex="50" CssClass="form-control input-sm btn-primary"
                                        OnClick="btnUpdate_OnClick" Visible="False" />
                                </div>

                            </div>
                            <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px; font-size: 14px">
                                <asp:GridView ID="gvDetails" runat="server" OnRowDataBound="gvDetails_RowDataBound" AutoGenerateColumns="False" ShowFooter="True"
                                    OnRowCancelingEdit="gvDetails_RowCancelingEdit" BackColor="White" GridLines="Both"
                                    BorderStyle="None" CellPadding="4" CssClass="Grid" Width="100%" Font-Size="11px"
                                    OnRowCommand="gvDetails_RowCommand" OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing"
                                    OnRowUpdating="gvDetails_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Serial">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSL" runat="server" Style="text-align: center" Text='<%# Eval("SLNO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSlEdit" runat="server" Style="text-align: center" Text='<%# Eval("SLNO") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtJobNoEdit" runat="server" AutoPostBack="true" Text='<%# Eval("JOBNO") %>'
                                                    CssClass="form-control input-sm" OnTextChanged="txtJobNoEdit_TextChanged"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtJobNo" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="12" AutoPostBack="True" OnTextChanged="txtJobNo_TextChanged"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobNo" runat="server" Text='<%# Eval("JOBNO") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job Year">
                                            <EditItemTemplate>
                                                <asp:Label ID="txtJobYearEdit" runat="server" Text='<%# Eval("JOBYY") %>' Style="text-align: center"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txtJobYear" runat="server" Style="text-align: center"></asp:Label>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobYear" runat="server" Text='<%# Eval("JOBYY") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <EditItemTemplate>
                                                <asp:Label ID="txtJobTPEdit" runat="server" Text='<%# Eval("JOBTP") %>' Style="text-align: center"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txtJobTP" runat="server" Style="text-align: center"></asp:Label>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobTP" runat="server" Text='<%# Eval("JOBTP") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch">
                                            <EditItemTemplate>
                                                <asp:Label ID="txtBranchEdit" runat="server" Text='<%# Eval("BRANCHID") %>' Style="text-align: center"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txtBranch" runat="server" Style="text-align: center"></asp:Label>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranch" runat="server" Text='<%# Eval("BRANCHID") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Branch">
                                            <EditItemTemplate>
                                                <asp:Label ID="txtREFTPEdit" runat="server" Text='<%# Eval("REFTP") %>' Style="text-align: center"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblREFTP" runat="server" Style="text-align: center"></asp:Label>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblREFTP" runat="server" Text='<%# Eval("REFTP") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="14px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Particulars">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParticulars" runat="server" Text='<%# Eval("EXPNM") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtParticularsEdit" runat="server" CssClass="form-control input-sm" TabIndex="61"
                                                    Text='<%# Eval("EXPNM") %>' AutoPostBack="True" OnTextChanged="txtParticularsEdit_TextChanged"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtParticulars" runat="server" TabIndex="31" CssClass="form-control input-sm"
                                                    AutoPostBack="True" OnTextChanged="txtParticulars_TextChanged"></asp:TextBox>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code" Visible="false">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCodeEdit" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="620" Text='<%# Eval("EXPID") %>' Style="text-align: center"
                                                    ReadOnly="True"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="320" Style="text-align: center" ReadOnly="True"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("EXPID") %>' Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expense Amount">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtAmountEdit" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="63" Text='<%# Eval("EXPAMT") %>' onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm"
                                                    TabIndex="33" onkeydown="return (event.keyCode!=13);" ClientIDMode="Static">.00</asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("EXPAMT") %>' Style="text-align: right"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRemarksGEdit" runat="server" CssClass="form-control input-sm txtalignleft"
                                                    TabIndex="64" Text='<%# Eval("REMARKS") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtRemarksG" runat="server" CssClass="form-control input-sm txtalignleft"
                                                    TabIndex="34"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("REMARKS") %>' Style="text-align: left;"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="16%" />
                                            <ItemStyle HorizontalAlign="Left" Width="16%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Party">
                                            <EditItemTemplate>
                                                <asp:Label ID="lblRemarksEdit" runat="server" Text='<%# Eval("ACCOUNTNM") %>' Style="text-align: left;"></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblRemarksFoot" runat="server" Style="text-align: left;"></asp:Label>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarksFoot" runat="server" Text='<%# Eval("ACCOUNTNM") %>' Style="text-align: left;"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="16%" />
                                            <ItemStyle HorizontalAlign="Left" Width="16%" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" Height="20px"
                                                    ImageUrl="~/Images/update.png" TabIndex="65" ToolTip="Update" Width="20px" />
                                                <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" Height="20px"
                                                    ImageUrl="~/Images/Cancel.png" TabIndex="66" ToolTip="Cancel" Width="20px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="SaveCon"
                                                    Height="20px" ImageUrl="~/Images/AddNewitem.png" TabIndex="35" ToolTip="Save &amp; Continue"
                                                    ValidationGroup="validaiton" Width="20px" />
                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Complete"
                                                    Height="20px" ImageUrl="~/Images/checkmark.png" TabIndex="36" ToolTip="Complete"
                                                    ValidationGroup="validaiton" Width="20px" />
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" Height="20px"
                                                    ImageUrl="~/Images/Edit.png" TabIndex="100" ToolTip="Edit" Width="20px" />
                                                <% if (dbFunctions.checkParmit("/CNF/UI/Job-Expense-Extended.aspx", "DELETER"))
                                                    { %>
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" Height="20px"
                                                    ImageUrl="~/Images/delete.png" OnClientClick="return confMSG()" TabIndex="101"
                                                    ToolTip="Delete" Width="20px" />
                                                <% } %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                            <ItemStyle HorizontalAlign="Center" Width="7%" />
                                            <FooterStyle HorizontalAlign="Center" Width="7%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle Font-Size="14px" />
                                    <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="15px" BackColor="#D9EDF7" />
                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" Font-Size="14px" />
                                </asp:GridView>
                            </div>
                            <div class="row form-class3px">
                                <table style="width: 100%; font-family: Calibri; font-size: 14px; font-weight: bold">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 42%"></td>
                                        <td style="width: 8%; text-align: right">Total :
                                        </td>
                                        <td style="width: 12%; text-align: right">
                                            <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control input-sm text-right" Font-Bold="True"
                                                Font-Size="14px" ReadOnly="True" Width="100%">.00</asp:TextBox>
                                        </td>
                                        <td style="width: 20%"></td>
                                        <td style="width: 10%"></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
