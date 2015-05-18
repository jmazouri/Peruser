using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Peruser.Annotations;

namespace Peruser
{
    public abstract class ImageLibrary : INotifyPropertyChanged
    {
        public static string IconPath
        {
            get { return ""; }
        }

        public virtual ObservableCollection<ImageData> Images { get; protected set; }

        public virtual string[] SortKinds
        {
            get { return new[] {"Name Ascending", "Name Descending", "Date Ascending", "Date Descending", "Random"}; }
        }

        public abstract string SourceUrl { get; }
        public abstract string Title { get; }

        public virtual void SortImages(string sortkind)
        {
            switch (sortkind)
            {
                case "Name Ascending":
                    Images = new ObservableCollection<ImageData>(Images.OrderBy(d => d.FileName));
                    break;
                case "Name Descending":
                    Images = new ObservableCollection<ImageData>(Images.OrderByDescending(d => d.FileName));
                    break;
                case "Date Ascending":
                    Images = new ObservableCollection<ImageData>(Images.OrderBy(d => d.LastModified));
                    break;
                case "Date Descending":
                    Images = new ObservableCollection<ImageData>(Images.OrderByDescending(d => d.LastModified));
                    break;
                case "Random":
                    Random rand = new Random();
                    Images = new ObservableCollection<ImageData>(Images.OrderBy(d=>rand.Next()));
                    break;
            }

            OnPropertyChanged("Images");
        }

        public virtual ImageLibrary CreateSubLibrary(ImageData data)
        {
            return null;
        }


        public static ImageLibrary FindImageInLibraries(IEnumerable<ImageLibrary> libraries, ImageData image)
        {
            return libraries.FirstOrDefault(d => d.Images.Contains(image));
        }

        public static ImageLibrary CreateLibrary()
        {
            return null;
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
