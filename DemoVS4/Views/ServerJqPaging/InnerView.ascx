<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<System.IO.FileInfo>>" %>
<ui>
<%foreach(var m in Model) {%>
<li> <%=m.Name %></li>
<%} %>
</ui>