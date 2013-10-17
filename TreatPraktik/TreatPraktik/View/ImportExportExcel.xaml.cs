using System.Windows;
using System.Windows.Controls;
using TreatPraktik.ViewModel;

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
    }
}
