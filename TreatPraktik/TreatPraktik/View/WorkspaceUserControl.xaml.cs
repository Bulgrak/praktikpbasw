using System;
using System.Collections.Generic;
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
        WorkspaceViewModel viewModel = new WorkspaceViewModel();
        public WorkspaceUserControl()
        {
            InitializeComponent();
            
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl item = sender as TabControl;
            if (item != null)
            {
                PageType hej = (PageType)item.SelectedItem;
                viewModel.PageTypeSelected = hej; 
            }
            int i = 1;
        }
    }
}
