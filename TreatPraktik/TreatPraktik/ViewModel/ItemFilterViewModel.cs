using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TreatPraktik.Model;

namespace TreatPraktik.ViewModel
{
    class ItemFilterViewModel
    {
        //private ICollectionView designItemsView;
        private string filterString;
        public string Language { get; set; }
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
            filterString = "";
            PopulateToolbox();
        }

        public void PopulateToolbox()
        {
            ImportExcel ie = ImportExcel.Instance;
            List<ktUIDesign> designList = ie.WorkSheetUIDesign.ktUIDesignList;
            List<ktResources> resourceList = ie.WorkSheetktResources.ktResourceList;
            List<ktResourceTranslation> resourceTranslationList = ie.WorkSheetktResourceTranslation.ktResourceTranslationList;
            List<ktResourceType> resourceTypeList = ie.WorkSheetktResourceType.ktResourceTypeList;
            ToolboxitemList = (
                //joiner tabeller, der vedrører header
                from a in designList
                join b in resourceList on a.ResxID equals b.ResourceResxID
                join c in resourceTranslationList.Where(d => d.LanguageID.Equals("1")) on b.ResourceID equals c.ResourceID
                join d in resourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on b.ResourceTypeID equals d.ResourceTypeID
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
                    ToolTip = g.TranslationText
                }).ToList();
            DesignItemsView = CollectionViewSource.GetDefaultView(ToolboxitemList);
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

        private bool ItemFilter(object item)
        {
            ToolboxItem toolboxItem = item as ToolboxItem;
            return toolboxItem.Header.ToLower().Contains(filterString.ToLower()); //Case-insensitive
        }
    }
}
