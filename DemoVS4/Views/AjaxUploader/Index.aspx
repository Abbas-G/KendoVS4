<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black"  href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"  id="stylesheet"/>
    <%--<script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>--%>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.js"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min.js")%>"></script>

    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="<%= Url.Content("~/Content/CustomLoader.css")%>" />
    <script src="<%= Url.Content("~/Scripts/CustomLoader.js") %>" type="text/javascript"></script>
    
<script src="http://malsup.github.com/jquery.form.js"></script>
</head>
<body>
    <div>
    <% using (Ajax.BeginForm("AjaxForm", "AjaxUploader", new AjaxOptions() { HttpMethod = "POST" }, new { enctype = "multipart/form-data" }))
{%>
    <%=Html.AntiForgeryToken()%>
    <input type="file" name="files"><br>
    <input type="submit" value="Upload File to Server">
<%}%>
    </div>

    <div class="progress progress-striped">
        <div class="progress-bar progress-bar-success">0%</div>
    </div>

<div id="status"></div>
</body>
<script>
    (function () {

        var bar = $('.progress-bar');
        var percent = $('.progress-bar');
        var status = $('#status');

        $('form').ajaxForm({
            beforeSend: function () {
                status.empty();
                var percentVal = '0%';
                bar.width(percentVal)
                percent.html(percentVal);
            },
            uploadProgress: function (event, position, total, percentComplete) {
                var percentVal = percentComplete + '%';
                bar.width(percentVal)
                percent.html(percentVal);
            },
            success: function () {
                var percentVal = '100%';
                bar.width(percentVal)
                percent.html(percentVal);
            },
            complete: function (xhr) {
                status.html(xhr.responseText);
            }
        });

    })();       
</script>
</html>
