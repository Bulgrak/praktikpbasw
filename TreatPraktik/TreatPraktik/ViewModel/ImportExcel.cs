using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using TreatPraktik.Model;


namespace TreatPraktik.ViewModel
{
    class ImportExcel
    {
        //Declare variables to hold refernces to Excel objects.
        Workbook workBook;
        SharedStringTable sharedStrings;
        IEnumerable<Sheet> workSheets;
        WorksheetPart ktExaminedGroupSheet;
        WorksheetPart ktUIDesignSheet;
        WorksheetPart ktUIFieldIncludedTypeSheet;
        WorksheetPart ktUIGroupOrderSheet;
        WorksheetPart ktUIOrderSheet;
        WorksheetPart ktUIPageTypeSheet;

        //Declare helper variables.
        string ktExaminedID;
        string ktUIDesignID;
        string ktUIFieldID;
        string ktUIGroupID;
        string ktUIOrderID;
        string ktUIPageID;
        List<ktExaminedGroup> ExaminedGroupList;
        public List<ktUIDesign> UIDesignList { get; set;}
        List<ktUIFieldIncludedType> UIFieldList;
        List<ktUiGroupOrder> UIGroupList;
        List<ktUIOrder> UIOrderList;
        List<ktUIPageType> UIPageList;

        public ImportExcel()
        {
            //Open the Excel workbook.
            using (SpreadsheetDocument document =
              SpreadsheetDocument.Open(@"C:\UITablesToStud.xlsx", true))
            {
                //References to the workbook and Shared String Table.
                workBook = document.WorkbookPart.Workbook;
                workSheets = workBook.Descendants<Sheet>();
                sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                //Reference to Excel Worksheet with ktExaminedGroup data.
                ktExaminedID = workSheets.First(s => s.Name == @"ktExaminedGroup").Id;
                ktExaminedGroupSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktExaminedID);

                //Load ktExaminedGroup data to business object.
                ExaminedGroupList = LoadExaminedGroup(ktExaminedGroupSheet.Worksheet, sharedStrings);

                //Reference to Excel Worksheet with ktExaminedGroup data.
                ktUIDesignID = workSheets.First(s => s.Name == @"ktUIDesign").Id;
                ktUIDesignSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktUIDesignID);

                //Load ktExaminedGroup data to business object.
                UIDesignList = LoadUIDesign(ktUIDesignSheet.Worksheet, sharedStrings);
            }
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public static List<ktExaminedGroup> LoadExaminedGroup(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            //Initialize the ktExaminedGroup list.
            List<ktExaminedGroup> result = new List<ktExaminedGroup>();

            //LINQ query to skip first row with column names.
            IEnumerable<Row> dataRows =
              from row in worksheet.Descendants<Row>()
              where row.RowIndex > 1
              select row;

            foreach (Row row in dataRows)
            {
                //LINQ query to return the row's cell values.
                //Where clause filters out any cells that do not contain a value.
                //Select returns the value of a cell unless the cell contains
                //  a Shared String.
                //If the cell contains a Shared String, its value will be a 
                //  reference id which will be used to look up the value in the 
                //  Shared String table.
                //IEnumerable<String> textValues =
                //  from cell in row.Descendants<Cell>()
                //  where cell.CellValue != null
                //  select
                //    (cell.DataType != null
                //      && cell.DataType.HasValue
                //      && cell.DataType == CellValues.SharedString
                //    ? sharedString.ChildElements[
                //      int.Parse(cell.CellValue.InnerText)].InnerText
                //    : cell.CellValue.InnerText)
                //  ;

                IEnumerable<String> textValues =
  from cell in row.Descendants<Cell>()
  where cell.CellValue != null
  select
    (cell.DataType != null
      && cell.DataType.HasValue
      && cell.DataType == CellValues.SharedString
    ? sharedString.ChildElements[
      int.Parse(cell.CellValue.InnerText)].InnerText
    : cell.CellValue.InnerText)
  ;

                //Check to verify the row contained data.
                if (textValues.Count() > 0)
                {
                    if (textValues.Count() == 8)
                    {
                        //Create a ktExaminedGroup and add it to the list.
                        var textArray = textValues.ToArray();
                        ktExaminedGroup examined = new ktExaminedGroup();
                        examined.ID = Int32.Parse(textArray[0]);
                        examined.GroupIdentifier = textArray[1];
                        examined.GroupType = textArray[2];
                        examined.GroupExpendable = Int32.Parse(textArray[3]);
                        examined.Name = textArray[4];
                        examined.Expanded = Int32.Parse(textArray[5]);
                        examined.DataQualityScore = Int32.Parse(textArray[6]);
                        examined.RequiredScore = Int32.Parse(textArray[7]);
                        result.Add(examined);
                    }
                    else
                    {
                        var textArray = textValues.ToArray();
                        ktExaminedGroup examined = new ktExaminedGroup();
                        examined.ID = Int32.Parse(textArray[0]);
                        examined.GroupIdentifier = textArray[1];
                        examined.GroupType = textArray[2];
                        examined.GroupExpendable = Int32.Parse(textArray[3]);
                        examined.Name = "";
                        examined.Expanded = Int32.Parse(textArray[4]);
                        examined.DataQualityScore = Int32.Parse(textArray[5]);
                        examined.RequiredScore = Int32.Parse(textArray[6]);
                        result.Add(examined);
                    }
                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }
            }

            //Return populated list of ktExaminedGroup.
            return result;
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public static List<ktUIDesign> LoadUIDesign(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            //Initialize the ktExaminedGroup list.
            List<ktUIDesign> result = new List<ktUIDesign>();

            //LINQ query to skip first row with column names.
            IEnumerable<Row> dataRows =
              from row in worksheet.Descendants<Row>()
              where row.RowIndex > 1
              select row;

            foreach (Row row in dataRows)
            {
                //LINQ query to return the row's cell values.
                //Where clause filters out any cells that do not contain a value.
                //Select returns the value of a cell unless the cell contains
                //  a Shared String.
                //If the cell contains a Shared String, its value will be a 
                //  reference id which will be used to look up the value in the 
                //  Shared String table.
                //IEnumerable<String> textValues =
                //  from cell in row.Descendants<Cell>()
                //  where cell.CellValue != null
                //  select
                //    (cell.DataType != null
                //      && cell.DataType.HasValue
                //      && cell.DataType == CellValues.SharedString
                //    ? sharedString.ChildElements[
                //      int.Parse(cell.CellValue.InnerText)].InnerText
                //    : cell.CellValue.InnerText)
                //  ;

                IEnumerable<String> textValues =
  from cell in row.Descendants<Cell>()
  where cell.CellValue != null
  select
    (cell.DataType != null
      && cell.DataType.HasValue
      && cell.DataType == CellValues.SharedString
    ? sharedString.ChildElements[
      int.Parse(cell.CellValue.InnerText)].InnerText
    : cell.CellValue.InnerText)
  ;

                //Check to verify the row contained data.
                if (textValues.Count() > 0)
                {
                    //Create a ktExaminedGroup and add it to the list.
                    //var textArray = textValues.ToArray();
                    //ktUIDesign design = new ktUIDesign();
                    //design.DesignID = Int32.Parse(textArray[0]);
                    //design.DatabaseTableName = textArray[1];
                    //design.DatabaseFieldName = textArray[2];
                    //design.CodeTableName = textArray[3];
                    //design.ResxID = textArray[4];
                    //design.ReadOnlyPolicy = Int32.Parse(textArray[5]);
                    //design.InputDataType = Int32.Parse(textArray[6]);
                    //design.MortyParameter = Int32.Parse(textArray[7]);
                    //design.RequiredID = Int32.Parse(textArray[8]);
                    //design.GUIUnitShortName = textArray[9];
                    //design.DatabaseUnitName = textArray[10];
                    //design.LabkaUnitName = textArray[11];
                    //design.DatabaseToUIConversion = textArray[12];
                    //design.DefaultValue = textArray[13];
                    //design.NormalRangeMinimum = Int32.Parse(textArray[14]);
                    //design.NormalRangeMaximum = Int32.Parse(textArray[15]);
                    //design.CopyEncounter = Int32.Parse(textArray[16]);
                    //design.CopyEpisode = Int32.Parse(textArray[17]);
                    //design.DataQualityScore = Int32.Parse(textArray[18]);
                    //design.CopyFinalEncounter = Int32.Parse(textArray[19]);
                    //result.Add(design);

                    var textArray = textValues.ToArray();
                    ktUIDesign design = new ktUIDesign();
                    design.DesignID = textArray[0];
                    design.DatabaseTableName = textArray[1];
                    design.DatabaseFieldName = textArray[2];
                    design.CodeTableName = textArray[3];
                    design.ResxID = textArray[4];
                    design.ReadOnlyPolicy = textArray[5];
                    design.InputDataType = textArray[6];
                    design.MortyParameter = textArray[7];
                    design.RequiredID = textArray[8];
                    design.GUIUnitShortName = textArray[9];
                    design.DatabaseUnitName = textArray[10];
                    design.LabkaUnitName = textArray[11];
                    design.DatabaseToUIConversion = textArray[12];
                    design.DefaultValue = textArray[13];
                    design.NormalRangeMinimum = textArray[14];
                    design.NormalRangeMaximum = textArray[15];
                    design.CopyEncounter = textArray[16];
                    design.CopyEpisode = textArray[17];
                    design.DataQualityScore = textArray[18];
                    design.CopyFinalEncounter = textArray[19];
                    result.Add(design);
                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }
            }

            //Return populated list of ktExaminedGroup.
            return result;
        }
    }
}
