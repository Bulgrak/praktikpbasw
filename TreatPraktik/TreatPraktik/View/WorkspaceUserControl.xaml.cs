using System;
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
        public Dictionary<string, Double> ScrollPositions { get; set; } // Binding causes reusing of scrollviewer (only one instance), so we need to save the scroll position for each tabitem
        public WorkspaceUserControl()
        {
            InitializeComponent();
            DataContext = WorkspaceViewModel.Instance;
            ScrollPositions = new Dictionary<string, double>();
            //SetupInitialScrollPositions();
            myTabControl.SelectedIndex = 0;
        }

        public void SetupInitialScrollPositions()
        {
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            foreach (PageType pt in wvm.PageList)
            {
                ScrollPositions.Add(pt.PageTypeID, 0);
            }
        }

        private void ResetScrollPositions()
        {
            int i = 0;
            while (i < ScrollPositions.Count)
            {
                KeyValuePair<string, double> item = ScrollPositions.ElementAt(i);
                string pageTypeID = item.Key;
                ScrollPositions[pageTypeID] = 0;
                i++;
            }
        }

        private void myTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            ScrollViewer sv = FindVisualChildByName<ScrollViewer>(this.myTabControl, "gtSv");
            if (sv != null)
            {
                Double test = sv.VerticalOffset;
                ScrollPositions[wvm.SelectedPage] = sv.VerticalOffset;
            }
            if (e.AddedItems.Count > 0)
            {
                PageType pt = (PageType)e.AddedItems[0];

                wvm.SelectedPage = pt.PageTypeID;
                if (sv != null)
                {
                    Double position = ScrollPositions[wvm.SelectedPage];
                    sv.ScrollToVerticalOffset(position);
                }
            }
            else
            {
                wvm.SelectedPage = "15";
                ResetScrollPositions();
                sv.ScrollToTop();
            }
        }

        private void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            string pageTypeID = wvm.SelectedPage;
            CreateGroupDialogBox dlg = new CreateGroupDialogBox();
            ObservableCollection<string> obscDepartmentList = new ObservableCollection<string>();
            obscDepartmentList.Add("-1");
            dlg.departmentUserControl.departmentsListBox.ItemsSource = obscDepartmentList;
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                string danishText = dlg.danishTextBox.Text;
                string englishText = dlg.englishTextBox.Text;
                List<string> departmentList = new List<string>();
                foreach (string s in dlg.departmentUserControl.departmentsListBox.Items)
                    departmentList.Add(s);
                wvm.CreateGroup(pageTypeID, englishText, danishText, departmentList);

                ScrollViewer sv = FindVisualChildByName<ScrollViewer>(this.myTabControl, "gtSv");
                sv.ScrollToBottom();
            }
        }

        private T FindVisualChildByName<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            T child = default(T);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var ch = VisualTreeHelper.GetChild(parent, i);
                child = ch as T;
                if (child != null && child.Name == name)
                    break;
                else
                    child = FindVisualChildByName<T>(ch, name);

                if (child != null) break;
            }
            return child;
        }
    }
}
