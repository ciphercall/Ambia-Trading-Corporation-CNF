<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Party-Information.aspx.cs" Inherits="alchemySoft.CNF.UI.Party_Information" %>

<%@ Import Namespace="alchemySoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>:: Party Information ::</title>
    <%-- <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
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
            $("#<%=txtPartynm.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListPartyName",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'term' : '" + $("#<%=txtPartynm.ClientID %>").val() + "','uTp' : '<%=HttpContext.Current.Request.Cookies["UserInfo"]%>','brCD' : '<%=HttpContext.Current.Request.Cookies["COMPANYID"]%>'}",
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
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    $("#<%=txtPartyID.ClientID %>").val(ui.item.x);
                    $.ajax({
                        url: "../../search.asmx/GetPartyAllInformation",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'accountcd' : '" + ui.item.x + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            if (data.d.length > 0) {
                                $("#<%=txtaddress.ClientID %>").val(data.d[0].Address);
                                $("#<%=txtcontact.ClientID %>").val(data.d[0].ContactNo);
                                $("#<%=txtEmail.ClientID %>").val(data.d[0].EmailId);
                                $("#<%=txtwebadd.ClientID %>").val(data.d[0].WebId);
                                $("#<%=txtAPname.ClientID %>").val(data.d[0].Apname);
                                $("#<%=txtapcontact.ClientID %>").val(data.d[0].ApNo);
                                $("#<%=ddlstatus.ClientID %>").val(data.d[0].Status);
                                $("#<%=txtlogmail.ClientID%>").val(data.d[0].LoginID);
                                $("#<%=txtpassword.ClientID%>").val(data.d[0].LoginPw);

                            } else {
                                $("#<%=txtaddress.ClientID %>").val("");
                                $("#<%=txtcontact.ClientID %>").val("");
                                $("#<%=txtEmail.ClientID %>").val("");
                                $("#<%=txtwebadd.ClientID %>").val("");
                                $("#<%=txtAPname.ClientID %>").val("");
                                $("#<%=txtapcontact.ClientID %>").val("");
                                $("#<%=txtlogmail.ClientID %>").val("");
                                $("#<%=txtpassword.ClientID %>").val("");
                            }
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                    $("#<%=txtaddress.ClientID %>").focus();
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
                        <h1>Party Information Entry</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <%-- ReSharper disable once Html.IdDuplication --%>
                        <%--<div class="btn-group pull-right" id="editOption">
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
                        <asp:Label runat="server" ID="lblerrmsg" Visible="False" ForeColor="#990000" Style="font-weight: 700"></asp:Label>
                    </div>



                    <asp:Label runat="server" ID="lblMY" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="lblSL" Visible="false"></asp:Label>

                    <div class="content_wrapper col-md-12">
                        <div class="row form-class3px">
                            <div class="col-md-2">Party Name</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPartynm" runat="server" TabIndex="1" required=""
                                    CssClass="form-control input-sm"></asp:TextBox>
                                <asp:TextBox ID="txtPartyID" runat="server" Style="display: none"
                                    CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Address</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtaddress" runat="server" required="" TabIndex="2" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class3px">
                            <div class="col-md-2">Contact No</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtcontact" runat="server" TabIndex="3" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Email ID</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmail" runat="server" TabIndex="4" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class3px">
                            <div class="col-md-2">Web Address</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtwebadd" runat="server" TabIndex="5" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">A.P. Name</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAPname" runat="server" TabIndex="6" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class3px">
                            <div class="col-md-2">Login Email</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtlogmail" runat="server" TabIndex="7" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Password</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtpassword" runat="server" TabIndex="7" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-class3px">
                            <div class="col-md-2">A.P. Contact No</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtapcontact" runat="server" TabIndex="7" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Status</div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlstatus" TabIndex="8" CssClass="form-control input-sm">
                                    <asp:ListItem Value="A">ACTIVE</asp:ListItem>
                                    <asp:ListItem Value="I">INACTIVE</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>

                        <div class="row form-class3px">
                            <div class="col-md-4"></div>
                            <div class="col-md-2">
                                <% if (dbFunctions.checkParmit("/CNF/UI/Party-Information.aspx", "INSERTR"))
                                    { %>
                                <asp:Button ID="btnSave" runat="server" CssClass="form-control input-sm btn-primary" TabIndex="11"
                                    Text="Save" OnClick="btnSave_Click1" />
                                <% } %>
                            </div>
                            <div class="col-md-2">
                                <% if (dbFunctions.checkParmit("/CNF/UI/Party-Information.aspx", "UPDATER"))
                                    { %>
                                <asp:Button ID="btnEdit" runat="server" CssClass="form-control input-sm btn-warning" TabIndex="11"
                                    Text="Update" OnClick="btnEdit_Click" />
                                <% } %>
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
