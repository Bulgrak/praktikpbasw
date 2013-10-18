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
using DocumentFormat.OpenXml.EMMA;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for GroupTableUserControl.xaml
    /// </summary>
    public partial class GroupTableUserControl : UserControl
    {
        public GroupTableContainerUserControl ParentGroupTableContainerUserControl { get; set; }

        public GroupTableUserControl()
        {
            InitializeComponent();
        }

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
                            //gt.Items[j + skipped].ItemOrder = j + skipped;
                            SolidColorBrush textColor2 = Brushes.Black;
                            InsertItem(gt.ItemOrder[j + skipped], counterRow, j % 4, textColor2);
                            j--;
                            skipped++;

                            continue;
                        }
                        gt.ItemOrder[j + skipped].Item.Header = "<NewLineItem>";
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
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
                        InsertItem(gt.ItemOrder[j + skipped], counterRow, j % 4, textColor);
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
                    }
                    else
                    {
                        if (counterColumn >= 4)
                        {
                            counterColumn = 0;
                        }
                        SolidColorBrush textColor = Brushes.Black;
                        InsertItem(gt.ItemOrder[j + skipped], counterRow, j % 4, textColor);
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
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

        public List<ItemTypeOrder> GetItemTypes()
        {
            List<ItemTypeOrder> itemTypeList = new List<ItemTypeOrder>();
            int i = 1;
            while (i < GroupTable.RowDefinitions.Count)
            {
                int j = 0;
                while (j < GroupTable.ColumnDefinitions.Count)
                {
                    Border bCell = (Border)GroupTable.GetCellChild(i, j);
                    ItemTypeOrder it = (ItemTypeOrder)bCell.DataContext;
                    if (it.DesignID != null)
                        itemTypeList.Add(it);
                    j++;
                }
                i++;
            }
            return itemTypeList;
        }

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
            gridGroupTable.RemoveRow(gridGroupTable.RowDefinitions.Count - 1);
            GroupTypeOrder gto = (GroupTypeOrder)gridGroupTable.DataContext;
            GroupType gt = gto.Group;

            ItemTypeOrder itToBeDeleted = (ItemTypeOrder)border.DataContext;
            gt.ItemOrder.Remove(itToBeDeleted);

            int i = Convert.ToInt32(itToBeDeleted.ItemOrder);
            ItemType itemType = new ItemType();
            border.DataContext = itemType;

            List<ItemTypeOrder> itemTypeList = GetItemTypes();
            bool stopCounting = false;

            while (i < itemTypeList.Count && !stopCounting)
            {
                itemTypeList[i].ItemOrder--;
                i++;
            }

            itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
            ObservableCollection<ItemTypeOrder> ocItemTypeList = new ObservableCollection<ItemTypeOrder>();

            //add list to observablecollection
            foreach (ItemTypeOrder it in itemTypeList)
            {
                ocItemTypeList.Add(it);
            }
            gt.ItemOrder = ocItemTypeList;
            gridGroupTable.ClearGrid();
            PopulateGroupTable(gt);
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

        private bool CheckIfRowIsEmpty(int row)
        {
            List<ItemTypeOrder> itemTypeList = GetItemsByRow(row);
            bool isEmpty = true;
            int i = 0;
            while (i < itemTypeList.Count && isEmpty)
            {
                if (itemTypeList[i].DesignID != null)
                {
                    isEmpty = false;
                }
                i++;
            }
            return isEmpty;
        }

        //private void GenerateEmptyFields(int row, bool wholeRow)
        //{
        //    List<ItemType> itemTypeListCheck = GetItemsByRow(row);
        //    GroupTypeOrder gto = (GroupTypeOrder)GroupTable.DataContext;
        //    GroupType gt = gto.Group;
        //    bool newLineItemExist = CheckForNewLineItem(row);
        //    bool addEmptyfields = false;
        //    int i = itemTypeListCheck.Count - 1;

        //    while (i >= 0)
        //    {
        //        ItemType itemType = itemTypeListCheck[i];
        //        string designID = itemType.DesignID;

        //        if (newLineItemExist && !wholeRow)
        //        {
        //            addEmptyfields = false;
        //        }

        //        if (newLineItemExist && designID != null && itemType.DesignID.Equals("198") && !wholeRow)
        //        {
        //            addEmptyfields = true;
        //            newLineItemExist = false;
        //            wholeRow = false;
        //        }

        //        if (!newLineItemExist && !addEmptyfields && itemType.DesignID != null && !wholeRow)
        //        {
        //            addEmptyfields = true;
        //        }

        //        if (addEmptyfields && itemType.DesignID == null && !wholeRow)
        //        {
        //            itemType.DesignID = "197";
        //            itemType.Header = "<EmptyField>";
        //            itemType.GroupTypeID = gt.GroupTypeID;
        //            itemType.IncludedTypeID = "1";
        //            gt.Items.Add(itemType);
        //        }

        //        if (wholeRow && designID == null && !newLineItemExist)
        //        {
        //            itemType.DesignID = "197";
        //            itemType.Header = "<EmptyField>";
        //            itemType.GroupTypeID = gt.GroupTypeID;
        //            itemType.IncludedTypeID = "1";
        //            gt.Items.Add(itemType);
        //        }
        //        i--;
        //    }
        //}

        private void GenerateEmptyFields(GroupType gt)
                {
            int i = 0;
            int j = 0;
            int k = 0;
            while (i < gt.Items.Count)
            {
                if (gt.Items.Count > 1)
                {
                    if (gt.Items[i].ItemOrder != i)
                    {
                        if (!gt.Items[i - 1].DesignID.Equals("198"))
                        {
                            k = (int)gt.Items[i].ItemOrder - (int)gt.Items[i - 1].ItemOrder;
                            j = k + i;
                            while (k > 1)
                            {
                                gt.Items.Insert(i, CreateEmptyField(gt));
                                k--;
                }
                            i = j - 1;
                        }
                        else
                        {
                            k = (int)gt.Items[i].ItemOrder - ((int)gt.Items[i - 1].ItemOrder + (i % 4));
                            j = k + i;
                            while (k > 1)
                {
                                gt.Items.Insert(i, CreateEmptyField(gt));
                                k--;
                            }
                            i = j - 1;
                        }
                }
                    i++;

                }
                else
                {
                    k = (int)gt.Items[i].ItemOrder;
                    j = k + i;
                    while (k > 0)
                {
                        gt.Items.Insert(i, CreateEmptyField(gt));
                        k--;
                    }
                    break;
                }
                }
                }

        private ItemType CreateEmptyField(GroupType gt)
                {
            ItemType emptyFieldItemType = new ItemType();
            emptyFieldItemType.DesignID = "197";
            emptyFieldItemType.Header = "<EmptyField>";
            emptyFieldItemType.GroupTypeID = gt.GroupTypeID;
            emptyFieldItemType.IncludedTypeID = "1";
            return emptyFieldItemType;
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
            tb.SetBinding(TextBlock.TextProperty, "Header");

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

            ItemType it = (ItemType)draggedTextBlock.DataContext;

            if (borderCell.DataContext is ItemType)
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
                ItemTypeOrder newItemType = new ItemTypeOrder();
                newItemType.DesignID = tbi.DesignID;
                newItemType.Item.Header = tbi.Header;
                newItemType.ItemOrder = itToBeMoved.ItemOrder;
                newItemType.Item.DanishTranslationText = tbi.DanishTranslationText;
                newItemType.Item.EnglishTranslationText = tbi.EnglishTranslationText;
                newItemType.Item.LanguageID = tbi.LanguageID;
                newItemType.GroupTypeID = gt.GroupTypeID;
                newItemType.Item.IncludedTypeID = "1";
                int index = gt.ItemOrder.IndexOf(itToBeMoved);

                gt.ItemOrder.Insert(index, newItemType);
                grid.ClearGrid();
                PopulateGroupTable(gt);
                DisableAllowDropByNewLineItem();

            }
            if (designID != 0 && !tbi.DesignID.Equals("198") && designID != 197)
            {
                ItemTypeOrder newItemType = new ItemTypeOrder();
                newItemType.DesignID = tbi.DesignID;
                newItemType.Item.Header = tbi.Header;
                newItemType.ItemOrder = itToBeMoved.ItemOrder;
                newItemType.Item.DanishTranslationText = tbi.DanishTranslationText;
                newItemType.Item.EnglishTranslationText = tbi.EnglishTranslationText;
                newItemType.Item.LanguageID = tbi.LanguageID;
                newItemType.GroupTypeID = gt.GroupTypeID;
                newItemType.Item.IncludedTypeID = "1";

                List<ItemTypeOrder> itemTypeList = GetItemTypes();
                int startPosition = itemTypeList.IndexOf(itToBeMoved);
                MoveItemsForward(startPosition, itemTypeList, grid, newItemType, gt);


                grid.ClearGrid();
                PopulateGroupTable(gt);
                DisableAllowDropByNewLineItem();
            }
            if (designID == 0)
            {
                itToBeMoved.DesignID = tbi.DesignID;
                itToBeMoved.Item.Header = tbi.Header;
                itToBeMoved.Item.DanishTranslationText = tbi.DanishTranslationText;
                itToBeMoved.Item.EnglishTranslationText = tbi.EnglishTranslationText;
                itToBeMoved.Item.LanguageID = tbi.LanguageID;
                itToBeMoved.GroupTypeID = gt.GroupTypeID;
                itToBeMoved.IncludedTypeID = "1";



                gt.ItemOrder.Add(itToBeMoved);
                gt.Items.Add(itToBeMoved);
                GenerateEmptyFields(gt);
                grid.ClearGrid();
                PopulateGroupTable(gt);
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
                itToBeMoved.Item.IncludedTypeID = "1";
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

            double targetItemTypeNo = targetItemType.ItemOrder; //affected item

            gt.ItemOrder.Remove(draggedItemType);
            int position = gt.ItemOrder.IndexOf(targetItemType);
            if (targetItemType != draggedItemType)
            {
                if (targetItemType.DesignID == null)
                {
                    int i = 0;
                    while (i < gt.ItemOrder.Count)
                    {
                        gt.ItemOrder[i].ItemOrder--;
                        i++;
                    }
                    draggedItemType.ItemOrder = targetItemType.ItemOrder;
                    int row = Grid.GetRow(target);
                    int column = Grid.GetColumn(target);
                    int rowCount = groupTable.RowDefinitions.Count;
                    int lastRow = groupTable.RowDefinitions.Count - 1;
                    RefreshGroupTable(gt);
                    Border newTarget = null;
                    if (rowCount == groupTable.RowDefinitions.Count + 1)
                    {
                        AddNewEmptyItemRow(); // Adds new row in case the item has been drop at the last row
                        if (row == lastRow)
                        {
                            newTarget =
                                (Border)GetCellItem(groupTable.RowDefinitions.Count - 1, column);
                        }
                        else
                        {
                            newTarget =
                                (Border)GetCellItem(groupTable.RowDefinitions.Count - 2, column);
                        }
                    }
                    else
                    {
                        newTarget = (Border)GetCellItem(row, column);
                    }
                    newTarget.DataContext = draggedItemType;
                    i = 1;
                    while (i < groupTable.RowDefinitions.Count - 2)
                    {
                        if (!CheckIfRowIsEmpty(i))
                        {
                            //GenerateEmptyFields(i, CheckForNewLineItem(i));
                        }
                        i++;
                    }
                    if (!(row == groupTable.RowDefinitions.Count - 2))
                    {
                        //GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, true);
                    }
                    else
                    {
                        //GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, false);
                    }
                    //GenerateEmptyFields(groupTable.RowDefinitions.Count - 1, false);
                    gt.Items.Add(draggedItemType);
                    List<ItemType> itemTypeList = gt.Items.ToList();
                    itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
                    ObservableCollection<ItemTypeOrder> ocItemTypeList = new ObservableCollection<ItemTypeOrder>(itemTypeList);
                    gt.ItemOrder = ocItemTypeList;

                    RefreshGroupTable(gt);
                }
                //else if (draggedItemType.DesignID.Equals("198"))
                //{
                //    Item newit = new Item();
                //    newit.ItemOrder = draggedItemType.ItemOrder;
                //    target.DataContext = newit;
                //    target2.DataContext = draggedItemType;
                //    gt.Items.Remove(draggedItemType);
                //    if (position != -1)
                //    {
                //        gt.Items.Insert(position, draggedItemType);
                //        //gt.Items[position].ItemOrder = targetItemTypeNo;
                //    }
                //    if (targetItemType.DesignID == null)
                //    {
                //        gt.Items.Add(draggedItemType);
                //    }
                //    draggedItemType.ItemOrder = targetItemTypeNo;
                //    //RefreshGroupTable(gt);
                //    int i = 1;
                //    while (i < groupTable.RowDefinitions.Count - 2)
                //    {

                //        if (!CheckIfRowIsEmpty(i))
                //        {
                //            GenerateEmptyFields(i, true);
                //        }
                //        i++;
                //    }
                //    GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, false);
                //    GenerateEmptyFields(groupTable.RowDefinitions.Count - 1, false);
                //    groupTable.ClearGrid();
                //    PopulateGroupTable(gt);
                //    DisableAllowDropByNewLineItem();
                //}
                //else if (draggedItemType.DesignID("198"))
                //{
                    
                //}

                else if (draggedItemType.DesignID.Equals("198"))
                {
                    if (draggedItemType.ItemOrder > targetItemType.ItemOrder)
                    {
                        gt.ItemOrder.Insert(position, draggedItemType);
                        draggedItemType.ItemOrder = targetItemTypeNo;
                    }
                    else
                    {
                        gt.ItemOrder[position].ItemOrder--;
                        draggedItemType.ItemOrder = targetItemTypeNo;
                        gt.ItemOrder.Insert(position, draggedItemType);
                    }
                    //gt = SortItemList(gt);
                    //RefreshGroupTable(gt);
                    int i = 1;
                    while (i < groupTable.RowDefinitions.Count - 2)
                    {
                        if (!CheckIfRowIsEmpty(i))
                        {
                            if (!CheckForNewLineItem(i))
                            {
                                //GenerateEmptyFields(i, true);
                            }
                            else
                            {
                                //GenerateEmptyFields(i, false);
                            }
                        }
                        
                        i++;
                    }
                    //GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, false);
                    //GenerateEmptyFields(groupTable.RowDefinitions.Count - 1, false);
                    gt = SortItemList(gt);
                    RefreshGroupTable(gt);
                }

                else if (targetItemType.DesignID != null && draggedItemType.DesignID != null && !draggedItemType.DesignID.Equals("198"))
                {
                    if (draggedItemType.ItemOrder > targetItemType.ItemOrder)
                    {
                        gt.ItemOrder.Insert(position, draggedItemType);
                        draggedItemType.ItemOrder = targetItemTypeNo;
                    }
                    else
                    {
                        gt.ItemOrder[position].ItemOrder--;
                        draggedItemType.ItemOrder = targetItemTypeNo;
                        gt.ItemOrder.Insert(position, draggedItemType);
                    }
                    //gt = SortItemList(gt);
                    //RefreshGroupTable(gt);
                    int i = 1;
                    while (i < groupTable.RowDefinitions.Count - 2)
                    {
                        if (!CheckIfRowIsEmpty(i))
                        {
                            //GenerateEmptyFields(i, true);
                        }
                        i++;
                    }
                    //GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, false);
                    //GenerateEmptyFields(groupTable.RowDefinitions.Count - 1, false);
                    gt = SortItemList(gt);
                    RefreshGroupTable(gt);
                }
            }
        }

        GroupType SortItemList(GroupType gt)
        {
            List<ItemTypeOrder> itemTypeList = gt.ItemOrder.ToList();
            itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
            ObservableCollection<ItemTypeOrder> ocItemTypeList = new ObservableCollection<ItemTypeOrder>(itemTypeList);
            gt.ItemOrder = ocItemTypeList;
            return gt;
        }

        void RefreshGroupTable(GroupType gt)
        {
            GroupTable.ClearGrid();
            PopulateGroupTable(gt);
            DisableAllowDropByNewLineItem();
        }

        void HandleGroupTableDrop(object sender, DragEventArgs e)
        {
            Grid targetGroupTable = GroupTable;
            Grid draggedGroupTable = e.Data.GetData("System.Windows.Controls.Grid") as Grid;

            ParentGroupTableContainerUserControl.MoveGroup((GroupTableUserControl)draggedGroupTable.Parent, (GroupTableUserControl)targetGroupTable.Parent);
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

        void MoveItemsForward(int startPosition, List<ItemTypeOrder> itemTypeList, Grid grid, ItemTypeOrder newItemType, GroupType gt)
        {
            bool stopCounting = false;
            int i = startPosition;
            while (i < itemTypeList.Count && !stopCounting)
            {
                if (itemTypeList[i].DesignID.Equals("198"))
                {
                    if (itemTypeList[i].ItemOrder % 4 == 3)
                    {
                        itemTypeList.RemoveAt(i);
                    }
                    else
                    {
                        itemTypeList[i].ItemOrder++;
                    }
                    stopCounting = true;
                }
                else
                {
                    itemTypeList[i].ItemOrder++;
                    i++;
                }
            }
            itemTypeList.Add(newItemType);

            itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
            ObservableCollection<ItemTypeOrder> ocItemTypeList = new ObservableCollection<ItemTypeOrder>();

            //add list to observablecollection
            foreach (ItemTypeOrder it in itemTypeList)
            {
                ocItemTypeList.Add(it);
            }
            gt.ItemOrder = ocItemTypeList;
            grid.ClearGrid();
            PopulateGroupTable(gt);
        }

        void border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bCell = sender as Border;
            Grid cellItem = (Grid)bCell.Child;
            if (bCell.DataContext is ItemType)
            {
                ItemType itemType = (ItemType)bCell.DataContext;
                if (itemType.Header != null)
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
                if (draggedItem.DataContext is ItemType) //Prevent dragging if GroupType
                {
                    ItemType it = (ItemType)draggedItem.DataContext;
                    if (it.Header != null)
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
                    AdornerLayer.GetAdornerLayer(this).Remove(adorner);
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