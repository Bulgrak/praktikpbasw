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
    /// Interaction logic for ImportExportExcel.xaml
    /// </summary>
    public partial class ImportExportExcel : UserControl
    {
        private ExportExcel exExcel;

        public ImportExportExcel()
        {
            InitializeComponent();
            exExcel = ExportExcel.Instance;
        }

        /// <summary>
        /// Not Implemented yet
        /// </summary>
        private void ImportExcel_Click(object sender, RoutedEventArgs e)
        {
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
                exExcel.CreateNewExcel(saveFile.FileName);
            }
        }
    }
}
