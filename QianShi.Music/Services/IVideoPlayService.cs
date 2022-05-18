using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QianShi.Music.Services
{
    public interface IVideoPlayService
    {
        event EventHandler<ProgressEventArgs>? ProgressChanged;
        event EventHandler<PropertyChangedEventArgs<bool>>? IsPlayingChanged;
        event EventHandler<PropertyChangedEventArgs<double>>? VolumeChanged;
        event EventHandler<PropertyChangedEventArgs<bool>>? IsMutedChanged;
        event EventHandler? PlayEnded;

        FrameworkElement Control { get; }
        bool IsPlaying { get; }
        bool IsMuted { get; }
        double Volume { get; }
        string Url { get; set; }

        void Play(string url);
        void Play();
        void Pause();
        void SetVolume(double value);
        void SetProgress(double value);
        void SetMute(bool isMute);
    }
}
