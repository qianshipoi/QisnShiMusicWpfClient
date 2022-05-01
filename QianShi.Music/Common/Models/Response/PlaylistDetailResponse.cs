using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class PlaylistDetailResponse
    {
        public int Code { get; set; }

        [JsonPropertyName("Playlist")]
        public Playlist PlaylistDetail { get; set; } = new Playlist();

        public List<string>? Urls { get; set; }
    }
}