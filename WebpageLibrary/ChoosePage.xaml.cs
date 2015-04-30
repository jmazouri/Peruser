using System.ComponentModel.DataAnnotations;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

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
