<%@ Page Title="Process" Language="C#" AutoEventWireup="true" MasterPageFile="~/alchemy.Master" CodeBehind="Process.aspx.cs" Inherits="alchemySoft.accounts.ui.Process" %>

<%@ Import Namespace="DynamicMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <script src="../../MenuCssJs/js/function.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+100" });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Process</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                <!-- logout option button -->
                <%--<div class="btn-group pull-right" id="editOption">
                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                        <i class="fa fa-cog"></i>
                    </button>
                    <ul class="dropdown-menu pull-right" style="" role="menu">
                        <% if (UserPermissionChecker.checkParmit("/Accounts/UI/Process.aspx", "INSERTR") == true)
                           { %>
                        <li><a href="Process.aspx"><i class="fa fa-plus"></i>Create</a>
                        </li>
                        <% } %>
                        <% if (UserPermissionChecker.checkParmit("/Accounts/UI/Process.aspx", "UPDATER") == true)
                           { %>
                        <li><a href="Process.aspx"><i class="fa fa-edit"></i>Update</a>
                        </li>
                        <% } %>
                        <% if (UserPermissionChecker.checkParmit("/Accounts/UI/Process.aspx", "DELETER") == true)
                           { %>
                        <li><a href="Process.aspx"><i class="fa fa-times"></i>Delete</a>
                        </li>
                        <% } %>
                    </ul>
                </div>--%>
                <!-- end logout option -->
                <asp:Label ID="lblSerial_Mrec" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblSerial_Jour" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblSerial_BUY" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblSerial_Mpay" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblSerial_Cont" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblSerial_SALE" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblSlSale_Dis" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblSerial_LC" runat="server" Visible="False"></asp:Label>


            </div>
            <!-- content header end -->


            <!-- Content Start From here -->
            <div class="content_wrapper  col-md-12"><br /><br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="row form-class3px">
                            <div class="col-md-1"></div>
                            <div class="col-md-2">
                                Date <strong>:</strong>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control input-sm" AutoPostBack="True"
                                    ClientIDMode="Static" OnTextChanged="txtDate_TextChanged" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnProcess" runat="server" CssClass="form-control btn-primary" Font-Bold="True" Font-Italic="True" Text="Process" OnClick="btnProcess_Click" TabIndex="2" />
                            </div>
                            <div class="col-md-3"></div>
                        </div>
                        <div class="row">
                            <div class="col-md-2"></div>
                            <div class="col-md-10 text-left">
                                <asp:Label ID="lblMSG_Incomplete" runat="server" ForeColor="green" Visible="False"></asp:Label>
                                <%--<asp:Image ID="ImageIncomplete" runat="server" Visible="False" Width="50px" Height="50px" ImageAlign="Right" ImageUrl="~/Images/Forward_Arrow.png" ToolTip="Complete the Transaction List" />--%>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                    <asp:GridView ID="GridView1" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="GridView2" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="GridView22" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="GridView3" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="GridView4" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="gridLC" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="gridSale_Ret" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="gridPurchase_Ret" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="gvSaleNew" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="gvSaleLtCost" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="gvBuyImport" runat="server">
                    </asp:GridView>
                    <asp:GridView ID="gridMultiple" runat="server">
                    </asp:GridView>
                </div>

                <!-- Content End From here -->

                <div class="row">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-5">
                        <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                            <asp:GridView ID="IncompleteGrid" runat="server" AutoGenerateColumns="False" Width="100%" Font-Size="10pt" OnRowDataBound="IncompleteGrid_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="Transaction Type">
                                        <HeaderStyle Width="70%" HorizontalAlign="Center" CssClass="text-center" />
                                        <ItemStyle Width="70%" HorizontalAlign="Left" CssClass="text-left" ForeColor="green" BackColor="ThreeDFace" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Invoice No">
                                        <HeaderStyle Width="30%" HorizontalAlign="Center" CssClass="text-center" />
                                        <ItemStyle Width="30%" HorizontalAlign="Center" CssClass="text-center" ForeColor="green" BackColor="ThreeDFace" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle Font-Size="9pt" />
                                <HeaderStyle BackColor="#286090" ForeColor="white" Font-Bold="True" CssClass="text-center" Font-Size="10pt" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-md-4">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
