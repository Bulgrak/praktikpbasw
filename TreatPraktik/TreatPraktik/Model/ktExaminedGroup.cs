using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    class ktExaminedGroup
    {
        public int ID { get; set; }
        public string GroupIdentifier { get; set; }
        public string GroupType { get; set; }
        public int GroupExpendable { get; set; }
        public string Name { get; set; }
        public int Expanded { get; set; }
        public int DataQualityScore { get; set; }
        public int RequiredScore { get; set; }

        public ktExaminedGroup()
        {

        }
    }
}
