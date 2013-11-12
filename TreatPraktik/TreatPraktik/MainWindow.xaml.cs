using System;
using System.Windows;
using TreatPraktik.ViewModel;

namespace TreatPraktik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.DefaultExt = ".xlsx";
            openFile.Filter = "Excel file (.xlsx)|*.xlsx";
            bool stop = false;
            while (!stop)
            {
                if (openFile.ShowDialog() == true)
                {
                    try
                    {
                        WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
                        wvm.LoadWorkspace(openFile.FileName);
                        ToolboxStandardItemsViewModel.Instance.PopulateToolbox();
                        ToolboxGroupsViewModel.Instance.PopulateGTList();

                        InitializeComponent();
                        WsUserControl.SetupInitialScrollPositions();
                        stop = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message,
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    stop = true;
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
