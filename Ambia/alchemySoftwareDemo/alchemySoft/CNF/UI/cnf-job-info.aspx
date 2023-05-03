<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="cnf-job-info.aspx.cs" Inherits="alchemySoft.CNF.UI.cnf_job_info" %>

<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Job Information</title>

    <%--<link href="../../MenuCssJs/ui-lightness/jquery.ui.theme.css" rel="stylesheet" />
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />

    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>--%>
    <script>
        $(document).ready(function () {
            BindControlEvents();

        });

        function BindControlEvents() {
            GetCompletionListCompanyNameWithBrancId();
            GetCompletionListPartyName();
        }

        function pageLoad() {
            GetCompletionListCompanyNameWithBrancId();
            GetCompletionListPartyName();
            GetCompletionListConsigName();
            GetCompletionListSupplier();
            //$(".select").select2({ placeholder: '' });

            $("#<%=txtCrDt.ClientID%>,#<%=txtFORWRDDT.ClientID%>,#<%=txtCRFDT.ClientID%>,#<%=txtInvoiceDT.ClientID%>,#<%=txtBEDT.ClientID%>").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });
            $("#<%=txtBLDT.ClientID%>,#<%=txtLCDT.ClientID%>,#<%=txtPermitDT.ClientID%>,#<%=txtAwbDT.ClientID%>,#<%=txtHblDT.ClientID%>").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });
            $("#<%=txtHawbDT.ClientID%>").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-10:+10" });
        }

        function GetCompletionListCompanyNameWithBrancId() {
            $("#<%=txtCompNM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListCompanyNameWithBrancId",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("#<%=txtCompNM.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.label,
                                    value: item.label,
                                    id: item.value
                                };
                            }));
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    $("#<%=txtCompID.ClientID %>").val(ui.item.id);
                    return true;
                }
            });
            }

            function GetCompletionListPartyName() {
                $("#<%=txtPartyNM.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "../../search.asmx/GetCompletionListPartyName",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: "{ 'term' : '" + $("#<%=txtPartyNM.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Request.Cookies["USERTYPE"]%>','brCD' : '<%=HttpContext.Current.Request.Cookies["COMPANYID"]%>'}",
                            dataFilter: function (data) { return data; },
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.label,
                                        value: item.label,
                                        id: item.value
                                    };
                                }));
                            },
                            error: function (result) {
                                alert("Error");
                            }
                        });
                    },
                    minLength: 1,
                    select: function (event, ui) {
                        $("#<%=txtPartyID.ClientID %>").val(ui.item.id);
                        $("#<%=txtSuppNM.ClientID %>").focus();
                        return true;
                    }
                });
            }



            function GetCompletionListConsigName() {
                $("#<%=txtConsigneeNM.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "../../search.asmx/GetCompletionListConsigName",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: "{ 'txt' : '" + $("#<%=txtConsigneeNM.ClientID %>").val() + "'}",
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

            function GetCompletionListSupplier() {
                $("#<%=txtSuppNM.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "../../search.asmx/GetCompletionListSupplier",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: "{ 'txt' : '" + $("#<%=txtSuppNM.ClientID %>").val() + "'}",
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

        $(function () {
            $('#txtExpenseNM').on('keyup', function () {
                if ($(this).val() == '') {
                    alert('Name can\'t be blank');
                }
            });
        });

    </script>

    <%--<style>
        .ui-autocomplete {
            max-width: 350px;
            max-height: 250px;
            overflow: auto;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1><strong>Job Information</strong></h1>

                        <!-- logout option button -->
                        <%-- ReSharper disable once Html.IdDuplication --%>
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <% if (dbFunctions.checkParmit("/CNF/UI/cnf-job-info.aspx", "UPDATER"))
                                    { %>
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-edit" ID="btnEditss" runat="server" Text="Edit"></asp:LinkButton>
                                </a>
                                </li>
                                <% } %>
                                <li><a href="#">
                                    <asp:LinkButton CssClass="fa fa-refresh" ID="btnRefreshss" runat="server" Text="Refresh"></asp:LinkButton></a>
                                </li>
                            </ul>
                        </div>
                        <!-- end logout option -->
                    </div>



                    <div class="content_wrapper">
                        <div class="col-md-12">

                            <div class="panel panel-default" style="border-color: #5cb85c; padding-left: 4px; padding-right: 4px; padding-bottom: 2px">
                                <div class="panel-heading">
                                    <asp:Label ID="lblJobNo" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="lblJobTP" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="lblJobReg" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="lblStatus" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="lblRefference" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="lblError" runat="server" ForeColor="#CC0000" Visible="False"
                                        Style="font-weight: 700"></asp:Label>
                                    <asp:Label ID="lblJobQuality" runat="server" Visible="False"></asp:Label>
                                    <asp:TextBox ID="txtCompID" runat="server" onfocus="blur()" Style="display: none"></asp:TextBox>


                                    <div class="row form-class3px">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-1">
                                            <asp:Label ID="lblCompADD" runat="server" Visible="False"></asp:Label>
                                            <asp:Button ID="btnEdit" Text="Edit" Width="100%" runat="server" CssClass="form-control btn-success" OnClick="btnEdit_Click"></asp:Button>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnRefresh" Text="Refresh" Width="120%" runat="server" CssClass="form-control btn-primary" OnClick="btnRefresh_Click"></asp:Button>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnPrint" Text="Print" runat="server" Width="100%" CssClass="form-control btn-default" OnClick="btnPrint_Click"></asp:Button>
                                        </div>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Company</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCompNM" runat="server" CssClass="form-control input-sm"
                                            TabIndex="1" Width="100%"></asp:TextBox>
                                    </div>
                                    <%--OnTextChanged="txtCompNM_TextChanged"--%>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Creation Date &amp; No</div>
                                    <div class="col-md-1">
                                        <asp:DropDownList ID="ddlJobTp" runat="server" Width="146%" CssClass="form-control input-sm" TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="ddlJobTp_SelectedIndexChanged">
                                            <asp:ListItem>EXPORT</asp:ListItem>
                                            <asp:ListItem>IMPORT</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtCrDt" runat="server" Width="150%" CssClass="form-control input-sm" TabIndex="3" AutoPostBack="True" OnTextChanged="txtCrDt_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1"></div>
                                    <div class="col-md-1">Job Year </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtJobYear" Width="146%" onfocus="blur()" runat="server" CssClass="form-control input-sm" TabIndex="4" AutoPostBack="True" OnTextChanged="txtJobYear_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtNo" Width="146%" runat="server" ReadOnly="True" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:DropDownList ID="ddlJobNo" Width="126%" runat="server" TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="ddlJobNo_SelectedIndexChanged" Visible="False" CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        Ref.:  
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlRefferenceType" Width="100%" runat="server" CssClass="form-control input-sm"
                                            TabIndex="4">
                                            <asp:ListItem Value="TTI">TTI</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <asp:GridView ID="GridView1" runat="server">
                                </asp:GridView>
                                <div class="row form-class3px">
                                    <div class="col-md-2">Register ID</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlRegID" runat="server" Width="100%" CssClass="form-control input-sm" TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="ddlRegID_SelectedIndexChanged">
                                            <asp:ListItem>CHITTAGONG</asp:ListItem>
                                            <asp:ListItem>COMILLA</asp:ListItem>
                                            <asp:ListItem>BENAPOLE</asp:ListItem>
                                            <asp:ListItem>DEPZ</asp:ListItem>
                                            <asp:ListItem>ICD</asp:ListItem>
                                            <asp:ListItem>AEPZ</asp:ListItem>
                                            <asp:ListItem>AIRPORT</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">Job Quality</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlJobQuality" runat="server" Width="100%" CssClass="form-control input-sm" TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="ddlJobQuality_SelectedIndexChanged">
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

                                <div class="row form-class3px">
                                    <div class="col-md-2">Party Name</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtPartyNM" runat="server" CssClass="form-control input-sm" TabIndex="6" Width="100%" AutoPostBack="True" OnTextChanged="txtPartyNM_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtPartyID" runat="server" ReadOnly="True" Width="19%" Visible="False"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px" style="font-size: 12px">
                                    <div class="col-md-2">Consignee Name &amp; Address</div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtConsigneeNM" runat="server" CssClass="form-control input-sm" TabIndex="6" Width="100%" AutoPostBack="True" OnTextChanged="ddlConsigneeNM_OnTextChanged"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddlConsigneeNM" runat="server" CssClass="form-control Select" TabIndex="6" Width="100%" AutoPostBack="True" OnTextChanged="ddlConsigneeNM_OnTextChanged"></asp:DropDownList>--%>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtConsigneeAdd" runat="server" CssClass="form-control input-sm" TabIndex="8" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Supplier Name</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtSuppNM" runat="server" CssClass="form-control input-sm" TabIndex="6" Width="100%" AutoPostBack="True" OnTextChanged="ddlSuppNM_OnTextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Goods Desc</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtGoodsDesc" runat="server" CssClass="form-control input-sm" TabIndex="6" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">Packages Details & Type</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtPkgDet" runat="server" CssClass="form-control input-sm" TabIndex="8" Width="100%">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtPackageType" runat="server" CssClass="form-control input-sm" TabIndex="8" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Net Weight</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtNetWeight" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-1">CBM</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCBM" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Gross Weight</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtGrossWeight" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>

                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">C&amp;F Value (USD)</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCNFVUSD" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtCNFVUSD_TextChanged">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Exchange Rate &amp; Type</div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtChangeRT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtChangeRT_TextChanged">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtExTP" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">C&amp;F Value (BDT)</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCNFVBDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                        <asp:TextBox ID="txtCRFVUSD" runat="server" Visible="False" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Vessel</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtVessel" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Rot No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtRotNO" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Line No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtLineNo" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Cleared On</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtClearedOn" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Forward Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFORWRDDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtFORWRDDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Assessable Value</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtAssessableVal" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtAssessableVal_TextChanged">.00</asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Commission</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCommission" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%">.00</asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">CRF No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCRFNO" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtCRFDT_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">CRF Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCRFDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Invoice No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtInvoiceNo_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Invoice Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtInvoiceDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtInvoiceDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">B/E No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtBENO" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">B/E Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtBEDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtBEDT_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtDelDT" runat="server" Visible="False" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtDelDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">B/L No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtBLNO" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">B/L Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtBLDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtBLDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">L/C No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtLCNO" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">L/C Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtLCDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtLCDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Shipping Agent Name</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtPermitNO" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtPermitNO_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtPermitDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtPermitDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Bond No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtAwbNo" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtAwbDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtAwbDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">File No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtHBlNo" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtHblDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtHblDT_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Ass. Group Name</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtHawbNo" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtHawbDT_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Date</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtHawbDT" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Container No</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtContainerNo" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                        <asp:TextBox ID="txtUnderTakeNo" runat="server" Visible="False" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnTextChanged="txtUnderTakeDt_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Remarks</div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtComRemarks" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2">Vat</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtVatNo" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">Date</div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm" TabIndex="12" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                            <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                            <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row form-class3px">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSave" runat="server" CssClass="form-control btn-success" Text="Save" Width="100%" OnClick="btnSave_Click"></asp:Button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<script src="../../MenuCssJs/js/select2.full.min.js"></script>
    <link href="../../MenuCssJs/css/select2.min.css" rel="stylesheet" />
    <script>
        $('.Select').select2();
    </script>--%>
</asp:Content>
