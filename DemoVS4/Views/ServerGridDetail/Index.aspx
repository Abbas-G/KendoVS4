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
        <div id="grid" >
        </div>
    </div>
     <script type="text/x-kendo-template" id="template">
                <div class="toolbar">
                    <label for="products">Search Products by Category:</label><input id="products" class="k-textbox"/>
                    <input type="button" value="search" onClick="ProductChange()" class="k-button"/>
                </div>     
                <button type="button" id="btnExport"  class="k-button" onClick="ExportToCSV()" >Export to csv!</button>
 </script>
    <script>
        //reference link http://blog.longle.net/tag/server-side-paging/
        var xhReq = new XMLHttpRequest();
        xhReq.open("POST", '<%=Url.Content("~/ServerGridDetail/GetJsonOutputForFoodUniqueCategory")%>', false);
        xhReq.send(null);
        var GlobalSearchFOOD = JSON.parse(xhReq.responseText);
        var record = 0;
        $(document).ready(function () {
            $("#grid").kendoGrid({
                dataSource: {
                    type: "json",
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    pageSize: 3,
                    transport: { read: { url: "/ServerGridDetail/GetJsonOutputForGridDataSelect", dataType: "json" }/*,
                        update: {
                            url: "/ServerGridDetail/GetJsonOutputForGridDataUpdatePopup",
                            dataType: "json",
                            type: "POST"
                        },
                        destroy: {
                            url: "/ServerGridDetail/GetJsonOutputForGridDataDeletePopup",
                            dataType: "json",
                            type: "POST"
                        },
                        create: {
                            url: "/ServerGridDetail/GetJsonOutputForGridDataCreatePopup",
                            dataType: "json",
                            type: "POST"
                        },
                        parameterMap: function (options, operation) {
                            if (operation !== "read" && options.models) {
                                return { models: kendo.stringify(options.models) };
                            }
                            //check dis for client to servrer data flow http://www.telerik.com/forums/best-strategies-for-datetime-handling-in-datasource-and-grid
                        } */
                    },
                    schema: { data: "File", total: "TotalCount", model: {
                        id: "ProductID",
                        fields: {
                            ProductID: { editable: false, nullable: true },
                            UniqueCode: { editable: false, nullable: true },
                            ProductName: { validation: { required: true, validationMessage: "Please enter time"} },
                            UnitPrice: { type: "number", validation: { required: true, min: 1} },
                            Discontinued: { type: "boolean" },
                            UnitsInStock: { type: "number", validation: { min: 0, required: true} },
                            Category: { type: "string", validation: { required: true} },
                            CreatedDate: { type: 'date', validation: { required: true} },
                            Duration: { type: "number", editable: false }
                        }
                    }
                    }
                },
                pageable: true,
               /* toolbar: [{
                    name: "my-create",
                    text: "Add new record"
                }],*/
                columns: [
                            { title: "&nbsp;", template: "#= ++record #", width: 30 },
                            { field: "ProductName", title: "Product Name" },
                            { field: "UniqueCode", title: "Unique Code" },
                            { field: "UnitPrice", title: "Unit Price"/*, footerTemplate: "Total: #=sum#"*/, format: "{0:c}" },
                            { field: "UnitsInStock", title: "Units In Stock" },
                            { field: "Discontinued", width: "100px" },
                            { field: "Category", title: "Category", filterable: { ui: GroupFilter }, editor: ColumnGroupFilter },
                            { field: "CreatedDate", title: "Date", type: "date", format: "{0:MM/dd/yyyy}" /* format: "{0:MM/dd/yyyy h:mm:ss tt}"*/ }
                            //,{ command: ["edit"/*, "destroy"*/], title: "Edit", width: "160px" },
                            //{ command: [{ text: 'Delete', click: deleteItem}], title: 'Actions' }
                            ],
                editable: "inline",
                dataBinding: onDataBinding,
                dataBound: onDataBound,
                change: onChange,
                pageable: {
                    refresh: true,
                    pageSizes: true
                },
                filterable: true,
                sortable: true,
                selectable: true,
                edit: function (e) {
                    var title = $(e.container).parent().find(".k-window-title");
                    var update = $(e.container).parent().find(".k-grid-update");
                    var cancel = $(e.container).parent().find(".k-grid-cancel");

                    if (!e.model.ProductID) {
                        $(update).html('<span class="k-icon k-update"></span>Add');
                        $(cancel).html('<span class="k-icon k-cancel"></span>Cancel');
                    } else {
                        $(update).html('<span class="k-icon k-update"></span>Update');
                        $(cancel).html('<span class="k-icon k-cancel"></span>Cancel');
                    }
                },
                save: onSave
            });

            $(".k-grid-my-create", grid.element).on("click", function (e) {
                var grid = $("#grid").data("kendoGrid");
                grid.dataSource.filter({});
                grid.dataSource.sort({});
                grid.addRow();
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

        function ColumnGroupFilter(container, options) {
            $('<input required="required" name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                dataTextField: "Category",
                dataValueField: "Category",
                dataSource: GlobalSearchFOOD

            });
            $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>').appendTo(container);
        }

        function GroupFilter(e) {
            e.kendoDropDownList({
                dataSource: GlobalSearchFOOD,
                dataTextField: "Category",
                dataValueField: "Category"
            });
        }

        function onSave(e) {
             if (e.model.ProductID != null) { }
             else {
                 var currentProductName = e.model.ProductName;
                 $.ajax({
                     url: '<%=Url.Content("~/ServerGridDetail/CheckDuplication")%>'
                               , type: "POST"
                               , data: { ProductName: currentProductName }
                               , async: false
                               , success: function (result) {
                                   if (result.value == 'true') {
                                       e.preventDefault();
                                       alert("Duplicates not allowed");
                                   }
                               }
                           });
             }
         }

         function deleteItem(e) {
             var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
             if (confirm('Are you sure you want to delete : ' + dataItem.name)) {
                 var grid = $("#grid").data("kendoGrid");
                 grid.dataSource.remove(dataItem);
                 grid.dataSource.sync();
                 grid.refresh();
             }
         }
    </script>
</body>
</html>
