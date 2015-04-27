using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Peruser;
using _4ChanLibrary;

namespace ChanLibrary
{
    public class ChanThreadLibrary : ImageLibrary
    {
        private string _postTitle;
        private string _boardName;

        new public static string IconPath
        {
            get { return "Icons/chanthreadicon.png"; }
        }

        public override ObservableCollection<ImageData> Images { get; protected set; }

        new public static ImageLibrary CreateLibrary(Configuration configuration)
        {
            ChooseThread threadDialog = new ChooseThread();
            if (threadDialog.ShowDialog() == true)
            {
                return new ChanThreadLibrary(threadDialog.Board, threadDialog.Thread);
            }

            return null;
        }

        

        internal ChanThreadLibrary(string board, string threadId)
        {
            _boardName = board;

            WebClient wc = new WebClient();
            string jsondata = wc.DownloadString(String.Format("http://a.4cdn.org/{0}/thread/{1}.json", board, threadId));
            JObject jo = JObject.Parse(jsondata);

            var firstPosts = jo.GetValue("posts").Select(d => d.ToObject<ChanPost>()).ToList();
            _postTitle = ChanUtil.StripComment(firstPosts.First().Com);

            Images = new ObservableCollection<ImageData>();

            foreach (ChanPost post in firstPosts.Where(d=>d.Filename != null))
            {
                string strippedComment = ChanUtil.StripComment(post.Com);
                Images.Add(new ImageData
                {
                    FileName = (post == firstPosts.First() ? "OP" :
                        (String.IsNullOrWhiteSpace(strippedComment) ? post.Filename + post.Ext : strippedComment)),
                    LastModified = Util.UnixTimeStampToDateTime(post.Time),
                    Path = String.Format("http://i.4cdn.org/{0}/{1}{2}", board, post.Tim, post.Ext)
                });
            }

            _sourceUrl = String.Format("http://boards.4chan.org/{0}/thread/{1}/", board, threadId);
        }

        public override string[] SortKinds
        {
            get { return new[] {"Last Post"}; }
        }

        public override string Title
        {
            get { return String.Format("{0} - /{1}/", _postTitle, _boardName); }
        }

        public override void SortImages(string sortkind)
        {
            //Uhh... nothing
        }

        private string _sourceUrl;
        public override string SourceUrl
        {
            get { return _sourceUrl; }
        }
    }
}
