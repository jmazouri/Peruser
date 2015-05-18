using System.Windows;
using MahApps.Metro.Controls;

namespace Peruser
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : MetroWindow
    {
        public PeruserConfig Config
        {
            get { return PeruserConfig.Current; }
            set { PeruserConfig.Current = value; }
        }

        public OptionsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
