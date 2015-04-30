using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ImgurLibrary
{
    /// <summary>
    /// Interaction logic for ChooseSubreddit.xaml
    /// </summary>
    public partial class ChooseSubreddit : MetroWindow
    {
        public ChooseSubreddit()
        {
            InitializeComponent();
            AllowsTransparency = true;
        }

        async void DoInput()
        {
            Subreddit = await this.ShowInputAsync("Choose a Subreddit", "", new MetroDialogSettings
            {
                ColorScheme = MetroDialogColorScheme.Accented
            });
            DialogResult = true;
            
        }

        public string Subreddit { get; private set; }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoInput();
        }
    }
}