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
        private readonly List<string> _allImages = new List<string>();
        private int _imageIndex;

        public string CurrentImage
        {
            get { return _allImages[_imageIndex]; }
        }

        private int ImageIndex
        {
            get { return _imageIndex; }
            set
            {
                if (Equals(value, _imageIndex)) return;

                _imageIndex = value;

                if (value < 0)
                {
                    _imageIndex = 0;
                }
                if (value > _allImages.Count - 1)
                {
                    _imageIndex = _allImages.Count - 1;
                }

                OnPropertyChanged();
                OnPropertyChanged("CurrentImage");
            }
        }

        public ImageBrowser()
        {
            foreach (string s in Directory.GetFiles(@"C:\Users\jmazo_000\Pictures\Test"))
            {
                _allImages.Add(s);
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
