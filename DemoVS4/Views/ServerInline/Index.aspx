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
    <link rel="stylesheet" type="text/css" media="screen" href="<%= Url.Content("~/Content/CustomLoader.css")%>" />
    <script src="<%= Url.Content("~/Scripts/CustomLoader.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
</head>
<body>
    <div>
    <div id="grid" >
    </div>
    <div id="popup_name" class="popup_block">
        <div class="k-loading-image"></div>
    </div>
    <script type="text/x-kendo-template" id="template">
                <div class="toolbar">
                  <button type="button" id="btnAddNew"  class="k-button" onClick="AddNewWindowFunc()" ><i class="k-icon k-add"></i>Add New Record</button>    
                    <label for="products">Search :</label><input id="products" class="k-textbox"/>
                    <input type="button" value="search" onClick="ProductChange()" class="k-button"/>
                </div>           
    </script>

    <div id="AddNew">
    </div>

    <script>
        var xhReq = new XMLHttpRequest();
        xhReq.open("POST", '<%=Url.Content("~/ServerInline/GetJsonOutputForitems")%>', false);
        xhReq.send(null);
        var Globalvalue = JSON.parse(xhReq.responseText);
        var record = 0;
        var wnd, detailsTemplate;


        $(document).ready(function () {
            var crudServiceBaseUrl = '<%=Url.Content("~/ServerInline")%>',

                        dataSource = new kendo.data.DataSource({
                            type: "json",
                            serverPaging: true,
                            serverFiltering: true,
                            serverSorting: true,
                            pageSize: 10,
                            transport: {
                                read: {
                                    url: crudServiceBaseUrl + "/GetJsonOutputForGridDataSelect",
                                    dataType: "json"
                                }
                            },
                            //batch: true,
                            schema: { data: "File", total: "TotalCount",
                                model: {
                                    id: "Id",
                                    fields: {
                                        Id: { editable: false, nullable: true },
                                        url: { validation: { required: true, validationMessage: "Please enter Url"} },
                                        DateS: { validation: { required: true, validationMessage: "Please enter Date"} },
                                        isArabic: { type: "boolean" },
                                        Description: { validation: { required: true, validationMessage: "Please enter Description"} },
                                        Order: { type: "string", validation: { required: true} },
                                        ImageUrl: { type: "file" }
                                    }
                                }
                            }
                        });

            $("#grid").kendoGrid({
                dataSource: dataSource,
                pageable: {
                    refresh: true,
                    pageSizes: true
                },
                filterable: true,
                sortable: true,
                selectable: true,
                toolbar: [{ text: "", template: kendo.template($("#template").html())}],
                columns: [
                //{ title: "&nbsp;", template: "#= ++record #", width: 30 },
                            {field: "Id", title: "&nbsp;", width: 30 },
                            { field: "Description", title: "Description" },
                            { field: "url", title: "url" },
                            { field: "DateS", title: "Date String" },
                            { field: "isArabic", width: "100px" },
                            { field: "Order", title: "Order", filterable: { ui: GroupFilter }, editor: ColumnGroupFilter },
                            { field: "ImageUrl", title: "ImageFile", editor: ColumnGroupFileFilter },
                            { title: "image", template: "<img src='../../Content/images/News/#= ImageUrl#' width='50' height='50'/>" },
                            { command: ["edit"], title: "Edit" },
                            { command: [{ text: "Delete", click: removeSingleRow, title: "Delete", className: "div k-grid k-grid-Remove"}], title: 'Delete' },
                            ],
                editable: "inline",
                dataBinding: function () {
                    record = (this.dataSource.page() - 1) * this.dataSource.pageSize();

                },
                save: onSave
            });


            //wnd = $("details").kendoWindow({ or
            wnd = $("<div />").kendoWindow({
                title: "Message Box",
                modal: true,
                visible: false,
                resizable: false,
                width: 300
            }).data("kendoWindow");

            var window = $("#AddNew").kendoWindow({
                title: "Add News",
                modal: true, //fade popup
                //position: { top: 100, left: 100 },
                width: "650px",
                height: "380px",
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

            var title = '<i class="k-icon k-add"></i> Add News'; // add images to the title of custom window
            window.wrapper.find('.k-window-title').html(title) //or speficy in declaration like title: "Add New Product"

        });

        /*************Add new content window box****************************/

        function AddNewWindowFunc() {
            fadeinout();
            $.ajax({
                url: '<%= Url.Content("~/ServerInline/AddNews") %>',
                cache: false,
                success: function (html, textStatus, XMLHttpRequest) {
                    if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                        window.location = '<%=Url.Content("~/Admin/Account/Login") %>';
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

        function ColumnGroupFilter(container, options) {
            $('<input required="required" name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                dataTextField: "value",
                dataValueField: "value",
                dataSource: Globalvalue

            });
            $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>').appendTo(container);
        }


        function ColumnGroupFileFilter(container, options) {
            //var flag = true;
            $('<input type="file" name="' + options.field + '" id="' + options.field + '"/>')
            // $('<input type="file" name="' + options.field + '" id="' + options.field + '"/>')
                        .appendTo(container)
                        .kendoUpload({
                            async: {
                                saveUrl: '<%= Url.Content("~/ServerInline/UploadFile") %>'
                            },
                            //complete: onUploadComplete
                            upload: function (e) {
                                var grid = $("#grid").data("kendoGrid");
                                var item = grid.dataItem(this.element.closest("tr"));
                                e.data = { id: item.Id };
                            },
                            success: function (e) {
                                var grid = $("#grid").data("kendoGrid");
                                var ImageUrl = e.response.ImageUrl;
                                var item = grid.dataItem(this.element.closest("tr"));
                                item.ImageUrl = ImageUrl;
                                //$("input[type='file']").val = FileUploader;
                                //e.data = { FileUploader: item.FileUploader };
                            }
                        });

            //$('<span class="k-invalid-msg" data-for="' + options.field + '"></span>').appendTo(container);

        }

        function GroupFilter(e) {
            e.kendoDropDownList({
                dataSource: Globalvalue,
                dataTextField: "value",
                dataValueField: "value"
            });
        }

        function onUploadComplete(e) {
            alert("Complete");
        }

        function ProductChange() {
            //alert($("#products").val());
            var kgrid = $("#grid").data("kendoGrid");
            var orfilter = { logic: "or", filters: [] };
            var andfilter = { logic: "and", filters: [] };
            orfilter.filters.push({ field: "Description", operator: "contains", value: $("#products").val() },
                                              { field: "DateS", operator: "contains", value: $("#products").val() },
                                              { field: "url", operator: "contains", value: $("#products").val() });
            //andfilter.filters.push(orfilter);
            //orfilter = { logic: "or", filters: [] };
            kgrid.dataSource.filter(orfilter);
        }

        function onSave(e) {
            fadeinout();
            if (e.model.Id != null) {
                //alert(e.model.Id + "  : " + e.model.Description + "  : " + e.model.url + "  : " + e.model.DateS + "  : " + e.model.Order + "  : " + e.model.ImageUrl + "  : " + e.model.image);

                $.ajax({
                    type: "POST",
                    url: '<%= Url.Content("~/ServerInline/editRow") %>',
                    cache: false,
                    data: { 'Id': e.model.Id,
                        'Description': e.model.Description,
                        'url': e.model.url,
                        'DateS': e.model.DateS,
                        'Order': e.model.Order,
                        'ImageUrl': e.model.ImageUrl,
                        'isArabic': e.model.isArabic
                    },
                    success: function (result, textStatus, XMLHttpRequest) {
                        if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                            window.location = '<%=Url.Content("~/Account/Login") %>';
                            window.location.reload();
                        }
                        else {
                            if (result.Value) {
                                $("#grid").data("kendoGrid").dataSource.read();
                                fadeover();


                            }
                            else {
                                fadeover();
                                alert("Server Error!! " + result.Value);
                            }
                        }
                    },
                    error: function (msg) {
                        fadeover();
                        alert(msg);
                    }
                });
            }
        }
        function removeSingleRow(e) {
            var tr = $(e.target).closest("tr");
            //alert("a");
            var data = this.dataItem(tr);
            var grid = $("#grid").data("kendoGrid");
            // alert(data.ID);
            /*server coding here*/
            if (confirm("Are you sure you want to delete this record!")) {
                fadeinout();
                $.ajax({
                    type: "POST",
                    url: '<%= Url.Content("~/ServerInline/DeleteRecords") %>',
                    cache: false,
                    data: { 'id': data.Id },
                    success: function (result) {
                        if (result.Value) {
                            fadeover();
                            grid.dataSource.remove(data); //on success delete selected row from rendo grid(client side)
                        }
                        else {
                            fadeover();
                            alert("Server Error!!");
                        }
                    },
                    error: function (msg) {
                        fadeover();
                        alert("failed");
                    }
                });
                fadeover();
            }
        }

        function showDetails(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            wnd.content(detailsTemplate(dataItem));
            wnd.center().open();
        }
    </script>
    </div>
</body>
</html>
