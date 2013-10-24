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
using TreatPraktik.Model;
using TreatPraktik.Model.ExcelObjects;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class GroupListViewModel : INotifyPropertyChanged
    {
        //private ICollectionView designItemsView;
        private string filterString;
        private string textSearchDescription;
        public string languageID { get; set; }
        public string textNoGroupsFound;
        //public string TextSearchDescription { get; set; } //Beskrivelse
        public ICollectionView DesignItemsView { get; set; }
        public ObservableCollection<GroupType> GTList { get; set; }
        private static GroupListViewModel instance;

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

        private GroupListViewModel()
        {
            //TextNoToolboxItemsFound = "No items to display";
            //LanguageID = "2";
            GTList = new ObservableCollection<GroupType>();
            LanguageID = "2";
            filterString = "";
            //PopulateToolbox();
        }

        private void PopulateGTList()
        {
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            foreach (GroupTypeOrder gto in wvm.GroupList)
            {

                GTList.Add(gto.Group);
                SetupToolBoxItemCollectionView();
            }
        }


        public static GroupListViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GroupListViewModel();
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
                foreach (GroupType tbi in GTList)
                {
                    tbi.LanguageID = languageID;
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
            DesignItemsView.SortDescriptions.Add(new SortDescription("GroupHeader", ListSortDirection.Ascending));
            DesignItemsView.Filter = ItemFilter;
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
            GroupType gtItem = item as GroupType;
            string header = gtItem.GroupHeader;
            return header.ToLower().Contains(filterString.ToLower()); //Case-insensitive
        }
    }
}
