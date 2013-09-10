using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkspaceObjects
{
    class PageType
    {
        public string PageTypeID { get; set; }             //The page ID
        public string PageName { get; set; }            //The page name
        public List<GroupType> Groups { get; set; }     //List of groups on the page

        public PageType()
        {

        }
    }
}
