using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktUIDesign
    {
        private static WorkSheetktUIDesign instance;

        public string SheetName { get; set; }
        public List<String> ColumnNames { get; set; }
        public List<ktUIDesign> ktUIDesignList { get; set; }

        //Initialize the ktUIDesign list.
        public List<ktUIDesign> Result { get; set; }

        public bool DataOnSheetOk;
        public bool ColumnHeadersOk;

        public WorkSheetktUIDesign()
        {
            SheetName = "ktUIDesign";
            ColumnNames = new List<string>();
            ktUIDesignList = new List<ktUIDesign>();

            

            DataOnSheetOk = true;
            ColumnHeadersOk = true;
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public bool LoadUIDesign(Worksheet worksheet, SharedStringTable sharedString)
        {
            Result = new List<ktUIDesign>();
            //Linq query to get the column headers on the sheet
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
                //LINQ query to skip first row with column names.
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
                            design.RangeMinimum = textArray[16];
                            design.RangeMaximum = textArray[17];
                            design.CopyEncounter = textArray[18];
                            design.CopyEpisode = textArray[19];
                            design.DataQualityScore = textArray[20];
                            design.CopyFinalEncounter = textArray[21];
                            Result.Add(design);
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
                if (ColumnNames.Any(a => a.Equals("DesignID")) &&
                    ColumnNames.Any(b => b.Equals("DatabaseTableName")) &&
                    ColumnNames.Any(c => c.Equals("DatabaseFieldName")) &&
                    ColumnNames.Any(d => d.Equals("CodeTableName")) &&
                    ColumnNames.Any(e => e.Equals("ResxID")) &&
                    ColumnNames.Any(f => f.Equals("ReadOnlyPolicyID")) &&
                    ColumnNames.Any(g => g.Equals("InputDataTypeID")) &&
                    ColumnNames.Any(h => h.Equals("MortyParameter")) &&
                    ColumnNames.Any(i => i.Equals("RequiredID")) &&
                    ColumnNames.Any(j => j.Equals("GUIUnitShortName")) &&
                    ColumnNames.Any(k => k.Equals("DatabaseUnitName")) &&
                    ColumnNames.Any(l => l.Equals("LabkaUnitName")) &&
                    ColumnNames.Any(m => m.Equals("DatabaseToUIConversion")) &&
                    ColumnNames.Any(n => n.Equals("DefaultValue")) &&
                    ColumnNames.Any(o => o.Equals("NormalRangeMinimum")) &&
                    ColumnNames.Any(p => p.Equals("NormalRangeMaximum")) &&
                    ColumnNames.Any(q => q.Equals("RangeMinimun")) &&
                    ColumnNames.Any(r => r.Equals("RangeMaximum")) &&
                    ColumnNames.Any(s => s.Equals("CopyEncounter")) &&
                    ColumnNames.Any(t => t.Equals("CopyEpisode")) &&
                    ColumnNames.Any(u => u.Equals("DataQualityScore")) &&
                    ColumnNames.Any(v => v.Equals("CopyFinalEncounter")))
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
            if (dataRows.FirstOrDefault() != null)
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
            ktUIDesignList = Result;
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkSheetktUIDesign Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkSheetktUIDesign();
                }
                return instance;
            }
        }
    }
}
