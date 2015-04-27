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
        public abstract string[] SortKinds { get; }



        public abstract string SourceUrl { get; }
        public abstract string Title { get; }
        public abstract void SortImages(string sortkind);


        public virtual ImageLibrary CreateSubLibrary(ImageData data)
        {
            return null;
        }


        public static ImageLibrary FindImageInLibraries(IEnumerable<ImageLibrary> libraries, ImageData image)
        {
            return libraries.FirstOrDefault(d => d.Images.Contains(image));
        }

        public static ImageLibrary CreateLibrary(Configuration configuration)
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
