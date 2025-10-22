using Microsoft.UI.Xaml;

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
        }
    }
}
