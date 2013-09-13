using System;
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
        public WorkspaceUserControl()
        {
            InitializeComponent();
            WorkspaceViewModel wvm = new WorkspaceViewModel();
            ObservableCollection<PageType> pageTypeList = wvm.PageList;

            for (int i = 0; i < pageTypeList.Count; i++)
            {
                PageType pt = pageTypeList[i];
                TabItem ti = new TabItem();
                ti.Header = pt.PageName;

                if (pt.PageTypeID.Equals("15") || pt.PageTypeID.Equals("16") || pt.PageTypeID.Equals("17")) //burde nok gøres i LINQ
                {
                    Group ucGroup = new Group();
                    ucGroup.groups = pageTypeList[i].Groups;
                    ucGroup.PopulateGrid();
                    ti.Content = ucGroup;

                    myTabControl.Items.Add(ti);
                    //myTabControl;
                }
            }
        }
    }
}
