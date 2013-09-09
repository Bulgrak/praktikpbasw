using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkflowObjects
{
    class GroupType
    {
        public string GroupName { get; set; }           //Group name
        public int GroupOrder { get; set; }             //Group order on the page
        public List<ItemType> Items { get; set; }       //Items in the group

    }
}
