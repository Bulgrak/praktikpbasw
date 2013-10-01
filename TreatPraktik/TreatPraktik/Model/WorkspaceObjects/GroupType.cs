using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class GroupType : INotifyPropertyChanged
    {
        public string PageTypeID { get; set; }
        public string GroupTypeID { get; set; }                         //Group ID
        //private string _groupName { get; set; }                         //Group name
        public double GroupOrder { get; set; }                          //Group order on the page
        private string groupHeader { get; set; }                         //The real name for the group
        private double groupOrder { get; set; }                          //Group order on the page
        public string GroupHeader { get; set; }                         //The real name for the group
        public string DepartmentID { get; set; }                        //Department ID for the group
        public ObservableCollection<ItemType> Items { get; set; }       //Items in the group
        private string languageID { get; set; }
        public string DanishTranslationText { get; set; }
        public string EnglishTranslationText { get; set; }
        public string ResourceType { get; set; }
        


        public GroupType()
        {
            Items = new ObservableCollection<ItemType>();
        }

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

        public string GroupHeader
        {
            get
            {
                return groupHeader;
            }
            set
            {
                groupHeader = value;
                OnPropertyChanged("GroupHeader");
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
                this.languageID = value;
                switch (languageID)
                {
                    case "1": GroupHeader = EnglishTranslationText; break;
                    case "2": GroupHeader = DanishTranslationText; break;
                    default: GroupHeader = EnglishTranslationText; break;
                }
            }
        }
        //public string GroupName
        //{
        //    get
        //    {
        //        return _groupName;
        //    }
        //    set
        //    {
        //        _groupName = value;
        //        OnPropertyChanged("GroupName");
        //    }
        //}

        public double GroupOrder
        {
            get 
            { 
                return groupOrder; 
            }
            set 
            {
                groupOrder = value;
                OnPropertyChanged("GroupOrder");
            }
        }
    }
}
