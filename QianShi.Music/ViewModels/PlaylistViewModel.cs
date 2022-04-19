﻿using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Services;
using QianShi.Music.Views;
using QianShi.Music.Views.Dialogs;

using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QianShi.Music.ViewModels
{
    public class PlaylistItem : BindableBase
    {
        public long Id { get; set; }
        public string? PicUrl { get; set; } = "https://oss.kuriyama.top/static/background.png";

        private ImageSource? _picImageSource;
        public ImageSource? PicImageSource
        {
            get => _picImageSource;
            set => SetProperty(ref _picImageSource, value);
        }
        public string Name { get; set; } = null!;
        public string ArtistName { get; set; } = null!;
        public string AlbumName { get; set; } = null!;
        public long Size { get; set; }
        private bool _isPlaying = false;
        public bool IsPlaying { get => _isPlaying; set => SetProperty(ref _isPlaying, value); }
    }

    public class PlaylistDetail : BindableBase
    {
        public long Id { get; set; }
        private string? _picUrl = "https://oss.kuriyama.top/static/background.png";
        public string? PicUrl { get => _picUrl; set => SetProperty(ref _picUrl, value); }
        private string? _name;
        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        private long _lastUpdateTime;
        public long LastUpdateTime { get => _lastUpdateTime; set => SetProperty(ref _lastUpdateTime, value); }
        private string? _description;
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        private int _count;
        public int Count { get => _count; set => SetProperty(ref _count, value); }
        private string? _creator;
        public string? Creator { get => _creator; set => SetProperty(ref _creator, value); }
    }

    public class PlaylistViewModel : NavigationViewModel
    {
        private readonly IPlaylistService _playlistService;
        private readonly IContainerProvider _containerProvider;
        private string _title;
        private bool _loading;
        private ObservableCollection<PlaylistItem> _playlists;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<PlaylistItem> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        private PlaylistDetail _detail = new PlaylistDetail();

        public bool Loading { get => _loading; set => SetProperty(ref _loading, value); }

        public PlaylistDetail Detail { get => _detail; set => SetProperty(ref _detail, value); }
        /// <summary>
        /// 播放歌单
        /// </summary>
        public DelegateCommand<PlaylistItem?> PlayCommand { get; private set; }
        /// <summary>
        /// 立即播放
        /// </summary>
        public DelegateCommand<PlaylistItem?> PlayImmediatelyCommand { get; private set; }

        public PlaylistViewModel(IContainerProvider containerProvider,
            IPlaylistService playlistService) : base(containerProvider)
        {
            _title = string.Empty;
            _playlists = new ObservableCollection<PlaylistItem>();
            PlayCommand = new DelegateCommand<PlaylistItem?>(Play);
            PlayImmediatelyCommand = new DelegateCommand<PlaylistItem?>(Play);
            _playlistService = playlistService;
            _containerProvider = containerProvider;
        }

        void Play(PlaylistItem? palylist)
        {
            if (palylist != null)
            {
                Playlists.Where(x => x.IsPlaying).ToList().ForEach(i => i.IsPlaying = false);
                palylist.IsPlaying = true;
            }
            else
            {
                Playlists.First().IsPlaying = false;
            }
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var playlistId = navigationContext.Parameters.GetValue<long>("PlaylistId");
            Title = playlistId.ToString();
            if (Detail.Id != playlistId)
            {
                Loading = true;
                var response = await _playlistService.GetPlaylistDetailAsync(playlistId);
                if (response.Code == 200)
                {
                    Detail.Id = response.PlaylistDetail.Id;
                    Detail.Name = response.PlaylistDetail.Name;
                    Detail.Description = response.PlaylistDetail.Description ?? String.Empty;
                    Detail.LastUpdateTime = response.PlaylistDetail.UpdateTime;
                    Detail.PicUrl = response.PlaylistDetail.CoverImgUrl;
                    Detail.Count = response.PlaylistDetail.TrackCount;
                    Detail.Creator = response.PlaylistDetail.Creator?.Nickname;
                    _playlists.Clear();

                    foreach (var track in response.PlaylistDetail.Tracks)
                    {
                        var playlistItem = new PlaylistItem
                        {
                            Id = track.Id,
                            AlbumName = track.Album.Name,
                            ArtistName = track.Artists[0].Name,
                            Name = track.Name,
                            IsPlaying = false,
                            PicUrl = track.Album.PicUrl + "?param=48y48",
                            Size = track.Size
                        };
                        playlistItem.PicImageSource = BitmapFrame.Create(
                            new Uri(playlistItem.PicUrl),
                            BitmapCreateOptions.None, BitmapCacheOption.Default);

                        _playlists.Add(playlistItem);
                        await Task.Delay(20);
                    }
                }
                Loading = false;
            }

            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (DialogHost.IsDialogOpen(Extensions.PrismManager.PlaylistDialogName))
            {
                var session = DialogHost.GetDialogSession(Extensions.PrismManager.PlaylistDialogName);
                if (session != null)
                    session.UpdateContent(new LoadingDialog());
                DialogHost.Close(Extensions.PrismManager.PlaylistDialogName);
            }
            base.OnNavigatedFrom(navigationContext);
        }

    }
}
