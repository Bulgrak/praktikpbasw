using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TreatPraktik.Model;

namespace TreatPraktik.View
{
    public class DragAdornerToolboxItem : Adorner
    {
        private string Text { get; set; }
        private Point _location;
        private Point _offset;

        public DragAdornerToolboxItem(UIElement adornedElement, Point offset)

            : base(adornedElement)
        {
            _offset = offset;
            ListBoxItem lbi = (ListBoxItem)adornedElement;
            if (lbi.DataContext is ToolboxItem)
            {
                ToolboxItem tbi = (ToolboxItem) lbi.DataContext;
                Text = tbi.Header;
            }
            else if (lbi.DataContext is ToolboxGroup)
            {
                ToolboxGroup tbi = (ToolboxGroup)lbi.DataContext;
                Text = tbi.Group.GroupHeader;
                
            }
            IsHitTestVisible = false;
        }

        public void UpdatePosition(Point location)
        {
            _location = location;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            var p = _location;
            p.Y = p.Y + 20;

            FormattedText formattedText = new FormattedText(
                    Text,
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    20,
                    Brushes.Black);
            dc.DrawText(formattedText, p);
        }
    }
}
