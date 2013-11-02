using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktUIGroupOrder
    {
        private static WorkSheetktUIGroupOrder instance;

        public string SheetName { get; set; }
        public List<String> ColumnNames { get; set; }
        public List<ktUIGroupOrder> ktUIGroupOrderList { get; set; }

        //Initialize the ktExaminedGroup list.
        public List<ktUIGroupOrder> Result { get; set; }

        public bool DataOnSheetOk;
        public bool ColumnHeadersOk;

        private WorkSheetktUIGroupOrder()
        {
            SheetName = "ktUIGroupOrder";
            ColumnNames = new List<string>();
            ktUIGroupOrderList = new List<ktUIGroupOrder>();
            Result = new List<ktUIGroupOrder>();

            DataOnSheetOk = true;
            ColumnHeadersOk = true;
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public bool LoadUIGroupOrder(Worksheet worksheet, SharedStringTable sharedString)
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
                                    : cell.CellValue.InnerText)
                            ;

                        //Check to verify the row contained data.
                        if (textValues.Count() > 0)
                        {
                            //Create a ktExaminedGroup and add it to the list.

                            var textArray = textValues.ToArray();
                            ktUIGroupOrder groupOrder = new ktUIGroupOrder();
                            groupOrder.DepartmentID = textArray[0];
                            groupOrder.PageTypeID = textArray[1];
                            groupOrder.GroupTypeID = textArray[2];
                            groupOrder.GroupOrder = Convert.ToDouble(textArray[3], CultureInfo.InvariantCulture);
                            Result.Add(groupOrder);
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
                if (ColumnNames.Any(a => a.Equals("DepartmentID")) &&
                    ColumnNames.Any(b => b.Equals("PageTypeID")) &&
                    ColumnNames.Any(c => c.Equals("GroupTypeID")) &&
                    ColumnNames.Any(d => d.Equals("GroupOrder")))
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
            //Return populated list of ktUIGroupOrder.
            ktUIGroupOrderList = Result;
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkSheetktUIGroupOrder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkSheetktUIGroupOrder();
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
