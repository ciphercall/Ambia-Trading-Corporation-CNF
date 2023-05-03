<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="alchemySoft._Default" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="/MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="/MenuCssJs/js/jquery-ui.js"></script>


    <style type="text/css">
        .panel-title {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- main content start here -->
    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>HOME</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

            </div>
            <!-- content header end -->
            <%--<div class="content_wrapper" style="height: calc(100vh - 124px) !important;">
                </div>--%>
            <div class="content_wrapper">

                <%--  <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true" style="padding: 10px">
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingOne">
                        <h4 class="panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                            <a style="text-decoration: none">USER INFORMATION
                            </a>
                        </h4>
                    </div>
                    <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                        <div class="panel-body">
                             
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingTwo">
                        <h4 class="panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                            <a style="text-decoration: none">OWN INFORMATION
                            </a>
                        </h4>
                    </div>
                    <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                        <div class="panel-body">
                             
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingThree">
                        <h4 class="panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                            <a style="text-decoration: none">JOURNAL VOUCHER
                            </a>
                        </h4>
                    </div>
                    <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                        <div class="panel-body">
                             
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingFour">
                        <h4 class="panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour">
                            <a style="text-decoration: none">CONTRA VOUCHER
                            </a>
                        </h4>
                    </div>
                    <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                        <div class="panel-body">
                             
                        </div>
                    </div>
                </div>
            </div>--%>
            </div>
        </div>
        <!-- content box end here -->


    </div>
    <!-- main content end here -->


</asp:Content>
