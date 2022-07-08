using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Models
{
    public class ArtistModel : IDataModel
    {
        public Artist Artist { get; set; }
        public List<Album>? Albums { get; set; }
        public List<Artist>? Artists { get; set; }
        public List<MovieVideo>? MovieVideos { get; set; }
        public List<Song>? Songs { get; set; }

        public ArtistModel(Artist artist)
        {
            this.Artist = artist;
        }
    }
}
