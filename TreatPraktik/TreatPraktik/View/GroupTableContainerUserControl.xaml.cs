using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for GroupContainerUserControl.xaml
    /// </summary>
    public partial class GroupTableContainerUserControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<GroupTypeOrder> _gtoObsCollection;
        //public ObservableCollection<GroupTypeOrder> GtoObsCollection { get; set; }
        public GroupTableContainerViewModel GTCViewModel { get; set; }

        public GroupTableContainerUserControl()
        {
            InitializeComponent();
            //this.DataContext = this;
            //GtoObsCollection = new ObservableCollection<GroupTypeOrder>();
            //GtoObsCollection.CollectionChanged += items_CollectionChanged;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public void AdjustGroupOrder(GroupTypeOrder draggedGroupTypeOrder, GroupTypeOrder targetGroupTypeOrder)
        {
            //GTCViewModel.AdjustGroupOrder(draggedGroupTypeOrder, targetGroupTypeOrder);
        }

        public ObservableCollection<GroupTypeOrder> GtoObsCollection
        {
            get { return _gtoObsCollection; }
            set
            {
                _gtoObsCollection = value;
                OnPropertyChanged("GtoObsCollection");
            }
        }

        #region DependencyProperty GtoCol
        private static void OnGtoColChangedCallBack(
         DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GroupTableContainerUserControl groupTableUserControl = sender as GroupTableContainerUserControl;
            ObservableCollection<GroupTypeOrder> groupTypeOrder = (ObservableCollection<GroupTypeOrder>)e.NewValue;
            groupTableUserControl.GtoCol = groupTypeOrder;
        }

        public ObservableCollection<GroupTypeOrder> GtoCol
        {
            get { return (ObservableCollection<GroupTypeOrder>)GetValue(GtoColProperty); }
            set
            {
                SetValue(GtoColProperty, value);
                OnPropertyChanged("GtoCol");
                //DataContext = this;
            }
        }

        public static readonly DependencyProperty GtoColProperty =
            DependencyProperty.Register("GtoCol", typeof(ObservableCollection<GroupTypeOrder>), typeof(GroupTableContainerUserControl), new PropertyMetadata(OnGtoColChangedCallBack));
        #endregion

        private void dropZoneToolboxGroup_Drop(object sender, DragEventArgs e)
        {
            ListBoxItem lbi = (ListBoxItem)e.Data.GetData("System.Windows.Controls.ListBoxItem");
            ToolboxGroup tbg = (ToolboxGroup)lbi.DataContext;
            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            string pageTypeID = wvm.SelectedPage;
            ObservableCollection<PageType> pageList = wvm.PageList;
            ObservableCollection<GroupTypeOrder> GroupTypeOrderCollection = pageList.First(x => x.PageTypeID.Equals(pageTypeID)).GroupTypeOrders;
            if (GroupTypeOrderCollection.Any(x => x.Group.GroupTypeID.Equals(tbg.Group.GroupTypeID)))
            {
                MessageBox.Show("The group already exists", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {

                GroupType gt = tbg.Group;
                GroupTableViewModel gtvm = new GroupTableViewModel();
                gtvm.GroupTypeOrderCollection = GroupTypeOrderCollection;
                gtvm.InsertGroupLast(gt, pageTypeID);
            }
        }

        private void dropZoneToolboxGroup_DragEnter(object sender, DragEventArgs e)
        {
            CheckDroppedElement(sender, e);
            e.Handled = true;
        }

        private void dropZoneToolboxGroup_DragOver(object sender, DragEventArgs e)
        {
            CheckDroppedElement(sender, e);
            e.Handled = true;
        }

        private void dropZoneToolboxGroup_DragLeave(object sender, DragEventArgs e)
        {
            CheckDroppedElement(sender, e);
            e.Handled = true;
        }

        private void CheckDroppedElement(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem)
            {
                ListBoxItem lbi = (ListBoxItem)e.Data.GetData("System.Windows.Controls.ListBoxItem");
                if (lbi.DataContext is ToolboxGroup)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void ScrollViewer_PreviewDragOver(object sender, DragEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            double tolerance = 30;
            double verticalPos = e.GetPosition(sv).Y;
            double offset = 5;

            if (verticalPos < tolerance) // Top of visible list?
            {
                sv.ScrollToVerticalOffset(sv.VerticalOffset - offset); //Scroll up.
            }
            else if (verticalPos > sv.ActualHeight - tolerance) //Bottom of visible list?
            {
                sv.ScrollToVerticalOffset(sv.VerticalOffset + offset); //Scroll down.    
            }
        }

        //private void ScrollViewer_PreviewDrop(object sender, DragEventArgs e)
        //{
        //    Double currentMousePosition = GetMousePosition().Y;
        //    ScrollViewer sv = (ScrollViewer)sender;
        //    sv.ScrollToVerticalOffset(currentMousePosition);
        //}

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
    }
}
