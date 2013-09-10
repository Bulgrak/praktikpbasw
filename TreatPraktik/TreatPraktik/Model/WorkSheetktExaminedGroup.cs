using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    class WorkSheetktExaminedGroup
    {
        public string SheetName { get; set; }
        public List<ktExaminedGroup> ExaminedGroupList { get; set; }

        public WorkSheetktExaminedGroup()
        {
            SheetName = "ktExaminedGroup";
            ExaminedGroupList = new List<ktExaminedGroup>();
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public void LoadExaminedGroup(Worksheet worksheet,
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
                        examined.ID = textArray[0];
                        examined.GroupIdentifier = textArray[1];
                        examined.GroupType = textArray[2];
                        examined.GroupExpendable = textArray[3];
                        examined.Name = textArray[4];
                        examined.Expanded = textArray[5];
                        examined.DataQualityScore = textArray[6];
                        examined.RequiredScore = textArray[7];
                        result.Add(examined);
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
            ExaminedGroupList = result;
        }
    }
}
