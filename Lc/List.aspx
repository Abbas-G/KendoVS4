<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="content-box-header">
        <ul class="content-box-tabs">
            <li><span ><%=Html.ActionLink("Detail","Index","Home") %></span></li>
            <li><span class="current"><%=Html.ActionLink("List", "List", "Home")%></span></li>
        </ul>
    </div>
 
   
  <div class="content-box-content">
    <div id="grid"></div>
    </div>


    <script type="text/x-kendo-template" id="template">
                <div class="toolbar">
                    <label for="Routes">Search:</label><input id="Routes" class="k-textbox" onChange="RouteChange()"/>
                    <input type="button" value="Search" onClick="RouteChange()" class="k-button"/>
                    <button type="button" id="btnExport"  class="k-button" onClick="ExportToCSV()" >Export to csv!</button>
                </div>     
                
 </script>
     <script>

         var record = 0;

         $(document).ready(function () {
             var crudServiceBaseUrl = '<%=Url.Content("~/Home")%>',

                        dataSource = new kendo.data.DataSource({
                            transport: {
                                read: {
                                    url: crudServiceBaseUrl + "/ListDataSelect",
                                    dataType: "json",
                                    type: "POST"
                                },
                                update: {
                                    url: crudServiceBaseUrl + "/ListDataUpdate",
                                    dataType: "json",
                                    type: "POST"
                                },
                                destroy: {
                                    url: crudServiceBaseUrl + "/ListDataDelete",
                                    dataType: "json",
                                    type: "POST"
                                },
                                create: {
                                    url: crudServiceBaseUrl + "/ListDataCreate",
                                    dataType: "json",
                                    type: "POST"
                                },
                                parameterMap: function (options, operation) {
                                    if (operation !== "read" && options.models) {
                                        return { models: kendo.stringify(options.models) };
                                    }
                                }
                            },
                            batch: true,
                            serverPaging: false,
                            pageSize: 5,
                            schema: {
                                model: {
                                    id: "RouteID",
                                    fields: {
                                        RouteID: { editable: false, nullable: true },
                                        RouteName: { validation: { required: true, validationMessage: "Please enter Route Name"} },
                                        RouteBrief: { validation: { required: true, validationMessage: "Please enter Route Brief"} },
                                        CreatedDate: { type: 'date',  editable: false, nullable: true  },
                                        ModifiedDate: { type: 'date',  editable: false, nullable: true },
                                        IsActive: { type: "boolean" }
                                    }
                                }
                            }
                        });

             $("#grid").kendoGrid({
                 dataSource: dataSource,
                 pageable: {
                 input: true,
                 numeric: true
                 },
                 pageable: {
                     refresh: true,
                     pageSizes: true
                 },
                 //height: 430,
                 filterable: { extra: false },
                 sortable: true,
                 toolbar: [{
                     name: "my-create",
                     text: "Add new record"
                 }, { text: "", template: kendo.template($("#template").html())}],
                 columns: [
                            { title: "&nbsp;", template: "#= ++record #", width: 30 },
                            { field: "RouteName", title: "Route Name" },
                            { field: "RouteBrief", title: "Route Brief", editor: textEditorInitialize },
                            { field: "IsActive", title: "IsActive" },
                            { field: "CreatedDate", title: "Created Date" , type: "date",  format: "{0:MM/dd/yyyy h:mm:ss tt}"},
                            { field: "ModifiedDate", title: "Modified Date", type: "date", format: "{0:MM/dd/yyyy h:mm:ss tt}" },       
                            { command: ["edit", "destroy"], title: "&nbsp;", width: "172px"}],
                 editable: "inline",
                 dataBinding: function () {
                     record = (this.dataSource.page() - 1) * this.dataSource.pageSize();

                 },
                 edit: function (e) {
                     var title = $(e.container).parent().find(".k-window-title");
                     var update = $(e.container).parent().find(".k-grid-update");
                     var cancel = $(e.container).parent().find(".k-grid-cancel");

                     if (!e.model.RouteID) {
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



         function ColumnGroupFilter(container, options) {
             $('<input required="required" name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                 dataTextField: "Category",
                 dataValueField: "Category",
                 dataSource: GlobalSearchFOOD

             });
             $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>').appendTo(container);
         }


         function RouteChange() {
             /* $.ajax({
             url: "/Grid/SearchFOODbyCategory"
             , type: "POST"
             , data: { searchString: $("#products").val() }
             , async: false
             , success: function (result) {
             //alert(result);
             // $("#grid").data("kendoGrid").dataSource.data(JSON.parse(result)); //if return type is string
             var grid = $("#grid").data("kendoGrid");
             grid.dataSource.filter({});
             grid.dataSource.sort({});
             grid.dataSource.data(result);
             }
             });*/
             //working

             // client side

             //$("#grid").data("kendoGrid").dataSource.filter({ field: "ProductName", operator: "contains", value: $("#products").val()}); //one parameter

             var kgrid = $("#grid").data("kendoGrid");
             var orfilter = { logic: "or", filters: [] };
             var andfilter = { logic: "and", filters: [] };
             orfilter.filters.push({ field: "RouteName", operator: "contains", value: $("#Routes").val() },
                                              { field: "RouteBrief", operator: "contains", value: $("#Routes").val() });
             //andfilter.filters.push(orfilter);
             //orfilter = { logic: "or", filters: [] };
             kgrid.dataSource.filter(orfilter);
         }

         function onSave(e) {
             if (e.model.RouteID != null) { }
             else {
                /* var currentProductName = e.model.ProductName;
                 $.ajax({
                     url: '<%=Url.Content("~/Grid/CheckDuplication")%>'
                               , type: "POST"
                               , data: { ProductName: currentProductName }
                               , async: false
                               , success: function (result) {
                                   if (result.value == 'true') {
                                       e.preventDefault();
                                       alert("Duplicates not allowed");
                                   }
                               }
                 });*/


                 //cleint side
                 e.model.RouteID = 0;
                 e.model.CreatedDate = "09/12/2014 3:05:54 PM";
                 e.model.ModifiedDate = "09/12/2014 3:05:54 PM";
                 var currentProductName = e.model.ProductName;
                 var currentProductID = e.model.ProductID;
                 var data = this.dataSource.data();
                 for (item in data) {
                 if (data[item].ProductName == currentProductName &&
                 data[item].ProductID != currentProductID) {
                 e.preventDefault();
                 alert("Duplicates not allowed");
                 }
                 }
             }
 }


         var textEditorInitialize = function (container, options) {
             $('<textarea name="' + options.field + '" style="width: ' + container.width() + 'px;height:' + container.height() + 'px" />').appendTo(container);
         };

         function ExportToCSV() {
             var dataSource = $("#grid").data("kendoGrid").dataSource;
             var filteredDataSource = new kendo.data.DataSource({
                 data: dataSource.data(),
                 filter: dataSource.filter()
             });

             filteredDataSource.read();
             var data = filteredDataSource.view();

             var result = "data:application/vnd.ms-excel,";

             result += "<table><tr><th>RouteID</th><th>RouteName</th><th>RouteBrief</th><th>IsActive</th><th>CreatedDate</th><th>ModifiedDate</th></tr>";

             for (var i = 0; i < data.length; i++) {
                 result += "<tr>";

                 result += "<td>";
                 result += data[i].RouteID;
                 result += "</td>";

                 result += "<td>";
                 result += data[i].RouteName;
                 result += "</td>";

                 result += "<td>";
                 result += data[i].RouteBrief;
                 result += "</td>";

                 result += "<td>";
                 result += data[i].IsActive;
                 result += "</td>";

                 result += "<td>";
                 result += kendo.format("{0:MM/dd/yyyy}",data[i].CreatedDate);
                 result += "</td>";

                 result += "<td>";
                 result += kendo.format("{0:MM/dd/yyyy}", data[i].ModifiedDate);
                 result += "</td>";

                

                 result += "</tr>";
             }

             result += "</table>";
             if (window.navigator.msSaveBlob) {
                 window.navigator.msSaveBlob(new Blob([result]), 'export.csv');
             } else {
                 window.open(result);
             }


             e.preventDefault();
         }
            
            </script>

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
.k-grid-toolbar
{
    height:50px !important;
    }
            </style>
</asp:Content>
