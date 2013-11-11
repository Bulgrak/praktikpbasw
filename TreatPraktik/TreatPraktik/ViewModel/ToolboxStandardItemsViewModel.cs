using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TreatPraktik.Model;
using TreatPraktik.Model.ExcelObjects;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class ToolboxStandardItemsViewModel : INotifyPropertyChanged
    {
        private string filterString;
        private string textSearchDescription;
        public string languageID { get; set; }
        public string textNoToolboxItemsFound;
        public ICollectionView DesignItemsView { get; set; }
        public List<ToolboxItem> ToolboxItemList { get; set; }
        private static ToolboxStandardItemsViewModel instance;

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

        private ToolboxStandardItemsViewModel()
        {
            ToolboxItemList = new List<ToolboxItem>();
            LanguageID = "2";
            filterString = "";
        }

        public static ToolboxStandardItemsViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToolboxStandardItemsViewModel();
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
                        TextSearchDescription = "Search for item or in description";
                        TextNoToolboxItemsFound = "No results found";
                        break;
                    case "2":
                        TextSearchDescription = "Søg efter item eller i beskrivelsen";
                        TextNoToolboxItemsFound = "Ingen resultater fundet";
                        break;
                }
                foreach (ToolboxItem tbi in ToolboxItemList)
                {
                    tbi.ItemType.LanguageID = languageID;
                }
                if (DesignItemsView != null)
                {
                    DesignItemsView.Refresh();
                }
            }
        }

        public void PopulateToolbox()
        {
            CreateToolboxItems();
            SetupToolBoxItemCollectionView();
            //DesignItemsView.Refresh();
        }

        public void SetupToolBoxItemCollectionView()
        {
            DesignItemsView = CollectionViewSource.GetDefaultView(ToolboxItemList);

            DesignItemsView.GroupDescriptions.Add(new PropertyGroupDescription("ItemType.Category"));
            //            DesignItemsView.SortDescriptions.Add(
            //  new SortDescription("Group", ListSortDirection.Ascending));
            DesignItemsView.SortDescriptions.Add(
                new SortDescription("ItemType.Header", ListSortDirection.Ascending));

            DesignItemsView.Filter = ItemFilter;
        }

        private void CreateToolboxItems()
        {
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            foreach (ItemType itemType in wvm.ItemTypeList)
            {
                ToolboxItem tbi = new ToolboxItem();
                if(itemType != null && !itemType.DesignID.Equals("197") && !itemType.DesignID.Equals("198"))
                {
                    tbi.ItemType = itemType;
                    ToolboxItemList.Add(tbi);
                }
                
            }
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

        public string TextNoToolboxItemsFound
        {
            get { return textNoToolboxItemsFound; }
            set
            {
                textNoToolboxItemsFound = value;
                OnPropertyChanged("TextNoToolboxItemsFound");
            }
        }

        private bool ItemFilter(object item)
        {
            ToolboxItem toolboxItem = item as ToolboxItem;
            ItemType it = toolboxItem.ItemType;
            string header = it.Header;
            string toolTip = it.ToolTip;
            if (toolTip != null)
            {
                return header.ToLower().Contains(filterString.ToLower()) ||
                       toolTip.ToLower().Contains(filterString.ToLower()); //Case-insensitive
            }
            else
            {
                return header.ToLower().Contains(filterString.ToLower());
            }
        }
    }
}
