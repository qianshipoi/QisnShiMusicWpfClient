using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class TopSongResponse
    {
        public int Code { get; set; }
        public List<TopSong>? Data { get; set; }

        public class TopSong
        {
            public bool Starred { get; set; }
            public int Popularity { get; set; }
            public int StarredNum { get; set; }
            public int PlayedNum { get; set; }
            public int DayPlays { get; set; }
            public long HearTime { get; set; }
            public string? Mp3Url { get; set; }
            public string? RtUrls { get; set; }
            public Album? Album { get; set; }
            public Music? MMusic { get; set; }
            public Music? LMusic { get; set; }
            public string? CopyFrom { get; set; }
            public string? CommentThreadId { get; set; }
            public Music? HMusic { get; set; }
            public long Mvid { get; set; }
            public long Ftype { get; set; }
            public long Rtype { get; set; }
            public string? Rurl { get; set; }
            public long Fee { get; set; }
            public int Status { get; set; }
            public string? Crbt { get; set; }
            public Music? BMusic { get; set; }
            public string? RtUrl { get; set; }
            public long No { get; set; }
            public List<string>? Alias { get; set; }
            public List<Artist>? Artists { get; set; }
            public int Score { get; set; }
            public long CopyrightId { get; set; }
            public string? Audition { get; set; }
            public string? Ringtone { get; set; }
            public string? Disc { get; set; }
            public long Position { get; set; }
            public long Duration { get; set; }
            public string? Name { get; set; }
            public long Id { get; set; }
            public bool Exclusive { get; set; }
            public Privilege? Privilege { get; set; }
        }

        public class Privilege
        {
            public long Id { get; set; }
            public long Fee { get; set; }
            public long Payed { get; set; }
            public long St { get; set; }
            public long Pl { get; set; }
            public long Dl { get; set; }
            public long Sp { get; set; }
            public long Cp { get; set; }
            public long Subp { get; set; }
            public bool Cs { get; set; }
            public long Maxbr { get; set; }
            public long Fl { get; set; }
            public bool Toast { get; set; }
            public long Flag { get; set; }
            public bool PreSell { get; set; }
        }

        public class Music
        {
            public long DfsId { get; set; }
            public long PlayTime { get; set; }
            public long VolumeDelta { get; set; }
            public long Sr { get; set; }
            public long Bitrate { get; set; }
            public string? Name { get; set; }
            public long Id { get; set; }
            public long Size { get; set; }
            public string? Extension { get; set; }
        }

        public class Album
        {
            public List<string>? Songs { get; set; }
            public bool Paid { get; set; }
            public bool OnSale { get; set; }
            public long PicId { get; set; }
            public Artist? Artist { get; set; }
            public string? CommentThreadId { get; set; }
            public long PublishTime { get; set; }
            public string? PicUrl { get; set; }
            public int Status { get; set; }
            public string? BriefDesc { get; set; }
            public string? BlurPicUrl { get; set; }
            public long CompanyId { get; set; }
            public long Pic { get; set; }
            public string? Tags { get; set; }
            public List<string>? Alias { get; set; }
            public List<Artist>? Artists { get; set; }
            public long CopyrightId { get; set; }
            public string? SubType { get; set; }
            public string? Description { get; set; }
            public string? Company { get; set; }
            public string? Name { get; set; }
            public long Id { get; set; }
            public string? Type { get; set; }
            public long Size { get; set; }
            public string? PicId_str { get; set; }
        }
    }

    /// <summary>
    /// 艺术家（歌手）
    /// </summary>
    public class Artist : IPlaylist
    {
        public long Img1v1Id { get; set; }
        public long TopicPerson { get; set; }
        public long PicId { get; set; }
        public string? Trans { get; set; }
        public int AlbumSize { get; set; }
        public string? Img1v1Url { get; set; }
        [JsonPropertyName("picUrl")]
        public string CoverImgUrl { get; set; } = null!;
        public bool Followed { get; set; }
        public string? BriefDesc { get; set; }
        public List<string> Alias { get; set; } = new List<string>();
        public long MusicSize { get; set; }
        public string Name { get; set; } = null!;
        public long Id { get; set; }
        public string? Img1v1Id_str { get; set; }
        public long AccountId { get; set; }
        public string IdentityIconUrl { get; set; } = string.Empty;
        public string Alg { get; set; } = string.Empty;
    }
}