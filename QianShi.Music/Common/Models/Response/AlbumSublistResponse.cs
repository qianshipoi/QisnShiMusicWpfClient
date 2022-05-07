namespace QianShi.Music.Common.Models.Response
{
    public class AlbumSublistResponse
    {
        public int Code { get; set; }

        public int Count { get; set; }

        public bool HasMore { get; set; }

        public int PaidCount { get; set; }

        public List<Album> Data { get; set; } = new List<Album>();

        //public class Subject : IPlaylist
        //{
        //    public List<string> Alias { get; set; } = new List<string>();
        //    public List<Artist> Artists { get; set; } = new();
        //    public long Id { get; set; }
        //    public List<string> Msg { get; set; } = new();
        //    public string Name { get; set; } = default!;
        //    public long PicId { get; set; }
        //    public string PicUrl { get; set; } = default!;
        //    public string CoverImgUrl { get => PicUrl; set => PicUrl = value; }
        //    public int Size { get; set; }
        //    public long SubTime { get; set; }
        //    public List<string> TransNames { get; set; } = new();
        //}
    }
}