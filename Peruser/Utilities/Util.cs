using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Peruser.Utilities
{
    public static class Util
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
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
