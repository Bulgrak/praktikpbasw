using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    class ToolboxItem
    {
        public string DesignID { get; set; }
        public string ResourceID { get; set; }
        public string ResxID { get; set; }
        public string Header { get; set; }
        public string ToolTip { get; set; }
        public string Category { get; set; }

        public ToolboxItem()
        {

        }
    }
}
