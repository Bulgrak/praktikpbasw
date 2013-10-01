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
    public class DragAdorner : Adorner
    {
        public TextBlock TB { get; set; }

        public DragAdorner(UIElement adornedElement, Point offset)

            : base(adornedElement)
        {

            this.offset = offset;

            //vbrush = new VisualBrush(AdornedElement);
            ListBoxItem lbi = (ListBoxItem)adornedElement;
            ToolboxItem tbi = (ToolboxItem)lbi.DataContext;
            TB = new TextBlock();
            TB.Text = tbi.Header;
            TB.Background = Brushes.Yellow;
            vbrush = new VisualBrush(TB);
            vbrush.Opacity = .5;
            IsHitTestVisible = false;

        }



        public void UpdatePosition(Point location)
        {

            this.location = location;

            this.InvalidateVisual();

        }



        protected override void OnRender(DrawingContext dc)
        {

            var p = location;

            p.Y = p.Y + 20;

            //dc.DrawRectangle(vbrush, null, new Rect(p, this.RenderSize));
            //dc.DrawRectangle(vbrush, null, new Rect(p, new Size(100, 100)));
                FormattedText formattedText = new FormattedText(
        TB.Text,
        CultureInfo.GetCultureInfo("en-us"),
        FlowDirection.LeftToRight,
        new Typeface("Verdana"),
        20,
        Brushes.Black);
            dc.DrawText(formattedText, p);
        }



        private Brush vbrush;

        private Point location;

        private Point offset;
    }
}
