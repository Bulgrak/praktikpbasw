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

                foreach (GroupTypeOrder group in page.Groups)
                {
                    group.Group.LanguageID = languageID;

                    foreach (ItemType item in group.Group.Items)
                    {
                        item.LanguageID = languageID;
                    }
                }
            }
            itemFilterVM.LanguageID = languageID;
        }
    }
}
