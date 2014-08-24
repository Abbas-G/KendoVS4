<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inline</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black"  href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"  id="stylesheet"/>
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min2.js")%>"></script>
    <%--<script src="<%= Url.Content("~/Content/kendo/js/kendo.web.min.js")%>"></script>--%>
</head>
<body>
    <div id="grid"></div>

     <script>
         var record = 0;

         $(document).ready(function () {
             var crudServiceBaseUrl = '<%=Url.Content("~/Grid")%>',

                        dataSource = new kendo.data.DataSource({
                            transport: {
                                /*read: function (options) {
                                var webMethod = "/MyWebService.asmx/Read";
                                $.ajax({
                                type: "POST",
                                url: webMethod,
                                //data: displayParams,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {
                                options.success(result.d);
                                }
                                })
                                },*/
                                read: {
                                    //url: crudServiceBaseUrl + "/GetJsonOutputFromController",
                                    url: "/MyWebService.asmx/ReadPureJson",
                                    dataType: "json",
                                    type: "POST"
                                },
                                update: {
                                    url: "/MyWebService.asmx/Update",
                                    dataType: "json",
                                    type: "POST"
                                },
                                destroy: {
                                    url: "/MyWebService.asmx/Delete",
                                    dataType: "json",
                                    type: "POST"
                                },
                                create: {
                                    url: "/MyWebService.asmx/Create",
                                    dataType: "json",
                                    type: "POST"
                                },
                                parameterMap: function (options, operation) {
                                    if (operation !== "read" && options.models) {
                                        return { models: kendo.stringify(options.models) };
                                    }
                                    //check dis for client to servrer data flow http://www.telerik.com/forums/best-strategies-for-datetime-handling-in-datasource-and-grid
                                }
                            },
                            batch: true,
                            serverPaging: false,
                            pageSize: 5,
                            schema: {
                                model: {
                                    id: "ProductID",
                                    fields: {
                                        ProductID: { editable: false, nullable: true },
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
                        });

             $("#grid").kendoGrid({
                 dataSource: dataSource,
                 pageable: {
                     input: true,
                     numeric: false
                 },
                 //height: 430,
                 filterable: { extra: false },
                 sortable: true,
                 toolbar: [{
                     name: "my-create",
                     text: "Add new record"
                 }],
                 columns: [
                            { title: "&nbsp;", template: "#= ++record #", width: 30 },
                            { field: "ProductName", title: "Product Name" },
                            { field: "UnitPrice", title: "Unit Price", format: "{0:c}" },
                            { field: "UnitsInStock", title: "Units In Stock" },
                            { field: "Discontinued", width: "100px" },
                            { field: "Category", title: "Category" },
                            { field: "CreatedDate", title: "Date", type: "date", format: "{0:MM/dd/yyyy h:mm:ss tt}" },
                            { field: "Duration", width: "100px" },
                            { command: ["edit"/*, "destroy"*/], title: "Edit", width: "160px" },
                            { command: [{ text: 'Delete', click: deleteItem}], title: 'Actions' }
                            ],
                 editable: "inline",
                 dataBinding: function () {
                     record = (this.dataSource.page() - 1) * this.dataSource.pageSize();

                 },
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





         function createNew() {
             var grid = $("#grid").data("kendoGrid");
             grid.dataSource.filter({});
             grid.dataSource.sort({});
             //add record using Grid API
             grid.addRow();
         }

         function onSave(e) {
             if (e.model.ProductID != null) { }
             else {
                 /*var currentProductName = e.model.ProductName;
                 var currentProductID = e.model.ProductID;
                 var data = this.dataSource.data();
                 for (item in data) {
                 if (data[item].ProductName == currentProductName &&
                 data[item].ProductID != currentProductID) {
                 e.preventDefault();
                 alert("Duplicates not allowed");
                 }
                 }*/
                 //cleint side
                 var currentProductName = e.model.ProductName;
                 var currentProductID = e.model.ProductID;
                 var data = this.dataSource.data();
                 for (item in data) {
                     if (data[item].ProductName == currentProductName &&
                                   data[item].ProductID != currentProductID) {
                         e.preventDefault();
                         alert("Duplicates not allowed");
                         //$("#spnDuplicate").val("Duplicates not allowed").change();
                         //$("#spnDuplicate").text("Duplicates not allowed")
                     }
                 }
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
            
  <!-- this style is add to solve "validation msg hidding in the bottom of grid" by changing validation style-->          
  <style>
#grid .k-tooltip-validation {
    margin-top: 0 !important;
    display: block;
    position: static;
    padding: 0;
}
  
#grid .k-callout {
    display: none;
}
            </style>
</body>
</html>
