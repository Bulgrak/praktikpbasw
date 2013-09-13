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
                TextBlock tb = new TextBlock();
                tb.Text = groups[i].GroupName;
                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(dg, i);
                Grid.SetColumn(dg, 0);
                myGrid.Children.Add(dg);
            }
        }
    }
}
