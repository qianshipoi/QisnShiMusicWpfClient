namespace QianShi.Music.Services
{
    public interface IVideoPlayService
    {
        event EventHandler<ProgressEventArgs>? ProgressChanged;

        event EventHandler<PropertyChangedEventArgs<bool>>? IsPlayingChanged;

        event EventHandler<PropertyChangedEventArgs<double>>? VolumeChanged;

        event EventHandler<PropertyChangedEventArgs<bool>>? IsMutedChanged;

        event EventHandler<PropertyChangedEventArgs<bool>>? IsFullScreenChanged;

        event EventHandler? PlayEnded;

        FrameworkElement Control { get; }
        string Url { get; set; }
        bool IsPlaying { get; }
        bool IsMuted { get; }
        double Volume { get; }
        double Total { get; }
        double Position { get; }
        bool IsFullScreen { get; }

        void Play(string url);

        void Play();

        void Pause();

        void SetVolume(double value);

        void SetProgress(double value);

        void SetMute(bool isMute);

        void Stop();

        void FullScreen();
    }
}