<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="MenuList.aspx.cs" Inherits="alchemySoft.Asl.Report.Report.MenuList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="alchemySoft" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Menu Information</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                <!-- logout option button -->
                <div class="btn-group pull-right" id="editOption">
                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                        <i class="fa fa-cog"></i>
                    </button>
                    <ul class="dropdown-menu pull-right" style="" role="menu">
                        <li><a href="/Asl/UI/MenuCreate.aspx"><i class="fa fa-plus"></i>Add Menu</a>
                        </li>
                        <li><a href="/Asl/UI/MenuUpdate.aspx"><i class="fa fa-edit"></i>Edit Menu</a>
                        </li>
                        <li><a href="/Asl/UI/MenuDelete.aspx"><i class="fa fa-times"></i>Delete Menu</a>
                        </li>

                    </ul>
                </div>
                <!-- end logout option -->


            </div>
            <!-- content header end -->


            <div class="content_wrapper">
                <div class="col-md-12">
                    <div class="panel panel-primary table-responsive">
                        <!-- Default panel contents -->
                        <div class="panel-heading">Menu List</div>

                        <!-- Table -->
                        <table class="table" style="font-size: 12px">
                            <tr class="table-head">
                                <td style="width: 20%; text-align: center">Module Name</td>
                                <td style="width: 10%; text-align: center">Menu Type</td>
                                <td style="width: 30%; text-align: center">Menu Name</td>
                                <td style="width: 50%; text-align: center">Link</td>
                            </tr>
                            <% SqlConnection con = new SqlConnection(dbFunctions.Connection);
                                if (con.State != ConnectionState.Open) con.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT ASL_MENUMST.MODULENM, CASE(ASL_MENU.MENUTP) WHEN 'F' THEN 'FORM' WHEN 'R' THEN 'REPORT' END AS MENUTP, 
                            ASL_MENU.MENUNM, ASL_MENU.FLINK FROM ASL_MENU INNER JOIN
                            ASL_MENUMST ON ASL_MENU.MODULEID = ASL_MENUMST.MODULEID
                            ORDER BY ASL_MENUMST.MODULENM, ASL_MENU.MENUTP, ASL_MENU.MENUNM", con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                foreach (var item in dr)
                                {%>
                            <tr>
                                <td style="width: 20%; text-align: left"><%=dr["MODULENM"].ToString() %></td>
                                <td style="width: 10%; text-align: center"><%=dr["MENUTP"].ToString() %></td>
                                <td style="width: 30%; text-align: left"><%=dr["MENUNM"].ToString() %></td>
                                <td style="width: 50%; text-align: left"><%=dr["FLINK"].ToString() %></td>

                            </tr>
                            <%}
                                dr.Close();
                                if (con.State != ConnectionState.Closed) con.Close();
                            %>
                        </table>
                    </div>

                </div>
            </div>
        </div>
    </div>


</asp:Content>
