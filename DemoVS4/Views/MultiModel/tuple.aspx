<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Tuple <List<DemoVS4.Controllers.Teacher>, List <DemoVS4.Controllers.Student>, DemoVS4.Controllers.Product>>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>tuple</title>
</head>
<body>
<p><b>Teacher List</b></p>
<table>
    <tr>
        <th>Id</th>
        <th>Code</th>
        <th>Name</th>
    </tr>
   <% foreach (var teacher in Model.Item1)
    {%>
        <tr>
            <td><%=teacher.TeacherId%></td>
            <td><%=teacher.Code%></td>
            <td><%=teacher.Name%></td>
        </tr>
     <%}%>
</table>
<p><b>Student List</b></p>
<table>
    <tr>
        <th>Id</th>
        <th>Code</th>
        <th>Name</th>
        <th>Enrollment No</th>
    </tr>
     <%foreach (var student in Model.Item2)
    {%>
        <tr>
            <td><%=student.StudentId%></td>
            <td><%=student.Code%></td>
            <td><%=student.Name%></td>
            <td><%=student.EnrollmentNo%></td>
        </tr>
     <%}%>
</table>

<% using (Html.BeginForm("form", "MultiModel", FormMethod.Post, new { @class = "myclass" }))
   {%>
   <%=Html.TextBoxFor(x=>Model.Item3.ProductName) %><br />
   <input type="submit" value="Login">

<%} %>
</body>
</html>
