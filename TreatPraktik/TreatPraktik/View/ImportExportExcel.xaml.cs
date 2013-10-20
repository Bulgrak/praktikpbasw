using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;
using System.Linq;
using System.Windows.Data;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for ImportExportExcel.xaml
    /// </summary>
    public partial class ImportExportExcel : UserControl
    {
        private readonly ExportExcel _exExcel;
        private WorkspaceViewModel _workspaceVM;

        public ImportExportExcel()
        {
            InitializeComponent();
            _exExcel = ExportExcel.Instance;
            _workspaceVM = WorkspaceViewModel.Instance;
        }

        /// <summary>
        /// Not Implemented yet
        /// </summary>
        private void ImportExcel_Click(object sender, RoutedEventArgs e)
        {
            _workspaceVM.CreateGroup("15", "1", 8.0, "New group", "Ny gruppe");
            //_workspaceVM.CreateGroup("15", "1", 9.0, "Jesper", "Er sej");

            //_workspaceVM.RenameGroup("15", "0", "Rename group", "Omdøb gruppe");
            
            MessageBox.Show("Ikke implementeret");
        }

        /// <summary>
        /// Button click opens a SaveFileDialog
        /// Choosen file path is passed to the ExportExcel class
        /// </summary>
        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
            saveFile.DefaultExt = ".xlsx";
            saveFile.Filter = "Excel file (.xlsx)|*.xlsx";

            if (saveFile.ShowDialog() == true)
            {
                _exExcel.CreateNewExcel(saveFile.FileName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PageType> pl = _workspaceVM.PageList;
            GroupTypeOrder gto = pl[14].Groups[1];
            pl[14].Groups[0].GroupOrder = 2;
            pl[14].Groups[1].GroupOrder = 1;
            pl[14].Groups.Remove(gto);
            //List<GroupTypeOrder> gtoList = pl[14].Groups.ToList();
            //gtoList = gtoList.OrderBy(o => o.GroupOrder).ToList();
            //ObservableCollection<GroupTypeOrder> gtoListSorted = new ObservableCollection<GroupTypeOrder>(gtoList);
            //pl[14].Groups.RemoveAt(0);
            pl[14].Groups.Insert(0, gto);
            //CollectionViewSource.GetDefaultView(gtoListSorted).Refresh();
            //pl[14].Groups = gtoListSorted;
            
        }
    }
}
