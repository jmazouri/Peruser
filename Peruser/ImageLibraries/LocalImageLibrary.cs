using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAPICodePack.Dialogs;
using Peruser.Annotations;

namespace Peruser.ImageLibraries
{
    public class LocalImageLibrary : ImageLibrary
    {
        private string _sourcePath = "";
        public override string Title
        {
            get { return _sourcePath; }
        }

        new public static string IconPath
        {
            get { return "Icons/iconfolder.png"; }
        }

        private List<ImageData> _images = new List<ImageData>();
        public override ObservableCollection<ImageData> Images { get; protected set; }

        public override string[] SortKinds
        {
            get
            {
                return new[]
                {
                    "Name Descending", "Name Ascending", "Date Descending", "Date Ascending"
                };
            }
        }

        public override void SortImages(string sortkind = "Date Descending")
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
            Images.Clear();
            foreach (ImageData img in _images) { Images.Add(img); }
        }

        public override string SourceUrl { get { return _sourcePath; } }

        new public static ImageLibrary CreateLibrary()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };

            if (dialog.ShowDialog() != CommonFileDialogResult.Cancel)
            {
                return new LocalImageLibrary(dialog.FileName);
            }

            return null;
        }

        public void SetPath(string filepath)
        {
            _images.Clear();
            foreach (string s in Directory.GetFiles(filepath))
            {
                if (Configuration.Current.AllowedFileTypes.Contains(Path.GetExtension(s).Substring(1)))
                {
                    _images.Add(new ImageData
                    {
                        Path = s,
                        FileName = Path.GetFileName(s),
                        LastModified = new FileInfo(s).LastWriteTime
                    });
                }
            }

            _sourcePath = filepath;

            SortImages();
            OnPropertyChanged("Images");
        }

        public LocalImageLibrary(string directoryPath)
        {
            //Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), Configuration
            Images = new ObservableCollection<ImageData>();
            SetPath(directoryPath);
        }
    }
}
