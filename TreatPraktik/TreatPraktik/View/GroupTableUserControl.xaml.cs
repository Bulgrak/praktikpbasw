﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;
using Point = System.Windows.Point;
using System.ComponentModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for GroupTableUserControl.xaml
    /// </summary>
    public partial class GroupTableUserControl : UserControl, INotifyPropertyChanged
    {
        public GroupTableContainerUserControl ParentGroupTableContainerUserControl { get; set; }
        public GroupTableViewModel GTViewModel { get; set; }
        public TextBlock GroupHeader { get; set; }
        public static GroupTypeOrder PreviousGroupTypeOrder { get; set; }
        private readonly List<string> _specialGroups = new List<string>{"1", "11", "14", "59"}; //Groups that shouldn't show their items

        //GroupTable - Color theme
        private readonly Color _cellColor = Color.FromRgb(228, 241, 255);
        private readonly Color _cellBorderColor = Color.FromRgb(151, 203, 255);
        private readonly Color _cellBorderHighlightColor = Colors.Red;
        private readonly Color _cellTextColor = Colors.Black;
        private readonly Color _groupHeaderColor = Color.FromRgb(151, 203, 255);
        //GroupTable - Color theme

        public GroupTableUserControl()
        {
            InitializeComponent();
            GTViewModel = new GroupTableViewModel();
            ParentGroupTableContainerUserControl = (GroupTableContainerUserControl)Parent;
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

        #region DependencyProperty GroupTypeOrderCollection
        private static void OnGroupTableCollectionChangedCallBack(
         DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GroupTableUserControl groupTableUserControl = sender as GroupTableUserControl;
            ObservableCollection<GroupTypeOrder> groupTypeOrder = (ObservableCollection<GroupTypeOrder>)e.NewValue;
            groupTableUserControl.GroupTypeOrderCollection = groupTypeOrder;
        }

        public ObservableCollection<GroupTypeOrder> GroupTypeOrderCollection
        {
            get { return (ObservableCollection<GroupTypeOrder>)GetValue(GroupTypeOrderCollectionProperty); }
            set
            {
                SetValue(GroupTypeOrderCollectionProperty, value);
                GTViewModel.GroupTypeOrderCollection = GroupTypeOrderCollection;
                if (GroupTypeOrderCollection.IndexOf(PreviousGroupTypeOrder) == GroupTypeOrderCollection.Count - 1)
                {
                    PreviousGroupTypeOrder = null;
                }
                OnPropertyChanged("GroupTypeOrderCollection");
            }
        }

        public static readonly DependencyProperty GroupTypeOrderCollectionProperty =
            DependencyProperty.Register("GroupTypeOrderCollection", typeof(ObservableCollection<GroupTypeOrder>), typeof(GroupTableUserControl), new PropertyMetadata(OnGroupTableCollectionChangedCallBack));
        #endregion

        #region DependencyProperty GroupTypeOrder
        private static void OnGroupTableChangedCallBack(
                DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GroupTableUserControl groupTableUserControl = sender as GroupTableUserControl;
            GroupTypeOrder groupTypeOrder = (GroupTypeOrder)e.NewValue;
            groupTableUserControl.MyGroupTypeOrder = groupTypeOrder;
        }

        public GroupTypeOrder MyGroupTypeOrder
        {
            get { return (GroupTypeOrder)GetValue(MyGroupTypeProperty); }
            set
            {
                SetValue(MyGroupTypeProperty, value);
                OnPropertyChanged("MyGroupTypeOrder");
                GTViewModel.Group = MyGroupTypeOrder.Group;
                PopulateGroupTable(MyGroupTypeOrder);
            }
        }

        public static readonly DependencyProperty MyGroupTypeProperty =
            DependencyProperty.Register("MyGroupTypeOrder", typeof(GroupTypeOrder), typeof(GroupTableUserControl), new PropertyMetadata(OnGroupTableChangedCallBack));
        #endregion

        public void PopulateGroupTable(GroupTypeOrder gto)
        {
            if (PreviousGroupTypeOrder == null || PreviousGroupTypeOrder.GroupTypeID != gto.GroupTypeID)
            {
                PreviousGroupTypeOrder = gto;
                GroupType gt = gto.Group;
                CreateColumns(GroupTable, 4);
                int counterRow = 0;
                int counterColumn = 0;
                AddNewGroupRow();
                InsertGroupItem(gto, 0, 0);
                if(!_specialGroups.Any(x => x.Equals(gt.GroupTypeID)))
                {
                    int skipped = 0;
                    for (int j = 0; j < gt.ItemOrder.Count - skipped; j++)
                    {
                        if (gt.ItemOrder[j + skipped].DesignID.Equals("198"))
                        {
                            if (j%4 == 0)
                            {
                                AddNewEmptyItemRow();
                                counterRow++;
                                gt.ItemOrder[j + skipped].Item.Header = "<NewLineItem>";
                                //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                                InsertItem(gt.ItemOrder[j + skipped], counterRow, j%4);
                                j--;
                                skipped++;

                                continue;
                            }
                            gt.ItemOrder[j + skipped].Item.Header = "<NewLineItem>";
                            //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                            InsertItem(gt.ItemOrder[j + skipped], counterRow, j%4);
                            skipped = skipped + j;
                            j = 0;
                        }
                        else if (j%4 == 0)
                        {
                            AddNewEmptyItemRow();
                            counterRow++;
                        }
                        if (gt.ItemOrder[j + skipped].DesignID.Equals("198"))
                        {
                            j--;
                            skipped++;
                            continue;
                        }
                        if (gt.ItemOrder[j + skipped].DesignID.Equals("197"))
                        {
                            gt.ItemOrder[j + skipped].Item.Header = "<EmptyField>";
                            InsertItem(gt.ItemOrder[j + skipped], counterRow, j%4);
                            //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                        }
                        else
                        {
                            if (counterColumn >= 4)
                            {
                                counterColumn = 0;
                            }
                            InsertItem(gt.ItemOrder[j + skipped], counterRow, j%4);
                            //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                        }
                        counterColumn++;
                    }
                    AddNewEmptyItemRow();
                }
            }
        }

        private void CreateColumns(Grid grid, int columns)
        {
            for (int j = 0; j < columns; j++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                grid.ColumnDefinitions.Add(cd);
            }
        }

        public GroupTypeOrder GetGroupType(Grid grid)
        {
            Border b = (Border)grid.GetCellChild(0, 0);
            GroupTypeOrder gto = (GroupTypeOrder)b.DataContext;
            return gto;
        }

        private void InsertItem(ItemTypeOrder item, int row, int column)
        {
            Border bCell = GetCellItem(row, column);
            Grid cellItem = (Grid)bCell.Child;
            TextBlock tb = (TextBlock)cellItem.Children[1];
            ItemTypeOrder dummyItemType = (ItemTypeOrder)bCell.DataContext;
            item.ItemOrder = dummyItemType.ItemOrder;
            bCell.DataContext = item;
            //tb.DataContext = itemType;
            //tb.SetBinding(TextBlock.TextProperty, "Header");
            tb.Foreground = new SolidColorBrush(_cellTextColor);
        }

        private void InsertGroupItem(GroupTypeOrder groupTypeOrder, int row, int column)
        {
            Border bCell = GetCellItem(row, column);
            bCell.DataContext = groupTypeOrder;
            Grid gridGroupCell = (Grid)bCell.Child;
            TextBlock tb = (TextBlock)gridGroupCell.Children[1];

            WorkspaceViewModel wvm = WorkspaceViewModel.Instance;
            List<GroupTypeOrder> gtoList = wvm.PageList.First(x => x.PageTypeID.Equals(wvm.SelectedPage)).GroupTypeOrders.Where(x => x.GroupTypeID.Equals(MyGroupTypeOrder.GroupTypeID)).ToList();
            List<string> departmentList = new List<string>();
            foreach (GroupTypeOrder gto in gtoList)
                departmentList.Add(gto.DepartmentID);
            departmentList.Sort();
            MultiBinding multiBinding = new MultiBinding();
            multiBinding.StringFormat = "{0} ({1})";
            multiBinding.Bindings.Add(new Binding("Group.GroupHeader"));
            multiBinding.Bindings.Add(new Binding() { Source = string.Join(",", departmentList.ToArray()) });
            tb.SetBinding(TextBlock.TextProperty, multiBinding);
        }

        public Border GetCellItem(int row, int column)
        {
            Border cellItem = (Border)GroupTable.Children
      .Cast<UIElement>()
      .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            return cellItem;
        }

        private void AddNewEmptyItemRow()
        {
            RowDefinition rd = new RowDefinition();
            GroupTable.RowDefinitions.Add(rd);
            int row = GroupTable.RowDefinitions.Count - 1;
            int i = 0;
            while (i < GroupTable.ColumnDefinitions.Count)
            {
                ItemTypeOrder item = new ItemTypeOrder();
                item.ItemOrder = ((row - 1) * 4) + i;
                Border border = CreateItemCell(item);
                Grid.SetRow(border, row);
                Grid.SetColumn(border, i);
                GroupTable.Children.Add(border);
                i++;
            }
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Visibility = Visibility.Hidden;

            Grid gridCell = (Grid)btn.Parent;
            Border border = (Border)gridCell.Parent;
            Grid gridGroupTable = (Grid)border.Parent;
            GroupTypeOrder gto = (GroupTypeOrder)gridGroupTable.DataContext;
            GroupType gt = gto.Group;

            ItemTypeOrder itToBeDeleted = (ItemTypeOrder)border.DataContext;
            //gt.ItemOrder.Remove(itToBeDeleted);
            GTViewModel.RemoveItemTypeOrder(gt, itToBeDeleted);
            GTViewModel.AdjustItemOrder(gt);
            RefreshGroupTable();
            DisableAllowDropByNewLineItem();
            PreviousGroupTypeOrder = null;
        }

        private void btnRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Button btn = sender as Button;
                btn.Visibility = Visibility.Hidden;
                StackPanel sp = (StackPanel)btn.Parent;
                Grid gridCell = (Grid)sp.Parent;
                Border border = (Border)gridCell.Parent;
                GroupTypeOrder gto = (GroupTypeOrder)border.DataContext;

                GTViewModel.RemoveGroup(gto);
                RefreshGroupTable();
                DisableAllowDropByNewLineItem();
            }
        }

        private List<ItemTypeOrder> GetItemsByRow(int row)
        {
            List<UIElement> uieList = GroupTable.GetGridCellChildrenByRow(row);
            List<ItemTypeOrder> itemTypeListCheck = new List<ItemTypeOrder>();
            foreach (UIElement uie in uieList)
            {
                if (uie is Border)
                {
                    Border bCell = (Border)uie;
                    ItemTypeOrder it = (ItemTypeOrder)bCell.DataContext;
                    itemTypeListCheck.Add(it);
                }
            }
            return itemTypeListCheck;
        }

        private bool CheckForNewLineItem(int row)
        {
            List<ItemTypeOrder> itemTypeListCheck = GetItemsByRow(row);
            bool newLineItemFound = false;
            int i = 0;
            while (i < itemTypeListCheck.Count)
            {
                string designID = itemTypeListCheck[i].DesignID;
                if (designID != null && designID.Equals("198"))
                {
                    newLineItemFound = true;
                }
                i++;
            }
            return newLineItemFound;
        }

        private void AddNewGroupRow()
        {
            RowDefinition rd = new RowDefinition();
            GroupTable.RowDefinitions.Add(rd);
            int rowNo = GroupTable.RowDefinitions.Count - 1;

            int i = 0;
            while (i < 4)
            {
                Border cellItem = CreateGroupCell();
                if (i == 0)
                {
                    Grid.SetRow(cellItem, rowNo);
                    Grid.SetColumn(cellItem, i);
                    Grid.SetColumnSpan(cellItem, 4);
                    GroupTable.Children.Add(cellItem);
                }
                i++;
            }
        }

        private Border CreateGroupCell()
        {
            Border bGroupCell = CreateBorderContainer(_cellBorderColor, _groupHeaderColor);
            bGroupCell.MouseEnter += border_MouseEnter;
            bGroupCell.MouseLeave += border_MouseLeave;
            bGroupCell.AllowDrop = true;
            bGroupCell.Drop += border_Drop;
            bGroupCell.DragEnter += border_DragEnter;
            bGroupCell.DragLeave += border_DragLeave;
            bGroupCell.DragOver += border_DragOver;
            TextBlock tb = new TextBlock { FontWeight = FontWeights.Bold, FontSize = 14.0 };
            Grid gridCell = new Grid();
            CreateColumns(gridCell, 2);
            Button clearGroupBtn = CreateClearGroupBtn();
            Button editGroupBtn = CreateEditGroupBtn();
            StackPanel spBtn = new StackPanel();
            spBtn.Orientation = Orientation.Horizontal;
            spBtn.HorizontalAlignment = HorizontalAlignment.Right;
            spBtn.VerticalAlignment = VerticalAlignment.Top;
            spBtn.Children.Add(editGroupBtn);
            spBtn.Children.Add(clearGroupBtn);
            Grid.SetColumn(spBtn, 1);
            Grid.SetColumn(tb, 0);
            gridCell.Children.Add(spBtn);
            gridCell.Children.Add(tb);
            bGroupCell.Child = gridCell;
            return bGroupCell;
        }

        private Border CreateItemCell(ItemTypeOrder itemType)
        {
            Border bCell = CreateBorderContainer(_cellBorderColor, _cellColor);
            bCell.DataContext = itemType;
            bCell.MouseEnter += border_MouseEnter;
            bCell.MouseLeave += border_MouseLeave;
            bCell.AllowDrop = true;
            bCell.Drop += border_Drop;
            bCell.DragEnter += border_DragEnter;
            bCell.DragLeave += border_DragLeave;
            bCell.DragOver += border_DragOver;
            TextBlock tb = new TextBlock
            {
                FontSize = 14.0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            tb.SetBinding(TextBlock.TextProperty, "Item.Header");
            //bCell.SetBinding(Border.ToolTipProperty, "Item.ToolTip");
            tb.SetBinding(TextBlock.ToolTipProperty, "Item.ToolTip");
            //ToolTipService.SetInitialShowDelay(bCell, 700);
            Grid gridCell = new Grid();
            CreateColumns(gridCell, 2);
            Button clearCellBtn = CreateClearCellBtn();
            Grid.SetColumn(clearCellBtn, 1);
            Grid.SetColumn(tb, 0);
            gridCell.Children.Add(clearCellBtn);
            gridCell.Children.Add(tb);
            bCell.Child = gridCell;
            return bCell;
        }

        void border_DragLeave(object sender, DragEventArgs e)
        {
            CheckDroppedItem(sender, e); //prevents cursor from flickering when dropping an item
            e.Handled = true;
        }

        void border_DragEnter(object sender, DragEventArgs e)
        {
            CheckDroppedItem(sender, e); //prevents cursor from flickering when dropping an item
            e.Handled = true;
        }

        void border_DragOver(object sender, DragEventArgs e)
        {
            CheckDroppedItem(sender, e); //prevents cursor from flickering when dropping an item
            e.Handled = true;
        }

        void CheckDroppedItem(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem) // Drag and drop of toolboxitems
            {
                CheckToolboxItemDrop(sender, e);
            }
            if (e.Data.GetData("System.Windows.Controls.Border") is Border) // Drag and drop of items
            {
                CheckItemTypeDrop(sender, e);
            }

            if (e.Data.GetData("System.Windows.Controls.Grid") is Grid) // Drag and drop of groups
            {
                CheckGroupTableDrop(sender, e);
            }
            PreviousGroupTypeOrder = null;
        }

        void CheckToolboxItemDrop(object sender, DragEventArgs e)
        {
            Border bCell = GetBorderByDropEvent(e);
            ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
            if (lbi.Content is ToolboxItem)
            {
                ToolboxItem tbi = (ToolboxItem) lbi.Content;

                if (!(bCell.DataContext is GroupTypeOrder))
                {
                    int row = Grid.GetRow(bCell);
                    bool containsRow = CheckForNewLineItem(row);
                    if (tbi.ItemType.DesignID.Equals("198") && containsRow)
                    {
                        e.Effects = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else if (lbi.Content is ToolboxGroup)
            {
                ToolboxGroup tbg = (ToolboxGroup)lbi.Content;

                if (bCell.DataContext is GroupTypeOrder)
                {
                    
                 
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
        }

        void CheckItemTypeDrop(object sender, DragEventArgs e)
        {
            Border borderCell = GetBorderByDropEvent(e);
            int row = Grid.GetRow(borderCell);
            Border draggedBorderCell = (Border)e.Data.GetData("System.Windows.Controls.Border");
            Grid draggedGridCell = (Grid)draggedBorderCell.Child;
            TextBlock draggedTextBlock = (TextBlock)draggedGridCell.Children[1];
            int draggedItemRow = Grid.GetRow(draggedBorderCell);
            Grid draggedGroupTable = (Grid)draggedBorderCell.Parent;

            GroupTypeOrder draggedGto = GetGroupType(draggedGroupTable);
            GroupTypeOrder gto = GetGroupType(GroupTable);
            GroupType draggedGt = draggedGto.Group;
            GroupType gt = gto.Group;

            ItemTypeOrder ito = (ItemTypeOrder)draggedTextBlock.DataContext;
            ItemType it = ito.Item;
            if (borderCell.DataContext is ItemTypeOrder)
            {
                //if (!draggedGt.GroupTypeID.Equals(gt.GroupTypeID))
                //{
                //    e.Effects = DragDropEffects.None;
                //}
                //else
                //{
                    bool containsRow = CheckForNewLineItem(row);
                    if (it.DesignID.Equals("198") && containsRow && draggedItemRow != row)
                    {
                        e.Effects = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                //}
            }
            if (borderCell.DataContext is GroupTypeOrder)
            {

                e.Effects = DragDropEffects.None;
            }
        }

        void CheckGroupTableDrop(object sender, DragEventArgs e)
        {
            Border bCell = GetBorderByDropEvent(e);

            if (bCell.DataContext is GroupTypeOrder)
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Gets dragged border which contain either an item or a whole group
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Border GetBorderByDropEvent(DragEventArgs e)
        {
            Border bCell = null;
            if (e.Source is Border)
            {
                bCell = e.Source as Border;

            }
            else if (e.Source is TextBlock)
            {
                TextBlock tb = e.Source as TextBlock;
                Grid gridCell = (Grid)tb.Parent;
                bCell = (Border)gridCell.Parent;
            }
            else if (e.Source is Image)
            {
                Image img = e.Source as Image;
                Button btn = (Button)img.Parent;
                if (btn.Parent is Grid)
                {
                    Grid gridCell = (Grid)btn.Parent;
                    bCell = (Border)gridCell.Parent;
                }
                else if (btn.Parent is StackPanel)
                {
                    StackPanel spGroupBtns = (StackPanel)btn.Parent;
                    Grid gridCell = (Grid)spGroupBtns.Parent;
                    bCell = (Border)gridCell.Parent;
                }
            }
            else if (e.Source is Button)
            {
                Button btn = e.Source as Button;
                if (btn.Parent is Grid)
                {
                    Grid gridCell = (Grid)btn.Parent;
                    bCell = (Border)gridCell.Parent;
                }
                else if (btn.Parent is StackPanel)
                {
                    StackPanel spGroupBtns = (StackPanel)btn.Parent;
                    Grid gridCell = (Grid)spGroupBtns.Parent;
                    bCell = (Border)gridCell.Parent;
                }
            }
            return bCell;
        }

        void HandleToolboxItemDrop(object sender, DragEventArgs e)
        {
            Border bCell = GetBorderByDropEvent(e);

            ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
            ToolboxItem tbi = (ToolboxItem)lbi.Content;
            ItemTypeOrder itToBeMoved = (ItemTypeOrder)bCell.DataContext;
            GroupType gt = MyGroupTypeOrder.Group;
            GTViewModel.HandleToolboxItemDrop(gt, tbi, itToBeMoved);
            RefreshGroupTable();
            DisableAllowDropByNewLineItem();
        }

        void HandleItemTypeDrop(object sender, DragEventArgs e)
        {
            Border target = GetBorderByDropEvent(e);
            ItemTypeOrder targetItemType = (ItemTypeOrder)target.DataContext;


            Border target2 = e.Data.GetData("System.Windows.Controls.Border") as Border;
            ItemTypeOrder draggedItemType = (ItemTypeOrder)target2.DataContext;
            Grid groupTable = (Grid)target2.Parent;
            GroupTypeOrder gto = GetGroupType(groupTable);
            GroupType gt = gto.Group;
            GTViewModel.HandleDropAndDropBetweenItems(gt, targetItemType, draggedItemType);
            gt.ItemOrder.Sort(i => i.ItemOrder);
            RefreshGroupTable();
        }

        void RefreshGroupTable()
        {
            PreviousGroupTypeOrder = null;
            GroupTable.ClearGrid();
            PopulateGroupTable(MyGroupTypeOrder);
            DisableAllowDropByNewLineItem();
        }

        void HandleGroupTableDrop(object sender, DragEventArgs e)
        {
            Grid targetGroupTable = e.Data.GetData("System.Windows.Controls.Grid") as Grid;
            GroupTableUserControl gtUC = (GroupTableUserControl)targetGroupTable.Parent;
            GroupTypeOrder targetGroupTypeOrder = MyGroupTypeOrder;

            GroupTypeOrder draggedGroupTypeOrder = gtUC.MyGroupTypeOrder;
            if (!targetGroupTypeOrder.GroupTypeID.Equals(draggedGroupTypeOrder.GroupTypeID))
            {
                GTViewModel.HandleGroupTableDrop(targetGroupTypeOrder, draggedGroupTypeOrder);
            }
        }

        void HandleToolboxGroupDrop(ToolboxGroup tbg)
        {
            GroupTypeOrder targetGroupTypeOrder = MyGroupTypeOrder;
            GroupType draggedGroupType = tbg.Group;
            GTViewModel.InsertGroup(targetGroupTypeOrder, draggedGroupType);
        }

        void border_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem) //Drag and drop toolboxitem
            {
                ListBoxItem lbi = (ListBoxItem) e.Data.GetData("System.Windows.Controls.ListBoxItem");
                if (lbi.DataContext is ToolboxItem)
                {
                    HandleToolboxItemDrop(sender, e);
                }
                else if (lbi.DataContext is ToolboxGroup)
                {
                    ToolboxGroup tbg = (ToolboxGroup)lbi.DataContext;
                    if (GroupTypeOrderCollection.Any(x => x.Group.GroupTypeID.Equals(tbg.Group.GroupTypeID)))
                    {
                        MessageBox.Show("The group already exists", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else
                    {
                        HandleToolboxGroupDrop(tbg);
                    }
                }
            }
            if (e.Data.GetData("System.Windows.Controls.Border") is Border) //Drag and drop between items
            {
                HandleItemTypeDrop(sender, e);
            }
            if (e.Data.GetData("System.Windows.Controls.Grid") is Grid) //Drag and drop groups 
            {
                HandleGroupTableDrop(sender, e);
            }
            PreviousGroupTypeOrder = null;
        }

        void EnableAllowDropNewLine(int startColumnPosition, int row)
        {
            int i = startColumnPosition;
            while (i < GroupTable.ColumnDefinitions.Count)
            {
                Border borderCell = (Border)GroupTable.GetCellChild(row, i);
                borderCell.AllowDrop = true;
                i++;
            }
        }

        void DisableAllowDropByNewLineItem()
        {
            int row = 1;
            while (row < GroupTable.RowDefinitions.Count)
            {
                List<ItemTypeOrder> itemTypeList = GetItemsByRow(row);
                bool found = false;
                int i = 0;
                while (i < itemTypeList.Count && !found)
                {
                    if (itemTypeList[i].DesignID != null && itemTypeList[i].DesignID.Equals("198"))
                    {
                        found = true;
                        i++; // To make sure that AllowDrop is disabled for the rest of the items
                    }
                    else
                    {
                        i++;
                    }
                }

                while (i < GroupTable.ColumnDefinitions.Count && found)
                {
                    Border borderCell = (Border)GroupTable.GetCellChild(row, i);
                    borderCell.AllowDrop = false;
                    i++;
                }
                row++;
            }
        }

        void border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bCell = sender as Border;
            Grid cellItem = (Grid)bCell.Child;
            if (bCell.DataContext is ItemTypeOrder)
            {
                ItemTypeOrder ito = (ItemTypeOrder)bCell.DataContext;
                ItemType it = ito.Item;
                if (it != null)
                {
                    Button btnClearCell = (Button)cellItem.Children[0];
                    btnClearCell.Visibility = Visibility.Visible;
                }
            }
            if (bCell.DataContext is GroupTypeOrder)
            {
                StackPanel spBtn = (StackPanel)cellItem.Children[0];
                Button btnEditGroup = (Button)spBtn.Children[0];
                Button btnRemoveGroup = (Button)spBtn.Children[1];
                btnEditGroup.Visibility = Visibility.Visible;
                btnRemoveGroup.Visibility = Visibility.Visible;
            }
        }

        void border_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bCell = sender as Border;
            if (bCell.DataContext is ItemTypeOrder)
            {
                Grid cellItem = (Grid)bCell.Child;
                Button btnClearCell = (Button)cellItem.Children[0];
                btnClearCell.Visibility = Visibility.Hidden;
            }
            else if (bCell.DataContext is GroupTypeOrder)
            {
                Grid cellItem = (Grid)bCell.Child;
                StackPanel spBtn = (StackPanel)cellItem.Children[0];
                Button btnEditGroup = (Button)spBtn.Children[0];
                Button btnRemoveGroup = (Button)spBtn.Children[1];
                btnEditGroup.Visibility = Visibility.Hidden;
                btnRemoveGroup.Visibility = Visibility.Hidden;
            }
        }

        public Border CreateBorderContainer(Color borderBrush, Color background)
        {
            Border bCell = new Border
            {
                BorderBrush = new SolidColorBrush(borderBrush),
                Background = new SolidColorBrush(background),
                BorderThickness = new Thickness(1),
                Height = 27.0,
                MaxHeight = 27.0,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            bCell.MouseMove += border_MouseMove;
            bCell.GiveFeedback += border_GiveFeedback;
            return bCell;
        }

        void border_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Border bCell = sender as Border;

            if (adorner != null)
            {
                var pos = bCell.PointFromScreen(GetMousePosition());
                adorner.UpdatePosition(pos);
                //e.Handled = true;
            }

        }

        void border_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Border && e.LeftButton == MouseButtonState.Pressed)
            {
                Border draggedItem = sender as Border;
                if (draggedItem.DataContext is ItemTypeOrder) //Prevent dragging if GroupType
                {
                    
                    ItemTypeOrder ito = (ItemTypeOrder)draggedItem.DataContext;
                    ItemType it = ito.Item;
                    if (it != null)
                    {
                        draggedItem.BorderBrush = new SolidColorBrush(_cellBorderHighlightColor);
                        adorner = new DragAdornerItem(draggedItem, e.GetPosition(draggedItem));
                        AdornerLayer.GetAdornerLayer(this).Add(adorner);
                        
                        if (it.DesignID.Equals("198"))
                        {
                            int row = Grid.GetRow(draggedItem);
                            EnableAllowDropNewLine(0, row);
                        }
                        DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.None | DragDropEffects.Move);
                        if (AdornerLayer.GetAdornerLayer(this) != null)
                        {
                            AdornerLayer.GetAdornerLayer(this).Remove(adorner);
                        }
                        draggedItem.BorderBrush = new SolidColorBrush(Color.FromRgb(151, 203, 255));
                    }
                }
                if (draggedItem.DataContext is GroupTypeOrder)
                {
                    Grid groupTable = (Grid)draggedItem.Parent;

                    adorner = new DragAdornerItem(groupTable, e.GetPosition(draggedItem));
                    AdornerLayer.GetAdornerLayer(this).Add(adorner);
                    DragDrop.DoDragDrop(draggedItem, groupTable, DragDropEffects.None | DragDropEffects.Move);
                    if (AdornerLayer.GetAdornerLayer(this) != null)
                    {
                        AdornerLayer.GetAdornerLayer(this).Remove(adorner);
                    }
                }
            }
        }

        public Button CreateClearCellBtn()
        {
            var uriSource = new Uri(@"/TreatPraktik;component/Ressources/Delete-icon.png", UriKind.Relative);
            Image imgRemoveIcon = new Image();
            imgRemoveIcon.Source = new BitmapImage(uriSource);

            Button btnClearCell = new Button
            {
                Width = 16.0,
                Height = 16.0,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Content = imgRemoveIcon,
                Visibility = Visibility.Hidden
            };
            btnClearCell.Click += btnRemoveItem_Click;

            return btnClearCell;
        }

        public Button CreateClearGroupBtn()
        {
            var uriSource = new Uri(@"/TreatPraktik;component/Ressources/Delete-icon.png", UriKind.Relative);
            Image imgRemoveIcon = new Image();
            imgRemoveIcon.Source = new BitmapImage(uriSource);

            Button btnClearCell = new Button
            {
                Width = 16.0,
                Height = 16.0,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Content = imgRemoveIcon,
                Visibility = Visibility.Hidden
            };
            btnClearCell.Click += btnRemoveGroup_Click;

            return btnClearCell;
        }

        public Button CreateEditGroupBtn()
        {
            var uriSource = new Uri(@"/TreatPraktik;component/Ressources/Edit-icon.png", UriKind.Relative); //Icon from http://www.turbomilk.com/
            Image imgRemoveIcon = new Image();
            imgRemoveIcon.Source = new BitmapImage(uriSource);

            Button btnEditGroup = new Button
            {
                Width = 16.0,
                Height = 16.0,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Content = imgRemoveIcon,
                Visibility = Visibility.Hidden
            };
            btnEditGroup.Click += btnEditGroup_Click;

            return btnEditGroup;
        }

        public void btnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            EditGroupDialogBox dlg = new EditGroupDialogBox();
            dlg.Title = "Edit Group - " + MyGroupTypeOrder.Group.GroupHeader;
            dlg.englishTextBox.Text = MyGroupTypeOrder.Group.EnglishTranslationText;
            dlg.danishTextBox.Text = MyGroupTypeOrder.Group.DanishTranslationText;

            ObservableCollection<string> obscDepartmentList = new ObservableCollection<string>();
            int i = 0;
            while (i < GroupTypeOrderCollection.Count)
            {
                if(GroupTypeOrderCollection[i].GroupTypeID == MyGroupTypeOrder.GroupTypeID)
                    obscDepartmentList.Add(GroupTypeOrderCollection[i].DepartmentID);
                i++;
            }
            dlg.departmentUserControl.departmentsListBox.ItemsSource = obscDepartmentList;

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                List<string> departmentList = new List<string>();
                foreach (string s in dlg.departmentUserControl.departmentsListBox.Items)
                    departmentList.Add(s);
                GTViewModel.EditGroup(MyGroupTypeOrder, dlg.englishTextBox.Text, dlg.danishTextBox.Text, departmentList);
            }
        }

        #region MousePositionLogic
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
        #endregion

        private DragAdornerItem adorner;
    }
}