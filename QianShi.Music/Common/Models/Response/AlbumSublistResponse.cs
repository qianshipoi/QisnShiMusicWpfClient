namespace QianShi.Music.Common.Models.Response
{
    public class AlbumSublistResponse
    {
        public int Code { get; set; }

        public int Count { get; set; }

        public bool HasMore { get; set; }

        public int PaidCount { get; set; }

        public List<Subject> Data { get; set; } = new List<Subject>();

        public class Subject : IPlaylist
        {
            public List<string> Alias { get; set; } = new List<string>();
            public List<Artist> Artists { get; set; } = new();
            public long Id { get; set; }
            public List<string> Msg { get; set; } = new();
            public string Name { get; set; } = default!;
            public long PicId { get; set; }
            public string PicUrl { get; set; } = default!;
            public string CoverImgUrl { get => PicUrl; set => PicUrl = value; }
            public int Size { get; set; }
            public long SubTime { get; set; }
            public List<string> TransNames { get; set; } = new();
        }
    }

    public class UserRecordResponse
    {
        public int Code { get; set; }

        public List<Subject> WeekData { get; set; } = new();
        public List<Subject> AllData { get; set; } = new();

        public class Subject
        {
            public int PlayCount { get; set; }
            public sbyte Score { get; set; }
            public Song Song { get; set; } = default!;
        }
    }
}