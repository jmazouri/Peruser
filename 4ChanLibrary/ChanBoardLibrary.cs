using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Peruser;
using Peruser.Utilities;
using _4ChanLibrary;

namespace ChanLibrary
{
    public class ChanBoardLibrary : ImageLibrary
    {
        private string _boardName;

        new public static string IconPath
        {
            get { return "Icons/chanboardicon.png"; }
        }

        public override ObservableCollection<ImageData> Images { get; protected set; }

        new public static ImageLibrary CreateLibrary()
        {
            ChooseBoard boardDialog = new ChooseBoard();
            if (boardDialog.ShowDialog() == true)
            {
                return new ChanBoardLibrary(boardDialog.Board.Board, 1);
            }
            return null;
        }

        private ChanBoardLibrary(string board, int page)
        {
            _boardName = board;

            WebClient wc = new WebClient();
            string jsondata = wc.DownloadString(String.Format("http://a.4cdn.org/{0}/{1}.json", board, page));
            JObject jo = JObject.Parse(jsondata);

            var firstPosts = jo.SelectTokens("threads[*].posts[0]", true).ToList();

            Images = new ObservableCollection<ImageData>();

            foreach (ChanPost post in firstPosts.Select(d => d.ToObject<ChanPost>()).ToList())
            {
                string strippedComment = ChanUtil.StripComment(post.Com, 120);
                if (String.IsNullOrWhiteSpace(strippedComment))
                {
                    strippedComment = post.No.ToString();
                }

                Images.Add(new ImageData
                {
                    FileName = strippedComment,
                    LastModified = Util.UnixTimeStampToDateTime(post.Time),
                    Path = String.Format("http://i.4cdn.org/{0}/{1}{2}", board, post.Tim, post.Ext),
                    ExtraData = new Dictionary<string, string>
                    {
                        { "ThreadNo", post.No.ToString() },
                        { "BoardName", board }
                    }
                });
            }

            _sourceUrl = String.Format("http://4chan.org/{0}", board);
        }

        public override ImageLibrary CreateSubLibrary(ImageData data)
        {
            if (data.ExtraData.ContainsKey("ThreadNo") && data.ExtraData.ContainsKey("BoardName"))
            {
                return new ChanThreadLibrary(data.ExtraData["BoardName"], data.ExtraData["ThreadNo"]);
            }

            return null;
        }

        public override string[] SortKinds
        {
            get { return new[] {"Last Post"}; }
        }

        public override string Title
        {
            get { return "4Chan - /"+_boardName+"/"; }
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
