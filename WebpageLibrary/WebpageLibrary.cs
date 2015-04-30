using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using CsQuery.ExtensionMethods;
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
            
        }

        new public static string IconPath
        {
            get { return "Icons/webpageicon.png"; }
        }

        private WebpageLibrary(Uri url, Dictionary<string, string> cookies)
        {
            WebClient w = new WebClient();

            //super high level hacks
            w.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.90 Safari/537.36");

            foreach (var entry in cookies)
            {
                w.Headers.Add(HttpRequestHeader.Cookie, String.Format("{0}={1}", entry.Key, entry.Value));
            }

            CQ dom = w.DownloadString(url);
            var allImages = dom[":not(a) > img"].ToList();
            var allLinks = dom["a"].ToList();
            var allVideos = dom["video > source:first-child"].ToList();

            _pageTitle = dom["title"].Text().Trim();
            _sourceUrl = url.ToString();

            var imageLinks = allImages.Select(d => d.GetAttribute("src"));
            var textLinks = allLinks.Select(d => d.GetAttribute("href"));
            var vidLinks = allVideos.Select(d => d.GetAttribute("src"));

            Uri baseUri = new Uri(url.Scheme + "://" + url.Host + String.Join("", url.Segments.Reverse().Skip(1).Reverse()));
            Uri newUri;

            List<ImageData> genericLinks = imageLinks.Union(textLinks).Union(vidLinks)
                .Distinct()
                .Where(d => !String.IsNullOrWhiteSpace(Path.GetExtension(d)))
                .Where(d => Configuration.Current.AllowedFileTypes.Contains(Path.GetExtension(d).Substring(1).ToLower()))
                .Select(d => IsOkayUri(d, baseUri, out newUri) ? newUri : null)
                .Where(d=>d != null)
                .Select(d => new ImageData
                {
                    FileName = Path.GetFileName(d.ToString()),
                    LastModified = DateTime.Now,
                    Path = (d.IsAbsoluteUri ? d.ToString() : new Uri(baseUri, d).ToString())
                }).ToList();

            Images = new ObservableCollection<ImageData>(genericLinks);
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
            return new WebpageLibrary(new Uri("http://boards.4chan.org/a/"), new Dictionary<string, string>());
        }
    }
}
