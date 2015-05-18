using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using RestSharp;

namespace Peruser.Utilities
{
    public static class Util
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static string SearchGoogleForImage(ImageData img)
        {
            var client = new RestClient("http://images.google.com/");
            var request = new RestRequest("searchbyimage/upload", Method.POST);
            request.AddParameter("image_url", "");
            request.AddParameter("btnG", "Search");
            request.AddFile("encoded_image", img.Path);
            request.AddParameter("image_content", "");
            request.AddParameter("filename", "");
            request.AddParameter("hl", "en");
            request.AddParameter("bih", 500);
            request.AddParameter("biw", 500);

            var response = client.Execute(request);

            return response.ResponseUri.ToString().Replace("imghp", "search");
        }

        public static string StripHtml(string html, bool parseEntities = true, bool removeDuplicateSpaces = true)
        {
            if (html == null) { return String.Empty; }

            string sourceHtml = html;

            if (parseEntities)
            {
                sourceHtml = WebUtility.HtmlDecode(sourceHtml);
            }

            string noHtml = Regex.Replace(sourceHtml, @"<[^>]+>|&nbsp;", "").Trim();

            if (removeDuplicateSpaces)
            {
                return Regex.Replace(noHtml, @"\s{2,}", " ");
            }

            return noHtml;
        }
    }
}
