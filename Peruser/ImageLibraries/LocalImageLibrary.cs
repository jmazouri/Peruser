using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Peruser.Annotations;

namespace Peruser.ImageLibraries
{
    public class LocalImageLibrary : IImageLibrary
    {
        public Configuration Configuration { get; set; }

        private List<ImageData> _images = new List<ImageData>();
        public List<ImageData> Images
        {
            get { return _images; }
        }

        public string SourcePath { get; set; }

        public void SortImages(string sortkind = "Date Descending")
        {
            switch (sortkind)
            {
                case "Name Descending":
                    _images = _images.OrderByDescending(d => d.FileName).ToList();
                    break;
                case "Name Ascending":
                    _images = _images.OrderBy(d => d.FileName).ToList();
                    break;
                case "Date Descending":
                    _images = _images.OrderByDescending(d => d.LastModified).ToList();
                    break;
                case "Date Ascending":
                    _images = _images.OrderBy(d => d.LastModified).ToList();
                    break;
            }
        }

        public void SetPath(string filepath)
        {
            _images.Clear();
            foreach (string s in Directory.GetFiles(filepath))
            {
                if (Configuration.AllowedFileTypes.Contains(Path.GetExtension(s).Substring(1)))
                {
                    _images.Add(new ImageData
                    {
                        Path = s,
                        FileName = Path.GetFileName(s),
                        LastModified = new FileInfo(s).LastWriteTime
                    });
                }
            }

            SortImages();
            OnPropertyChanged("Images");
        }

        public LocalImageLibrary(string directoryPath, Configuration config)
        {
            Configuration = config;
            SetPath(directoryPath);
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
