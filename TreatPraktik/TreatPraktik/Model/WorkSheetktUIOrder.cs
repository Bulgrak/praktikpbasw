using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    class WorkSheetktUIOrder
    {
        public string SheetName { get; set; }
        public List<ktUIOrder> ktUIOrderList { get; set; }

        public WorkSheetktUIOrder()
        {
            SheetName = "ktUIGroupOrder";
            ktUIOrderList = new List<ktUIOrder>();
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public void LoadUIOrder(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            //Initialize the ktExaminedGroup list.
            List<ktUIOrder> result = new List<ktUIOrder>();

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

                    var textArray = textValues.ToArray();
                    ktUIOrder order = new ktUIOrder();
                    order.DesignID = textArray[0];
                    order.GroupOrder = textArray[1];
                    order.GroupTypeID = textArray[2];
                    order.PageTypeID = textArray[3];
                    order.IncludedTypeID = textArray[4];
                    result.Add(order);
                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }
                //Return populated list of ktExaminedGroup.
                ktUIOrderList = result;
            }
        }
    }
}
