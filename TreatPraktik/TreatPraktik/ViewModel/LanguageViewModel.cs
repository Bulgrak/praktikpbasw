using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    class LanguageViewModel
    {
        private WorkspaceViewModel WorkspaceVM;
        private ItemFilterViewModel itemFilterVM;

        public LanguageViewModel()
        {
            WorkspaceVM = WorkspaceViewModel.Instance;
            itemFilterVM = ItemFilterViewModel.Instance;
        }

        public void ChangeLanguage(string languageID)
        {
            foreach (PageType page in WorkspaceVM.PageList)
            {
                page.LanguageID = languageID;

                foreach (GroupTypeOrder group in page.GroupTypeOrders)
                {
                    group.Group.LanguageID = languageID;

                    foreach (ItemTypeOrder item in group.Group.ItemOrder)
                    {
                        item.Item.LanguageID = languageID;
                    }
                }
            }
            itemFilterVM.LanguageID = languageID;
        }
    }
}
