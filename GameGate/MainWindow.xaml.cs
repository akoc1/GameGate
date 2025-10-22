using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using Windows.UI;

namespace GameGate
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeWindowProperties();
        }

        private void InitializeWindowProperties()
        {
            ExtendsContentIntoTitleBar = true;

            AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icons/favicon.ico"));
            AppWindow.SetTitleBarIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icons/favicon.ico"));
            AppWindow.SetTaskbarIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icons/favicon.ico"));

            var primary = (Color)Application.Current.Resources["PrimaryColor"];
            var primaryDark = (Color)Application.Current.Resources["PrimaryDarkColor"];

            AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
            //AppWindow.TitleBar.ButtonHoverBackgroundColor = primary;
            //AppWindow.TitleBar.ButtonPressedBackgroundColor = primaryDark;
        }
    }
}
