using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Linq;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.Ressources.ExcelClasses
{
    class GroupOrder
    {
        readonly WorkSheetktUIGroupOrder _groupOrder;
        readonly SharedRessources _sharedResources;
        readonly WorkspaceViewModel _workspaceVm;

        //private ObservableCollection<PageType> _pages;
        //private List<ktUIGroupOrder> _orders;

        public GroupOrder()
        {
            _groupOrder = WorkSheetktUIGroupOrder.Instance;
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
                SheetId = 3U,       // <-- Change for each sheet that is created
                Name = _groupOrder.SheetName
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

            const string header1 = "DepartmentID";
            int index1 = _sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = _sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString(CultureInfo.InvariantCulture));
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header2 = "PageTypeID";
            int index2 = _sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = _sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString(CultureInfo.InvariantCulture));
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header3 = "GroupTypeID";
            int index3 = _sharedResources.InsertSharedStringItem(header3, shareStringPart);
            Cell headerCell3 = _sharedResources.InsertCellInWorksheet("C", 1, worksheetPart);
            headerCell3.CellValue = new CellValue(index3.ToString(CultureInfo.InvariantCulture));
            headerCell3.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header4 = "GroupOrder";
            int index4 = _sharedResources.InsertSharedStringItem(header4, shareStringPart);
            Cell headerCell4 = _sharedResources.InsertCellInWorksheet("D", 1, worksheetPart);
            headerCell4.CellValue = new CellValue(index4.ToString(CultureInfo.InvariantCulture));
            headerCell4.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            #endregion

            #region Insert columns and rows

            int columnCount = 1;
            uint rowCount = 2;

            //_pages = _workspaceVm.PageList;
            //_orders = _groupOrder.ktUIGroupOrderList;

            //foreach (ktUIGroupOrder order in _orders)
            //{
            //    if (columnCount >= 4)
            //    {
            //        columnCount = 1;
            //    }

            //    string text1 = order.DepartmentID;
            //    Cell cell1 =
            //        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //            rowCount, worksheetPart);
            //    cell1.CellValue = new CellValue(text1);
            //    cell1.DataType = CellValues.Number;
            //    columnCount++;

            //    string text2 = order.PageTypeID;
            //    Cell cell2 =
            //        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //            rowCount, worksheetPart);
            //    cell2.CellValue = new CellValue(text2);
            //    cell2.DataType = CellValues.Number;
            //    columnCount++;

            //    string text3 = order.GroupTypeID;
            //    Cell cell3 =
            //        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //            rowCount, worksheetPart);
            //    cell3.CellValue = new CellValue(text3);
            //    cell3.DataType = CellValues.Number;
            //    columnCount++;

            //    double text4 = Convert.ToDouble(order.GroupOrder);
            //    Cell cell4 =
            //        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //            rowCount, worksheetPart);
            //    cell4.CellValue = new CellValue(text4.ToString(CultureInfo.InvariantCulture));
            //    cell4.DataType = CellValues.Number;

            //    rowCount++;
            //}

            //foreach (PageType page in _pages)
            //{
            //    foreach (GroupTypeOrder gtOrder in page.Groups)
            //    {
            //        if (_orders.Any(x => x.GroupTypeID != gtOrder.GroupTypeID))
            //        {
            //            if (columnCount >= 4)
            //            {
            //                columnCount = 1;
            //            }

            //            string text1 = gtOrder.DepartmentID; //order.DepartmentID;
            //            Cell cell1 =
            //                _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //                    rowCount, worksheetPart);
            //            cell1.CellValue = new CellValue(text1);
            //            cell1.DataType = CellValues.Number;
            //            columnCount++;

            //            string text2 = gtOrder.PageTypeID; //order.PageTypeID;
            //            Cell cell2 =
            //                _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //                    rowCount, worksheetPart);
            //            cell2.CellValue = new CellValue(text2);
            //            cell2.DataType = CellValues.Number;
            //            columnCount++;

            //            string text3 = gtOrder.GroupTypeID;  //order.GroupTypeID;
            //            Cell cell3 =
            //                _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //                    rowCount, worksheetPart);
            //            cell3.CellValue = new CellValue(text3);
            //            cell3.DataType = CellValues.Number;
            //            columnCount++;

            //            double text4 = Convert.ToDouble(gtOrder.GroupOrder);  //Convert.ToDouble(order.GroupOrder);
            //            Cell cell4 =
            //                _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
            //                    rowCount, worksheetPart);
            //            cell4.CellValue = new CellValue(text4.ToString(CultureInfo.InvariantCulture));
            //            cell4.DataType = CellValues.Number;

            //            rowCount++;
            //        }
            //    }

            //}



            foreach (PageType page in _workspaceVm.PageList)
            {
                foreach (GroupTypeOrder group in page.GroupTypeOrders)
                {
                    if (columnCount >= 4)
                    {
                        columnCount = 1;
                    }

                    string text1 = group.DepartmentID;
                    Cell cell1 =
                        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                            rowCount, worksheetPart);
                    cell1.CellValue = new CellValue(text1);
                    cell1.DataType = CellValues.Number;
                    columnCount++;

                    string text2 = page.PageTypeID;
                    Cell cell2 =
                        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                            rowCount, worksheetPart);
                    cell2.CellValue = new CellValue(text2);
                    cell2.DataType = CellValues.Number;
                    columnCount++;

                    string text3 = group.GroupTypeID;
                    Cell cell3 =
                        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                            rowCount, worksheetPart);
                    cell3.CellValue = new CellValue(text3);
                    cell3.DataType = CellValues.Number;
                    columnCount++;

                    double text4 = Convert.ToDouble(group.GroupOrder);
                    Cell cell4 =
                        _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                            rowCount, worksheetPart);
                    cell4.CellValue = new CellValue(text4.ToString(CultureInfo.InvariantCulture));
                    cell4.DataType = CellValues.Number;

                    rowCount++;
                }

            }

            #endregion

            worksheetPart.Worksheet.Save();
        }
    }
}
