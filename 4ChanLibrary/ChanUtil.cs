using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Peruser;
using Peruser.Utilities;

namespace _4ChanLibrary
{
    public static class ChanUtil
    {
        public static string StripComment(string com, int maxlength = 80)
        {
            string strippedComment = Util.StripHtml(com);
            strippedComment = Regex.Replace(strippedComment, @"(>>\d{9,})", "");
            return (strippedComment.Length > maxlength ? strippedComment.Substring(0, maxlength - 1) : strippedComment);
        }
    }
}
