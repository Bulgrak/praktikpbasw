using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class GroupTypeOrder : INotifyPropertyChanged
    {
        public string DepartmentID { get; set; }
        public string PageTypeID { get; set; }
        public string GroupTypeID { get; set; }
        public GroupType Group { get; set; }
        private double groupOrder { get; set; }


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
