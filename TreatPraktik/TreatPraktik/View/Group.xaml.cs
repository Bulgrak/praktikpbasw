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

        /// <summary>
        /// Checks if a particular group appears multiple times and retrieves the department
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public List<string> GetDepartments(int initialPosition, GroupTypeOrder gto)
        {
            List<string> departmentList = new List<string>();
            int j = initialPosition;
            while (j < Groups.Count)
            {
                if (gto.GroupTypeID.Equals(Groups[j].GroupTypeID))
                {
                    departmentList.Add(Groups[j].DepartmentID);
                }
                j++;
            }

            return departmentList;
        }

        public bool IsGroupsOccuringMultipleTimes(ObservableCollection<GroupTypeOrder> groups, GroupType gt)
        {
            int count = groups.Count(item => item.GroupTypeID == gt.GroupTypeID);
            if (count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetIndexLastOccurrence(GroupTypeOrder gto, int initialPosition)
        {
            int i = initialPosition + 1;
            bool stop = false;
            while (i < Groups.Count && !stop)
            {
                if (!Groups[i].GroupTypeID.Equals(gto.GroupTypeID))
                    stop = true;
                else
                    i++;
            }
            return i;
        }

        public void CreateGroupTables()
        {
            int skipped = 0;
            for (int i = 0; i < Groups.Count; i++)
            {
                GroupType gt = Groups[i].Group;
                Grid gridGroup = new Grid { DataContext = Groups[i] };

                TextBlock tbGroupNumber = new TextBlock
                {
                    DataContext = Groups[i],
                    FontSize = 18,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.LightSlateGray
                };
                tbGroupNumber.SetBinding(TextBlock.TextProperty, "GroupOrder");
                TextBlock tbDepartNumber = new TextBlock
                {
                    DataContext = Groups[i],
                    FontSize = 12,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.DarkSlateGray
                };
                if (IsGroupsOccuringMultipleTimes(Groups, gt))
                {
                    int initialValue = i;
                    i = GetIndexLastOccurrence(Groups[i], i);
                    skipped = skipped + (i - initialValue);
                    PopulateGroupTable(gt, gridGroup);
                    List<string> deptNumbers = GetDepartments(initialValue, Groups[initialValue]);

                    tbDepartNumber.Text = "Dept: " + String.Join(",", deptNumbers);
                }
                else
                {
                    PopulateGroupTable(gt, gridGroup);
                    tbDepartNumber.Text = "Dept: " + Groups[i].DepartmentID;
                }

                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(gridGroup, i - skipped);
                Grid.SetColumn(gridGroup, 1);
                myGrid.Children.Add(gridGroup);
                StackPanel spInformation = new StackPanel
                {
                    Name = "spInformation",
                    Orientation = Orientation.Vertical
                };

                spInformation.Children.Add(tbGroupNumber);
                spInformation.Children.Add(tbDepartNumber);
                Grid.SetRow(spInformation, i - skipped);
                Grid.SetColumn(spInformation, 0);
                myGrid.Children.Add(spInformation);

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
                Grid.SetRow(sp, i - skipped);
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
            StackPanel spDescription = (StackPanel) uieListMoveUp[0];
            TextBlock tb = (TextBlock)spDescription.Children[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            gt.GroupOrder--;
            StackPanel spBtn = (StackPanel)uieListMoveUp[1];
            Button btnUp = (Button)spBtn.Children[0];
            Button btnDown = (Button)spBtn.Children[1];
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
            StackPanel spDescription = (StackPanel) uieListMoveDown[0];
            TextBlock tb = (TextBlock)spDescription.Children[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            gt.GroupOrder++;

            StackPanel spBtn = (StackPanel)uieListMoveDown[1];
            Button btnDown = (Button)spBtn.Children[1];
            Button btnUp = (Button)spBtn.Children[0];
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

        void MoveGroupToRow(List<UIElement> uieList, int row)
        {
            foreach (UIElement uie in uieList)
            {
                Grid.SetRow(uie, row);
            }
            StackPanel spDescription = (StackPanel)uieList[0];
            TextBlock tb = (TextBlock)spDescription.Children[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            gt.GroupOrder = row + 1;
        }

        public void PopulateGroupTable(GroupType gt, Grid groupTable)
        {
            CreateColumns(groupTable, 4);
            int counterRow = 0;
            int counterColumn = 0;
            AddNewGroupRow(groupTable);
            InsertGroupItem(groupTable, gt, 0, 0, false);
            if (!gt.GroupTypeID.Equals("1") && !gt.GroupTypeID.Equals("11")) //Special groups, which shouldn't show any items
            {
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
                            //gt.Items[j + skipped].ItemOrder = j + skipped;
                            SolidColorBrush textColor2 = Brushes.Black;
                            InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor2);
                            j--;
                            skipped++;

                            continue;
                        }
                        gt.Items[j + skipped].Header = "<NewLineItem>";
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
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
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
                    }
                    else
                    {
                        if (counterColumn >= 4)
                        {
                            counterColumn = 0;
                        }
                        SolidColorBrush textColor = Brushes.Black;
                        InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor);
                        //gt.Items[j + skipped].ItemOrder = j + skipped;
                    }
                    counterColumn++;
                }
                AddNewEmptyItemRow(groupTable);
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
            ItemType dummyItemType = (ItemType)tb.DataContext;
            itemType.ItemOrder = dummyItemType.ItemOrder;
            tb.DataContext = itemType;
            tb.SetBinding(TextBlock.TextProperty, "Header");
            tb.Foreground = textColor;
        }

        private void InsertGroupItem(Grid groupTable, GroupType groupType, int row, int column, bool allowDrop)
        {
            Border border = GetCellItem(groupTable, row, column);
            Grid gridGroupCell = (Grid) border.Child;
            TextBlock tb = (TextBlock)gridGroupCell.Children[1];
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

        private bool CheckIfRowIsEmpty(Grid groupTable, int row)
        {
            List<ItemType> itemTypeList = GetItemsByRow(groupTable, row);
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
            if (e.Data.GetData("System.Windows.Controls.Border") is Border) // Drag and drop of items
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
                Grid draggedGroupTable = (Grid) draggedBorderCell.Parent;

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
                        bool containsRow = CheckForNewLineItem(groupTable, row);
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

            if (e.Data.GetData("System.Windows.Controls.Grid") is Grid) // Drag and drop of groups
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
                if(tb.DataContext is GroupType)
                {
                    e.Effects = DragDropEffects.Move;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
        }

        void border_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem) //Drag and drop toolboxitem
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
                    PopulateGroupTable(gt, grid);
                    DisableAllowDropByNewLineItem(grid);

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

                    List<ItemType> itemTypeList = GetItemTypes(grid);
                    int startPosition = itemTypeList.IndexOf(itToBeMoved);
                    MoveItemsForward(startPosition, itemTypeList, grid, newItemType, gt);


                    grid.ClearGrid();
                    PopulateGroupTable(gt, grid);
                    DisableAllowDropByNewLineItem(grid);
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

                    gt.Items.Add(itToBeMoved);
                    grid.ClearGrid();
                    PopulateGroupTable(gt, grid);
                    if (tbi.DesignID.Equals("198"))
                    {
                        DisableAllowDropByNewLineItem(grid);
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
            if (e.Data.GetData("System.Windows.Controls.Border") is Border) //Drag and drop between items
            {
                //dragged item
                Border target = null;
                Grid gridCell = null;
                TextBlock tb = null;
                ItemType it = null;
                Image img = null;
                Button btn = null;
                if (e.Source is Border)
                {
                    target = e.Source as Border;
                    gridCell = (Grid)target.Child;
                    tb = (TextBlock)gridCell.Children[1];
                    it = (ItemType)tb.DataContext;
                }
                if (e.Source is TextBlock)
                {
                    tb = e.Source as TextBlock;
                    gridCell = (Grid)tb.Parent;
                    target = (Border)gridCell.Parent;
                    it = (ItemType)tb.DataContext;
                }
                if (e.Source is Image)
                {
                    img = e.Source as Image;
                    btn = (Button)img.Parent;
                    gridCell = (Grid)btn.Parent;
                    target = (Border)gridCell.Parent;
                    tb = (TextBlock)gridCell.Children[1];
                    it = (ItemType)tb.DataContext;
                }
                if (e.Source is Button)
                {
                    btn = e.Source as Button;
                    gridCell = (Grid)btn.Parent;
                    target = (Border)gridCell.Parent;
                    tb = (TextBlock)gridCell.Children[1];
                    it = (ItemType)tb.DataContext;

                }
                Border target2 = e.Data.GetData("System.Windows.Controls.Border") as Border;
                Grid gridCell2 = (Grid)target2.Child;
                TextBlock tb2 = (TextBlock)gridCell2.Children[1];
                ItemType it2 = (ItemType)tb2.DataContext;
                Grid groupTable2 = (Grid)target2.Parent;
                Grid groupTable = (Grid)target.Parent;

                GroupType gt2 = GetGroupType(groupTable2);
                GroupType gt = GetGroupType(groupTable);

                double draggedItemTypeNo = it.ItemOrder; //affected item
                double itemTypeSwitch = it2.ItemOrder; //dragged item

                gt.Items.Remove(it2);
                int position = gt.Items.IndexOf(it);
                if (it != it2)
                {
                    if (it.DesignID == null)
                    {
                        int i = 0;
                        while (i < gt.Items.Count)
                        {
                            gt.Items[i].ItemOrder--;
                            i++;
                        }
                        it2.ItemOrder = it.ItemOrder;
                        int row = Grid.GetRow(target);
                        int column = Grid.GetColumn(target);
                        int rowCount = groupTable.RowDefinitions.Count;
                        int lastRow = groupTable.RowDefinitions.Count - 1;
                        groupTable.ClearGrid();
                        PopulateGroupTable(gt, groupTable);
                        Border newTarget = null;
                        if (rowCount == groupTable.RowDefinitions.Count + 1)
                        {
                            AddNewEmptyItemRow(groupTable);
                            if (row == lastRow)
                            {
                                newTarget =
                                    (Border)GetCellItem(groupTable, groupTable.RowDefinitions.Count - 1, column);
                            }
                            else
                            {
                                newTarget =
                                    (Border)GetCellItem(groupTable, groupTable.RowDefinitions.Count - 2, column);
                            }
                        }
                        else
                        {
                            newTarget = (Border)GetCellItem(groupTable, row, column);
                        }
                        Grid newGridCell = (Grid)newTarget.Child;
                        TextBlock newtb = (TextBlock)newGridCell.Children[1];
                        newtb.DataContext = it2;
                        i = 1;
                        while (i < groupTable.RowDefinitions.Count - 2)
                        {

                            if (!CheckIfRowIsEmpty(groupTable, i))
                            {
                                GenerateEmptyFields(groupTable, i, true);
                            }
                            i++;
                        }
                        if (!(row == groupTable.RowDefinitions.Count - 2))
                        {
                            GenerateEmptyFields(groupTable, groupTable.RowDefinitions.Count - 2, true);
                        }
                        else
                        {
                            GenerateEmptyFields(groupTable, groupTable.RowDefinitions.Count - 2, false);
                        }
                        GenerateEmptyFields(groupTable, groupTable.RowDefinitions.Count - 1, false);
                        gt.Items.Add(it2);
                        List<ItemType> itemTypeList = gt.Items.ToList();
                        itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
                        ObservableCollection<ItemType> ocItemTypeList = new ObservableCollection<ItemType>(itemTypeList);
                        gt.Items = ocItemTypeList;

                        groupTable.ClearGrid();
                        PopulateGroupTable(gt, groupTable);
                        DisableAllowDropByNewLineItem(groupTable);
                    }
                    if (it2.DesignID.Equals("198"))
                    {
                        ItemType newit = new ItemType();
                        newit.ItemOrder = it2.ItemOrder;
                        tb2.DataContext = newit;
                        tb.DataContext = it2;
                        gt.Items.Remove(it2);
                        if (position != -1)
                        {
                            gt.Items.Insert(position, it2);
                            gt.Items[position].ItemOrder = draggedItemTypeNo;
                        }
                        if (it.DesignID == null)
                        {
                            gt.Items.Add(it2);
                        }
                        it2.ItemOrder = draggedItemTypeNo;
                        int i = 1;
                        while (i < groupTable.RowDefinitions.Count - 2)
                        {

                            if (!CheckIfRowIsEmpty(groupTable, i))
                            {
                                GenerateEmptyFields(groupTable, i, true);
                            }
                            i++;
                        }
                        GenerateEmptyFields(groupTable, groupTable.RowDefinitions.Count - 2, false);
                        GenerateEmptyFields(groupTable, groupTable.RowDefinitions.Count - 1, false);
                        groupTable.ClearGrid();
                        PopulateGroupTable(gt, groupTable);
                        DisableAllowDropByNewLineItem(groupTable);
                    }

                    if (it.DesignID != null && it2.DesignID != null && !it2.DesignID.Equals("198"))
                    {
                        if (it2.ItemOrder > it.ItemOrder)
                        {
                            gt.Items.Insert(position, it2);
                            it2.ItemOrder = draggedItemTypeNo;
                        }
                        else
                        {

                            gt.Items[position].ItemOrder--;
                            it2.ItemOrder = draggedItemTypeNo;
                            gt.Items.Insert(position, it2);

                        }

                        int i = 1;
                        while (i < groupTable.RowDefinitions.Count - 2)
                        {

                            if (!CheckIfRowIsEmpty(groupTable, i))
                            {
                                GenerateEmptyFields(groupTable, i, true);
                            }
                            i++;
                        }
                        GenerateEmptyFields(groupTable, groupTable.RowDefinitions.Count - 2, false);
                        GenerateEmptyFields(groupTable, groupTable.RowDefinitions.Count - 1, false);
                        List<ItemType> itemTypeList = gt.Items.ToList();
                        itemTypeList = itemTypeList.OrderBy(o => o.ItemOrder).ToList();
                        ObservableCollection<ItemType> ocItemTypeList = new ObservableCollection<ItemType>(itemTypeList);
                        gt.Items = ocItemTypeList;

                        groupTable.ClearGrid();
                        PopulateGroupTable(gt, groupTable);
                        DisableAllowDropByNewLineItem(groupTable);
                    }
                }
            }
            if (e.Data.GetData("System.Windows.Controls.Grid") is Grid) //Drag and drop groups 
            {
                //dragged item
                Border target = null;
                Grid gridCell = null;
                TextBlock tb = null;
                ItemType it = null;
                Image img = null;
                Button btn = null;
                if (e.Source is Border)
                {
                    target = e.Source as Border;
                    gridCell = (Grid)target.Child;
                    tb = (TextBlock)gridCell.Children[1];
                    //it = (ItemType)tb.DataContext;
                }
                if (e.Source is TextBlock)
                {
                    tb = e.Source as TextBlock;
                    gridCell = (Grid)tb.Parent;
                    target = (Border)gridCell.Parent;
                    //it = (ItemType)tb.DataContext;
                }
                if (e.Source is Image)
                {
                    img = e.Source as Image;
                    btn = (Button)img.Parent;
                    gridCell = (Grid)btn.Parent;
                    target = (Border)gridCell.Parent;
                    tb = (TextBlock)gridCell.Children[1];
                    //it = (ItemType)tb.DataContext;
                }
                if (e.Source is Button)
                {
                    btn = e.Source as Button;
                    gridCell = (Grid)btn.Parent;
                    target = (Border)gridCell.Parent;
                    tb = (TextBlock)gridCell.Children[1];
                    //it = (ItemType)tb.DataContext;

                }
                Grid targetGroupTable = (Grid) target.Parent;
                int targetRow = Grid.GetRow(targetGroupTable);


                Grid draggedGroupTable = e.Data.GetData("System.Windows.Controls.Grid") as Grid;
                int draggedRow = Grid.GetRow(draggedGroupTable);
                if (targetRow < draggedRow)
                {
                    List<UIElement> uieListToBeMoved = myGrid.GetAllGridCellChildrenListByRow(draggedRow);
                    //int i = targetRow;
                    //while (i < draggedRow)
                    int i = draggedRow;
                    while (i >= targetRow)
                    {
                        List<UIElement> uieList = myGrid.GetAllGridCellChildrenListByRow(i);
                        MoveGroupDown(uieList, i);
                        i--;
                    }
                    MoveGroupToRow(uieListToBeMoved, targetRow);
                }
                else
                {
                    List<UIElement> uieListToBeMoved = myGrid.GetAllGridCellChildrenListByRow(draggedRow);
                    //int i = targetRow;
                    //while (i < draggedRow)
                    int i = draggedRow + 1;
                    while (i <= targetRow)
                    {
                        List<UIElement> uieList = myGrid.GetAllGridCellChildrenListByRow(i);
                        MoveGroupUp(uieList, i - 1);
                        i++;
                    }
                    MoveGroupToRow(uieListToBeMoved, targetRow);
                }

                int test = 0;
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

        void DisableAllowDropNewLine(int startColumnPosition, int row, Grid groupTable)
        {
            int i = startColumnPosition;
            while (i < groupTable.ColumnDefinitions.Count)
            {
                Border borderCell = (Border)groupTable.GetCellChild(row, i);
                borderCell.AllowDrop = false;
                i++;
            }
        }

        void EnableAllowDropNewLine(int startColumnPosition, int row, Grid groupTable)
        {
            int i = startColumnPosition;
            while (i < groupTable.ColumnDefinitions.Count)
            {
                Border borderCell = (Border)groupTable.GetCellChild(row, i);
                borderCell.AllowDrop = true;
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
            if (tb.DataContext is ItemType)
            {
                ItemType itemType = (ItemType) tb.DataContext;
                if (itemType.Header != null)
                {
                    Button btnClearCell = (Button) cellItem.Children[0];
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
                Grid draggedItemGridCell = (Grid) draggedItem.Child;
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
                            EnableAllowDropNewLine(0, row, groupTable);
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

        public Button CreateClearGroupBtn()
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
            //btnClearCell.Click += btnRemove_Click;

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
