using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Peruser.ImgurApi;

namespace ImgurLibrary.ImgurApi
{
    public static class ImgurInfo
    {
        public static List<ImgurImage> GetImagesFromSubreddit(string subreddit, string sort = "top", string window = "week")
        {
            var w = new WebClient();
            w.Headers.Add("Authorization", "Client-ID 5cc135541ec45ae");

            try
            {
                string url = String.Format("https://api.imgur.com/3/gallery/r/{0}/{1}/{2}", subreddit, sort, window);
                string jsondata = w.DownloadString(url);
                dynamic returnedObject = JObject.Parse(jsondata);

                return JsonConvert.DeserializeObject<List<ImgurImage>>(returnedObject.data.ToString());
            }
            catch (WebException)
            {
                return new List<ImgurImage>();
            }
        }
    }
}
