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
                DataGrid dg = new DataGrid();
                dg.IsReadOnly = true;
                dg.LoadingRow += dg_LoadingRow;
                //dg.GridLinesVisibility = DataGridGridLinesVisibility.None;
                Color colorGroupRow = (Color)ColorConverter.ConvertFromString("#97CBFF");
                dg.HorizontalGridLinesBrush = new SolidColorBrush(colorGroupRow);
                dg.VerticalGridLinesBrush = new SolidColorBrush(colorGroupRow);
                DataTable dt = new DataTable();
                for (int j = 0; j < 4; j++)
                {
                    DataColumn cl = new DataColumn("Col" + j, typeof(string));
                    dt.Columns.Add(cl);
                }
                int counterRow = 0;
                int counterColumn = 0;
                DataRow rw = dt.NewRow();
                dt.Rows.Add(rw);
                rw["Col0"] = gt.GroupName;
               
                int skipped = 0;
                for (int j = 0; j < gt.Items.Count - skipped; j++)
                {
                    if (gt.Items[j+skipped].DesignID.Equals("198"))
                    {
                        skipped = skipped + j;
                        j=0;
                    } else if (j % 4 == 0)
                    {
                        rw = dt.NewRow();
                        dt.Rows.Add();
                        counterRow++;
                    }
                    if (gt.Items[j+skipped].DesignID.Equals("198"))
                    {
                        j--;
                        skipped++;
                        continue;
                    }
                    if (gt.Items[j + skipped].DesignID.Equals("197"))
                    {
                        //Empty field
                    }
                    else
                    {
                        //if (counterRow >= 4)
                        //{
                        //    counterRow = 1;
                        //}
                        if (counterColumn >= 4)
                        {
                            counterColumn = 0;
                        }
                        //rw["Col" + counterRow] = gt.Items[j].DatabaseFieldName;
                        dt.Rows[counterRow][j % 4] = gt.Items[j + skipped].DatabaseFieldName;
                        //dt.Rows[1][0] = "Test";
                        int hej = 1;
                    }
                    counterColumn++;
                }
                dg.HeadersVisibility = DataGridHeadersVisibility.None;
                //rw["Gert"] = "Value2";
                dg.ItemsSource = dt.DefaultView;
                dg.Items.Refresh();
                //TextBlock tb = new TextBlock();
                //tb.Text = groups[i].GroupName;
                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(dg, i);
                Grid.SetColumn(dg, 0);
                myGrid.Children.Add(dg);
                //DataGridRow test = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(0);
                //if (test == null)
                //{
                //    // May be virtualized, bring into view and try again.
                //    dg.UpdateLayout();
                //    dg.ScrollIntoView(dg.Items[0]);
                //    test = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(0);
                //}
                //DataRowView dgr = (DataRowView)dg.Items[0];
                DataRowView drv = (DataRowView)dg.Items[0];
                Style cellStyle = new Style(typeof(DataGridCell));
                cellStyle.Setters.Add(new Setter(DataGridCell.AllowDropProperty, true));
                //itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(List_PreviewMouseLeftButtonDown)));
                cellStyle.Setters.Add(new EventSetter(DataGridCell.DropEvent, new DragEventHandler(DataGridCell_Drop)));
                //itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listbox1_Drop)));
                dg.CellStyle = cellStyle;
                dg.UpdateLayout();

                int k = 0;
            }
        }

        void dg_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is DataGrid && e.LeftButton == MouseButtonState.Pressed)
            {

                DataGrid draggedItem = sender as DataGrid;
                //draggedItem.IsSelected = true;
                DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Copy);
            }
        }

        private void DisableDragAndDropFirstRow(DataGrid dg)
        {
            int i = 0;
            while(i < 4)
            {
                DataGridCell dgc = dg.GetCell(0, i);
                dgc.AllowDrop = false;
            }
        }

        private void dg_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            Color colorGroupRow = (Color)ColorConverter.ConvertFromString("#97CBFF");
            Color colorItemRow = (Color)ColorConverter.ConvertFromString("#E4F1FF");
            int index = e.Row.GetIndex();
            if (index == 0)
                e.Row.Background = new SolidColorBrush(colorGroupRow);
            //else if (index == 1)
            //    e.Row.Background = Brushes.Red;
            //else if (index == 2)
            //    e.Row.Background = Brushes.White;
            else
            {
                e.Row.Background = new SolidColorBrush(colorItemRow);
            }
        }

        private void DataGridCell_Drop(object sender, DragEventArgs e)
        {



            DataGridCell source = sender as DataGridCell;
            ListBoxItem test = e.Data.GetData("System.Windows.Controls.ListBoxItem") as ListBoxItem;
            ToolboxItem tbi = (ToolboxItem)test.Content;
            source.DataContext = tbi;
            source.Content = tbi.Header;
            //var test = (ToolboxItem)e.Source;
            int i = 1;
            //ListView listView = sender as ListView;
            //listView.Items.Add(contact);

        }
    }
}
