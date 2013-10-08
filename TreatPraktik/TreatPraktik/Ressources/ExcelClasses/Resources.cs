using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.Ressources.ExcelClasses
{
    class Resources
    {
        readonly WorkSheetktResources _resources;
        readonly SharedRessources _sharedResources;
        readonly WorkspaceViewModel _workspaceVM;

        public Resources()
        {
            _resources = WorkSheetktResources.Instance;
            _sharedResources = new SharedRessources();
            _workspaceVM = WorkspaceViewModel.Instance;
        }

        public void CreateSheet(Sheets sheets, SpreadsheetDocument spreadsheetDocument, WorkbookPart workbookPart)
        {
            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 4U,
                Name = _resources.SheetName
            };
            sheets.Append(sheet);

            //Add cells to the sheet
            InsertTextIntoCells(spreadsheetDocument, worksheetPart);
        }

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

            string header1 = "ResourceID";
            int index1 = _sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = _sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString(CultureInfo.InvariantCulture));
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header2 = "ResourceTypeID";
            int index2 = _sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = _sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString(CultureInfo.InvariantCulture));
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header3 = "ResourceResxID";
            int index3 = _sharedResources.InsertSharedStringItem(header3, shareStringPart);
            Cell headerCell3 = _sharedResources.InsertCellInWorksheet("C", 1, worksheetPart);
            headerCell3.CellValue = new CellValue(index3.ToString(CultureInfo.InvariantCulture));
            headerCell3.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            #endregion

            #region Insert ktResources into the excel document

            int columnCount = 1;
            uint rowCount = 2;

            foreach (ktResources resource in _resources.ktResourceList)
            {
                if (columnCount >= 3)
                {
                    columnCount = 1;
                }

                string text1 = resource.ResourceID;
                Cell cell1 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                cell1.CellValue = new CellValue(text1);
                cell1.DataType = CellValues.Number;
                columnCount++;

                string text2 = resource.ResourceTypeID;
                Cell cell2 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                cell2.CellValue = new CellValue(text2);
                cell2.DataType = CellValues.Number;
                columnCount++;

                string text3 = resource.ResourceResxID;
                Cell cell3 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                cell3.CellValue = new CellValue(text3);
                cell3.DataType = CellValues.String;

                rowCount++;
            }

            #endregion

            #region Insert new ktResource items into excel

            foreach (PageType page in _workspaceVM.PageList)
            {
                foreach (GroupTypeOrder gtOrder in page.Groups)
                {
                    if (gtOrder.GroupTypeID.Equals("58") || gtOrder.GroupTypeID.Equals("60"))
                    {
                        break;
                    }

                    if (!_resources.ktResourceList.Any(x => x.ResourceResxID.Equals(gtOrder.Group.ResourceType)))
                    {
                        if (columnCount >= 3)
                        {
                            columnCount = 1;
                        }

                        string text1 = gtOrder.Group.ResourceID;
                        Cell cell1 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1);
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        string text2 = gtOrder.Group.ResourceTypeID;
                        Cell cell2 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell2.CellValue = new CellValue(text2);
                        cell2.DataType = CellValues.Number;
                        columnCount++;

                        string text3 = gtOrder.Group.ResourceType;
                        Cell cell3 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3);
                        cell3.DataType = CellValues.String;

                        rowCount++;
                    }
                }
            }

            #endregion

            worksheetPart.Worksheet.Save();
        }
    }
}
