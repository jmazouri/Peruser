namespace Peruser.ImgurApi
{
    public class ImgurImage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public object Description { get; set; }
        public long Datetime { get; set; }
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public long Views { get; set; }
        public long Bandwidth { get; set; }
        public object Vote { get; set; }
        public bool Favorite { get; set; }
        public bool Nsfw { get; set; }
        public string Section { get; set; }
        public object AccountUrl { get; set; }
        public object AccountId { get; set; }
        public string WebM { get; set; }
        public string Link { get; set; }
        public string RedditComments { get; set; }
        public object CommentCount { get; set; }
        public int Ups { get; set; }
        public int Downs { get; set; }
        public int Score { get; set; }
        public bool IsAlbum { get; set; }
    }
}
