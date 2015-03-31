<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ReportB</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black"  href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"  id="stylesheet"/>
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min2.js")%>"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="<%= Url.Content("~/Content/CustomLoader.css")%>" />
    <script src="<%= Url.Content("~/Scripts/CustomLoader.js") %>" type="text/javascript"></script>
</head>
<body>
    <div style="margin-left:8%"><b>Stats from </b>

     <input id="startDate" value="2014-03-30"  />
    <b> to</b>
    <input id="endDate" />
    <input type="button" onclick="onChange()" value="Search" class="k-button"/>
    <span id="alertMsg"></span>
    <br />
    <br />
    <div id="TargetContent">
    <% string eDate = System.DateTime.Now.ToString("yyyy-MM-dd"); %>
        <%=Html.Action("PartialReportB", "Report", new { StartDate = eDate, EndDate = eDate })%>
    </div>
    </div>
    <script>
        var GstartDate, GendDate;
        $(document).ready(function () {
            $("#menu").kendoMenu();

            $("#startDate").kendoDatePicker({
                format: 'yyyy-MM-dd',
                value: new Date()
            });
            $("#endDate").kendoDatePicker({
                format: 'yyyy-MM-dd',
                value: new Date()
            });
        });

        function onChange() {
            var startDate = new Date($("#startDate").val());
            var endDate = new Date($("#endDate").val());
            GstartDate = $("#startDate").val();
            GendDate = $("#endDate").val();
            if (startDate > endDate) {
                //alert("Start date cnnot be greater then end date!!");
                document.getElementById('alertMsg').innerHTML = '<label style="color:red">Start date cannot be greater then end date!!</label>';
            }
            else {
                fadeinout();
                $.ajax({
                    url: '<%= Url.Content("~/Report/PartialReportB") %>',
                    cache: false,
                    data: { 'StartDate': $("#startDate").val(), 'EndDate': $("#endDate").val() },
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
        }
    </script>
    <div id="popup_name" class="popup_block">
        <div class="k-loading-image">
        </div>
</body>
</html>
