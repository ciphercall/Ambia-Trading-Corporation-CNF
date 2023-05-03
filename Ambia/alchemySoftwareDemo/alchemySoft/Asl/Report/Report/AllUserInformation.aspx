<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="AllUserInformation.aspx.cs" Inherits="alchemySoft.Asl.Report.Report.AllUserInformation" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="alchemySoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=lblMsg.ClientID%>").fadeOut(20000);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>User Information</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div>
            <!-- content header end -->

            <div class="content_wrapper">
                <br />
                <div class="col-md-12">
                    <div class="row form-class">
                        <div class="col-md-2">
                            <strong>
                                <asp:Label runat="server" ID="lblComLabel" Text="Company Name"></asp:Label></strong>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="txtCompanyName" Width="100%" AutoPostBack="True" CssClass="form-control input-sm" OnSelectedIndexChanged="txtCompanyName_TextChanged" />
                        </div>
                        <div class="col-md-4"></div>
                    </div>
                </div>
            <div class="">
                <div class="row form-class">
                    <div class="col-md-12 text-center">
                        <asp:Label runat="server" ID="lblMsg" Visible="False" ForeColor="red"></asp:Label>
                    </div>
                </div>
            </div>


            <% if (Session["CompanyIdForReport"] != null)
                { %>
            <div class="form-class">
                <div class="col-md-12">
                    <div class="panel panel-primary table-responsive">
                        <!-- Default panel contents -->
                        <asp:GridView ID="gv_User" runat="server" AutoGenerateColumns="False" CssClass="Grid">
                            <Columns>
                                <asp:BoundField HeaderText="User Name" DataField="USERNM">
                                    <HeaderStyle Width="12%" />
                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Department" DataField="DEPTNM">
                                    <HeaderStyle Width="12%" />
                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Operation Type" DataField="OPTP">
                                    <HeaderStyle Width="8%" />
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Contact No" DataField="MOBNO">
                                    <HeaderStyle Width="8%" />
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Email" DataField="EMAILID">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Login Id" DataField="LOGINBY">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle Width="7%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Status" DataField="STATUS">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle Width="5%" HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            </div>

            <% } %>
        </div>
    </div>

</asp:Content>
