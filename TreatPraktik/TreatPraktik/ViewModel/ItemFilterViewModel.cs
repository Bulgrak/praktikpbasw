using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TreatPraktik.Model;
using TreatPraktik.Model.ExcelObjects;

namespace TreatPraktik.ViewModel
{
    public class ItemFilterViewModel : INotifyPropertyChanged
    {
        //private ICollectionView designItemsView;
        private string filterString;
        private string textSearchDescription;
        public string languageID { get; set; }
        public string textNoToolboxItemsFound;
        //public string TextSearchDescription { get; set; } //Beskrivelse
        public ICollectionView DesignItemsView { get; set; }
        public List<ToolboxItem> ToolboxItemList { get; set; }
        private static ItemFilterViewModel instance;

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

        private ItemFilterViewModel()
        {
            ToolboxItemList = new List<ToolboxItem>();
            //TextNoToolboxItemsFound = "No items to display";
            
            //LanguageID = "2";
            LanguageID = "2";
            filterString = "";
            PopulateToolbox();
        }

        public static ItemFilterViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemFilterViewModel();
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
            ToolboxItemList = CreateToolboxItems();

            DesignItemsView = CollectionViewSource.GetDefaultView(ToolboxItemList);
            DesignItemsView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
//            DesignItemsView.SortDescriptions.Add(
  //  new SortDescription("Group", ListSortDirection.Ascending));
            DesignItemsView.SortDescriptions.Add(
                new SortDescription("Header", ListSortDirection.Ascending));

            DesignItemsView.Filter = ItemFilter;
        }

        private List<ToolboxItem> CreateToolboxItems()
        {
            List<ToolboxItem> toolboxItemList = new List<ToolboxItem>();
            ImportExcel ie = ImportExcel.Instance;
            List<ktUIDesign> designList = ie.WorkSheetUIDesign.ktUIDesignList;
            List<ktResources> resourceList = ie.WorkSheetktResources.ktResourceList;
            List<ktResourceTranslation> resourceTranslationList = ie.WorkSheetktResourceTranslation.ktResourceTranslationList;
            List<ktResourceType> resourceTypeList = ie.WorkSheetktResourceType.ktResourceTypeList;
            List<QAktUIDesign> qaktuidesignList = ie.WorkSheetQAktUIDesign.QAktUIDesignList;
            List<QAGroup> qagrouplist = ie.WorkSheetQAGroups.QAGroupsList;
            toolboxItemList = (
                //joiner tabeller, der vedrører header
                from a in designList
                join b in resourceList on a.ResxID equals b.ResourceResxID
                join c in resourceTranslationList.Where(d => d.LanguageID.Equals("1")) on b.ResourceID equals c.ResourceID
                join d in resourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on b.ResourceTypeID equals d.ResourceTypeID
                join i in qaktuidesignList on a.DesignID equals i.DesignID
                join j in qagrouplist on i.TypeID equals j.TypeID
                //joiner tabeller, der vedrører tooltips
                join f in resourceList on a.ResxID equals f.ResourceResxID
                join g in resourceTranslationList.Where(d => d.LanguageID.Equals("1")) on f.ResourceID equals g.ResourceID
                join h in resourceTypeList.Where(d => d.ResourceTypeID.Equals("3")) on f.ResourceTypeID equals h.ResourceTypeID

                select new ToolboxItem
                {
                    DesignID = a.DesignID,
                    ResourceID = b.ResourceID,
                    ResxID = a.ResxID,
                    ResourceType = b.ResourceTypeID,
                    Header = c.TranslationText,
                    ToolTip = g.TranslationText,
                    Category = j.Type
                }).ToList();

            var query = (from aa in ie.WorkSheetktResources.ktResourceList
                         join bb in ie.WorkSheetktResourceTranslation.ktResourceTranslationList on aa.ResourceID equals bb.ResourceID
                         join cc in ie.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on aa.ResourceTypeID equals cc.ResourceTypeID
                         select new
                         {
                             bb.LanguageID,
                             aa.ResourceResxID,
                             bb.TranslationText
                         }).ToList();

            foreach (ToolboxItem itemType in toolboxItemList)
            {
                foreach (var item in query)
                {
                    if (item.ResourceResxID == itemType.ResxID)
                    {
                        if (item.LanguageID.Equals("2"))
                        {
                            itemType.DanishTranslationText = item.TranslationText;

                        }
                        else if (item.LanguageID.Equals("1"))
                        {
                            itemType.EnglishTranslationText = item.TranslationText;
                        }
                    }
                }
                itemType.LanguageID = "2";
            }

            return toolboxItemList;
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
            string header = toolboxItem.Header;
            string toolTip = toolboxItem.ToolTip;
            return header.ToLower().Contains(filterString.ToLower()) || toolTip.ToLower().Contains(filterString.ToLower()); //Case-insensitive
        }
    }
}
