using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<GroupType> groups { get; set; }

        public Group()
        {
            InitializeComponent();
        }

        public void CreateGroupTables()
        {
            for (int i = 0; i < groups.Count; i++)
            {
                GroupType gt = groups[i];
                Grid gridGroup = new Grid();
                gridGroup.DataContext = gt;

                PopulateGroupTable(gt, gridGroup);

                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(gridGroup, i);
                Grid.SetColumn(gridGroup, 1);
                myGrid.Children.Add(gridGroup);
                TextBlock tbGroupNumber = new TextBlock();
                Grid.SetRow(tbGroupNumber, i);
                Grid.SetColumn(tbGroupNumber, 0);
                tbGroupNumber.Text = Convert.ToString(gt.GroupOrder);
                tbGroupNumber.FontSize = 18;
                tbGroupNumber.FontWeight = FontWeights.ExtraBold;
                tbGroupNumber.Foreground = Brushes.LightSlateGray;
                myGrid.Children.Add(tbGroupNumber);
            }
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
                    gt.Items[j + skipped].Header = "<NewRowItem>";
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
                    //InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true);
                    j--;
                    skipped++;
                    continue;
                }
                if (gt.Items[j + skipped].DesignID.Equals("197"))
                {
                    SolidColorBrush textColor = Brushes.Black;
                    InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor);
                }
                else
                {
                    if (counterColumn >= 4)
                    {
                        counterColumn = 0;
                    }
                    SolidColorBrush textColor = Brushes.Black;
                    InsertItem(groupTable, gt.Items[j + skipped], counterRow, j % 4, true, textColor);
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
            int row = Grid.GetRow(border);
            Grid groupTable = (Grid)border.Parent;
            GroupType gt = (GroupType)gridGroupTable.DataContext;
            TextBlock tb = (TextBlock)gridCell.Children[1];
            ItemType itToBeDeleted = (ItemType)tb.DataContext;

            int i = Convert.ToInt32(itToBeDeleted.ItemOrder);
            ItemType itemType = new ItemType();
            tb.DataContext = itemType;

            if (CheckForEmptyRow(groupTable, row))
            {
                ClearNewRowItems(groupTable, row - 1);
                groupTable.RemoveRow(row);

            }
            else
            {
                CheckForEmptyFields(groupTable, row);
            }
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
                CheckRow(groupTable, n, false);
                n++;
            }


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
                    it.DatabaseFieldName = null;
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
                if (itemTypeListCheck[3].DesignID != null && !itemTypeListCheck[3].DesignID.Equals("198"))
                {
                    deleteEmptyField = false;
                }
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


        private bool CheckForEmptyRow(Grid groupTable, int row)
        {
            List<ItemType> itemTypeList = GetItemsByRow(groupTable, row);
            int i = 0;
            bool isEmpty = true;
            while (i < itemTypeList.Count && isEmpty)
            {
                string designID = itemTypeList[i].DesignID;
                if (designID == null || designID.Equals("198") || designID.Equals("197"))
                    i++;
                else
                    isEmpty = false;
            }
            return isEmpty;
        }

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
            TextBlock tb = new TextBlock();
            tb.FontSize = 14.0;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Stretch;
            tb.DataContext = itemType;
            tb.SetBinding(TextBlock.TextProperty, "Header");
            //tb.AllowDrop = true;
            //tb.Drop += tb_Drop;

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
                if (designID != 0 && designID != 198)
                {
                    Border b1 = (Border)gridCell.Parent;
                    int row = Grid.GetRow(b1);
                    ItemType newItemType = new ItemType();
                    newItemType.DesignID = tbi.DesignID;
                    newItemType.Header = tbi.Header;
                    newItemType.ItemOrder = itToBeMoved.ItemOrder;
                    Grid grid = (Grid)b1.Parent;
                    GroupType gt = GetGroupType(grid);
                    List<ItemType> itemTypeList = GetItemTypes(grid);
                    int i = itemTypeList.IndexOf(itToBeMoved);
                    bool stopCounting = false;
                    int n = 0;
                    while (n < grid.RowDefinitions.Count - 1)
                    {
                        CheckRow(grid, row, false);
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
                else
                {
                    itToBeMoved.DesignID = tbi.DesignID;
                    itToBeMoved.Header = tbi.Header;
                    Border bTarget = (Border)gridCell.Parent;
                    Grid grid = (Grid)bTarget.Parent;
                    int row = Grid.GetRow(bTarget);
                    int col = Grid.GetColumn(bTarget);
                    if (row == grid.RowDefinitions.Count - 1)
                    {
                        AddNewEmptyItemRow(grid);
                        CheckRow(grid, row - 1, false);
                    }
                    CheckRow(grid, row, true);
                }
            }
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
