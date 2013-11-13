using System;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.Ressources.ExcelClasses
{
    class ResourceType
    {
        readonly WorkSheetktResourceType _resourceType;
        readonly SharedRessources _sharedResources;
        readonly WorkspaceViewModel _workspaceVm;


        public ResourceType()
        {

            _resourceType = WorkSheetktResourceType.Instance;
            _sharedResources = SharedRessources.Instance;
            _workspaceVm = WorkspaceViewModel.Instance;
        }

        /// <summary>
        /// Creates the sheet
        /// </summary>
        /// <param name="sheets">The sheet for this item</param>
        /// <param name="spreadsheetDocument">The spreadsheet containing the sheets</param>
        /// <param name="workbookPart">The workbookpart associated with the spreadsheet</param>
        public void CreateSheet(Sheets sheets, SpreadsheetDocument spreadsheetDocument, WorkbookPart workbookPart)
        {
            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 2U,
                Name = _resourceType.SheetName
            };
            sheets.Append(sheet);

            //Add cells to the sheet
            InsertTextIntoCells(spreadsheetDocument, worksheetPart);
        }

        /// <summary>
        /// Creates the content of the shet (columns, rows, cells)
        /// </summary>
        /// <param name="spreadsheetDocument">The spreadsheet containing the sheets</param>
        /// <param name="worksheetPart">The worksheetpart for this item</param>
        private void InsertTextIntoCells(SpreadsheetDocument spreadsheetDocument, WorksheetPart worksheetPart)
        {
            // Get the SharedStringTablePart. If it does not exist, create a new one.
            SharedStringTablePart shareStringPart;
            if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Any())
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }

            #region Excel headers

            string header1 = "ResourceTypeID";
            int index1 = _sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = _sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString(CultureInfo.InvariantCulture));
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header2 = "ResourceType";
            int index2 = _sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = _sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString(CultureInfo.InvariantCulture));
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            #endregion

            #region insert the items from the temporary list into the ktUIOrder excel sheet

            int columnCount = 1;
            uint rowCount = 2;

            foreach (ktResourceType resourceType in _resourceType.ktResourceTypeList)
            {
                    if (columnCount >= 2)
                    {
                        columnCount = 1;
                    }

                    string text1 = resourceType.ResourceTypeID;
                    Cell cell1 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                    cell1.CellValue = new CellValue(text1);
                    cell1.DataType = CellValues.Number;
                    columnCount++;

                    string text2 = resourceType.ResourceType;
                    Cell cell2 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                    cell2.CellValue = new CellValue(text2);
                    cell2.DataType = CellValues.String;
                    columnCount++;

                    rowCount++;
            }

            #endregion

            worksheetPart.Worksheet.Save();
        }
    }
}
