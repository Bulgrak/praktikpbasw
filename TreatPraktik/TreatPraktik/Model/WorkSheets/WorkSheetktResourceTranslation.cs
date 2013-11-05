using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktResourceTranslation
    {
        private static WorkSheetktResourceTranslation instance;

        public string SheetName { get; set; }
        public List<string> ColumnNames { get; set; }
        public List<ktResourceTranslation> ktResourceTranslationList { get; set; }

        //Initialize the ktResourceTranslation list.
        public List<ktResourceTranslation> Result { get; set; }

        public bool DataOnSheetOk { get; set; }
        public bool ColumnHeadersOk { get; set; }

        public WorkSheetktResourceTranslation()
        {
            SheetName = "ktResourceTranslation";
            ColumnNames = new List<string>();
            ktResourceTranslationList = new List<ktResourceTranslation>();
            Result = new List<ktResourceTranslation>();

            DataOnSheetOk = true;
            ColumnHeadersOk = true;
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public bool LoadktResourceTranslation(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            //LINQ query to skip first row with column names.
            Row columnRow =
   (from row in worksheet.Descendants<Row>()
    where row.RowIndex == 1
    select row).First();

            IEnumerable<String> headerValues =
                    from cell in columnRow.Descendants<Cell>()
                    where cell.CellValue != null
                    select
                        (cell.DataType != null
                         && cell.DataType.HasValue
                         && cell.DataType == CellValues.SharedString
                            ? sharedString.ChildElements[
                                int.Parse(cell.CellValue.InnerText)].InnerText
                            : cell.CellValue.InnerText);

            foreach (string header in headerValues)
            {
                ColumnNames.Add(header);
            }

            if (CheckColumnNames(ColumnNames))
            {

                IEnumerable<Row> dataRows =
                  from row in worksheet.Descendants<Row>()
                  where row.RowIndex > 1
                  select row;
                if (CheckDataInSheets(dataRows))
                {

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
                            if (textValues.Count() == 3)
                            {
                                var textArray = textValues.ToArray();
                                ktResourceTranslation resourceTranslation = new ktResourceTranslation();
                                resourceTranslation.LanguageID = textArray[0];
                                resourceTranslation.ResourceID = textArray[1];
                                resourceTranslation.TranslationText = textArray[2];
                                Result.Add(resourceTranslation);
                            }
                            else
                            {
                                var textArray = textValues.ToArray();
                                ktResourceTranslation resourceTranslation = new ktResourceTranslation();
                                resourceTranslation.LanguageID = textArray[0];
                                resourceTranslation.ResourceID = textArray[1];
                                resourceTranslation.TranslationText = "";
                                Result.Add(resourceTranslation);
                            }
                        }
                        else
                        {
                            //If no cells, then you have reached the end of the table.
                            break;
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the excel file contains the correct column names
        /// </summary>
        /// <param name="headers">List of column headers for the selected sheet</param>
        /// <returns></returns>
        public bool CheckColumnNames(List<string> headers)
        {
            if (ColumnNames.Count != 0)
            {
                if (ColumnNames.Any(a => a.Equals("LanguageID")) &&
                    ColumnNames.Any(b => b.Equals("ResourceID")) &&
                    ColumnNames.Any(c => c.Equals("TranslationText")))
                {
                    return true;
                }
            }
            ColumnHeadersOk = false;
            return false;
        }

        /// <summary>
        /// Check if the excel sheet contains data
        /// </summary>
        /// <param name="dataRows">A list of all the rows on the sheet</param>
        /// <returns></returns>
        public bool CheckDataInSheets(IEnumerable<Row> dataRows)
        {
            if (dataRows.ElementAt(0).ElementAt(0).Elements().Count() != 0)
            //if (dataRows.FirstOrDefault() != null)
            {
                return true;
            }
            DataOnSheetOk = false;
            return false;
        }

        /// <summary>
        /// Accept and save the changes for the excel sheet
        /// </summary>
        public void AcceptChanges()
        {
            //Return populated list of ktExaminedGroup.
            ktResourceTranslationList = Result;
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkSheetktResourceTranslation Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkSheetktResourceTranslation();
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }
    }
}
