using QianShi.Music.Common.Models.Response;

using MvUrl = QianShi.Music.Common.Models.MvUrl;

namespace QianShi.Music.Services
{
    public interface IVideoPlayStoreService
    {
        event EventHandler<PropertyChangedEventArgs<MvDetail?>>? CurrentChanged;

        MvDetail? Current { get; }
        MvUrl? Url { get; }
        ObservableCollection<MvUrl> MvUrls { get; }
        ObservableCollection<MovieVideo> Related { get; }
        bool IsPlaying { get; }

        Task SetMovieVideo(long mvId);

        void SetUrl(MvUrl mvUrl);

        void Play();

        void Pause();
    }
}