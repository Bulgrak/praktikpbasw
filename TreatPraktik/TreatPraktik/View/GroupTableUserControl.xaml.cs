using System;
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
            set { 
                SetValue(MyGroupTypeProperty, value);
                OnPropertyChanged("MyGroupTypeOrder");
                PopulateGroupTable(MyGroupTypeOrder.Group);
            }
        }

        public static readonly DependencyProperty MyGroupTypeProperty =
            DependencyProperty.Register("MyGroupTypeOrder", typeof(GroupTypeOrder), typeof(GroupTableUserControl), new PropertyMetadata(OnGroupTableChangedCallBack));
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
                OnPropertyChanged("GroupTypeOrderCollection");
            }
        }

        public static readonly DependencyProperty GroupTypeOrderCollectionProperty =
            DependencyProperty.Register("GroupTypeOrderCollection", typeof(ObservableCollection<GroupTypeOrder>), typeof(GroupTableUserControl), new PropertyMetadata(OnGroupTableCollectionChangedCallBack));
        #endregion


        public void PopulateGroupTable(GroupType gt)
        {
            CreateColumns(GroupTable, 4);
            int counterRow = 0;
            int counterColumn = 0;
            AddNewGroupRow();
            InsertGroupItem(gt, 0, 0);
            if (!gt.GroupTypeID.Equals("1") && !gt.GroupTypeID.Equals("11")) //Special groups, which shouldn't show any items
            {
                int skipped = 0;
                for (int j = 0; j < gt.ItemOrder.Count - skipped; j++)
                {
                    if (gt.ItemOrder[j + skipped].DesignID.Equals("198"))
                    {
                        if (j % 4 == 0)
                        {
                            AddNewEmptyItemRow();
                            counterRow++;
                            gt.ItemOrder[j + skipped].Item.Header = "<NewLineItem>";
                            //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                            SolidColorBrush textColor2 = Brushes.Black;
                            InsertItem(gt.ItemOrder[j + skipped], counterRow, j % 4, textColor2);
                            j--;
                            skipped++;

                            continue;
                        }
                        gt.ItemOrder[j + skipped].Item.Header = "<NewLineItem>";
                        //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                        SolidColorBrush textColor = Brushes.Black;
                        InsertItem(gt.ItemOrder[j + skipped], counterRow, j % 4, textColor);
                        skipped = skipped + j;
                        j = 0;
                    }
                    else if (j % 4 == 0)
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
                        SolidColorBrush textColor = Brushes.Black;
                        gt.ItemOrder[j + skipped].Item.Header = "<EmptyField>";
                        InsertItem(gt.ItemOrder[j + skipped], counterRow, j % 4, textColor);
                        //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                    }
                    else
                    {
                        if (counterColumn >= 4)
                        {
                            counterColumn = 0;
                        }
                        SolidColorBrush textColor = Brushes.Black;
                        InsertItem(gt.ItemOrder[j + skipped], counterRow, j % 4, textColor);
                        //gt.ItemOrder[j + skipped].ItemOrder = j + skipped;
                    }
                    counterColumn++;
                }
                AddNewEmptyItemRow();
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

        public GroupType GetGroupType(Grid grid)
        {
            Border b = (Border)grid.GetCellChild(0, 0);
            GroupType gt = (GroupType)b.DataContext;
            return gt;
        }

        //public GroupTypeOrder GetGroupTypeOrder(Grid grid)
        //{
        //    GroupTableUserControl gtUC = (GroupTableUserControl) grid.Parent;

        //}

        private void InsertItem(ItemTypeOrder item, int row, int column, SolidColorBrush textColor)
        {
            Border bCell = GetCellItem(row, column);
            Grid cellItem = (Grid)bCell.Child;
            TextBlock tb = (TextBlock)cellItem.Children[1];
            ItemTypeOrder dummyItemType = (ItemTypeOrder)bCell.DataContext;
            item.ItemOrder = dummyItemType.ItemOrder;
            bCell.DataContext = item;
            //tb.DataContext = itemType;
            //tb.SetBinding(TextBlock.TextProperty, "Header");
            tb.Foreground = textColor;
        }

        private void InsertGroupItem(GroupType groupType, int row, int column)
        {
            Border bCell = GetCellItem(row, column);
            bCell.DataContext = groupType;
            Grid gridGroupCell = (Grid)bCell.Child;
            TextBlock tb = (TextBlock)gridGroupCell.Children[1];
            tb.SetBinding(TextBlock.TextProperty, "GroupHeader");
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
                Border border = CreateItemCell(Colors.Black, Colors.Yellow, item);
                Grid.SetRow(border, row);
                Grid.SetColumn(border, i);
                GroupTable.Children.Add(border);
                i++;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Visibility = Visibility.Hidden;

            Grid gridCell = (Grid)btn.Parent;
            Border border = (Border)gridCell.Parent;
            Grid gridGroupTable = (Grid)border.Parent;
            GroupTypeOrder gto = (GroupTypeOrder)gridGroupTable.DataContext;
            GroupType gt = gto.Group;

            ItemTypeOrder itToBeDeleted = (ItemTypeOrder)border.DataContext;
            gt.ItemOrder.Remove(itToBeDeleted);
            GTViewModel.AdjustItemOrder(gt);
            RefreshGroupTable(gt);
            DisableAllowDropByNewLineItem();
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
                    //Grid gridCell = (Grid)b.Child;
                    //TextBlock tb = (TextBlock)gridCell.Children[1];
                    //Item it = (Item)b.DataContext;
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
                Color cellColorGroup = (Color)ColorConverter.ConvertFromString("#97CBFF");
                Border cellItem = CreateGroupCell(cellColorGroup, cellColorGroup);
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

        private Border CreateGroupCell(Color borderBrush, Color background)
        {
            Border bGroupCell = CreateBorderContainer(borderBrush, background);
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
            Grid.SetColumn(clearGroupBtn, 1);
            Grid.SetColumn(tb, 0);
            gridCell.Children.Add(clearGroupBtn);
            gridCell.Children.Add(tb);
            bGroupCell.Child = gridCell;
            return bGroupCell;
        }

        private Border CreateItemCell(Color borderBrush, Color background, ItemTypeOrder itemType)
        {
            Border bCell = CreateBorderContainer(borderBrush, background);
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
                VerticalAlignment = VerticalAlignment.Stretch//,
                //DataContext = itemType
            };
            tb.SetBinding(TextBlock.TextProperty, "Item.Header");

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
        }

        void CheckToolboxItemDrop(object sender, DragEventArgs e)
        {
            Border bCell = GetBorderByDropEvent(e);
            ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
            ToolboxItem tbi = (ToolboxItem)lbi.Content;

            if (!(bCell.DataContext is GroupType))
            {
                int row = Grid.GetRow(bCell);
                bool containsRow = CheckForNewLineItem(row);
                if (tbi.DesignID.Equals("198") && containsRow)
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

        void CheckItemTypeDrop(object sender, DragEventArgs e)
        {
            Border borderCell = GetBorderByDropEvent(e);
            int row = Grid.GetRow(borderCell);
            Border draggedBorderCell = (Border)e.Data.GetData("System.Windows.Controls.Border");
            Grid draggedGridCell = (Grid)draggedBorderCell.Child;
            TextBlock draggedTextBlock = (TextBlock)draggedGridCell.Children[1];
            int draggedItemRow = Grid.GetRow(draggedBorderCell);
            Grid draggedGroupTable = (Grid)draggedBorderCell.Parent;

            GroupType draggedGt = GetGroupType(draggedGroupTable);
            GroupType gt = GetGroupType(GroupTable);

            ItemTypeOrder ito = (ItemTypeOrder)draggedTextBlock.DataContext;
            ItemType it = ito.Item;
            if (borderCell.DataContext is ItemTypeOrder)
            {
                if (!draggedGt.GroupTypeID.Equals(gt.GroupTypeID))
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    bool containsRow = CheckForNewLineItem(row);
                    if (it.DesignID.Equals("198") && containsRow && draggedItemRow != row)
                    {
                        e.Effects = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                }
            }
            if (borderCell.DataContext is GroupType)
            {

                e.Effects = DragDropEffects.None;
            }
        }

        void CheckGroupTableDrop(object sender, DragEventArgs e)
        {
            Border bCell = GetBorderByDropEvent(e);

            if (bCell.DataContext is GroupType)
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

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
                Grid gridCell = (Grid)btn.Parent;
                bCell = (Border)gridCell.Parent;
            }
            else if (e.Source is Button)
            {
                Button btn = e.Source as Button;
                Grid gridCell = (Grid)btn.Parent;
                bCell = (Border)gridCell.Parent;
            }
            return bCell;
        }

        void HandleToolboxItemDrop(object sender, DragEventArgs e)
        {
            Border bCell = GetBorderByDropEvent(e);

            ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
            ToolboxItem tbi = (ToolboxItem)lbi.Content;

            ItemTypeOrder itToBeMoved = (ItemTypeOrder)bCell.DataContext;
            int designID = Convert.ToInt32(itToBeMoved.DesignID);
            int row = Grid.GetRow(bCell);
            Grid grid = (Grid)bCell.Parent;
            GroupTypeOrder gto = (GroupTypeOrder)grid.DataContext;
            GroupType gt = gto.Group;
            if (tbi.DesignID.Equals("198") && designID != 0)
            {
                ItemTypeOrder itemTypeOrder = new ItemTypeOrder();
                itemTypeOrder.DesignID = tbi.DesignID;
                ItemType itemType = new ItemType();
                itemType.DesignID = tbi.DesignID;
                itemType.Header = tbi.Header;
                itemTypeOrder.ItemOrder = itToBeMoved.ItemOrder;
                itemType.DanishTranslationText = tbi.DanishTranslationText;
                itemType.EnglishTranslationText = tbi.EnglishTranslationText;
                itemType.LanguageID = tbi.LanguageID;
                itemTypeOrder.GroupTypeID = gt.GroupTypeID;
                itemTypeOrder.IncludedTypeID = "1";
                itemTypeOrder.Item = itemType;

                int index = gt.ItemOrder.IndexOf(itToBeMoved);

                gt.ItemOrder.Insert(index, itemTypeOrder);
                int draggedIndex = gt.ItemOrder.IndexOf(itemTypeOrder);

                GTViewModel.AdjustItemOrderNewLineItem(gt, draggedIndex);
                RefreshGroupTable(gt);
                DisableAllowDropByNewLineItem();

            }
            if (designID != 0 && !tbi.DesignID.Equals("198") && designID != 197)
            {
                ItemTypeOrder itemTypeOrder = new ItemTypeOrder();
                itemTypeOrder.DesignID = tbi.DesignID;
                ItemType itemType = new ItemType();
                itemType.DesignID = tbi.DesignID;
                itemType.Header = tbi.Header;
                itemTypeOrder.ItemOrder = itToBeMoved.ItemOrder;
                itemType.DanishTranslationText = tbi.DanishTranslationText;
                itemType.EnglishTranslationText = tbi.EnglishTranslationText;
                itemType.LanguageID = tbi.LanguageID;
                itemTypeOrder.GroupTypeID = gt.GroupTypeID;
                itemTypeOrder.IncludedTypeID = "1";
                itemTypeOrder.Item = itemType;

                //List<ItemTypeOrder> itemTypeList = GetItemTypes();
                int startPosition = gt.ItemOrder.IndexOf(itToBeMoved);
                GTViewModel.MoveItemsForward(startPosition, itemTypeOrder, gt);
                RefreshGroupTable(gt);
                DisableAllowDropByNewLineItem();
            }
            if (designID == 0)
            {
                itToBeMoved.DesignID = tbi.DesignID;
                ItemType itemType = new ItemType();
                itemType.DesignID = tbi.DesignID;
                itemType.Header = tbi.Header;
                itemType.DanishTranslationText = tbi.DanishTranslationText;
                itemType.EnglishTranslationText = tbi.EnglishTranslationText;
                itemType.LanguageID = tbi.LanguageID;
                itToBeMoved.GroupTypeID = gt.GroupTypeID;
                itToBeMoved.IncludedTypeID = "1";
                itToBeMoved.Item = itemType;
                gt.ItemOrder.Add(itToBeMoved);
                GTViewModel.GenerateEmptyFields(gt);
                RefreshGroupTable(gt);
                if (tbi.DesignID.Equals("198"))
                {
                    DisableAllowDropByNewLineItem();
                }
            }
            if (designID == 197 && !tbi.DesignID.Equals("198"))
            {
                itToBeMoved.DesignID = tbi.DesignID;
                itToBeMoved.Item.Header = tbi.Header;
                itToBeMoved.Item.DanishTranslationText = tbi.DanishTranslationText;
                itToBeMoved.Item.EnglishTranslationText = tbi.EnglishTranslationText;
                itToBeMoved.Item.LanguageID = tbi.LanguageID;
                itToBeMoved.GroupTypeID = gt.GroupTypeID;
                itToBeMoved.IncludedTypeID = "1";
            }
        }

        void HandleItemTypeDrop(object sender, DragEventArgs e)
        {
            Border target = GetBorderByDropEvent(e);
            ItemTypeOrder targetItemType = (ItemTypeOrder)target.DataContext;

            Border target2 = e.Data.GetData("System.Windows.Controls.Border") as Border;
            ItemTypeOrder draggedItemType = (ItemTypeOrder)target2.DataContext;
            //Grid groupTable2 = (Grid)target2.Parent;
            Grid groupTable = (Grid)target.Parent;
            GroupType gt = GetGroupType(groupTable);
            int draggedPosition = gt.ItemOrder.IndexOf(draggedItemType);
            double targetItemTypeNo = targetItemType.ItemOrder; //affected item
            int targetPosition = gt.ItemOrder.IndexOf(targetItemType);


            if (targetItemType != draggedItemType)
            {
                gt.ItemOrder.Remove(draggedItemType);
                if (targetItemType.DesignID == null)
                {
                    GTViewModel.AdjustItemOrder(gt);
                    draggedItemType.ItemOrder = targetItemType.ItemOrder;
                    gt.ItemOrder.Add(draggedItemType);
                    gt.ItemOrder.Sort(i => i.ItemOrder);
                    GTViewModel.GenerateEmptyFields(gt);
                    RefreshGroupTable(gt);
                }

                else if (draggedItemType.DesignID.Equals("198"))
                {
                    if (draggedItemType.ItemOrder > targetItemType.ItemOrder)
                    {
                        gt.ItemOrder.Insert(targetPosition, draggedItemType);
                        draggedItemType.ItemOrder = targetItemTypeNo;
                        GTViewModel.AdjustItemOrder(gt);
                    }
                    else
                    {
                        draggedItemType.ItemOrder = targetItemTypeNo;
                        gt.ItemOrder.Insert(targetPosition - 1, draggedItemType);
                        GTViewModel.AdjustItemOrder(gt);
                    }
                    GTViewModel.GenerateEmptyFields(gt);
                    gt.ItemOrder.Sort(i => i.ItemOrder);
                    RefreshGroupTable(gt);
                }

                else if (targetItemType.DesignID != null && draggedItemType.DesignID != null /*&& !draggedItemType.DesignID.Equals("198")*/)
                {
                    if (draggedItemType.ItemOrder > targetItemType.ItemOrder)
                    {
                        gt.ItemOrder.Insert(targetPosition, draggedItemType);
                        draggedItemType.ItemOrder = targetItemTypeNo;
                    }
                    else
                    {
                        if (gt.ItemOrder.Count != targetPosition)
                        {
                            draggedItemType.ItemOrder = targetItemTypeNo;
                            gt.ItemOrder.Insert(targetPosition, draggedItemType);
                        }
                        else
                        {
                            draggedItemType.ItemOrder = targetItemTypeNo;
                            gt.ItemOrder.Add(draggedItemType);
                        }
                    }
                    GTViewModel.AdjustItemOrder(gt, targetPosition, draggedPosition);

                    GTViewModel.GenerateEmptyFields(gt);
                    gt.ItemOrder.Sort(i => i.ItemOrder);
                    RefreshGroupTable(gt);
                }
            }
        }

        void RefreshGroupTable(GroupType gt)
        {
            GroupTable.ClearGrid();
            PopulateGroupTable(gt);
            DisableAllowDropByNewLineItem();
        }

        void HandleGroupTableDrop(object sender, DragEventArgs e)
        {
            Grid targetGroupTable = e.Data.GetData("System.Windows.Controls.Grid") as Grid;
            GroupTableUserControl gtUC = (GroupTableUserControl)targetGroupTable.Parent;
            GroupTypeOrder targetGroupTypeOrder = MyGroupTypeOrder;

            GroupTypeOrder draggedGroupTypeOrder = gtUC.MyGroupTypeOrder;
            GTViewModel.HandleGroupTableDrop(targetGroupTypeOrder, draggedGroupTypeOrder);
            //ParentGroupTableContainerUserControl.AdjustGroupOrder(draggedGroupTypeOrder, targetGroupTypeOrder);
            //ParentGroupTableContainerUserControl.MoveGroup((GroupTableUserControl)draggedGroupTable.Parent, (GroupTableUserControl)targetGroupTable.Parent);
        }

        void border_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem) //Drag and drop toolboxitem
            {
                HandleToolboxItemDrop(sender, e);
            }
            if (e.Data.GetData("System.Windows.Controls.Border") is Border) //Drag and drop between items
            {
                HandleItemTypeDrop(sender, e);
            }
            if (e.Data.GetData("System.Windows.Controls.Grid") is Grid) //Drag and drop groups 
            {
                HandleGroupTableDrop(sender, e);
            }
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
            if (bCell.DataContext is GroupType)
            {
                Button btnClearCell = (Button)cellItem.Children[0];
                btnClearCell.Visibility = Visibility.Visible;
            }
        }

        void border_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bCell = sender as Border;
            Grid cellItem = (Grid)bCell.Child;
            Button btnClearCell = (Button)cellItem.Children[0];
            btnClearCell.Visibility = Visibility.Hidden;
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
            if (adorner != null)
            {
                Border bCell = sender as Border;
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
                        adorner = new DragAdornerItem(draggedItem, e.GetPosition(draggedItem));
                        AdornerLayer.GetAdornerLayer(this).Add(adorner);
                        if (it.DesignID.Equals("198"))
                        {
                            int row = Grid.GetRow(draggedItem);
                            EnableAllowDropNewLine(0, row);
                        }
                        DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.None | DragDropEffects.Move);
                        AdornerLayer.GetAdornerLayer(this).Remove(adorner);
                    }
                }
                if (draggedItem.DataContext is GroupType)
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
            btnClearCell.Click += btnRemove_Click;

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
            //btnClearCell.Click += btnRemove_Click;

            return btnClearCell;
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