using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using System;
using Google.GData.Analytics;
using Google.GData.Client;
//using System.Drawing;
using System.IO;
using System.Web;

namespace Report.Web.Models
{
    public class ExcelHelper
    {
        //
        // GET: /ExcelHelper/

        public int BusinessID { get; set; }
        public int BusinessID2 { get; set; }
        public bool CreatePackageForReportA(string filePath, AtomEntryCollection entriesCountry ,string title , string startdate, string enddate)
        {   try
                {
                   /* if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        System.Threading.Thread.Sleep(1000);
                    }*/

            using (SpreadsheetDocument xl = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            //using (SpreadsheetDocument package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook, false))
            {
                try
                {


                    WorkbookPart wbp = xl.AddWorkbookPart();
                    WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();

                    Workbook wb = new Workbook();
                    FileVersion fv = new FileVersion();
                    fv.ApplicationName = "Microsoft Office Excel";
                    Worksheet ws = new Worksheet();
                    SheetData sheetData = new SheetData();

                    

                    WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
                    /*wbsp.Stylesheet = CreateStylesheet();*/

                    Stylesheet stylesheet = new Stylesheet();
                    stylesheet.Append(GetReportNuberingFormat());
                    stylesheet.Append(GetReportFonts());
                    stylesheet.Append(GetReportFills());
                    stylesheet.Append(GetReportBorders());
                    stylesheet.Append(GetReportStyleFormats());
                    stylesheet.Append(GetReportFormats());

                    wbsp.Stylesheet = stylesheet;
                    wbsp.Stylesheet.Save();




                    //SheetData sheetData = new SheetData();
                    MergeCells mergeCells = new MergeCells();

                    sheetData.Append(GetTitelCaptionRow("NHP REPORT From " + startdate + " To " + enddate, "B2", 2U));
                    uint rowIndex = 6U;
                        sheetData.Append(GetSubTitelValueRow(title, "C" + rowIndex, rowIndex));
                        rowIndex++;
                        //sheetData.Append(GetTableHeaderRow(rowIndex+1));

                        Row rowM = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:11" } };
                        Cell cellM = new Cell() { CellReference = "C" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Date");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "D" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Location");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "E" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Action");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "F" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Document");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "G" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Total Visitors");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        sheetData.Append(rowM);
                        rowIndex++;


                        int i = 0, k = 0;
                        uint evenColor = 7U;
                        uint oddColor = 9U;
                        uint cellColor = evenColor;

                        int? totalB = 0;
                        foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
                        {
                            if (pointEntry.Dimensions[2].Value.Equals("Surat")) { }
                            else
                            { 
                                cellColor = ((i % 2 == 0) ? evenColor : oddColor); i++;
                                Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                                //mergeCells.Append(GetMergeCell("B" + rowIndex + ":C" + rowIndex));
                                row.Append(GetTableValueCell((Convert.ToInt32(pointEntry.Dimensions[0].Value)).ToString("####-##-##"), "C" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[1].Value + "," + pointEntry.Dimensions[2].Value, "D" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[4].Value, "E" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[3].Value, "F" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Metrics[0].Value, "G" + rowIndex, cellColor, false));
                                totalB = totalB + Convert.ToInt32(pointEntry.Metrics[0].Value);

                                sheetData.Append(row);
                                rowIndex++;
                            

                            }

                        }
                        Row row1 = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                        mergeCells.Append(GetMergeCell("C" + rowIndex + ":D" + rowIndex + ":E" + rowIndex + ":F" + rowIndex));
                        row1.Append(GetTableValueCell("Total Visitors", "C" + rowIndex , cellColor, false));
                        row1.Append(GetTableValueCell("", "D" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell("", "E" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell("", "F" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell(totalB.ToString(), "G" + rowIndex, cellColor, false));
                        sheetData.Append(row1);
                        rowIndex++;
                    

                    ws.Append(sheetData);
                    wsp.Worksheet = ws;

                    string image1path = HttpContext.Current.Server.MapPath("~/ReportContent/Svg.Png");
                    InsertImage(ws, 6500000, 1500000, image1path);

                    wsp.Worksheet.Save();
                    Sheets sheets = new Sheets();
                    Sheet sheet = new Sheet();
                    sheet.Name = "Sheet1";
                    sheet.SheetId = 20;
                    sheet.Id = wbp.GetIdOfPart(wsp);
                    sheets.Append(sheet);
                    wb.Append(fv);
                    wb.Append(sheets);

                    xl.WorkbookPart.Workbook = wb;
                    xl.WorkbookPart.Workbook.Save();
                    xl.Close();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
               
            }
        }
        catch (Exception e)
        {
            return false;
        }
        }

        public bool CreatePackageForReportB(string filePath, AtomEntryCollection entriesCountry, string title, string startdate, string enddate)
        {
            try
            {
                /* if (System.IO.File.Exists(filePath))
                 {
                     System.IO.File.Delete(filePath);
                     System.Threading.Thread.Sleep(1000);
                 }*/

                using (SpreadsheetDocument xl = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                //using (SpreadsheetDocument package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook, false))
                {
                    try
                    {


                        WorkbookPart wbp = xl.AddWorkbookPart();
                        WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();

                        Workbook wb = new Workbook();
                        FileVersion fv = new FileVersion();
                        fv.ApplicationName = "Microsoft Office Excel";
                        Worksheet ws = new Worksheet();
                        SheetData sheetData = new SheetData();



                        WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
                        /*wbsp.Stylesheet = CreateStylesheet();*/

                        Stylesheet stylesheet = new Stylesheet();
                        stylesheet.Append(GetReportNuberingFormat());
                        stylesheet.Append(GetReportFonts());
                        stylesheet.Append(GetReportFills());
                        stylesheet.Append(GetReportBorders());
                        stylesheet.Append(GetReportStyleFormats());
                        stylesheet.Append(GetReportFormats());

                        wbsp.Stylesheet = stylesheet;
                        wbsp.Stylesheet.Save();




                        //SheetData sheetData = new SheetData();
                        MergeCells mergeCells = new MergeCells();

                        sheetData.Append(GetTitelCaptionRow("NHP REPORT From " + startdate + " To " + enddate, "B2", 2U));
                        uint rowIndex = 6U;
                        sheetData.Append(GetSubTitelValueRow(title, "C" + rowIndex, rowIndex));
                        rowIndex++;
                        //sheetData.Append(GetTableHeaderRow(rowIndex+1));

                        Row rowM = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:11" } };
                        Cell cellM = new Cell() { CellReference = "C" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Action");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "D" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Document");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "E" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Person");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "F" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Total Visitors");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        sheetData.Append(rowM);
                        rowIndex++;


                        int i = 0, k = 0;
                        uint evenColor = 7U;
                        uint oddColor = 9U;
                        uint cellColor = evenColor;

                        int? totalB = 0;
                        foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
                        {
                            if (pointEntry.Dimensions[3].Value.Equals("Surat")) { }
                            else
                            {
                                cellColor = ((i % 2 == 0) ? evenColor : oddColor); i++;
                                Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                                //mergeCells.Append(GetMergeCell("B" + rowIndex + ":C" + rowIndex));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[2].Value, "C" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[0].Value , "D" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[1].Value, "E" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Metrics[0].Value, "F" + rowIndex, cellColor, false));
                                totalB = totalB + Convert.ToInt32(pointEntry.Metrics[0].Value);

                                sheetData.Append(row);
                                rowIndex++;


                            }

                        }
                        Row row1 = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                        mergeCells.Append(GetMergeCell("C" + rowIndex + ":D" + rowIndex + ":E" + rowIndex ));
                        row1.Append(GetTableValueCell("Total Visitors", "C" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell("", "D" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell("", "E" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell(totalB.ToString(), "F" + rowIndex, cellColor, false));
                        sheetData.Append(row1);
                        rowIndex++;


                        ws.Append(sheetData);
                        wsp.Worksheet = ws;

                        string image1path = HttpContext.Current.Server.MapPath("~/ReportContent/Svg.Png");
                        InsertImage(ws, 6500000, 1500000, image1path);

                        wsp.Worksheet.Save();
                        Sheets sheets = new Sheets();
                        Sheet sheet = new Sheet();
                        sheet.Name = "Sheet1";
                        sheet.SheetId = 20;
                        sheet.Id = wbp.GetIdOfPart(wsp);
                        sheets.Append(sheet);
                        wb.Append(fv);
                        wb.Append(sheets);

                        xl.WorkbookPart.Workbook = wb;
                        xl.WorkbookPart.Workbook.Save();
                        xl.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CreatePackageForReportC(string filePath, AtomEntryCollection entriesCountry, string title, string startdate, string enddate)
        {
            try
            {
                /* if (System.IO.File.Exists(filePath))
                 {
                     System.IO.File.Delete(filePath);
                     System.Threading.Thread.Sleep(1000);
                 }*/

                using (SpreadsheetDocument xl = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                //using (SpreadsheetDocument package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook, false))
                {
                    try
                    {


                        WorkbookPart wbp = xl.AddWorkbookPart();
                        WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();

                        Workbook wb = new Workbook();
                        FileVersion fv = new FileVersion();
                        fv.ApplicationName = "Microsoft Office Excel";
                        Worksheet ws = new Worksheet();
                        SheetData sheetData = new SheetData();



                        WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
                        /*wbsp.Stylesheet = CreateStylesheet();*/

                        Stylesheet stylesheet = new Stylesheet();
                        stylesheet.Append(GetReportNuberingFormat());
                        stylesheet.Append(GetReportFonts());
                        stylesheet.Append(GetReportFills());
                        stylesheet.Append(GetReportBorders());
                        stylesheet.Append(GetReportStyleFormats());
                        stylesheet.Append(GetReportFormats());

                        wbsp.Stylesheet = stylesheet;
                        wbsp.Stylesheet.Save();




                        //SheetData sheetData = new SheetData();
                        MergeCells mergeCells = new MergeCells();

                        sheetData.Append(GetTitelCaptionRow("NHP REPORT From " + startdate + " To " + enddate, "B2", 2U));
                        uint rowIndex = 6U;
                        sheetData.Append(GetSubTitelValueRow(title, "C" + rowIndex, rowIndex));
                        rowIndex++;
                        //sheetData.Append(GetTableHeaderRow(rowIndex+1));

                        Row rowM = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:11" } };
                        Cell cellM = new Cell() { CellReference = "C" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Action");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "D" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Document");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "E" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Person");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "F" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Total Visitors");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "G" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Unique Visitors");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        sheetData.Append(rowM);
                        rowIndex++;


                        int i = 0, k = 0;
                        uint evenColor = 7U;
                        uint oddColor = 9U;
                        uint cellColor = evenColor;

                        int? totalB = 0;
                        int? totalB1 = 0;
                        foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
                        {
                            if (pointEntry.Dimensions[3].Value.Equals("Surat")) { }
                            else
                            {
                                cellColor = ((i % 2 == 0) ? evenColor : oddColor); i++;
                                Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                                //mergeCells.Append(GetMergeCell("B" + rowIndex + ":C" + rowIndex));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[0].Value, "C" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[1].Value, "D" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[2].Value, "E" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Metrics[1].Value, "F" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Metrics[2].Value, "G" + rowIndex, cellColor, false));
                                totalB = totalB + Convert.ToInt32(pointEntry.Metrics[1].Value);
                                totalB1 = totalB1 + Convert.ToInt32(pointEntry.Metrics[2].Value);
                                sheetData.Append(row);
                                rowIndex++;


                            }

                        }
                        Row row1 = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                        mergeCells.Append(GetMergeCell("C" + rowIndex + ":D" + rowIndex + ":E" + rowIndex));
                        row1.Append(GetTableValueCell("Total", "C" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell("", "D" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell("", "E" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell(totalB.ToString(), "F" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell(totalB1.ToString(), "G" + rowIndex, cellColor, false));
                        sheetData.Append(row1);
                        rowIndex++;


                        ws.Append(sheetData);
                        wsp.Worksheet = ws;

                        string image1path = HttpContext.Current.Server.MapPath("~/ReportContent/Svg.Png");
                        InsertImage(ws, 6500000, 1500000, image1path);

                        wsp.Worksheet.Save();
                        Sheets sheets = new Sheets();
                        Sheet sheet = new Sheet();
                        sheet.Name = "Sheet1";
                        sheet.SheetId = 20;
                        sheet.Id = wbp.GetIdOfPart(wsp);
                        sheets.Append(sheet);
                        wb.Append(fv);
                        wb.Append(sheets);

                        xl.WorkbookPart.Workbook = wb;
                        xl.WorkbookPart.Workbook.Save();
                        xl.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CreatePackageForReportD(string filePath, AtomEntryCollection entriesCountry, string title, string startdate, string enddate)
        {
            try
            {
                /* if (System.IO.File.Exists(filePath))
                 {
                     System.IO.File.Delete(filePath);
                     System.Threading.Thread.Sleep(1000);
                 }*/

                using (SpreadsheetDocument xl = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                //using (SpreadsheetDocument package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook, false))
                {
                    try
                    {


                        WorkbookPart wbp = xl.AddWorkbookPart();
                        WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();

                        Workbook wb = new Workbook();
                        FileVersion fv = new FileVersion();
                        fv.ApplicationName = "Microsoft Office Excel";
                        Worksheet ws = new Worksheet();
                        SheetData sheetData = new SheetData();



                        WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
                        /*wbsp.Stylesheet = CreateStylesheet();*/

                        Stylesheet stylesheet = new Stylesheet();
                        stylesheet.Append(GetReportNuberingFormat());
                        stylesheet.Append(GetReportFonts());
                        stylesheet.Append(GetReportFills());
                        stylesheet.Append(GetReportBorders());
                        stylesheet.Append(GetReportStyleFormats());
                        stylesheet.Append(GetReportFormats());

                        wbsp.Stylesheet = stylesheet;
                        wbsp.Stylesheet.Save();




                        //SheetData sheetData = new SheetData();
                        MergeCells mergeCells = new MergeCells();

                        sheetData.Append(GetTitelCaptionRow("NHP REPORT From " + startdate + " To " + enddate, "B2", 2U));
                        uint rowIndex = 6U;
                        sheetData.Append(GetSubTitelValueRow(title, "C" + rowIndex, rowIndex));
                        rowIndex++;
                        //sheetData.Append(GetTableHeaderRow(rowIndex+1));

                        Row rowM = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:11" } };
                        Cell cellM = new Cell() { CellReference = "C" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Device Name");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "D" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Version");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        cellM = new Cell() { CellReference = "E" + rowIndex, StyleIndex = (UInt32Value)6U };
                        cellM.CellValue = new CellValue("Total Visitors");
                        cellM.DataType = new EnumValue<CellValues>(CellValues.String);
                        rowM.Append(cellM);

                        sheetData.Append(rowM);
                        rowIndex++;


                        int i = 0, k = 0;
                        uint evenColor = 7U;
                        uint oddColor = 9U;
                        uint cellColor = evenColor;

                        int? totalB = 0;
                        foreach (Google.GData.Analytics.DataEntry pointEntry in entriesCountry)
                        {
                            //if (pointEntry.Dimensions[2].Value.Equals("Surat")) { }
                            //else
                            //{
                                cellColor = ((i % 2 == 0) ? evenColor : oddColor); i++;
                                Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                                //mergeCells.Append(GetMergeCell("B" + rowIndex + ":C" + rowIndex));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[0].Value, "C" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Dimensions[1].Value, "D" + rowIndex, cellColor, false));
                                row.Append(GetTableValueCell(pointEntry.Metrics[0].Value, "E" + rowIndex, cellColor, false));
                                totalB = totalB + Convert.ToInt32(pointEntry.Metrics[0].Value);
                                sheetData.Append(row);
                                rowIndex++;


                            //}

                        }
                        Row row1 = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" } };
                        mergeCells.Append(GetMergeCell("C" + rowIndex + ":D" + rowIndex ));
                        row1.Append(GetTableValueCell("Total", "C" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell("", "D" + rowIndex, cellColor, false));
                        row1.Append(GetTableValueCell(totalB.ToString(), "E" + rowIndex, cellColor, false));
                        sheetData.Append(row1);
                        rowIndex++;


                        ws.Append(sheetData);
                        wsp.Worksheet = ws;

                        string image1path = HttpContext.Current.Server.MapPath("~/ReportContent/Svg.Png");
                        InsertImage(ws, 2500000, 1000000, image1path);

                        wsp.Worksheet.Save();
                        Sheets sheets = new Sheets();
                        Sheet sheet = new Sheet();
                        sheet.Name = "Sheet1";
                        sheet.SheetId = 20;
                        sheet.Id = wbp.GetIdOfPart(wsp);
                        sheets.Append(sheet);
                        wb.Append(fv);
                        wb.Append(sheets);

                        xl.WorkbookPart.Workbook = wb;
                        xl.WorkbookPart.Workbook.Save();
                        xl.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static Stylesheet CreateStylesheet()
        {
            Stylesheet ss = new Stylesheet();

            Fonts fts = new Fonts();
            DocumentFormat.OpenXml.Spreadsheet.Font ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName ftn = new FontName();
            ftn.Val = StringValue.FromString("Calibri");
            FontSize ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(11);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            ftn = new FontName();
            ftn.Val = StringValue.FromString("Palatino Linotype");
            ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(18);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            fts.Count = UInt32Value.FromUInt32((uint)fts.ChildElements.Count);

            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Solid;
            patternFill.ForegroundColor = new ForegroundColor();
            patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00ff9728");
            patternFill.BackgroundColor = new BackgroundColor();
            patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);

            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);

            border = new Border();
            border.LeftBorder = new LeftBorder();
            border.LeftBorder.Style = BorderStyleValues.Thin;
            border.RightBorder = new RightBorder();
            border.RightBorder.Style = BorderStyleValues.Thin;
            border.TopBorder = new TopBorder();
            border.TopBorder.Style = BorderStyleValues.Thin;
            border.BottomBorder = new BottomBorder();
            border.BottomBorder.Style = BorderStyleValues.Thin;
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);
            borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);

            CellStyleFormats csfs = new CellStyleFormats();
            CellFormat cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);
            csfs.Count = UInt32Value.FromUInt32((uint)csfs.ChildElements.Count);

            uint iExcelIndex = 164;
            NumberingFormats nfs = new NumberingFormats();
            CellFormats cfs = new CellFormats();

            cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);

            NumberingFormat nfDateTime = new NumberingFormat();
            nfDateTime.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDateTime.FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss");
            nfs.Append(nfDateTime);

            NumberingFormat nf4decimal = new NumberingFormat();
            nf4decimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nf4decimal.FormatCode = StringValue.FromString("#,##0.0000");
            nfs.Append(nf4decimal);

            // #,##0.00 is also Excel style index 4
            NumberingFormat nf2decimal = new NumberingFormat();
            nf2decimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nf2decimal.FormatCode = StringValue.FromString("#,##0.00");
            nfs.Append(nf2decimal);

            // @ is also Excel style index 49
            NumberingFormat nfForcedText = new NumberingFormat();
            nfForcedText.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfForcedText.FormatCode = StringValue.FromString("@");
            nfs.Append(nfForcedText);

            // index 1
            cf = new CellFormat();
            cf.NumberFormatId = nfDateTime.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 2
            cf = new CellFormat();
            cf.NumberFormatId = nf4decimal.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 3
            cf = new CellFormat();
            cf.NumberFormatId = nf2decimal.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 4
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 5
            // Header text
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 1;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 6
            // column text
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 1;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 7
            // coloured 2 decimal text
            cf = new CellFormat();
            cf.NumberFormatId = nf2decimal.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 2;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 8
            // coloured column text
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 2;
            cf.BorderId = 1;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            nfs.Count = UInt32Value.FromUInt32((uint)nfs.ChildElements.Count);
            cfs.Count = UInt32Value.FromUInt32((uint)cfs.ChildElements.Count);

            ss.Append(nfs);
            ss.Append(fts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(csfs);
            ss.Append(cfs);

            CellStyles css = new CellStyles();
            CellStyle cs = new CellStyle();
            cs.Name = StringValue.FromString("Normal");
            cs.FormatId = 0;
            cs.BuiltinId = 0;
            css.Append(cs);
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
            ss.Append(css);

            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;
            ss.Append(dfs);

            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = StringValue.FromString("TableStyleMedium9");
            tss.DefaultPivotStyle = StringValue.FromString("PivotStyleLight16");
            ss.Append(tss);

            return ss;
        }

        protected static void InsertImage(Worksheet ws, long x, long y, long? width, long? height, string sImagePath)
        {
            try
            {
                WorksheetPart wsp = ws.WorksheetPart;
                DrawingsPart dp;
                ImagePart imgp;
                WorksheetDrawing wsd;

                ImagePartType ipt;
                switch (sImagePath.Substring(sImagePath.LastIndexOf('.') + 1).ToLower())
                {
                    case "png":
                        ipt = ImagePartType.Png;
                        break;
                    case "jpg":
                    case "jpeg":
                        ipt = ImagePartType.Jpeg;
                        break;
                    case "gif":
                        ipt = ImagePartType.Gif;
                        break;
                    default:
                        return;
                }

                if (wsp.DrawingsPart == null)
                {
                    //----- no drawing part exists, add a new one

                    dp = wsp.AddNewPart<DrawingsPart>();
                    imgp = dp.AddImagePart(ipt, wsp.GetIdOfPart(dp));
                    wsd = new WorksheetDrawing();
                }
                else
                {
                    //----- use existing drawing part

                    dp = wsp.DrawingsPart;
                    imgp = dp.AddImagePart(ipt);
                    dp.CreateRelationshipToPart(imgp);
                    wsd = dp.WorksheetDrawing;
                }

                using (FileStream fs = new FileStream(sImagePath, FileMode.Open))
                {
                    imgp.FeedData(fs);
                }

                int imageNumber = dp.ImageParts.Count<ImagePart>();
                if (imageNumber == 1)
                {
                    Drawing drawing = new Drawing();
                    drawing.Id = dp.GetIdOfPart(imgp);
                    ws.Append(drawing);
                }

                NonVisualDrawingProperties nvdp = new NonVisualDrawingProperties();
                nvdp.Id = new UInt32Value((uint)(1024 + imageNumber));
                nvdp.Name = "Picture " + imageNumber.ToString();
                nvdp.Description = "";
                DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
                picLocks.NoChangeAspect = true;
                picLocks.NoChangeArrowheads = true;
                NonVisualPictureDrawingProperties nvpdp = new NonVisualPictureDrawingProperties();
                nvpdp.PictureLocks = picLocks;
                NonVisualPictureProperties nvpp = new NonVisualPictureProperties();
                nvpp.NonVisualDrawingProperties = nvdp;
                nvpp.NonVisualPictureDrawingProperties = nvpdp;

                DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
                stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

                BlipFill blipFill = new BlipFill();
                DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
                blip.Embed = dp.GetIdOfPart(imgp);
                blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print;
                blipFill.Blip = blip;
                blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
                blipFill.Append(stretch);

                DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
                DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
                offset.X = 0;
                offset.Y = 0;
                t2d.Offset = offset;
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(sImagePath);

                DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();

                if (width == null)
                    extents.Cx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
                else
                    extents.Cx = width;

                if (height == null)
                    extents.Cy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
                else
                    extents.Cy = height;

                bm.Dispose();
                t2d.Extents = extents;
                ShapeProperties sp = new ShapeProperties();
                sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
                sp.Transform2D = t2d;
                DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
                prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
                prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
                sp.Append(prstGeom);
                sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

                DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
                picture.NonVisualPictureProperties = nvpp;
                picture.BlipFill = blipFill;
                picture.ShapeProperties = sp;

                Position pos = new Position();
                pos.X = x;
                pos.Y = y;
                Extent ext = new Extent();
                ext.Cx = extents.Cx;
                ext.Cy = extents.Cy;
                AbsoluteAnchor anchor = new AbsoluteAnchor();
                anchor.Position = pos;
                anchor.Extent = ext;
                anchor.Append(picture);
                anchor.Append(new ClientData());
                wsd.Append(anchor);
                wsd.Save(dp);
            }
            catch (Exception ex)
            {
                throw ex; // or do something more interesting if you want
            }
        }

        protected static void InsertImage(Worksheet ws, long x, long y, string sImagePath)
        {
            InsertImage(ws, x, y, null, null, sImagePath);
        }

        #region "reports Content"
        private Row GetTitelCaptionRow(string cellValue, string cellRefrence, uint rowIndex)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "2:2" }, Height = 35.25D };
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)2U };
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            return row;
        }
        private Row GetTitleValueRow(string cellValue, string cellRefrence, uint rowIndex)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "2:2" }, Height = 26.25D };
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)1U };
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            return row;
        }
        private Row GetSubTitelCaptionRow(string cellValue, string cellRefrence, uint rowIndex)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "2:2" }, Height = 18D };
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)3U };
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            return row;
        }
        private Row GetSubTitelValueRow(string cellValue, string cellRefrence, uint rowIndex)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "2:2" } };
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)4U };
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            return row;
        }
        private Row GetLargeValueRow(string cellValue, string cellRefrence, uint rowIndex, double height)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" }, Height = height, CustomHeight = true };
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)5U };
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            return row;
        }
        private Row GetValueRow(string cellValue, string cellRefrence, uint rowIndex)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:11" } };
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)0U };
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            return row;
        }
        private Row GetTableHeaderRow(uint rowIndex)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:11" } };
            Cell cell = new Cell() { CellReference = "C" + rowIndex, StyleIndex = (UInt32Value)6U };
            cell.CellValue = new CellValue("#");
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            cell = new Cell() { CellReference = "D" + rowIndex, StyleIndex = (UInt32Value)6U };
            cell.CellValue = new CellValue("Brands");
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            cell = new Cell() { CellReference = "E" + rowIndex, StyleIndex = (UInt32Value)6U };
            cell.CellValue = new CellValue("Percentage(%)");
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);


            return row;
        }

        private Cell GetTableValueCell(string cellValue, string cellRefrence, uint styleIndex, bool isNumeric)
        {
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)styleIndex };
            cell.CellValue = new CellValue(cellValue);
            if (isNumeric)
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            else
                cell.DataType = new EnumValue<CellValues>(CellValues.String);

            return cell;
        }
        private Row GetCopyRightRow(string cellValue, string cellRefrence, uint rowIndex)
        {
            Row row = new Row() { RowIndex = (UInt32Value)rowIndex, Spans = new ListValue<StringValue>() { InnerText = "1:13" }, Height = 46.5D, CustomHeight = true };
            Cell cell = new Cell() { CellReference = cellRefrence, StyleIndex = (UInt32Value)14U };
            cell.CellValue = new CellValue(cellValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            row.Append(cell);

            return row;
        }
        private MergeCell GetMergeCell(string cellRange)
        {
            MergeCell mergeCel = new MergeCell() { Reference = cellRange };
            return mergeCel;
        }

        #endregion

        #region "Style"
        private void GenerateWorkbookStylesContent(WorkbookStylesPart workbookStylesPart)
        {
            Stylesheet stylesheet = new Stylesheet();
            stylesheet.Append(GetReportNuberingFormat());
            stylesheet.Append(GetReportFonts());
            stylesheet.Append(GetReportFills());
            stylesheet.Append(GetReportBorders());
            stylesheet.Append(GetReportStyleFormats());
            stylesheet.Append(GetReportFormats());

            workbookStylesPart.Stylesheet = stylesheet;
            // workbookStylesPart.Stylesheet = GenerateStyleSheet();
        }

        private NumberingFormats GetReportNuberingFormat()
        {
            NumberingFormats numberingFormats = new NumberingFormats() { Count = (UInt32Value)1U };
            NumberingFormat numberingFormat = new NumberingFormat() { NumberFormatId = (UInt32Value)8U, FormatCode = "\"$\"#,##0.00_);[Red]\\(\"$\"#,##0.00\\)" };
            numberingFormats.Append(numberingFormat);

            return numberingFormats;
        }

        private Fonts GetReportFonts()
        {
            Fonts fonts = new Fonts() { Count = (UInt32Value)8U };
            fonts.Append(GetFont(11D, false, false));
            fonts.Append(GetFont(20D, false, true));
            fonts.Append(GetFont(28D, false, true));
            fonts.Append(GetFont(14D, false, true));
            fonts.Append(GetFont(11D, false, true));
            fonts.Append(GetFont(11D, false, false));
            fonts.Append(GetFont(11D, true, false));
            fonts.Append(GetFont(10D, false, true));

            return fonts;
        }


        private Font GetFont(DoubleValue size, bool isReplacement, bool isBold)
        {
            Font font = new Font();
            font.Append(new FontName() { Val = "Arial" });
            font.Append(new FontSize() { Val = size });
            if (isReplacement) font.Color = new Color() { Rgb = "FFCB0000" };
            if (isBold) font.Append(new Bold());

            return font;
        }


        private Fills GetReportFills()
        {
            Fills fills = new Fills() { Count = (UInt32Value)6U };
            fills.Append(GetFill(PatternValues.None, string.Empty, string.Empty));
            fills.Append(GetFill(PatternValues.Gray125, string.Empty, string.Empty));
            fills.Append(GetFill(PatternValues.Solid, "FF488ECA", string.Empty));
            fills.Append(GetFill(PatternValues.Solid, "FFE6E6E6", string.Empty));
            fills.Append(GetFill(PatternValues.Solid, "FFD9D9D9", string.Empty));
            fills.Append(GetFill(PatternValues.Solid, "FFFFD1D1", string.Empty));

            return fills;
        }
        private Fill GetFill(PatternValues pattern, string foreColor, string backColor)
        {
            Fill fill = new Fill();
            PatternFill patternFill = new PatternFill() { PatternType = pattern };
            if (!string.IsNullOrEmpty(foreColor)) patternFill.Append(new ForegroundColor() { Rgb = foreColor });
            if (!string.IsNullOrEmpty(backColor)) patternFill.Append(new BackgroundColor() { Rgb = backColor });
            fill.Append(patternFill);

            return fill;
        }

        private Borders GetReportBorders()
        {
            Borders borders = new Borders() { Count = (UInt32Value)2U };
            borders.Append(GetBorder(false));
            borders.Append(GetBorder(true));

            return borders;
        }
        private Border GetBorder(bool applyStyle)
        {
            Border border = new Border();
            border.Append(applyStyle ? new LeftBorder() { Style = BorderStyleValues.Thin } : new LeftBorder());
            border.Append(applyStyle ? new RightBorder() { Style = BorderStyleValues.Thin } : new RightBorder());
            border.Append(applyStyle ? new TopBorder() { Style = BorderStyleValues.Thin } : new TopBorder());
            border.Append(applyStyle ? new BottomBorder() { Style = BorderStyleValues.Thin } : new BottomBorder());

            border.Append(new DiagonalBorder());

            return border;
        }

        private CellStyleFormats GetReportStyleFormats()
        {
            CellStyleFormats cellStyleFormats = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)4U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };
            cellStyleFormats.Append(cellFormat);
            return cellStyleFormats;
        }

        private CellFormats GetReportFormats()
        {
            CellFormats cellFormats = new CellFormats() { Count = (UInt32Value)15U };
            cellFormats.Append(GetCellFormat(0U, 0U, 0U, false, HorizontalAlignmentValues.Left));//Normal
            cellFormats.Append(GetCellFormat(1U, 0U, 0U, false, HorizontalAlignmentValues.Left));//Name
            cellFormats.Append(GetCellFormat(2U, 0U, 0U, false, HorizontalAlignmentValues.Left));//Title
            cellFormats.Append(GetCellFormat(3U, 0U, 0U, false, HorizontalAlignmentValues.Left));//ac no
            cellFormats.Append(GetCellFormat(4U, 0U, 0U, false, HorizontalAlignmentValues.Left));//value
            cellFormats.Append(GetCellFormat(0U, 0U, 0U, true, HorizontalAlignmentValues.Left));//Normal wrap
            cellFormats.Append(GetCellFormat(0U, 2U, 1U, true, HorizontalAlignmentValues.Center));//Horizontal center align wrap border

            cellFormats.Append(GetCellFormat(5U, 3U, 1U, true, HorizontalAlignmentValues.Left));//Table left light gray
            cellFormats.Append(GetCellFormat(5U, 3U, 1U, true, HorizontalAlignmentValues.Right));//Table right light gray
            cellFormats.Append(GetCellFormat(5U, 4U, 1U, true, HorizontalAlignmentValues.Left));//Table left drak gray
            cellFormats.Append(GetCellFormat(5U, 4U, 1U, true, HorizontalAlignmentValues.Right));//Table right drak gray
            cellFormats.Append(GetCellFormat(6U, 5U, 1U, true, HorizontalAlignmentValues.Left));//Table left replace
            cellFormats.Append(GetCellFormat(6U, 5U, 1U, true, HorizontalAlignmentValues.Right));//Table right replace
            cellFormats.Append(GetCellFormat(7U, 0U, 1U, true, HorizontalAlignmentValues.Right));//Table total 

            cellFormats.Append(GetCellFormat(7U, 0U, 0U, true, HorizontalAlignmentValues.Center));//Table right replace

            return cellFormats;
        }
        private CellFormat GetCellFormat(uint font, uint fill, uint border, bool wrapText, HorizontalAlignmentValues horizontalPosition)
        {
            CellFormat cellFormat = new CellFormat()
            {
                NumberFormatId = (UInt32Value)0U,
                FontId = (UInt32Value)font,
                FillId = (UInt32Value)fill,
                BorderId = (UInt32Value)border,
                FormatId = (UInt32Value)0U,
                ApplyFont = true
            };

            Alignment alignment = new Alignment() { Horizontal = horizontalPosition, Vertical = VerticalAlignmentValues.Top };
            if (wrapText) alignment.WrapText = true;
            cellFormat.Append(alignment);

            return cellFormat;
        }
        
        #endregion
    }
}
