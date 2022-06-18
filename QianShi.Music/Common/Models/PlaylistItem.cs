namespace QianShi.Music.Common.Models
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
}