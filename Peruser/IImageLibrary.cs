using System.Collections.Generic;
using System.ComponentModel;

namespace Peruser
{
    public interface IImageLibrary : INotifyPropertyChanged
    {
        string IconPath { get; }
        List<ImageData> Images { get; }
        string[] SortKinds { get; }
        string SourcePath { get; set; }
        string Title { get; }
        void SortImages(string sortkind);
    }
}
