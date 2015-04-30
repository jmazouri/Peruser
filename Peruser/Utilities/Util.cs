using System;
using System.Net;
using System.Text.RegularExpressions;

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
