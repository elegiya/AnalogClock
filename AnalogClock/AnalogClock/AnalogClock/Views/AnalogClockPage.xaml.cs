using System;

using Xamarin.Forms;

namespace AnalogClock.Views
{
    public partial class AnalogClockPage : ContentPage
    {
        public HandParams secondParams;
        public HandParams minuteParams;
        public HandParams hourParams;

        public BoxView[] tickMarks;
        public BoxView secondHand, minuteHand, hourHand;
        
        public AnalogClockPage()
        {
            InitializeComponent();

            secondParams = new HandParams(0.02, 1.1, 0.85);
            minuteParams = new HandParams(0.05, 0.8, 0.9);
            hourParams = new HandParams(0.125, 0.65, 0.9);

            tickMarks = new BoxView[60];
            SetContent();
            
            Device.StartTimer(TimeSpan.FromMilliseconds(16), OnTimerTick);
        }

        private void SetContent()
        {
            for (int i = 0; i < tickMarks.Length; i++)
            {
                tickMarks[i] = new BoxView
                {
                    Color = Color.Yellow
                };
                absoluteLayout.Children.Add(tickMarks[i]);
            }
            
            absoluteLayout.Children.Add(hourHand =
                new BoxView
                {
                    Color = Color.Lime
                });
            absoluteLayout.Children.Add(minuteHand =
                new BoxView
                {
                    Color = Color.Green
                });
            absoluteLayout.Children.Add(secondHand =
                new BoxView
                {
                    Color = Color.Teal
                });

            Content = absoluteLayout;
        }

        bool OnTimerTick()
        {
            DateTime dateTime = DateTime.Now;
            hourHand.Rotation = 30 * (dateTime.Hour % 12) + 0.5 * dateTime.Minute;
            minuteHand.Rotation = 6 * dateTime.Minute + 0.1 * dateTime.Second;
            
            double seconds = dateTime.Millisecond / 1000.0;
            if (seconds < 0.5)
            {
                seconds = 0.5 * Easing.SpringIn.Ease(seconds / 0.5);
            }
            else
            {
                seconds = 0.5 * (1 + Easing.SpringOut.Ease((seconds - 0.5) / 0.5));
            }
            secondHand.Rotation = 6 * (dateTime.Second + seconds);
            return true;
        }
    }
}
