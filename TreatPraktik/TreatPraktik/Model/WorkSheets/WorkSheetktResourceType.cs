using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkSheets
{
    public class WorkSheetktResourceType
    {
        private static WorkSheetktResourceType instance;

                public string SheetName { get; set; }
        public List<ktResourceType> ktResourceTypeList { get; set; }

        public WorkSheetktResourceType()
        {
            SheetName = "ktResourceType";
            ktResourceTypeList = new List<ktResourceType>();
        }

        /// <summary>
        /// Helper method for creating a list of ktExaminedGroup 
        /// from an Excel worksheet.
        /// </summary>
        public void LoadktResourceType(Worksheet worksheet,
          SharedStringTable sharedString)
        {
            //Initialize the ktExaminedGroup list.
            List<ktResourceType> result = new List<ktResourceType>();

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
                    ktResourceType resourceType = new ktResourceType();
                    resourceType.ResourceTypeID = textArray[0];
                    resourceType.ResourceType = textArray[1];
                    result.Add(resourceType);
                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }
                //Return populated list of ktExaminedGroup.
                ktResourceTypeList = result;
            }
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkSheetktResourceType Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkSheetktResourceType();
                }
                return instance;
            }
        }
    }
}
