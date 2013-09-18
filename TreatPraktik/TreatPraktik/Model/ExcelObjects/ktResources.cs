using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    class ktResources
    {
        public string ResourceID { get; set; }
        public string ResourceTypeID { get; set; }
        public string ResourceResxID { get; set; }

        public ktResources()
        {
            ResourceID = "";
            ResourceTypeID = "";
            ResourceResxID = "";
        }
    }
}
