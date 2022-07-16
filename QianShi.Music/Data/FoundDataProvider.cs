using QianShi.Music.Common;
using QianShi.Music.Common.Models.Request;
using QianShi.Music.Services;

using System.Collections.Concurrent;

namespace QianShi.Music.Data
{
    public interface IFoundPlaylist
    {
        ObservableCollection<IPlaylist> Playlists { get; }
        bool HasMore { get; }
        Task GetDataAsync();
    }

    public class BoutiquePlaylist : BindableBase, IFoundPlaylist
    {
        private readonly IPlaylistService _playlistService;
        private bool _hasMore = false;
        private long? _before = null;

        public ObservableCollection<IPlaylist> Playlists { get; } = new();

        public bool HasMore
        {
            get => _hasMore;
            private set => SetProperty(ref _hasMore, value);
        }

        public BoutiquePlaylist(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public async Task GetDataAsync()
        {
            if (!HasMore && Playlists.Count > 0)
            {
                return;
            }

            var response = await _playlistService.GetTopPlaylistHighqualityAsnyc(new TopPlaylistHighqualityRequest
            {
                Limit = 40,
                Before = _before
            });

            if (response != null)
            {
                response.Playlists.ForEach(item =>
                {
                    _ = string.IsNullOrEmpty(item.CoverImgUrl) ? null : item.CoverImgUrl += "?param=200y200";
                });

                Playlists.AddRange(response.Playlists);

                HasMore = response.More;
                _before = response.Lasttime;
            }
        }
    }

    public class RecommendedPalylist : BindableBase, IFoundPlaylist
    {
        private readonly IPlaylistService _playlistService;

        public ObservableCollection<IPlaylist> Playlists { get; } = new();

        public bool HasMore => Playlists.Count > 0;

        public RecommendedPalylist(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public async Task GetDataAsync()
        {
            var response = await _playlistService.GetPersonalizedAsync(100);
            if (response != null)
            {
                response.Result.ForEach(item =>
                {
                    _ = string.IsNullOrEmpty(item.CoverImgUrl) ? null : item.CoverImgUrl += "?param=200y200";
                });

                Playlists.AddRange(response.Result);
                RaisePropertyChanged(nameof(HasMore));
            }
        }
    }

    public class TopPlaylist : BindableBase, IFoundPlaylist
    {
        private readonly IPlaylistService _playlistService;

        public ObservableCollection<IPlaylist> Playlists { get; } = new();
        public bool HasMore => Playlists.Count > 0;

        public TopPlaylist(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public async Task GetDataAsync()
        {
            var response = await _playlistService.GetToplistAsync();
            if (response != null)
            {
                response.List.ForEach(item =>
                {
                    _ = string.IsNullOrEmpty(item.CoverImgUrl) ? null : item.CoverImgUrl += "?param=200y200";
                });

                Playlists.AddRange(response.List);
                RaisePropertyChanged(nameof(HasMore));
            }
        }
    }

    public class CurrentPlaylist : BindableBase, IFoundPlaylist
    {
        private readonly IPlaylistService _playlistService;
        private readonly string _catName;
        private int _offset = 0;
        private bool _hasMore;
        public ObservableCollection<IPlaylist> Playlists { get; } = new();

        public bool HasMore
        {
            get => _hasMore;
            private set => SetProperty(ref _hasMore, value);
        }

        public CurrentPlaylist(IPlaylistService playlistService, string catName)
        {
            _playlistService = playlistService;
            _catName = catName;
        }

        public async Task GetDataAsync()
        {
            var response = await _playlistService.GetTopPlaylistAsync(new TopPlaylistRequest
            {
                Cat = _catName,
                Offset = _offset
            });
            if (response != null)
            {
                response.Playlists.ForEach(item =>
                {
                    _ = string.IsNullOrEmpty(item.CoverImgUrl) ? null : item.CoverImgUrl += "?param=200y200";
                });

                Playlists.AddRange(response.Playlists);
                HasMore = response.More;
                _offset += response.Playlists.Count;
            }
        }
    }

    public class FoundDataProvider : IFoundDataProvider
    {
        private readonly IPlaylistService _playlistService;
        private readonly ConcurrentDictionary<string, IFoundPlaylist> _cache = new();

        public FoundDataProvider(
            IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public IFoundPlaylist CreatePlaylist(string catName)
        {
            switch (catName)
            {
                case "精品":
                    return _cache.GetOrAdd(catName, (name) => new BoutiquePlaylist(_playlistService));
                case "推荐":
                    return _cache.GetOrAdd(catName, (name) => new RecommendedPalylist(_playlistService));
                case "排行榜":
                    return _cache.GetOrAdd(catName, (name) => new TopPlaylist(_playlistService));
                default:
                    return _cache.GetOrAdd(catName, (name) => new CurrentPlaylist(_playlistService, name));
            }
        }
    }
}
