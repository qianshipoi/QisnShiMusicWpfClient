using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class PlaylistDetailResponse : BaseResponse
    {

        [JsonPropertyName("playlist")]
        public Playlist PlaylistDetail { get; set; } = new Playlist();

        public List<string>? Urls { get; set; }
    }
}