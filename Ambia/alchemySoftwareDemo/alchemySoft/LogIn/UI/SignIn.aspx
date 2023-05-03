<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="alchemySoft.LogIn.UI.SignIn" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Sign In</title>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="icon" type="image/png" href="images/icons/favicon.ico" />
    <link rel="stylesheet" type="text/css" href="/login/vendor/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="/login/fonts/font-awesome-4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="/login/fonts/iconic/css/material-design-iconic-font.min.css">
    <link rel="stylesheet" type="text/css" href="/login/vendor/animate/animate.css">
    <link rel="stylesheet" type="text/css" href="/login/vendor/css-hamburgers/hamburgers.min.css">
    <link rel="stylesheet" type="text/css" href="/login/vendor/animsition/css/animsition.min.css">
    <link rel="stylesheet" type="text/css" href="/login/vendor/select2/select2.min.css">
    <link rel="stylesheet" type="text/css" href="/login/vendor/daterangepicker/daterangepicker.css">
    <link rel="stylesheet" type="text/css" href="/login/css/util.css">
    <link rel="stylesheet" type="text/css" href="/login/css/main.css">
</head>
<body>
    <div class="limiter">
        <div class="container-login100" style="background-image: url('/login/Bacground.jpg');">
            <div class="wrap-login100">
                <span class="login100-form-title p-b-34 p-t-27" style="margin-bottom: 20px">Whelcome To Ambia
                </span>
                <form class="login100-form validate-form" id="frm" runat="server">
                    <span class="login100-form-logo">
                        <img src="../../Images/checkmark.png" style="border-radius: 69px;" /> 
                    </span>
                    <div class="wrap-input100 validate-input" data-validate="Enter username"> 
                        <asp:TextBox runat="server" ID="txtUser" CssClass="input100" Height="30px"  placeholder="Username"></asp:TextBox>
                        <span class="focus-input100" data-placeholder="&#xf207;"></span>
                    </div>
                    <div class="wrap-input100 validate-input" data-validate="Enter password"> 
                        <asp:TextBox runat="server" ID="txtPassword" Height="30px" TextMode="Password" CssClass="input100" placeholder="Password"></asp:TextBox>
                        <span class="focus-input100" data-placeholder="&#xf191;"></span>
                    </div> 
                    <asp:TextBox runat="server" Visible="false" ID="txtIp" ClientIDMode="Static"></asp:TextBox>
                    <asp:TextBox runat="server" Visible="false" ID="txtLotiLongTude"></asp:TextBox>
                    <asp:TextBox ID="txtlink" ClientIDMode="Static" Style="display: none" class="form-control" runat="server"></asp:TextBox>
                    <div class="contact100-form-checkbox">
                        <a class="txt1" href="forgetPassword.aspx">Forgot Password?<br />
                            <asp:Label ID="lblMsg" runat="server" Visible="true" ForeColor="Red" Width="100%"></asp:Label>
                        </a> 
                        <asp:Button ID="btnLogin" runat="server" CssClass="login100-form-btn" style="display: inline-block; float: right" Text="Login" OnClick="btnLogin_Click"/>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <div id="dropDownSelect1"></div>
    <script src="/login/vendor/jquery/jquery-3.2.1.min.js"></script>
    <script src="/login/vendor/animsition/js/animsition.min.js"></script>
    <script src="/login/vendor/bootstrap/js/popper.js"></script>
    <script src="/login/vendor/bootstrap/js/bootstrap.min.js"></script>
    <script src="/login/vendor/select2/select2.min.js"></script>
    <script src="/login/vendor/daterangepicker/moment.min.js"></script>
    <script src="/login/vendor/daterangepicker/daterangepicker.js"></script>
    <script src="/login/vendor/countdowntime/countdowntime.js"></script>
    <script src="/login/js/main.js"></script>
</body>
</html>
