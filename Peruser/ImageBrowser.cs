using System.ComponentModel;
using System.Runtime.CompilerServices;
using Peruser.Annotations;

namespace Peruser
{
    public class ImageBrowser : INotifyPropertyChanged
    {
        private IImageLibrary curImageLibrary;
        private int _imageIndex;

        public string CurrentImage
        {
            get { return curImageLibrary.Images[_imageIndex].Path; }
        }

        public string ImageIndexDisp
        {
            get { return (ImageIndex + 1) + "/" + curImageLibrary.Images.Count; }
        }

        public void SetLibrary(IImageLibrary library)
        {
            curImageLibrary = library;
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

        public void SortImages(string sortkind)
        {
            curImageLibrary.SortImages(sortkind);
            ImageIndex = 0;
        }

        public ImageBrowser(IImageLibrary library)
        {
            SetLibrary(library);
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
