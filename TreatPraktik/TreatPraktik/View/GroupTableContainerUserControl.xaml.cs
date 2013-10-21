using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for GroupContainerUserControl.xaml
    /// </summary>
    public partial class GroupTableContainerUserControl : UserControl
    {
        public ObservableCollection<GroupTypeOrder> GtoObsCollection { get; set; }

        public GroupTableContainerUserControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
