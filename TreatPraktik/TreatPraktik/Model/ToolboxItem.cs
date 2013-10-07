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
        private string header { get; set; }
        public string ToolTip { get; set; }
        public string Category { get; set; }
        private string languageID { get; set; }
        public string DanishTranslationText { get; set; }
        public string EnglishTranslationText { get; set; }

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
                return header;
            }
            set
            {
                header = value;
                OnPropertyChanged("Header");
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
                    case "1": Header = EnglishTranslationText; break;
                    case "2": Header = DanishTranslationText; break;
                    default: Header = EnglishTranslationText; break;
                }
            }
        }
    }
}
