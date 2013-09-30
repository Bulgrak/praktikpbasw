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
        public string PageTypeID { get; set; }                          //The page ID
        public string PageName { get; set; }                          //The page name
        public ObservableCollection<GroupType> Groups { get; set; }     //List of groups on the page


        public PageType()
        {
            Groups = new ObservableCollection<GroupType>();
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
