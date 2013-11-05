using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktResources
    {
        private static WorkSheetktResources instance;

        public string SheetName { get; set; }
        public List<String> ColumnNames { get; set; }
        public List<ktResources> ktResourceList { get; set; }

        public List<ktResources> Result { get; set; }

        public bool DataOnSheetOk { get; set; }
        public bool ColumnHeadersOk { get; set; }

        public WorkSheetktResources()
        {
            SheetName = "ktResources";
            ColumnNames = new List<string>();
            ktResourceList = new List<ktResources>();
            Result = new List<ktResources>();

            DataOnSheetOk = true;
            ColumnHeadersOk = true;
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public bool LoadktResources(Worksheet worksheet,
          SharedStringTable sharedString)
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
                            var textArray = textValues.ToArray();
                            ktResources resource = new ktResources();
                            resource.ResourceID = textArray[0];
                            resource.ResourceTypeID = textArray[1];
                            resource.ResourceResxID = textArray[2];
                            Result.Add(resource);
                        }
                        else
                        {
                            //If no cells, then you have reached the end of the table.
                            break;
                        }
                        //Return populated list of ktExaminedGroup.

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
                if (ColumnNames.Any(a => a.Equals("ResourceID")) &&
                    ColumnNames.Any(b => b.Equals("ResourceTypeID")) &&
                    ColumnNames.Any(c => c.Equals("ResourceResxID")))
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
            ktResourceList = Result;
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkSheetktResources Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkSheetktResources();
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
