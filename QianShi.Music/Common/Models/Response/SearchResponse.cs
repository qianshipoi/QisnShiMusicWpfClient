namespace QianShi.Music.Common.Models.Response
{
    public class SearchResponse
    {
        public int Code { get; set; }

        public ISearchResultBase Result { get; set; } = null!;
    }

    public class SearchResponse<T> : SearchResponse where T : ISearchResultBase
    {
        private T _result = default!;
        public new T Result
        {
            get => _result;
            set
            {
                _result = value;
                base.Result = value;
            }
        }
    }

    public interface ISearchResultBase
    { }

    public record SongSearchResult : ISearchResultBase
    {
        public bool HasMore { get; set; }
        public int SongCount { get; set; }
        public List<Song> Songs { get; set; } = new List<Song>();

        public class Song
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
            public Album Album { get; set; } = null!;
            public List<Artist> Artists { get; set; } = new List<Artist>();
            public long Duration { get; set; }
            /// <summary>
            /// 为 1 无法播放
            /// </summary>
            public int Fee { get; set; }
            public long Mvid { get; set; }
            public long CopyrightId { get; set; }
            public int Ftype { get; set; }
            public int mark { get; set; }
            public string? RUrl { get; set; }
            public int Rtype { get; set; }
            public int Status { get; set; }
            public List<string> TransNames { get; set; } = new List<string>();
        }
    }

    public record AlbumSearchResult : ISearchResultBase
    {
        public int AlbumCount { get; set; }
        public List<Album> Albums { get; set; } = new();
    }

    public record ArtistSearchResult : ISearchResultBase
    {
        public int ArtistCount { get; set; }
        public bool HasMore { get; set; }
        public dynamic? SearchQcReminder { get; set; }
        public List<Artist> Artists { get; set; } = new();
    }

    public record PlaylistSearchResult : ISearchResultBase
    {
        public bool HasMore { get; set; }
        public int PlaylistCount { get; set; }
        public dynamic? SearchQcReminder { get; set; }
        public List<Playlist> Playlists { get; set; } = new();
    }

    public record MvSearchResult : ISearchResultBase
    {
        public int MvCount { get; set; }
        public List<Mv> Mvs { get; set; } = new();
    }

    public class Mv
    {
        public string Alg { get; set; } = string.Empty;
        public string ArTransName { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public long ArtistId { get; set; }
        public List<string>? Alias { get; set; }
        public List<Artist> Artists { get; set; } = new();
        public dynamic? BriefDesc { get; set; }
        public string Cover { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public long Duration { get; set; }
        public long Id { get; set; }
        public long PlayCount { get; set; }
        public int Mark { get; set; }
        public string Name { get; set; } = string.Empty;
        public dynamic? TransNames { get; set; }
    }
}

