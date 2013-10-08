using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.Ressources.ExcelClasses
{
    class ResourceTranslation
    {
        readonly WorkSheetktResourceTranslation _rTranslation;
        readonly SharedRessources _sharedResources;
        readonly WorkspaceViewModel _workspaceVM;

        public ResourceTranslation()
        {
            _rTranslation = WorkSheetktResourceTranslation.Instance;
            _sharedResources = SharedRessources.Instance;
            _workspaceVM = WorkspaceViewModel.Instance;
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
                SheetId = 5U,
                Name = _rTranslation.SheetName
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

            string header1 = "LanguageID";
            int index1 = _sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = _sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString(CultureInfo.InvariantCulture));
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header2 = "ResourceID";
            int index2 = _sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = _sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString(CultureInfo.InvariantCulture));
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header3 = "TranslationText";
            int index3 = _sharedResources.InsertSharedStringItem(header3, shareStringPart);
            Cell headerCell3 = _sharedResources.InsertCellInWorksheet("C", 1, worksheetPart);
            headerCell3.CellValue = new CellValue(index3.ToString(CultureInfo.InvariantCulture));
            headerCell3.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            #endregion

            #region Insert original ktResourceTranslation items into the excel document

            int columnCount = 1;
            uint rowCount = 2;

            foreach (ktResourceTranslation resTrans in _rTranslation.ktResourceTranslationList)
            {
                if (columnCount >= 3)
                {
                    columnCount = 1;
                }

                string text1 = resTrans.LanguageID;
                Cell cell1 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                cell1.CellValue = new CellValue(text1);
                cell1.DataType = CellValues.Number;
                columnCount++;

                string text2 = resTrans.ResourceID;
                Cell cell2 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                cell2.CellValue = new CellValue(text2);
                cell2.DataType = CellValues.Number;
                columnCount++;

                string text3 = resTrans.TranslationText;
                Cell cell3 = _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                cell3.CellValue = new CellValue(text3);
                cell3.DataType = CellValues.String;

                rowCount++;
            }

            #endregion

            #region Insert new ktResourceTranslation items into excel

            foreach (PageType page in _workspaceVM.PageList)
            {
                foreach (GroupTypeOrder gtOrder in page.Groups)
                {
                    if (gtOrder.GroupTypeID.Equals("58") || gtOrder.GroupTypeID.Equals("60"))
                    {
                        break;
                    }

                    if (!_rTranslation.ktResourceTranslationList.Any(x => x.ResourceID.Equals(gtOrder.Group.ResourceTypeID)))
                    {
                        if (columnCount >= 3)
                        {
                            columnCount = 1;
                        }

                        string text1 = "1";     // <- English translation
                        Cell cell1 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1);
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        string text2 = gtOrder.Group.ResourceID;
                        Cell cell2 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell2.CellValue = new CellValue(text2);
                        cell2.DataType = CellValues.Number;
                        columnCount++;

                        string text3 = gtOrder.Group.EnglishTranslationText;
                        Cell cell3 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3);
                        cell3.DataType = CellValues.String;

                        rowCount++;
                        columnCount = 1;

                        string text4 = "2";     // <- Danish translation
                        Cell cell4 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell4.CellValue = new CellValue(text4);
                        cell4.DataType = CellValues.Number;
                        columnCount++;

                        string text5 = gtOrder.Group.ResourceTypeID;
                        Cell cell5 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell5.CellValue = new CellValue(text5);
                        cell5.DataType = CellValues.Number;
                        columnCount++;

                        string text6 = gtOrder.Group.DanishTranslationText;
                        Cell cell6 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell6.CellValue = new CellValue(text6);
                        cell6.DataType = CellValues.String;

                        rowCount++;
                    }
                }
            }

            #endregion

            worksheetPart.Worksheet.Save();
        }
    }
}
