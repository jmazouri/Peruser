using System;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ChanLibrary
{
    public partial class ChooseThread : MetroWindow
    {
        public ChooseThread()
        {
            InitializeComponent();
            AllowsTransparency = true;
        }

        public string Thread { get; set; }
        public string Board { get; set; }

        async void DoInput()
        {
            string input = await this.ShowInputAsync("4Chan", "Input Thread URL", new MetroDialogSettings
            {
                ColorScheme = MetroDialogColorScheme.Accented
            });
            
            VerifyUrl(input);
        }

        private void VerifyUrl(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                DialogResult = false;
                return;
            }

            try
            {
                Uri threadUri = new Uri(input);
                Thread = threadUri.Segments[3].TrimEnd('/');
                Board = threadUri.Segments[1].TrimEnd('/');

                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't parse URL. Make sure it's a thread.");
            }
        }

        private void ChooseThread_OnLoaded(object sender, RoutedEventArgs e)
        {
            DoInput();
        }
    }
}
