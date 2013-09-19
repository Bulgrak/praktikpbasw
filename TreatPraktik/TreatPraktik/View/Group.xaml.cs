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

        public void PopulateGrid()
        {
            for (int i = 0; i < groups.Count; i++)
            {

                GroupType gt = groups[i];
                Grid grid = new Grid();
                Border border = new Border();
                Color colorGroupRow = (Color)ColorConverter.ConvertFromString("#97CBFF");
                for (int j = 0; j < 4; j++)
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
                Button btnAddNewRow = new Button();
                btnAddNewRow.HorizontalAlignment = HorizontalAlignment.Right;
                btnAddNewRow.VerticalAlignment = VerticalAlignment.Bottom;
                btnAddNewRow.Content = "Row+";
                btnAddNewRow.Click += btnAddNewRow_Click;
                Grid.SetRow(btnAddNewRow, i);
                Grid.SetColumn(btnAddNewRow, 0);
                myGrid.Children.Add(btnAddNewRow);
            }
        }

        void btnAddNewRow_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Grid myGrid = (Grid)btn.Parent;
            UIElementCollection uiElementCollection = myGrid.Children;
            int i = 0;
            
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
            //tb.Text = itemType.DatabaseFieldName;
            int i = 0;
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
            //tb.Text = groupType.GroupName;
            int i = 0;
        }


        private void AddNewItemRow(Grid grid)
        {
            RowDefinition rd = new RowDefinition();
            grid.RowDefinitions.Add(rd);
            int rowNo = grid.RowDefinitions.Count - 1;
            int itemOrderCounter = 0;
            Color colorItemRow = (Color)ColorConverter.ConvertFromString("#E4F1FF");
            int i = 0;
            while (i < 4)
            {
                ItemType itemType = new ItemType();
                itemType.ItemOrder = ((rowNo -1)* 4) + i;
                Border border = new Border();
                //border.BorderBrush = new SolidColorBrush(colorItemRow);
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                border.Background = new SolidColorBrush(Colors.Yellow);
                border.BorderThickness = new Thickness(1.0);
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
        }

        void tb_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Controls.ListBoxItem") is ListBoxItem)
            {
                
                TextBlock textBlock = e.Source as TextBlock;
                ItemType itemType = (ItemType)textBlock.DataContext;

                TextBlock source = sender as TextBlock;
                ListBoxItem lbi = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
                ToolboxItem tbi = (ToolboxItem)lbi.Content;
                Border test = (Border)textBlock.Parent;
                var test2 = test.Parent;
                itemType.DesignID = tbi.DesignID;
                itemType.Header = tbi.Header;
                textBlock.Text = tbi.Header;

                //var test = (ToolboxItem)e.Source;
                int i = 1;
                //ListView listView = sender as ListView;
                //listView.Items.Add(contact);

            }
        }

        private void MoveAllItemsOnceCellForward()
        {

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
                //border.Padding = new Thickness(0);
                TextBlock tb = new TextBlock();
                tb.FontWeight = FontWeights.Bold;
                tb.FontSize = 14.0;
                border.Child = tb;
                Grid.SetRow(border, rowNo);
                Grid.SetColumn(border, i);
                grid.Children.Add(border);
                i++;
            }
        }
    }
}
