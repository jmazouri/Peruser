using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;

namespace ChanLibrary
{
    public partial class ChooseBoard : MetroWindow
    {
        public ChooseBoard()
        {
            WebClient wc = new WebClient();
            string jsondata = wc.DownloadString("http://a.4cdn.org/boards.json");

            JObject jo = JObject.Parse(jsondata);

            var firstPosts = jo.GetValue("boards").ToList();

            BoardList = firstPosts.Select(d => d.ToObject<ChanBoard>()).ToList();

            InitializeComponent();
            AllowsTransparency = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetBoard();
        }

        public ChanBoard Board { get; set; }

        public List<ChanBoard> BoardList { get; set; } 

        private void SetBoard()
        {
            Board = (BoardSelector.SelectedItem as ChanBoard);
            DialogResult = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetBoard();
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
