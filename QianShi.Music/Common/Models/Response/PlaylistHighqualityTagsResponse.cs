namespace QianShi.Music.Common.Models.Response
{
    public class PlaylistHighqualityTagsResponse
    {
        public int Code { get; set; }
        public List<Tag>? Tags { get; set; }

        public class Tag
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int Type { get; set; }
            public int Category { get; set; }
            public bool Hot { get; set; }
        }
    }
}