using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using Google.GData.Analytics;
using Google.GData.Client;

namespace Report.Web.Models
{
    public class PDFHelper
    {
        public int BusinessID { get; set; }
        public int BusinessID2 { get; set; }

        public bool GeneratePDFSummary(string filePath, AtomEntryCollection entriesCountry, string title, string startdate, string enddate,string ReportFor)
        {

            Document pdfDoc = new Document(PageSize.LETTER, 0f, 0f, 20f, 100f);

            /***save file**/
            MemoryStream myMemoryStream = new MemoryStream();
            //PdfWriter myPDFWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
            /***/

            try
            {
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                pdfPage page = new pdfPage();

                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfWriter.PageEvent = page;
                pdfDoc.Open();

                //pdfDoc.Add(new Chunk(""));
                //// pdfDoc.Add(GetFirstPage(business));

                //pdfDoc.NewPage();
                //pdfDoc.Add(GetSecondPage(business, auditReport));

                //pdfDoc.NewPage();
                pdfDoc.Add(new Phrase("NHP REPORT From " + startdate + " To " + enddate, FontFactory.GetFont("Arial", 28, 1, new BaseColor(0, 82, 155))));

                //pdfDoc.Add(GetSummaryPage(business1, whsDetail1, business2, whsDetail2));
                //pdfDoc.Add();
                //PdfPTable tableMain = new PdfPTable(1);
                //tableMain.DefaultCell.Border = PdfPCell.NO_BORDER;
                //PdfPCell cellHead = new PdfPCell(new Phrase(""));
                //cellHead.Border = PdfPCell.NO_BORDER;
                //tableMain.AddCell(cellHead);

                PdfPTable table = new PdfPTable(1);
                //table.DefaultCell.Border = PdfPCell.NO_BORDER;

                    PdfPCell cellTitle = new PdfPCell(new Phrase(title, FontFactory.GetFont("Arial", 20, 1, new BaseColor(0, 82, 155))));
                    cellTitle.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cellTitle.Border = PdfPCell.NO_BORDER;
                    table.AddCell(cellTitle);

                    PdfPTable table2 = new PdfPTable(1);
                    table2.DefaultCell.Border = PdfPCell.NO_BORDER;
                    if (ReportFor.Equals("A"))
                        table2.AddCell(GetSummaryTableforReportA(entriesCountry));
                    if (ReportFor.Equals("B"))
                        table2.AddCell(GetSummaryTableforReportB(entriesCountry));
                    if (ReportFor.Equals("C"))
                        table2.AddCell(GetSummaryTableforReportC(entriesCountry));
                    if (ReportFor.Equals("D"))
                        table2.AddCell(GetSummaryTableforReportD(entriesCountry));
                    
                    //table2.AddCell(GetSummaryTable(whsDetail1, true));
                    

                    PdfPTable table3 = new PdfPTable(1);
                    table3.DefaultCell.Border = PdfPCell.NO_BORDER;
                    string imagepath = HttpContext.Current.Server.MapPath("~/ReportContent/Svg.Png");
                    Image gif = Image.GetInstance(imagepath);
                    //gif.ScaleToFit(300, 200);
                    PdfPCell cell = new PdfPCell();
                    cell.AddElement(gif);
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.Border = PdfPCell.NO_BORDER;
                    table3.AddCell(cell);
                    table2.AddCell(table3);

                    table.AddCell(table2);

                pdfDoc.Add(table);


                /* PdfPTable table = new PdfPTable(2);
                 PdfPCell cell = new PdfPCell();
                 Paragraph p = new Paragraph();
                 p.Add(new Phrase("Test "));
                 //p.Add(new Chunk(image, 0, 0));
                 p.Add(new Phrase(" more text "));
                 //p.Add(new Chunk(image, 0, 0));
                 //p.Add(new Chunk(image, 0, 0));
                 p.Add(new Phrase(" end."));
                 cell.AddElement(p);
                 table.AddCell(cell);
                 table.AddCell(new PdfPCell(new Phrase("test 2")));
                 pdfDoc.Add(table);
                 //pdfDoc.Add(new Paragraph("GIF"));*/


                pdfDoc.Close();

                /***save file**/
                byte[] content = myMemoryStream.ToArray();
                //string fileName = "Report_" + uniqueId + ".pdf";
                //string filePath = HttpContext.Current.Server.MapPath("~/Report/") + fileName;

                /*if (System.IO.File.Exists(filePath))
                {
                     System.IO.File.Delete(filePath);
                     System.Threading.Thread.Sleep(1000);
                }*/

                //using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write)) 
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    fs.Write(content, 0, (int)content.Length);
                }

                /***end**/
            }
            catch (System.Threading.ThreadAbortException tex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private PdfPTable GetSummaryTableforReportA(AtomEntryCollection entriesCountry)
        {
            PdfPTable summaryTable = new PdfPTable(5);
            summaryTable.SplitLate = false;
            summaryTable.SplitRows = false;
            summaryTable.WidthPercentage = 50;

            summaryTable.AddCell(GetTitleCell("Date", false));
            summaryTable.AddCell(GetTitleCell("Location", false));
            summaryTable.AddCell(GetTitleCell("Action", false));
            summaryTable.AddCell(GetTitleCell("Document", false));
            summaryTable.AddCell(GetTitleCell("Total Visitors", false));
            int i = 0, k = 0;
            BaseColor evenColor = new BaseColor(230, 230, 230);
            BaseColor oddColor = new BaseColor(217, 217, 217);
            BaseColor rowColor = evenColor;
            int? totalB = 0;

            foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
            {
                rowColor = ((i % 2 == 0) ? evenColor : oddColor); i++;

                if (pointEntry.Dimensions[2].Value.Equals("Surat")) { }
                else
                {
                    summaryTable.AddCell(GetExistingNameCell((Convert.ToInt32(pointEntry.Dimensions[0].Value)).ToString("####-##-##"), rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[1].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[4].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[3].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Metrics[0].Value, rowColor, false));
                    totalB = totalB + Convert.ToInt32(pointEntry.Metrics[0].Value);
                }
            }
             summaryTable.AddCell(GetReplacementValueCellColspan4("Total Visitors", true, true));
             summaryTable.AddCell(GetExistingNameCell(totalB+"", rowColor, false));


            return summaryTable;
        }

        private PdfPTable GetSummaryTableforReportB(AtomEntryCollection entriesCountry)
        {
            PdfPTable summaryTable = new PdfPTable(4);
            summaryTable.SplitLate = false;
            summaryTable.SplitRows = false;
            summaryTable.WidthPercentage = 50;

            summaryTable.AddCell(GetTitleCell("Action", false));
            summaryTable.AddCell(GetTitleCell("Document", false));
            summaryTable.AddCell(GetTitleCell("Person", false));
            summaryTable.AddCell(GetTitleCell("Total Visitors", false));
            int i = 0, k = 0;
            BaseColor evenColor = new BaseColor(230, 230, 230);
            BaseColor oddColor = new BaseColor(217, 217, 217);
            BaseColor rowColor = evenColor;
            int? totalB = 0;

            foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
            {
                rowColor = ((i % 2 == 0) ? evenColor : oddColor); i++;

                if (pointEntry.Dimensions[3].Value.Equals("Surat")) { }
                else
                {
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[2].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[0].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[1].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Metrics[0].Value, rowColor, false));
                    totalB = totalB + Convert.ToInt32(pointEntry.Metrics[0].Value);
                }
            }
            summaryTable.AddCell(GetReplacementValueCellColspan3("Total Visitors", true, true));
            summaryTable.AddCell(GetExistingNameCell(totalB + "", rowColor, false));


            return summaryTable;
        }

        private PdfPTable GetSummaryTableforReportC(AtomEntryCollection entriesCountry)
        {
            PdfPTable summaryTable = new PdfPTable(5);
            summaryTable.SplitLate = false;
            summaryTable.SplitRows = false;
            summaryTable.WidthPercentage = 50;

            summaryTable.AddCell(GetTitleCell("Action", false));
            summaryTable.AddCell(GetTitleCell("Document", false));
            summaryTable.AddCell(GetTitleCell("Person", false));
            summaryTable.AddCell(GetTitleCell("Total Visitors", false));
            summaryTable.AddCell(GetTitleCell("Unique Visitors", false));
            int i = 0, k = 0;
            BaseColor evenColor = new BaseColor(230, 230, 230);
            BaseColor oddColor = new BaseColor(217, 217, 217);
            BaseColor rowColor = evenColor;
            int? totalB = 0;
            int? totalB1 = 0;
            foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
            {
                rowColor = ((i % 2 == 0) ? evenColor : oddColor); i++;

                if (pointEntry.Dimensions[3].Value.Equals("Surat")) { }
                else
                {
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[0].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[1].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[2].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Metrics[1].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Metrics[2].Value, rowColor, false));
                    totalB = totalB + Convert.ToInt32(pointEntry.Metrics[1].Value);
                    totalB1 = totalB1 + Convert.ToInt32(pointEntry.Metrics[2].Value);
                }
            }
            summaryTable.AddCell(GetReplacementValueCellColspan3("Total", true, true));
            summaryTable.AddCell(GetExistingNameCell(totalB + "", rowColor, false));
            summaryTable.AddCell(GetExistingNameCell(totalB1 + "", rowColor, false));


            return summaryTable;
        }

        private PdfPTable GetSummaryTableforReportD(AtomEntryCollection entriesCountry)
        {
            PdfPTable summaryTable = new PdfPTable(3);
            summaryTable.SplitLate = false;
            summaryTable.SplitRows = false;
            summaryTable.WidthPercentage = 50;

            summaryTable.AddCell(GetTitleCell("Device Name", false));
            summaryTable.AddCell(GetTitleCell("Version", false));
            summaryTable.AddCell(GetTitleCell("Total Visitors", false));
            int i = 0, k = 0;
            BaseColor evenColor = new BaseColor(230, 230, 230);
            BaseColor oddColor = new BaseColor(217, 217, 217);
            BaseColor rowColor = evenColor;
            int? totalB = 0;

            foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
            {
                rowColor = ((i % 2 == 0) ? evenColor : oddColor); i++;

                //if (pointEntry.Dimensions[2].Value.Equals("Surat")) { }
                //else
                //{
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[0].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Dimensions[1].Value, rowColor, false));
                    summaryTable.AddCell(GetExistingNameCell(pointEntry.Metrics[0].Value, rowColor, false));
                    totalB = totalB + Convert.ToInt32(pointEntry.Metrics[0].Value);
                //}
            }
            summaryTable.AddCell(GetReplacementValueCellColspan2("Total Visitors", true, true));
            summaryTable.AddCell(GetExistingNameCell(totalB + "", rowColor, false));


            return summaryTable;
        }
        private PdfPCell GetTitleCell(string cellText, bool isFirstCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, BaseColor.WHITE)));
            //cell.BackgroundColor = new BaseColor(208, 100, 61);
            //cell.BackgroundColor = new BaseColor(159, 204, 231);
            //cell.BackgroundColor = new BaseColor(52, 104, 151);
            cell.BackgroundColor = new BaseColor(0, 82, 155);
            if (isFirstCell) cell.Colspan = 2;

            return cell;
        }
        private PdfPCell GetExistingNameCell(string cellText, BaseColor rowColor, bool isNumericCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, BaseColor.BLACK)));
            cell.BackgroundColor = rowColor;
            cell.HorizontalAlignment = (isNumericCell ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT);
            return cell;
        }
        private PdfPCell GetExistingValueCell(string cellText, BaseColor rowColor, bool isNumericCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, new BaseColor(203, 0, 0))));
            cell.BackgroundColor = rowColor;
            cell.HorizontalAlignment = (isNumericCell ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT);
            return cell;
        }
        private PdfPCell GetReplacementValueCell(string cellText, bool isFirstCell, bool isNumericCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, BaseColor.BLACK)));
            cell.BackgroundColor = new BaseColor(255, 209, 209);
            cell.HorizontalAlignment = (isNumericCell ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT);
            if (isFirstCell) cell.Colspan = 2;
            return cell;
        }

        private PdfPCell GetReplacementValueCellColspan4(string cellText, bool isFirstCell, bool isNumericCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, BaseColor.BLACK)));
            cell.BackgroundColor = new BaseColor(255, 209, 209);
            cell.HorizontalAlignment = (isNumericCell ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT);
            if (isFirstCell) cell.Colspan = 4;
            return cell;
        }
        private PdfPCell GetReplacementValueCellColspan3(string cellText, bool isFirstCell, bool isNumericCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, BaseColor.BLACK)));
            cell.BackgroundColor = new BaseColor(255, 209, 209);
            cell.HorizontalAlignment = (isNumericCell ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT);
            if (isFirstCell) cell.Colspan = 3;
            return cell;
        }
        private PdfPCell GetReplacementValueCellColspan2(string cellText, bool isFirstCell, bool isNumericCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, BaseColor.BLACK)));
            cell.BackgroundColor = new BaseColor(255, 209, 209);
            cell.HorizontalAlignment = (isNumericCell ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT);
            if (isFirstCell) cell.Colspan = 2;
            return cell;
        }

        private PdfPCell GetSummaryValueCell(string cellText, BaseColor rowColor, bool doRowSpan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9, 0, BaseColor.BLACK)));
            cell.BackgroundColor = rowColor;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (doRowSpan) cell.Rowspan = 2;
            return cell;
        }
        private PdfPCell GetTotalCell(string cellText, BaseColor rowColor, bool isFirstCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 10, 1, BaseColor.BLACK)));
            cell.BackgroundColor = rowColor;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (isFirstCell) cell.Colspan = 6;

            return cell;
        }

    }
    class pdfPage : iTextSharp.text.pdf.PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            base.OnStartPage(writer, doc);

            //if (doc.PageNumber == 1)
            //{
            //    PdfPTable headerTbl = new PdfPTable(1);
            //    headerTbl.WidthPercentage = 100;
            //    headerTbl.TotalWidth = doc.PageSize.Width;

            //    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Content/images/CoverPage.png"));
            //    logo.ScaleAbsolute(doc.PageSize.Width, doc.PageSize.Height);

            //    PdfPCell cell = new PdfPCell();
            //    cell.Border = Rectangle.NO_BORDER;
            //    cell.CellEvent = new TestCellEvent()
            //    {
            //        CellImage = logo
            //    };

            //    headerTbl.AddCell(cell);
            //    headerTbl.WriteSelectedRows(0, -1, 0, 0, writer.DirectContent);
            //}
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            base.OnEndPage(writer, doc);

            //if (doc.PageNumber > 1)
            //{
            //    PdfPTable footerTbl = new PdfPTable(1);
            //    footerTbl.WidthPercentage = 100;
            //    footerTbl.TotalWidth = doc.PageSize.Width;

            //    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Content/images/PDFFooter.png"));
            //    logo.ScaleToFit(doc.PageSize.Width, 500);

            //    PdfPCell cell = new PdfPCell(new PdfPCell());
            //    cell.Phrase = new Phrase(doc.PageNumber.ToString());
            //    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //    cell.PaddingTop = 90;
            //    cell.PaddingRight = 60;
            //    cell.Border = Rectangle.NO_BORDER;
            //    cell.CellEvent = new TestCellEvent()
            //    {
            //        CellImage = logo
            //    };

            //    footerTbl.AddCell(cell);
            //    footerTbl.WriteSelectedRows(0, -1, 0, 105, writer.DirectContent);
            //}
        }
    }

    class TestCellEvent : IPdfPCellEvent
    {
        public Image CellImage;
        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            PdfContentByte cb = canvases[PdfPTable.BACKGROUNDCANVAS];
            CellImage.SetAbsolutePosition(0, 0);
            cb.AddImage(CellImage);
        }
    }
}
