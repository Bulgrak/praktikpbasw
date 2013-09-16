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
    class ItemFilterViewModel
    {
        //private ICollectionView designItemsView;
        private string filterString;
        private string textSearchDescription;
        public string Language { get; set; }
        public string TextNoToolboxItemsFound { get; set; }
        //public string TextSearchDescription { get; set; } //Beskrivelse
        public List<ToolboxItem> ToolboxitemList { get; set; }

        public ICollectionView DesignItemsView { get; set;}

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        public ItemFilterViewModel()
        {
            Language = "English";
            //TextNoToolboxItemsFound = "No items to display";
            TextNoToolboxItemsFound = "No results found";
            TextSearchDescription = "Search for item or in description";
            filterString = "";
            PopulateToolbox();
        }

        public void ChangeLanguage(string language)
        {
            Language = language;
        }

        public void PopulateToolbox()
        {
            ImportExcel ie = ImportExcel.Instance;
            List<ktUIDesign> designList = ie.WorkSheetUIDesign.ktUIDesignList;
            List<ktResources> resourceList = ie.WorkSheetktResources.ktResourceList;
            List<ktResourceTranslation> resourceTranslationList = ie.WorkSheetktResourceTranslation.ktResourceTranslationList;
            List<ktResourceType> resourceTypeList = ie.WorkSheetktResourceType.ktResourceTypeList;
            List<QAktUIDesign> qaktuidesignList = ie.WorkSheetQAktUIDesign.QAktUIDesignList;
            List<QAGroup> qagrouplist = ie.WorkSheetQAGroups.QAGroupsList;
            ToolboxitemList = (
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
                    Header = c.TranslationText,
                    ToolTip = g.TranslationText,
                    Category = j.Type
                }).ToList();

            DesignItemsView = CollectionViewSource.GetDefaultView(ToolboxitemList);
            DesignItemsView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
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

        private bool ItemFilter(object item)
        {
            ToolboxItem toolboxItem = item as ToolboxItem;
            string header = toolboxItem.Header;
            string toolTip = toolboxItem.ToolTip;
            return header.ToLower().Contains(filterString.ToLower()) || toolTip.ToLower().Contains(filterString.ToLower()); //Case-insensitive
        }


    }
}
