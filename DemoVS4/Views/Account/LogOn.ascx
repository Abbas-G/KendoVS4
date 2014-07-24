<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DemoVS4.Models.LogOnModel>" %>

<% using (Ajax.BeginForm("LogOn", "Account", new AjaxOptions { UpdateTargetId = "divLoginUserControl", OnSuccess = "fadeover", HttpMethod = "Post" }))
   {%>
<input type="text" placeholder="Username" id="UserName" name="UserName"
    value="<%=Model.UserName %>">
<input type="password"  placeholder="Password" id="Password"
    name="Password" value="<%=Model.Password %>">
<div style="color: red;">
    <%= Html.ValidationMessageFor(m => m.UserName)%></div>
<div style="color: red;">
    <%= Html.ValidationMessageFor(m => m.Password)%></div>
<div style="color: red;">
    <%= Html.ValidationSummary(true, "")%></div>
<input type="submit" class="btn btn-default" onclick="fadeinout();" value="Login">

<%} %>