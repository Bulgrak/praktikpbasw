﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for WorkspaceUserControl.xaml
    /// </summary>
    public partial class WorkspaceUserControl : UserControl
    {
        //private ObservableCollection<PageType> pageTypeList;
        private ExportExcel exExcel;
        private WorkspaceViewModel wvm;
        public ObservableCollection<PageType> PageList { get; set; }
        public ICollectionView PageTypeItemsView { get; set; }

        public WorkspaceUserControl()
        {
            InitializeComponent();
            
            wvm = WorkspaceViewModel.Instance;
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"Ressources\Configuration.xlsx");
            wvm.LoadWorkspace(path);

            //PageList = wvm.PageList;
            PageTypeItemsView = CollectionViewSource.GetDefaultView(wvm.PageList);
            PageTypeItemsView.Filter = ItemFilter;
            PageTypeItemsView.Refresh();
            DataContext = this;
            //LoadWorkspaceUserControl();
        }

        private bool ItemFilter(object item)
        {
            PageType pt = (PageType) item;
            if (pt.PageTypeID.Equals("15") || pt.PageTypeID.Equals("16") || pt.PageTypeID.Equals("17"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LoadWorkspaceUserControl()
        {
            //pageTypeList = wvm.PageList;
            //wvm.PageList.CollectionChanged += pageTypeList_CollectionChanged;

            //for (int i = 0; i < wvm.PageList.Count; i++)
            //{
            //    PageType pt = wvm.PageList[i];
            //    TabItem ti = new TabItem();
            //    ti.DataContext = pt;
            //    ti.SetBinding(TabItem.HeaderProperty, "PageName");
            //    //ti.Header = pt.PageName;

            //    if (pt.PageTypeID.Equals("15") || pt.PageTypeID.Equals("16") || pt.PageTypeID.Equals("17")) //burde nok gøres i LINQ
            //    {
            //        //ObservableCollection<GroupType> groupType = new ObservableCollection<GroupType>();
            //        //foreach (GroupTypeOrder gto in pt.Groups)
            //        //{
            //        //    groupType.Add(gto.Group);
            //        //}
            //        GroupTableContainerUserControl ucGroupTableContainerUserControl = new GroupTableContainerUserControl();
            //        ucGroupTableContainerUserControl.GtoObsCollection = pt.Groups;
            //        //ucGroupTableContainerUserControl.CreateGroupTables();

            //        ti.Content = ucGroupTableContainerUserControl;

            //        myTabControl.Items.Add(ti);
            //    }
            //}

        }
    }
}
