<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Process.aspx.cs" Inherits="alchemy.accounts.UI.Process" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Process</title>
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <link href="../../MenuCssJs/ui-lightness/jquery.ui.theme.css" rel="stylesheet" />
    <link href="../../MenuCssJs/ui-lightness/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+0" });
            $('#txtDate').change(function () {
                $('#ContentPlaceHolder1_btnProcess').focus();
            })
            $('.imgimg').hide();
            $('#ContentPlaceHolder1_btnProcess').click(function () {
                $('.imgimg').show();
                $('#ContentPlaceHolder1_btnProcess').hide();
            })
            $(window).load(function () {
                $('.imgimg').hide(1000);
                $('#ContentPlaceHolder1_btnProcess').show();
            });


        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-10 pull-right" id="mainContentBox" style="background: #0094ff">
        <div id="contentBox" style="border-radius: 10px">
            <div id="contentHeaderBox">
                <h1>Process</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->
                <!-- logout option button -->
                <div class="btn-group pull-right" id="editOption">
                    <%--<button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                        <i class="fa fa-cog"></i>
                    </button>
                    <ul class="dropdown-menu pull-right" style="" role="menu">
                        <% if (DynamicMenu.dbFunctions.checkParmit("/Accounts/ui/Process.aspx", "INSERTR") == true)
                           { %>
                        <li><a href="#"><i class="fa fa-plus"></i>Create</a>
                        </li>
                        <% } %>

                        <% if (DynamicMenu.dbFunctions.checkParmit("/Accounts/ui/Process.aspx", "UPDATER") == true)
                           { %>
                        <li><a href="#"><i class="fa fa-edit"></i>Edit</a>
                        </li>
                        <% } %>

                        <% if (DynamicMenu.dbFunctions.checkParmit("/Accounts/ui/Process.aspx", "DELETER") == true)
                           { %>
                        <li><a href="#"><i class="fa fa-edit"></i>Delete</a>
                        </li>
                        <% } %>
                    </ul>--%>
                </div>
                <!-- end logout option -->
            </div>
            <!-- content header end -->
            <!-- Content Start From here -->
            <div class="form-class">
                <!-- Main Content -->
                <div class="row form-class3px">
                    <div class="col-md-2">
                        <asp:Label ID="lblSerial_Do" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_Mr" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_DNote" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_CNote" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_Mrec" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_Jour" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_BUY" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_Mpay" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_Cont" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_SALE" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSlSale_Dis" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblSerial_LC" runat="server" Visible="False"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        Date:
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtDate" runat="server" Width="100%" CssClass="form-control"
                            ClientIDMode="Static" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnProcess" runat="server" CssClass="form-control"
                            Font-Bold="True" Font-Italic="True" Text="Process"
                            OnClick="btnProcess_Click" TabIndex="2" />
                    </div>
                </div>
                <div class="row form-class3px">
                    <div class="col-md-4">
                    </div>
                    <div class="col-md-2 ">
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Image ID="img" CssClass="imgimg" runat="server" ImageUrl="../../Images/processing.gif" />
                    </div>
                </div>
                <div class="row form-class3px">
                    <div class="col-md-2">
                        <asp:GridView ID="GridView1" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="GridView2" runat="server">
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
                        <asp:GridView ID="gridMultiple" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="gvMicCollection" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="gvMicCollectionMember" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="gvBuyImport" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="GridView5_frf_Do" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="GridView6_frf_Mr" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="GridView7_frf_DNote" runat="server">
                        </asp:GridView>
                        <asp:GridView ID="GridView7_frf_CNote" runat="server">
                        </asp:GridView>
                    </div>
                    <div class="col-md-2">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
