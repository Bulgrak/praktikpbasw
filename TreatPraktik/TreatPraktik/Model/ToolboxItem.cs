using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    public class ToolboxItem
    {
        public string DesignID { get; set; }
        public string ResourceID { get; set; }
        public string ResxID { get; set; }
        public string ResourceType { get; set; }
        private string _header { get; set; }
        public string _toolTip { get; set; }
        public string Category { get; set; }
        private string languageID { get; set; }
        public string DanishTranslationText { get; set; }
        public string EnglishTranslationText { get; set; }
        public string DanishTranslationToolTip{ get; set; }
        public string EnglishTranslationToolTip { get; set; }

        public ToolboxItem()
        {

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
                _header = value;
                OnPropertyChanged("_toolTip");
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
                    case "1": 
                        Header = EnglishTranslationText;
                        _toolTip = EnglishTranslationToolTip;
                        break;
                    case "2": 
                        Header = DanishTranslationText;
                        _toolTip = DanishTranslationToolTip;
                        break;
                    default: Header = EnglishTranslationText; break;
                }
            }
        }
    }
}
