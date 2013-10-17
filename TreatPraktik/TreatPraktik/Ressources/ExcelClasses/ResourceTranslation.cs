using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        readonly WorkSheetktResources _resources;
        readonly SharedRessources _sharedResources;
        readonly WorkspaceViewModel _workspaceVM;

        readonly List<GroupTypeOrder> _tempList;

        public ResourceTranslation()
        {
            _rTranslation = WorkSheetktResourceTranslation.Instance;
            _resources = WorkSheetktResources.Instance;
            _sharedResources = SharedRessources.Instance;
            _workspaceVM = WorkspaceViewModel.Instance;

            _tempList = new List<GroupTypeOrder>();
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

            int columnCount = 1;
            uint rowCount = 2;

            #region Create list containing all the resource translations, but also the ResourceTypeID from ktResources

            var query = (from a in _rTranslation.ktResourceTranslationList
                join b in _resources.ktResourceList on a.ResourceID equals b.ResourceID
                select new
                {
                    a.TranslationText,
                    a.LanguageID,
                    a.ResourceID,
                    b.ResourceTypeID
                }).ToList();

            #endregion

            #region Create temporary list containing the groups needed to create the ktUIOrder excel sheet

            foreach (PageType page in _workspaceVM.PageList)
            {
                foreach (GroupTypeOrder group in page.Groups)
                {
                    if (group.GroupTypeID.Equals("58") || group.GroupTypeID.Equals("60"))
                    {
                        continue;
                    }

                    if (!_tempList.Any(x => x.GroupTypeID.Equals(group.GroupTypeID)))
                    {
                        _tempList.Add(group);
                    }
                }
            }

            #endregion

            #region Inserting data rows

            //Insert all from the original list + changes --> renaming of groups
            foreach (var qItem in query)
            {
                if (qItem.ResourceTypeID.Equals("1"))
                {
                    //Indsætter alle de grupper der ikke indgår i templisten under ResourceTypeID 1
                    if (!_tempList.Any(x => x.Group.ResourceID.Equals(qItem.ResourceID)))
                    {
                        if (columnCount >= 3)
                        {
                            columnCount = 1;
                        }
                        string text9 = qItem.LanguageID;
                        Cell cell9 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell9.CellValue = new CellValue(text9);
                        cell9.DataType = CellValues.Number;
                        columnCount++;

                        string text10 = qItem.ResourceID;
                        Cell cell10 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell10.CellValue = new CellValue(text10);
                        cell10.DataType = CellValues.Number;
                        columnCount++;

                        string text11 = qItem.TranslationText;
                        Cell cell11 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell11.CellValue = new CellValue(text11);
                        cell11.DataType = CellValues.String;

                        rowCount++;
                    }
                    //Indsætter de groups der er i templisten
                    else
                    {
                        var gtOrder =
                            (from a in _tempList
                                where a.Group.ResourceID.Equals(qItem.ResourceID)
                                select a).FirstOrDefault();

                        if (columnCount >= 3)
                        {
                            columnCount = 1;
                        }

                        string text1 = qItem.LanguageID;
                        Cell cell1 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1);
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        string text2 = gtOrder.Group.ResourceID;
                        Cell cell2 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell2.CellValue = new CellValue(text2);
                        cell2.DataType = CellValues.Number;
                        columnCount++;

                        string text3 = qItem.LanguageID.Equals("1") ? gtOrder.Group.EnglishTranslationText : gtOrder.Group.DanishTranslationText;
                        Cell cell3 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3);
                        cell3.DataType = CellValues.String;

                        rowCount++;
                    }
                }
                //Indsætter alle de andre translation informationer
                else
                {
                    if (columnCount >= 3)
                    {
                        columnCount = 1;
                    }

                    string text7 = qItem.LanguageID;
                    Cell cell7 =
                        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                            rowCount, worksheetPart);
                    cell7.CellValue = new CellValue(text7);
                    cell7.DataType = CellValues.Number;
                    columnCount++;

                    string text8 = qItem.ResourceID;
                    Cell cell8 =
                        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                            rowCount, worksheetPart);
                    cell8.CellValue = new CellValue(text8);
                    cell8.DataType = CellValues.Number;
                    columnCount++;

                    string text9 = qItem.TranslationText;
                    Cell cell9 =
                        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                            rowCount, worksheetPart);
                    cell9.CellValue = new CellValue(text9);
                    cell9.DataType = CellValues.String;

                    rowCount++;
                }
            }

            //Indsætter de groups der ikke indgår in den originale liste --> Ny oprettede grupper
            foreach (var order in _tempList)
            {
                if (!query.Any(x => x.ResourceID.Equals(order.Group.ResourceID)))
                {
                    for (int i = 1; i < 3; i++)
                    {
                        if (columnCount >= 3)
                        {
                            columnCount = 1;
                        }

                        string text1 = i.ToString();
                        Cell cell1 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1);
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        string text2 = order.Group.ResourceID;
                        Cell cell2 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell2.CellValue = new CellValue(text2);
                        cell2.DataType = CellValues.Number;
                        columnCount++;

                        string text3 = text1.Equals("1")
                            ? order.Group.EnglishTranslationText
                            : order.Group.DanishTranslationText;
                        Cell cell3 =
                            _sharedResources.InsertCellInWorksheet(
                                _sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3);
                        cell3.DataType = CellValues.String;

                        rowCount++;
                    }
                }
            }

            #endregion

            //Saves the excel sheet
            worksheetPart.Worksheet.Save();
        }
    }
}
