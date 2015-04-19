using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Peruser.Annotations;
using Peruser.ImgurApi;
using Path = System.IO.Path;

namespace Peruser.ImageLibraries
{
    public class RedditImageLibrary : IImageLibrary
    {
        private string curSubreddit = "";

        private List<ImageData> loadedImages = new List<ImageData>(); 
        public List<ImageData> Images
        {
            get { return loadedImages; }
        }

        public string[] SortKinds
        {
            get { return new[] {"Hot", "Top", "Newest"}; }
        }

        public string SourcePath { get; set; }

        public string Title
        {
            get { return curSubreddit; }
        }

        public void SortImages(string sortkind)
        {
            
        }

        public RedditImageLibrary(string subreddit, Configuration config)
        {
            loadedImages = ImgurInfo.GetImagesFromSubreddit(subreddit)
                .Select(d => new ImageData
                {
                    FileName = d.Title + " (" + d.Score + ")",
                    Path = (d.WebM ?? d.Link),
                    LastModified = Util.UnixTimeStampToDateTime(d.Datetime)
                }).ToList();
            curSubreddit = "/r/" + subreddit;
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
