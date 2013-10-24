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
using TreatPraktik.ViewModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for GroupListUserControl.xaml
    /// </summary>
    public partial class GroupListUserControl : UserControl
    {
        public GroupListUserControl()
        {
            InitializeComponent();
            GroupListViewModel glvm = GroupListViewModel.Instance;
            DataContext = glvm;
            glvm.PopulateToolbox();
        }
    }
}
