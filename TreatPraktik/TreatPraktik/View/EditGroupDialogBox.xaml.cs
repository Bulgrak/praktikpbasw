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
using System.Windows.Shapes;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for EditGroupDialogBox.xaml
    /// </summary>
    public partial class EditGroupDialogBox : Window
    {
        public EditGroupDialogBox()
        {
            InitializeComponent();
        }

        public Thickness DocumentMargin
        {
            get { return (Thickness)this.DataContext; }
            set { this.DataContext = value; }
        }

        void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Dialog box canceled
            this.DialogResult = false;
        }

        void okButton_Click(object sender, RoutedEventArgs e)
        {
            // Don't accept the dialog box if there is invalid data
            if (!IsValid(this)) return;

            // Dialog box accepted
            this.DialogResult = true;
        }

        // Validate all dependency objects in a window
        bool IsValid(DependencyObject node)
        {
            // Check if dependency object was passed
            if (node != null)
            {
                // Check if dependency object is valid.
                // NOTE: Validation.GetHasError works for controls that have validation rules attached 
                bool isValid = !Validation.GetHasError(node);
                if (!isValid)
                {
                    // If the dependency object is invalid, and it can receive the focus,
                    // set the focus
                    if (node is IInputElement) Keyboard.Focus((IInputElement)node);
                    return false;
                }
            }

            // If this dependency object is valid, check all child dependency objects
            foreach (object subnode in LogicalTreeHelper.GetChildren(node))
            {
                if (subnode is DependencyObject)
                {
                    // If a child dependency object is invalid, return false immediately,
                    // otherwise keep checking
                    if (IsValid((DependencyObject)subnode) == false) return false;
                }
            }

            // All dependency objects are valid
            return true;
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
