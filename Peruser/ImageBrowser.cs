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
        private ImageLibrary curImageLibrary;
        private int _imageIndex;

        public ImageData CurrentImage
        {
            get
            {
                if (curImageLibrary == null || curImageLibrary.Images.Count == 0)
                {
                    return new ImageData
                    {
                        FileName = "Nothing",
                        Path = "",
                        LastModified = DateTime.Now
                    };
                }

                return curImageLibrary.Images[_imageIndex];
            }
        }

        public string ImageIndexDisp
        {
            get
            {
                if (curImageLibrary == null)
                {
                    return "No Images";
                }
                return (ImageIndex + 1) + "/" + curImageLibrary.Images.Count;
            }
        }

        public string[] ValidSorts
        {
            get
            {
                if (curImageLibrary == null)
                {
                    return new[] {"None"};
                }
                return curImageLibrary.SortKinds;
            }
        }

        public void SetLibrary(ImageLibrary library)
        {
            curImageLibrary = library;
            ImageIndex = 0;
            OnPropertyChanged("ValidSorts");
        }

        private int ImageIndex
        {
            get { return _imageIndex; }
            set
            {
                _imageIndex = value;

                if (value < 0)
                {
                    _imageIndex = curImageLibrary.Images.Count - 1;
                }
                if (value > curImageLibrary.Images.Count - 1)
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
            int foundIndex = curImageLibrary.Images.IndexOf(image);

            if (foundIndex >= 0)
            {
                ImageIndex = foundIndex;
            }
        }

        public void SetIndexToPath(string path)
        {
            ImageData foundImage = curImageLibrary.Images.FirstOrDefault(d => d.Path == path);
            if (foundImage != null)
            {
                SetIndexToImage(foundImage);
            }
        }

        public void SortImages(string sortkind)
        {
            if (curImageLibrary != null)
            {
                curImageLibrary.SortImages(sortkind);
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
