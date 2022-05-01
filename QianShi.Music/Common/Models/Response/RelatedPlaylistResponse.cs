namespace QianShi.Music.Common.Models.Response
{
    public class RelatedPlaylistResponse
    {
        public int Code { get; set; }
        public List<Playlist>? Playlists { get; set; }

        public class Playlist
        {
            public Creator? Creator { get; set; }
            public string? CoverImgUrl { get; set; }
            public string? Name { get; set; }
            public string? Id { get; set; }
        }

        public class Creator
        {
            public string? UserId { get; set; }
            public string? Nickname { get; set; }
        }
    }
}