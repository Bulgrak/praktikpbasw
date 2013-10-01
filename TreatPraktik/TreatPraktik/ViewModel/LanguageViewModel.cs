﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    class LanguageViewModel
    {
        private WorkspaceViewModel WorkspaceVM;

        public LanguageViewModel()
        {
            WorkspaceVM = WorkspaceViewModel.Instance;
        }

        public void ChangeLanguage(string languageID)
        {
            foreach (PageType page in WorkspaceVM.PageList)
            {
                page.LanguageID = languageID;

                foreach (GroupType group in page.Groups)
                {
                    group.LanguageID = languageID;

                    foreach (ItemType item in group.Items)
                    {
                        item.LanguageID = languageID;
                    }
                }
            }
        }
    }
}
