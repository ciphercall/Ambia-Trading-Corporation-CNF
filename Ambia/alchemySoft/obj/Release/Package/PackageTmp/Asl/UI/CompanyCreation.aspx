<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="CompanyCreation.aspx.cs" Inherits="alchemySoft.Asl.UI.CompanyCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Create Company </h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                        <!-- logout option button -->
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <li><a href="#"><i class="fa fa-plus"></i>Add record</a>
                                </li>
                                <li><a href="#"><i class="fa fa-edit"></i>Edit record</a>
                                </li>
                                <li><a href="#"><i class="fa fa-times"></i>Delete record</a>
                                </li>

                            </ul>
                        </div>
                        <!-- end logout option -->


                    </div>
                    <!-- content header end -->


                    <div class="content_wrapper">
                        <div class="col-md-12">
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">Company Name</div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="txtComName" Width="100%" TabIndex="1" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">Address</div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="txtAddress" Width="100%" TabIndex="2" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">Contact No</div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="txtContactNo" Width="100%" TabIndex="3" CssClass="form-control" MaxLength="11"></asp:TextBox>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">Email Id</div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="txtEmailId" Width="100%" TabIndex="4" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">Website Id</div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="txtWebsiteId" Width="100%" TabIndex="5" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2">Status</div>
                                <div class="col-md-5">
                                    <asp:DropDownList runat="server" ID="ddlStatus" Width="100%" TabIndex="6" CssClass="form-control">
                                        <asp:ListItem Value="A">Active</asp:ListItem>
                                        <asp:ListItem Value="I">Inctive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2"></div>
                                <div class="col-md-5;">
                                    <strong>
                                        <asp:Label runat="server" ID="lblMsg" ForeColor="red" Visible="False"></asp:Label></strong>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row form-class">
                                <div class="col-md-2"></div>
                                <div class="col-md-2"></div>
                                <div class="col-md-5">
                                    <asp:Button runat="server" ID="btnSubmit" Width="100%" TabIndex="7" CssClass="form-control btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                                </div>
                                <div class="col-md-3"></div>
                            </div>

                        </div>
                    </div>
                </div>
                <!-- Content Write From Here-->

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- main content end here -->
    <script>
        $(document).ready(function () {
            $("#<%=lblMsg.ClientID%>").fadeOut(10000);
        })
    </script>
</asp:Content>
