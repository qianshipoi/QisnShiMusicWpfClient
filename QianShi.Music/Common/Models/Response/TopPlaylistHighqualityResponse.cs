namespace QianShi.Music.Common.Models.Response
{
    public class TopPlaylistHighqualityResponse
    {
        public int Code { get; set; }
        public long Total { get; set; }
        public bool More { get; set; }
        public long Lasttime { get; set; }
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
