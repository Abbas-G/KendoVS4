<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black"  href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"  id="stylesheet"/>
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min2.js")%>"></script>
    <%--<script src="<%= Url.Content("~/Content/kendo/js/kendo.web.min.js")%>"></script>--%>
</head>
<body>
     <div>
        <div id="grid" name="grid">
        </div>
    </div>

    <script>
        //reference link http://blog.longle.net/tag/server-side-paging/
        var record = 0;
        $(document).ready(function () {
            $("#grid").kendoGrid({
                dataSource: {
                    type: "json",
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    pageSize: 3,
                    transport: { read: { url: "/ServerGrid/GetAllWithServerOptions", dataType: "json"} },
                    schema: { data: "File", total: "TotalCount" }
                },
                pageable: true,
                columns: [
                     { title: "&nbsp;", template: "#= ++record #", width: 30 },
                    { field: "text", title: "Text" },
                    { field: "value", title: "Value", template: " <img src='/Content/kendo/peoples/#= value #' height='70' width='70'/>" }
                ],
                dataBinding: onDataBinding,
                dataBound: onDataBound,
                change: onChange,
                pageable: {
                    refresh: true,
                    pageSizes: true
                },
                filterable: true,
                sortable: true,
                selectable: true
            });
        });

        function onChange(arg) {
            /*var selected = $.map(this.select(), function(item) {
            return $(item).text();
            });
            alert("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");*/
        }
        function onDataBound(arg) {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
        }

        function onDataBinding(arg) {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    </script>
</body>
</html>
