using QianShi.Music.Common.Models.Response;
using QianShi.Music.Models;
using QianShi.Music.Services;

using Album = QianShi.Music.Common.Models.Response.Album;
using Artist = QianShi.Music.Common.Models.Response.Artist;

namespace QianShi.Music.Data
{
    /// <summary> Artist Data Provider </summary>
    public class ArtistDataProvider : IDataProvider<ArtistModel, long>
    {
        private readonly IPlaylistService _playlistService;
        private readonly Dictionary<long, ArtistModel> _cache = new Dictionary<long, ArtistModel>();

        public ArtistDataProvider(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public async Task<ArtistModel?> GetDataAsync(long id)
        {
            if (_cache.ContainsKey(id))
            {
                return _cache[id];
            }

            var (artist, songs) = await GetHotSongsAsync(id);
            if (artist == null)
            {
                return null;
            }

            var aristModel = new ArtistModel(artist)
            {
                Songs = songs
            };

            aristModel.Albums = await GetAlbumsAsync(id);
            aristModel.MovieVideos = await GetMovieVideosAsync(id);
            aristModel.Artists = await GetArtistsAsync(id);

            _cache.Add(id, aristModel);

            return aristModel;
        }

        private async Task<List<Album>?> GetAlbumsAsync(long id)
        {
            var response = await _playlistService.ArtistAlbum(new Common.Models.Request.ArtistAlbumRequest
            {
                Id = id,
                Limit = 20
            });
            if (response.Code == 200)
            {
                return response.HotAlbums.Select(x =>
                {
                    x.CoverImgUrl += "?param=200y200";
                    return x;
                }).ToList();
            }
            return null;
        }

        private async Task<List<Artist>?> GetArtistsAsync(long id)
        {
            var response = await _playlistService.SimiArtist(id);
            if (response.Code == 200)
            {
                return response.Artists.Take(12).Select(x =>
                {
                    x.CoverImgUrl += "?param=200y200";
                    return x;
                }).ToList();
            }
            return null;
        }

        private async Task<(Artist? artist, List<Song>? songs)> GetHotSongsAsync(long id)
        {
            var response = await _playlistService.Artists(id);
            if (response.Code == 200)
            {
                var songs = response.HotSongs.Select(x =>
                {
                    x.Album.CoverImgUrl += "?param=48y48";
                    return x;
                }).Take(12).ToList();

                return (response.Artist, songs);
            }
            return (null, null);
        }

        private async Task<List<MovieVideo>?> GetMovieVideosAsync(long id)
        {
            var response = await _playlistService.ArtistMv(new Common.Models.Request.ArtistMvRequest
            {
                Id = id,
                Limit = 20
            });
            if (response.Code == 200)
            {
                var mvs = response.Mvs.Select(x =>
                {
                    x.CoverImgUrl += "?param=464y260";
                    return x;
                }).ToList();

                return mvs;
            }
            return null;
        }
    }
}
