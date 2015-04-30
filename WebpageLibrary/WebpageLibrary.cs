using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using CsQuery;
using Peruser;

namespace WebpageLibrary
{
    public class WebpageLibrary : ImageLibrary
    {
        public override string[] SortKinds
        {
            get { return new[] {"Page Order"}; }
        }

        private string _sourceUrl;
        public override string SourceUrl
        {
            get { return _sourceUrl; }
        }

        private string _pageTitle;
        public override string Title
        {
            get { return _pageTitle; }
        }

        public override void SortImages(string sortkind)
        {
            //I dunno lol
        }

        new public static string IconPath
        {
            get { return "Icons/webpageicon.png"; }
        }

        public IEnumerable<ImageData> ScrapePage(WebpageConfig configuration)
        {
            WebClient w = new WebClient();

            //super high level hacks
            w.Headers.Add(HttpRequestHeader.UserAgent, configuration.UserAgent);

            foreach (var entry in configuration.RealCookies)
            {
                w.Headers.Add(HttpRequestHeader.Cookie, String.Format("{0}={1}", entry.Key, entry.Value));
            }

            Uri url = new Uri(configuration.PageUrl);
            CQ dom = w.DownloadString(url);

            w.Dispose();

            var allImages = dom["img"].ToList();

            if (configuration.SkipImagesInsideLinks)
            {
                //Don't get images within links, ie thumbnails
                allImages = dom[":not(a) > img"].ToList();
            }

            var allLinks = dom["a"].ToList();
            var allVideos = dom["video > source:first-child"].ToList();

            _pageTitle = dom["title"].Text().Trim();
            if (configuration.TitleOverride != null)
            {
                _pageTitle = configuration.TitleOverride;
            }
            _sourceUrl = url.ToString();

            var imageLinks = allImages.Select(d => d.GetAttribute("src"));
            var textLinks = allLinks.Select(d => d.GetAttribute("href"));
            var vidLinks = allVideos.Select(d => d.GetAttribute("src"));

            Uri baseUri = new Uri(url.Scheme + "://" + url.Host + String.Join("", url.Segments.Reverse().Skip(1).Reverse()));
            Uri newUri;

            //This can probably made a bit cleaner, but it works
            List<ImageData> genericLinks = imageLinks.Union(textLinks).Union(vidLinks)
                .Distinct()
                .Where(d => !String.IsNullOrWhiteSpace(Path.GetExtension(d)))
                .Where(d => Configuration.Current.AllowedFileTypes.Contains(Path.GetExtension(d).Substring(1).ToLower()))
                .Select(d => IsOkayUri(d, baseUri, out newUri) ? newUri : null)
                .Where(d => d != null)
                .Select(d => new ImageData
                {
                    FileName = Path.GetFileName(d.ToString()),
                    LastModified = DateTime.Now,
                    Path = (d.IsAbsoluteUri ? d.ToString() : new Uri(baseUri, d).ToString())
                }).ToList();

            return genericLinks;
        }

        private WebpageLibrary(WebpageConfig configuration)
        {
            Images = new ObservableCollection<ImageData>(ScrapePage(configuration));
        }

        private bool IsOkayUri(string u, Uri baseUri, out Uri newUri)
        {
            try
            {
                var uri = new UriBuilder(u)
                {
                    Scheme = Uri.UriSchemeHttp,
                    Port = -1
                };
                newUri = uri.Uri;
            }
            catch (Exception)
            {
                try
                {
                    var uriWithBase = new Uri(baseUri, u);
                    newUri = uriWithBase;
                }
                catch (Exception)
                {
                    newUri = null;
                    return false;
                }
            }

            return true;
        }

        new public static ImageLibrary CreateLibrary()
        {
            ChoosePage dialog = new ChoosePage();
            if (dialog.ShowDialog() == true)
            {
                return new WebpageLibrary(dialog.Config);
            }

            return null;
        }
    }
}
