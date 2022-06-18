using QianShi.Music.Common.Models.Response;

using MvUrl = QianShi.Music.Common.Models.MvUrl;

namespace QianShi.Music.Services;

public class VideoPlayStoreService : IVideoPlayStoreService
{
    private readonly IPlaylistService _playlistService;
    private readonly IVideoPlayService _videoPlayService;
    private long _mvId;
    private MvDetail? _current;
    private MvUrl? _url;

    public event EventHandler<PropertyChangedEventArgs<MvDetail?>>? CurrentChanged;

    public MvDetail? Current
    {
        get => _current;
        private set
        {
            if (_current == value) return;
            var oldValue = _current;
            _current = value;
            CurrentChanged?.Invoke(this, new(_current, oldValue));
        }
    }

    public MvUrl? Url
    {
        get => _url;
        private set
        {
            if (Equals(_url, value)) return;
            if (_url != null) _url.IsActive = false;
            if (value != null)
            {
                value.IsActive = true;
                _videoPlayService.Url = value.Url;
            }
            _url = value;
        }
    }

    public ObservableCollection<MvUrl> MvUrls { get; } = new();
    public ObservableCollection<MovieVideo> Related { get; } = new();
    public bool IsPlaying { get; private set; }

    public VideoPlayStoreService(IPlaylistService playlistService, IVideoPlayService videoPlayService)
    {
        _playlistService = playlistService;
        _videoPlayService = videoPlayService;

        IsPlaying = _videoPlayService.IsPlaying;
        _videoPlayService.IsPlayingChanged += (s, e) => IsPlaying = e.NewValue;
    }

    public async Task SetMovieVideo(long mvId)
    {
        if (mvId == _mvId) return;
        _mvId = mvId;
        await GetMvDetail();
        await GetSimiMv();
    }

    private async Task GetMvDetail()
    {
        Url = null;
        var detailResponse = await _playlistService.MvDetail(_mvId);

        if (detailResponse.Code == 200)
        {
            Current = detailResponse.Data;
            MvUrls.Clear();
            foreach (var br in Current.Brs)
            {
                var urlResponse = await _playlistService.MvUrl(new Common.Models.Request.MvUrlRequest
                {
                    Id = _mvId,
                    R = br.Br
                });
                if (urlResponse.Code == 200)
                {
                    MvUrls.Add(new Common.Models.MvUrl()
                    {
                        Br = br.Br,
                        Url = urlResponse.Data.Url
                    });
                }
            }

            if (MvUrls.Count > 0) Url = MvUrls[^1];
        }
    }

    private async Task GetSimiMv()
    {
        var response = await _playlistService.SimiMv(_mvId);

        if (response.Code == 200)
        {
            Related.Clear();
            Related.AddRange(response.Mvs);
        }
    }

    public void SetUrl(MvUrl mvUrl)
    {
        var isPlaying = IsPlaying;
        if (isPlaying) Pause();
        Url = mvUrl;
        _videoPlayService.SetProgress(0);
        Play();
    }

    public void Play()
    {
        if (IsPlaying || Url == null) return;
        _videoPlayService.Play(Url.Url);
    }

    public void Pause()
    {
        if (!IsPlaying) return;
        _videoPlayService.Pause();
    }
}