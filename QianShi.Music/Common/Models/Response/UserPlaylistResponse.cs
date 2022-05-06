namespace QianShi.Music.Common.Models.Response
{
    public class UserPlaylistResponse
    {
        public int Code { get; set; }
        public bool More { get; set; }
        public string Version { get; set; } = default!;

        public List<Playlist> Playlist { get; set; } = default!;
    }
}