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

namespace ChanLibrary
{
    public partial class ChooseThread : Window
    {
        public ChooseThread()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VerifyUrl();
        }

        public string Thread { get; set; }
        public string Board { get; set; }

        private void VerifyUrl()
        {
            try
            {
                Uri threadUri = new Uri(ThreadUrlBox.Text);
                Thread = threadUri.Segments[3].TrimEnd('/');
                Board = threadUri.Segments[1].TrimEnd('/');

                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't parse URL. Make sure it's a thread.");
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VerifyUrl();
            }
        }
    }
}
