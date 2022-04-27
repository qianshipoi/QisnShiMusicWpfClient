namespace QianShi.Music.Common.Models.Response
{
    public class AlbumResponse
    {
        public int Code { get; set; }

        public Album Album { get; set; } = new Album();

        public bool ResourceState { get; set; }

        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
