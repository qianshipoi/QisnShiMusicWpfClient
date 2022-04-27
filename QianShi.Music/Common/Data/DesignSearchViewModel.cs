using Prism.Mvvm;

using QianShi.Music.Common.Models.Response;

using System.Collections.ObjectModel;

namespace QianShi.Music.Common.Data
{
    public class MovieVideo
    {
        public string CoverImgUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Artist Artist { get; set; } = new Artist();
    }

    public class DesignSearchViewModel : BindableBase
    {
        public ObservableCollection<Artist> Artists { get; set; }
        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<Song> Songs { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }
        public ObservableCollection<MovieVideo> MovieVideos { get; set; }
        public DesignSearchViewModel()
        {
            var cover = "https://oss.kuriyama.top/static/background.png";
            Artists = new ObservableCollection<Artist>();
            Albums = new ObservableCollection<Album>();
            Songs = new ObservableCollection<Song>();
            Playlists = new ObservableCollection<Playlist>();
            MovieVideos = new ObservableCollection<MovieVideo>();

            for (int i = 0; i < 3; i++)
            {
                Artists.Add(new Artist
                {
                    Name = $"作者{i + 1}",
                    CoverImgUrl = cover
                });
                Albums.Add(new Album
                {
                    Name = $"专辑{i + 1}",
                    Description = $"描述{i + 1}",
                    CoverImgUrl = cover
                });
            }

            for (int i = 0; i < 16; i++)
            {
                Songs.Add(new Song
                {
                    Name = $"歌曲{i + 1}",
                    Album = new Album
                    {
                        CoverImgUrl = cover
                    },
                    Artists = new List<Artist>
                    {
                        new Artist { Name = $"作者{i+1}" }
                    }
                });
            }

            for (int i = 0; i < 12; i++)
            {
                Playlists.Add(new Playlist
                {
                    Name = $"歌单{i + 1}",
                    CoverImgUrl = cover
                });
            }

            for (int i = 0; i < 5; i++)
            {
                MovieVideos.Add(new MovieVideo
                {
                    Name = $"MV{i + 1}",
                    CoverImgUrl = cover,
                    Artist = new Artist { Name = $"作者{i + 1}" }
                });
            }
        }
    }
}
