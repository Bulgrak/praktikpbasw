using System.Windows;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktExaminedGroup
    {
        private static WorkSheetktExaminedGroup instance;

        public string SheetName { get; set; }
        public List<String> ColumnNames { get; set; }
        public List<ktExaminedGroup> ExaminedGroupList { get; set; }

        //Initialize the ktExaminedGroup list.
        public List<ktExaminedGroup> Result { get; set; }

        public bool DataOnSheetOk { get; set; }
        public bool ColumnHeadersOk { get; set; }

        private WorkSheetktExaminedGroup()
        {
            SheetName = "ktExaminedGroup";
            ColumnNames = new List<string>();
            ExaminedGroupList = new List<ktExaminedGroup>();
            Result = new List<ktExaminedGroup>();

            DataOnSheetOk = true;
            ColumnHeadersOk = true;
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public bool LoadExaminedGroup(Worksheet worksheet, SharedStringTable sharedString)
        {
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
                        IEnumerable<String> textValues =
                            from cell in row.Descendants<Cell>()
                            where cell.CellValue != null
                            select
                                (cell.DataType != null
                                 && cell.DataType.HasValue
                                 && cell.DataType == CellValues.SharedString
                                    ? sharedString.ChildElements[
                                        int.Parse(cell.CellValue.InnerText)].InnerText
                                    : cell.CellValue.InnerText);

                        //Check to verify the row contained data.
                        if (textValues.Count() > 0)
                        {
                            if (textValues.Count() == 8)
                            {
                                //Create a ktExaminedGroup and add it to the list.
                                var textArray = textValues.ToArray();
                                ktExaminedGroup examined = new ktExaminedGroup();
                                examined.ID = textArray[0];
                                examined.GroupIdentifier = textArray[1];
                                examined.GroupType = textArray[2];
                                examined.GroupExpendable = textArray[3];
                                examined.Name = textArray[4];
                                examined.Expanded = textArray[5];
                                examined.DataQualityScore = textArray[6];
                                examined.RequiredScore = textArray[7];
                                Result.Add(examined);
                            }
                            else
                            {
                                var textArray = textValues.ToArray();
                                ktExaminedGroup examined = new ktExaminedGroup();
                                examined.ID = textArray[0];
                                examined.GroupIdentifier = textArray[1];
                                examined.GroupType = textArray[2];
                                examined.GroupExpendable = textArray[3];
                                examined.Name = "";
                                examined.Expanded = textArray[4];
                                examined.DataQualityScore = textArray[5];
                                examined.RequiredScore = textArray[6];
                                Result.Add(examined);
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
                if (ColumnNames.Any(a => a.Equals("ID")) && 
                    ColumnNames.Any(b => b.Equals("GroupIdentifier")) &&
                    ColumnNames.Any(c => c.Equals("GroupType")) &&
                    ColumnNames.Any(d => d.Equals("GroupExpandable")) &&
                    ColumnNames.Any(e => e.Equals("Name")) &&
                    ColumnNames.Any(f => f.Equals("Expanded")) &&
                    ColumnNames.Any(g => g.Equals("DataQualityScore")) &&
                    ColumnNames.Any(h => h.Equals("RequiredScore")))
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
            ExaminedGroupList = Result;
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkSheetktExaminedGroup Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkSheetktExaminedGroup();
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
