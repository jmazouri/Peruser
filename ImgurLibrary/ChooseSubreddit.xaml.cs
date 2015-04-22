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

namespace ImgurLibrary
{
    /// <summary>
    /// Interaction logic for ChooseSubreddit.xaml
    /// </summary>
    public partial class ChooseSubreddit : Window
    {
        public ChooseSubreddit()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string Subreddit
        {
            get { return SubredditBox.Text; }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
            }
        }
    }
}
