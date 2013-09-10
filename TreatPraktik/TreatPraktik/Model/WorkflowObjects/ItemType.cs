using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkflowObjects
{
    class ItemType
    {
        public string DesignID { get; set; }            //ID of the item
        public int GroupOrder { get; set; }             //Item order in the group
        public string DatabaseFieldName { get; set; }   //Item name

        public ItemType()
        {

        }
    }
}
