<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PopUp</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black"  href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"  id="stylesheet"/>
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min.js")%>"></script>
</head>
<body>
     <div id="grid"></div>

<script type="text/x-kendo-template" id="template">
                <div class="toolbar">
                    <label for="products">Search Products by Category:</label><input id="products" class="k-textbox"/>
                    <input type="button" value="search" onClick="ProductChange()" class="k-button"/>
                </div>
 </script>

  <script type="text/x-kendo-template" id="AlertBoxDetail">
                <div id="details-container">
                    #= ProductName #
                </div>
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
				data-role="combobox"  required="required"/>	                                <!- dropdownlist , combobox-->
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
                var wnd, detailsTemplate;

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
                                }
                            },
                            batch: true,
                            pageSize: 5,
                            schema: {
                                model: {
                                    id: "ProductID",
                                    fields: {
                                        ProductID: { editable: false, nullable: true },
                                        UniqueCode: { editable: false, nullable: true },
                                        ProductName: { validation: { required: true} },
                                        UnitPrice: { type: "number", validation: { required: true, min: 1} },
                                        Discontinued: { type: "boolean" },
                                        UnitsInStock: { type: "number", validation: { min: 0, required: true} },
                                        Category: { type: "string", validation: { required: true} },
                                        CreatedDate: { type: 'date', validation: { required: true} },
                                        Duration: { type: "number", editable: true }
                                    }
                                }
                            }
                        });

                    $("#grid").kendoGrid({
                        dataSource: dataSource,
                        pageable: true,
                        resizable: true,
                        toolbar: [{
                            name: "my-create",
                            text: "Add new record"
                        }, { text: "", template: kendo.template($("#template").html())}],
                        columns: [
                            { field: "ProductName", title: "Product Name" },
                            { field: "UnitPrice", title: "Unit Price", format: "{0:c}", width: "100px" },
                            { field: "UnitsInStock", title: "Units In Stock", width: "150px" },
                            { field: "Discontinued", width: "100px" },
                            { field: "Category", title: "Category", filterable: { ui: GroupFilter}/*, editor: ColumnGroupFilter*/ },
                            { field: "CreatedDate", title: "Date", type: "date", format: "{0:MM/dd/yyyy}" /*,template: '#= kendo.toString(CreatedDate, "MM/dd/yyyy") #'*/ },
                            { field: "Duration", width: "100px" },
                        //{ command: ["edit", "destroy"], title: "Edit", width: "160px" },
                            {command: [{ text: 'Delete', click: deleteItem }, { text: 'edit', click: editItem}], title: 'Action' }
                            ],
                        //editable: "popup",
                        editable: {
                            mode: "popup",
                            template: kendo.template($("#popup_editor").html()),
                            window: {
                                title: "Edit Records(A demo by AG)"
                            }
                        },
                        dataBinding: function () {
                            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();

                        },
                        edit: function (e) {
                            //add a title
                            if (!e.model.ProductID) {
                                $(".k-window .k-window-title").text("Add new record");
                                $(".k-window .k-grid-update").html("<span class=\"k-icon k-update\"></span>Create");
                            }

                            //popup templates width
                            /*var editWindow = this.editable.element.data("kendoWindow");
                            editWindow.wrapper.css({ width: 600 });*/
                        },
                        save: onSave
                    });

                    $(".k-grid-my-create", grid.element).on("click", function (e) {
                        var grid = $("#grid").data("kendoGrid");
                        grid.dataSource.filter({});
                        grid.dataSource.sort({});
                        grid.addRow();
                    });

                    wnd = $("<div />").kendoWindow({
                        title: "Alert Box",
                        modal: true,
                        visible: false,
                        resizable: false,
                        width: 300
                    }).data("kendoWindow");

                    detailsTemplate = kendo.template($("#AlertBoxDetail").html());
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
                    , success: function (result) {
                    //$("#grid").data("kendoGrid").dataSource.data(JSON.parse(result)); //if return type is string
                    $("#grid").data("kendoGrid").dataSource.data(result);
                    }
                    });*/


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

               function deleteItem(e) {
                   var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                   if (confirm('Are you sure you want to delete : ' + dataItem.ProductName)) {
                       var grid = $("#grid").data("kendoGrid");
                       grid.dataSource.remove(dataItem);
                       grid.dataSource.sync();
                       grid.refresh();
                   }
               }
               

               function editItem(e) {
                   var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                       var grid = $("#grid").data("kendoGrid");
                       grid.dataSource.edit(dataItem);
                       grid.dataSource.sync();
                       grid.refresh();
                   }

              function onSave(e) {
                       if (e.model.ProductID != null) { }
                       else {
                           var currentProductName = e.model.ProductName;
                           /*$.ajax({
                               url: '<%=Url.Content("~/Grid/CheckDuplication")%>'
                               , type: "POST"
                               , data: { ProductName: currentProductName }
                               , success: function (result) {
                                   if (result.value == 'true') {
                                       e.preventDefault();
                                       wnd.content(detailsTemplate({ ProductName: "Product Name '" + currentProductName + "' already exist" }));
                                       wnd.center().open();
                                       return false;
                                   }
                               }
                           });*///ajax not working
                           
                           /*var xhReq = new XMLHttpRequest();
                           xhReq.open("POST", '<%=Url.Content("~/Grid/CheckDuplication")%>', false);
                           xhReq.send(null);
                           var AllData = JSON.parse(xhReq.responseText);
                           for (item in AllData) {
                               if (AllData[item].ProductName == currentProductName) {
                                   e.preventDefault();
                                   wnd.content(detailsTemplate({ ProductName: "Product Name '" + currentProductName + "' already exist" }));
                                   wnd.center().open();
                               }
                           }*/

                           var currentProductID = e.model.ProductID;
                           var data = this.dataSource.data();
                           for (item in data) {
                               if (data[item].ProductName == currentProductName &&
                                   data[item].ProductID != currentProductID) {
                                   e.preventDefault();
                                   //alert("Duplicates not allowed");
                                   //$("#spnDuplicate").val("Duplicates not allowed").change();
                                   $("#spnDuplicate").text("Duplicates not allowed")
                               }
                           }
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
   
.k-widget.k-tooltip-validation {
    background-color: #FFE4C4 !important;
}
/*.k-edit-form-container
{
    height: 600px;
}*/
            </style>   
</body>
</html>
