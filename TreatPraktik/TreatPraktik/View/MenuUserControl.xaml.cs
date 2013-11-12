using System;
using System.Collections.Generic;
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
using TreatPraktik.ViewModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        private string languageID;
        private LanguageViewModel languageWM;
        private readonly ExportExcel _exExcel;
        private WorkspaceViewModel _workspaceVM;

        public MenuUserControl()
        {
            InitializeComponent();
            languageWM = new LanguageViewModel();
            _exExcel = ExportExcel.Instance;
            _workspaceVM = WorkspaceViewModel.Instance;
        }

        //Change language to English
        private void English_Click(object sender, RoutedEventArgs e)
        {
            LanguageID = "1";
        }

        //Change language to Danish
        private void Danish_Click(object sender, RoutedEventArgs e)
        {
            LanguageID = "2";
        }

        public string LanguageID
        {
            set
            {
                languageID = value;
                languageWM.ChangeLanguage(LanguageID);
            }
            get
            {
                return languageID;
            }
        }

        private void ImportExcel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.DefaultExt = ".xlsx";
            openFile.Filter = "Excel file (.xlsx)|*.xlsx";

            if (openFile.ShowDialog() == true)
            {
                _workspaceVM.LoadNewConfigurations(openFile.FileName);
                //_workspaceVM.LoadWorkspace(openFile.FileName);
            }
        }

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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (_workspaceVM._changedFlag && MessageBox.Show("Save your changes before exit?", "Save changes?", MessageBoxButton.OKCancel, MessageBoxImage.Question) ==  MessageBoxResult.OK)
            {
                ExportExcel_Click(null, null);

                Application.Current.Shutdown();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
}
