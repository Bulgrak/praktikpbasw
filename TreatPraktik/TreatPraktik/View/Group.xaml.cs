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

        public void PopulateGroupTable(GroupType gt, Grid grid)
        {
            CreateColumns(grid);
            int counterRow = 0;
            int counterColumn = 0;
            AddNewGroupRow(grid);
            TextBlock groupHeaderName = new TextBlock();
            InsertGroupItem(grid, gt, 0, 0, false);
            int skipped = 0;
            for (int j = 0; j < gt.Items.Count - skipped; j++)
            {
                if (gt.Items[j + skipped].DesignID.Equals("198"))
                {
                    InsertItem(grid, gt.Items[j + skipped], counterRow, j % 4, true);
                    skipped = skipped + j;
                    j = 0;
                }
                else if (j % 4 == 0)
                {
                    AddNewEmptyItemRow(grid);
                    counterRow++;
                }
                if (gt.Items[j + skipped].DesignID.Equals("198"))
                {
                    //InsertItem(grid, gt.Items[j + skipped], counterRow, j % 4, true);
                    j--;
                    skipped++;
                    continue;
                }
                if (gt.Items[j + skipped].DesignID.Equals("197"))
                {
                    InsertItem(grid, gt.Items[j + skipped], counterRow, j % 4, true);
                }
                else
                {
                    if (counterColumn >= 4)
                    {
                        counterColumn = 0;
                    }
                    InsertItem(grid, gt.Items[j + skipped], counterRow, j % 4, true);
                }
                counterColumn++;
            }
            AddNewEmptyItemRow(grid);
        }

        private void CreateColumns(Grid grid)
        {
            for (int j = 0; j < 4; j++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                grid.ColumnDefinitions.Add(cd);
            }
        }

        private void RemoveRow(Grid grid, int rowCount)
        {
            grid.RemoveGridCellChildrenByRow(rowCount);
            grid.RowDefinitions.RemoveAt(rowCount);
            grid.UpdateCellContentsRowPosition(rowCount);
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
                    TextBlock tb = (TextBlock)b.Child;
                    ItemType it = (ItemType)tb.DataContext;
                    if (it.DesignID != null)
                        itemTypeList.Add(it);
                    j++;
                }
                i++;
            }
            return itemTypeList;
        }

        private void InsertItem(Grid groupTable, ItemType itemType, int row, int column, bool allowDrop)
        {
            Border border = (Border)groupTable.Children
      .Cast<UIElement>()
      .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            TextBlock tb = (TextBlock)border.Child;
            tb.AllowDrop = allowDrop;
            tb.DataContext = itemType; //skal sættes til at indeholde et toolboxitem
            tb.Text = itemType.Header;
        }

        private void InsertGroupItem(Grid groupTable, GroupType groupType, int row, int column, bool allowDrop)
        {
            Border border = (Border)groupTable.Children
      .Cast<UIElement>()
      .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            TextBlock tb = (TextBlock)border.Child;
            tb.AllowDrop = allowDrop;
            tb.DataContext = groupType; //skal sættes til at indeholde et toolboxitem
            tb.Text = groupType.GroupHeader;
        }

        private void AddNewEmptyItemRow(Grid groupTable)
        {
            RowDefinition rd = new RowDefinition();
            groupTable.RowDefinitions.Add(rd);
            int rowNo = groupTable.RowDefinitions.Count - 1;
            int i = 0;
            while (i < 4)
            {
                ItemType itemType = new ItemType();
                itemType.ItemOrder = ((rowNo - 1) * 4) + i;
                Border border = CreateCell(Colors.Black, Colors.Yellow);

                TextBlock tb = new TextBlock();
                tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                tb.VerticalAlignment = VerticalAlignment.Stretch;
                tb.DataContext = itemType;
                tb.AllowDrop = true;
                tb.Drop += tb_Drop;
                border.Child = tb;

                Grid.SetRow(border, rowNo);
                Grid.SetColumn(border, i);
                groupTable.Children.Add(border);
                i++;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int rowNo = Grid.GetRow(btn);
            Grid groupTable = (Grid)btn.Parent;

            List<UIElement> elementsToRemove = new List<UIElement>();

            RemoveRow(groupTable, rowNo);
        }

        private void tb_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem)
            {
                TextBlock target = e.Source as TextBlock;
                TextBlock source = sender as TextBlock;
                ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
                ToolboxItem tbi = (ToolboxItem)lbi.Content;

                ItemType itToBeMoved = (ItemType)target.DataContext;
                int designID = Convert.ToInt32(itToBeMoved.DesignID);
                if (designID != 0 && designID != 198)
                {
                    Border b1 = (Border)target.Parent;
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
                    CheckRow(grid, row - 1);
                    CheckRow(grid, row);
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
                    grid.Children.Clear();
                    grid.RowDefinitions.Clear();
                    grid.ColumnDefinitions.Clear();
                    PopulateGroupTable(gt, grid);
                }
                else
                {
                    itToBeMoved.DesignID = tbi.DesignID;
                    itToBeMoved.Header = tbi.Header;
                    target.Text = tbi.Header;
                    Border bTarget = (Border)target.Parent;
                    Grid grid = (Grid)bTarget.Parent;
                    int row = Grid.GetRow(bTarget);
                    int col = Grid.GetColumn(bTarget);
                    if (row == grid.RowDefinitions.Count - 1)
                    {
                        AddNewEmptyItemRow(grid);
                        CheckRow(grid, row - 1);
                    }
                }
            }
        }

        private void CheckRow(Grid grid, int row)
        {
            List<UIElement> uieList = new List<UIElement>();
            List<ItemType> itemTypeListCheck = new List<ItemType>();

            int col = grid.ColumnDefinitions.Count - 1;
            while (0 <= col)
            {
                UIElement uie = grid.GetGridCellChildren(row, col);
                uieList.Add(uie);
                col--;
            }
            foreach (UIElement uie in uieList)
            {
                if (uie is Border)
                {
                    Border b = (Border)uie;
                    TextBlock tb = (TextBlock)b.Child;
                    ItemType it = (ItemType)tb.DataContext;


                    itemTypeListCheck.Add(it);
                }
            }

            bool addNewRowItem = true;
            int i = 0;
            while (i < itemTypeListCheck.Count)
            {
                string designID = itemTypeListCheck[i].DesignID;
                if (itemTypeListCheck[0].DesignID != null && !itemTypeListCheck[0].DesignID.Equals("198"))
                {
                    addNewRowItem = false;
                }
                if (designID != null && designID.Equals("198"))
                {
                    addNewRowItem = false;
                }

                if (addNewRowItem && designID != null && !designID.Equals("197"))
                {
                    itemTypeListCheck[i - 1].DesignID = "198";
                    addNewRowItem = false;
                }

                if (!addNewRowItem && designID == null)
                {
                    itemTypeListCheck[i].DesignID = "197";
                }
                i++;
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
                Border cellItem = CreateCell(cellColorGroup, cellColorGroup);
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

        private Border CreateCell(Color borderBrush, Color background)
        {
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(borderBrush);
            border.Background = new SolidColorBrush(background);
            border.BorderThickness = new Thickness(1);
            border.Height = 27.0;
            border.MaxHeight = 27.0;
            border.HorizontalAlignment = HorizontalAlignment.Stretch;

            TextBlock tb = new TextBlock();
            tb.FontWeight = FontWeights.Bold;
            tb.FontSize = 14.0;

            border.Child = tb;
            return border;
        }
    }
}
