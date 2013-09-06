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
        private ICollectionView designItemsView;
        private string filterString;
        public string Language { get; set; }

        public ICollectionView DesignItemsView
        {
            get { return designItemsView; }
            set { designItemsView = value; }
        }

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
            //IList<ktUIDesign> ktUIDesignItems = ktUIDesignItems();
            ImportExcel ie = new ImportExcel();
            Language = "English";
            IList<ktUIDesign> ktUIDesignItems = ie.WorkSheetUIDesign.ktUIDesignList;
            IList<ktResources> ktResources = ie.WorkSheetktResources.ktResourceList;
            IList<ktResourceTranslation> ktResourceTranslations = ie.WorkSheetktResourceTranslation.ktResourceTranslationList;
            IList<ktResourceTranslation> ListBoxItems = new List<ktResourceTranslation>();
            //if (Language.Equals("English"))
            //{
            //    foreach(ktUIDesign design in ktUIDesignItems)
            //    {
            //        bool found = false;
            //        int i = 0;
            //        while (i < ktResources.Count)
            //        {
            //            if (design.ResxID.Equals(ktResources[i].ResourceResxID))
            //            {
            //                found = true;
                            
            //            }
            //        }
            //    }
            //    IList<ktResourceTranslation> ktUIResourceTranslations = ie.WorkSheetktResourceTranslation.ktResourceTranslationList;
            //}
            //IList<ktResources> ktResources
            //List<ktUIDesign> uiDesignItems = LoadTestDesignItems();
            filterString = "";
            designItemsView = CollectionViewSource.GetDefaultView(ktUIDesignItems);
            designItemsView.Filter = ItemFilter;
            //DesignItemsView.
        }


        //public List<ktUIDesign> LoadTestDesignItems() //Test - Bare udkommenter
        //{
        //    List<ktUIDesign> itemsList = new List<ktUIDesign>();
        //    int i = 0;
        //    while (i < 5)
        //    {
        //        ktUIDesign d1 = new ktUIDesign();
        //        d1.DatabaseFieldName = "Test" + i;
        //        d1.DesignID = i;
        //        itemsList.Add(d1);
        //        i++;
        //    }

        //    return itemsList;
        //}

        public List<ktUIDesign> LoadDesignItems()
        {
            return null;
        }

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged("FilterString");
                designItemsView.Refresh();
            }
        }

        private bool ItemFilter(object item)
        {
            ktUIDesign ktUIDesign = item as ktUIDesign;
            return ktUIDesign.DatabaseFieldName.ToLower().Contains(filterString.ToLower()); //Case-insensitive
        }
    }
}
