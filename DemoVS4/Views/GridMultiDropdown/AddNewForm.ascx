<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DemoVS4.Core.Manager.DridDataObj>" %>


<% using (Ajax.BeginForm("AddNewForm", "GridMultiDropdown", new AjaxOptions { UpdateTargetId = "status", OnSuccess = "fadeover", HttpMethod = "Post" }, new { id = "AddNewFormId" }))
   {%>
<div id="AddNewValidator">

            <table>
            <tr>
            <td><label for="ProductNameform" >Product Name :</label></td>
            <td><%=Html.TextBoxFor(x => x.ProductName, new { Class = "k-textbox", placeholder = "Product Name", Required = "Required", ValidationMessage = "Please Enter Product name" })%>
            <%--<input type="text" id="ProductNameform" name="ProductNameform" class="k-textbox" placeholder="Product Name" required validationMessage="Please Enter Product name" />--%>
            </td>
            <td><span class="k-invalid-msg" data-for="ProductName"></span></td>
            </tr>
            
            <tr>
            <td><label for="UnitPriceform" >UnitPric Name:</label></td>
            <td>
            <%=Html.TextBoxFor(x => x.UnitPrice, new { min = "0", max = "10", value = "0", Required = "Required" })%>
            <%--<input id="UnitPrice" name="UnitPrice" type="text" min="1" max="10" value="1" required data-max-msg="Enter value between 1 and 10" />--%>
            <%--<span class="k-invalid-msg" data-for="UnitPrice"></span>--%>
            </td>
            <td><span class="k-invalid-msg" data-for="UnitPrice"></span></td>
            </tr>
            
            <tr>
            <td><label for="UnitsInStockform">UnitsInStock :</label></td>
            <td>
            <%=Html.TextBoxFor(x => x.UnitsInStock, new { min = "0", value = "0", Required = "Required" })%>
            <%--<input type="tel" id="tel" name="tel" class="k-textbox" pattern="\d{10}" placeholder="Please enter a ten digit phone number" required
                        validationMessage="Please enter a ten digit phone number"/>--%>
            </td>
            <td><span class="k-invalid-msg" data-for="UnitsInStock"></span></td>
            </tr>
            
            <tr>
            <td><label for="Discontinuedform">Discontinued :</label></td>
            <td>
            <%=Html.CheckBoxFor(x=>x.Discontinued)%>   <%--Html.CheckBoxFor() is wired, its send two value on server if true i.e "true,false" and if fasle send one value i.e "false" --%>
            <%--<input type="checkbox" name="Discontinued" />--%>
            <%--<input type="email" id="email" name="Email" class="k-textbox" placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid" />--%>
            </td>
            <td></td>
            </tr>

            <tr>
            <td><label for="Categoryform">Category :</label></td>
            <td>
            <%--<%=Html.TextBoxFor(x => x.Category)%>--%>
            <select name="Category" id="Category"></select>
            <%--<input type="email" id="email" name="Email" class="k-textbox" placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid" />--%>
            </td>
            <td><span class="k-invalid-msg" data-for="Category"></span></td>
            </tr>

            <tr>
            <td><label for="CreatedDateform">Date :</label></td>
            <td>
            <%=Html.TextBoxFor(x => x.CreatedDate, new { Required = "Required" })%>
            <%--<input type="email" id="email" name="Email" class="k-textbox" placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid" />--%>
            </td>
             <td><span class="k-invalid-msg" data-for="CreatedDate"></span></td>
            </tr>
            
            <tr>
            <td>
            <button class="k-button" type="button" name="Submit" id="Submit" >Add New Record</button>
			<%--<button class="k-button" type="button" name="cancel" id="cancel" onclick="clearRecords()">Clear</button>--%>
            </td>
            </tr>
            </table>
                 <div class="status" id="status">
                </div>
                
                </div>
                <%} %>

                
            <script>

                $(document).ready(function () {
                    
                    $("#UnitPrice").kendoNumericTextBox();
                    $("#UnitsInStock").kendoNumericTextBox();
                    $("#CreatedDate").kendoDatePicker({
                        format: "MM/dd/yyyy",
                        value: new Date()
                    });


                    $("#Category").kendoMultiSelect({
                        placeholder: "Select Category...",
                        dataSource: GlobalSearchFOOD
                    });

                    var multiSelect = $("#Category").data("kendoMultiSelect");

                    if (multiSelect.value() != "")
                        multiSelect.value($("#Category").val().split(", "));


                    var validator = $("#AddNewValidator").kendoValidator({      //validator for all pluss multi selct
                        rules: {
                            hasItems: function (input) {
                                if (input.is("[name=Category]")) {
                                    //Get the MultiSelect instance
                                    var ms = input.data("kendoMultiSelect");
                                    if (ms.value().length === 0) {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        },
                        messages: {
                            hasItems: "Please select at least one Category"
                        }
                    }).data("kendoValidator");

                    //or
                    //var validator = $("#AddNewValidator").kendoValidator().data("kendoValidator");  // simple validation
                    var status = $(".status");

                    $("#Submit").click(function () {

                        if (validator.validate()) {
                            status.text("Saving.....")
                                .removeClass("invalid")
                                .addClass("valid");
                            fadeinout();
                            $("#AddNewFormId").submit();

                        } else {
                            status.text("There is invalid data in the form.")
                                .removeClass("valid")
                                .addClass("invalid");
                        }
                    });
                });


                 function clearRecords() {
                     /**clear previous record **/
                    document.getElementById("UnitPrice").value = "";
                    document.getElementById("UnitsInStock").value = "";
                    document.getElementById("ProductName").value = "";
                    document.getElementById("Category").value = "";
                    $("#CreatedDate").data("kendoDatePicker").value(new Date());

                    $("#AddNew").data("kendoWindow").element.find("span.k-tooltip-validation").hide();
                    $(".status").text("").removeClass("invalid").addClass("valid");
                }
            </script>