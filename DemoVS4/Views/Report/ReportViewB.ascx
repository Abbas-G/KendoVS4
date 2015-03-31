<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<br />
<div>
<img src="<%= Url.Content("~/Content/images/excel.png")%>" onclick="ExportToExcel('excel');" alt="ExportToExcel" style="float:right;cursor:pointer"/>
<img src="<%= Url.Content("~/Content/images/pdf.png")%>" onclick="ExportToExcel('pdf');" alt="ExportToExcel" style="float:right;cursor:pointer"/>
</div>
<br />
<br />
<table id="Cgrid">
    <thead>
        <tr>
            <th>
                Action
            </th>
            <th>
                Document
            </th>
            <th>
                Person
            </th>
            <th>
                Total Visitors
            </th>
        </tr>
    </thead>
    <tbody>
        <%Google.GData.Client.AtomEntryCollection detailListDocCount = (Google.GData.Client.AtomEntryCollection)ViewData["EntriesDocCount"];
          foreach (Google.GData.Analytics.DataEntry pointEntry in detailListDocCount)
          {
              if (pointEntry.Dimensions[3].Value.Equals("Surat")) { }
              else
              {%>
        <tr>
            <td>
                <%=pointEntry.Dimensions[2].Value%>
            </td>
            <td>
                <%=pointEntry.Dimensions[0].Value%>
            </td>
            <td>
                <%=pointEntry.Dimensions[1].Value%>
            </td>
            <td>
                <%=pointEntry.Metrics[0].Value%>
            </td>
        </tr>
        <%}
          } %>
    <tbody>
</table>
<%--<b>How many times people accessing app? :-
    <%=summary[1].Value%>
    (Total No. of Events)</b>--%>
    <div style="width:880px;overflow-x:hidden;" >
<div id="chart">
</div>
</div>

<script>
    $("#Cgrid").kendoGrid({
        //height: 350,
        sortable: true,
        dataSource: { pageSize: 10 }, //scrollable: true,
        pageable: true,
        filterable: { extra: false,
            operators: { // redefine the string operators
                string: {
                    contains: "Contains"
                }
            }
        }
    });

    setTimeout(function () {
        createChart();
    }, 100);

    function createChart() {
        var SDate = '<%=ViewData["startDate"] %>';
        var Edate = '<%=ViewData["endDate"]  %>';
        var jsonObject = new kendo.data.DataSource({
            transport: {
                read: {
                    url: '<%= Url.Content("~/Report/GetReportB")%>',
                    dataType: "json",
                    type: "POST",
                    data: { StartDate: SDate, EndDate: Edate }
                }
            }
                , schema: {
                    model: {
                        fields: {
                            category: { type: "string" },
                            valueX: { type: "number" },
                            Date: { type: "date" }
                        }
                    }
                },
            requestStart: function () {
                fadeinout();
            },
            requestEnd: function () {
                fadeover();

            }
        });

        $("#chart").kendoChart({
            dataSource: jsonObject,
            height: 1500,
            title: {
                text: "[X-Axis : Document , Y-Axis : No. of Visitor(s) occur]"
            },
            //chartArea: { margin: 0, padding: 0, height: (screen.height * 0.50), width: (screen.width * 2) },
            //plotArea: { margin: 0, padding: 0, height: (screen.height * 0.50), width: (screen.width * 2) },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series:
        [{
            field: "valueX",
            name: "Total Visitors"
        }],
            categoryAxis: {
                field: "category",


                labels: {
                    rotation: -90,
                    color: "#000"
                }
            },
            valueAxis: {
                labels: {

            }//,
            //majorUnit: 10000
        },
        tooltip: {
            visible: true
        }
    });
}

function ExportToExcel(Type) {
    var chart = $("#chart").data("kendoChart");
    fadeinout();
    //alert(chart.svg().replace(new RegExp('&', 'g'), '\N'));
    $.ajax({
        url: '<%= Url.Content("~/Report/ExportB") %>',
        cache: false,
        type: "POST",
        data: { 'ReportType': Type, 'StartDate': $("#startDate").val(), 'EndDate': $("#endDate").val(), 'svg': chart.svg().replace(new RegExp('&', 'g'), '\N') },
        success: function (html, textStatus, XMLHttpRequest) {
            if (XMLHttpRequest.getResponseHeader('X-LOGON') == "true") {
                window.location = '<%=Url.Content("~/Account/LogOn") %>';
                window.location.reload();
            }
            else {
                if (html.Value != "False")
                    window.open('<%= Url.Content("~/ReportContent/") %>' + html.Value, "_parent");
                else
                    alert("Server Error!");
                fadeover();
            }
        }
                ,
        error: function (msg) {
            alert("Error!");
            fadeover();
        }
    });
}
</script>

