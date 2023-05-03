<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="ChartofAccounts.aspx.cs" Inherits="alchemy.accounts.UI.ChartofAccounts" %>

<%@ Import Namespace="alchemySoft" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../MenuCssJs/ui-gray/jquery-ui.css" rel="stylesheet" />
    <script src="../../MenuCssJs/js/jquery-2.1.3.js"></script>
    <script src="../../MenuCssJs/js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // BindControlEvents();
            //var dlg = $("").dialog({
            //    autoOpen: false,
            //    modal: false,
            //    title: "",
            //    position: ["center", "center"],
            //    resizable: false
            //});
            //dlg.parent().appendTo(jQuery("form:first"));

        });
        function pageLoad() {
            BindControlEvents();
        }
        function BindControlEvents() {
            $("#txtDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "-100:+100" });
            Search_GetCompletionListLavelCode();
        }
        //function shai_dailog() {
        //    $("").dialog("open");
        //}
        function ConfirmationBox(username) {
            var result = confirm('Are you sure you want to delete ' + username + ' Details?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        function confMSG() {
            if (confirm("Are you Sure to Delete?")) { }
            else { return false; }
        }

        function Search_GetCompletionListLavelCode() {
            $("#<%=txtHdName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "../../search.asmx/GetCompletionListLavelCode",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'txt' : '" + $("#<%=txtHdName.ClientID %>").val() + "'}",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    value: item
                                };
                            }));
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    $("#<%=btnSubmit.ClientID %>").focus();
                    return true;
                }
            });
            }

    </script>

    <style>
        .ui-autocomplete {
            max-width: 350px;
            max-height: 250px;
            overflow: auto;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>


            <div class="col-md-10 pull-right" id="mainContentBox">
                <div id="contentBox">
                    <div id="contentHeaderBox">
                        <h1>Chart of Accounts Entry</h1>
                        <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->

                        <%Session["UPDATER_chartofAcc"] = dbFunctions.checkParmit("/Accounts/UI/ChartofAccounts.aspx", "UPDATER");
                            Session["DELETER_chartofAcc"] = dbFunctions.checkParmit("/Accounts/UI/ChartofAccounts.aspx", "DELETER");
                            Session["INSERTR_chartofAcc"] = dbFunctions.checkParmit("/Accounts/UI/ChartofAccounts.aspx", "INSERTR"); %>
                        <!-- logout option button -->
                        <div class="btn-group pull-right" id="editOption">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <i class="fa fa-cog"></i>
                            </button>
                            <ul class="dropdown-menu pull-right" style="" role="menu">
                                <% if (Session["INSERTR_chartofAcc"].ToString()== "True")
                                    { %>
                                <li><a href="CostPool.aspx"><i class="fa fa-plus"></i>Create</a>
                                </li>
                                <% } %>

                                <% if (Session["UPDATER_chartofAcc"].ToString()== "True")
                                    { %>
                                <li><a href="CostPool.aspx"><i class="fa fa-edit"></i>Edit</a>
                                </li>
                                <% } %>

                               <% if (Session["DELETER_chartofAcc"].ToString()== "True")
                                    { %>
                                <li><a href="CostPool.aspx"><i class="fa fa-edit"></i>Delete</a>
                                </li>
                                <% } %>
                            </ul>
                        </div>
                        <!-- end logout option -->
                    </div>
                    <!-- content header end -->


                    <!-- Content Start From here -->
                    <div class="content_wrapper  col-md-12">
                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlLevelID" runat="server" OnSelectedIndexChanged="ddlLevelID_SelectedIndexChanged"
                                    AutoPostBack="True" TabIndex="1" CssClass="form-control input-sm" Width="100%">
                                    <asp:ListItem>SELECT</asp:ListItem>
                                    <asp:ListItem Value="1">ASSET</asp:ListItem>
                                    <asp:ListItem Value="2">LIABILITY</asp:ListItem>
                                    <asp:ListItem Value="3">INCOME</asp:ListItem>
                                    <asp:ListItem Value="4">EXPENDETURE</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">

                                <asp:TextBox ID="txtHdName" runat="server" TabIndex="2" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                                <asp:TextBox ID="txtCode" runat="server" ReadOnly="True" TabIndex="6"
                                    Visible="False"></asp:TextBox>

                            </div>

                            <div class="col-md-2">
                                <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" Width="100%"
                                    OnClick="btnSubmit_Click" TabIndex="7" CssClass="form-control input-sm btn-primary" />
                            </div>
                        </div>

                        <div class="row form-class">
                            <div class="col-md-2"></div>
                            <div class="col-md-3">Level Code:<asp:Label ID="lblLvlID" runat="server" Text="LevelCode"></asp:Label></div>
                            <div class="col-md-2">Sub Level Code :<asp:Label ID="lblBotCode" runat="server"></asp:Label></div>
                            <div class="col-md-3"></div>
                            <div class="col-md-2"></div>
                        </div>

                        <div class="text-center">
                            <asp:Label ID="lblIncrLevel" runat="server" Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="lblSelLvlCD" runat="server" Visible="False"></asp:Label>
                        </div>


                        <asp:Label ID="lblMxAccCode" runat="server" Text="MaxAccCode"></asp:Label>
                        <asp:Label ID="lblNewLvlCD" runat="server"></asp:Label>
                        <asp:Label ID="lblAccTP" runat="server"></asp:Label>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        <asp:Label ID="lblBRCD" runat="server"></asp:Label>
                        
                        <div class="table table-responsive table-hover" style="border: 1px solid #ddd; border-radius: 5px;">
                            <asp:GridView ID="gvDetails" runat="server" BackColor="White" BorderStyle="None" CssClass="Gridview text-center"
                                AutoGenerateColumns="False" ShowFooter="True"
                                CellPadding="3" CellSpacing="1" GridLines="Both" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                                OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating"
                                OnRowCommand="gvDetails_RowCommand" OnRowDataBound="gvDetails_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Account Head">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccHdNM" runat="server" Text='<%# Eval("ACCOUNTNM") %>' Width="98%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAccHead" runat="server" Text='<%#Eval("ACCOUNTNM") %>' TabIndex="20"
                                                CssClass="form-control input-sm" Width="98%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAccHead" runat="server" Width="98%" TabIndex="8" CssClass="form-control input-sm" />
                                        </FooterTemplate>
                                        <HeaderStyle Width="45%" />
                                        <ItemStyle Width="45%" HorizontalAlign="Left" CssClass="paddingleft10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcountCode" runat="server" Text='<%#Eval("ACCOUNTCD") %>' Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAccCode" runat="server" Text='<%#Eval("ACCOUNTCD") %>' ReadOnly="True" CssClass="form-control input-sm"
                                                Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Control Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblControlCode" CssClass="" runat="server" Text='<%#Eval("CONTROLCD") %>' Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtContolCode" CssClass="" runat="server" Text='<%#Eval("CONTROLCD") %>' ReadOnly="True"
                                                Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Access">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccess" runat="server" Text='<%#Eval("BRANCHNM") %>' Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlAccessEdit" runat="server" Width="98%" CssClass="form-control input-sm" TabIndex="21"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlAccessEdit_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlAccess" runat="server" Width="98%" AutoPostBack="True" CssClass="form-control input-sm" TabIndex="9"
                                                OnSelectedIndexChanged="ddlAccess_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AccessCD" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccessCD" runat="server" Text='<%#Eval("BRANCHCD") %>' Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblAccessCDEdit" runat="server" Width="98%">
                                            </asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblAccessCDFot" runat="server" Width="98%">
                                            </asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" CssClass="txtColor"
                                                ImageUrl="~/Images/update.png" ToolTip="Update" Height="20px" Width="20px" TabIndex="22" />
                                            <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" CssClass="txtColor"
                                                ImageUrl="~/Images/Cancel.png" ToolTip="Cancel" Height="20px" Width="20px" TabIndex="23" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                          <% if (Session["UPDATER_chartofAcc"].ToString()== "True")
                                                { %>
                                            <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" CssClass="txtColor"
                                                ImageUrl="~/Images/Edit.png" ToolTip="Edit" Height="20px" Width="20px" TabIndex="30" />
                                            <% } %>
                                           <% if (Session["DELETER_chartofAcc"].ToString()== "True")
                                                { %>
                                            <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" Text="Edit" runat="server"
                                                CssClass="txtColor" ImageUrl="~/Images/delete.png" ToolTip="Delete" Height="20px"
                                                Width="20px" OnClientClick="return confMSG()" TabIndex="31" />
                                            <% } %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                           <% if (Session["INSERTR_chartofAcc"].ToString()== "True")
                                                { %>
                                            <asp:ImageButton ID="imgbtnAdd" runat="server" CssClass="txtColor" ImageUrl="~/Images/AddNewitem.png"
                                                CommandName="AddNew" Width="25px" Height="25px" ToolTip="Add new Record" ValidationGroup="validaiton"
                                                TabIndex="10" />
                                            <% } %>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle Font-Names="Calibri" Font-Size="14px" />
                                <HeaderStyle CssClass="text-center" HorizontalAlign="Center" Height="30px" Font-Size="13px" BackColor="#D9EDF7" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            </asp:GridView>


                        </div>

                        <div class="text-center">
                            <h5>
                                <asp:Label ID="lblresult" runat="server"></asp:Label></h5>
                        </div>

                    </div>
            </div>
            <!-- Content End From here -->
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
