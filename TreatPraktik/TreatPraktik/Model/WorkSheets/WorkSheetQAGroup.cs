using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreatPraktik.Model.ExcelObjects;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetQAGroup
    {
                public string SheetName { get; set; }
        public List<QAGroup> QAGroupsList { get; set; }

        public WorkSheetQAGroup()
        {
            SheetName = "QAGroups";
            QAGroupsList = new List<QAGroup>();
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public void LoadQAGroups(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            //Initialize the ktExaminedGroup list.
            List<QAGroup> result = new List<QAGroup>();

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
                    QAGroup qaGroup = new QAGroup();
                    qaGroup.TypeID = textArray[0];
                    qaGroup.Type = textArray[1];
                    result.Add(qaGroup);
                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }
                //Return populated list of ktExaminedGroup.
                QAGroupsList = result;
            }
        }
    }
}
