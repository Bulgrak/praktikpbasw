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
    /// Interaction logic for SpecialFields.xaml
    /// </summary>
    public partial class SpecialFields : UserControl
    {
        public SpecialFields()
        {
            InitializeComponent();
            DataContext = new SpecialFieldsViewModel();
            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseMoveEvent, new MouseEventHandler(List_MouseMove)));
            //itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listbox1_Drop)));
            lstSpecialItems.ItemContainerStyle = itemContainerStyle;
        }

        void List_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {

                ListBoxItem draggedItem = sender as ListBoxItem;
                draggedItem.IsSelected = true;
                DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Copy);
            }
        }

        public void ToolboxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {

                ListBoxItem draggedItem = sender as ListBoxItem;
                draggedItem.IsSelected = true;
                DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Copy);
            }
        }
    }
}
