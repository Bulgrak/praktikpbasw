using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : UserControl
    {
        public ObservableCollection<GroupTypeOrder> groups { get; set; }

        public Group()
        {
            InitializeComponent();
        }

        public void CreateGroupTables()
        {
            for (int i = 0; i < groups.Count; i++)
            {
                GroupType gt = groups[i].Group;
                Grid gridGroup = new Grid();
                gridGroup.DataContext = groups[i];

                PopulateGroupTable(gt, gridGroup);

                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(gridGroup, i);
                Grid.SetColumn(gridGroup, 1);
                myGrid.Children.Add(gridGroup);
                TextBlock tbGroupNumber = new TextBlock();
                Grid.SetRow(tbGroupNumber, i);
                Grid.SetColumn(tbGroupNumber, 0);
                tbGroupNumber.DataContext = groups[i];
                tbGroupNumber.SetBinding(TextBlock.TextProperty, "GroupOrder");

                //tbGroupNumber.Text = Convert.ToString(gt.GroupOrder);
                tbGroupNumber.FontSize = 18;
                tbGroupNumber.FontWeight = FontWeights.ExtraBold;
                tbGroupNumber.Foreground = Brushes.LightSlateGray;



                myGrid.Children.Add(tbGroupNumber);

                //Create stackpanel
                Button btnGroupMoveUp = new Button();
                btnGroupMoveUp.Name = "btnGroupMoveUp";
                btnGroupMoveUp.Height = 17;
                btnGroupMoveUp.Width = 17;
                var uriSource = new Uri(@"/TreatPraktik;component/Ressources/Arrow-up.ico", UriKind.Relative);
                BitmapImage logo = new BitmapImage();
                Image img = new Image();
                img.Source = new BitmapImage(uriSource);
                //btnGroupMoveUp.Content = "Up";
                btnGroupMoveUp.Content = img;
                btnGroupMoveUp.Click += btnGroupMoveUp_Click;
                btnGroupMoveUp.FontSize = 9;
                btnGroupMoveUp.DataContext = gridGroup; //i = rowNo
                if (i == 0)
                {
                    btnGroupMoveUp.IsEnabled = false;
                }
                Button btnGroupMoveDown = new Button();
                btnGroupMoveDown.Name = "btnGroupMoveDown";
                btnGroupMoveDown.Height = 17;
                btnGroupMoveDown.Width = 17;
                var uriSource2 = new Uri(@"/TreatPraktik;component/Ressources/Arrow-down.ico", UriKind.Relative);
                BitmapImage logo2 = new BitmapImage();
                Image img2 = new Image();
                img2.Source = new BitmapImage(uriSource2);
                //btnGroupMoveDown.Content = "Down";
                btnGroupMoveDown.Content = img2;
                btnGroupMoveDown.Click += btnGroupMoveDown_Click;
                btnGroupMoveDown.FontSize = 9;
                btnGroupMoveDown.DataContext = gridGroup; //i == rowNo
                if (i == groups.Count - 1)
                {
                    btnGroupMoveDown.IsEnabled = false;
                }
                StackPanel sp = new StackPanel();
                sp.Name = "spBtnUpDown";
                Grid.SetRow(sp, i);
                Grid.SetColumn(sp, 0);
                sp.Orientation = Orientation.Vertical;
                sp.HorizontalAlignment = HorizontalAlignment.Left;
                sp.VerticalAlignment = VerticalAlignment.Bottom;
                //sp.Margin = new Thickness(0, 0, 10, 0);
                sp.Children.Add(btnGroupMoveUp);
                sp.Children.Add(btnGroupMoveDown);
                //End create stackpanel

                myGrid.Children.Add(sp);

            }
        }

        void btnGroupMoveDown_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            StackPanel sp = (StackPanel)btn.Parent;
            Grid gridGroup = (Grid)btn.DataContext;
            int row = Grid.GetRow(sp);
            //Grid.SetRow(gridGroup, row + 1);
            List<UIElement> uieListMoveDown = myGrid.GetGridCellChildrenListByRow(row);
            List<UIElement> uieListMoveUp = myGrid.GetGridCellChildrenListByRow(row + 1);
            MoveGroupUp(uieListMoveUp, row);
            MoveGroupDown(uieListMoveDown, row);


            //UIElementCollection test = myGrid.Children;
            //int i = 0;
            //throw new NotImplementedException();
        }

        void MoveGroupUp(List<UIElement> uieListMoveUp, int row)
        {
            foreach (UIElement uie in uieListMoveUp)
            {
                Grid.SetRow(uie, row);
            }
            TextBlock tb = (TextBlock)uieListMoveUp[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            //int groupNumber = Convert.ToInt32(gt.GroupOrder);
            gt.GroupOrder--;
            //gt.GroupOrder =
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



        void MoveGroupDown(List<UIElement> uieListMoveDown, int row)
        {
            foreach (UIElement uie in uieListMoveDown)
            {
                Grid.SetRow(uie, row + 1);
            }
            TextBlock tb = (TextBlock)uieListMoveDown[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            //int groupNumber = Convert.ToInt32(gt.GroupOrder);
            gt.GroupOrder++;

            StackPanel sp = (StackPanel)uieListMoveDown[1];
            Button btnDown = (Button)sp.Children[1];
            Button btnUp = (Button)sp.Children[0];
            if (gt.GroupOrder == groups.Count)
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
            Grid gridGroup = (Grid)btn.DataContext;
            int row = Grid.GetRow(sp);
            //Grid.SetRow(gridGroup, row + 1);
            List<UIElement> uieListMoveDown = myGrid.GetGridCellChildrenListByRow(row - 1);
            List<UIElement> uieListMoveUp = myGrid.GetGridCellChildrenListByRow(row);
            MoveGroupUp(uieListMoveUp, row - 1);
            MoveGroupDown(uieListMoveDown, row - 1);
            
            
        }

        public void PopulateGroupTable(GroupType gt, Grid groupTable)
        {
            CreateColumns(groupTable, 4);
            int counterRow = 0;
            int counterColumn = 0;
            AddNewGroupRow(groupTable);
            TextBlock groupHeaderName = new TextBlock();
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
            int j = 0;
            while (i < groupTable.RowDefinitions.Count)
            {
                j = 0;
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
            GroupType gt = (GroupType)gridGroupTable.DataContext;
            TextBlock tb = (TextBlock)gridCell.Children[1];

            ItemType itToBeDeleted = (ItemType)tb.DataContext;
            gt.Items.Remove(itToBeDeleted);



            int i = Convert.ToInt32(itToBeDeleted.ItemOrder);
            ItemType itemType = new ItemType();
            tb.DataContext = itemType;

            //if (CheckForEmptyRow(groupTable, row))
            //{
            //    if (row != 1)
            //    {
            //        ClearNewRowItems(groupTable, row - 1);
            //    }
            //    ClearNewRowItems(groupTable, row);
            //    groupTable.RemoveRow(row);

            //}
            //else
            //{
            //    if(!itToBeDeleted.DesignID.Equals("197"))
            //    CheckForEmptyFields(groupTable, row);
            //}
            List<ItemType> itemTypeList = GetItemTypes(groupTable);
            //groupTable.RemoveRow(row);
            //////////////
            bool stopCounting = false;
            // CheckRow(grid, row - 1);

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
                //CheckRow(groupTable, n, false);
                n++;
            }
            if (itToBeDeleted.DesignID.Equals("198"))
            {
                //CheckForEmptyFields(groupTable, row);
                //CheckRow(groupTable, row, false);
                if (row == groupTable.RowDefinitions.Count - 2)
                    ClearNewRowItems(groupTable, groupTable.RowDefinitions.Count - 2);
            }
            //CheckForEmptyFields(groupTable, row);
            //CheckRow(groupTable, row -1, true);
            //n = 1;
            //while (n < groupTable.RowDefinitions.Count - 2)
            //{
            //    CheckRow(groupTable, n, false);
            //    n++;
            //}
            DisableAllowDropByNewLineItem(groupTable);
        }

        private void ClearNewRowItems(Grid groupTable, int row)
        {

            List<ItemType> itemTypeListCheck = GetItemsByRow(groupTable, row);
            int i = 0;
            bool cleared = false;
            while (i < itemTypeListCheck.Count)
            {
                ItemType it = itemTypeListCheck[i];
                if (it.DesignID != null && it.DesignID.Equals("198"))
                {
                    it.DesignID = null;
                    it.Header = null;
                    it.ItemOrder = 0;
                    cleared = true;
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

        private void CheckForEmptyFields(Grid groupTable, int row)
        {
            List<ItemType> itemTypeListCheck = GetItemsByRow(groupTable, row);
            bool deleteEmptyField = true;
            int i = itemTypeListCheck.Count - 1;
            while (i >= 0)
            {
                string designID = itemTypeListCheck[i].DesignID;
                //if (itemTypeListCheck[3].DesignID != null && !itemTypeListCheck[3].DesignID.Equals("198"))
                //{
                //    deleteEmptyField = false;
                //}
                if (designID != null && designID.Equals("198"))
                {
                    deleteEmptyField = false;
                }

                if (deleteEmptyField && designID != null && designID.Equals("197"))
                {
                    itemTypeListCheck[i].DesignID = null;
                    itemTypeListCheck[i].Header = null;
                }
                i--;
            }
        }


        //private bool CheckForEmptyRow(Grid groupTable, int row)
        //{
        //    List<ItemType> itemTypeList = GetItemsByRow(groupTable, row);
        //    int i = 0;
        //    bool isEmpty = true;
        //    while (i < itemTypeList.Count && isEmpty)
        //    {
        //        string designID = itemTypeList[i].DesignID;
        //        if (designID == null || designID.Equals("198") || designID.Equals("197"))
        //            i++;
        //        else
        //            isEmpty = false;
        //    }
        //    return isEmpty;
        //}

        private void CheckRow(Grid groupTable, int row, bool addEmptyFieldsOnly)
        {
            List<ItemType> itemTypeListCheck = GetItemsByRow(groupTable, row);
            bool addNewRowItem = true;
            int i = itemTypeListCheck.Count - 1;
            while (i >= 0)
            {
                string designID = itemTypeListCheck[i].DesignID;
                if (itemTypeListCheck[3].DesignID != null && !itemTypeListCheck[3].DesignID.Equals("198"))
                {
                    addNewRowItem = false;
                }
                if (designID != null && designID.Equals("198"))
                {
                    addNewRowItem = false;
                }

                if (addNewRowItem && designID != null && !designID.Equals("197"))
                {
                    if (!addEmptyFieldsOnly)
                    {
                        itemTypeListCheck[i + 1].DesignID = "198";
                        itemTypeListCheck[i + 1].Header = "<NewRowItem>";
                    }
                    addNewRowItem = false;
                }

                if (!addNewRowItem && designID == null)
                {
                    itemTypeListCheck[i].DesignID = "197";
                    itemTypeListCheck[i].Header = "<EmptyFieldItem>";
                }
                i--;
            }
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

        //private void GenerateEmptyFields(Grid groupTable, int row, bool wholeRow)
        //{
        //    List<ItemType> itemTypeListCheck = GetItemsByRow(groupTable, row);
        //    bool newLineItemExist = CheckForNewLineItem(groupTable, row);
        //    bool addEmptyfields = false;
        //    int i = itemTypeListCheck.Count - 1;

        //    while (i >= 0)
        //    {
        //        string designID = itemTypeListCheck[i].DesignID;

        //        if (newLineItemExist && !wholeRow)
        //        {
        //            addEmptyfields = false;
        //        }

        //        if (newLineItemExist && designID != null && itemTypeListCheck[i].DesignID.Equals("198") && !wholeRow)
        //        {
        //            addEmptyfields = true;
        //            newLineItemExist = false;
        //            wholeRow = false;
        //        }

        //        if (!newLineItemExist && !addEmptyfields && designID != null && !wholeRow)
        //        {
        //            addEmptyfields = true;
        //        }

        //        if (addEmptyfields && designID == null && !wholeRow)
        //        {
        //            itemTypeListCheck[i].DesignID = "197";
        //            itemTypeListCheck[i].Header = "<EmptyField>";
        //        }

        //        if (wholeRow && designID == null && !newLineItemExist)
        //        {
        //            itemTypeListCheck[i].DesignID = "197";
        //            itemTypeListCheck[i].Header = "<EmptyField>";
        //        }


        //        i--;
        //    }
        //}



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
            TextBlock tb = new TextBlock();
            tb.FontWeight = FontWeights.Bold;
            tb.FontSize = 14.0;
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
            TextBlock tb = new TextBlock();
            tb.FontSize = 14.0;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Stretch;
            tb.DataContext = itemType;
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
                bool containsRow = CheckForNewLineItem(grid, row);
                //if (designID == 198 && !containsRow)
                //{

                //}
                //else
                //{
                    if (designID != 0 /*&& designID != 197*/)
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
                        moveItemsForward(startPosition, itemTypeList, grid, newItemType, gt);
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
                        Border bTarget = (Border)gridCell.Parent;
                        int col = Grid.GetColumn(bTarget);
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
                            int startColumnPosition = Grid.GetColumn(borderCell);
                            //DisableAllowDrop(startColumnPosition, row, grid);
                            DisableAllowDropByNewLineItem(grid);
                        }
                        gt.Items.Add(itToBeMoved);
                    }
                //}
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

        void moveItemsForward(int startPosition, List<ItemType> itemTypeList, Grid grid, ItemType newItemType, GroupType gt)
        {

            bool stopCounting = false;
            int i = startPosition;
            int n = 0;
            while (n < grid.RowDefinitions.Count - 1)
            {
                //CheckRow(grid, row, false);
                n++;
            }
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
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(borderBrush);
            border.Background = new SolidColorBrush(background);
            border.BorderThickness = new Thickness(1);
            border.Height = 27.0;
            border.MaxHeight = 27.0;
            border.HorizontalAlignment = HorizontalAlignment.Stretch;
            return border;
        }

        public Button CreateClearCellBtn()
        {
            Button btnClearCell = new Button();
            btnClearCell.Width = 16.0;
            btnClearCell.Height = 16.0;
            btnClearCell.HorizontalAlignment = HorizontalAlignment.Right;
            btnClearCell.VerticalAlignment = VerticalAlignment.Top;
            var uriSource = new Uri(@"/TreatPraktik;component/Ressources/Delete-icon.png", UriKind.Relative);
            BitmapImage logo = new BitmapImage();
            Image img = new Image();
            img.Source = new BitmapImage(uriSource);
            btnClearCell.Content = img;
            btnClearCell.Visibility = Visibility.Hidden;
            btnClearCell.Click += btnRemove_Click;
            return btnClearCell;
        }
    }
}
