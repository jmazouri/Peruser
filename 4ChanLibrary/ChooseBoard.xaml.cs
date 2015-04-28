using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json.Linq;
using Peruser;

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
