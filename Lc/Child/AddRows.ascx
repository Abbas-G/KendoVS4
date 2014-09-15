<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript" language="javascript">
	     var intTextBox = 3;
      function Add() {
        if (intTextBox > 51) {
                alert("Maximum 50 allow");   
            }
            else
            {

                var contentID = document.getElementById('AddMore');
                var newTBDiv = document.createElement('div');
                newTBDiv.setAttribute('id', 'file_div' + intTextBox);
                var i= parseInt(intTextBox)-1;
                
                newTBDiv.innerHTML = "<table><tr><td ><b>"+i+".</b></td><td><input type='text' id='RName" + i + "' name='RName" + i + "' size='25'/></td><td><input type='text' id='RBrief" + i + "' name='RBrief" + i + "' size='55'/></td><td> <a href='javascript:Add()'  ><img src='<%=Url.Content("~/Content/images/add.png")%>' width='20' height='20' alt='Add Row' /></a>&nbsp;<a href='javascript:Remove()'  ><img src='<%=Url.Content("~/Content/images/Remove.png")%>' width='20' height='20' alt='Remove Row' /></a></td><td></td></tr></table>";

                contentID.appendChild(newTBDiv);


                intTextBox = intTextBox + 1;
            }

        }

        function Remove(){
            if (intTextBox == 3) {
              
              
            }
            else {
                
                intTextBox = intTextBox - 1;
                var contentID = document.getElementById('AddMore');
                contentID.removeChild(document.getElementById('file_div' + intTextBox));

            }

        }
</script>
<%using (Html.BeginForm("Index", "Child", FormMethod.Post, new { enctype = "multipart/form-data", @name = "contacts-form", @id = "contacts-form" }))
  {%>
<table>
    <tr>
        <td>
        </td>
        <td>
            <b>Name</b>
        </td>
        <td>
            <b>Brief</b>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <b>1.</b>
            <input id="HiddenFileId" name="HiddenFileId" type="hidden" value='<%=ViewData["categoryId"]%>' />
        </td>
        <td>
            <input type="text" id="RName1" name="RName1" size="25">
        </td>
        <td>
            <input type="text" id="RBrief1" name="RBrief1" size="55">
        </td>
        <td>
            <a href="javascript:Add()">
                <img src="<%= Url.Content("~/Content/images/add.png")%>" width="20" height="20" alt="Add Row" /></a>
            <a href="javascript:Remove()">
                <img src="<%= Url.Content("~/Content/images/Remove.png")%>" width="20" height="20"
                    alt="Remove Row" /></a>
        </td>
        <td>
        </td>
    </tr>
    <input type="submit" value="save" /><%=ViewData["OnSuccess"]%>
</table>
<div id="AddMore">
</div>
<%} %>
