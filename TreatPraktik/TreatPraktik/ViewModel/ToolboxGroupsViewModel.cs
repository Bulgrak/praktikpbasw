using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using DocumentFormat.OpenXml.Presentation;
using TreatPraktik.Model;
using TreatPraktik.Model.ExcelObjects;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class ToolboxGroupsViewModel : INotifyPropertyChanged
    {
        private string filterString;
        private string textSearchDescription;
        public string languageID { get; set; }
        public string textNoGroupsFound;
        public ICollectionView DesignItemsView { get; set; }
        public ObservableCollection<ToolboxGroup> GTList { get; set; }
        private static ToolboxGroupsViewModel instance;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private ToolboxGroupsViewModel()
        {
            GTList = new ObservableCollection<ToolboxGroup>();
            LanguageID = "2";
            filterString = "";
        }

        public void PopulateGTList()
        {
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            List<string> checkList = new List<string>();
            List<string> blackList = new List<string>();
            //Blacklist the following pages: 3, 18, 19, 255, 100
            //foreach (PageType pt in wvm.PageList)
            //{
            //    if (
            //        pt.PageTypeID.Equals("3") ||
            //        pt.PageTypeID.Equals("18") ||
            //        pt.PageTypeID.Equals("19") ||
            //        pt.PageTypeID.Equals("255") ||
            //        pt.PageTypeID.Equals("100")
            //        )
            //    {
            //        foreach (GroupTypeOrder gto in pt.GroupTypeOrders)
            //        {
  
            //            blackList.Add(gto.GroupTypeID);
            //        }
            //    }
            //}

            //foreach (PageType pt in wvm.PageList)
            //{

            //    foreach (GroupTypeOrder gto in pt.GroupTypeOrders)
            //    {
            //        if (!gto.GroupTypeID.Equals("60") && !gto.GroupTypeID.Equals("58") && !blackList.Exists(x => x.Equals(gto.GroupTypeID)) && !checkList.Exists(x => x.Equals(gto.GroupTypeID)))
            //        {
            //            ToolboxGroup tbGroup = new ToolboxGroup();
            //            tbGroup.Group = gto.Group;
            //            GTList.Add(tbGroup);
            //            //GTList.Add(gto.Group);
            //            checkList.Add(gto.GroupTypeID);
            //        }
            //    }
            //}


            foreach (PageType pt in wvm.PageList)
            {
                //3, 18, 19, 255, 100
                if (
                    !pt.PageTypeID.Equals("3") &&
                    !pt.PageTypeID.Equals("18") &&
                    !pt.PageTypeID.Equals("19") &&
                    !pt.PageTypeID.Equals("255") &&
                    !pt.PageTypeID.Equals("100")
                    )
                {
                    foreach (GroupTypeOrder gto in pt.GroupTypeOrders)
                    {
                        if (!gto.GroupTypeID.Equals("60") && !gto.GroupTypeID.Equals("58"))
                        {
                            int i = 0;
                            bool alreadyExist = false;
                            while (i < checkList.Count && !alreadyExist)
                            {
                                if (checkList[i].Equals(gto.GroupTypeID))
                                    alreadyExist = true;
                                i++;
                            }

                            if (!alreadyExist)
                            {

                                ToolboxGroup tbGroup = new ToolboxGroup();
                                tbGroup.Group = gto.Group;
                                GTList.Add(tbGroup);
                                //GTList.Add(gto.Group);
                                checkList.Add(gto.GroupTypeID);
                            }
                        }
                    }
                }
            }

            SetupToolBoxItemCollectionView();
        }


        public static ToolboxGroupsViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToolboxGroupsViewModel();
                }
                return instance;
            }
        }

        public string LanguageID
        {
            get
            {
                return languageID;
            }
            set
            {
                languageID = value;
                switch (LanguageID)
                {
                    case "1":
                        TextSearchDescription = "Search for a group";
                        TextNoGroupsFound = "No results found";
                        break;
                    case "2":
                        TextSearchDescription = "Søg efter gruppe";
                        TextNoGroupsFound = "Ingen resultater fundet";
                        break;
                }
                foreach (ToolboxGroup tbGroup in GTList)
                {
                    tbGroup.Group.LanguageID = languageID;
                }
                if (DesignItemsView != null)
                {
                    DesignItemsView.Refresh();
                }
            }
        }

        public void PopulateToolbox()
        {

            //DesignItemsView.Refresh();
        }

        public void SetupToolBoxItemCollectionView()
        {
            DesignItemsView = CollectionViewSource.GetDefaultView(GTList);
            DesignItemsView.SortDescriptions.Add(new SortDescription("Group.GroupHeader", ListSortDirection.Ascending));
            DesignItemsView.Filter = ItemFilter;
            DesignItemsView.Refresh();
        }

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged("FilterString");
                DesignItemsView.Refresh();
            }
        }

        public string TextSearchDescription
        {
            get { return textSearchDescription; }
            set
            {
                textSearchDescription = value;
                OnPropertyChanged("TextSearchDescription");
            }
        }

        public string TextNoGroupsFound
        {
            get { return textNoGroupsFound; }
            set
            {
                textNoGroupsFound = value;
                OnPropertyChanged("TextNoToolboxItemsFound");
            }
        }

        private bool ItemFilter(object item)
        {
            ToolboxGroup tbGroup = item as ToolboxGroup;
            string header = tbGroup.Group.GroupHeader;
            return header.ToLower().Contains(filterString.ToLower()); //Case-insensitive
        }
    }
}
