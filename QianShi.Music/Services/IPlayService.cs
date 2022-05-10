using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Services
{
    public interface IPlayService
    {
        event EventHandler<SongChangedEventArgs> CurrentChanged;

        event EventHandler<IsPlayingChangedEventArgs> IsPlayingChanged;

        event EventHandler<ProgressEventArgs> ProgressChanged;

        /// <summary>
        /// 当前歌曲
        /// </summary>
        Song? Current { get; }

        bool IsPlaying { get; }
        /// <summary>
        /// 播放
        /// </summary>
        void Play();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();
        /// <summary>
        /// 下一曲
        /// </summary>
        void Next();
        /// <summary>
        /// 上一曲
        /// </summary>
        void Previous();

        void Add(Song song);

        void Add(IEnumerable<Song> songs);

        void Remove(Song song);

        void Clear();

        void JumpToQueue(Song song);
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

        public IsPlayingChangedEventArgs( bool isPlaying)
        {
            IsPlaying = isPlaying;
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
}
