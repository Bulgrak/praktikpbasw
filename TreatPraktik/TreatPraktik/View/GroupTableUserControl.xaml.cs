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
                for (int j = 0; j < gt.Items.Count - skipped; j++)
                {
                    if (gt.Items[j + skipped].DesignID.Equals("198"))
                    {
                        if (j % 4 == 0)
                        {
                            AddNewEmptyItemRow();
                            counterRow++;
                            gt.Items[j + skipped].Header = "<NewLineItem>";
                            //gt.Items[j + skipped].ItemOrder = j + skipped;
                            SolidColorBrush textColor2 = Brushes.Black;
                            InsertItem(gt.Items[j + skipped], counterRow, j % 4, textColor2);
                            j--;
                            skipped++;

                            continue;
                        }
                        gt.Items[j + skipped].Header = "<NewLineItem>";
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
                        SolidColorBrush textColor = Brushes.Black;
                        InsertItem(gt.Items[j + skipped], counterRow, j % 4, textColor);
                        skipped = skipped + j;
                        j = 0;
                    }
                    else if (j % 4 == 0)
                    {
                        AddNewEmptyItemRow();
                        counterRow++;
                    }
                    if (gt.Items[j + skipped].DesignID.Equals("198"))
                    {
                        j--;
                        skipped++;
                        continue;
                    }
                    if (gt.Items[j + skipped].DesignID.Equals("197"))
                    {
                        SolidColorBrush textColor = Brushes.Black;
                        InsertItem(gt.Items[j + skipped], counterRow, j % 4, textColor);
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
                    }
                    else
                    {
                        if (counterColumn >= 4)
                        {
                            counterColumn = 0;
                        }
                        SolidColorBrush textColor = Brushes.Black;
                        InsertItem(gt.Items[j + skipped], counterRow, j % 4, textColor);
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
            Grid gridGroupCell = (Grid)b.Child;
            TextBlock tb = (TextBlock)gridGroupCell.Children[1];
            GroupType gt = (GroupType)tb.DataContext;
            return gt;
        }

        public List<ItemType> GetItemTypes()
        {
            List<ItemType> itemTypeList = new List<ItemType>();
            int i = 1;
            while (i < GroupTable.RowDefinitions.Count)
            {
                int j = 0;
                while (j < GroupTable.ColumnDefinitions.Count)
                {
                    Border b = (Border)GroupTable.GetCellChild(i, j);
                    Grid gridCell = (Grid)b.Child;
                    TextBlock tb = (TextBlock)gridCell.Children[1];
                    ItemType it = (ItemType)tb.DataContext;
                    if (it.DesignID != null)
                        itemTypeList.Add(it);
                    j++;
                }
                i++;
            }
            return itemTypeList;
        }

        private void InsertItem(ItemType itemType, int row, int column, SolidColorBrush textColor)
        {
            Border border = GetCellItem(row, column);
            Grid cellItem = (Grid)border.Child;
            TextBlock tb = (TextBlock)cellItem.Children[1];
            ItemType dummyItemType = (ItemType)tb.DataContext;
            itemType.ItemOrder = dummyItemType.ItemOrder;
            tb.DataContext = itemType;
            tb.SetBinding(TextBlock.TextProperty, "Header");
            tb.Foreground = textColor;
        }

        private void InsertGroupItem(GroupType groupType, int row, int column)
        {
            Border bCell = GetCellItem(row, column);
            bCell.DataContext = groupType;
            Grid gridGroupCell = (Grid)bCell.Child;
            TextBlock tb = (TextBlock)gridGroupCell.Children[1];
            tb.DataContext = groupType;
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
                ItemType itemType = new ItemType();
                itemType.ItemOrder = ((row - 1) * 4) + i;
                Border border = CreateItemCell(Colors.Black, Colors.Yellow, itemType);
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
            TextBlock tb = (TextBlock)gridCell.Children[1];

            ItemType itToBeDeleted = (ItemType)tb.DataContext;
            gt.Items.Remove(itToBeDeleted);

            int i = Convert.ToInt32(itToBeDeleted.ItemOrder);
            ItemType itemType = new ItemType();
            tb.DataContext = itemType;

            List<ItemType> itemTypeList = GetItemTypes();
            bool stopCounting = false;

            while (i < itemTypeList.Count && !stopCounting)
            {
                itemTypeList[i].ItemOrder--;
                i++;
            }

            itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
            ObservableCollection<ItemType> ocItemTypeList = new ObservableCollection<ItemType>();

            //add list to observablecollection
            foreach (ItemType it in itemTypeList)
            {
                ocItemTypeList.Add(it);
            }
            gt.Items = ocItemTypeList;
            gridGroupTable.ClearGrid();
            PopulateGroupTable(gt);
            DisableAllowDropByNewLineItem();
        }

        private List<ItemType> GetItemsByRow(int row)
        {
            List<UIElement> uieList = GroupTable.GetGridCellChildrenByRow(row);
            List<ItemType> itemTypeListCheck = new List<ItemType>();
            foreach (UIElement uie in uieList)
            {
                if (uie is Border)
                {
                    Border b = (Border)uie;
                    Grid gridCell = (Grid)b.Child;
                    TextBlock tb = (TextBlock)gridCell.Children[1];
                    ItemType it = (ItemType)tb.DataContext;
                    itemTypeListCheck.Add(it);
                }
            }
            return itemTypeListCheck;
        }

        private bool CheckForNewLineItem(int row)
        {
            List<ItemType> itemTypeListCheck = GetItemsByRow(row);
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
            List<ItemType> itemTypeList = GetItemsByRow(row);
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

        private void GenerateEmptyFields(int row, bool wholeRow)
        {
            List<ItemType> itemTypeListCheck = GetItemsByRow(row);
            GroupTypeOrder gto = (GroupTypeOrder)GroupTable.DataContext;
            GroupType gt = gto.Group;
            bool newLineItemExist = CheckForNewLineItem(row);
            bool addEmptyfields = false;
            int i = itemTypeListCheck.Count - 1;

            while (i >= 0)
            {
                ItemType itemType = itemTypeListCheck[i];
                string designID = itemType.DesignID;

                if (newLineItemExist && !wholeRow)
                {
                    addEmptyfields = false;
                }

                if (newLineItemExist && designID != null && itemType.DesignID.Equals("198") && !wholeRow)
                {
                    addEmptyfields = true;
                    newLineItemExist = false;
                    wholeRow = false;
                }

                if (!newLineItemExist && !addEmptyfields && itemType.DesignID != null && !wholeRow)
                {
                    addEmptyfields = true;
                }

                if (addEmptyfields && itemType.DesignID == null && !wholeRow)
                {
                    itemType.DesignID = "197";
                    itemType.Header = "<EmptyField>";
                    itemType.GroupTypeID = gt.GroupTypeID;
                    itemType.IncludedTypeID = "1";
                    gt.Items.Add(itemType);
                }

                if (wholeRow && designID == null && !newLineItemExist)
                {
                    itemType.DesignID = "197";
                    itemType.Header = "<EmptyField>";
                    itemType.GroupTypeID = gt.GroupTypeID;
                    itemType.IncludedTypeID = "1";
                    gt.Items.Add(itemType);
                }
                i--;
            }
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
            Border border = CreateBorderContainer(borderBrush, background);
            border.MouseEnter += border_MouseEnter;
            border.MouseLeave += border_MouseLeave;
            border.AllowDrop = true;
            border.Drop += border_Drop;
            border.DragEnter += border_DragEnter;
            border.DragLeave += border_DragLeave;
            border.DragOver += border_DragOver;
            TextBlock tb = new TextBlock { FontWeight = FontWeights.Bold, FontSize = 14.0 };
            Grid gridCell = new Grid();
            CreateColumns(gridCell, 2);
            Button clearGroupBtn = CreateClearGroupBtn();
            Grid.SetColumn(clearGroupBtn, 1);
            Grid.SetColumn(tb, 0);
            gridCell.Children.Add(clearGroupBtn);
            gridCell.Children.Add(tb);
            border.Child = gridCell;
            return border;
        }

        private Border CreateItemCell(Color borderBrush, Color background, ItemType itemType)
        {
            Border border = CreateBorderContainer(borderBrush, background);
            border.MouseEnter += border_MouseEnter;
            border.MouseLeave += border_MouseLeave;
            border.AllowDrop = true;
            border.Drop += border_Drop;
            border.DragEnter += border_DragEnter;
            border.DragLeave += border_DragLeave;
            border.DragOver += border_DragOver;
            TextBlock tb = new TextBlock
            {
                FontSize = 14.0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                DataContext = itemType
            };
            tb.SetBinding(TextBlock.TextProperty, "Header");

            Grid gridCell = new Grid();
            CreateColumns(gridCell, 2);
            Button clearCellBtn = CreateClearCellBtn();
            Grid.SetColumn(clearCellBtn, 1);
            Grid.SetColumn(tb, 0);
            gridCell.Children.Add(clearCellBtn);
            gridCell.Children.Add(tb);
            border.Child = gridCell;
            return border;
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
            TextBlock tb = null;
            Grid gridCell = null;
            if (e.Source is Border)
            {
                Border target = e.Source as Border;
                gridCell = (Grid)target.Child;
                tb = (TextBlock)gridCell.Children[1];
            }
            else
            {
                tb = e.Source as TextBlock;
                gridCell = (Grid)tb.Parent;
            }
            ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
            ToolboxItem tbi = (ToolboxItem)lbi.Content;

            if (!(tb.DataContext is GroupType))
            {
                Border borderCell = (Border)gridCell.Parent;
                int row = Grid.GetRow(borderCell);
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
            TextBlock tb = null;
            Grid gridCell = null;
            Grid groupTable = null;
            Border borderCell = null;
            ItemType it = null;
            Image img = null;
            Button btn = null;
            int row = 0;
            if (e.Source is Border)
            {
                borderCell = e.Source as Border;

                groupTable = (Grid)borderCell.Parent;
                gridCell = (Grid)borderCell.Child;
                tb = (TextBlock)gridCell.Children[1];
            }
            if (e.Source is TextBlock)
            {
                tb = e.Source as TextBlock;
                gridCell = (Grid)tb.Parent;
                borderCell = (Border)gridCell.Parent;
                groupTable = (Grid)borderCell.Parent;
            }
            if (e.Source is Image)
            {
                img = e.Source as Image;
                btn = (Button)img.Parent;
                gridCell = (Grid)btn.Parent;
                borderCell = (Border)gridCell.Parent;
                groupTable = (Grid)borderCell.Parent;
                tb = (TextBlock)gridCell.Children[1];
            }
            if (e.Source is Button)
            {
                btn = e.Source as Button;
                gridCell = (Grid)btn.Parent;
                borderCell = (Border)gridCell.Parent;
                groupTable = (Grid)borderCell.Parent;
                tb = (TextBlock)gridCell.Children[1];
            }
            row = Grid.GetRow(borderCell);
            Border draggedBorderCell = (Border)e.Data.GetData("System.Windows.Controls.Border");
            Grid draggedGridCell = (Grid)draggedBorderCell.Child;
            TextBlock draggedTextBlock = (TextBlock)draggedGridCell.Children[1];
            int draggedItemRow = Grid.GetRow(draggedBorderCell);
            Grid draggedGroupTable = (Grid)draggedBorderCell.Parent;

            GroupType draggedGt = GetGroupType(draggedGroupTable);
            GroupType gt = GetGroupType(groupTable);

            it = (ItemType)draggedTextBlock.DataContext;

            if (tb.DataContext is ItemType)
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
            if (tb.DataContext is GroupType)
            {

                e.Effects = DragDropEffects.None;
            }   
        }

        void CheckGroupTableDrop(object sender, DragEventArgs e)
        {
            TextBlock tb = null;
            Grid gridCell = null;
            Grid groupTable = null;
            Border borderCell = null;
            ItemType it = null;
            Image img = null;
            Button btn = null;
            int row = 0;
            if (e.Source is Border)
            {
                borderCell = e.Source as Border;

                groupTable = (Grid)borderCell.Parent;
                gridCell = (Grid)borderCell.Child;
                tb = (TextBlock)gridCell.Children[1];
            }
            if (e.Source is TextBlock)
            {
                tb = e.Source as TextBlock;
                gridCell = (Grid)tb.Parent;
                borderCell = (Border)gridCell.Parent;
                groupTable = (Grid)borderCell.Parent;
            }
            if (e.Source is Image)
            {
                img = e.Source as Image;
                btn = (Button)img.Parent;
                gridCell = (Grid)btn.Parent;
                borderCell = (Border)gridCell.Parent;
                groupTable = (Grid)borderCell.Parent;
                tb = (TextBlock)gridCell.Children[1];
            }
            if (e.Source is Button)
            {
                btn = e.Source as Button;
                gridCell = (Grid)btn.Parent;
                borderCell = (Border)gridCell.Parent;
                groupTable = (Grid)borderCell.Parent;
                tb = (TextBlock)gridCell.Children[1];
            }
            if (tb.DataContext is GroupType)
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        void HandleToolboxItemDrop(object sender, DragEventArgs e)
        {
            TextBlock tb = null;
            Grid gridCell = null;
            Border target = null;
            if (e.Source is Border)
            {
                target = e.Source as Border;
                gridCell = (Grid)target.Child;
                tb = (TextBlock)gridCell.Children[1];
            }
            else
            {

                tb = e.Source as TextBlock;
                gridCell = (Grid)tb.Parent;
                target = (Border)gridCell.Parent;
            }
            ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
            ToolboxItem tbi = (ToolboxItem)lbi.Content;

            ItemType itToBeMoved = (ItemType)tb.DataContext;
            int designID = Convert.ToInt32(itToBeMoved.DesignID);
            Border borderCell = (Border)gridCell.Parent;
            int row = Grid.GetRow(borderCell);
            Grid grid = (Grid)borderCell.Parent;
            GroupTypeOrder gto = (GroupTypeOrder)grid.DataContext;
            GroupType gt = gto.Group;
            if (tbi.DesignID.Equals("198") && designID != 0)
            {
                ItemType newItemType = new ItemType();
                newItemType.DesignID = tbi.DesignID;
                newItemType.Header = tbi.Header;
                newItemType.ItemOrder = itToBeMoved.ItemOrder;
                newItemType.DanishTranslationText = tbi.DanishTranslationText;
                newItemType.EnglishTranslationText = tbi.EnglishTranslationText;
                newItemType.LanguageID = tbi.LanguageID;
                newItemType.GroupTypeID = gt.GroupTypeID;
                newItemType.IncludedTypeID = "1";
                int index = gt.Items.IndexOf(itToBeMoved);

                gt.Items.Insert(index, newItemType);
                grid.ClearGrid();
                PopulateGroupTable(gt);
                DisableAllowDropByNewLineItem();

            }
            if (designID != 0 && !tbi.DesignID.Equals("198") && designID != 197)
            {
                ItemType newItemType = new ItemType();
                newItemType.DesignID = tbi.DesignID;
                newItemType.Header = tbi.Header;
                newItemType.ItemOrder = itToBeMoved.ItemOrder;
                newItemType.DanishTranslationText = tbi.DanishTranslationText;
                newItemType.EnglishTranslationText = tbi.EnglishTranslationText;
                newItemType.LanguageID = tbi.LanguageID;
                newItemType.GroupTypeID = gt.GroupTypeID;
                newItemType.IncludedTypeID = "1";

                List<ItemType> itemTypeList = GetItemTypes();
                int startPosition = itemTypeList.IndexOf(itToBeMoved);
                MoveItemsForward(startPosition, itemTypeList, grid, newItemType, gt);


                grid.ClearGrid();
                PopulateGroupTable(gt);
                DisableAllowDropByNewLineItem();
            }
            if (designID == 0)
            {
                itToBeMoved.DesignID = tbi.DesignID;
                itToBeMoved.Header = tbi.Header;
                itToBeMoved.DanishTranslationText = tbi.DanishTranslationText;
                itToBeMoved.EnglishTranslationText = tbi.EnglishTranslationText;
                itToBeMoved.LanguageID = tbi.LanguageID;
                itToBeMoved.GroupTypeID = gt.GroupTypeID;
                itToBeMoved.IncludedTypeID = "1";
                if (row == grid.RowDefinitions.Count - 1)
                {
                    GenerateEmptyFields(row, false);
                    if (grid.RowDefinitions.Count != 2)
                    {
                        GenerateEmptyFields(row - 1, true);
                    }
                    AddNewEmptyItemRow();
                }
                if (row + 1 == grid.RowDefinitions.Count - 1)
                {
                    GenerateEmptyFields(row, false);
                    if (grid.RowDefinitions.Count != 3)
                    {
                        GenerateEmptyFields(row - 1, true);
                    }
                }

                gt.Items.Add(itToBeMoved);
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
                itToBeMoved.Header = tbi.Header;
                itToBeMoved.DanishTranslationText = tbi.DanishTranslationText;
                itToBeMoved.EnglishTranslationText = tbi.EnglishTranslationText;
                itToBeMoved.LanguageID = tbi.LanguageID;
                itToBeMoved.GroupTypeID = gt.GroupTypeID;
                itToBeMoved.IncludedTypeID = "1";
            }
        }

        void HandleItemTypeDrop(object sender, DragEventArgs e)
        {
            //dragged item
            Border target = null;
            Grid gridCell = null;
            TextBlock tb = null;
            ItemType targetItemType = null;
            Image img = null;
            Button btn = null;
            if (e.Source is Border)
            {
                target = e.Source as Border;
                gridCell = (Grid)target.Child;
                tb = (TextBlock)gridCell.Children[1];
                targetItemType = (ItemType)tb.DataContext;
            }
            if (e.Source is TextBlock)
            {
                tb = e.Source as TextBlock;
                gridCell = (Grid)tb.Parent;
                target = (Border)gridCell.Parent;
                targetItemType = (ItemType)tb.DataContext;
            }
            if (e.Source is Image)
            {
                img = e.Source as Image;
                btn = (Button)img.Parent;
                gridCell = (Grid)btn.Parent;
                target = (Border)gridCell.Parent;
                tb = (TextBlock)gridCell.Children[1];
                targetItemType = (ItemType)tb.DataContext;
            }
            if (e.Source is Button)
            {
                btn = e.Source as Button;
                gridCell = (Grid)btn.Parent;
                target = (Border)gridCell.Parent;
                tb = (TextBlock)gridCell.Children[1];
                targetItemType = (ItemType)tb.DataContext;

            }
            Border target2 = e.Data.GetData("System.Windows.Controls.Border") as Border;
            Grid gridCell2 = (Grid)target2.Child;
            TextBlock tb2 = (TextBlock)gridCell2.Children[1];
            ItemType draggedItemType = (ItemType)tb2.DataContext;
            Grid groupTable2 = (Grid)target2.Parent;
            Grid groupTable = (Grid)target.Parent;

            GroupType gt2 = GetGroupType(groupTable2);
            GroupType gt = GetGroupType(groupTable);

            double targetItemTypeNo = targetItemType.ItemOrder; //affected item
            double draggedItemTypeNo = draggedItemType.ItemOrder; //dragged item

            gt.Items.Remove(draggedItemType);
            int position = gt.Items.IndexOf(targetItemType);
            if (targetItemType != draggedItemType)
            {
                if (targetItemType.DesignID == null)
                {
                    int i = 0;
                    while (i < gt.Items.Count)
                    {
                        gt.Items[i].ItemOrder--;
                        i++;
                    }
                    draggedItemType.ItemOrder = targetItemType.ItemOrder;
                    int row = Grid.GetRow(target);
                    int column = Grid.GetColumn(target);
                    int rowCount = groupTable.RowDefinitions.Count;
                    int lastRow = groupTable.RowDefinitions.Count - 1;
                    groupTable.ClearGrid();
                    PopulateGroupTable(gt);
                    Border newTarget = null;
                    if (rowCount == groupTable.RowDefinitions.Count + 1)
                    {
                        AddNewEmptyItemRow();
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
                    Grid newGridCell = (Grid)newTarget.Child;
                    TextBlock newtb = (TextBlock)newGridCell.Children[1];
                    newtb.DataContext = draggedItemType;
                    i = 1;
                    while (i < groupTable.RowDefinitions.Count - 2)
                    {

                        if (!CheckIfRowIsEmpty(i))
                        {
                            GenerateEmptyFields(i, true);
                        }
                        i++;
                    }
                    if (!(row == groupTable.RowDefinitions.Count - 2))
                    {
                        GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, true);
                    }
                    else
                    {
                        GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, false);
                    }
                    GenerateEmptyFields(groupTable.RowDefinitions.Count - 1, false);
                    gt.Items.Add(draggedItemType);
                    List<ItemType> itemTypeList = gt.Items.ToList();
                    itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
                    ObservableCollection<ItemType> ocItemTypeList = new ObservableCollection<ItemType>(itemTypeList);
                    gt.Items = ocItemTypeList;

                    groupTable.ClearGrid();
                    PopulateGroupTable(gt);
                    DisableAllowDropByNewLineItem();
                }
                if (draggedItemType.DesignID.Equals("198"))
                {
                    ItemType newit = new ItemType();
                    newit.ItemOrder = draggedItemType.ItemOrder;
                    tb2.DataContext = newit;
                    tb.DataContext = draggedItemType;
                    gt.Items.Remove(draggedItemType);
                    if (position != -1)
                    {
                        gt.Items.Insert(position, draggedItemType);
                        gt.Items[position].ItemOrder = targetItemTypeNo;
                    }
                    if (targetItemType.DesignID == null)
                    {
                        gt.Items.Add(draggedItemType);
                    }
                    draggedItemType.ItemOrder = targetItemTypeNo;
                    int i = 1;
                    while (i < groupTable.RowDefinitions.Count - 2)
                    {

                        if (!CheckIfRowIsEmpty(i))
                        {
                            GenerateEmptyFields(i, true);
                        }
                        i++;
                    }
                    GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, false);
                    GenerateEmptyFields(groupTable.RowDefinitions.Count - 1, false);
                    groupTable.ClearGrid();
                    PopulateGroupTable(gt);
                    DisableAllowDropByNewLineItem();
                }

                if (targetItemType.DesignID != null && draggedItemType.DesignID != null && !draggedItemType.DesignID.Equals("198"))
                {
                    if (draggedItemType.ItemOrder > targetItemType.ItemOrder)
                    {
                        gt.Items.Insert(position, draggedItemType);
                        draggedItemType.ItemOrder = targetItemTypeNo;
                    }
                    else
                    {

                        gt.Items[position].ItemOrder--;
                        draggedItemType.ItemOrder = targetItemTypeNo;
                        gt.Items.Insert(position, draggedItemType);

                    }

                    int i = 1;
                    while (i < groupTable.RowDefinitions.Count - 2)
                    {

                        if (!CheckIfRowIsEmpty(i))
                        {
                            GenerateEmptyFields(i, true);
                        }
                        i++;
                    }
                    GenerateEmptyFields(groupTable.RowDefinitions.Count - 2, false);
                    GenerateEmptyFields(groupTable.RowDefinitions.Count - 1, false);
                    List<ItemType> itemTypeList = gt.Items.ToList();
                    itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
                    ObservableCollection<ItemType> ocItemTypeList = new ObservableCollection<ItemType>(itemTypeList);
                    gt.Items = ocItemTypeList;

                    groupTable.ClearGrid();
                    PopulateGroupTable(gt);
                    DisableAllowDropByNewLineItem();
                }
            }
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
                List<ItemType> itemTypeList = GetItemsByRow(row);
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

        void MoveItemsForward(int startPosition, List<ItemType> itemTypeList, Grid grid, ItemType newItemType, GroupType gt)
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
            ObservableCollection<ItemType> ocItemTypeList = new ObservableCollection<ItemType>();

            //add list to observablecollection
            foreach (ItemType it in itemTypeList)
            {
                ocItemTypeList.Add(it);
            }
            gt.Items = ocItemTypeList;
            grid.ClearGrid();
            PopulateGroupTable(gt);
        }

        void border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;

            Grid cellItem = (Grid)border.Child;
            TextBlock tb = (TextBlock)cellItem.Children[1];
            if (tb.DataContext is ItemType)
            {
                ItemType itemType = (ItemType)tb.DataContext;
                if (itemType.Header != null)
                {
                    Button btnClearCell = (Button)cellItem.Children[0];
                    btnClearCell.Visibility = Visibility.Visible;
                }
            }
            if (tb.DataContext is GroupType)
            {
                Button btnClearCell = (Button)cellItem.Children[0];
                btnClearCell.Visibility = Visibility.Visible;
            }
        }

        void border_MouseLeave(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            Grid cellItem = (Grid)border.Child;
            Button btnClearCell = (Button)cellItem.Children[0];
            btnClearCell.Visibility = Visibility.Hidden;
        }

        public Border CreateBorderContainer(Color borderBrush, Color background)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(borderBrush),
                Background = new SolidColorBrush(background),
                BorderThickness = new Thickness(1),
                Height = 27.0,
                MaxHeight = 27.0,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            border.MouseMove += border_MouseMove;
            border.GiveFeedback += border_GiveFeedback;
            return border;
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
            Point current = e.GetPosition(this);
            if (sender is Border && e.LeftButton == MouseButtonState.Pressed)
            {

                Border draggedItem = sender as Border;
                Grid draggedItemGridCell = (Grid)draggedItem.Child;
                TextBlock draggedItemTb = (TextBlock)draggedItemGridCell.Children[1];
                if (draggedItemTb.DataContext is ItemType) //Prevent dragging if GroupType
                {
                    Grid groupTable = (Grid)draggedItem.Parent;
                    Grid gridCell = (Grid)draggedItem.Child;
                    TextBlock tb = (TextBlock)gridCell.Children[1];
                    ItemType it = (ItemType)tb.DataContext;
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
                if (draggedItemTb.DataContext is GroupType)
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
            BitmapImage logo = new BitmapImage();
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
