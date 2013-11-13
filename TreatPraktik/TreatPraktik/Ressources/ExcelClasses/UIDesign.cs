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
    class UIDesign
    {
        readonly WorkSheetktUIDesign _uiDesign;
        readonly SharedRessources _sharedResources;
        readonly WorkspaceViewModel _workspaceVm;

        public UIDesign()
        {
            _uiDesign = WorkSheetktUIDesign.Instance;
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
                Name = _uiDesign.SheetName
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

            const string header1 = "DesignID";
            int index1 = _sharedResources.InsertSharedStringItem(header1, shareStringPart);
            Cell headerCell1 = _sharedResources.InsertCellInWorksheet("A", 1, worksheetPart);
            headerCell1.CellValue = new CellValue(index1.ToString(CultureInfo.InvariantCulture));
            headerCell1.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header2 = "DatabaseTableName";
            int index2 = _sharedResources.InsertSharedStringItem(header2, shareStringPart);
            Cell headerCell2 = _sharedResources.InsertCellInWorksheet("B", 1, worksheetPart);
            headerCell2.CellValue = new CellValue(index2.ToString(CultureInfo.InvariantCulture));
            headerCell2.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header3 = "DatabaseFieldName";
            int index3 = _sharedResources.InsertSharedStringItem(header3, shareStringPart);
            Cell headerCell3 = _sharedResources.InsertCellInWorksheet("C", 1, worksheetPart);
            headerCell3.CellValue = new CellValue(index3.ToString(CultureInfo.InvariantCulture));
            headerCell3.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header4 = "CodeTableName";
            int index4 = _sharedResources.InsertSharedStringItem(header4, shareStringPart);
            Cell headerCell4 = _sharedResources.InsertCellInWorksheet("D", 1, worksheetPart);
            headerCell4.CellValue = new CellValue(index4.ToString(CultureInfo.InvariantCulture));
            headerCell4.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header5 = "ResxID";
            int index5 = _sharedResources.InsertSharedStringItem(header5, shareStringPart);
            Cell headerCell5 = _sharedResources.InsertCellInWorksheet("E", 1, worksheetPart);
            headerCell5.CellValue = new CellValue(index5.ToString(CultureInfo.InvariantCulture));
            headerCell5.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header6 = "ReadOnlyPolicyID";
            int index6 = _sharedResources.InsertSharedStringItem(header6, shareStringPart);
            Cell headerCell6 = _sharedResources.InsertCellInWorksheet("F", 1, worksheetPart);
            headerCell6.CellValue = new CellValue(index6.ToString(CultureInfo.InvariantCulture));
            headerCell6.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header7 = "InputDataTypeID";
            int index7 = _sharedResources.InsertSharedStringItem(header7, shareStringPart);
            Cell headerCell7 = _sharedResources.InsertCellInWorksheet("G", 1, worksheetPart);
            headerCell7.CellValue = new CellValue(index7.ToString(CultureInfo.InvariantCulture));
            headerCell7.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header8 = "MortyParameter";
            int index8 = _sharedResources.InsertSharedStringItem(header8, shareStringPart);
            Cell headerCell8 = _sharedResources.InsertCellInWorksheet("H", 1, worksheetPart);
            headerCell8.CellValue = new CellValue(index8.ToString(CultureInfo.InvariantCulture));
            headerCell8.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header9 = "RequiredID";
            int index9 = _sharedResources.InsertSharedStringItem(header9, shareStringPart);
            Cell headerCell9 = _sharedResources.InsertCellInWorksheet("I", 1, worksheetPart);
            headerCell9.CellValue = new CellValue(index9.ToString(CultureInfo.InvariantCulture));
            headerCell9.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header10 = "GUIUnitShortName";
            int index10 = _sharedResources.InsertSharedStringItem(header10, shareStringPart);
            Cell headerCell10 = _sharedResources.InsertCellInWorksheet("J", 1, worksheetPart);
            headerCell10.CellValue = new CellValue(index10.ToString(CultureInfo.InvariantCulture));
            headerCell10.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header11 = "DatabaseUnitName";
            int index11 = _sharedResources.InsertSharedStringItem(header11, shareStringPart);
            Cell headerCell11 = _sharedResources.InsertCellInWorksheet("K", 1, worksheetPart);
            headerCell11.CellValue = new CellValue(index11.ToString(CultureInfo.InvariantCulture));
            headerCell11.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header12 = "LabkaUnitName";
            int index12 = _sharedResources.InsertSharedStringItem(header12, shareStringPart);
            Cell headerCell12 = _sharedResources.InsertCellInWorksheet("L", 1, worksheetPart);
            headerCell12.CellValue = new CellValue(index12.ToString(CultureInfo.InvariantCulture));
            headerCell12.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header13 = "DatabaseToUIConversion";
            int index13 = _sharedResources.InsertSharedStringItem(header13, shareStringPart);
            Cell headerCell13 = _sharedResources.InsertCellInWorksheet("M", 1, worksheetPart);
            headerCell13.CellValue = new CellValue(index13.ToString(CultureInfo.InvariantCulture));
            headerCell13.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header14 = "DefaultValue";
            int index14 = _sharedResources.InsertSharedStringItem(header14, shareStringPart);
            Cell headerCell14 = _sharedResources.InsertCellInWorksheet("N", 1, worksheetPart);
            headerCell14.CellValue = new CellValue(index14.ToString(CultureInfo.InvariantCulture));
            headerCell14.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header15 = "NormalRangeMinimum";
            int index15 = _sharedResources.InsertSharedStringItem(header15, shareStringPart);
            Cell headerCell15 = _sharedResources.InsertCellInWorksheet("O", 1, worksheetPart);
            headerCell15.CellValue = new CellValue(index15.ToString(CultureInfo.InvariantCulture));
            headerCell15.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header16 = "NormalRangeMaximum";
            int index16 = _sharedResources.InsertSharedStringItem(header16, shareStringPart);
            Cell headerCell16 = _sharedResources.InsertCellInWorksheet("P", 1, worksheetPart);
            headerCell16.CellValue = new CellValue(index16.ToString(CultureInfo.InvariantCulture));
            headerCell16.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header17 = "RangeMinimun";
            int index17 = _sharedResources.InsertSharedStringItem(header17, shareStringPart);
            Cell headerCell17 = _sharedResources.InsertCellInWorksheet("Q", 1, worksheetPart);
            headerCell17.CellValue = new CellValue(index17.ToString(CultureInfo.InvariantCulture));
            headerCell17.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header18 = "RangeMaximum";
            int index18 = _sharedResources.InsertSharedStringItem(header18, shareStringPart);
            Cell headerCell18 = _sharedResources.InsertCellInWorksheet("R", 1, worksheetPart);
            headerCell18.CellValue = new CellValue(index18.ToString(CultureInfo.InvariantCulture));
            headerCell18.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header19 = "CopyEncounter";
            int index19 = _sharedResources.InsertSharedStringItem(header19, shareStringPart);
            Cell headerCell19 = _sharedResources.InsertCellInWorksheet("S", 1, worksheetPart);
            headerCell19.CellValue = new CellValue(index19.ToString(CultureInfo.InvariantCulture));
            headerCell19.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header20 = "CopyEpisode";
            int index20 = _sharedResources.InsertSharedStringItem(header20, shareStringPart);
            Cell headerCell20 = _sharedResources.InsertCellInWorksheet("T", 1, worksheetPart);
            headerCell20.CellValue = new CellValue(index20.ToString(CultureInfo.InvariantCulture));
            headerCell20.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header21 = "DataQualityScore";
            int index21 = _sharedResources.InsertSharedStringItem(header21, shareStringPart);
            Cell headerCell21 = _sharedResources.InsertCellInWorksheet("U", 1, worksheetPart);
            headerCell21.CellValue = new CellValue(index21.ToString(CultureInfo.InvariantCulture));
            headerCell21.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            const string header22 = "CopyFinalEncounter";
            int index22 = _sharedResources.InsertSharedStringItem(header22, shareStringPart);
            Cell headerCell22 = _sharedResources.InsertCellInWorksheet("V", 1, worksheetPart);
            headerCell22.CellValue = new CellValue(index22.ToString(CultureInfo.InvariantCulture));
            headerCell22.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            #endregion

            #region Insert original ktUIExaminedGroup items into excel

            int columnCount = 1;
            uint rowCount = 2;

            foreach (ktUIDesign uiDesign in _uiDesign.ktUIDesignList)
            {
                if (columnCount >= 22)
                {
                    columnCount = 1;
                }

                string text1 = uiDesign.DesignID;
                Cell cell1 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell1.CellValue = new CellValue(text1);
                cell1.DataType = CellValues.Number;
                columnCount++;

                string text2 = uiDesign.DatabaseTableName;
                Cell cell2 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell2.CellValue = new CellValue(text2);
                cell2.DataType = CellValues.String;
                columnCount++;

                string text3 = uiDesign.DatabaseFieldName;
                Cell cell3 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell3.CellValue = new CellValue(text3);
                cell3.DataType = CellValues.String;
                columnCount++;

                string text4 = uiDesign.CodeTableName;
                Cell cell4 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell4.CellValue = new CellValue(text4.ToString(CultureInfo.InvariantCulture));
                cell4.DataType = CellValues.Number;
                columnCount++;

                string text5 = uiDesign.ResxID;
                Cell cell5 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell5.CellValue = new CellValue(text5.ToString(CultureInfo.InvariantCulture));
                cell5.DataType = CellValues.String;
                columnCount++;

                string text6 = uiDesign.ReadOnlyPolicy;
                Cell cell6 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell6.CellValue = new CellValue(text6.ToString(CultureInfo.InvariantCulture));
                cell6.DataType = CellValues.Number;
                columnCount++;

                string text7 = uiDesign.InputDataType;
                Cell cell7 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell7.CellValue = new CellValue(text7.ToString(CultureInfo.InvariantCulture));
                cell7.DataType = CellValues.Number;
                columnCount++;

                string text8 = uiDesign.MortyParameter;
                Cell cell8 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell8.CellValue = new CellValue(text8.ToString(CultureInfo.InvariantCulture));
                cell8.DataType = CellValues.Number;

                string text9 = uiDesign.RequiredID;
                Cell cell9 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell9.CellValue = new CellValue(text9.ToString(CultureInfo.InvariantCulture));
                cell9.DataType = CellValues.Number;

                string text10 = uiDesign.GUIUnitShortName;
                Cell cell10 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell10.CellValue = new CellValue(text10.ToString(CultureInfo.InvariantCulture));
                cell10.DataType = CellValues.Number;

                string text11 = uiDesign.DatabaseUnitName;
                Cell cell11 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell11.CellValue = new CellValue(text11.ToString(CultureInfo.InvariantCulture));
                cell11.DataType = CellValues.Number;

                string text12 = uiDesign.LabkaUnitName;
                Cell cell12 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell12.CellValue = new CellValue(text12.ToString(CultureInfo.InvariantCulture));
                cell12.DataType = CellValues.Number;

                string text13 = uiDesign.DatabaseToUIConversion;
                Cell cell13 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell13.CellValue = new CellValue(text13.ToString(CultureInfo.InvariantCulture));
                cell13.DataType = CellValues.Number;

                string text14 = uiDesign.DefaultValue;
                Cell cell14 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell14.CellValue = new CellValue(text14.ToString(CultureInfo.InvariantCulture));
                cell14.DataType = CellValues.Number;

                string text15 = uiDesign.NormalRangeMinimum;
                Cell cell15 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell15.CellValue = new CellValue(text15.ToString(CultureInfo.InvariantCulture));
                cell15.DataType = CellValues.Number;

                string text16 = uiDesign.NormalRangeMaximum;
                Cell cell16 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell16.CellValue = new CellValue(text16.ToString(CultureInfo.InvariantCulture));
                cell16.DataType = CellValues.Number;

                string text17 = uiDesign.RangeMinimum;
                Cell cell17 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell17.CellValue = new CellValue(text17.ToString(CultureInfo.InvariantCulture));
                cell17.DataType = CellValues.Number;

                string text18 = uiDesign.RangeMaximum;
                Cell cell18 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell18.CellValue = new CellValue(text18.ToString(CultureInfo.InvariantCulture));
                cell18.DataType = CellValues.Number;

                string text19 = uiDesign.CopyEncounter;
                Cell cell19 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell19.CellValue = new CellValue(text19.ToString(CultureInfo.InvariantCulture));
                cell19.DataType = CellValues.Number;

                string text20 = uiDesign.CopyEpisode;
                Cell cell20 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell20.CellValue = new CellValue(text20.ToString(CultureInfo.InvariantCulture));
                cell20.DataType = CellValues.Number;

                string text21 = uiDesign.DataQualityScore;
                Cell cell21 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell21.CellValue = new CellValue(text21.ToString(CultureInfo.InvariantCulture));
                cell21.DataType = CellValues.Number;

                string text22 = uiDesign.CopyFinalEncounter;
                Cell cell22 =
                    _sharedResources.InsertCellInWorksheet(
                        _sharedResources.Number2String(columnCount, true),
                        rowCount, worksheetPart);
                cell22.CellValue = new CellValue(text22.ToString(CultureInfo.InvariantCulture));
                cell22.DataType = CellValues.Number;

                rowCount++;
            }

            #endregion

            #region Insert new ktUIExaminedGroup items into excel

            foreach (ktUIDesign uiDesign in _uiDesign.ktUIDesignList)
            {

                        if (columnCount >= 22)
                        {
                            columnCount = 1;
                        }

                        string text1 = uiDesign.DesignID;
                        Cell cell1 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell1.CellValue = new CellValue(text1);
                        cell1.DataType = CellValues.Number;
                        columnCount++;

                        string text2 = uiDesign.DatabaseTableName;
                        Cell cell2 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell2.CellValue = new CellValue(text2);
                        cell2.DataType = CellValues.String;
                        columnCount++;

                        string text3 = uiDesign.DatabaseFieldName;
                        Cell cell3 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell3.CellValue = new CellValue(text3);
                        cell3.DataType = CellValues.String;
                        columnCount++;

                        string text4 = uiDesign.CodeTableName;
                        Cell cell4 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell4.CellValue = new CellValue(text4.ToString(CultureInfo.InvariantCulture));
                        cell4.DataType = CellValues.String;
                        columnCount++;

                        string text5 = uiDesign.ResxID;
                        Cell cell5 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell5.CellValue = new CellValue(text5.ToString(CultureInfo.InvariantCulture));
                        cell5.DataType = CellValues.String;
                        columnCount++;

                        string text6 = uiDesign.ReadOnlyPolicy;
                        Cell cell6 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell6.CellValue = new CellValue(text6.ToString(CultureInfo.InvariantCulture));
                        cell6.DataType = CellValues.Number;
                        columnCount++;

                        string text7 = uiDesign.InputDataType;
                        Cell cell7 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell7.CellValue = new CellValue(text7.ToString(CultureInfo.InvariantCulture));
                        cell7.DataType = CellValues.Number;
                        columnCount++;

                        string text8 = uiDesign.MortyParameter;
                        Cell cell8 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell8.CellValue = new CellValue(text8.ToString(CultureInfo.InvariantCulture));
                        cell8.DataType = CellValues.Number;

                        string text9 = uiDesign.RequiredID;
                        Cell cell9 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell9.CellValue = new CellValue(text9.ToString(CultureInfo.InvariantCulture));
                        cell9.DataType = CellValues.Number;

                        string text10 = uiDesign.GUIUnitShortName;
                        Cell cell10 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell10.CellValue = new CellValue(text10.ToString(CultureInfo.InvariantCulture));
                        cell10.DataType = CellValues.String;

                        string text11 = uiDesign.DatabaseUnitName;
                        Cell cell11 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell11.CellValue = new CellValue(text11.ToString(CultureInfo.InvariantCulture));
                        cell11.DataType = CellValues.String;

                        string text12 = uiDesign.LabkaUnitName;
                        Cell cell12 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell12.CellValue = new CellValue(text12.ToString(CultureInfo.InvariantCulture));
                        cell12.DataType = CellValues.String;

                        string text13 = uiDesign.DatabaseToUIConversion;
                        Cell cell13 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell13.CellValue = new CellValue(text13.ToString(CultureInfo.InvariantCulture));
                        cell13.DataType = CellValues.String;

                        string text14 = uiDesign.DefaultValue;
                        Cell cell14 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell14.CellValue = new CellValue(text14.ToString(CultureInfo.InvariantCulture));
                        cell14.DataType = CellValues.String;

                        string text15 = uiDesign.NormalRangeMinimum;
                        Cell cell15 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell15.CellValue = new CellValue(text15.ToString(CultureInfo.InvariantCulture));
                        cell15.DataType = CellValues.String;

                        string text16 = uiDesign.NormalRangeMaximum;
                        Cell cell16 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell16.CellValue = new CellValue(text16.ToString(CultureInfo.InvariantCulture));
                        cell16.DataType = CellValues.String;

                        string text17 = uiDesign.RangeMinimum;
                        Cell cell17 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell17.CellValue = new CellValue(text17.ToString(CultureInfo.InvariantCulture));
                        cell17.DataType = CellValues.String;

                        string text18 = uiDesign.RangeMaximum;
                        Cell cell18 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell18.CellValue = new CellValue(text18.ToString(CultureInfo.InvariantCulture));
                        cell18.DataType = CellValues.String;

                        string text19 = uiDesign.CopyEncounter;
                        Cell cell19 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell19.CellValue = new CellValue(text19.ToString(CultureInfo.InvariantCulture));
                        cell19.DataType = CellValues.Number;

                        string text20 = uiDesign.CopyEpisode;
                        Cell cell20 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell20.CellValue = new CellValue(text20.ToString(CultureInfo.InvariantCulture));
                        cell20.DataType = CellValues.Number;

                        string text21 = uiDesign.DataQualityScore;
                        Cell cell21 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell21.CellValue = new CellValue(text21.ToString(CultureInfo.InvariantCulture));
                        cell21.DataType = CellValues.Number;

                        string text22 = uiDesign.CopyFinalEncounter;
                        Cell cell22 =
                            _sharedResources.InsertCellInWorksheet(_sharedResources.Number2String(columnCount, true),
                                rowCount, worksheetPart);
                        cell22.CellValue = new CellValue(text22.ToString(CultureInfo.InvariantCulture));
                        cell22.DataType = CellValues.Number;

                        rowCount++;
  
            }

            #endregion

            worksheetPart.Worksheet.Save();
        }
    }
}
