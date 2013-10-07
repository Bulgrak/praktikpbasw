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
    class ExaminedGroup
    {
        readonly WorkSheetktExaminedGroup _examinedGroup;
        readonly SharedRessources _sharedResources;
        readonly WorkspaceViewModel _workspaceVm;

        public ExaminedGroup()
        {
            _examinedGroup = WorkSheetktExaminedGroup.Instance;
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
                SheetId = 1U,
                Name = _examinedGroup.SheetName
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

            const string header1 = "ID";
            int index1 = _sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = _sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString(CultureInfo.InvariantCulture));
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header2 = "GroupIdentifier";
            int index2 = _sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = _sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString(CultureInfo.InvariantCulture));
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header3 = "GroupType";
            int index3 = _sharedResources.InsertSharedStringItem(header3, shareStringPart);
            Cell headerCell3 = _sharedResources.InsertCellInWorksheet("C", 1, worksheetPart);
            headerCell3.CellValue = new CellValue(index3.ToString(CultureInfo.InvariantCulture));
            headerCell3.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header4 = "GroupExpandable";
            int index4 = _sharedResources.InsertSharedStringItem(header4, shareStringPart);
            Cell headerCell4 = _sharedResources.InsertCellInWorksheet("D", 1, worksheetPart);
            headerCell4.CellValue = new CellValue(index4.ToString(CultureInfo.InvariantCulture));
            headerCell4.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header5 = "Name";
            int index5 = _sharedResources.InsertSharedStringItem(header5, shareStringPart);
            Cell headerCell5 = _sharedResources.InsertCellInWorksheet("E", 1, worksheetPart);
            headerCell5.CellValue = new CellValue(index5.ToString(CultureInfo.InvariantCulture));
            headerCell5.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header6 = "Expanded";
            int index6 = _sharedResources.InsertSharedStringItem(header6, shareStringPart);
            Cell headerCell6 = _sharedResources.InsertCellInWorksheet("F", 1, worksheetPart);
            headerCell6.CellValue = new CellValue(index6.ToString(CultureInfo.InvariantCulture));
            headerCell6.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header7 = "DataQualityScore";
            int index7 = _sharedResources.InsertSharedStringItem(header7, shareStringPart);
            Cell headerCell7 = _sharedResources.InsertCellInWorksheet("G", 1, worksheetPart);
            headerCell7.CellValue = new CellValue(index7.ToString(CultureInfo.InvariantCulture));
            headerCell7.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header8 = "RequiredScore";
            int index8 = _sharedResources.InsertSharedStringItem(header8, shareStringPart);
            Cell headerCell8 = _sharedResources.InsertCellInWorksheet("H", 1, worksheetPart);
            headerCell8.CellValue = new CellValue(index8.ToString(CultureInfo.InvariantCulture));
            headerCell8.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            
            #endregion

            #region Insert original ktUIExaminedGroup items into excel

            int columnCount = 1;
            uint rowCount = 2;

            foreach (ktExaminedGroup exGroup in _examinedGroup.ExaminedGroupList)
            {
                if (columnCount >= 8)
                {
                    columnCount = 1;
                }

                string text1 = exGroup.ID;
                Cell cell1 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell1.CellValue = new CellValue(text1);
                cell1.DataType = CellValues.Number;
                columnCount++;

                string text2 = exGroup.GroupIdentifier;
                Cell cell2 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell2.CellValue = new CellValue(text2);
                cell2.DataType = CellValues.String;
                columnCount++;

                string text3 = exGroup.GroupType;
                Cell cell3 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell3.CellValue = new CellValue(text3);
                cell3.DataType = CellValues.String;
                columnCount++;

                string text4 = exGroup.GroupExpendable;
                Cell cell4 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell4.CellValue = new CellValue(text4.ToString(CultureInfo.InvariantCulture));
                cell4.DataType = CellValues.Number;
                columnCount++;

                string text5 = exGroup.Name;
                Cell cell5 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell5.CellValue = new CellValue(text5.ToString(CultureInfo.InvariantCulture));
                cell5.DataType = CellValues.String;
                columnCount++;

                string text6 = exGroup.Expanded;
                Cell cell6 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell6.CellValue = new CellValue(text6.ToString(CultureInfo.InvariantCulture));
                cell6.DataType = CellValues.Number;
                columnCount++;

                string text7 = exGroup.DataQualityScore;
                Cell cell7 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell7.CellValue = new CellValue(text7.ToString(CultureInfo.InvariantCulture));
                cell7.DataType = CellValues.Number;
                columnCount++;

                string text8 = exGroup.RequiredScore;
                Cell cell8 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell8.CellValue = new CellValue(text8.ToString(CultureInfo.InvariantCulture));
                cell8.DataType = CellValues.Number;

                rowCount++;
            }

            #endregion

            #region Insert new ktUIExaminedGroup items into excel

            foreach (PageType page in _workspaceVm.PageList)
            {
                foreach (GroupTypeOrder gtOrder in page.Groups)
                {
                    if (!_examinedGroup.ExaminedGroupList.Any(x => x.ID.Equals(gtOrder.GroupTypeID)))
                    {
                        if (columnCount >= 8)
                        {
                            columnCount = 1;
                        }

                        string text1 = gtOrder.GroupTypeID;
                        Cell cell1 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1);
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        string text2 = "NULL";
                        Cell cell2 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell2.CellValue = new CellValue(text2);
                        cell2.DataType = CellValues.String;
                        columnCount++;

                        string text3 = gtOrder.Group.ResourceType;
                        Cell cell3 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3);
                        cell3.DataType = CellValues.String;
                        columnCount++;

                        string text4 = "0";
                        Cell cell4 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell4.CellValue = new CellValue(text4.ToString(CultureInfo.InvariantCulture));
                        cell4.DataType = CellValues.Number;
                        columnCount++;

                        string text5 = "NULL";
                        Cell cell5 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell5.CellValue = new CellValue(text5.ToString(CultureInfo.InvariantCulture));
                        cell5.DataType = CellValues.String;
                        columnCount++;

                        string text6 = "1";
                        Cell cell6 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell6.CellValue = new CellValue(text6.ToString(CultureInfo.InvariantCulture));
                        cell6.DataType = CellValues.Number;
                        columnCount++;

                        string text7 = "0";
                        Cell cell7 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell7.CellValue = new CellValue(text7.ToString(CultureInfo.InvariantCulture));
                        cell7.DataType = CellValues.Number;
                        columnCount++;

                        string text8 = "0";
                        Cell cell8 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell8.CellValue = new CellValue(text8.ToString(CultureInfo.InvariantCulture));
                        cell8.DataType = CellValues.Number;

                        rowCount++;
                    }
                }
            }

            #endregion

            worksheetPart.Worksheet.Save();
        }
    }
}
