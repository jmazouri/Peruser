using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ImgurLibrary;
using ImgurLibrary.ImgurApi;
using Peruser.Annotations;
using Peruser.ImgurApi;
using Peruser.Utilities;
using Path = System.IO.Path;

namespace Peruser.ImageLibraries
{
    public class ImgurLibrary : ImageLibrary
    {
        private string _curSubreddit;

        private string _sortKind = "time";
        private string _sortWindow = "";

        new public static string IconPath
        {
            get { return "Icons/imguricon.png"; }
        }

        public override string SourceUrl { get { return String.Format("http://imgur.com/r/{0}", _curSubreddit); } }

        private List<ImageData> _loadedImages;

        public override string[] SortKinds
        {
            get { return new[] { "Top - Day", "Top - Week", "Top - Month", "Top - Year", "Top - All", "Newest" }; }
        }

        new public static ImageLibrary CreateLibrary()
        {
            var dialog = new ChooseSubreddit();
            return dialog.ShowDialog() == true && !String.IsNullOrWhiteSpace(dialog.Subreddit) ? new ImgurLibrary(dialog.Subreddit) : null;
        }

        public override string Title
        {
            get { return "/r/"+_curSubreddit; }
        }

        public override void SortImages(string sortkind)
        {
            _sortKind = sortkind.Contains("Top") ? "top" : "time";
            _sortWindow = sortkind.Contains("Top") ? sortkind.Split('-')[1].Trim().ToLower() : "";
            LoadSubreddit();

            Images.Clear();
            foreach (ImageData img in _loadedImages) { Images.Add(img); }
        }

        private void LoadSubreddit()
        {
            _loadedImages = ImgurInfo.GetImagesFromSubreddit(_curSubreddit, _sortKind, _sortWindow)
                .Select(d => new ImageData
                {
                    FileName = d.Title + " (" + d.Score + ")",
                    Path = (d.WebM ?? d.Link),
                    LastModified = Util.UnixTimeStampToDateTime(d.Datetime)
                }).ToList();

        }

        private ImgurLibrary(string subreddit)
        {
            _curSubreddit = subreddit;
            Images = new ObservableCollection<ImageData>();
            SortImages("Newest");
        }
    }
}
