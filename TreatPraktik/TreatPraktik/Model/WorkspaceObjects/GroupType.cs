using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class GroupType : INotifyPropertyChanged
    {
        public string GroupTypeID { get; set; }                         //Group ID
        private string _groupHeader;                                    //The real name for the group
        public ObservableCollection<ItemTypeOrder> ItemOrder { get; set; }       //Items in the group
        private string _languageID;
        public string DanishTranslationText { get; set; }
        public string EnglishTranslationText { get; set; }
        public string ResourceType { get; set; }
        public string ResourceTypeID { get; set; }
        public string ResourceID { get; set; }

        public GroupType()
        {
            ItemOrder = new ObservableCollection<ItemTypeOrder>();
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
                return _groupHeader;
            }
            set
            {
                _groupHeader = value;
                OnPropertyChanged("GroupHeader");
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
                _languageID = value;
                switch (_languageID)
                {
                    case "1": GroupHeader = EnglishTranslationText; break;
                    case "2": GroupHeader = DanishTranslationText; break;
                    default: GroupHeader = EnglishTranslationText; break;
                }
            }
        }
    }
}
