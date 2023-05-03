<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgetPassword.aspx.cs" Inherits="alchemySoft.LogIn.UI.forgetPassword" %>

<!DOCTYPE html>
<html>
<head>
    <title>Reset Password</title>
    <link href="../css/style.css" rel="stylesheet" />
    <script src="../js/jquery-2.1.3.js"></script>
    <!-- Custom Theme files -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Reset Password Form Responsive, Login form web template, Sign up Web Templates, Flat Web Templates, Login signup Responsive web template, Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyEricsson, Motorola web design" />
    <!--google fonts-->
    <link href='//fonts.googleapis.com/css?family=Roboto:400,100,300,500,700,900' rel='stylesheet' type='text/css'>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#cPass').keyup(function () { 
                var pass = $('#Pass').val();
                var cPass = $('#cPass').val();
                if(pass!=cPass)
                {
                    $('#passMsg').text('Password missmatch ✖');
                    $('#passMsg').css('color', 'red');
                }else
                {
                    $('#passMsg').text('Password missmatch ✔');
                    $('#passMsg').css('color', 'green');
                }
            })
        })
    </script>
</head>
<body>
    <!--element start here-->
    <div class="elelment">
        <h2>Reset Password </h2>
        <div class="element-main" style="padding-bottom: 0px;">
            <form id="f" runat="server">

                <div class="alert" runat="server" id="msg" visible="false">
                    <asp:Label ID="msgtokenEntry" Visible="false" runat="server"> Please enter your email address.which you have opened an account in this software !</asp:Label>
                </div>
                <asp:MultiView ID="mV" runat="server">

                    <asp:View ID="v1" runat="server">
                        <asp:Label ID="diNekot" runat="server" Visible="false"></asp:Label>
                        <br />
                        <h1>Forgot Password</h1>
                        <hr />
                        <p style="font-size: 10pt">Please enter your email address.which you have opened an account in this software</p>
                        <asp:TextBox type="text" runat="server" ID="email" value="Your e-mail address" Placeholder="Your e-mail address"></asp:TextBox>
                        <asp:Button type="submit" runat="server" ID="reset" Text="Reset sPassword" OnClick="reset_Click" />
                        <br />
                    </asp:View>
                    <asp:View ID="v2" runat="server">
                        <br />
                        <h1>Enter Token Number</h1>
                        <hr />
                        <p>Please check your email for a message with your code. Your code is 10 digits long.</p>
                        <asp:TextBox ID="tokenid" runat="server"  Placeholder="Enter Token Number">
                        </asp:TextBox>
                        <br />
                        <span style="font-size: 10pt">We sent your code to: &nbsp;&nbsp;
                            <asp:Label ID="sentEmail" runat="server" ForeColor="#cccccc"></asp:Label>
                        </span>
                        <div class="tokenPortion">
                            <div style="display: inline-block;">
                                  <asp:Button ID="Button1" runat="server" Width="100%" CssClass="btn" Text=" Didn't get a code? " OnClick="dontGet_Click" />
                            </div>
                            <div style="display: inline-block; float: right">
                                <asp:Button ID="submit" runat="server" Width="100%" CssClass="btn" Text="Continue" OnClick="submit_Click" />
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="v3" runat="server">
                        <br />
                        <h1>Enter New Password</h1>
                        <hr />
                        <asp:TextBox type="text" ID="Pass" runat="server" placeholder="Enter New Passwor"></asp:TextBox>
                        <asp:TextBox type="text" ID="cPass" runat="server" placeholder="Enter Confirm New Password"></asp:TextBox>
                        <span id="passMsg" style="font-size:9pt"></span>
                        <asp:Button ID="btnChangePass" runat="server" Width="100%" Text="Submit" OnClick="btnChangePass_Click" /><br />
                    </asp:View>
                </asp:MultiView>
            </form>
        </div>
    </div>
    <div class="copy-right">
        <p>Developed By <a href="http://alchemy-bd.com/">Alchemy Software</a></p>
    </div>

    <!--element end here-->
</body>
</html>
