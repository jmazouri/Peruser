using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Peruser.Annotations;

namespace Peruser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, INotifyPropertyChanged
    {
        public ImageBrowser Browser { get; set; }
        public ObservableCollection<IImageLibrary> Libraries { get; set; }
            
        Configuration Configuration { get; set; }

        private bool IsMuted = true;

        public string MediaDurationFormatted
        {
            get
            {
                if (MediaPlayerElement.MediaDuration == 0)
                {
                    return "00:00/00:00";
                }

                if (MediaPlayerElement.MediaPosition > MediaPlayerElement.MediaDuration)
                {
                    MediaPlayerElement.MediaPosition = 0;
                }

                TimeSpan position = new TimeSpan(MediaPlayerElement.MediaPosition);
                TimeSpan duration = new TimeSpan(MediaPlayerElement.MediaDuration);

                return position.ToString(@"mm\:ss") + "/" + duration.ToString(@"mm\:ss");
                
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

            Browser = new ImageBrowser();

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

        private void LibraryTreeList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var value = e.NewValue as IImageLibrary;
            if (value != null)
            {
                Browser.SetLibrary(value);
            }
        }

        private void MediaPlayerElement_MouseDown(object sender, MouseEventArgs e)
        {
            //This is the only way to get the treeview to unfocus
            MuteButton.Focus();
        }

        /*
        private void LoadRedditButton_OnClick(object sender, RoutedEventArgs e)
        {
            ChooseSubreddit dialog = new ChooseSubreddit();
            if (dialog.ShowDialog() == true)
            {
                ImgurLibrary newImgurLibrary = new ImgurLibrary(dialog.Subreddit, Configuration);

                if (newImgurLibrary.Images.Count == 0)
                {
                    MessageBox.Show("Error: Subreddit \""+dialog.Subreddit+"\" has no images. Omit the /r/ bit, if it's there.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                Libraries.Add(newImgurLibrary);
                Browser.SetLibrary(newImgurLibrary);
            }
        }
        */

        private void AddLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var imageLibrary = button.DataContext as IImageLibrary;
            if (imageLibrary != null)
            {
                
            }
            /*
             * Libraries.Add(newLibrary);
             * Browser.SetLibrary(newLibrary);
            */
        }
    }
}
