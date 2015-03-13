<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%-- <% Html.EnableClientValidation(); %>
  <% using (Ajax.BeginForm("AddCategoryPDFForm", new { id = ViewData["categoryId"], pid = ViewData["parentId"] }, new AjaxOptions { OnSuccess = "pageload()" }))
               {%>--%>
<table>
    <%-- <tr>
  <td style="width:50px"></td>
 <td colspan="2" style="background-color: #C8C8C8;"><label><b>PDF Photo</b></label></td>
 <td colspan="2" style="background-color: #C8C8C8;"><label><b>PDF File</b></label></td>
 <td colspan="2" style="background-color: #C8C8C8;"><label><b>PDF Name</b></label></td>
 <td style="background-color: #C8C8C8;"></td>
 <td style="background-color: #FFFFFF;"></td>
 </tr>--%>
  <tr>
        <td style="width: 50px">
            
        </td>
        <td>
            <b>PDF Thumbnail Url</b>
        </td>
        
        <td>
            <b>PDF File Url </b>
        </td>
        
        <td>
            <b>PDF File Name </b>
        </td>
        <td>
            <b>Version </b>
        </td>
        
      <td>
          
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td style="width: 50px">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>1.</b>
            <input id="HiddenFileId" name="HiddenFileId" type="hidden" value='<%=ViewData["categoryId"]%>' />
        </td>
        <td>
            <input type="text" id="image1" name="image1" size="30">
        </td>
        <td>
            <input type="text" id="file1" name="file1" size="30">
        </td>
        <td>
            <input type="text" id="TxtFile1" name="TxtFile1" size="30" />
        </td>
       <td>
       <input type="text" id='TxtAddVersion1' name='TxtAddVersion1' size="3"  />
       </td>
        <td>
            <a href="javascript:Add()"><img src="<%= Url.Content("~/Content/images/add.png")%>" width="20" height="20" alt="Add Row" /></a><a href="javascript:Remove()"><img src="<%= Url.Content("~/Content/images/Remove.png")%>" width="20" height="20" alt="Remove Row" /></a>
        </td>
        <td>
        </td>
    </tr>
</table>
<div id="PDFcontent">
</div>
<table>
    <tr>
        <td style="width: 50px">
        </td>
        <td style="height: 50px; vertical-align: bottom;">
        <b><label id="checkNew"></label></b>
        </td>
        <td style="height: 50px; vertical-align: bottom;">
            <div align="right">
               Do you want to send notification? <%= Html.CheckBox("CheckAddPDF", null) %> <input type="submit" value="Save Changes" onclick="javascript:return OnSubmit(3);" /></div>
        </td>
    </tr>
</table>
<%--   <%} %>--%>
