<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-box-header">
        <ul class="content-box-tabs">
            <li><span>Detail</span></li>
            <li><span class="current">List</span></li>
        </ul>
    </div>
    <div class="content-box-content">
        <%Html.RenderPartial("AddRows"); %>
    </div>
</asp:Content>
