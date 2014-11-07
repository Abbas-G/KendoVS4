<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inline</title>
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" title="custom_black" href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_black.css")%>"
        id="stylesheet" />
    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.all.min2.js")%>"></script>
    <%--<script src="<%= Url.Content("~/Content/kendo/js/kendo.web.min.js")%>"></script>--%>
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="<%= Url.Content("~/Content/CustomLoader.css")%>" />
    <script src="<%= Url.Content("~/Scripts/CustomLoader.js") %>" type="text/javascript"></script>
</head>
<body>
    <div id="grid">
    </div>
    <div id="AddNew">
        
    </div>
    <div id="popup_name" class="popup_block">
        <div class="k-loading-image">
        </div>
    </div>
    <script type="text/x-kendo-template" id="template">
                <div class="toolbar">
                    <label for="products">Search Products by Category:</label><input id="products" class="k-textbox"/>
                    <input type="button" value="search" onClick="ProductChange()" class="k-button"/>
                </div>     
                <button type="button" id="btnAddNew"  class="k-button" onClick="AddNewWindowFunc()" >Add New</button>
                <button type="button" id="btnExport"  class="k-button" onClick="ExportToCSV()" >Export to csv!</button>
    </script>
    <script>
        var xhReq = new XMLHttpRequest();
        xhReq.open("POST", '<%=Url.Content("~/GridMultiDropdown/GetJsonOutputForFoodUniqueCategory")%>', false);
        xhReq.send(null);
        var GlobalSearchFOOD = JSON.parse(xhReq.responseText);
        var record = 0;
        var AddWin;

        $(document).ready(function () {


            var crudServiceBaseUrl = '<%=Url.Content("~/GridMultiDropdown")%>',

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
                                    e.sender.read();  //or $("#grid").data("kendoGrid").dataSource.read(); 
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
                                        Category: { validation: { required: true} }, // type: "string" is remove as we need to use array for multiselect
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
                toolbar: [/*{
                     name: "my-create",
                     text: "Add new record"
                 },*/{text: "", template: kendo.template($("#template").html())}],
                columns: [
                            { title: "&nbsp;", template: "#= ++record #", width: 30 },
                            { field: "ProductName", title: "Product Name" },
                            { field: "UniqueCode", title: "Unique Code" },
                            { field: "UnitPrice", title: "Unit Price"/*, footerTemplate: "Total: #=sum#"*/, format: "{0:c}" },
                            { field: "UnitsInStock", title: "Units In Stock" },
                            { field: "Discontinued", width: "100px" },
                            { field: "Category", title: "Category", filterable: { ui: GroupFilter }, editor: CategoryEditor, template: "#= Category.join(', ') #" },
                            { field: "CreatedDate", title: "Date", type: "date", format: "{0:MM/dd/yyyy}" /* format: "{0:MM/dd/yyyy h:mm:ss tt}"*/ },
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

            /***add new records window***/
            /*var window = $("#AddNew");

            if (!window.data("kendoWindow")) {
            window.kendoWindow({
            width: "550px",
            modal: true,
            actions: ["Custom", "Minimize", "Maximize", "Close"],
            title: "Add New Record",
            close: function () {
            }
            });
            }
            window.data("kendoWindow").wrapper.find(".k-i-custom").click(function (e) {
            //alert("Custom action button clicked on 1st window (hello world)");
            e.preventDefault();
            });
            $('#AddNew').parent().css("top", "20%");
            $('#AddNew').parent().css("left", "10%");
            $("#AddNew").data("kendoWindow").close();*/

            var window= $("#AddNew").kendoWindow({
                title: "Add New Product",
                modal: true, //fade popup
                //position: { top: 100, left: 100 },
                width: "650px",
                height: "400px",
                visible: false,
                open: function (e) {
                    //$("body").addClass("ob-no-scroll");
                },
                close: function (e) {
                    //$("body").removeClass("ob-no-scroll");
                    $("#grid").data("kendoGrid").dataSource.read(); 
                }
            }).data("kendoWindow");

            $('#AddNew').parent().css("top", "20%");
            $('#AddNew').parent().css("left", "20%");

            var title = '<i class="k-icon k-add"></i> Add New Product'; // add images to the title of custom window
            window.wrapper.find('.k-window-title').html(title) //or speficy in declaration like title: "Add New Product"

            /***end**/



        });

        function CategoryEditor(container, options) {
            $("<select required='required'  multiple='multiple' " +
      "data-bind='value : Category'/>").appendTo(container).kendoMultiSelect({
          dataSource: GlobalSearchFOOD//,
          //dataTextField: "Category",
          //dataValueField: "Category"
      });
            $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>').appendTo(container);
        }


        function ColumnGroupFilter(container, options) {
            $('<input required="required" name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                //dataTextField: "Category",
                //dataValueField: "Category",
                dataSource: GlobalSearchFOOD

            });
            $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>').appendTo(container);
        }

        function GroupFilter(e) {
            e.kendoDropDownList({
                dataSource: GlobalSearchFOOD//,
                //dataTextField: "Category",
                //dataValueField: "Category"
            });
        }



        /*************Add new content window box****************************/     

        function AddNewWindowFunc() {
            fadeinout();
            $.ajax({
                url: '<%= Url.Content("~/GridMultiDropdown/AddNewForm") %>',
                cache: false,
                success: function (html, textStatus, XMLHttpRequest) {
                    if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                        window.location = '<%=Url.Content("~/Account/LogIn") %>';
                        window.location.reload();
                    }
                    else {
                        $('#AddNew').html(html);
                        fadeover();
                        $("#AddNew").data("kendoWindow").open();
                    }
                }
            });
            
        }

       

        /***************************************************************************************/


        function ProductChange() {
            /* $.ajax({
            url: "/GridMultiDropdown/SearchFOODbyCategory"
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
            orfilter.filters.push(          
                                              { field: "ProductName", operator: "contains", value: $("#products").val() }//,
            // { field: "Category", operator: "contains", value: $("#products").val() } //category would not be use bcoz its type=string being remove  as we need to use array for multiselect
                                  );
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

        function onSave(e) {
            if (e.model.ProductID != null) { }
            else {
                var currentProductName = e.model.ProductName;
                $.ajax({
                    url: '<%=Url.Content("~/GridMultiDropdown/CheckDuplication")%>'
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
               .valid {
                    color: green;
                }

                .invalid {
                    color: red;
                }
        .ob-no-scroll
        {
            overflow: hidden;
        }
        #grid .k-tooltip-validation
        {
            margin-top: 0 !important;
            display: block;
            position: static;
            padding: 0;
        }
        
        #grid .k-callout
        {
            display: none;
        }
    </style>
</body>
</html>
