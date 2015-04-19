using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Peruser.ImgurApi
{
    public class ImgurInfo
    {
        public static List<ImgurImage> GetImagesFromSubreddit(string subreddit)
        {
            WebClient w = new WebClient();
            w.Headers.Add("Authorization", "Client-ID 5cc135541ec45ae");

            try
            {
                string jsondata = w.DownloadString(String.Format("https://api.imgur.com/3/gallery/r/{0}/", subreddit));
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
