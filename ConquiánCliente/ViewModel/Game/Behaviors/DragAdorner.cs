using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ConquiánCliente.ViewModel.Game.Behaviors
{
    public class DragAdorner : Adorner
    {
        private const double DEFAULT_OFFSET_X = 20.0;
        private const double DEFAULT_OFFSET_Y = 20.0;
        private const double ORIGIN_COORDINATE = 0.0;
        private const int SINGLE_CHILD_COUNT = 1;

        private readonly UIElement child;
        private Point currentPosition;
        private Point startOffset;

        public DragAdorner(UIElement adornedElement, UIElement visual, Point startPoint) : base(adornedElement)
        {
            this.child = visual;
            this.currentPosition = startPoint;
            this.startOffset = new Point(DEFAULT_OFFSET_X, DEFAULT_OFFSET_Y);
            this.IsHitTestVisible = false;

            AddVisualChild(child);
        }

        protected override int VisualChildrenCount
        {
            get
            {
                int count = SINGLE_CHILD_COUNT;
                return count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            Visual visualChild = this.child;
            return visualChild;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this.child.Measure(constraint);

            Size desiredSize = this.child.DesiredSize;
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Point origin = new Point(ORIGIN_COORDINATE, ORIGIN_COORDINATE);
            Size desiredSize = this.child.DesiredSize;
            Rect arrangeRect = new Rect(origin, desiredSize);

            this.child.Arrange(arrangeRect);

            return desiredSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup transformGroup = new GeneralTransformGroup();
            GeneralTransform baseTransform = base.GetDesiredTransform(transform);

            double offsetX = this.currentPosition.X - this.startOffset.X;
            double offsetY = this.currentPosition.Y - this.startOffset.Y;
            TranslateTransform translation = new TranslateTransform(offsetX, offsetY);

            transformGroup.Children.Add(baseTransform);
            transformGroup.Children.Add(translation);

            return transformGroup;
        }

        public void UpdatePosition(Point position)
        {
            this.currentPosition = position;
            RefreshLayer();
        }

        private void RefreshLayer()
        {
            AdornerLayer layer = this.Parent as AdornerLayer;

            if (layer != null)
            {
                layer.Update(this.AdornedElement);
            }
        }
    }
}
