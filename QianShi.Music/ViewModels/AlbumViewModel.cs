using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.ViewModels.Dialogs;
using QianShi.Music.Views.Dialogs;

using System.Collections.ObjectModel;
using System.Windows;

namespace QianShi.Music.ViewModels
{
    public class AlbumViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string AlbumIdParameterName = "PlaylistId";

        private readonly IContainerProvider _containerProvider;
        private readonly IPlaylistService _playlistService;
        private PlaylistDetail _detail = new();
        private bool _loading;
        private ObservableCollection<SongBindable> _playlists;
        private string _title;

        public AlbumViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService)
            : base(containerProvider)
        {
            _title = string.Empty;
            _playlists = new ObservableCollection<SongBindable>();

            PlayCommand = new DelegateCommand<SongBindable?>(Play);
            PlayImmediatelyCommand = new DelegateCommand<SongBindable?>(Play);
            _playlistService = playlistService;
            _containerProvider = containerProvider;
            ShowDescriptionCommand = new DelegateCommand(ShowDescription);
        }

        public PlaylistDetail Detail { get => _detail; set => SetProperty(ref _detail, value); }

        public bool KeepAlive => false;

        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        /// <summary>
        /// 播放歌单
        /// </summary>
        public DelegateCommand<SongBindable?> PlayCommand { get; private set; }

        /// <summary>
        /// 立即播放
        /// </summary>
        public DelegateCommand<SongBindable?> PlayImmediatelyCommand { get; private set; }

        public ObservableCollection<SongBindable> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        public DelegateCommand ShowDescriptionCommand { get; private set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var playlistId = navigationContext.Parameters.GetValue<long>(AlbumIdParameterName);
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

        private void Play(SongBindable? song)
        {
            if (song != null)
            {
                Playlists.Where(x => x.IsPlaying).ToList().ForEach(i => i.IsPlaying = false);
                song.IsPlaying = true;
            }
            else
            {
                Playlists.First().IsPlaying = false;
            }
        }

        private async void ShowDescription()
        {
            var dialog = _containerProvider.Resolve<DescriptionDialog>();

            if (dialog is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (dialog.DataContext is IDialogHostAware aware)
            {
                aware.DialogHostName = PrismManager.PlaylistDialogName;
            }

            await DialogHost.Show(dialog, PrismManager.PlaylistDialogName, openedEventHandler: (sender, eventArgs) =>
            {
                if (dialog.DataContext is IDialogHostAware dialogHostAware)
                {
                    dialogHostAware.OnDialogOpend(new DialogParameters { { DescriptionDialogViewModel.DescriptionParameterName, Detail.Description } });
                }
                eventArgs.Session.UpdateContent(dialog);
            });
        }
    }

    public class SongBindable : BindableBase
    {
        private bool _isLike;
        private bool _isPlaying;
        public string? ArtistName { get; set; }
        public long Id { get; set; }
        public bool IsLike { get => _isLike; set => SetProperty(ref _isLike, value); }
        public bool IsPlaying { get => _isPlaying; set => SetProperty(ref _isPlaying, value); }
        public string Name { get; set; } = null!;
        public long Time { get; set; }
    }
}