using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TreatPraktik.View
{
    public static class GridExtensions
    {

        /// <summary>
        /// Gets the contents of a cell by row number and column number
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static UIElement GetGridCellChildren(this Grid grid, int row, int col)
        {
            return grid.Children.Cast<UIElement>().First(
                                x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
        }

        public static List<UIElement> GetGridCellChildrenList(this Grid grid, int row, int col)
        {
            return grid.Children.Cast<UIElement>().Where(
                                x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col).ToList();
        }

        public static List<UIElement> GetGridCellChildrenByRow(this Grid grid, int row)
        {
            List<UIElement> uieList = new List<UIElement>();
            int i = 0;
            while (i < grid.ColumnDefinitions.Count)
            {
                UIElement uie = grid.GetGridCellChildren(row, i);
                uieList.Add(uie);
                i++;
            }
            return uieList;
        }

        /// <summary>
        /// Updates items row position in the grid from a certain row number
        /// </summary>
        /// <param name="myGrid"></param>
        /// <param name="row"></param>
        public static void UpdateCellContentsRowPosition(this Grid grid, int row)
        {
            if (row < grid.RowDefinitions.Count)
            {
                for (int i = row + 1; i <= grid.RowDefinitions.Count; i++)
                {
                    for (int j = 0; j < grid.ColumnDefinitions.Count; j++)
                    {
                        UIElement uie = grid.GetGridCellChildren(i, j);
                        //List<UIElement> children = grid.GetGridCellChildren(i, j);
                        //foreach (UIElement uie in children)
                        //{
                        Grid.SetRow(uie, i - 1);
                        //}
                    }
                }
            }
        }

        public static void UpdateCellContentsRowPositionAlternative(this Grid grid, int row)
        {
            if (row < grid.RowDefinitions.Count)
            {
                for (int i = row + 1; i <= grid.RowDefinitions.Count; i++)
                {
                    for (int j = 0; j < grid.ColumnDefinitions.Count; j++)
                    {
                        List<UIElement> children = grid.GetGridCellChildrenList(i, j);
                        foreach (UIElement uie in children)
                        {
                            Grid.SetRow(uie, i - 1);
                        }
                    }
                }
            }
        }

        public static void RemoveGridCellChildrenByRow(this Grid grid, int row)
        {
            List<UIElement> elementsToRemove = new List<UIElement>();

            foreach (UIElement element in grid.Children)
            {
                if (Grid.GetRow(element) == row)
                    elementsToRemove.Add(element);
            }
            foreach (UIElement element in elementsToRemove)
                grid.Children.Remove(element);
        }


        /// <summary>
        /// Gets the content of a cell in the grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public static UIElement GetCellChild(this Grid grid, int row, int column)
        {
            UIElement uie = grid.Children
                .Cast<UIElement>()
                .First(a => Grid.GetRow(a) == row && Grid.GetColumn(a) == column);
            return uie;
        }
        
        /// <summary>
        /// Removes all children, rows and columns
        /// </summary>
        /// <param name="grid"></param>
        public static void ClearGrid(this Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
        }

        public static void RemoveRow(this Grid grid, int row)
        {
            grid.RemoveGridCellChildrenByRow(row);
            grid.RowDefinitions.RemoveAt(row);
            grid.UpdateCellContentsRowPosition(row);
        }
    }
}
