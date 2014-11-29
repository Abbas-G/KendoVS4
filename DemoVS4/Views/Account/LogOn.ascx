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



<%--<% using (Html.BeginForm("Foo", "Bar", FormMethod.Post, new { @class = "myclass"})) { %>
                            <label style="color:red;"><%= Html.ValidationSummary(true, "") %></label>
                            <table style="width:300px;border:0" >
                            <tr>
                                <td>
                                     <label>
                                    Username</label>
                                </td>
                                <td>
                                     <%= Html.TextBoxFor(m => m.UserName, new { Style="color: rgb(150, 150, 150);" }) %> 
                                </td>
                            </tr>
                                 <tr>
                                <td>
                                     <label>
                                    Password</label>
                                </td>
                                <td> <%= Html.PasswordFor(m => m.Password, new { Style = "color: rgb(150, 150, 150);" })%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="color:Red">
                                   <%= Html.ValidationMessageFor(m => m.UserName) %><br />
                            <%= Html.ValidationMessageFor(m => m.Password) %>
                                </td>
                            </tr>
                            </table>
                           
                           
                            <input type="submit" value="Login">
                               <% } %>  --%>