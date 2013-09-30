﻿using System;
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
        public string GroupHeader { get; set; }                         //The real name for the group
        public string DepartmentID { get; set; }                        //Department ID for the group
        public ObservableCollection<ItemType> Items { get; set; }       //Items in the group
        

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
    }
}
