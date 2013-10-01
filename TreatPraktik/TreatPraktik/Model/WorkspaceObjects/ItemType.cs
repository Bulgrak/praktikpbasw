﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class ItemType : INotifyPropertyChanged
    {
        public string GroupTypeID { get; set; }
        public string DesignID { get; set; }            //ID of the item
        public Double ItemOrder { get; set; }          //Item order in the group
        public string DatabaseFieldName { get; set; }   //Item name
        private string header { get; set; }
        //public string English { get; set; }
        //public string Danish { get; set; }
        //public string LanguageID { get; set; }

        public ItemType()
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

        //public string LanguageID
        //{
        //    get
        //    {
        //        return LanguageID;
        //    }
        //    set
        //    {
        //        LanguageID = value;
        //        if(LanguageID == 1)
        //        {
        //        header = English
        //        }
        //        yyy
        //        OnPropertyChanged("Header");
        //    }
        //}
    }
}
