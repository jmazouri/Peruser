using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
using MahApps.Metro.Controls.Dialogs;
using Peruser;

namespace WebpageLibrary
{
    public partial class ChoosePage : MetroWindow
    {
        public WebpageConfig Config { get; set; }

        public ChoosePage()
        {
            Config = new WebpageConfig();
            InitializeComponent();
            AllowsTransparency = true;
        }

        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //Stupid no async in catch blocks...
            bool hasBroken = false;

            try
            {
                Validator.ValidateObject(Config, new ValidationContext(Config));

                await this.ShowMessageAsync("Looks good", "This might take a second.", MessageDialogStyle.Affirmative, new MetroDialogSettings
                {
                    ColorScheme = MetroDialogColorScheme.Accented,
                    AffirmativeButtonText = "Alright"
                });

                DialogResult = true;
            }
            catch (ValidationException)
            {
                hasBroken = true;
            }

            if (hasBroken)
            {
                await this.ShowMessageAsync("Error", "Invalid input. Make sure you've specified a URL.", MessageDialogStyle.Affirmative, new MetroDialogSettings
                {
                    ColorScheme = MetroDialogColorScheme.Accented
                });
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
