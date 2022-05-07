using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class SearchResponse
    {
        public int Code { get; set; }

        public string? Msg { get; set; }

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

    public record MovieVideoSearchResult : ISearchResultBase
    {
        [JsonPropertyName("mvCount")]
        public int MovieVideoCount { get; set; }
        [JsonPropertyName("mvs")]
        public List<MovieVideo>? MovieVideos { get; set; }
    }

    public class MovieVideo : IPlaylist
    {
        public string Alg { get; set; } = string.Empty;
        public string ArTransName { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public long ArtistId { get; set; }
        public List<string>? Alias { get; set; }
        public Artist? Artist { get; set; }
        public List<Artist>? Artists { get; set; }
        public string? BriefDesc { get; set; }
        public string Cover { get; set; } = string.Empty;
        /// <summary>
        /// 有些接口返回为Cover 有些返回为Imgurl
        /// </summary>
        public string Imgurl { get => Cover; set => Cover = value; }
        public string CoverImgUrl { get => Cover; set => Cover = value; }
        public string Desc { get; set; } = string.Empty;
        public long Duration { get; set; }
        public long Id { get; set; }
        public long PlayCount { get; set; }
        public int Mark { get; set; }
        public string Name { get; set; } = string.Empty;
        public dynamic? TransNames { get; set; }
        public string? PublishTime { get; set; }
    }
}