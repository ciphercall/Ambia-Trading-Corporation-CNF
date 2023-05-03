<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="alchemySoft.LogIn.UI.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
     <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Profile</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div><br /><br/>
            <!-- content header end -->


            <!-- Content Start From here -->
            <div class="content_wrapper  col-md-12">
                
                <div class="row form-class">
                    <div class="col-md-3">
                        <div class="profile">
                            <p>
                            <%--<img src="/../../Images/profile.jpg"/>--%>
                            </p>
                            <h2><asp:Label runat="server" ID="lblUserName"></asp:Label></h2>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div class="row form-class">
                            <div class="col-md-4">
                                Company Name
                                </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblCompanyName"></asp:Label>
                                </div>
                        </div>
                        <%--<div class="row form-class">
                            <div class="col-md-4">
                                Branch
                                </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblBranch"></asp:Label>
                                </div>
                        </div>--%>
                        <div class="row form-class">
                            <div class="col-md-4">
                                Department
                                </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblDepartment"></asp:Label>
                                </div>
                        </div>
                        <div class="row form-class">
                            <div class="col-md-4">
                                Address
                                </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblAddress"></asp:Label>
                                </div>
                        </div>
                        
                        <div class="row form-class">
                            <div class="col-md-4">
                                Mobile No
                                </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblMobile"></asp:Label>
                                </div>
                        </div>
                         <div class="row form-class">
                            <div class="col-md-4">
                                Email
                                </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblEmail"></asp:Label>
                                </div>
                        </div>
                        
                        <div class="row form-class">
                            <div class="col-md-4">
                                Login Time
                                </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="lblTimeFrom"></asp:Label> to <asp:Label runat="server" ID="lbltimeTo"></asp:Label>
                                </div>
                        </div>
                    </div>
                    
                    <div class="col-md-1">
                        </div>
                </div>
                
            </div>
            <!-- Content End From here -->
        </div>
    </div>
    

</asp:Content>
