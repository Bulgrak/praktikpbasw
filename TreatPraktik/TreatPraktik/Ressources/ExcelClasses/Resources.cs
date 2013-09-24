using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreatPraktik.Model.WorkSheets;

namespace TreatPraktik.Ressources.ExcelClasses
{
    class Resources
    {
        WorkSheetktResources resources;
        SharedRessources sharedResources;

        public Resources()
        {
            resources = WorkSheetktResources.Instance;
            sharedResources = new SharedRessources();
        }

        public void CreateSheet(Sheets sheets, SpreadsheetDocument spreadsheetDocument, WorkbookPart workbookPart)
        {
            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = (UInt32Value)2U,
                Name = resources.SheetName
            };
            sheets.Append(sheet);

            //Add cells to the sheet
            InsertTextIntoCells(spreadsheetDocument, sheet, worksheetPart);
        }

        private void InsertTextIntoCells(SpreadsheetDocument spreadsheetDocument, Sheet sheet, WorksheetPart worksheetPart)
        {
            // Get the SharedStringTablePart. If it does not exist, create a new one.
            SharedStringTablePart shareStringPart;
            if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }

            string text = resources.ktResourceList[1].ResourceResxID;

            //Insert cell text into the SharedStringsTable, and get the index of the text
            int index = sharedResources.InsertSharedStringItem(text, shareStringPart);

            // Insert cell A1 into the new worksheet.
            Cell cell = sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);

            // Set the value of cell A1.
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            worksheetPart.Worksheet.Save();
        }
    }
}
