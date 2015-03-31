<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black"  href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"  id="stylesheet"/>
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min.js")%>"></script>

    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="<%= Url.Content("~/Content/CustomLoader.css")%>" />
    <script src="<%= Url.Content("~/Scripts/CustomLoader.js") %>" type="text/javascript"></script>
</head>
<body>
<span id="alertMsg"></span>
    <div id="TargetContent">
    <%=Html.Action("childajax", "PartialViewChildAction", new { msg = "From Page" })%>
    </div>
    <input type="button" onclick="onChange()" value="Search" class="k-button"/>
    <script>
        function onChange() {
            fadeinout();
            $.ajax({
                url: '<%= Url.Content("~/PartialViewChildAction/childajax") %>',
                cache: false,
                data: { 'msg': 'From Ajax' },
                success: function (html, textStatus, XMLHttpRequest) {
                    if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                        //window.location = '<%=Url.Content("~/Account/LogOn") %>';
                        // window.location.reload();
                    }
                    else {
                        $('#TargetContent').html(html);
                        fadeover();
                    }
                },
                error: function (msg) {
                    fadeover();
                    alert("server error!!");

                }
            });
            document.getElementById('alertMsg').innerHTML = '';
        }
    </script>
</body>
</html>
