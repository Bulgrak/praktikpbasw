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
            myGrid.VerticalAlignment = VerticalAlignment.Top;
        }

        public void PopulateGrid()
        {
            for (int i = 0; i < groups.Count; i++)
            {

                GroupType gt = groups[i];
                Grid grid = new Grid();
                grid.DataContext = gt;
                Border border = new Border();
                Color colorGroupRow = (Color)ColorConverter.ConvertFromString("#97CBFF");
                for (int j = 0; j < 5; j++)
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(cd);
                }
                int counterRow = 0;
                int counterColumn = 0;
                AddNewGroupRow(grid);
                TextBlock groupHeaderName = new TextBlock();
                InsertGroupItem(grid, gt, 0, 0, false);
                //AddNewItemRow(grid);
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
                        AddNewItemRow(grid);
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
                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(grid, i);
                Grid.SetColumn(grid, 1);
                myGrid.Children.Add(grid);
                TextBlock tbGroupNumber = new TextBlock();
                Grid.SetRow(tbGroupNumber, i);
                Grid.SetColumn(tbGroupNumber, 0);
                tbGroupNumber.Text = Convert.ToString(gt.GroupOrder);
                tbGroupNumber.FontSize = 18;
                tbGroupNumber.FontWeight = FontWeights.ExtraBold;
                tbGroupNumber.Foreground = Brushes.LightSlateGray;
                myGrid.Children.Add(tbGroupNumber);
                AddNewRowBtn(grid);
            }
        }

        public void PopulateGroup(GroupType gt, Grid grid)
        {
            Border border = new Border();
            Color colorGroupRow = (Color)ColorConverter.ConvertFromString("#97CBFF");
            for (int j = 0; j < 5; j++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                grid.ColumnDefinitions.Add(cd);
            }
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
                    AddNewItemRow(grid);
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
            //RowDefinition rd = new RowDefinition();
            //myGrid.RowDefinitions.Add(rd);
            //Grid.SetRow(grid, i);
            //Grid.SetColumn(grid, 1);
            //myGrid.Children.Add(grid);
            //TextBlock tbGroupNumber = new TextBlock();
            //Grid.SetRow(tbGroupNumber, i);
            //Grid.SetColumn(tbGroupNumber, 0);
            //tbGroupNumber.Text = Convert.ToString(gt.GroupOrder);
            //tbGroupNumber.FontSize = 18;
            //tbGroupNumber.FontWeight = FontWeights.ExtraBold;
            //tbGroupNumber.Foreground = Brushes.LightSlateGray;
            //myGrid.Children.Add(tbGroupNumber);
            AddNewRowBtn(grid);
            grid.UpdateLayout();
        }

        void RemoveRow(Grid gridi, int rowCount)
        {
            List<UIElement> elementsToRemove = new List<UIElement>();

            foreach (UIElement element in gridi.Children)
            {
                if (Grid.GetRow(element) == rowCount)
                    elementsToRemove.Add(element);
            }
            foreach (UIElement element in elementsToRemove)
                gridi.Children.Remove(element);

            gridi.RowDefinitions.RemoveAt(rowCount);
            UpdateItemsRowPosition(gridi, rowCount);
        }

        public void UpdateItemsRowPosition(Grid grid, int row)
        {
            if (row < grid.RowDefinitions.Count)
            {
                for (int i = row + 1; i <= grid.RowDefinitions.Count; i++)
                {
                    for (int j = 0; j < grid.ColumnDefinitions.Count; j++)
                    {
                        List<UIElement> children = GetGridCellChildren(grid, i, j);
                        foreach (UIElement uie in children)
                        {
                            Grid.SetRow(uie, i - 1);
                        }
                    }
                }
            }
        }

        public List<UIElement> GetGridCellChildren(Grid grid, int row, int col)
        {
            return grid.Children.Cast<UIElement>().Where(
                                x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col).ToList();
        }

        GroupType GetGroupType(Grid grid)
        {
            Border b = (Border)grid.Children
     .Cast<UIElement>()
     .First(a => Grid.GetRow(a) == 0 && Grid.GetColumn(a) == 0);
            TextBlock tb = (TextBlock)b.Child;
            GroupType gt = (GroupType)tb.DataContext;
            return gt;
        }

        List<ItemType> GetItemTypes(Grid grid)
        {
            List<ItemType> itemTypeList = new List<ItemType>();
            int i = 1;
            int j = 0;
            while (i < grid.RowDefinitions.Count - 1)
            {
                j = 0;
                while (j < grid.ColumnDefinitions.Count - 1)
                {
                    Border b = (Border)grid.Children
 .Cast<UIElement>()
 .First(a => Grid.GetRow(a) == i && Grid.GetColumn(a) == j);
                    TextBlock tb = (TextBlock)b.Child;
                    ItemType it = (ItemType)tb.DataContext;
                    if (it.DesignID != null)
                        itemTypeList.Add(it);
                    //borderList.Add(b);
                    j++;
                }
                i++;
            }
            return itemTypeList;
        }

        void AddNewRowBtn(Grid grid)
        {
            RowDefinition rdAddNewRow = new RowDefinition();
            grid.RowDefinitions.Add(rdAddNewRow);
            Button btnAddNewRow = new Button();
            btnAddNewRow.Content = "Add new row";
            btnAddNewRow.Click += btnAddNewRow_Click;
            btnAddNewRow.HorizontalAlignment = HorizontalAlignment.Right;
            btnAddNewRow.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(btnAddNewRow, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(btnAddNewRow, 0);
            Grid.SetColumnSpan(btnAddNewRow, 4);

            grid.Children.Add(btnAddNewRow);
        }

        void btnAddNewRow_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Grid grid = (Grid)btn.Parent;
            int rowCount = Grid.GetRow(btn);
            List<UIElement> elementsToRemove = new List<UIElement>();

            foreach (UIElement element in grid.Children)
            {
                if (Grid.GetRow(element) == rowCount)
                    elementsToRemove.Add(element);
            }
            foreach (UIElement element in elementsToRemove)
                grid.Children.Remove(element);

            grid.RowDefinitions.RemoveAt(rowCount);
            AddNewItemRow(grid);
            AddNewRowBtn(grid);
        }

        private void InsertItem(Grid grid, ItemType itemType, int row, int column, bool allowDrop)
        {
            Border border = (Border)grid.Children
      .Cast<UIElement>()
      .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            TextBlock tb = (TextBlock)border.Child;
            tb.AllowDrop = allowDrop;
            tb.DataContext = itemType; //skal sættes til at indeholde et toolboxitem
            tb.Text = itemType.Header;
        }

        private void InsertGroupItem(Grid grid, GroupType groupType, int row, int column, bool allowDrop)
        {
            Border border = (Border)grid.Children
      .Cast<UIElement>()
      .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            TextBlock tb = (TextBlock)border.Child;
            tb.AllowDrop = allowDrop;
            tb.DataContext = groupType; //skal sættes til at indeholde et toolboxitem
            tb.Text = groupType.GroupHeader;
        }


        private void AddNewItemRow(Grid grid)
        {
            RowDefinition rd = new RowDefinition();
            grid.RowDefinitions.Add(rd);
            int rowNo = grid.RowDefinitions.Count - 1;
            Color colorItemRow = (Color)ColorConverter.ConvertFromString("#E4F1FF");
            int i = 0;
            while (i < 4)
            {
                ItemType itemType = new ItemType();
                itemType.ItemOrder = ((rowNo - 1) * 4) + i;
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                border.Background = new SolidColorBrush(Colors.Yellow);
                border.BorderThickness = new Thickness(1.0);
                border.Height = 27.0;
                border.MaxHeight = 27.0;
                border.HorizontalAlignment = HorizontalAlignment.Stretch;
                TextBlock tb = new TextBlock();
                tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                tb.VerticalAlignment = VerticalAlignment.Stretch;
                tb.DataContext = itemType;
                tb.AllowDrop = true;
                tb.Drop += tb_Drop;
                border.Child = tb;

                Grid.SetRow(border, rowNo);
                Grid.SetColumn(border, i);
                grid.Children.Add(border);
                i++;
            }
            Button btnRemove = new Button();
            btnRemove.Width = 16.0;
            btnRemove.Height = 16.0;
            btnRemove.HorizontalAlignment = HorizontalAlignment.Left;
            btnRemove.VerticalAlignment = VerticalAlignment.Center;
            var uriSource = new Uri(@"/TreatPraktik;component/Ressources/Delete-icon.png", UriKind.Relative);
            BitmapImage logo = new BitmapImage();
            Image img = new Image();
            img.Source = new BitmapImage(uriSource);
            btnRemove.Content = img;
            btnRemove.Click += btnRemove_Click;
            btnRemove.DataContext = rowNo;
            Grid.SetColumn(btnRemove, 4);
            Grid.SetRow(btnRemove, rowNo);
            grid.Children.Add(btnRemove);
        }

        void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int rowNo = Grid.GetRow(btn);
            Grid grid = (Grid)btn.Parent;


            List<UIElement> elementsToRemove = new List<UIElement>();

            //foreach (UIElement element in grid.Children)
            //{

            //    if (Grid.GetRow(element) == grid.RowDefinitions.Count - 1)

            //        elementsToRemove.Add(element);

            //}

            //foreach (UIElement element in elementsToRemove)

            //   grid.Children.Remove(element);
            //grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count - 1);

            RemoveRow(grid, rowNo);
        }

        private void RemoveItemRow(Grid grid)
        {
            int lastRow = grid.RowDefinitions.Count - 1;
            grid.RowDefinitions.RemoveAt(lastRow);
        }

        void tb_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem)
            {

                TextBlock textBlock = e.Source as TextBlock;
                //ItemType itemType = (ItemType)textBlock.DataContext;

                TextBlock source = sender as TextBlock;
                ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
                ToolboxItem tbi = (ToolboxItem)lbi.Content;

                ItemType itToBeMoved = (ItemType)textBlock.DataContext;
                int designID = Convert.ToInt32(itToBeMoved.DesignID); 
                if (designID != 0 && designID != 198)
                {
                        Border b1 = (Border)textBlock.Parent;
                        ItemType newItemType = new ItemType();
                        newItemType.DesignID = tbi.DesignID;
                        newItemType.Header = tbi.Header;
                        newItemType.ItemOrder = itToBeMoved.ItemOrder;
                        Grid grid = (Grid)b1.Parent;
                        GroupType gt = GetGroupType(grid);
                        //gt.Items = GetItemTypes(grid, );

                        //Grid.GetRow(b);newItemType
                        //int i = 1;
                        //int j = 0;
                        //int i = Grid.GetRow(b1);
                        List<ItemType> itemTypeList = GetItemTypes(grid);
                        int i = itemTypeList.IndexOf(itToBeMoved);
                    bool stopCounting = false;
                        while (i < itemTypeList.Count && !stopCounting)
                        {
                            if (itemTypeList[i].DesignID.Equals("198"))
                            {
                                itemTypeList.RemoveAt(i);
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
                        PopulateGroup(gt, grid);
                        //           while (i < grid.RowDefinitions.Count - 1)
                        //           {
                        //               j = 0;
                        //               while (j < grid.ColumnDefinitions.Count - 1)
                        //               {
                        //                   Border b = (Border)grid.Children
                        //.Cast<UIElement>()
                        //.First(a => Grid.GetRow(a) == i && Grid.GetColumn(a) == j);
                        //                   TextBlock tb = (TextBlock)b.Child;
                        //                   ItemType it = (ItemType)tb.DataContext;
                        //                   if (it.DesignID != null)
                        //                       itemTypeList.Add(it);
                        //                   //borderList.Add(b);
                        //                   j++;
                        //               }
                        //               i++;
                        //           }
                }
                else
                {
                    itToBeMoved.DesignID = tbi.DesignID;
                    itToBeMoved.Header = tbi.Header;
                    textBlock.Text = tbi.Header;
                }
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
                Color colorGroupRow = (Color)ColorConverter.ConvertFromString("#97CBFF");

                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(colorGroupRow);
                border.Background = new SolidColorBrush(colorGroupRow);
                border.BorderThickness = new Thickness(0);
                border.Height = 27.0;
                border.MaxHeight = 27.0;
                border.HorizontalAlignment = HorizontalAlignment.Stretch;

                TextBlock tb = new TextBlock();
                tb.FontWeight = FontWeights.Bold;
                tb.FontSize = 14.0;

                border.Child = tb;

                if (i == 0)
                {
                    Grid.SetRow(border, rowNo);
                    Grid.SetColumn(border, i);
                    Grid.SetColumnSpan(border, 4);
                    grid.Children.Add(border);
                }
                //else
                //{
                //    Grid.SetRow(border, rowNo);
                //    Grid.SetColumn(border, i);
                //    grid.Children.Add(border);
                //}

                i++;
            }
        }
    }
}
