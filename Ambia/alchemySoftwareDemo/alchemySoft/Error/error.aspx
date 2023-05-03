<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="DynamicMenu.Error.error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Erorr Found</title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/font-awesome.css" rel="stylesheet" />
    <style>
         
       
    </style>
</head>
<body style="background: #000000">
    <form id="form1" runat="server">
        <div class="head">
            <h1 style="font-size: 36px; margin-top: 8px; font-family: 'Dodger 3D'">
                <img src="images/alchemy.png" style="width: 40px; height: 40px; display: inline-block" />
                Alchemy Software</h1>
        </div>
        <div class="main">
            <%--  <h1>
            <img src="images/alchemy.png" style="width: 40px; height: 40px; display: inline-block" />
            Modern Error Widget</h1>--%>

            <div class="w3layouts_main_grids">

                <div class="w3_main_grid_left">
                    <div>
                        <h2 style="font-size: 60pt">Error...</h2>
                    </div>
                    <br />
                    <br />
                    <br />
                    <h3 style="text-align: center">Oops...
                    <br />
                        Somthing Went Wrong!</h3>
                    <div class="w3ls_form">
                        <form action="#" method="post">
                            <a href="/DeshBoard/UI/Default.aspx">
                                <input type="submit" style="width: 48%" value="Back To Home Page">
                            </a>
                            <a href="https://mail.google.com/mail/u/0/">
                                <input type="submit" style="width: 48%" value="Send Mail to Alchemy"></a>
                        </form>
                    </div>
                </div>
                <div class="w3_main_grid_right w3_agileits_grid_right"> 
                        <div style="height: 180px; color: red;">
                            <%if (Session["ERROR"] != null)
                              {
                                  string err = Session["ERROR"].ToString();
                                  int len = err.Length;
                                  if (len > 500)
                                      err = err.Substring(0, 500) + "...";%>
                         Error: <%=err %>

                            <%} %>
                        </div>
                     
                    <div class="agileits_main_grid">
                        <div class="agileits_main_grid_left">
                            <h4>Follow Us On :</h4>
                        </div>
                        <div class="agileits_main_grid_right">
                            <ul class="agileinfo_social">
                                <li><a class="wthree_facebook" href="http://www.facebook.com/AlchemySoftware" target="_blank"><i class="fa fa-facebook" aria-hidden="true"></i></a></li>
                                <li><a class="agile_twitter" href="https://twitter.com/alchemybd" target="_blank"><i class="fa fa-twitter" aria-hidden="true"></i></a></li>
                                <li><a class="w3_agile_instagram" href="https://www.youtube.com/channel/UCLjaZsyNzsZxgu3hWzuTdaw" target="_blank"><i class="fa fa-youtube" aria-hidden="true"></i></a></li>
                                <li><a class="w3_agile_instagram" href="https://plus.google.com/100639053145643474788" target="_blank"><i class="fa fa-google-plus" aria-hidden="true"></i></a></li>
                                 
                            </ul>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="footer">
            <a style="color: white; margin-top: 5px; text-decoration: none;" href="http://alchemy-bd.com/contact" target="_blank">Contact </a>| <a style="color: white" href="http://alchemy-bd.com/faq" target="_blank">Faq </a>
            <br />
            <span><a style="color: white; text-decoration: none" href="http://alchemy-bd.com" target="_blank">Developed By Alchemy Software</a></span>
        </div>
    </form>
</body>
</html>
