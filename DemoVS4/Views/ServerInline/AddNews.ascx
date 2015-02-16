<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DemoVS4.Controllers.ItemsObj>" %>
<% using (Ajax.BeginForm("AddNews", "News", new AjaxOptions { UpdateTargetId = "status", OnSuccess = "fadeover", HttpMethod = "Post" }, new { id = "AddNewFormId" }))
   {%>
   <%--<% using (Html.BeginForm("AddNews", "News",
                                FormMethod.Post, new { enctype = "multipart/form-data", id = "AddNewFormId" }))
       { %>--%>
<div id="AddNewValidator">
    <p>
        <span id="spnDuplicate" style="color: red"></span>
    </p>
    <table cellspacing="15">
        <tr>
            <td>
                <label for="Descriptionform">
                    Description :</label>
            </td>
            <td>
                <%=Html.TextBoxFor(x => x.Description, new { Class = "k-textbox", placeholder = "Description", Required = "Required", ValidationMessage = "Please Enter Description", Style = "width: 300px;" })%>
            </td>
            <td>
                <span class="k-invalid-msg" data-for="Description"></span>
            </td>
        </tr>
        <tr><td colspan="3">&nbsp;</td> </tr>

         <tr>
            <td>
                <label for="urlform">
                    URL :</label>
            </td>
            <td>
                <%=Html.TextBoxFor(x => x.url, new { Class = "k-textbox", placeholder = "url", Required = "Required", ValidationMessage = "Please Enter url", Style = "width: 300px;" })%>
            </td>
            <td>
                <span class="k-invalid-msg" data-for="url"></span>
            </td>
        </tr>
        <tr><td colspan="3">&nbsp;</td> </tr>
        <tr>
            <td>
                <label for="DateSform">
                    Date :</label>
            </td>
            <td>
                <%=Html.TextBoxFor(x => x.DateS, new { Class = "k-textbox", placeholder = "Date", Required = "Required", ValidationMessage = "Please Enter Date", Style = "width: 300px;" })%>
            </td>
            <td>
                <span class="k-invalid-msg" data-for="DateS"></span>
            </td>
        </tr>
        <tr><td colspan="3">&nbsp;</td> </tr>
        <tr>
            <td>
                <label for="ReOrderform">
                    Order :</label>
            </td>
            <td>
                <%=Html.TextBoxFor(x => x.Order, new { Class = "k-textbox", placeholder = "ReOrder", Required = "Required", ValidationMessage = "Please Enter Order" })%>
            </td>
            <td>
                <span class="k-invalid-msg" data-for="Order"></span>
            </td>
        </tr>
        <tr><td colspan="3">&nbsp;</td> </tr>
        <tr>
            <td>
                <label for="ImageUrlform">
                    Image File :</label>
            </td>
            <td>
                <input type="file" name="ImageUrl" id="ImageUrl" style="color: rgb(150, 150, 150);"  />
                <%=Html.Hidden("hdnfile") %>
            </td>
            <td>
                <span id="lblValResume" ></span>
            </td>
        </tr>
        <tr><td colspan="3">&nbsp;</td> </tr>
        <tr>
            <td>
                <label for="Discontinuedform">
                    IsActive :</label>
            </td>
            <td>
                <%=Html.CheckBoxFor(x => x.isArabic)%>
                <%--Html.CheckBoxFor() is wired, its send two value on server if true i.e "true,false" and if fasle send one value i.e "false" --%>
            </td>
            <td>
            </td>
        </tr>
        <tr><td colspan="3">&nbsp;</td> </tr>
        <tr>
            <td colspan="3">
                <button class="k-button" type="button" name="Submit" id="Submit">
                    Add News</button>
                <%--<button class="k-button" type="button" name="cancel" id="cancel" onclick="clearRecords()">Clear</button>--%>
            </td>
        </tr>
    </table>
    <div class="status" id="status">
    </div>
</div>
<%} %>
<script>
    var xhReq1 = new XMLHttpRequest();
    xhReq1.open("POST", '<%=Url.Content("~/ServerInline/GetJsonOutputForitems")%>', false);
    xhReq1.send(null);
    var Globalvalue1 = JSON.parse(xhReq1.responseText);
    $("#Order").kendoDropDownList({
        dataTextField: "value",
        dataValueField: "value",
        dataSource: Globalvalue1
    });

    $("#ImageUrl").kendoUpload({
        async: {
            saveUrl: '<%= Url.Content("~/ServerInline/UploadFile2") %>'
        },
        select: onSelect,
        multiple: false,
        upload: function (e) {
            // e.data = { id: 1 };
        },
        success: function (e) {
            var ImageUrl = e.response.ImageUrl;
            //alert(ImageUrl);
            document.getElementById("hdnfile").value = ImageUrl;
            e.data = { ImageUrl: ImageUrl };
            //alert(e.data.FileUploader);

        }
    });

    $(document).ready(function () {
        var validator = $("#AddNewValidator").kendoValidator().data("kendoValidator");  // simple validation
        var status = $(".status");

        $("#Submit").click(function () {

            if (validator.validate()) {
                //                var val = $("#Imagefile").val();
                //                var fi = document.getElementById('Imagefile');
                //                var MaxSize = 1;
                //                var sizeInBytes = MaxSize * 1024 * 1024;
                //                var isValid = true;
                //                if (val == "") {
                //                    $('#Imagefile').css('border', '1px solid red');
                //                    isValid = false;
                //                }
                //                else {
                //                    if (val.length > 0 && val.substring(val.lastIndexOf('.') + 1).toLowerCase() != "png" && val.substring(val.lastIndexOf('.') + 1).toLowerCase() != "jpg") {
                //                        $("#lblValResume").html("");
                //                        $("#lblValResume").append("<label>png and jpg file is allowed \n</label");
                //                        $('#Imagefile').css('border', '1px solid red');
                //                        isValid = false;
                //                    }
                //                    else {
                //                        var appname = window.navigator.appName;
                //                        //alert(appname);
                //                        if (appname.indexOf("Internet Explorer") != -1) {
                //                            $("#lblValResume").html("");
                //                            $('#Imagefile').css('border', '');
                //                        }
                //                        else {
                //                            if (fi.files[0].size > sizeInBytes) {
                //                                //alert("more size");
                //                                $("#lblValResume").html("");
                //                                $("#lblValResume").append("<label>file should not be more than 1 mb. \n</label");
                //                                $('#Imagefile').css('border', '1px solid red');
                //                                isValid = false;
                //                            }
                //                            else {
                //                                $("#lblValResume").html("");
                //                                $('#Imagefile').css('border', '');
                //                            }
                //                        }
                //                    }
                //                }
                //                if (isValid == true) {
                //                    status.text("Saving.....")
                //                                .removeClass("invalid")
                //                                .addClass("valid");
                //                    $("#AddNewFormId").submit();
                //                }
                //                fadeover();

                status.text("Saving.....")
                                .removeClass("invalid")
                                .addClass("valid");
                $("#AddNewFormId").submit();


            } else {
                status.text("There is invalid data in the form.")
                                .removeClass("valid")
                                .addClass("invalid");
                fadeover();
            }
        });
    });


    function onSelect(e) {
        // alert(e.files.length);
        if (e.files.length > 1) {
            alert("Please select max 1 files.");
            e.preventDefault();
        }
        $.each(e.files, function (index, value) {
            //alert(value.extension);
            if (value.extension != ".jpg") {
                e.preventDefault();
                alert("Please upload jpg image files");
            }
        });
    }

    function clearRecords() {

    }
</script>
