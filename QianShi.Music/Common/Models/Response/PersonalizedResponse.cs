using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class PersonalizedResponse
    {
        public int Code { get; set; }
        public bool HasTaste { get; set; }
        public int Category { get; set; }
        public List<PersonalizedPlaylist> Result { get; set; } = new List<PersonalizedPlaylist>();
    }

    public class PersonalizedPlaylist : IPlaylist
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; } = null!;
        public string Copywriter { get; set; } = null!;

        [JsonPropertyName("PicUrl")]
        public string CoverImgUrl { get; set; } = null!;

        public bool CanDislike { get; set; }
        public long PlayCount { get; set; }
        public bool HighQuality { get; set; }
    }
}