using System;
using System.ComponentModel;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class ItemType : INotifyPropertyChanged
    {
        public string ResourceType { get; set; }
        public string DesignID { get; set; }            //ID of the item
        private string _header;
        private string _toolTip;
        private string _languageID;
        public string DanishTranslationText { get; set; }
        public string EnglishTranslationText { get; set; }
        public string DanishTranslationToolTip { get; set; }
        public string EnglishTranslationToolTip { get; set; }
        private string _category;


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

        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
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
                            ToolTip = EnglishTranslationToolTip != "" ? EnglishTranslationToolTip : "n/a";
                            break;
                        case "2": 
                            Header = DanishTranslationText;
                            ToolTip = DanishTranslationToolTip != "" ? DanishTranslationToolTip : "n/a";
                            break;
                        default: 
                            Header = EnglishTranslationText != "" ? EnglishTranslationText : "n/a";
                            ToolTip = EnglishTranslationToolTip != "" ? EnglishTranslationToolTip : "n/a";
                            break;
                    }
                }
            }
        }
    }
}
