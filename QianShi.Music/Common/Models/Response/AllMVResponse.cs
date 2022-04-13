namespace QianShi.Music.Common.Models.Response
{
    public class AllMVResponse
    {
        public int Code { get; set; }
        public long Count { get; set; }
        public bool HasMore { get; set; }
        public List<MV>? Data { get; set; }
        public class MV
        {
            public long Id{ get; set; }
            public string? Cover { get; set; }
            public string? Name { get; set; }
            public long PlayCount { get; set; }
            public string? BriefDesc { get; set; }
            public string? Desc { get; set; }
            public string? ArtistName { get; set; }
            public long ArtistId { get; set; }
            public long Duration { get; set; }
            public long Mark { get; set; }
            public bool Subed { get; set; }
            public List<Artist>? Artists { get; set; }
        }

        public class Artist
        {
            public long Id { get; set; }
            public string? Name { get; set; }
            public List<string>? Alias { get; set; }
            public List<string>? TransNames { get; set; }
        }
    }

}
