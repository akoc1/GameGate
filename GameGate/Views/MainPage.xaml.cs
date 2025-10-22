using GameGate.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GameGate.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; private set; }

        public MainPage()
        {
            InitializeComponent();

            ViewModel = new MainPageViewModel();
            DataContext = ViewModel;
        }
    }
}
