<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>bootpag</title>
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
        <script src="<%= Url.Content("~/Content/bootpag/bootstrap.min.js")%>"></script>
        <script type="text/javascript" src="<%= Url.Content("~/Content/bootpag/jquery.bootpag.js")%>"></script>
        <link href="<%= Url.Content("~/Content/bootpag/bootstrap-combined.min.css")%>" rel="stylesheet">
         <link rel="stylesheet" type="text/css" media="screen" href="<%= Url.Content("~/Content/CustomLoader.css")%>" />
        <script src="<%= Url.Content("~/Scripts/CustomLoader.js") %>" type="text/javascript"></script>

        <style type="text/css">
            p{
                text-align: center;
            }
        </style>
</head>
<body>
    <div>
    <input type="hidden" id="hdntotal" value="<%=ViewData["Total"]%>" />
    <input type="hidden" id="PSize" value="<%=ViewData["PageSize"]%>" />
    <p class="page4">No page selected</p>
        <p class="demo4 pagination"></p>

        <script type="text/javascript">
            $(document).ready(function () {
                $.ajax({
                    url: '<%= Url.Content("~/ServerJqPaging/getPage") %>',
                    data: { "page": 1, "total": document.getElementById("hdntotal").value },
                    type: "POST",
                    cache: false,
                    success: function (html, textStatus, XMLHttpRequest) {
                        if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                            window.location = '<%=Url.Content("~/Account/LogIn") %>';
                            window.location.reload();
                        }
                        else {
                            $('.page4').html(html);
                        }
                    }
                });

            });

            $('.demo4').bootpag({
                total: <%=ViewData["PageNumber"]  %>,
                //page: 2,
                maxVisible: 5,
                leaps: false,
                firstLastUse: true,
                //href: '#page-{{number}}',
                first: 'First',
                last: 'Last'
            }).on('page', function (event, num) {
                //$(".page4").html("Page " + num);
                fadeinout();
                $.ajax({
                    url: '<%= Url.Content("~/ServerJqPaging/getPage") %>',
                    data: { "page": num },
                    type: "POST",
                    cache: false,
                    success: function (html, textStatus, XMLHttpRequest) {
                        if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                            window.location = '<%=Url.Content("~/Account/LogIn") %>';
                            window.location.reload();
                        }
                        else {
                            $('.page4').html(html);
                            fadeover();
                        }
                    }
                });

            });
        </script>
    </div>

    <div id="popup_name" class="popup_block">
        <div class="k-loading-image"></div>
    </div>
</body>
</html>
