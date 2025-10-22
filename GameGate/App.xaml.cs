using GameGate.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GameGate
{
    public partial class App : Application
    {
        private Window m_window;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            Frame rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));
            m_window.Content = rootFrame;

            m_window.Activate();
        }
    }
}
