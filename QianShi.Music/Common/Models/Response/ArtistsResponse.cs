namespace QianShi.Music.Common.Models.Response
{
    public class ArtistsResponse
    {
        public int Code { get; set; }
        public Artist Artist { get; set; } = default!;
        public bool More { get; set; }
        public List<Song> HotSongs { get; set; } = new List<Song>();
    }
}