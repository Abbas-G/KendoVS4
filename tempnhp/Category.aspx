<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Category
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainMenuContent" runat="server">
    <ul id="main-nav">
        <li><a href="<%= Url.Content("~/Category/Category") %>">Category</a></li>
        <li><a href="<%= Url.Content("~/SubCategory/SubCategory") %>">SubCategory</a></li>
        <li><a href="<%= Url.Content("~/List/List") %>">List</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <h2>
        <img src="<%= Url.Content("~/Content/images/tools_32.png") %>" alt="Manage Topics" /><label
            style="color: Black;">Edit Category</label>
    </h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" language="javascript">
    var intTextBox = 3;
    function pageload(id){
    intTextBox=3;
  window.location = '<%=Url.Content("~/Category/Category") %>'; 
    }
        
    function Add() {
        if (intTextBox > 51) {
                alert("Maximum 50 allow");   
            }
            else
            {

                var contentID = document.getElementById('PDFcontent');
                var newTBDiv = document.createElement('div');
                newTBDiv.setAttribute('id', 'file_div' + intTextBox);
                var i= parseInt(intTextBox)-1;
                
                newTBDiv.innerHTML = "<table><tr><td style='width:50px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>"+i+".</b></td><td><input type='text' id='image" + i + "' name='image" + i + "' size='30'/></td><td><input type='text' id='file" + i + "' name='file" + i + "' size='30'/></td><td><input type='text' id='TxtFile" + i + "' name='TxtFile" + i + "' size='30' /></td><td><input type='text' id='TxtAddVersion" + i + "' name='TxtAddVersion" + i + "' size='3'  /></td><td> <a href='javascript:Add()'  ><img src='<%=Url.Content("~/Content/images/add.png")%>' width='20' height='20' alt='Add Row' /></a><a href='javascript:Remove()'  ><img src='<%=Url.Content("~/Content/images/Remove.png")%>' width='20' height='20' alt='Remove Row' /></a></td><td></td></tr></table>";

                contentID.appendChild(newTBDiv);


                intTextBox = intTextBox + 1;
            }

        }

        function Remove(){
            if (intTextBox == 3) {
              
              
            }
            else {
                
                intTextBox = intTextBox - 1;
                var contentID = document.getElementById('PDFcontent');
                contentID.removeChild(document.getElementById('file_div' + intTextBox));
               
                contentID.removeChild(document.getElementById('title' + intTextBox));

            }

        }
        
        function AddVideo() {
        if (intTextBox > 51) {
                alert("Maximum 50 allow");   
            }
            else
            {

                var contentID = document.getElementById('PDFcontent');
                var newTBDiv = document.createElement('div');
                newTBDiv.setAttribute('id', 'file_div' + intTextBox);
                var i= parseInt(intTextBox)-1;
                
                newTBDiv.innerHTML = "<table><tr><td style='width:50px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>"+i+".</b></td><td><input type='text' id='image" + i + "' name='image" + i + "' size='20'/></td><td><input type='text' id='file" + i + "' name='file" + i + "' size='22'/></td><td><input type='text' id='youtubefile" + i + "' name='youtubefile" + i + "' size='22'></td><td><input type='text' id='TxtFile" + i + "' name='TxtFile" + i + "' size='22' /></td><td><input type='text' id='TxtAddVersion" + i + "' name='TxtAddVersion" + i + "' size='3'  /></td><td> <a href='javascript:AddVideo()'  ><img src='<%=Url.Content("~/Content/images/add.png")%>' width='20' height='20' alt='Add Row' /></a><a href='javascript:Remove()'  ><img src='<%=Url.Content("~/Content/images/Remove.png")%>' width='20' height='20' alt='Remove Row' /></a></td><td></td></tr></table>";

                contentID.appendChild(newTBDiv);


                intTextBox = intTextBox + 1;
            }

        }

        function check() {
            var i=1;
            while (i <= intTextBox) {
                if (document.getElementById('file' + intTextBox).value == null) {
                    alert("Select Image in image" + intTextBox); 
                }
            
            
            } 
        }
           
       function updateCategoryDetails(list) {
            fadeinout();
            var val = list.options[list.selectedIndex].value;
            if (val == null || val == "") {
                val = "0";
                fadeover();
            }
           $.ajax({
            url: '<%= Url.Content("~/Category/AddCategoryDetails") %>',
                cache: false,
                data: "Id=" + val,
                success: function(html, textStatus, XMLHttpRequest) {
                    if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                        window.location = '<%=Url.Content("~/Account/LogOn") %>';
                        window.location.reload();
                    }
                    else {
                        $('#AddEditCategoryDetails').html(html);
                        fadeover();
                    }
                }
            });

        }
       
       function showDiv(i) {
           
           //eval("document.radioForm.q" + i + "[0].checked") == true
            $.ajax({
            url: '<%= Url.Content("~/Category/AddCategoryPDF") %>',
                cache: false,
                data: "Id=" + i,
                success: function(html, textStatus, XMLHttpRequest) {
                    if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                        window.location = '<%=Url.Content("~/Account/LogOn") %>';
                        window.location.reload();
                    }
                    else {
                        $('#AddEditCategoryPDF').html(html);
                        
                    }
                }
            });

        }
        
        function OnSubmit(i)
        {
        //HiddenOnSubmit.value=i;
        document.getElementById("HiddenOnSubmit").value=i;
        if(i==2)
        {
            var check = false;
            var cnt=document.getElementById("HiddenCnt").value;
            //alert(cnt);
            for (var i = 1; i <= cnt; i++) {
                    var orderingtext = document.getElementById("reorderSelect" + i.toString()).value;
                    //alert(orderingtext);
                    var j = 1;
                    while( j < cnt )
                            {
                                if (i != j) {

                                    var othrstext = document.getElementById("reorderSelect" + j.toString()).value;
                                    //alert(i+" "+j+" :"+orderingtext + " " + orderingtext);
                                   if (orderingtext == othrstext)
                                    {
                                        check = true;
                                    }
                                }
                                j++;
                            }

                     }
                     if (check) {
                     //alert("check");
                         document.getElementById("checkval").style.color = "red";
                         document.getElementById("checkval").innerHTML = "One Order must be for one number only : check duplication.";
                         return false;
                         
                     }
                     else {
                     //alert("uncheck");
                        // document.forms[0].submit();
                        fadeinout();
                        return true;
                        
                     }
            }
            else if(i==3)
            {
                var cnt=intTextBox-2;
                
                var flag=0;
                for(var j=1; j<=cnt; j++)
                {   
                    flag=0;
                    var image=document.getElementById("image" + j.toString()).value;
                    if(image.length>0)
                    {
                        var file=document.getElementById("file" + j.toString()).value;
                        if(file.length>0)
                        {
                            var version=document.getElementById("TxtAddVersion" + j.toString()).value;
                            if(version.length>0)
                            {
                            flag=1;
                            }
                            else
                            {
                            document.getElementById("checkNew").style.color = "red";
                            document.getElementById("checkNew").innerHTML = "Please fill Version ";
                            flag=0;
                            break;
                            }
                        }
                        else
                        {
                        document.getElementById("checkNew").style.color = "red";
                        document.getElementById("checkNew").innerHTML = "Please fill PDF File URL ";
                        flag=0;
                        break;
                        }
                    }
                    else
                    {
                    flag=0;
                    document.getElementById("checkNew").style.color = "red";
                    document.getElementById("checkNew").innerHTML = "Please fill PDF Thumbnail URL ";
                    break;
                    }
                   
                }
                
                if(flag==1)
                {
                intTextBox=3;
                fadeinout();
                return true;
                }
                else
                return false;
            }
            else{}
            
            fadeinout();
        }
        
        function handleEditCategoryClick(Obj, imagen, cid, pid) {
            //alert(Obj + imagen + id);
            if (document.getElementById(Obj).style.display == 'none') {
                //document.getElementById(Obj).style.display = "";
                fadeinout();
                document.getElementById(Obj).style.display = "";
                document.getElementById(imagen).src = '<%=Url.Content("~/Content/images/SliderCollapse.gif") %>';
                // alert("b");
                $.ajax({
                    url: '<%= Url.Content("~/Category/AddCurrentCategoryPDF") %>',
                    cache: false,
                    data: "cid=" + cid+"&pid="+pid,
                    success: function(html, textStatus, XMLHttpRequest) {
                        if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                            window.location = '<%=Url.Content("~/Account/LogOn") %>';
                            window.location.reload();
                        }
                        else {
                            $("#" + Obj).html(html);
                            fadeover();
                        }
                    }
                });

            }
            else {

                document.getElementById(Obj).style.display = "none";
                document.getElementById(imagen).src = '<%=Url.Content("~/Content/images/SliderExpand.gif") %>';

            }
        }
        
        
         function handleAddCategoryClick(Obj, imagen, cid, pid) {
            //alert(Obj + imagen + id);
            intTextBox=3;
            if (document.getElementById(Obj).style.display == 'none') {
                //document.getElementById(Obj).style.display = "";
                fadeinout();
                document.getElementById(Obj).style.display = "";
                document.getElementById(imagen).src = '<%=Url.Content("~/Content/images/SliderCollapse.gif") %>';
                // alert("b");
                $.ajax({
                    url: '<%= Url.Content("~/Category/AddCategoryPDF") %>',
                    cache: false,
                    data: "cid=" + cid+"&pid="+pid,
                    success: function(html, textStatus, XMLHttpRequest) {
                        if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                            window.location = '<%=Url.Content("~/Account/LogOn") %>';
                            window.location.reload();
                        }
                        else {
                            $("#" + Obj).html(html);
                            fadeover();
                        }
                    }
                });

            }
            else {

                document.getElementById(Obj).style.display = "none";
                document.getElementById(imagen).src = '<%=Url.Content("~/Content/images/SliderExpand.gif") %>';

            }
        }
          
   function OnSubmitVideo(i)
        {
        //HiddenOnSubmit.value=i;
        document.getElementById("HiddenOnSubmit").value=i;
        if(i==2)
        {
            var check = false;
            var cnt=document.getElementById("HiddenCnt").value;
            //alert(cnt);
            for (var i = 1; i <= cnt; i++) {
                    var orderingtext = document.getElementById("reorderSelect" + i.toString()).value;
                    //alert(orderingtext);
                    var j = 1;
                    while( j < cnt )
                            {
                                if (i != j) {

                                    var othrstext = document.getElementById("reorderSelect" + j.toString()).value;
                                    //alert(i+" "+j+" :"+orderingtext + " " + orderingtext);
                                   if (orderingtext == othrstext)
                                    {
                                        check = true;
                                    }
                                }
                                j++;
                            }

                     }
                     if (check) {
                     //alert("check");
                         document.getElementById("checkval").style.color = "red";
                         document.getElementById("checkval").innerHTML = "One Order must be for one number only : check duplication.";
                         return false;
                         
                     }
                     else {
                     //alert("uncheck");
                        // document.forms[0].submit();
                        fadeinout();
                        return true;
                        
                     }
            }
            else if(i==3)
            {
                var cnt=intTextBox-2;
                
                var flag=0;
                for(var j=1; j<=cnt; j++)
                {   
                    flag=0;
                    var image=document.getElementById("image" + j.toString()).value;
                    if(image.length>0)
                    {
                        var file=document.getElementById("file" + j.toString()).value;
                        if(file.length>0)
                        {
                            if(file.substr(-4) === '.mp4')
                            {
                            var youtubefile=document.getElementById("youtubefile" + j.toString()).value;
                            if(youtubefile.length>0)
                            {
                                var version=document.getElementById("TxtAddVersion" + j.toString()).value;
                                if(version.length>0)
                                {
                                flag=1;
                                }
                                else
                                {
                                document.getElementById("checkNew").style.color = "red";
                                document.getElementById("checkNew").innerHTML = "Please fill Version ";
                                flag=0;
                                break;
                                }
                              }
                              else
                              {
                                document.getElementById("checkNew").style.color = "red";
                                document.getElementById("checkNew").innerHTML = "Please fill YouTube File URL ";
                                flag=0;
                                break;
                              }
                            }
                            else
                            {
                                document.getElementById("checkNew").style.color = "red";
                                document.getElementById("checkNew").innerHTML = "MP4 File Url must contain .mp4 file ";
                                flag=0;
                                break;
                            }
                            
                        }
                        else
                        {
                        document.getElementById("checkNew").style.color = "red";
                        document.getElementById("checkNew").innerHTML = "Please fill MP4 File URL ";
                        flag=0;
                        break;
                        }
                    }
                    else
                    {
                    flag=0;
                    document.getElementById("checkNew").style.color = "red";
                    document.getElementById("checkNew").innerHTML = "Please fill Video Thumbnail URL ";
                    break;
                    }
                   
                }
                
                if(flag==1)
                {
                intTextBox=3;
                fadeinout();
                return true;
                }
                else
                return false;
            }
            else{}
            
            fadeinout();
        }
    </script>

    <div class="content-box">
        <div class="content-box-header">
            <h3>
                Edit Category
            </h3>
        </div>
        <br />
        <table>
            <tr>
                <td style="width: 8%;">
                </td>
                <td style="width: 30%;">
                    <label>
                        <b>Category Name</b>
                    </label>
                </td>
                <td style="width: 5%;">
                    :
                </td>
                <td style="width: 50%;">
                    <% NHP.Core.Manager.CategoriesManager cm = new NHP.Core.Manager.CategoriesManager();
                       NHP.Web.Models.CategoryModel categoryModel = new NHP.Web.Models.CategoryModel();
                       NHP.Core.DAL.CategorySub data;
                       string a = Convert.ToString(ViewData["Category"]);
                       if (ViewData["Category"] != "0")
                       {
                           data = cm.getCategoryById(Convert.ToInt32(ViewData["Category"])); // viewdata here
                           categoryModel.Cateid = data.CategoryId;
                           categoryModel.CateName = data.CategoryName;

                       }
                       Html.RenderPartial("List", categoryModel); 
                            
                    %>
                    <label>
                        <%=ViewData["ErrorCategory"] %></label>
                </td>
            </tr>
        </table>
        <div id="AddEditCategoryDetails">
        </div>
          <div id="popup_name" class="popup_block">
        <img src="<%= Url.Content("~/Content/images/loader.gif") %>" align="Center" alt="" />
    </div>
    </div>
</asp:Content>
