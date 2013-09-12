using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkspaceObjects
{
    class PageType : INotifyPropertyChanged
    {
        public string PageTypeID { get; set; }                          //The page ID
        private string _pageName { get; set; }                          //The page name
        private ObservableCollection<GroupType> _groups { get; set; }     //List of groups on the page


        public PageType()
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

        public string PageName
        {
            get
            {
                return _pageName;
            }
            set
            {
                _pageName = value;
                OnPropertyChanged("PageName");
            }
        }

        public ObservableCollection<GroupType> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
                OnPropertyChanged("Groups");
            }
        }

    }
}
