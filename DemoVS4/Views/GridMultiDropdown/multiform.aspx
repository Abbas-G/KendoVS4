<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>multiform</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black" href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"
        id="stylesheet" />
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min2.js")%>"></script>
    <%--<script src="<%= Url.Content("~/Content/kendo/js/kendo.web.min.js")%>"></script>--%>
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="<%= Url.Content("~/Content/CustomLoader.css")%>" />
    <script src="<%= Url.Content("~/Scripts/CustomLoader.js") %>" type="text/javascript"></script>
</head>
<body>
    <div>
    <% using (Ajax.BeginForm("multiform", "GridMultiDropdown", new AjaxOptions { UpdateTargetId = "status", OnSuccess = "fadeover", HttpMethod = "Post" }, new { id = "AddNewFormId" }))
   {%>
    <select name="Category" id="Category"></select>
    <button class="k-button" type="button" name="Submit" id="Submit" >Submit</button>
   <div class="status" id="status">
                </div>
                <div id="popup_name" class="popup_block">
        <div class="k-loading-image">
        </div>
   <%} %>
    </div>
    <script>
        var xhReq = new XMLHttpRequest();
        xhReq.open("POST", '<%=Url.Content("~/GridMultiDropdown/GetJsonOutputItem")%>', false);
        xhReq.send(null);
        var GlobalSearchFOOD = JSON.parse(xhReq.responseText);
        var data = [
            { Text: "Test1", Value: "1" },
            { Text: "Test2", Value: "2" },
            { Text: "Test3", Value: "3" }
        ];

        $(document).ready(function () {
            $("#Category").kendoMultiSelect({
                placeholder: "Select Category...",
                //dataSource: GlobalSearchFOOD
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: data
            });

            var multiSelect = $("#Category").data("kendoMultiSelect");

            if (multiSelect.value() != "")
                multiSelect.value($("#Category").val().split(", "));

            $("#Submit").click(function () {
                    $("#AddNewFormId").submit();
            });

                });
    </script>
</body>
</html>
