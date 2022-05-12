using Prism.Mvvm;

namespace QianShi.Music.Common.Models.Response
{
    public partial class Song : BindableBase
    {
        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { SetProperty(ref _isPlaying, value); }
        }

        public string? Url { get; set; }
    }
}