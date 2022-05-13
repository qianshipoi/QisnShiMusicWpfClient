using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Services
{
    public interface IPlayService
    {
        event EventHandler<ProgressEventArgs>? ProgressChanged;
        event EventHandler<PropertyChangedEventArgs<bool>>? IsMutedChanged;
        event EventHandler<PropertyChangedEventArgs<double>>? VolumeChanged;
        event EventHandler<PropertyChangedEventArgs<bool>>? IsPlayingChanged;
        event EventHandler? PlayEnded;

        double Volume { get; }
        bool IsMuted { get; }
        bool IsPlaying { get; }

        void Play();
        void Play(string url);
        void Pause();
        void SetVolume(double volume);
        void SetMute(bool isMute);
        void SetProgress(double value);
    }

    public class SongChangedEventArgs : EventArgs
    {
        public Song? NewSong { get; private set; }

        public SongChangedEventArgs(Song? song)
        {
            NewSong = song;
        }
    }

    public class IsPlayingChangedEventArgs : EventArgs
    {
        public bool IsPlaying { get; private set; }

        public IsPlayingChangedEventArgs(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }
    }

    public class PropertyChangedEventArgs<T> : EventArgs
    {
        public T? NewValue { get; private set; }

        public T? OldValue { get; private set; }

        public PropertyChangedEventArgs(T? newValue, T? oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public double Value { get; set; }
        public double Total { get; set; }

        public ProgressEventArgs(double value, double total)
        {
            Value = value;
            Total = total;
        }
    }

    public class VolumeChangedEventArgs : EventArgs
    {
        public double Value { get; set; }

        public VolumeChangedEventArgs(double value)
        {
            Value = value;
        }
    }
}