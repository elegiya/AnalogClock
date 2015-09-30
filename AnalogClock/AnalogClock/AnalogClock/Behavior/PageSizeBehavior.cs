using System;
using AnalogClock.Views;
using Xamarin.Forms;

namespace AnalogClock.Behavior
{
    public class PageSizeBehavior : Behavior<ContentPage>
    {
        protected override void OnAttachedTo(ContentPage page)
        {
            base.OnAttachedTo(page);
            page.SizeChanged += OnSizeChanged;
        }

        private async void OnSizeChanged(object sender, EventArgs e)
        {
            var page = (AnalogClockPage) sender;
            Point center = new Point(page.Width/2, page.Height/2);
            double radius = 0.45*Math.Min(page.Width, page.Height);

            InitializeViewBoxes(radius, center, page.tickMarks);

            var Layout = PositionHands(radius, center);

            Layout(page.secondHand, page.secondParams);
            Layout(page.minuteHand, page.minuteParams);
            Layout(page.hourHand, page.hourParams);
        }

        private static Action<BoxView, HandParams> PositionHands(double radius, Point center)
        {
            Action<BoxView, HandParams> Layout = (boxView, handParams) =>
            {
                double width = handParams.Width*radius;
                double height = handParams.Height*radius;
                double offset = handParams.Offset;

                AbsoluteLayout.SetLayoutBounds(boxView,
                    new Rectangle(center.X - 0.5*width,
                        center.Y - offset*height,
                        width, height));

                boxView.AnchorX = 0.51;
                boxView.AnchorY = handParams.Offset;
            };
            return Layout;
        }

        private void InitializeViewBoxes(double radius, Point center, BoxView[] tickMarks)
        {
            for (int i = 0; i < tickMarks.Length; i++)
            {
                double size = radius/(i%5 == 0 ? 15 : 30);
                double radians = i*2*Math.PI/tickMarks.Length;
                double x = center.X + radius*Math.Sin(radians) - size/2;
                double y = center.Y - radius*Math.Cos(radians) - size/2;
                AbsoluteLayout.SetLayoutBounds(tickMarks[i], new Rectangle(x, y, size, size));

                tickMarks[i].AnchorX = 0.51;
                tickMarks[i].AnchorY = 0.51;
                tickMarks[i].Rotation = 180*radians/Math.PI;
            }
        }
    }
}
