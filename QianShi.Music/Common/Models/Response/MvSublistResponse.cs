using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class MvSublistResponse
    {
        public int Code { get; set; }
        public int Count { get; set; }
        public bool HasMore { get; set; }

        public List<MovieVideoSubject> Data { get; set; } = new();
    }

    public class MovieVideoSubject : IPlaylist
    {
        public string? Alg { get; set; }
        public string? AliaName { get; set; }
        public string Vid { get; set; } = default!;
        public long Id { get => int.Parse(Vid); set => Vid = value.ToString(); }
        public string Title { get; set; } = default!;
        public string Name { get => Title; set => Title = value; }

        [JsonPropertyName("coverUrl")]
        public string CoverImgUrl { get; set; } = default!;

        public List<CreatorType> Creator { get; set; } = default!;
        public long Durationms { get; set; }
        public dynamic? MarkTypes { get; set; }
        public long PlayTime { get; set; }

        public class CreatorType
        {
            public long UserId { get; set; }
            public string? UserName { get; set; }
        }
    }
}