using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class MvDetailResponse
    {
        public int Code { get; set; }
        public string LoadingPic { get; set; } = String.Empty;
        public string BufferPic { get; set; } = String.Empty;
        public string LoadingPicFS { get; set; } = String.Empty;
        public string BufferPicFS { get; set; } = String.Empty;
        public bool Subed { get; set; }
        public MpData Mp { get; set; } = default!;
        public MvDetail Data { get; set; } = default!;

        public class MpData
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("fee")]
            public int Fee { get; set; }

            [JsonPropertyName("mvFee")]
            public int MvFee { get; set; }

            [JsonPropertyName("payed")]
            public int Payed { get; set; }

            [JsonPropertyName("pl")]
            public int Pl { get; set; }

            [JsonPropertyName("dl")]
            public int Dl { get; set; }

            [JsonPropertyName("cp")]
            public int Cp { get; set; }

            [JsonPropertyName("sid")]
            public int Sid { get; set; }

            [JsonPropertyName("st")]
            public int St { get; set; }

            [JsonPropertyName("normal")]
            public bool Normal { get; set; }

            [JsonPropertyName("unauthorized")]
            public bool Unauthorized { get; set; }

            [JsonPropertyName("msg")]
            public dynamic? Msg { get; set; }
        }
    }

    public class MvDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int ArtistId { get; set; }
        public string ArtistName { get; set; } = default!;
        public string BriefDesc { get; set; } = string.Empty;
        public object? Desc { get; set; }
        public string Cover { get; set; } = default!;

        [JsonPropertyName("coverId_str")]
        public string CoverIdStr { get; set; } = default!;

        public long CoverId { get; set; }
        public int PlayCount { get; set; }
        public int SubCount { get; set; }
        public int ShareCount { get; set; }
        public int CommentCount { get; set; }
        public int Duration { get; set; }
        public int NType { get; set; }
        public string PublishTime { get; set; } = default!;
        public object? Price { get; set; }
        public List<BrData> Brs { get; set; } = default!;
        public List<Artist> Artists { get; set; } = default!;
        public string CommentThreadId { get; set; } = default!;
        public List<VideoGroupData>? VideoGroup { get; set; }

        public record BrData
        {
            [JsonPropertyName("size")]
            public int Size { get; set; }

            [JsonPropertyName("br")]
            public int Br { get; set; }

            [JsonPropertyName("point")]
            public int Point { get; set; }
        }

        public record VideoGroupData
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = default!;

            [JsonPropertyName("type")]
            public int Type { get; set; }
        }
    }
}