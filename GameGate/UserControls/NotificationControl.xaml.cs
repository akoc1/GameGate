using GameGate.Models;
using GameGate.Services;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace GameGate.UserControls
{
    public sealed partial class NotificationControl : UserControl
    {
        private Storyboard? _currentProgressStoryboard;
        private Storyboard? _currentOpenStoryboard;
        private Storyboard? _currentCloseStoryboard;

        public NotificationControl()
        {
            InitializeComponent();
            NotificationService.Instance.OnNotify += ShowNotification;
        }

        private void ShowNotification(NotificationMessage msg)
        {
            // stop prev anims
            _currentProgressStoryboard?.Stop();
            _currentOpenStoryboard?.Stop();
            _currentCloseStoryboard?.Stop();

            switch (msg.NotificationType)
            {
                case NotificationType.Info:
                    IndicatorRectangle.Fill = new SolidColorBrush(Colors.DodgerBlue);
                    break;
                case NotificationType.Warning:
                    IndicatorRectangle.Fill = new SolidColorBrush(Colors.Orange);
                    break;
                case NotificationType.Error:
                    IndicatorRectangle.Fill = new SolidColorBrush(Colors.Crimson);
                    break;
            }

            MessageText.Text = msg.Message;
            ProgressBar.Value = 0;

            var openAnim = new Storyboard();
            _currentOpenStoryboard = openAnim;

            var opacityAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(opacityAnim, RootGrid);
            Storyboard.SetTargetProperty(opacityAnim, "Opacity");

            var translateAnim = new DoubleAnimation
            {
                From = 20,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(translateAnim, RootTransform);
            Storyboard.SetTargetProperty(translateAnim, "Y");

            openAnim.Children.Add(opacityAnim);
            openAnim.Children.Add(translateAnim);

            RootGrid.Visibility = Visibility.Visible;
            openAnim.Begin();

            var progressAnim = new DoubleAnimation
            {
                From = 100,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1.75)),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(progressAnim, ProgressBar);
            Storyboard.SetTargetProperty(progressAnim, "Value");

            var progressStoryboard = new Storyboard();
            _currentProgressStoryboard = progressStoryboard;
            progressStoryboard.Children.Add(progressAnim);

            progressStoryboard.Completed += (s, e) => CloseNotification();
            progressStoryboard.Begin();
        }

        private void CloseNotification()
        {
            _currentCloseStoryboard?.Stop();

            var closeAnim = new Storyboard();
            _currentCloseStoryboard = closeAnim;

            var opacityAnim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(opacityAnim, RootGrid);
            Storyboard.SetTargetProperty(opacityAnim, "Opacity");

            var translateAnim = new DoubleAnimation
            {
                From = 0,
                To = 20,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(translateAnim, RootTransform);
            Storyboard.SetTargetProperty(translateAnim, "Y");

            closeAnim.Children.Add(opacityAnim);
            closeAnim.Children.Add(translateAnim);

            closeAnim.Completed += (s, e) => RootGrid.Visibility = Visibility.Collapsed;

            closeAnim.Begin();
        }
    }
}
