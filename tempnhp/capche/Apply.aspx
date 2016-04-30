<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tia Solutions - Applying for <%=ViewData["postion"] %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptStyle" runat="server">


    <link href="<%= Url.Content("~/Content/kendo/style/examples-offline.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/style/kendo.common.min.css")%>" rel="stylesheet" />
    <link href="<%= Url.Content("~/Content/kendo/style/CustomThemes/custom_blue_opal.css")%>" rel="stylesheet" />

    <script src="<%= Url.Content("~/Content/kendo/js/jquery.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/kendo.web.min.js")%>"></script>
    <script src="<%= Url.Content("~/Content/kendo/js/console.js")%>"></script>
    
<script type="text/javascript" language="javascript">
    var capache = '<%=Session["CaptchaImageText"] %>';

    function validateEmail() {
        var reg = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/
        var address = $("#txtEmail").val();
        if (reg.test(address) == false) {
            return false;
        }
        return true;
    }

    function updateCapche() {
        $.ajax({
            url: '<%= Url.Content("~/Career/Refresh") %>',
            cache: false,
            success: function(html, textStatus, XMLHttpRequest) {
                if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                    window.location = '<%=Url.Content("~/Account/LogOn") %>';
                    window.location.reload();
                }
                else {
                    $('#addImg').html(html);
                    alert(html);
                }
            }
        });

    }
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<!-- Start nav menu Div -->
<div class="container">
			    
     <!-- start header here-->
	<header>
		<div id="fdw">
				<!--nav-->
					<nav>
						<ul>
                        <li ><a href="<%=Url.Content("~/") %>">Home</a></li>
                        <li ><a href="<%=Url.Content("~/Home/AboutUs") %>">About Us</a></li>
                        <li ><a href="<%=Url.Content("~/Home/Approach") %>">Approach</a></li>
                        <li><a href="<%=Url.Content("~/Home/Portfolio") %>">Portfolio</a></li>
                        <li class="current" ><a href="<%=Url.Content("~/Home/Career") %>">Career</a></li>
                        <li ><a href="<%=Url.Content("~/Home/Technologies") %>">Technologies</a>
								<ul class="sub_menu">
									<li><a class="subCurrent" href="<%=Url.Content("~/Home/MSDotNet") %>">.Net</a></li>
                                    <li><a href="<%=Url.Content("~/Home/MobileApp") %>">Mobile Application</a></li>
								</ul>
							</li>
                        <li><a href="<%=Url.Content("~/Home/ContactUs") %>">Contact Us</a></li>  
                        </ul>
					</nav>
		</div><!-- end fdw -->
	</header><!-- end header -->
    
</div>
<!-- End nav menu Div -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Start main Div -->
<div class="main">

   <!-- Start slider Div -->
   
<div class="slider1">   
   
 <div><span style="font-size:24px; color:#515151;">Career</span></div>
 <div class="m-top10 page_menu"><span style="color:#b2b2b2;">You are here: <a href="<%=Url.Content("~/") %>">Home</a> » Career</span></div>

   
</div>   
   
   
   <!-- End slider Div -->

<!-- Start center Div -->
<%=ViewData["MessageLabel"] %>
<div class="center">
    
    <% using (Html.BeginForm("Apply", "Career",
                                FormMethod.Post, new { enctype = "multipart/form-data" ,id="formApply"}))
       { %>
       
       <div class="c_appleft">
        
       <div class="c_name"><h2 style="font-weight:normal; margin-bottom:5px;">Personal Detail</h2></div>
       
      
        <div class="contact_name">Your Name :*</div>
        <div id="VFirstName"><input type="text" id="FirstName" name="FirstName" class="k-textbox"  placeholder="First name" required data-max-msg="Please enter your first name" /></div>
        &nbsp;
        <div id="VLastName"><input type="text" id="LastName" name="LastName" class="k-textbox"  placeholder="Last name" required data-max-msg="Please enter your last name" /></div>
        <div class="contact_name"></div>
        
        <div class="contact_name">Age (18 to 35) :*</div>
        <div id="VAge"><input id="Age" name="Age" type="text" min="18" max="35" placeholder="Select Age" required data-max-msg="Enter value between 18 and 20" /></div>
        <span class="k-invalid-msg" data-for="Age"></span>
        <div class="contact_name"></div>
        
        <div class="contact_name">Gender :*</div>
        <div id="VGender"><select name="Gender" id="Gender" required data-required-msg="Select Gender">
                        <option value="">--Select--</option>
                        <option value="Male">Male</option>
                        <option value="Female">Female</option>
                    </select></div>
        <span class="k-invalid-msg" data-for="Gender"></span>
        <div class="contact_name"></div>
    
       <div class="contact_name">Phone Number :*</div>
        <div id="Vtel"><input type="tel" id="tel" name="tel" class="c_inputbox" pattern="\d{10}" placeholder="Please enter a ten digit phone number" required
                        validationMessage="Please enter a ten digit phone number"/></div>

        
        <div class="contact_name">Email :*</div>
        <div id="VEmail"><input type="email" id="Email" name="Email" class="c_inputbox" placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid" /></div>
        
        <div class="contact_name">City :*</div>
        <div id="VCity"><input type="text" id="City" name="City" class="k-textbox" placeholder="City Name" required validationMessage="Please enter your City Name" /></div>
        <div class="contact_name"></div>
        
         <div class="contact_name">Minimum notice period (0-6 week) : </div>
         <div class="box12">
         <input id="NoticePeriod" name="NoticePeriod" class="balSlider" value="0" />
         </div>
        <div class="contact_name"></div>
        </div>
        
        <div class="c_appright">
        <div class="c_name"><h2 style="font-weight:normal; margin-bottom:5px;">Education Detail</h2></div>
         <div class="contact_name">Educations :*</div>
        <div id="VEducations"><input type="text" id="Educations" name="Educations" class="k-textbox" placeholder="Your Educations" required validationMessage="Please enter your Educations" /></div>
        <div class="contact_name"></div>
        
        <div class="contact_name">Designation (.Net Developer | iPhone (ios) | Web Designer) :* </div>
        <div id="VDesignation"><select name="Designation" id="Designation" required data-required-msg="Select Designation">
                        <option value="">--Select--</option>
                        <%
                            foreach (var item in (List<SelectListItem>)ViewData["postionList"])
                            {
                                if (item.Value == (string)ViewData["postion"])
                                {%>
                                <option value='<%=item.Value%>' selected="selected"><%=item.Text %></option>
                                <%}
                                else
                                {%>
                                <option value='<%=item.Value%>'><%=item.Text %></option>
                            <%}
                            }%>
                    </select></div>
        <span class="k-invalid-msg" data-for="Designation"></span>
        <div class="contact_name"></div>
        
        <div class="contact_name">Total Experience (0-5 year) (0-11 Month):* </div>
        <div id="VYear"><input id="Year" name="Year" type="text" min="0" max="5"  placeholder="year" required data-max-msg="Enter value between 0 and 5" /></div>
        <span class="k-invalid-msg" data-for="Year"></span>
        &nbsp;
        <div id="VMonth"><input id="Month" name="Month" type="text" min="0" max="11"  placeholder="Month" required data-max-msg="Enter value between 0 and 11" /></div>
        <span class="k-invalid-msg" data-for="Month"></span>
        <div class="contact_name"></div>
        
        <div class="contact_name">Current Salary : </div>
        <input id="CurrentSalary" name="CurrentSalary" type="text" min="0" value="0" required/>&nbsp;&nbsp;Per/Year
        <span class="k-invalid-msg" data-for="CurrentSalary"></span>
        <div class="contact_name"></div>
        
        <div class="contact_name">Expected Salary:</div>
        <input id="ExpectedSalary" name="ExpectedSalary" type="text" min="0" value="0" required/>&nbsp;&nbsp;Per/Year
        <span class="k-invalid-msg" data-for="ExpectedSalary"></span>
        <div class="contact_name"></div>
        
        <div class="contact_name">Upload Resume (docx/doc) :</div>
        <input type="file" name="Resume" id="Resume" style="color: rgb(150, 150, 150);" required validationMessage="Please upload your resume" />
        <input type="hidden" id="hdnPosition" name="hdnPosition" value="<%=ViewData["postion"] %>" />
        <span id="lblValResume" ></span>
        <div class="contact_name"></div>
        
        <div class="contact_name">Verification Code :*</div>
        <%--<%= Ajax.ActionLink("Refresh", "Refresh", "Career", null, new AjaxOptions { UpdateTargetId = "addImg" }, new { Title = "Click to refresh" })%>--%>
        <%--<input type="button" onclick="updateCapche()" />--%>
        <div id="addImg"><%Html.RenderPartial("Refresh"); %></div>
        <input type="text" id="txtCodeNumber" name="txtCodeNumber" class="c_inputbox" placeholder="Validation Code" required validationMessage="Please enter validation code" />
        <span id="lblValCode" ></span>
        <div class="contact_name"></div>
        <br />
        </div>
  <%--<input type="button" value="Apply" class="c_button" style="cursor:pointer" onclick="Validate()"  /> --%>
  <div class="c_appleft">
                    <button class="c_button" type="button" style="cursor:pointer">Submit</button>
                    <%--<input type="button" id="clear" value="Clear" class="c_button" style="cursor:pointer"/>--%>
                    <br />
                <span class="status">
                </span>
                </div>
<%} %>
</div>
<!-- End center Div -->
</div>
<!-- End main Div -->

<style scoped>
    .valid {
                    color: green;
                }

                .invalid {
                    color: red;
                }
                .k-invalid {
                  border-color: red;
                }
               /* .k-slider-selection
                {
                    background-color: #0090CD;
                }*/
                
</style>
 <script>
     $(document).ready(function() {

         //$("#Resume").kendoUpload();
         //         $("#Resume").kendoTooltip({
         //             content: "Hello!",
         //             position: "left"
         //         });
         //         $('#Resume').change(function() {
         //             var f = this.files[0];
         //             alert(f.size + " :" + f.fileSize);
         //         });

         $('#txtCodeNumber').css('border', '1px solid #7EC6E3');
         //$('#tel').css('border', '1px solid #7EC6E3');
         //$('#Email').css('border', '1px solid #7EC6E3');

         $("#Gender").kendoDropDownList();
         $("#Designation").kendoDropDownList();

         $("#Age").kendoNumericTextBox({ format: "{0:n0}", decimals: 0 });
         $("#Year").kendoNumericTextBox({ format: "{0:n0}", decimals: 0 });
         $("#Month").kendoNumericTextBox({ format: "{0:n0}", decimals: 0 });
         $("#CurrentSalary").kendoNumericTextBox({ format: "{0:n0}", decimals: 0 });
         $("#ExpectedSalary").kendoNumericTextBox({ format: "{0:n0}", decimals: 0 });

         var slider = $("#NoticePeriod").kendoSlider({
             increaseButtonTitle: "Right",
             decreaseButtonTitle: "Left",
             min: 0,
             max: 6,
             smallStep: 1,
             largeStep: 1
         }).data("kendoSlider");

         //var validator = $("#formApply").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorFN = $("#VFirstName").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorLN = $("#VLastName").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorAge = $("#VAge").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorGender = $("#VGender").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatortel = $("#Vtel").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorEmail = $("#VEmail").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorCity = $("#VCity").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorEducations = $("#VEducations").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorDesignation = $("#VDesignation").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorYear = $("#VYear").kendoValidator({ validateOnBlur: false }).data("kendoValidator");
         var validatorMonth = $("#VMonth").kendoValidator({ validateOnBlur: false }).data("kendoValidator");



         var status = $(".status");
         // $("#< validatorElement > span.k-tooltip-validation").hide();

         $("button").click(function() {

             var val = $("#Resume").val();
             var fi = document.getElementById('Resume');
             var MaxSize = 1;
             var sizeInBytes = MaxSize * 1024 * 1024;
             var isValid = true;
             var strMsg = "";


             //$("#< validatorElement > span.k-tooltip-validation").hide();
             if (val == "") {
                 $('#Resume').css('border', '1px solid red');
                 isValid = false;
             }
             else {
                 if (val.length > 0 && val.substring(val.lastIndexOf('.') + 1).toLowerCase() != "docx" && val.substring(val.lastIndexOf('.') + 1).toLowerCase() != "doc") {
                     $("#lblValResume").html("");
                     $("#lblValResume").append("<label>docx and doc file is allowed \n</label");
                     strMsg = strMsg + ">> Only docx or doc file are allowed.  \n";
                     $('#Resume').css('border', '1px solid red');
                     isValid = false;
                 }
                 else {
                     var appname = window.navigator.appName;
                     //alert(appname);
                     if (appname.indexOf("Internet Explorer") != -1) {
                         $("#lblValResume").html("");
                         $('#Resume').css('border', '');
                     }
                     else {
                         if (fi.files[0].size > sizeInBytes) {
                             //alert("more size");
                             $("#lblValResume").html("");
                             $("#lblValResume").append("<label>file should not be more than 1 mb. \n</label");
                             strMsg = strMsg + ">>File should be less then or equal to 1 mb.  \n";
                             $('#Resume').css('border', '1px solid red');
                             isValid = false;
                         }
                         else {
                             $("#lblValResume").html("");
                             $('#Resume').css('border', '');
                         }
                     }
                 }
             }

             var code = document.getElementById('txtCodeNumber').value;
             //alert(code + capache)
             if (code != capache) {
                 strMsg = strMsg + ">> Incorrect Validation Code. \n";
                 $('#txtCodeNumber').css('border', '1px solid red');
                 isValid = false;
             }
             else { $('#txtCodeNumber').css('border', '1px solid #7EC6E3'); } //CCCCCC

             /*if (!validator.validate()) {
             validator.hideMessages();
             isValid = false;
             }*/
             if (!validatorFN.validate()) {
                 validatorFN.hideMessages();
                 isValid = false;
             }
             if (!validatorLN.validate()) {
                 validatorLN.hideMessages();
                 isValid = false;
             }
             if (!validatorAge.validate()) {
                 validatorAge.hideMessages();
                 $('#VAge').css('border', '1px solid red');
                 $('#VAge').css('width', '149px');
                 isValid = false;
             }
             else { $('#VAge').css('border', ''); }

             if (!validatorGender.validate()) {
                 validatorGender.hideMessages();
                 $('#VGender').css('border', '1px solid red');
                 $('#VGender').css('width', '149px');
                 isValid = false;
             }
             else { $('#VGender').css('border', ''); }

             if (!validatortel.validate()) {
                 validatortel.hideMessages();
                 isValid = false;
             }

             if (!validatorEmail.validate()) {
                 validatorEmail.hideMessages();
                 isValid = false;
             }

             if (!validatorCity.validate()) {
                 validatorCity.hideMessages();
                 isValid = false;
             }

             if (!validatorEducations.validate()) {
                 validatorEducations.hideMessages();
                 isValid = false;
             }

             if (!validatorDesignation.validate()) {
                 validatorDesignation.hideMessages();
                 $('#VDesignation').css('border', '1px solid red');
                 $('#VDesignation').css('width', '149px');
                 isValid = false;
             }
             else { $('#VDesignation').css('border', ''); }

             if (!validatorYear.validate()) {
                 validatorYear.hideMessages();
                 $('#VYear').css('border', '1px solid red');
                 $('#VYear').css('width', '149px');
                 isValid = false;
             }
             else { $('#VYear').css('border', ''); }

             if (!validatorMonth.validate()) {
                 validatorMonth.hideMessages();
                 $('#VMonth').css('border', '1px solid red');
                 $('#VMonth').css('width', '149px');
                 isValid = false;
             }
             else { $('#VMonth').css('border', ''); }


             if (isValid == true) {
                 document.getElementById("formApply").submit();
                 fadeinout();
             }
             else {
                 /*status.text(strMsg)
                 .removeClass("valid")
                 .addClass("invalid");*/

             }
         });

         $("#clear").click(function() {
             /* $('#txtCodeNumber').css('border', '1px solid #7EC6E3');
             $('#VFirstName').css('border', '1px solid #7EC6E3');
             $('#VLastName').css('border', '1px solid #7EC6E3');
             $('#VAge').css('border', '');
             $('#VGender').css('border', '');
             $('#Vtel').css('border', '');
             $('#VEmail').css('border', '');
             $('#VCity').css('border', '1px solid #7EC6E3');
             $('#VEducations').css('border', '1px solid #7EC6E3');
             $('#VDesignation').css('border', '');
             $('#VYear').css('border', '');
             $('#VMonth').css('border', '');
             $('#Resume').css('border', '');*/
         });
     });

     function msieversion() {
         var ua = window.navigator.userAgent
         var msie = ua.indexOf("MSIE ")

         if (msie > 0)      // If Internet Explorer, return version number
             return parseInt(ua.substring(msie + 5, ua.indexOf(".", msie)))
         else                 // If another browser, return 0
             return 0

     }
  </script>

<div id="popup_name" class="popup_block">
        <img src="<%= Url.Content("~/Content/images/Carreerloader.gif") %>" align="Center" alt="" />
    </div>
</asp:Content>




