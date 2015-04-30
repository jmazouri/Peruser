using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebpageLibrary
{
    public class WebpageConfig
    {
        [DisplayName("Skip Images Inside Links?")]
        public bool SkipImagesInsideLinks { get; set; }

        [DisplayName("User Agent")]
        public string UserAgent { get; set; }

        public List<string> Cookies
        {
            get { return RealCookies.Select(d => d.Key + "=" + d.Value).ToList(); }
            set
            {
                try
                {
                    RealCookies = value.Select(d => d.Split('=')).ToDictionary(d => d[0], d => d[1]);
                }
                catch (Exception)
                {
                    RealCookies = new Dictionary<string, string>();
                }
            }
        }

        [Browsable(false)]
        public Dictionary<string, string> RealCookies { get; set; }

        [DisplayName("Display Title")]
        public string TitleOverride { get; set; }

        [Required]
        [DisplayName("Page URL")]
        public string PageUrl { get; set; }

        public WebpageConfig()
        {
            SkipImagesInsideLinks = true;
            UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.90 Safari/537.36";
            Cookies = new List<string> { "DNT=1" };
            TitleOverride = null;
        }
    }
}
