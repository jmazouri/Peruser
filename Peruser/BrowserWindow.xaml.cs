using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Peruser.Annotations;
using Peruser.ImageLibraries;

namespace Peruser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BrowserWindow : MetroWindow, INotifyPropertyChanged
    {
        public string ToastMessage { get; set; }
        public ImageBrowser Browser { get; set; }
        public ObservableCollection<ImageLibrary> Libraries { get; set; }

        private int LibraryIndex
        {
            get
            {
                return Libraries.IndexOf(Browser.CurrentLibrary);
            }
            set
            {
                if (Libraries.Count == 0) { return; }
                int libraryIndex = value;

                if (value < 0)
                {
                    libraryIndex = Libraries.Count - 1;
                }
                if (value > Libraries.Count - 1)
                {
                    libraryIndex = 0;
                }

                Browser.SetLibrary(Libraries[libraryIndex]);
                ToastMessage = Libraries[libraryIndex].Title;
                OnPropertyChanged("ToastMessage");
            }
        }
            
        private bool _isMuted;

        public string MediaDurationFormatted
        {
            get
            {
                if (MediaPlayerElement.MediaDuration == 0)
                {
                    return "";
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
            Browser = new ImageBrowser();
            Libraries = new ObservableCollection<ImageLibrary>();

            InitializeComponent();

            AllowsTransparency = true;
            
            Style buttonStyle = (Style) FindResource("MetroCircleButtonStyle");

            foreach (Type type in LibraryContainer.Container)
            {
                string imagePath = type.GetProperty("IconPath").GetValue(null) as string;

                if (imagePath == null)
                {
                    continue;
                }

                Button b = new Button
                {
                    DataContext = type,
                    Content = new Image()
                    {
                        Source = new BitmapImage(new Uri(imagePath, UriKind.Relative)),
                        Width = 16,
                        Height = 16
                    },
                    Style = buttonStyle,
                    Width = 32,
                    Height = 32
                };

                b.Click += AddLibrary_OnClick;

                AddLibraryPanel.Children.Add(b);
            }

            string cmdLine = Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();

            if (cmdLine != null)
            {
                string dirPath = cmdLine.TrimEnd('\\');

                if (Directory.Exists(dirPath))
                {
                    Libraries.Add(new LocalImageLibrary(dirPath));
                }
                else
                {
                    if (File.Exists(cmdLine))
                    {
                        Libraries.Add(new LocalImageLibrary(Path.GetDirectoryName(cmdLine)));
                        Browser.SetLibrary(Libraries.First());
                        Browser.SetIndexToPath(cmdLine);
                    }
                }
            }

            _isMuted = Configuration.Current.Mute;
            MuteButton_OnClick(null, null);
        }

        private void BrowserWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control & ModifierKeys.Shift) == (ModifierKeys.Control & ModifierKeys.Shift))
                {
                    LibraryIndex--;
                }
                else
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        LibraryIndex++;
                    }
                }
            }

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

                MediaZoombox.Focus();
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

                MediaZoombox.Focus();
            }

        }

        private void MuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isMuted)
            {
                MuteButton.Content = "unmute";
                MediaPlayerElement.Volume = 0;
            }
            else
            {
                MuteButton.Content = "mute";
                MediaPlayerElement.Volume = 30;
            }

            _isMuted = !_isMuted;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Browser.SortImages(e.AddedItems[0].ToString());
            OnPropertyChanged("Libraries");
        }

        private void LibraryTreeList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var value = e.NewValue as ImageLibrary;
            if (value != null && value != Browser.CurrentLibrary)
            {
                LibraryIndex = Libraries.IndexOf(value);
            }
            else
            {
                var newValue = e.NewValue as ImageData;
                if (newValue != null)
                {
                    LibraryIndex = Libraries.IndexOf(ImageLibrary.FindImageInLibraries(Libraries, newValue));
                    Browser.SetIndexToImage(newValue);
                }
            }
        }

        private void AddLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var imageLibrary = button.DataContext as Type;
            if (imageLibrary == null) return;

            var newLibrary = imageLibrary.GetMethod("CreateLibrary").Invoke(null, null) as ImageLibrary;
            if (newLibrary == null) return;

            Libraries.Add(newLibrary);
            Browser.SetLibrary(newLibrary);
        }

        private void FilePath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Uri.IsWellFormedUriString(Browser.CurrentImage.Path, UriKind.Absolute) || File.Exists(Browser.CurrentImage.Path))
            {
                Process.Start(Browser.CurrentImage.Path);
            }
        }

        private void LibraryTreeList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var value = LibraryTreeList.SelectedItem as ImageLibrary;
            if (value != null)
            {
                Process.Start(value.SourceUrl);
            }
            else
            {
                var newValue = LibraryTreeList.SelectedItem as ImageData;
                if (newValue == null) return;

                ImageLibrary subLibrary = Browser.CurrentLibrary.CreateSubLibrary(newValue);
                if (subLibrary == null) return;

                Libraries.Add(subLibrary);
                Browser.SetLibrary(subLibrary);
            }
        }

        private void MediaPlayerElement_OnMediaPositionChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("MediaDurationFormatted");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow window = new OptionsWindow();
            window.ShowDialog();
        }

        private void MediaPlayerElement_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            MediaPlayerElement.RenderTransform = new TranslateTransform();
            MediaZoombox.FitToBounds();
            MediaZoombox.Focus();
        }

        private void BrowserWindow_OnDeactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = Configuration.Current.AlwaysOnTop;
        }

        private void CommandBinding_DeleteLibrary(object sender, ExecutedRoutedEventArgs e)
        {
            Libraries.Remove((e.Parameter as ContentPresenter).Content as ImageLibrary);
            Browser.SetLibrary(Libraries.Any() ? Libraries[0] : null);
            OnPropertyChanged("Libraries");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
