using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ImgurLibrary;
using Peruser.Annotations;
using Peruser.ImgurApi;
using Path = System.IO.Path;

namespace Peruser.ImageLibraries
{
    public class ImgurLibrary : ImageLibrary
    {
        private string curSubreddit = "";

        private List<ImageData> loadedImages = new List<ImageData>();

        new public static string IconPath
        {
            get { return "Icons/imguricon.png"; }
        }

        public override List<ImageData> Images
        {
            get { return loadedImages; }
        }

        public override string[] SortKinds
        {
            get { return new[] { "Top - Day", "Top - Week", "Top - Month", "Top - Year", "Top - All", "Newest" }; }
        }

        public string SourcePath { get; set; }

        new public static ImageLibrary CreateLibrary(Configuration configuration)
        {
            ChooseSubreddit dialog = new ChooseSubreddit();
            if (dialog.ShowDialog() == true)
            {
                return new ImgurLibrary(dialog.Subreddit);
            }

            return null;
        }

        public override string Title
        {
            get { return curSubreddit; }
        }

        public override void SortImages(string sortkind)
        {
            
        }

        private ImgurLibrary(string subreddit)
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
    }
}
