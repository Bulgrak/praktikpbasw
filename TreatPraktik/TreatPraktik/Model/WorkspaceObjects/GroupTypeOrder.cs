using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class GroupTypeOrder
    {
        public string DepartmentID { get; set; }
        public string PageTypeID { get; set; }
        public string GroupTypeID { get; set; }
        public GroupType Group { get; set; }
        public double GroupOrder { get; set; }
    }
}
