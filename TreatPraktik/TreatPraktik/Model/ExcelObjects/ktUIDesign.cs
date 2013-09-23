using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    public class ktUIDesign
    {
        public string DesignID { get; set; }
        public string DatabaseTableName { get; set; }
        public string DatabaseFieldName { get; set; }
        public string CodeTableName { get; set; }
        public string ResxID { get; set; }
        public string ReadOnlyPolicy { get; set; }
        public string InputDataType { get; set; }
        public string MortyParameter { get; set; }
        public string RequiredID { get; set; }
        public string GUIUnitShortName { get; set; }
        public string DatabaseUnitName { get; set; }
        public string LabkaUnitName { get; set; }
        public string DatabaseToUIConversion { get; set; }
        public string DefaultValue { get; set; }
        public string NormalRangeMinimum { get; set; }
        public string NormalRangeMaximum { get; set; }
        public string RangeMinimum { get; set; }
        public string RangeMaximum { get; set; }
        public string CopyEncounter { get; set; }
        public string CopyEpisode { get; set; }
        public string DataQualityScore { get; set; }
        public string CopyFinalEncounter { get; set; }

        public ktUIDesign()
        {

        }
    }
}
