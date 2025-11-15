using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ConquiánCliente.View.Game.Behaviors
{
    public class DragAdorner : Adorner
    {
        private UIElement child;
        private Point currentPosition;
        private Point startOffset;

        public DragAdorner(UIElement adornedElement, UIElement visual, Point startPoint) : base(adornedElement)
        {
            this.child = visual;
            this.currentPosition = startPoint;
            this.startOffset = new Point(20, 20);
            this.IsHitTestVisible = false;
            AddVisualChild(child);
        }

        protected override int VisualChildrenCount => 1;
        protected override Visual GetVisualChild(int index) => child;

        protected override Size MeasureOverride(Size constraint)
        {
            child.Measure(constraint);
            return child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            child.Arrange(new Rect(new Point(0, 0), child.DesiredSize));
            return child.DesiredSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(currentPosition.X - startOffset.X, currentPosition.Y - startOffset.Y));
            return result;
        }

        public void UpdatePosition(Point position)
        {
            this.currentPosition = position;
            var layer = this.Parent as AdornerLayer;
            if (layer != null)
            {
                layer.Update(AdornedElement);
            }
        }
    }
}
