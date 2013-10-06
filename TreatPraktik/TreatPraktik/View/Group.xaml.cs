using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : UserControl
    {
        public ObservableCollection<GroupTypeOrder> Groups { get; set; }

        public Group()
        {
            InitializeComponent();
        }

        public void CreateGroupTables()
        {
            for (int i = 0; i < Groups.Count; i++)
            {
                GroupType gt = Groups[i].Group;
                Grid gridGroup = new Grid { DataContext = Groups[i] };

                PopulateGroupTable(gt, gridGroup);

                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(gridGroup, i);
                Grid.SetColumn(gridGroup, 1);
                myGrid.Children.Add(gridGroup);
                TextBlock tbGroupNumber = new TextBlock
                {
                    DataContext = Groups[i],
                    FontSize = 18,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.LightSlateGray
                };
                tbGroupNumber.SetBinding(TextBlock.TextProperty, "GroupOrder");
                Grid.SetRow(tbGroupNumber, i);
                Grid.SetColumn(tbGroupNumber, 0);
                myGrid.Children.Add(tbGroupNumber);

                var uriSourceGroupMoveUpIcon = new Uri(@"/TreatPraktik;component/Ressources/Arrow-up.ico", UriKind.Relative);
                Image imgBtnGroupMoveUpIcon = new Image { Source = new BitmapImage(uriSourceGroupMoveUpIcon) };
                Button btnGroupMoveUp = new Button
                {
                    Name = "btnGroupMoveUp",
                    Height = 17,
                    Width = 17,
                    Content = imgBtnGroupMoveUpIcon,
                    FontSize = 9,
                    DataContext = gridGroup
                };

                btnGroupMoveUp.Click += btnGroupMoveUp_Click;

                if (i == 0)
                {
                    btnGroupMoveUp.IsEnabled = false;
                }

                var uriSourceGroupMoveDownIcon = new Uri(@"/TreatPraktik;component/Ressources/Arrow-down.ico", UriKind.Relative);
                Image imgBtnGroupMoveDownIcon = new Image { Source = new BitmapImage(uriSourceGroupMoveDownIcon) };

                Button btnGroupMoveDown = new Button
                {
                    Name = "btnGroupMoveDown",
                    Height = 17,
                    Width = 17,
                    Content = imgBtnGroupMoveDownIcon,
                    DataContext = gridGroup,
                    FontSize = 9
                };
                btnGroupMoveDown.Click += btnGroupMoveDown_Click;

                if (i == Groups.Count - 1)
                {
                    btnGroupMoveDown.IsEnabled = false;
                }
                StackPanel sp = new StackPanel
                {
                    Name = "spBtnUpDown",
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                sp.Children.Add(btnGroupMoveUp);
                sp.Children.Add(btnGroupMoveDown);
                Grid.SetRow(sp, i);
                Grid.SetColumn(sp, 0);
                myGrid.Children.Add(sp);

            }
        }

        void btnGroupMoveDown_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            StackPanel sp = (StackPanel)btn.Parent;
            Grid gridGroup = (Grid)btn.DataContext;
            int row = Grid.GetRow(sp);
            List<UIElement> uieListMoveDown = myGrid.GetAllGridCellChildrenListByRow(row);
            List<UIElement> uieListMoveUp = myGrid.GetAllGridCellChildrenListByRow(row + 1);
            MoveGroupUp(uieListMoveUp, row);
            MoveGroupDown(uieListMoveDown, row);
        }

        void MoveGroupUp(IList<UIElement> uieListMoveUp, int row)
        {
            foreach (UIElement uie in uieListMoveUp)
            {
                Grid.SetRow(uie, row);
            }
            TextBlock tb = (TextBlock)uieListMoveUp[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            gt.GroupOrder--;
            StackPanel sp = (StackPanel)uieListMoveUp[1];
            Button btnUp = (Button)sp.Children[0];
            Button btnDown = (Button)sp.Children[1];
            if (gt.GroupOrder == 1)
            {

                btnUp.IsEnabled = false;
            }
            else
            {
                btnUp.IsEnabled = true;
            }
            btnDown.IsEnabled = true;
        }



        void MoveGroupDown(IList<UIElement> uieListMoveDown, int row)
        {
            foreach (UIElement uie in uieListMoveDown)
            {
                Grid.SetRow(uie, row + 1);
            }
            TextBlock tb = (TextBlock)uieListMoveDown[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            gt.GroupOrder++;

            StackPanel sp = (StackPanel)uieListMoveDown[1];
            Button btnDown = (Button)sp.Children[1];
            Button btnUp = (Button)sp.Children[0];
            if (gt.GroupOrder == Groups.Count)
            {
                btnDown.IsEnabled = false;
            }
            else
            {
                btnDown.IsEnabled = true;
            }
            btnUp.IsEnabled = true;
        }

        void btnGroupMoveUp_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            StackPanel sp = (StackPanel)btn.Parent;
            int row = Grid.GetRow(sp);
            List<UIElement> uieListMoveDown = myGrid.GetAllGridCellChildrenListByRow(row - 1);
            List<UIElement> uieListMoveUp = myGrid.GetAllGridCellChildrenListByRow(row);
            MoveGroupUp(uieListMoveUp, row - 1);
            MoveGroupDown(uieListMoveDown, row - 1);
        }

        public void PopulateGroupTable(GroupType gt, Grid groupTable)
        {
            CreateColumns(groupTable, 4);
            int counterRow = 0;
            int counterColumn = 0;
            AddNewGroupRow(groupTable);
            InsertGroupItem(groupTable, gt, 0, 0, false);
            int skipped = 0;
            for (int j = 0; j < gt.Items.Count - skipped; j++)
            {
                if (gt.Items[j + skipped].DesignID.Equals("198"))
                {
                    if (j % 4 == 0)
                    {
                        AddNewEmptyItemRow(groupTable);
                        counterRow++;
                        gt.Items[j + skipped].Header = "<NewLineItem>";
                        gt.Items[j + skipped].ItemOrder = j + skipped;
                        SolidColorBrush textColor2 = Brushes.Black;
                        InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor2);
                        j--;
                        skipped++;

                        continue;
                    }
                    gt.Items[j + skipped].Header = "<NewLineItem>";
                    gt.Items[j + skipped].ItemOrder = j + skipped;
                    SolidColorBrush textColor = Brushes.Black;
                    InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor);
                    skipped = skipped + j;
                    j = 0;
                }
                else if (j % 4 == 0)
                {
                    AddNewEmptyItemRow(groupTable);
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
                    InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor);
                    gt.Items[j + skipped].ItemOrder = j + skipped;
                }
                else
                {
                    if (counterColumn >= 4)
                    {
                        counterColumn = 0;
                    }
                    SolidColorBrush textColor = Brushes.Black;
                    InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor);
                    gt.Items[j + skipped].ItemOrder = j + skipped;
                }
                counterColumn++;
            }
            AddNewEmptyItemRow(groupTable);
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
            TextBlock tb = (TextBlock)b.Child;
            GroupType gt = (GroupType)tb.DataContext;
            return gt;
        }

        public List<ItemType> GetItemTypes(Grid groupTable)
        {
            List<ItemType> itemTypeList = new List<ItemType>();
            int i = 1;
            while (i < groupTable.RowDefinitions.Count)
            {
                int j = 0;
                while (j < groupTable.ColumnDefinitions.Count)
                {
                    Border b = (Border)groupTable.GetCellChild(i, j);
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

        private void InsertItem(Grid groupTable, ItemType itemType, int row, int column, bool allowDrop, SolidColorBrush textColor)
        {
            Border border = GetCellItem(groupTable, row, column);
            Grid cellItem = (Grid)border.Child;
            TextBlock tb = (TextBlock)cellItem.Children[1];
            tb.DataContext = itemType;
            tb.SetBinding(TextBlock.TextProperty, "Header");
            tb.Foreground = textColor;
        }

        private void InsertGroupItem(Grid groupTable, GroupType groupType, int row, int column, bool allowDrop)
        {
            Border border = GetCellItem(groupTable, row, column);
            TextBlock tb = (TextBlock)border.Child;
            tb.DataContext = groupType;
            tb.SetBinding(TextBlock.TextProperty, "GroupHeader");
        }

        public Border GetCellItem(Grid groupTable, int row, int column)
        {
            Border cellItem = (Border)groupTable.Children
      .Cast<UIElement>()
      .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            return cellItem;
        }

        private void AddNewEmptyItemRow(Grid groupTable)
        {
            RowDefinition rd = new RowDefinition();
            groupTable.RowDefinitions.Add(rd);
            int row = groupTable.RowDefinitions.Count - 1;
            int i = 0;
            while (i < groupTable.ColumnDefinitions.Count)
            {
                ItemType itemType = new ItemType();
                itemType.ItemOrder = ((row - 1) * 4) + i;
                Border border = CreateItemCell(Colors.Black, Colors.Yellow, itemType);
                Grid.SetRow(border, row);
                Grid.SetColumn(border, i);
                groupTable.Children.Add(border);
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
            int row = Grid.GetRow(border);
            Grid groupTable = (Grid)border.Parent;
            GroupTypeOrder gto = (GroupTypeOrder)gridGroupTable.DataContext;
            GroupType gt = gto.Group;
            TextBlock tb = (TextBlock)gridCell.Children[1];

            ItemType itToBeDeleted = (ItemType)tb.DataContext;
            gt.Items.Remove(itToBeDeleted);



            int i = Convert.ToInt32(itToBeDeleted.ItemOrder);
            ItemType itemType = new ItemType();
            tb.DataContext = itemType;


            List<ItemType> itemTypeList = GetItemTypes(groupTable);
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
            PopulateGroupTable(gt, gridGroupTable);

            int n = 1;
            while (n < groupTable.RowDefinitions.Count - 2)
            {
                n++;
            }
            if (itToBeDeleted.DesignID.Equals("198"))
            {
                if (row == groupTable.RowDefinitions.Count - 2)
                    ClearNewRowItems(groupTable, groupTable.RowDefinitions.Count - 2);
            }
            DisableAllowDropByNewLineItem(groupTable);
        }

        private void ClearNewRowItems(Grid groupTable, int row)
        {

            List<ItemType> itemTypeListCheck = GetItemsByRow(groupTable, row);
            int i = 0;
            while (i < itemTypeListCheck.Count)
            {
                ItemType it = itemTypeListCheck[i];
                if (it.DesignID != null && it.DesignID.Equals("198"))
                {
                    it.DesignID = null;
                    it.Header = null;
                    it.ItemOrder = 0;
                }
                i++;
            }
        }

        private List<ItemType> GetItemsByRow(Grid groupTable, int row)
        {
            List<UIElement> uieList = groupTable.GetGridCellChildrenByRow(row);
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

        private bool CheckForNewLineItem(Grid groupTable, int row)
        {
            List<ItemType> itemTypeListCheck = GetItemsByRow(groupTable, row);
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

        private void GenerateEmptyFields(Grid groupTable, int row, bool wholeRow)
        {
            List<ItemType> itemTypeListCheck = GetItemsByRow(groupTable, row);
            GroupTypeOrder gto = (GroupTypeOrder)groupTable.DataContext;
            GroupType gt = gto.Group;
            bool newLineItemExist = CheckForNewLineItem(groupTable, row);
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


        private void AddNewGroupRow(Grid grid)
        {
            RowDefinition rd = new RowDefinition();
            grid.RowDefinitions.Add(rd);
            int rowNo = grid.RowDefinitions.Count - 1;

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
                    grid.Children.Add(cellItem);
                }
                i++;
            }
        }

        private Border CreateGroupCell(Color borderBrush, Color background)
        {
            Border border = CreateBorderContainer(borderBrush, background);
            TextBlock tb = new TextBlock { FontWeight = FontWeights.Bold, FontSize = 14.0 };
            border.Child = tb;
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
        }

        void border_DragEnter(object sender, DragEventArgs e)
        {
            CheckDroppedItem(sender, e); //prevents cursor from flickering when dropping an item
        }

        void border_DragOver(object sender, DragEventArgs e)
        {
            CheckDroppedItem(sender, e); //prevents cursor from flickering when dropping an item
        }

        void CheckDroppedItem(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem)
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
                ListBoxItem draggedItem = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
                ToolboxItem tbi = (ToolboxItem)lbi.Content;

                ItemType itToBeMoved = (ItemType)tb.DataContext;
                int designID = Convert.ToInt32(itToBeMoved.DesignID);
                Border borderCell = (Border)gridCell.Parent;
                int row = Grid.GetRow(borderCell);
                Grid grid = (Grid)borderCell.Parent;
                bool containsRow = CheckForNewLineItem(grid, row);
                if (tbi.DesignID.Equals("198") && containsRow)
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }
        }

        void border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem)
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

                ItemType itToBeMoved = (ItemType)tb.DataContext;
                int designID = Convert.ToInt32(itToBeMoved.DesignID);
                Border borderCell = (Border)gridCell.Parent;
                int row = Grid.GetRow(borderCell);
                Grid grid = (Grid)borderCell.Parent;
                GroupTypeOrder gto = (GroupTypeOrder)grid.DataContext;
                GroupType gt = gto.Group;
                if (designID != 0)
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

                    List<ItemType> itemTypeList = GetItemTypes(grid);
                    int startPosition = itemTypeList.IndexOf(itToBeMoved);
                    MoveItemsForward(startPosition, itemTypeList, grid, newItemType, gt);
                    DisableAllowDropByNewLineItem(grid);
                }
                else
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
                        GenerateEmptyFields(grid, row, false);
                        if (grid.RowDefinitions.Count != 2)
                        {
                            GenerateEmptyFields(grid, row - 1, true);
                        }
                        AddNewEmptyItemRow(grid);
                    }
                    if (row + 1 == grid.RowDefinitions.Count - 1)
                    {
                        GenerateEmptyFields(grid, row, false);
                        if (grid.RowDefinitions.Count != 3)
                        {
                            GenerateEmptyFields(grid, row - 1, true);
                        }
                    }
                    if (tbi.DesignID.Equals("198"))
                    {
                        DisableAllowDropByNewLineItem(grid);
                    }
                    gt.Items.Add(itToBeMoved);
                }
            }
            if (e.Data.GetData("System.Windows.Controls.Border") is Border) //drag and drop mellem items
            {
                //dragged item
                Border target = null;
                Grid gridCell = null;
                TextBlock tb = null;
                ItemType it = null;
                if (e.Source is Border)
                {
                target = e.Source as Border;
                gridCell = (Grid)target.Child;
                tb = (TextBlock)gridCell.Children[1];
                it = (ItemType)tb.DataContext;
                }
                else
                {
                    tb = e.Source as TextBlock;
                    it = (ItemType)tb.DataContext;
                }

                //item that will switch place with dragged item
 

                    Border target2 = e.Data.GetData("System.Windows.Controls.Border") as Border;
                    Grid gridCell2 = (Grid)target2.Child;
                    TextBlock tb2 = (TextBlock)gridCell2.Children[1];
                    ItemType it2 = (ItemType)tb2.DataContext;

                

                double draggedItemTypeNo = it.ItemOrder;
                double itemTypeSwitch = it2.ItemOrder;

                //Switch items
                it.ItemOrder = itemTypeSwitch;
                it2.ItemOrder = draggedItemTypeNo;

                tb.DataContext = it2;
                tb2.DataContext = it;
            }
        }

        void DisableAllowDrop(int startColumnPosition, int row, Grid groupTable)
        {
            int i = startColumnPosition + 1;
            while (i < groupTable.ColumnDefinitions.Count)
            {
                Border borderCell = (Border)groupTable.GetCellChild(row, i);
                borderCell.AllowDrop = false;
                i++;
            }
        }

        void DisableAllowDropByNewLineItem(Grid groupTable)
        {
            int row = 1;
            while (row < groupTable.RowDefinitions.Count)
            {
                List<ItemType> itemTypeList = GetItemsByRow(groupTable, row);
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

                while (i < groupTable.ColumnDefinitions.Count && found)
                {
                    Border borderCell = (Border)groupTable.GetCellChild(row, i);
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
            PopulateGroupTable(gt, grid);
        }

        void border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;

            Grid cellItem = (Grid)border.Child;
            TextBlock tb = (TextBlock)cellItem.Children[1];
            ItemType itemType = (ItemType)tb.DataContext;
            if (itemType.Header != null)
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
                Border lbl = sender as Border;
                var pos = lbl.PointFromScreen(GetMousePosition());
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
                if (!(draggedItem.Child is TextBlock)) //Prevent dragging if GroupType
                {

                    Grid gridCell = (Grid)draggedItem.Child;
                    TextBlock tb = (TextBlock)gridCell.Children[1];
                    ItemType it = (ItemType)tb.DataContext;
                    if (it.Header != null)
                    {
                        //draggedItem.IsSelected = true;
                        adorner = new DragAdornerItem(draggedItem, e.GetPosition(draggedItem));
                        AdornerLayer.GetAdornerLayer(this).Add(adorner);
                        DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Move);
                        AdornerLayer.GetAdornerLayer(this).Remove(adorner);
                    }
                }
            }
            startPoint = current;
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
        private DragAdornerItem adorner;
    }
}
