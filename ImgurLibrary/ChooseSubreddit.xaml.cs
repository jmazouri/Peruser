using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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