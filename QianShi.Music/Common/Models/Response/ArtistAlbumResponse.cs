namespace QianShi.Music.Common.Models.Response
{
    public class ArtistAlbumResponse
    {
        public int Code { get; set; }
        public Artist Artist { get; set; } = default!;
        public bool More { get; set; }
        public List<Album> HotAlbums { get; set; } = default!;
    }
}
