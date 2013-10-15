using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace TreatPraktik.View
{
    public class DragAdornerGroup : Adorner
    {
        private string Text { get; set; }
        private Point _location;
        private Point _offset;
        private VisualBrush _vbrush;

        public DragAdornerGroup(UIElement adornedElement, Point offset)

            : base(adornedElement)
        {
            _offset = offset;
            _vbrush = new VisualBrush(adornedElement);

            IsHitTestVisible = false;
        }

        public void UpdatePosition(Point location)
        {
            _location = location;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            Point p = _location;
            p.Offset(-_offset.X, -_offset.Y);

            dc.DrawRectangle(_vbrush, null, new Rect(p, this.RenderSize));
        }
    }
}
