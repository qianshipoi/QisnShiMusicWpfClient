using System.Text.Json.Serialization;

namespace QianShi.Music.Common.Models.Response
{
    public class AlbumNewestResponse
    {
        public int Code { get; set; }

        public List<Album> Albums { get; set; } = new List<Album>();
    }
    public class Album : IPlaylist
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Size { get; set; }
        [JsonPropertyName("PicUrl")]
        public string CoverImgUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Tags { get; set; } = null!;
        public Artist Artist { get; set; } = new Artist();
        public long PublishTime { get; set; }
    }
}
