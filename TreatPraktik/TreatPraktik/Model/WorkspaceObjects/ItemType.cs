using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class ItemType
    {
        public string GroupTypeID { get; set; }
        public string DesignID { get; set; }            //ID of the item
        public Double ItemOrder { get; set; }          //Item order in the group
        public string DatabaseFieldName { get; set; }   //Item name
        public string Header { get; set; }
        public string IncludedTypeID { get; set; }

        public ItemType()
        {

        }
    }
}
