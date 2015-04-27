using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Peruser.Annotations;

namespace Peruser
{
    public class ImageBrowser : INotifyPropertyChanged
    {
        public ImageLibrary CurrentLibrary { get; set; }
        private int _imageIndex;

        public ImageData CurrentImage
        {
            get
            {
                if (CurrentLibrary == null || CurrentLibrary.Images.Count == 0)
                {
                    return new ImageData
                    {
                        FileName = "Nothing",
                        Path = "",
                        LastModified = DateTime.Now
                    };
                }

                return CurrentLibrary.Images[_imageIndex];
            }
        }

        public string ImageIndexDisp
        {
            get
            {
                if (CurrentLibrary == null)
                {
                    return "No Images";
                }
                return (ImageIndex + 1) + "/" + CurrentLibrary.Images.Count;
            }
        }

        public string[] ValidSorts
        {
            get
            {
                if (CurrentLibrary == null)
                {
                    return new[] {"None"};
                }
                return CurrentLibrary.SortKinds;
            }
        }

        public void SetLibrary(ImageLibrary library)
        {
            CurrentLibrary = library;
            ImageIndex = 0;
            OnPropertyChanged("ValidSorts");
        }

        private int ImageIndex
        {
            get { return _imageIndex; }
            set
            {
                if (CurrentLibrary == null)
                {
                    _imageIndex = 0;
                    return;
                }

                _imageIndex = value;

                if (value < 0)
                {
                    _imageIndex = CurrentLibrary.Images.Count - 1;
                }
                if (value > CurrentLibrary.Images.Count - 1)
                {
                    _imageIndex = 0;
                }

                OnPropertyChanged();
                OnPropertyChanged("CurrentImage");
                OnPropertyChanged("ImageIndexDisp");
            }
        }

        public void SetIndexToImage(ImageData image)
        {
            int foundIndex = CurrentLibrary.Images.IndexOf(image);

            if (foundIndex >= 0)
            {
                ImageIndex = foundIndex;
            }
        }

        public void SetIndexToPath(string path)
        {
            ImageData foundImage = CurrentLibrary.Images.FirstOrDefault(d => d.Path == path);
            if (foundImage != null)
            {
                SetIndexToImage(foundImage);
            }
        }

        public void SortImages(string sortkind)
        {
            if (CurrentLibrary != null)
            {
                CurrentLibrary.SortImages(sortkind);
                ImageIndex = 0;
            }
        }

        public void NextImage()
        {
            ImageIndex++;
        }
        public void PrevImage()
        {
            ImageIndex--;
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
