﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ExportExcel exExcel;
        private WorkspaceViewModel wvm;

        public WorkspaceUserControl()
        {
            InitializeComponent();
            wvm = WorkspaceViewModel.Instance;
            wvm.LoadWorkspace(@"C:\UITablesToStud.xlsx");
            ObservableCollection<PageType> pageTypeList = wvm.PageList;


            for (int i = 0; i < pageTypeList.Count; i++)
            {
                PageType pt = pageTypeList[i];
                TabItem ti = new TabItem();
                ti.DataContext = pt;
                ti.SetBinding(TabItem.HeaderProperty, "PageName");
                //ti.Header = pt.PageName;
                
                if (pt.PageTypeID.Equals("15") || pt.PageTypeID.Equals("16") || pt.PageTypeID.Equals("17")) //burde nok gøres i LINQ
                {
                    //ObservableCollection<GroupType> groupType = new ObservableCollection<GroupType>();
                    //foreach (GroupTypeOrder gto in pt.Groups)
                    //{
                    //    groupType.Add(gto.Group);
                    //}
                    GroupContainerUserControl ucGroupContainerUserControl = new GroupContainerUserControl();
                    ucGroupContainerUserControl.GtoObsCollection = pt.Groups;
                    ucGroupContainerUserControl.CreateGroupTables();
                    ti.Content = ucGroupContainerUserControl;

                    myTabControl.Items.Add(ti);
                }
            }
        }
    }
}
