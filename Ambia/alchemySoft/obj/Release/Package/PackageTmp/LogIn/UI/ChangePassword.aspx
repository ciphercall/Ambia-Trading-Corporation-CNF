<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="alchemySoft.LogIn.UI.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    
     <script type="text/javascript">
         $(document).ready(function () {
             $("#<%=lblMsg.ClientID%>, #msg").fadeOut(20000);
         });
        
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
     <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Change Password</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div>
            <!-- content header end -->


            <!-- Content Start From here -->
            <div class="content_wrapper  col-md-12">
                <br />
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">Old Password</div>
                    <div class="col-md-4">
                        <asp:TextBox runat="server" CssClass="form-control input-sm" Width="100%" ID="txtOldPass" placeholder="Old Password" Type="Password"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">New Password</div>
                    <div class="col-md-4">
                        <asp:TextBox runat="server" CssClass="form-control input-sm" Width="100%" ID="txtNewPass" placeholder="New Password" Type="Password"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">Confirm Password</div>
                    <div class="col-md-4">
                        <asp:TextBox runat="server" CssClass="form-control input-sm" Width="100%" ClientIDMode="Static" ID="txtConfirmPass" placeholder="Confirm Password" Type="Password"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-12 text-center"><span><label id="msg"></label></span>
                        <asp:Label runat="server" ID="lblMsg" Visible="False"></asp:Label>
                    </div>
                </div>
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:Button runat="server" ID="btnSubmit" Width="100%" Text="Submit" CssClass="form-control btn-primary input-sm" OnClick="btnSubmit_Click"/>
                    </div>
                    <div class="col-md-5"></div>

                </div>
            </div>
            <!-- Content End From here -->
        </div>
    </div>
    
    <script>
        jQuery("#<%=txtConfirmPass.ClientID%>").on('input paste', function () {
            //alert("fire");
           var newpass = $("#<%=txtNewPass.ClientID%>").val();
            var conpass =$("#<%=txtConfirmPass.ClientID%>").val();

            if (newpass == conpass) {
                $("#msg").text("Password match");
                $("#msg").css("color", 'green');
            } else {
                $("#msg").text("Password missmatch");
                $("#msg").css("color",'red');
            }
        });
    </script>
</asp:Content>
