using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameGate
{
    public sealed partial class MainWindow : Window
    {
        public List<Game> Games { get; set; }
        private SortingOptions sortingOption = SortingOptions.Random;
        public MainWindow()
        {
            this.InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;

            Task task = GetGames();
        }

        private async Task GetGames()
        {
            ShowProgressRing();

            Games = new List<Game>();

            Games.AddRange(await GameService.GetSteamGames());

            await Task.Delay(500);

            if (Games.Count == 0)
            {
                InfoTextBlock.Visibility = Visibility.Visible;
                HideProgressRing();
                return;
            }

            SortGames();

            HideProgressRing();
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Game game)
            {
                game.Launch();
            }
        }

        private void ShowProgressRing()
        {
            GamesProgressRing.IsActive = true;
            GamesScrollViewer.Visibility = Visibility.Collapsed;
        }

        private void HideProgressRing()
        {
            GamesProgressRing.IsActive = false;
            GamesScrollViewer.Visibility = Visibility.Visible;
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await GetGames();
        }

        private void ColumnsNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (GamesUniformGridLayout != null)
            {
                if ((int)ColumnsNumberBox.Value > Games.Count)
                {
                    ColumnsNumberBox.Value = Games.Count;
                    ColumnsNumberBox.Maximum = Games.Count;
                }

                GamesUniformGridLayout.MaximumRowsOrColumns = (int)ColumnsNumberBox.Value;
            }

        }

        private void SortMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuFlyoutItem)
            {
                switch (menuFlyoutItem.Tag)
                {
                    case "az":
                        sortingOption = SortingOptions.AtoZ;
                        break;
                    case "za":
                        sortingOption = SortingOptions.ZtoA;
                        break;
                    case "random":
                        sortingOption = SortingOptions.Random;
                        break;
                    case "hours_played_least":
                        sortingOption = SortingOptions.HoursPlayedLeast;
                        break;
                    case "hours_played_most":
                        sortingOption = SortingOptions.HoursPlayedMost;
                        break;
                }

                SortGames();
            }
        }

        private void SortGames()
        {
            if (Games.Count < 1)
                return;

            switch (sortingOption)
            {
                case SortingOptions.AtoZ:
                    Games = Games.OrderBy(x => x.Name).ToList();
                    break;
                case SortingOptions.ZtoA:
                    Games = Games.OrderByDescending(x => x.Name).ToList();
                    break;
                case SortingOptions.Random:
                    Games = Games.OrderBy(x => Random.Shared.Next()).ToList();
                    break;
                case SortingOptions.HoursPlayedLeast:
                    break;
                case SortingOptions.HoursPlayedMost:
                    break;
            }

            GamesItemsRepeater.ItemsSource = Games;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuFlyoutItem && menuFlyoutItem.DataContext is Game game)
            {
                DisplayAboutDialog(game);
            }
        }

        private async void DisplayAboutDialog(Game game)
        {
            BitmapImage image = new BitmapImage { UriSource = new Uri(game.HeaderPath) };
            Image headerImage = new Image { Source = image };

            Border headerImageBorder = new Border { CornerRadius = new CornerRadius(8) };

            headerImageBorder.Child = headerImage;

            StackPanel mainStackPanel = new StackPanel { Orientation = Orientation.Vertical, Spacing = 8 };
            StackPanel titleStackPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };

            TextBlock gameTitleTextBlock = new TextBlock { Text = game.Name };
            TextBlock launcherTextBlock = new TextBlock { Text = game.GetLauncherName() };
            launcherTextBlock.Foreground = new SolidColorBrush { Color = Colors.SkyBlue };

            titleStackPanel.Children.Add(gameTitleTextBlock);
            titleStackPanel.Children.Add(launcherTextBlock);

            gameTitleTextBlock.FontSize = 24;

            launcherTextBlock.FontSize = 16;
            launcherTextBlock.FontWeight = FontWeights.Bold;
            launcherTextBlock.VerticalAlignment = VerticalAlignment.Center;

            mainStackPanel.Children.Add(headerImageBorder);
            mainStackPanel.Children.Add(titleStackPanel);

            ContentDialog aboutDL = new ContentDialog()
            {
                XamlRoot = rootGrid.XamlRoot,
                Content = mainStackPanel,
                CloseButtonText = "Ok"
            };

            await aboutDL.ShowAsync();
        }

        private void OpenGameFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
