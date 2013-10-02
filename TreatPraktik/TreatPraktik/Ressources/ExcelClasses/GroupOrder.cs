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
    class GroupOrder
    {
        WorkSheetktUIGroupOrder groupOrder;
        SharedRessources sharedResources;
        WorkspaceViewModel workspaceVM;

        public GroupOrder()
        {
            groupOrder = WorkSheetktUIGroupOrder.Instance;
            sharedResources = SharedRessources.Instance;
            workspaceVM = WorkspaceViewModel.Instance;
        }

        public void CreateSheet(Sheets sheets, SpreadsheetDocument spreadsheetDocument, WorkbookPart workbookPart)
        {
            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = (UInt32Value)3U,
                Name = groupOrder.SheetName
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

            #region Excel headers

            string header1 = "DepartmentID";
            int index1 = sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString());
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header2 = "PageTypeID";
            int index2 = sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString());
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header3 = "GroupTypeID";
            int index3 = sharedResources.InsertSharedStringItem(header3, shareStringPart);
            Cell headerCell3 = sharedResources.InsertCellInWorksheet("C", 1, worksheetPart);
            headerCell3.CellValue = new CellValue(index3.ToString());
            headerCell3.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            string header4 = "GroupOrder";
            int index4 = sharedResources.InsertSharedStringItem(header4, shareStringPart);
            Cell headerCell4 = sharedResources.InsertCellInWorksheet("D", 1, worksheetPart);
            headerCell4.CellValue = new CellValue(index4.ToString());
            headerCell4.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            #endregion

            int columnCount = 1;
            uint rowCount = 2;

            #region Columns and rows

            for (int i = 0; i < groupOrder.ktUIGroupOrderList.Count; i++)
            {
                if (columnCount >= 4)
                {
                    columnCount = 1;
                }

                ktUIGroupOrder go = groupOrder.ktUIGroupOrderList[i];

                if (go.PageTypeID != "15" && go.PageTypeID != "16" && go.PageTypeID != "17")
                {
                    string text1 = go.DepartmentID;
                    Cell cell1 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                    cell1.CellValue = new CellValue(text1.ToString());
                    cell1.DataType = CellValues.Number;
                    columnCount++;

                    string text2 = go.PageTypeID;
                    Cell cell2 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                    cell2.CellValue = new CellValue(text2.ToString());
                    cell2.DataType = CellValues.Number;
                    columnCount++;

                    string text3 = go.GroupTypeID;
                    Cell cell3 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                    cell3.CellValue = new CellValue(text3.ToString());
                    cell3.DataType = CellValues.Number;
                    columnCount++;

                    double text4 = go.GroupOrder;
                    Cell cell4 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                    cell4.CellValue = new CellValue(text4.ToString());
                    cell4.DataType = CellValues.Number;

                    rowCount++;
                }

                //string text4 = go.GroupOrder;
                //int index4 = sharedResources.InsertSharedStringItem(text4, shareStringPart);
                //Cell cell4 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                //cell4.CellValue = new CellValue(index4.ToString());
                //cell4.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            }

            #endregion

            foreach (PageType page in workspaceVM.PageList)
            {
                if (page.PageTypeID.Equals("15") || page.PageTypeID.Equals("16") || page.PageTypeID.Equals("17"))
	            {
                    foreach (GroupTypeOrder group in page.Groups)
	                {
                        if (columnCount >= 4)
                        {
                            columnCount = 1;
                        }

                        string text1 = group.DepartmentID;
                        Cell cell1 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1.ToString());
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        string text2 = page.PageTypeID;
                        Cell cell2 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell2.CellValue = new CellValue(text2.ToString());
                        cell2.DataType = CellValues.Number;
                        columnCount++;

                        string text3 = group.GroupTypeID;
                        Cell cell3 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3.ToString());
                        cell3.DataType = CellValues.Number;
                        columnCount++;

                        double text4 = Convert.ToDouble(group.GroupOrder);
                        Cell cell4 = sharedResources.InsertCellInWorksheet(sharedResources.Number2String(columnCount, true), rowCount, worksheetPart);
                        cell4.CellValue = new CellValue(text4.ToString());
                        cell4.DataType = CellValues.Number;

                        rowCount++;
                    }
                }
            }

            worksheetPart.Worksheet.Save();
        }
    }
}
