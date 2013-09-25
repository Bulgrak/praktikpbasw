using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktUIGroupOrder
    {
        private static WorkSheetktUIGroupOrder instance;

        public string SheetName { get; set; }
        public List<ktUIGroupOrder> ktUIGroupOrderList { get; set; }

        private WorkSheetktUIGroupOrder()
        {
            SheetName = "ktUIGroupOrder";
            ktUIGroupOrderList = new List<ktUIGroupOrder>();
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public void LoadUIGroupOrder(Worksheet worksheet, SharedStringTable sharedString)
        {
            //Initialize the ktExaminedGroup list.
            List<ktUIGroupOrder> result = new List<ktUIGroupOrder>();

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
                    groupOrder.GroupOrder = textArray[3];
                    result.Add(groupOrder);
                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }
                //Return populated list of ktExaminedGroup.
                ktUIGroupOrderList = result;
            }
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
        }
    }
}
