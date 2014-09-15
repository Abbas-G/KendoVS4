<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-box-header">
        <ul class="content-box-tabs">
            <li><span  class="current" ><%=Html.ActionLink("Detail","Index","Child") %></span></li>
            <li><span ><%=Html.ActionLink("List", "List", "Child")%></span></li>
        </ul>
    </div>
    <script>
        var dataSourceParent = [{ Name: "AAA"},{ Name: "BBB"}];
        $(document).ready(function () {
            $("#SearchParent").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Name",
                dataSource: dataSourceParent,
                //filter: "contains",
                //autoBind: false,
                //placeholder: "Merchandise",  //for kendoComboBox
                optionLabel: "--Search--",
                index: 0,
                // placeholder: "Merchandise",
                change: configChildRows
            });
        });
        function configChildRows() {
            var field = document.getElementById('SearchParent');
            if (field.value != null && field.value != "" && field.value != "--Search--") {
                fadeinout();
                $.ajax({
                    url: '<%= Url.Content("~/Child/AddRows") %>',
                    cache: false,
                    success: function (html, textStatus, XMLHttpRequest) {
                        if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                            window.location = '<%=Url.Content("~/Account/Login") %>';
                            window.location.reload();
                        }
                        else {
                            $('#TargetChildAddRows').html(html);
                            fadeover();
                        }
                    }
                });
            }
        }
    </script>
    <div class="content-box-content">
    <input id="SearchParent" name="SearchParent" maxlength="524288"  />
    <div id="TargetChildAddRows">
        
    </div>
<div id="popup_name" class="popup_block">
        <div class="k-loading-image"></div>
    </div>
  
    </div>
</asp:Content>
