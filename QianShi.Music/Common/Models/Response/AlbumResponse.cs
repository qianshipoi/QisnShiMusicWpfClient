using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class AlbumResponse
    {
        public int Code { get; set; }

        public Album Album { get; set; } = new Album();

        public bool ResourceState { get; set; }

        public List<Song> Songs { get; set; } = new List<Song>();

        public class Song 
        {
            public long Id { get;set; }
            public string Name { get; set; } = null!;

            [JsonPropertyName("ar")]
            public List<Artist> Artists { get; set; } = null!;

            [JsonPropertyName("al")]
            public Album Album { get; set; } = null!;

            [JsonPropertyName("dt")]
            public long Time { get; set; }
        }
    }
}
