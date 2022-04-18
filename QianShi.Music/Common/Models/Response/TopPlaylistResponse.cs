using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class TopPlaylistResponse
    {
        public int Code { get; set; }
        public long Total { get; set; }
        public bool More { get; set; }
        public string? Cat { get; set; }
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }

    public class Playlist : IPlaylist
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long TrackNumberUpdateTime { get; set; }
        public int Status { get; set; }
        public long UserId { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
        public int SubscribedCount { get; set; }
        public int TrackCount { get; set; }
        public int ColudTrackCount { get; set; }
        [JsonPropertyName("CoverImgUrl")]
        public string CoverImgUrl { get; set; } = null!;
        public long CoverImgId { get; set; }
        public string? Description { get; set; }
        public List<string>? Tags { get; set; }
        public long PlayCount { get; set; }
        public long TrackUpdateTime { get; set; }
        public int SpecialType { get; set; }
        public long TotalDuration { get; set; }
        public Creator? Creator { get; set; }
        public List<Creator>? Subscribers { get; set; }
        public Creator? Subscribed { get; set; }
        public List<Track> Tracks { get; set; } = new List<Track>();
        public string? CommentThreadId { get; set; }
        public bool NewImported { get; set; }
        public int AdType { get; set; }
        public bool HighQuality { get; set; }
        public int Privacy { get; set; }
        public bool Ordered { get; set; }
        public bool Anonimous { get; set; }
        public int CoverStatus { get; set; }
        public RecommendInfo? RecommendInfo { get; set; }
        public int ShareCount { get; set; }
        public string? CoverImgId_str { get; set; }
        public string? Alg { get; set; }
        public long CommentCount { get; set; }

        public class Track
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
            [JsonPropertyName("ar")]
            public List<Artist> Artists { get; set; } = new List<Artist>();
            [JsonPropertyName("al")]
            public Album Album { get; set; } = new Album();
            [JsonPropertyName("dt")]
            public long Size { get; set; }  // 毫秒
            public long MV { get; set; }
        }

        public class Artist
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
            public List<string> Alias { get;set; } = new List<string>();
        }

        public class Album
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
            public string PicUrl { get; set; } = null!;
        }

    }
    public class RecommendInfo
    {
        public string? Alg { get; set; }
        public string? LogInfo { get; set; }
    }

    public class Creator
    {
        public bool DefaultAvatar { get; set; }
        public long Province { get; set; }
        public int AuthStatus { get; set; }
        public bool Followed { get; set; }
        public string? AvatarUrl { get; set; }
        public int AccountStatus { get; set; }
        public int Gender { get; set; }
        public int City { get; set; }
        public long Birthday { get; set; }
        public long UserId { get; set; }
        public int UserType { get; set; }
        public string? Nickname { get; set; }
        public string? Signature { get; set; }
        public string? Description { get; set; }
        public string? DetailDescription { get; set; }
        public long AvatarImgId { get; set; }
        public long BackgroundImgId { get; set; }
        public string? BackgroundUrl { get; set; }
        public int Authority { get; set; }
        public bool Mutual { get; set; }
        public List<string>? ExpertTags { get; set; }
        public Dictionary<int, string>? Experts { get; set; }
        public int DjStatus { get; set; }
        public int VipType { get; set; }
        public List<string>? RemarkName { get; set; }
        public int AuthenticationTypes { get; set; }
        public AvatarDetail? AvatarDetail { get; set; }
        public bool Anchor { get; set; }
        public string? BackgroundImgIdStr { get; set; }
        public string? AvatarImgIdStr { get; set; }
        public string? AvatarImgId_str { get; set; }
    }

    public class AvatarDetail
    {
        public int UserType { get; set; }
        public int IdentityLevel { get; set; }
        public string? IdentityIconUrl { get; set; }
    }
}
