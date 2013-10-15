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
        public List<ktResources> ktResourceList { get; set; }

        public WorkSheetktResources()
        {
            SheetName = "ktResources";
            ktResourceList = new List<ktResources>();
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public void LoadktResources(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            //Initialize the ktExaminedGroup list.
            List<ktResources> result = new List<ktResources>();

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
                      : cell.CellValue.InnerText);

                //Check to verify the row contained data.
                if (textValues.Count() > 0)
                {
                    var textArray = textValues.ToArray();
                    ktResources resource = new ktResources();
                    resource.ResourceID = textArray[0];
                    resource.ResourceTypeID = textArray[1];
                    resource.ResourceResxID = textArray[2];
                    result.Add(resource);
                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }
                //Return populated list of ktExaminedGroup.
                ktResourceList = result;
            }
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
