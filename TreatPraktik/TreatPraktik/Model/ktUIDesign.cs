using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    class ktUIDesign
    {
        public int DesignID { get; set; }
        public string DatabaseTableName { get; set; }
        public string DatabaseFieldName { get; set; }
        public string CodeTableName { get; set; }
        public string ResxID { get; set; }
        public int ReadOnlyPolicy { get; set; }
        public int InputDataType { get; set; }
        public int MortyParameter { get; set; }
        public int RequiredID { get; set; }
        public string GUIUnitShortName { get; set; }
        public string DatabaseUnitName { get; set; }
        public string LabkaUnitName { get; set; }
        public string DatabaseToUIConversion { get; set; }
        public string DefaultValue { get; set; }
        public int NormalRangeMinimum { get; set; }
        public int NormalRangeMaximum { get; set; }
        public int RangeMinimum { get; set; }
        public int RangeMaximum { get; set; }
        public int CopyEncounter { get; set; }
        public int CopyEpisode { get; set; }
        public int DataQualityScore { get; set; }
        public int CopyFinalEncounter { get; set; }

        public ktUIDesign()
        {

        }
    }
}
