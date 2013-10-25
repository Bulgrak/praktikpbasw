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
        public WorkspaceUserControl()
        {
            InitializeComponent();
            DataContext = WorkspaceViewModel.Instance;
            myTabControl.SelectedIndex = 0;
        }

        private void myTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            if (e.AddedItems.Count > 0)
            {
                PageType pt = (PageType)e.AddedItems[0];

                wvm.SelectedPage = pt.PageTypeID;
            }
            else
            {
                wvm.SelectedPage = "15";
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
            }
        }
    }
}
