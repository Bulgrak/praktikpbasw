using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.Ressources.ExcelClasses
{
    class Order
    {
        WorkSheetktUIOrder order;
        SharedRessources sharedResources;
        WorkspaceViewModel workspaceVM;

        List<GroupType> tempList;

        public Order()
        {
            tempList = new List<GroupType>();

            order = WorkSheetktUIOrder.Instance;
            sharedResources = SharedRessources.Instance;
            workspaceVM = WorkspaceViewModel.Instance;
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

            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = (UInt32Value)4U,
                Name = order.SheetName
            };
            sheets.Append(sheet);

            //Add cells to the sheet
            InsertTextIntoCells(spreadsheetDocument, sheet, worksheetPart);
        }

        /// <summary>
        /// Creates the content of the shet (columns, rows, cells)
        /// </summary>
        /// <param name="spreadsheetDocument">The spreadsheet containing the sheets</param>
        /// <param name="sheet">The sheet for this item</param>
        /// <param name="worksheetPart">The worksheetpart for this item</param>
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

            #region Excel headers

            string header1 = "DesignID";
            int index1 = sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString());
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header2 = "GroupOrder";
            int index2 = sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString());
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header3 = "GroupTypeID";
            int index3 = sharedResources.InsertSharedStringItem(header3, shareStringPart);
            Cell headerCell3 = sharedResources.InsertCellInWorksheet("C", 1, worksheetPart);
            headerCell3.CellValue = new CellValue(index3.ToString());
            headerCell3.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            //string header4 = "PageTypeID";
            //int index4 = sharedResources.InsertSharedStringItem(header4, shareStringPart);
            //Cell headerCell4 = sharedResources.InsertCellInWorksheet("D", 1, worksheetPart);
            //headerCell4.CellValue = new CellValue(index4.ToString());
            //headerCell4.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header4 = "IncludedTypeID";
            int index4 = sharedResources.InsertSharedStringItem(header4, shareStringPart);
            Cell headerCell4 = sharedResources.InsertCellInWorksheet("D", 1, worksheetPart);
            headerCell4.CellValue = new CellValue(index4.ToString());
            headerCell4.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            #endregion

            #region Create temporary list containing the items needed to create the ktUIOrder excel sheet

            foreach (PageType page in workspaceVM.PageList)
            {
                foreach (GroupType group in page.Groups)
                {
                    for (int i = 0; i < group.Items.Count; i++)
                    {
                        if (!tempList.Any(x => x.GroupTypeID == group.Items[i].GroupTypeID))
                        {
                            tempList.Add(group);
                        }
                    }
                }
            }

            #endregion

            #region insert the items from the temporary list into the ktUIOrder excel sheet

            int columnCount = 1;
            uint rowCount = 2;

            foreach (GroupType group in tempList)
            {
                for (int i = 0; i < group.Items.Count; i++)
                {
                    if (columnCount >= 4)
                    {
                        columnCount = 1;
                    }
                        //tempList = group.Items.Select(i => i)

                        string text1 = group.Items[i].DesignID;
                        Cell cell1 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1.ToString());
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        double text2 = group.Items[i].ItemOrder;
                        Cell cell2 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell2.DataType = CellValues.Number;
                        cell2.CellValue = new CellValue(DoubleValue.FromDouble(text2));
                        
                        columnCount++;

                        string text3 = group.Items[i].GroupTypeID;
                        Cell cell3 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3.ToString());
                        cell3.DataType = CellValues.Number;
                        columnCount++;

                        //string text4 = page.PageTypeID;
                        //Cell cell4 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        //cell4.CellValue = new CellValue(text4.ToString());
                        //cell4.DataType = CellValues.Number;
                        //columnCount++;

                        string text5 = group.Items[i].IncludedTypeID;
                        Cell cell5 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell5.CellValue = new CellValue(text5.ToString());
                        cell5.DataType = CellValues.Number;

                        rowCount++;
                }
            }
            
            #endregion

            worksheetPart.Worksheet.Save();
        }
    }
}
