using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using Peruser.Annotations;
using Peruser.ImageLibraries;

namespace Peruser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, INotifyPropertyChanged
    {
        public ImageBrowser Browser { get; set; }
        Configuration Configuration { get; set; }

        private bool IsMuted = true;
        private bool IsLooping = true;

        public string MediaDurationFormatted
        {
            get
            {
                if (MediaPlayerElement.MediaDuration == 0)
                {
                    return "00:00/00:00";
                }

                TimeSpan position = new TimeSpan(MediaPlayerElement.MediaPosition);
                TimeSpan duration = new TimeSpan(MediaPlayerElement.MediaDuration);

                if (position > duration)
                {
                    return duration.ToString(@"mm\:ss") + duration.ToString(@"mm\:ss");
                }
                else
                {
                    return position.ToString(@"mm\:ss") + "/" + duration.ToString(@"mm\:ss");
                }
                
            }
        }

        public BrowserWindow()
        {
            if (File.Exists("config.json"))
            {
                Configuration = Configuration.Deserialize(File.ReadAllText("config.json"));
            }
            else
            {
                File.WriteAllText("config.json", Configuration.Serialize(new Configuration
                {
                    DefaultSort = "Date Descending",
                    AllowedFileTypes = new[] { "webm", "jpg", "gif", "png", "jpeg", "bmp", "mp4", "avi", "mkv", "flv" }
                }));

                Configuration = Configuration.Deserialize(File.ReadAllText("config.json"));
            }

            Browser = new ImageBrowser(new LocalImageLibrary(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), Configuration));
            InitializeComponent();

            Timer t = new Timer(200);
            t.Elapsed += delegate
            {
                OnPropertyChanged("MediaDurationFormatted");
            };
            t.Start();
        }

        private void BrowserWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    if (MediaPlayerElement.MediaPosition + TimeSpan.FromSeconds(3).Ticks > MediaPlayerElement.MediaDuration)
                    {
                        MediaPlayerElement.MediaPosition = MediaPlayerElement.MediaPosition;
                    }
                    else
                    {
                        MediaPlayerElement.MediaPosition += TimeSpan.FromSeconds(3).Ticks;
                    }
                }
                else
                {
                    Browser.NextImage();
                }
                
            }

            if (e.Key == Key.Left)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    if (MediaPlayerElement.MediaPosition - TimeSpan.FromSeconds(3).Ticks < 0)
                    {
                        MediaPlayerElement.MediaPosition = 0;
                    }
                    else
                    {
                        MediaPlayerElement.MediaPosition -= TimeSpan.FromSeconds(3).Ticks;
                    }
                }
                else
                {
                    Browser.PrevImage();
                }
                
            }

            e.Handled = true;
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog {IsFolderPicker = true};

            if (dialog.ShowDialog() != CommonFileDialogResult.Cancel)
            {
                Browser.SetLibrary(new LocalImageLibrary(dialog.FileName, Configuration));
                //Browser.SetPath(dialog.FileName);
            }
        }

        private void MuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsMuted)
            {
                MuteButton.Content = "Unmute";
                MediaPlayerElement.Volume = 0;
            }
            else
            {
                MuteButton.Content = "Mute";
                MediaPlayerElement.Volume = 30;
            }

            IsMuted = !IsMuted;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Browser.SortImages(SortBox.Text);
            Configuration.DefaultSort = SortBox.Text;
            File.WriteAllText("config.json", Configuration.Serialize(Configuration));
            Focus();
        }
    }
}
