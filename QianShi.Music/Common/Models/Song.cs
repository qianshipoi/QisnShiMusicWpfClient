namespace QianShi.Music.Common.Models.Response
{
    public partial class Song : BindableBase
    {
        private bool _isPlaying;

        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty(ref _isPlaying, value);
        }

        public string? Url { get; set; }

        private bool _isLike;

        public bool IsLike
        {
            get => _isLike;
            set => SetProperty(ref _isLike, value);
        }
    }
}