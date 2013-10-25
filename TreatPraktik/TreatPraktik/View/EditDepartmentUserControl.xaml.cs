using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for EditDepartmentUserControl.xaml
    /// </summary>
    public partial class EditDepartmentUserControl : UserControl
    {
        public EditDepartmentUserControl()
        {
            InitializeComponent();
        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            string departmentNo = departmentNoTextBox.Text;
            if (!departmentNoTextBox.Text.Trim().Equals(""))
            {
                ObservableCollection<string> departmentList = (ObservableCollection<string>)departmentsListBox.ItemsSource;
                //ObservableCollection<string> departmentList2 = new ObservableCollection<string> { "12", "12" };
                if (!departmentList.Contains(departmentNo))
                {
                    departmentList.Add(departmentNo);
                }
                if (!departmentNo.Equals("-1"))
                {
                    departmentList.Remove("-1");
                }
                if (departmentNo.Equals("-1"))
                {
                    departmentList.Clear();
                    departmentList.Add("-1");
                }
                departmentNoTextBox.Clear();
            }
        }

        private void btnRemoveDepartment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button.DataContext as string;
            ObservableCollection<string> departmentList = (ObservableCollection<string>)departmentsListBox.ItemsSource;
            departmentList.Remove(task);
            if (departmentList.Count == 0)
            {
                departmentList.Add("-1");
            }
        }

        private void departmentNoTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //CheckIsNumeric(e);
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void CheckIsNumeric(TextCompositionEventArgs e)
        {
            //int result;

            //if (!(int.TryParse(e.Text, out result)))
            //{

            //        e.Handled = true;
            //}
            //if (e.Text == "-" && !departmentNoTextBox.Text.Contains("-"))
            //    e.Handled = true;
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.]+"); //regex that matches disallowed text
            if (text.Equals("-") && departmentNoTextBox.Text.Equals(""))
            {
                return true;
            }
            else
            {

            }
            return !regex.IsMatch(text);
        }
    }
}
