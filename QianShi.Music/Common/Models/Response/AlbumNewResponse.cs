namespace QianShi.Music.Common.Models.Response
{
    public class AlbumNewResponse
    {
        public int Code { get; set; }
        public int Total { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
