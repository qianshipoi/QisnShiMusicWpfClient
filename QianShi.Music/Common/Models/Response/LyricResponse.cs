using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class LyricResponse
    {
        [JsonPropertyName("sgc")]
        public bool Sgc { get; set; }

        [JsonPropertyName("sfy")]
        public bool Sfy { get; set; }

        [JsonPropertyName("qfy")]
        public bool Qfy { get; set; }

        [JsonPropertyName("lrc")]
        public LyricData Lrc { get; set; }

        [JsonPropertyName("klyric")]
        public LyricData Klyric { get; set; }

        [JsonPropertyName("tlyric")]
        public LyricData Tlyric { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }


        public class LyricData
        {
            [JsonPropertyName("version")]
            public int Version { get; set; }

            [JsonPropertyName("lyric")]
            public string Lyric { get; set; }
        }
    }


}