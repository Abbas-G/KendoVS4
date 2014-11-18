<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DemoVS4.Core.DAL.Product>>" %>
<ui>
<%foreach(var m in Model) {%>
<li> <%=m.ProductName %></li>
<%} %>
</ui>