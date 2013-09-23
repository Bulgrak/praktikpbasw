using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    public class ktExaminedGroup
    {
        public string ID { get; set; }
        public string GroupIdentifier { get; set; }
        public string GroupType { get; set; }
        public string GroupExpendable { get; set; }
        public string Name { get; set; }
        public string Expanded { get; set; }
        public string DataQualityScore { get; set; }
        public string RequiredScore { get; set; }

        public ktExaminedGroup()
        {

        }
    }
}
