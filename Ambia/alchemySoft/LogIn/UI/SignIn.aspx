<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="alchemySoft.LogIn.UI.SignIn" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="Sergey Pozhilov (GetTemplate.com)" />

    <title>Sign in</title>

    <script>
        $(document).ready(function () {
            $('#<%=txtlink.ClientID%>').val($.session.get('URLLINK'));
            $.getJSON("http://jsonip.appspot.com?callback=?",
               function (data) {
                   $("#<%=txtIp.ClientID %>").val(data.ip);
               });
        });
    </script>

   <link rel="shortcut icon" href="icon.icns" />

    <link rel="stylesheet" media="screen" href="http://fonts.googleapis.com/css?family=Open+Sans:300,400,700" />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../assets/css/font-awesome.min.css" />

    <!-- Custom styles for our template -->
    <link rel="stylesheet" href="../assets/css/bootstrap-theme.css" media="screen" />
    <link rel="stylesheet" href="../assets/css/main.css" />

    <style type="text/css">
        #Brand {
            text-decoration: none;
        }

            #Brand:hover {
                color: white;
                cursor: pointer;
            }

            #Brand p {
                margin-top: -9px;
                font-size: 30px;
                color: white;
            }

        .brnd {
            color: white;
        }

            .brnd:hover {
                color: white;
                cursor: pointer;
            }
            .footer {
    position: fixed;
    left: 0;
    bottom: 0;
    width: 100%;
    background-color: #713a3a;
    color: white;
    text-align: left;
}
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="script1"></asp:ScriptManager>
        <!-- Fixed navbar -->

        <div class="navbar navbar-inverse navbar-fixed-top headroom" style="background: #713a3a">
            <div class="container">
                <div class="row">
                    <div class="col-md-8">
                        <%--<img src="/Images/logo.png" style="width: 10%; height: 50px; float: left;" alt="" />--%>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <a id="Brand">
                            <p style="font-family: Neuropolitical">&nbsp;&nbsp;AMBIA KNITTING & DYEING LTD.</p>
                        </a>
                    </div>

                    <div class="col-md-4 pull-right">
                        <%--<p id="brnd">--%>
                        <a class="btn brnd pull-right" href="LogIn.aspx">SIGN IN / SIGN UP</a>
                        <%--</p>--%>
                    </div>
                </div>
                <!--/.nav-collapse -->
            </div>
        </div>

        <!-- /.navbar -->

        <header id="head" class="secondary"></header>

        <!-- container -->
        <div class="container">

            <div class="row">

                <!-- Article main content -->
                <article class="col-xs-12 maincontent">
                    <%--<header class="page-header">
					<h1 class="page-title">Sign in</h1>
				</header>--%>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>


                            <div class="col-md-4 col-md-offset-4 col-sm-8 col-sm-offset-2" style="padding-top: 10px">
                                <br />
                                <div class="panel panel-default">
                                  
                                    <div class="panel-body">
                                      <h2>WELCOME TO AMBIA</h2>  
                                        <h3 class="thin text-center">Sign in</h3> 
                                        <hr />
                                        <asp:TextBox runat="server" Style="display: none" ID="txtIp" ClientIDMode="Static"></asp:TextBox>
                                        <asp:TextBox runat="server" Style="display: none" ID="txtLotiLongTude"></asp:TextBox>
                                        <asp:TextBox ID="txtlink" ClientIDMode="Static" Style="display: none" class="form-control" runat="server"></asp:TextBox>

                                        <div class="top-margin">
                                            <label>Username/Email <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtUser" type="text" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="top-margin">
                                            <label>Password <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtPassword" type="password" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="text-center; top-margin">
                                            <asp:Label runat="server" ID="lblMsg" Visible="False" ForeColor="red"></asp:Label>
                                        </div>

                                        <hr />
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <b><a href="forgetPassword.aspx">Forgot password?</a></b>
                                            </div>
                                            <div class="col-lg-6 text-right">
                                                <asp:Button ID="btnSubmit" runat="server" CssClass="form-control" BackColor="#713a3a" ForeColor="White" type="submit" Text="Sign in" OnClick="btnLogin_Click" BorderColor="#6699FF"></asp:Button>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </article>
                <!-- /Article -->

            </div>
        </div>
        <!-- /container -->


        <footer id="footer" class="top-space">

            <div class="footer">
                <div class="container">
                    <div class="row">

                        <div class="col-md-4 widget" style="font-size:10pt">
                            <h3 class="widget-title">Contact</h3>
                            <div class="widget-body">
                                <p>
                                    Mobile : 01678060322<br />Email:akdl@ambiagroup.com,akdl_office@ambiagroup.com  
                                    <br />
                                    Halimkharchar, Charkanai Patiya, Chittagong, Bangladesh
                                </p>
                            </div>
                        </div>

                        <div class="col-md-5 widget">
                        </div>

                        <div class="col-md-3 widget">
                            <h3 class="widget-title">Follow me</h3>
                            <div class="widget-body">
                                <p class="follow-me-icons clearfix">
                                    <a href="#"><i class="fa fa-google-plus"></i></a>
                                    <a href="#" target="_blank"><i class="fa fa-facebook"></i></a>
                                </p>

                                <p>
                                    Copyright &copy; <%=DateTime.Now.Year %><br />
                                    Developed by <a href="http://alchemy-bd.com/" rel="designer">Alchemy Software Ltd.</a>
                                </p>
                            </div>
                        </div>

                    </div>
                    <!-- /row of widgets -->
                </div>
            </div>

            <%--<div class="footer2">
                <div class="container">
                    <div class="row">

                        <div class="col-md-6 widget">
                            <div class="widget-body">
                                <p class="simplenav">
                                    <a href="#">Home</a> | 
								<a href="#">About</a> |
								<a href="#">Sidebar</a> |
								<a href="#">Contact</a> |
								<b><a href="#">Sign up</a></b>
                                </p>
                            </div>
                        </div>

                        <div class="col-md-6 widget">
                            <div class="widget-body">
                                <p class="text-right">
                                    Copyright &copy; 2014, Mejab Enterprise. Developed by <a href="http://alchemy-bd.com/" rel="designer">Alchemy Software</a>
                                </p>
                            </div>
                        </div>

                    </div>
                    <!-- /row of widgets -->
                </div>
            </div>--%>
        </footer>





        <!-- JavaScript libs are placed at the end of the document so the pages load faster -->
        <script src="../assets/js/jquery-2.1.3.js"></script>
        <script src="../assets/js/bootstrap.min.js"></script>
        <script src="../assets/js/headroom.min.js"></script>
        <script src="../assets/js/jQuery.headroom.min.js"></script>
        <script src="../assets/js/template.js"></script>

        <script>
            $(document).ready(function () {
                navigator.geolocation.getCurrentPosition(showPosition);
                function showPosition(position) {
                    var coordinates = position.coords;
                    var long = coordinates.longitude;
                    var loti = coordinates.latitude;
                    $("#<%=txtLotiLongTude.ClientID %>").val(loti + ", " + long);

                }
            });
        </script>
    </form>
</body>
</html>