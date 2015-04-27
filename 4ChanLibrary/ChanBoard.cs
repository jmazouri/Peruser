using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChanLibrary
{
    public class ChanBoard
    {
        public string Board { get; set; }
        public string Title { get; set; }
        public int WsBoard { get; set; }
        public int PerPage { get; set; }
        public int Pages { get; set; }
        public int MaxFilesize { get; set; }
        public int MaxWebmFilesize { get; set; }
        public int MaxCommentChars { get; set; }
        public int BumpLimit { get; set; }
        public int ImageLimit { get; set; }
        public int IsArchived { get; set; }

        public override string ToString()
        {
            return String.Format("/{0}/ - {1}", Board, Title);
        }
    }
}
