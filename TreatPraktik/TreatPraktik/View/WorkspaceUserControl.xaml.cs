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
        public WorkspaceUserControl()
        {
            InitializeComponent();
            DataContext = WorkspaceViewModel.Instance;
            myTabControl.SelectedIndex = 0;
        }

        private void myTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageType pt = (PageType)e.AddedItems[0];
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            wvm.SelectedPage = pt.PageTypeID;
        }
    }
}
