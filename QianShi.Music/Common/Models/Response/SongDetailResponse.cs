using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class SongDetailResponse
    {
        public int Code { get; set; }
        public List<Song> Songs { get; set; } = new List<Song>();
    }

    public record Song
    {
        [JsonPropertyName("al")]
        public Album Album { get; set; } = null!;
        public List<string> Alia { get; set; } = new List<string>();
        [JsonPropertyName("ar")]
        public List<Artist> Artists { get; set; } = new List<Artist>();
        public string Cd { get; set; } = string.Empty;
        public string Cf { get; set; } = string.Empty;
        public long Copyright { get; set; }
        public long Cp { get; set; }
        public dynamic? Crbt { get; set; }
        public long DjId { get; set; }
        public long Dt { get; set; }
        public dynamic? EntertainmentTags { get; set; }
        public int Fee { get; set; }
        public int Ftype { get; set; }
        public Quality? H { get; set; }
        public Quality? Hr { get; set; }
        public long Id { get; set; }
        public Quality? L { get; set; }
        public Quality? M { get; set; }
        public long Mark { get; set; }
        public int Mst { get; set; }
        public int Mv { get; set; }
        public string Name { get; set; } = string.Empty;
        public int No { get; set; }
        public int OriginCoverType { get; set; }
        public dynamic? OriginSongSimpleData { get; set; }
        public int Pop { get; set; }
        public int Pst { get; set; }
        public long PublishTime { get; set; }
        public bool ResourceState { get; set; }
        public string Rt { get; set; } = string.Empty;
        public List<string> RtUrls { get; set; } = new();
        public int Rtype { get; set; }
        public dynamic? Rurl { get; set; }
        [JsonPropertyName("s_id")]
        public int Sid { get; set; }
        public int Single { get; set; }
        public int St { get; set; }
        public int T { get; set; }
        public dynamic? TagPicList { get; set; }
        public dynamic? SongJumpInfo { get; set; }
        public List<string> Tns { get; set; } = new();
        public Quality? Sq { get; set; }
        public int V { get; set; }
        public int Version { get; set; }
    }
}
