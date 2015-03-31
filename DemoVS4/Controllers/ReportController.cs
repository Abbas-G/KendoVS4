using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Svg;
using System.Text;
using System.Web.Hosting;
using Report.Web.Models;
using Google.GData.Analytics;
using Google.GData.Client;


namespace DemoVS4.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        Report.Web.Models.AnalyticsManager analyticsManager = new Report.Web.Models.AnalyticsManager();
        public ActionResult Index()
        {
            return View();
        }

        #region ReportB
        public ActionResult ReportB()
        {
            return View();
        }
        public ActionResult PartialReportB(string StartDate, string EndDate)
        {
            string token = analyticsManager.InitializeToken();
            string segment = "";
            AtomEntryCollection DocCount = analyticsManager.GetReportDataDocCount(token, segment, StartDate, EndDate, "pageviews");
            ViewData["EntriesDocCount"] = TempData["EntriesDocCountTemp"] = DocCount;
            ViewData["startDate"] = StartDate;
            ViewData["endDate"] = EndDate;
            return PartialView("ReportViewB");
        }
        public JsonResult GetReportB(string StartDate, string EndDate)
        {
            string token = analyticsManager.InitializeToken();
            string segment = "";
            string startDate = StartDate;
            string endDate = EndDate;
            AtomEntryCollection DocCount = analyticsManager.GetReportDataDocCountForChart(token, segment, startDate, endDate, "pageviews");
            List<Report.Web.Models.AxisPoint> list = analyticsManager.GetGraphDataforDocCount(DocCount, startDate, endDate);

            var ListWitoutFK = list.Select(x => new
            {
                category = x.X,
                valueX = x.Y//,
                // Date = Convert.ToInt32(x.Date).ToString("####/##/##")
            });

            return Json(ListWitoutFK);
        }
        [ValidateInput(false), HttpPost]
        public JsonResult ExportB(string ReportType, string StartDate, string EndDate, string svg)
        {
            try
            {
                string token = analyticsManager.InitializeToken();
                string segment = "";
                //AtomEntryCollection DocCount = analyticsManager.GetReportDataDocCount(token, segment, StartDate, EndDate, "pageviews");
                AtomEntryCollection DocCount = (AtomEntryCollection)TempData["EntriesDocCountTemp"];
                TempData["EntriesDocCountTemp"] = TempData["EntriesDocCountTemp"];
                string Description = "Which document accessing app:-";
                string fileName = "";
                bool chk = true;
                if (ExportChartToPng(svg))
                {
                    if (ReportType.Equals("excel"))
                    {

                        fileName = "Report.xlsx";

                        DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/ReportContent"));
                        foreach (var item in dirInfo.GetFiles())
                        {
                            if (item.Name.Equals(fileName))
                            {
                                try
                                {
                                    String PreviousPath = dirInfo.FullName + @"\" + item.Name;
                                    System.IO.File.Delete(PreviousPath);
                                    break;
                                }
                                catch (IOException exp)
                                { break; }
                            }
                        }

                        string filePath = Server.MapPath("~/ReportContent/") + fileName;

                        ExcelHelper gclass = new ExcelHelper();
                        chk = gclass.CreatePackageForReportB(filePath, DocCount, Description, StartDate, EndDate);
                        return Json(new { Value = (chk) ? fileName : "False" });
                    }
                    else
                    {
                        fileName = "Report.pdf";

                        DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/ReportContent"));
                        foreach (var item in dirInfo.GetFiles())
                        {
                            if (item.Name.Equals(fileName))
                            {
                                try
                                {
                                    String PreviousPath = dirInfo.FullName + @"\" + item.Name;
                                    System.IO.File.Delete(PreviousPath);
                                    break;
                                }
                                catch (IOException exp)
                                { break; }
                            }
                        }

                        string filePath = Server.MapPath("~/ReportContent/") + fileName;

                        PDFHelper pdfHelper = new PDFHelper();
                        chk = pdfHelper.GeneratePDFSummary(filePath, DocCount, Description, StartDate, EndDate, "B");
                        return Json(new { Value = (chk) ? fileName : "False" });
                    }
                }
                else
                { return Json(new { Value = "False" }); }

            }
            catch (Exception e)
            {
                return Json(new { Value = "False" });
            }

        }
        #endregion

        [NonAction]
        [ValidateInput(false)]
        public bool ExportChartToPng(string svg)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/ReportContent"));
            try
            {
                foreach (var item in dirInfo.GetFiles())
                {
                    if (item.Name.Equals("Svg.Png"))
                    {
                        try
                        {
                            String PreviousPath = dirInfo.FullName + @"\" + item.Name;
                            System.IO.File.Delete(PreviousPath);
                            break;
                        }
                        catch (IOException exp)
                        { break; }
                    }
                }
                var byteArray = Encoding.ASCII.GetBytes(svg);
                using (var stream = new MemoryStream(byteArray))
                {
                    var loSVGDocument = SvgDocument.Open(stream);
                    foreach (var child in loSVGDocument.Children)
                    {
                        SetFont(child);
                    }
                    var bitmap = loSVGDocument.Draw();
                    var path = Path.Combine(Server.MapPath("~/ReportContent"), "Svg.Png");
                    bitmap.Save(path);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private void SetFont(SvgElement element)
        {
            foreach (var child in element.Children)
            {
                SetFont(child);
            }

            try
            {
                //var svgText = Parent as SvgText; //try to cast the element as a SvgText
                //if it succeeds you can modify the font

                var svgText = element as SvgText;
                float lfFontsize = 12.0f;
                //svgText.Font = new Font("Arial") as iTextSharp.text.Font;// new Font("Arial", 12.0f);
                //svgText.Font.FontFamily = "Arial";
                svgText.FontSize = new SvgUnit(12.0f);

            }
            catch
            {

            }
        }
    }
}
