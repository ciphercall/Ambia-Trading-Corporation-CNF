<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="RptRecPayDet.aspx.cs" Inherits="DynamicMenu.Accounts.Report.UI.RptRecPayDet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindControlEvents();
        });

        function BindControlEvents() {
            // $("#txtFrom,#txtTo").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+10" });
            $(function () {
                $("#txtFrom").datepicker({
                    defaultDate: "",
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "-10:+10",
                    onClose: function (selectedDate) {
                        $("#txtTo").datepicker("option", "minDate", selectedDate);
                    }
                });
                $("#txtTo").datepicker({
                    defaultDate: "",
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "-10:+10",
                    onClose: function (selectedDate) {
                        $("#txtFrom").datepicker("option", "maxDate", selectedDate);
                    }
                });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Receipts  Payment Statement Details</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


            </div>
            <!-- content header end -->


            <!-- Content Start From here -->
           <br />
          <div class="content_wrapper  col-md-12">
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">From</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control input-sm"
                            ClientIDMode="Static" TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">To</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTo" runat="server" CssClass="form-control input-sm"
                            ClientIDMode="Static" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-4"></div>
                </div>
                <div class="row form-class">
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search"
                            CssClass="form-control input-sm btn-primary" OnClick="btnSearch_Click" TabIndex="3" />
                    </div>
                    <div class="col-md-5"></div>

                </div>
            </div>
            <!-- Content End From here -->
        </div>
    </div>
</asp:Content>
