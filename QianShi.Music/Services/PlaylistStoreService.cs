using System.Collections.ObjectModel;

using QianShi.Music.Common.Models.Request;
using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Services;

public class PlaylistStoreService : IPlaylistStoreService
{
    private readonly IPlaylistService _playlistService;
    private readonly List<Song> _likedSongs = new();
    private readonly UserData _userInfo = UserData.Instance;
    public IReadOnlyList<Song> LikedSongs { get; }

    public PlaylistStoreService(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
        LikedSongs = new ReadOnlyCollection<Song>(_likedSongs);
        UserData.Instance.LoginChanged += Instance_LoginChanged;
        _ = GetLikedSongsAsync();
    }

    public async Task GetLikedSongsAsync()
    {
        if (_userInfo.IsLogin)
        {
            _likedSongs.Clear();
            var likedPlaylistResponse = await _playlistService.UserPlaylist(new UserPlaylistRequest
            {
                Uid = _userInfo.Id,
                Limit = 10
            });
            if (likedPlaylistResponse.Code == 200)
            {

                var likePlaylist = likedPlaylistResponse.Playlist.FirstOrDefault();
                if (likePlaylist != null)
                {
                    var playlistResponse = await _playlistService.GetPlaylistDetailAsync(likePlaylist.Id);
                    if (playlistResponse.Code == 200)
                    {
                        _likedSongs.AddRange(playlistResponse.PlaylistDetail.Tracks);
                    }
                }
            }
        }
    }

    private async void Instance_LoginChanged(object? sender, PropertyChangedEventArgs<bool> e)
    {
        if (e.NewValue)
        {
            await GetLikedSongsAsync();
        }
        else
        {
            _likedSongs.Clear();
        }
    }

    public bool HasLikedSong(Song song) => HasLikedSong(song.Id);

    public bool HasLikedSong(long id)
    {
        if (!_userInfo.IsLogin) return false;
        return _likedSongs.Any(x => x.Id == id);
    }

}