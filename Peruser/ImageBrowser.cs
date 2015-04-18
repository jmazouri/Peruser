using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Peruser.Annotations;

namespace Peruser
{
    public class ImageBrowser : INotifyPropertyChanged
    {
        private List<ImageData> _allImages = new List<ImageData>();
        private int _imageIndex;

        public string CurrentImage
        {
            get { return _allImages[_imageIndex].Path; }
        }

        public string ImageIndexDisp
        {
            get { return (ImageIndex + 1) + "/" + _allImages.Count; }
        }

        public void SetPath(string filepath)
        {
            _allImages.Clear();
            foreach (string s in Directory.GetFiles(filepath))
            {
                _allImages.Add(new ImageData
                {
                    Path = s,
                    FileName = Path.GetFileName(s),
                    LastModified = new FileInfo(s).LastWriteTime
                });
            }

            SortImages();

            ImageIndex = 0;
        }

        public void SortImages(string sortkind = "Date Descending")
        {
            switch (sortkind)
            {
                case "Name Descending":
                    _allImages = _allImages.OrderByDescending(d => d.FileName).ToList();
                    break;
                case "Name Ascending":
                    _allImages = _allImages.OrderBy(d => d.FileName).ToList();
                    break;
                case "Date Descending":
                    _allImages = _allImages.OrderByDescending(d => d.LastModified).ToList();
                    break;
                case "Date Ascending":
                    _allImages = _allImages.OrderBy(d => d.LastModified).ToList();
                    break;
            }

            ImageIndex = 0;
        }

        private int ImageIndex
        {
            get { return _imageIndex; }
            set
            {
                _imageIndex = value;

                if (value < 0)
                {
                    _imageIndex = _allImages.Count - 1;
                }
                if (value > _allImages.Count - 1)
                {
                    _imageIndex = 0;
                }

                OnPropertyChanged();
                OnPropertyChanged("CurrentImage");
                OnPropertyChanged("ImageIndexDisp");
            }
        }

        public ImageBrowser(string filepath)
        {
            SetPath(filepath);
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
