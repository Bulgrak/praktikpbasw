using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class PageType : INotifyPropertyChanged
    {
        public string ResourceType { get; set; }
        public string PageTypeID { get; set; }                          //The page ID
        private string pageName;                                        //The page name
        private string languageID { get; set; }
        public string DanishTranslationText { get; set; }
        public string EnglishTranslationText { get; set; }
        //public List<string> GroupTypeIDs { get; set; }
        public ObservableCollection<GroupTypeOrder> Groups { get; set; }     //List of groups on the page

        public PageType()
        {
            //GroupTypeIDs = new List<string>();
            Groups = new ObservableCollection<GroupTypeOrder>();
        }

        public string PageName
        {
            get
            {
                return pageName;
            }
            set
            {
                pageName = value;
                OnPropertyChanged("PageName");
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
                    case "1": PageName = EnglishTranslationText; break;
                    case "2": PageName = DanishTranslationText; break;
                    default: PageName = EnglishTranslationText; break;
                }
            }
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
    }
}
