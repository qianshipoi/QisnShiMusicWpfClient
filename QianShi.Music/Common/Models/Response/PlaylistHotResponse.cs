namespace QianShi.Music.Common.Models.Response
{
    public class PlaylistHotResponse
    {
        public int Code { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public class Tag
        {
            public PlaylistTag? PlaylistTag { get; set; }
            public bool Activity { get; set; }
            public long CreateTime { get; set; }
            public long UsedCount { get; set; }
            public bool Hot { get; set; }
            public int Position { get; set; }
            public int Categroy { get; set; }
            public string? Name { get; set; }
            public int Id { get; set; }
            public int Type { get; set; }
        }

        /// <summary>
        /// 歌单标签
        /// </summary>
        public class PlaylistTag
        {
            public int Id { get; set; }

            public string? Name { get; set; }

            public int Category { get; set; }

            public long UsedCount { get; set; }

            public int Type { get; set; }

            public int Position { get; set; }

            public long CreateTime { get; set; }

            public int HighQuality { get; set; }

            public int HighQualityPos { get; set; }
            public int OfficialPos { get; set; }
        }
    }
}