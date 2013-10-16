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
    /// Interaction logic for GroupContainerUserControl.xaml
    /// </summary>
    public partial class GroupContainerUserControl : UserControl
    {
        public ObservableCollection<GroupTypeOrder> GtoObsCollection { get; set; }

        public GroupContainerUserControl()
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
            while (j < GtoObsCollection.Count)
            {
                if (gto.GroupTypeID.Equals(GtoObsCollection[j].GroupTypeID))
                {
                    departmentList.Add(GtoObsCollection[j].DepartmentID);
                }
                j++;
            }

            return departmentList;
        }

        public bool IsGroupsOccuringMultipleTimes(GroupType gt)
        {
            int count = GtoObsCollection.Count(gto => gto.GroupTypeID == gt.GroupTypeID);
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
            while (i < GtoObsCollection.Count && !stop)
            {
                if (!GtoObsCollection[i].GroupTypeID.Equals(gto.GroupTypeID))
                    stop = true;
                else
                    i++;
            }
            return i;
        }

        public void CreateGroupTables()
        {
            int skipped = 0;
            for (int i = 0; i < GtoObsCollection.Count; i++)
            {
                GroupType gt = GtoObsCollection[i].Group;
                GroupTableUserControl groupTableUserControl = new GroupTableUserControl{ DataContext = GtoObsCollection[i] };

                TextBlock tbGroupNumber = new TextBlock
                {
                    DataContext = GtoObsCollection[i],
                    FontSize = 18,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.LightSlateGray
                };
                tbGroupNumber.SetBinding(TextBlock.TextProperty, "GroupOrder");
                TextBlock tbDepartNumber = new TextBlock
                {
                    DataContext = GtoObsCollection[i],
                    FontSize = 12,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.DarkSlateGray
                };
                if (IsGroupsOccuringMultipleTimes(gt))
                {
                    int initialValue = i;
                    i = GetIndexLastOccurrence(GtoObsCollection[i], i);
                    skipped = skipped + (i - initialValue);
                    groupTableUserControl.ParentGroupContainerUserControl = this;
                    groupTableUserControl.PopulateGroupTable(gt);
                    List<string> deptNumbers = GetDepartments(initialValue, GtoObsCollection[initialValue]);

                    tbDepartNumber.Text = "Dept: " + String.Join(",", deptNumbers);
                }
                else
                {
                    groupTableUserControl.ParentGroupContainerUserControl = this;
                    groupTableUserControl.PopulateGroupTable(gt);
                    tbDepartNumber.Text = "Dept: " + GtoObsCollection[i].DepartmentID;
                }

                RowDefinition rd = new RowDefinition();
                myGrid.RowDefinitions.Add(rd);
                Grid.SetRow(groupTableUserControl, i - skipped);
                Grid.SetColumn(groupTableUserControl, 1);
                myGrid.Children.Add(groupTableUserControl);
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

                //Button btnGroupMoveUp = CreateBtnGroupMoveUp(groupTableUserControl);

                //if (i == 0)
                //{
                //    btnGroupMoveUp.IsEnabled = false;
                //}

                //Button btnGroupMoveDown = CreateBtnGroupMoveDown(groupTableUserControl);

                //if (i == GtoObsCollection.Count - 1)
                //{
                //    btnGroupMoveDown.IsEnabled = false;
                //}

                //StackPanel spBtn = CreateSPBtn();

                //spBtn.Children.Add(btnGroupMoveUp);
                //spBtn.Children.Add(btnGroupMoveDown);
                //Grid.SetRow(spBtn, i - skipped);
                //Grid.SetColumn(spBtn, 0);
                //myGrid.Children.Add(spBtn);

            }
        }

        //private Button CreateBtnGroupMoveUp(GroupTableUserControl groupTableUserControl)
        //{
        //    var uriSourceGroupMoveUpIcon = new Uri(@"/TreatPraktik;component/Ressources/Arrow-up.ico", UriKind.Relative);
        //    Image imgBtnGroupMoveUpIcon = new Image { Source = new BitmapImage(uriSourceGroupMoveUpIcon) };
        //    Button btnGroupMoveUp = new Button
        //    {
        //        Name = "btnGroupMoveUp",
        //        Height = 17,
        //        Width = 17,
        //        Content = imgBtnGroupMoveUpIcon,
        //        FontSize = 9,
        //        DataContext = groupTableUserControl.GroupTable
        //    };

        //    btnGroupMoveUp.Click += btnGroupMoveUp_Click;
        //    return btnGroupMoveUp;
        //}

        //private StackPanel CreateSPBtn()
        //{
        //    StackPanel sp = new StackPanel
        //    {
        //        Name = "spBtnUpDown",
        //        Orientation = Orientation.Vertical,
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        VerticalAlignment = VerticalAlignment.Bottom
        //    };
        //    return sp;
        //}

        //private Button CreateBtnGroupMoveDown(GroupTableUserControl groupTableUserControl)
        //{
        //    var uriSourceGroupMoveDownIcon = new Uri(@"/TreatPraktik;component/Ressources/Arrow-down.ico", UriKind.Relative);
        //    Image imgBtnGroupMoveDownIcon = new Image { Source = new BitmapImage(uriSourceGroupMoveDownIcon) };

        //    Button btnGroupMoveDown = new Button
        //    {
        //        Name = "btnGroupMoveDown",
        //        Height = 17,
        //        Width = 17,
        //        Content = imgBtnGroupMoveDownIcon,
        //        DataContext = groupTableUserControl.GroupTable,
        //        FontSize = 9
        //    };
        //    btnGroupMoveDown.Click += btnGroupMoveDown_Click;
        //    return btnGroupMoveDown;
        //}

        //private void btnGroupMoveDown_Click(object sender, RoutedEventArgs e)
        //{
        //    Button btn = sender as Button;
        //    StackPanel sp = (StackPanel)btn.Parent;
        //    Grid gridGroup = (Grid)btn.DataContext;
        //    int row = Grid.GetRow(sp);
        //    List<UIElement> uieListMoveDown = myGrid.GetAllGridCellChildrenListByRow(row);
        //    List<UIElement> uieListMoveUp = myGrid.GetAllGridCellChildrenListByRow(row + 1);
        //    MoveGroupUp(uieListMoveUp, row);
        //    MoveGroupDown(uieListMoveDown, row);
        //}

        private void MoveGroupUp(IList<UIElement> uieListMoveUp, int row)
        {
            foreach (UIElement uie in uieListMoveUp)
            {
                Grid.SetRow(uie, row);
            }
            StackPanel spDescription = (StackPanel) uieListMoveUp[0];
            TextBlock tb = (TextBlock)spDescription.Children[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            gt.GroupOrder--;
            //StackPanel spBtn = (StackPanel)uieListMoveUp[1];
            //Button btnUp = (Button)spBtn.Children[0];
            //Button btnDown = (Button)spBtn.Children[1];
            //if (gt.GroupOrder == 1)
            //{

            //    btnUp.IsEnabled = false;
            //}
            //else
            //{
            //    btnUp.IsEnabled = true;
            //}
            //btnDown.IsEnabled = true;
        }



        private void MoveGroupDown(IList<UIElement> uieListMoveDown, int row)
        {
            foreach (UIElement uie in uieListMoveDown)
            {
                Grid.SetRow(uie, row + 1);
            }
            StackPanel spDescription = (StackPanel) uieListMoveDown[0];
            TextBlock tb = (TextBlock)spDescription.Children[0];
            GroupTypeOrder gt = (GroupTypeOrder)tb.DataContext;
            gt.GroupOrder++;

            //StackPanel spBtn = (StackPanel)uieListMoveDown[1];
            //Button btnDown = (Button)spBtn.Children[1];
            //Button btnUp = (Button)spBtn.Children[0];
            //if (gt.GroupOrder == GtoObsCollection.Count)
            //{
            //    btnDown.IsEnabled = false;
            //}
            //else
            //{
            //    btnDown.IsEnabled = true;
            //}
            //btnUp.IsEnabled = true;
        }

        //private void btnGroupMoveUp_Click(object sender, RoutedEventArgs e)
        //{
        //    Button btn = sender as Button;
        //    StackPanel sp = (StackPanel)btn.Parent;
        //    int row = Grid.GetRow(sp);
        //    List<UIElement> uieListMoveDown = myGrid.GetAllGridCellChildrenListByRow(row - 1);
        //    List<UIElement> uieListMoveUp = myGrid.GetAllGridCellChildrenListByRow(row);
        //    MoveGroupUp(uieListMoveUp, row - 1);
        //    MoveGroupDown(uieListMoveDown, row - 1);
        //}

        public void MoveGroupToRow(List<UIElement> uieList, int row)
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

        public void MoveGroup(GroupTableUserControl draggedGroupTableUserControl, GroupTableUserControl targetGroupTableUserControl)
        {
            int targetRow = Grid.GetRow(targetGroupTableUserControl);
            int draggedRow = Grid.GetRow(draggedGroupTableUserControl);
            if (targetRow < draggedRow)
            {
                List<UIElement> uieListToBeMoved = myGrid.GetAllGridCellChildrenListByRow(draggedRow);
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
                int i = draggedRow + 1;
                while (i <= targetRow)
                {
                    List<UIElement> uieList = myGrid.GetAllGridCellChildrenListByRow(i);
                    MoveGroupUp(uieList, i - 1);
                    i++;
                }
                MoveGroupToRow(uieListToBeMoved, targetRow);
            }
        }
    }
}
