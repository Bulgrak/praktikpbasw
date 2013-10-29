using System;
using System.ComponentModel;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class ItemType : INotifyPropertyChanged
    {
        public string ResourceType { get; set; }
        //public string GroupTypeID { get; set; }
        public string DesignID { get; set; }            //ID of the item
        //public Double ItemOrder { get; set; }          //Item order in the group
        private string _header;
        private string _toolTip;
        
        private string _languageID;
        public string DanishTranslationText { get; set; }
        public string EnglishTranslationText { get; set; }
        public string DanishTranslationToolTip { get; set; }
        public string EnglishTranslationToolTip { get; set; }

        public ItemType()
        {

        }

        public ItemType(string danish, string designId, string english, string groupTypeId, string header, 
            string includedTypeId, double itemOrder, string languageId, string resourceType)
        {
            this.ResourceType = resourceType;
            //this.GroupTypeID = groupTypeId;
            this.DesignID = designId;
            //this.ItemOrder = itemOrder;
            this.Header = header;
            //this.IncludedTypeID = includedTypeId;
            this.LanguageID = languageId;
            this.DanishTranslationText = danish;
            this.EnglishTranslationText = english;
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

        public string Header
        {
            get 
            { 
                return _header;
            }
            set 
            { 
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        public string ToolTip
        {
            get
            {
                return _toolTip;
            }
            set
            {
                _toolTip = value;
                OnPropertyChanged("ToolTip");
            }
        }

        public string LanguageID
        {
            get
            {
                return _languageID;
            }
            set
            {
                this._languageID = value;
                if(DesignID != null && !DesignID.Equals("197") && !DesignID.Equals("198"))
                {
                    switch (_languageID)
                    {
                        case "1": 
                            Header = EnglishTranslationText;
                            ToolTip = EnglishTranslationToolTip;
                            break;
                        case "2": 
                            Header = DanishTranslationText;
                            ToolTip = DanishTranslationToolTip;
                            break;
                        default: 
                            Header = EnglishTranslationText; 
                            break;
                    }
                }
            }
        }
    }
}
