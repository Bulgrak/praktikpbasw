using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for ToolboxGroupsUserControl.xaml
    /// </summary>
    public partial class ToolboxGroupsUserControl : UserControl
    {
        public UIElement TopGrid { get; set; }
        public ToolboxGroupsUserControl()
        {
            InitializeComponent();
            ToolboxGroupsViewModel glvm = ToolboxGroupsViewModel.Instance;
            DataContext = glvm;
            glvm.PopulateGTList();
            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            //itemContainerStyle.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            //itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(List_PreviewMouseLeftButtonDown)));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseMoveEvent, new MouseEventHandler(List_MouseMove)));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewGiveFeedbackEvent, new GiveFeedbackEventHandler(List_GiveFeedback)));
            //itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listbox1_Drop)));
            lstItems.ItemContainerStyle = itemContainerStyle;
            TopGrid = (UIElement)this.Parent;
        }


        void List_MouseMove(object sender, MouseEventArgs e)
        {
            Point current = e.GetPosition(this);
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {

                ListBoxItem draggedItem = sender as ListBoxItem;
                draggedItem.IsSelected = true;
                adorner = new DragAdornerToolboxItem(draggedItem, GetMousePosition());
                AdornerLayer.GetAdornerLayer(this).Add(adorner);
                DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Copy);
                AdornerLayer.GetAdornerLayer(this).Remove(adorner);
            }
            startPoint = current;
        }

        void List_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (adorner != null)
            {
                ListBoxItem lbl = sender as ListBoxItem;
                var pos = lbl.PointFromScreen(GetMousePosition());
                adorner.UpdatePosition(pos);
              //e.Handled = true;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        private Point startPoint;
        private DragAdornerToolboxItem adorner;
    }
}
