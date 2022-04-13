namespace QianShi.Music.Common.Models.Response
{
    /// <summary>
    /// 所有榜单
    /// </summary>
    public class ToplistResponse
    {
        public int Code { get; set; }

        public List<Toplist> List { get; set; } = new();

        public class Toplist :  IPlaylist
        {
            public long Id { get; set; }
            public string UpdateFrequency { get; set; } = null!;
            public string CoverImgUrl { get; set; } = null!;
            public string Description { get; set; } = null!;
            public string Name { get; set; } = null!;
            public long PlayCount { get; set; }
        }
    }

}
