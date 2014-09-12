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
    
    <script type="text/x-kendo-template" id="template">
                <div class="toolbar">
                    <label for="products">Search Products by Category:</label><input id="products" class="k-textbox"/>
                    <input type="button" value="search" onClick="ProductChange()" class="k-button"/>
                </div>     
                <button type="button" id="btnExport"  class="k-button" onClick="ExportToCSV()" >Export to csv!</button>
 </script>

    <script id="popup_editor" type="text/x-kendo-template">
			<p>Custom editor template</p>
            <p><span id="spnDuplicate" style="color:red"></span></p>

			<div class="k-edit-label">
				<label>Product Name</label>
			</div>
			<input type="text" class="k-input k-textbox" id="ProductName" name="ProductName" data-bind="value:ProductName" required/>
			<span class="k-invalid-msg" data-for="ProductName"></span>

            # if( UniqueCode!=null) {#
            <div class="k-edit-label"><label>Unique Code : </label></div>
            #: UniqueCode#
            <br/>
            # }#
						
			<div class="k-edit-label">
				<label>UnitPrice</label>
			</div>
			<input type="text" name="UnitPrice" data-type="number" data-bind="value:UnitPrice" data-role="numerictextbox" max="10" min="1" value="1" required  />
			<span class="k-invalid-msg" data-for="UnitPrice"></span>
			
			<div class="k-edit-label">
				<label>Discontinued</label>
			</div>
			<input type="checkbox" 
				name="Discontinued" 
				data-type="boolean" 
				data-bind="value:Discontinued"/>
				
				<br/>
				
			<div class="k-edit-label">
				<label>UnitsInStock</label>
			</div>
			<input type="text" name="UnitsInStock" data-type="number" data-bind="value:UnitsInStock" data-role="numerictextbox" max="10" min="1" required  />
			<span class="k-invalid-msg" data-for="UnitsInStock"></span>
			<br/>
			<div class="k-edit-label">
				<label>Category</label>
			</div>
			<input name="Category" 
				data-bind="value:Category" 
				data-value-field="Category" 
				data-text-field="Category" 
				data-source="GlobalSearchFOOD" 
				data-role="dropdownlist" data-option-label="--Select--" required="required"/>	                                <!- dropdownlist , combobox dont have data-option-label-->
		    <span class="k-invalid-msg" data-for="Category"></span>
				<br/>

            <div class="k-edit-label">
				<label>Created Date</label>
			</div>
                <input type="text" name="CreatedDate" type="date" data-bind="value:CreatedDate" data-role="datepicker" required  />  <!- datetimepicker , datepicker (type=date)-->
			<span class="k-invalid-msg" data-for="CreatedDate"></span> 
            <br/>

            <div class="k-edit-label">
				<label>Duration</label>
			</div>
            <input type="text" name="Duration" data-type="number" data-bind="value:Duration" data-role="numerictextbox"  required  />
			<span class="k-invalid-msg" data-for="Duration"></span>

            
            <br/><br/><br/>
            
		</script>
     <script>
         var xhReq = new XMLHttpRequest();
         xhReq.open("POST", '<%=Url.Content("~/Grid/GetJsonOutputForFoodUniqueCategory")%>', false);
         xhReq.send(null);
         var GlobalSearchFOOD = JSON.parse(xhReq.responseText);
         var record = 0;

         $(document).ready(function () {
             var crudServiceBaseUrl = '<%=Url.Content("~/Grid")%>',

                        dataSource = new kendo.data.DataSource({
                            transport: {
                                read: {
                                    url: crudServiceBaseUrl + "/GetJsonOutputForGridDataSelect",
                                    dataType: "json",
                                    type: "POST"
                                },
                                update: {
                                    url: crudServiceBaseUrl + "/GetJsonOutputForGridDataUpdatePopup",
                                    dataType: "json",
                                    type: "POST"
                                },
                                destroy: {
                                    url: crudServiceBaseUrl + "/GetJsonOutputForGridDataDeletePopup",
                                    dataType: "json",
                                    type: "POST"
                                },
                                create: {
                                    url: crudServiceBaseUrl + "/GetJsonOutputForGridDataCreatePopup",
                                    dataType: "json",
                                    type: "POST"
                                },
                                parameterMap: function (options, operation) {
                                    if (operation !== "read" && options.models) {
                                        return { models: kendo.stringify(options.models) };
                                    }
                                    //check dis for client to servrer data flow http://www.telerik.com/forums/best-strategies-for-datetime-handling-in-datasource-and-grid
                                }
                            }/*,
                            requestEnd: function (e) {
                                var response = e.response;
                                var type = e.type;
                                //alert(type); // displays "read"
                                if (type=="update")
                                    e.sender.read();
                            }*/
                            ,
                            batch: true,
                            serverPaging: false,
                            pageSize: 5,
                            schema: {
                                model: {
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
                            }/*, group: {
                                field: "Category", aggregates: [
                                        { field: "Category", aggregate: "count" }
                                     ]
                            }

                            , aggregate: [{ field: "UnitPrice", aggregate: "sum" }]*/
                            //use footerTemplate keyword in respected columns
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
                     // My own version of "Add new record" button, with name **popup**
                     name: "my-create",
                     text: "Add new record",
                     iconClass: "k-icon k-add"
                 }, { text: "", template: kendo.template($("#template").html())}],
                 columns: [
                            { title: "&nbsp;", template: "#= ++record #", width: 30 },
                            { field: "ProductName", title: "Product Name" },
                            { field: "UniqueCode", title: "Unique Code" },
                            { field: "UnitPrice", title: "Unit Price"/*, footerTemplate: "Total: #=sum#"*/, format: "{0:c}" },
                            { field: "UnitsInStock", title: "Units In Stock" },
                            { field: "Discontinued", width: "100px" },
                            { field: "Category", title: "Category", filterable: { ui: GroupFilter }, editor: ColumnGroupFilter },
                            { field: "CreatedDate", title: "Date", type: "date", format: "{0:MM/dd/yyyy h:mm:ss tt}" },
                            { field: "Duration", width: "100px" },
                            { command: ["edit"/*, "destroy"*/], title: "Edit", width: "160px" },
                            { command: [{ text: 'Delete', click: deleteItem}], title: 'Actions' }
                            ],
                 editable: "inline",
                 dataBinding: function () {
                     record = (this.dataSource.page() - 1) * this.dataSource.pageSize();

                 },
                 dataBound: function (e) {
                     var grid = $("#grid").data("kendoGrid");
                     var gridData = grid.dataSource.view();
                     for (var i = 0; i < gridData.length; i++) {
                         var currentUid = gridData[i].uid;
                         if (gridData[i].Category != "Men") {
                             var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                             //var editButton = $(currenRow).find(".k-grid-edit");
                             //editButton.hide();
                             var deleteButton = $(currenRow).find(".k-grid-Delete"); //text name define at command i.e Delete
                             deleteButton.hide();
                         }
                     }
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

                     //to hide delete button on cancel
                     $(".k-grid-cancel").on("click", function () {
                         setTimeout(function () {
                             $("#grid").data("kendoGrid").trigger("dataBound");
                         });
                     });
                 },
                 save: onSave
             });

             $(".k-grid-my-create", grid.element).on("click", function (e) {
                 var grid = $("#grid").data("kendoGrid");
                 grid.dataSource.filter({});
                 grid.dataSource.sort({});

                 // Temporarily set editable to "popup"
                 var popupWithOption = {
                     mode: "popup",
                     template: kendo.template($("#popup_editor").html()),
                     window: {
                         title: "Edit Records(A demo by AG)"
                     }
                 };
                 grid.options.editable = popupWithOption;
                 // Insert row
                 grid.addRow();
                 // Revert editable to inline
                 grid.options.editable = "inline";
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

         function GroupFilter(e) {
             e.kendoDropDownList({
                 dataSource: GlobalSearchFOOD,
                 dataTextField: "Category",
                 dataValueField: "Category"
             });
         }

         function ProductChange() {
             /*$.ajax({
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

             /* client side*/

             //$("#grid").data("kendoGrid").dataSource.filter({ field: "ProductName", operator: "contains", value: $("#products").val()}); //one parameter

             var kgrid = $("#grid").data("kendoGrid");
             var orfilter = { logic: "or", filters: [] };
             var andfilter = { logic: "and", filters: [] };
             orfilter.filters.push({ field: "ProductName", operator: "contains", value: $("#products").val() },
                                              { field: "Category", operator: "contains", value: $("#products").val() });
             //andfilter.filters.push(orfilter);
             //orfilter = { logic: "or", filters: [] };
             kgrid.dataSource.filter(orfilter);
         }

         function createNew() {
             var grid = $("#grid").data("kendoGrid");
             grid.dataSource.filter({});
             grid.dataSource.sort({});
             //add record using Grid API
             grid.addRow();
         }

         function deleteItem(e) {
             var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
             if (confirm('Are you sure you want to delete : ' + dataItem.ProductName)) {
                 var grid = $("#grid").data("kendoGrid");
                 grid.dataSource.remove(dataItem);
                 grid.dataSource.sync();
                 grid.refresh();
             }
         }

         function onSave(e) {
             if (e.model) { //e.model value would be null on popup add in this mix example

                 if (e.model.ProductID != null) { }
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
                           }); //ajax now working using async:false*/

                     /*//cleint side
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
                     }*/
                 }
             }
         }
         function ExportToCSV() {
             var dataSource = $("#grid").data("kendoGrid").dataSource;
             var filteredDataSource = new kendo.data.DataSource({
                 data: dataSource.data(),
                 filter: dataSource.filter()
             });

             filteredDataSource.read();
             var data = filteredDataSource.view();

             var result = "data:application/vnd.ms-excel,";

             result += "<table><tr><th>ProductID</th><th>ProductName</th><th>UnitPrice</th><th>Discontinued</th><th>UnitsInStock</th><th>Category</th></tr>";

             for (var i = 0; i < data.length; i++) {
                 result += "<tr>";

                 result += "<td>";
                 result += data[i].ProductID;
                 result += "</td>";

                 result += "<td>";
                 result += data[i].ProductName;
                 result += "</td>";

                 /*result += "<td>";
                 result += kendo.format("{0:MM/dd/yyyy}", data[i].OrderDate);
                 result += "</td>";*/

                 result += "<td>";
                 result += data[i].UnitPrice;
                 result += "</td>";

                 result += "<td>";
                 result += data[i].Discontinued;
                 result += "</td>";

                 result += "<td>";
                 result += data[i].UnitsInStock;
                 result += "</td>";

                 result += "<td>";
                 result += data[i].Category;
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
