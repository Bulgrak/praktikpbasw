using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktUIOrder
    {
        private static WorkSheetktUIOrder instance;

        public string SheetName { get; set; }
        public List<String> ColumnNames { get; set; }
        public List<ktUIOrder> ktUIOrderList { get; set; }

        //Initialize the ktUIOrder list.
        public List<ktUIOrder> Result { get; set; }

        public bool DataOnSheetOk;
        public bool ColumnHeadersOk;

        public WorkSheetktUIOrder()
        {
            SheetName = "ktUIOrder";
            ColumnNames = new List<string>();
            ktUIOrderList = new List<ktUIOrder>();
            

            DataOnSheetOk = true;
            ColumnHeadersOk = true;
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public bool LoadUIOrder(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            Result = new List<ktUIOrder>();
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
                    if (textValues.Count() == 5)
                    {
                        //Create a ktUIOrder and add it to the list.
                        var textArray = textValues.ToArray();
                        ktUIOrder order = new ktUIOrder();
                        order.DesignID = textArray[0];
                        order.GroupOrder = double.Parse(textArray[1], CultureInfo.InvariantCulture); // Convert.ToDouble(textArray[1]);
                        order.GroupTypeID = textArray[2];
                        order.PageTypeID = textArray[3];
                        order.IncludedTypeID = textArray[4];
                        Result.Add(order);
                    }
                    else
                    {
                        var textArray = textValues.ToArray();
                        ktUIOrder order = new ktUIOrder();
                        order.DesignID = textArray[0];
                        order.GroupOrder = double.Parse(textArray[1], CultureInfo.InvariantCulture); // Convert.ToDouble(textArray[1]);
                        order.GroupTypeID = textArray[2];
                        order.PageTypeID = "";
                        order.IncludedTypeID = textArray[4];
                        Result.Add(order);
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
                if (ColumnNames.Any(a => a.Equals("DesignID")) &&
                    ColumnNames.Any(b => b.Equals("GroupOrder")) &&
                    ColumnNames.Any(c => c.Equals("GroupTypeID")) &&
                    ColumnNames.Any(d => d.Equals("PageTypeID")) &&
                    ColumnNames.Any(e => e.Equals("IncludedTypeID")))
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
            ktUIOrderList = Result;
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkSheetktUIOrder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkSheetktUIOrder();
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
