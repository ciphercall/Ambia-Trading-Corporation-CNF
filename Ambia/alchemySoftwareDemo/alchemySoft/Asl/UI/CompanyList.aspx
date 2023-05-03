<%@ Page Title="" Language="C#" MasterPageFile="~/alchemy.Master" AutoEventWireup="true" CodeBehind="CompanyList.aspx.cs" Inherits="alchemySoft.Asl.UI.CompanyList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="alchemySoft" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-md-10 pull-right" id="mainContentBox">
        <div id="contentBox">
            <div id="contentHeaderBox">
                <h1>Company Information</h1>
                <!-- <span class="pull-right" id="editOption"><i class="fa fa-cog"></i></span> -->


                <!-- logout option button -->
                <div class="btn-group pull-right" id="editOption">
                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                        <i class="fa fa-cog"></i>
                    </button>
                    <ul class="dropdown-menu pull-right" style="" role="menu">
                        <li><a href="#"><i class="fa fa-plus"></i>Add record</a>
                        </li>
                        <li><a href="#"><i class="fa fa-edit"></i>Edit record</a>
                        </li>
                        <li><a href="#"><i class="fa fa-times"></i>Delete record</a>
                        </li>

                    </ul>
                </div>
                <!-- end logout option -->


            </div>
            <!-- content header end -->


            <div class="content_wrapper">
                <div class="col-md-12">
                    <div class="panel panel-primary table table-responsive">
                        <!-- Default panel contents -->
                        <div class="panel-heading">
                            <table style="font-size: 14px; width: 100%">
                                <tr class="">
                                    <td style="width: 12%; text-align: center">Company ID</td>
                                    <td style="width: 20%; text-align: center">Company Name</td>
                                    <td style="width: 18%; text-align: center">Address</td>
                                    <td style="width: 15%; text-align: center">Contact No</td>
                                    <td style="width: 10%; text-align: center">Email</td>
                                    <td style="width: 15%; text-align: center">Web Site</td>
                                    <td style="width: 10%; text-align: center">Status</td>
                                </tr>
                            </table>
                        </div>

                        <!-- Table -->
                        <table class="table" style="font-size: 12px">

                            <% SqlConnection con = new SqlConnection(dbFunctions.Connection);
                                if (con.State != ConnectionState.Open) con.Open();
                                SqlCommand cmd = new SqlCommand("SELECT COMPID, COMPNM, ADDRESS, CONTACTNO,EMAILID, WEBID, STATUS FROM ASL_COMPANY WHERE COMPID !='100'", con);
                                SqlDataReader dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {%>
                            <tr>
                                <td style="width: 12%; text-align: center"><%=dr["COMPID"].ToString() %></td>
                                <td style="width: 20%; text-align: left"><%=dr["COMPNM"].ToString() %></td>
                                <td style="width: 23%; text-align: left"><%=dr["ADDRESS"].ToString() %></td>
                                <td style="width: 15%; text-align: center"><%=dr["CONTACTNO"].ToString() %></td>
                                <td style="width: 10%; text-align: left"><%=dr["EMAILID"].ToString() %></td>
                                <td style="width: 15%; text-align: left"><%=dr["WEBID"].ToString() %></td>
                                <td style="width: 7%;  text-align: center"><%=dr["STATUS"].ToString() %></td>
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
