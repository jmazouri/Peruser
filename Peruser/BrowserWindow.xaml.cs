using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Peruser.Annotations;
using Peruser.ImageLibraries;

namespace Peruser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, INotifyPropertyChanged
    {
        public string ToastMessage { get; set; }
        public ImageBrowser Browser { get; set; }
        public ObservableCollection<ImageLibrary> Libraries { get; set; }
        private float _imageScale = 1;

        private int LibraryIndex
        {
            get
            {
                //Browser.SetLibrary(Libraries[Libraries.IndexOf(Browser.CurrentLibrary) + 1]);
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

        void LoadSaveConfiguration()
        {
            string pathToConfig = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");
            if (!File.Exists(pathToConfig))
            {
                File.WriteAllText(pathToConfig, Configuration.Serialize(new Configuration
                {
                    AllowedFileTypes = new[] { "webm", "jpg", "gif", "png", "jpeg", "bmp", "mp4", "avi", "mkv", "flv" }
                }));
            }

            Configuration = Configuration.Deserialize(File.ReadAllText(pathToConfig));
        }

        public BrowserWindow()
        {
            LoadSaveConfiguration();

            Browser = new ImageBrowser();
            Libraries = new ObservableCollection<ImageLibrary>();

            InitializeComponent();

            foreach (Type type in LibraryContainer.Container)
            {
                Button b = new Button();
                b.DataContext = type;
                b.Content =
                    new Image()
                    {
                        Source =
                            new BitmapImage(new Uri(type.GetProperty("IconPath").GetValue(null) as string,
                                UriKind.Relative))
                    };

                b.Click += AddLibrary_OnClick;

                b.Width = 24;

                AddLibraryPanel.Children.Add(b);
            }

            string cmdLine = Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();

            if (cmdLine != null)
            {
                string dirPath = cmdLine.TrimEnd('\\');

                if (Directory.Exists(dirPath))
                {
                    Libraries.Add(new LocalImageLibrary(dirPath, Configuration));
                }
                else
                {
                    if (File.Exists(cmdLine))
                    {
                        Libraries.Add(new LocalImageLibrary(Path.GetDirectoryName(cmdLine), Configuration));
                        Browser.SetLibrary(Libraries.First());
                        Browser.SetIndexToPath(cmdLine);
                    }
                }
            }

            Timer t = new Timer(200);
            t.Elapsed += delegate
            {
                OnPropertyChanged("MediaDurationFormatted");
            };
            t.Start();
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
            Browser.SortImages(e.AddedItems[0].ToString());
            Focus();
            OnPropertyChanged("Libraries");
        }

        private void LibraryTreeList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var value = e.NewValue as ImageLibrary;
            if (value != null)
            {
                Browser.SetLibrary(value);
            }
            else
            {
                var newValue = e.NewValue as ImageData;
                if (newValue != null)
                {
                    Browser.SetLibrary(ImageLibrary.FindImageInLibraries(Libraries, newValue));
                    Browser.SetIndexToImage(newValue);
                }
            }
        }

        private void MediaPlayerElement_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                ScaleTransform scaleTransform1 = new ScaleTransform(1, 1, MediaPlayerElement.ActualWidth / 2, MediaPlayerElement.ActualHeight / 2);
                MediaPlayerElement.RenderTransform = scaleTransform1;
                _imageScale = 1;
            }

            //This is the only way to get the treeview to unfocus
            MuteButton.Focus();
        }

        private void AddLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var imageLibrary = button.DataContext as Type;
            if (imageLibrary == null) return;

            var newLibrary = imageLibrary.GetMethod("CreateLibrary").Invoke(null, new object[] {Configuration}) as ImageLibrary;
            if (newLibrary == null) return;

            Libraries.Add(newLibrary);
            Browser.SetLibrary(newLibrary);
        }

        private void MediaPlayerElement_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _imageScale += (e.Delta > 0 ? 0.06f : -0.06f);

            double xCenter = Mouse.GetPosition(MediaPlayerElement).X;
            double yCenter = Mouse.GetPosition(MediaPlayerElement).Y;

            ScaleTransform scaleTransform1 = new ScaleTransform(_imageScale, _imageScale, xCenter, yCenter);
            MediaPlayerElement.RenderTransform = scaleTransform1;
        }

        private void FilePath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Uri.IsWellFormedUriString(Browser.CurrentImage.Path, UriKind.Absolute) || File.Exists(Browser.CurrentImage.Path))
            {
                Process.Start(Browser.CurrentImage.Path);
            }
        }
    }
}
