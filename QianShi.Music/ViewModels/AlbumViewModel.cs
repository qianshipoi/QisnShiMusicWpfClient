using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using QianShi.Music.Common;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views.Dialogs;

using System.Collections.ObjectModel;
using System.Windows;

namespace QianShi.Music.ViewModels
{
    public class AlbumViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        private readonly IPlaylistService _playlistService;
        private readonly IContainerProvider _containerProvider;
        private readonly IDialogHostService _dialogHostService;
        private string _title;
        private bool _loading;
        private ObservableCollection<SongBindable> _playlists;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<SongBindable> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        private PlaylistDetail _detail = new PlaylistDetail();

        public bool Loading
        {
            get => _loading;
            set
            {
                SetProperty(ref _loading, value);
            }
        }

        public PlaylistDetail Detail { get => _detail; set => SetProperty(ref _detail, value); }
        /// <summary>
        /// 播放歌单
        /// </summary>
        public DelegateCommand<SongBindable?> PlayCommand { get; private set; }
        /// <summary>
        /// 立即播放
        /// </summary>
        public DelegateCommand<SongBindable?> PlayImmediatelyCommand { get; private set; }

        public DelegateCommand ShowDescriptionCommand { get; private set; }

        public bool KeepAlive => false;

        public AlbumViewModel(IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IDialogHostService dialogHostService) : base(containerProvider)
        {
            _title = string.Empty;
            _playlists = new ObservableCollection<SongBindable>();
            _dialogHostService = dialogHostService;

            PlayCommand = new DelegateCommand<SongBindable?>(Play);
            PlayImmediatelyCommand = new DelegateCommand<SongBindable?>(Play);
            _playlistService = playlistService;
            _containerProvider = containerProvider;
            ShowDescriptionCommand = new DelegateCommand(ShowDescription);
        }

        void Play(SongBindable? palylist)
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

        async void ShowDescription()
        {
            var parameters = new DialogParameters();
            parameters.Add("Description", Detail.Description);

            var dialog = _containerProvider.Resolve<DescriptionDialog>();

            if (dialog is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (dialog.DataContext is IDialogHostAware aware)
            {
                aware.DialogHostName = PrismManager.PlaylistDialogName;
            }

            DialogOpenedEventHandler eventHandler = (sender, eventArgs) =>
            {
                if (dialog.DataContext is IDialogHostAware aware)
                {
                    aware.OnDialogOpend(parameters);
                }
                eventArgs.Session.UpdateContent(dialog);
            };

            await DialogHost.Show(dialog, PrismManager.PlaylistDialogName, eventHandler);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var playlistId = navigationContext.Parameters.GetValue<long>("PlaylistId");
            Title = playlistId.ToString();
            if (Detail.Id != playlistId)
            {
                Loading = true;
                var response = await _playlistService.GetAblumAsync(playlistId);
                if (response.Code == 200)
                {
                    Detail.Id = response.Album.Id;
                    Detail.Name = response.Album.Name;
                    Detail.Description = response.Album.Description ?? String.Empty;
                    Detail.LastUpdateTime = response.Album.PublishTime;
                    Detail.PicUrl = response.Album.CoverImgUrl;
                    Detail.Count = response.Album.Size;
                    Detail.Creator = response.Album.Artist.Name;
                    _playlists.Clear();

                    Playlists.AddRange(response.Songs.Select(x => new SongBindable
                    {
                        Id = x.Id,
                        ArtistName = x.Artists[0].Name,
                        Name = x.Name,
                        Time = x.Dt
                    }));
                }
                Loading = false;
            }

            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (DialogHost.IsDialogOpen(PrismManager.PlaylistDialogName))
            {
                var session = DialogHost.GetDialogSession(PrismManager.PlaylistDialogName);
                if (session != null)
                    session.UpdateContent(new LoadingDialog());
                DialogHost.Close(PrismManager.PlaylistDialogName);
            }
            base.OnNavigatedFrom(navigationContext);
        }

    }

    public class SongBindable : BindableBase
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ArtistName { get; set; }
        public long Time { get; set; }

        private bool _isLike;
        public bool IsLike { get => _isLike; set => SetProperty(ref _isLike, value); }

        private bool _isPlaying;
        public bool IsPlaying { get => _isPlaying; set => SetProperty(ref _isPlaying, value); }
    }
}
