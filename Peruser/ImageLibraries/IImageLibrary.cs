using System.Collections.Generic;
using System.ComponentModel;

namespace Peruser
{
    public interface IImageLibrary : INotifyPropertyChanged
    {
        List<ImageData> Images { get; }
        string SourcePath { get; set; }
        void SortImages(string sortkind);
    }
}
