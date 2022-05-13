using Prism.Mvvm;

namespace QianShi.Music.Common.Models
{
    public class PlaylistDetail : BindableBase
    {
        public long Id { get; set; }
        private string? _picUrl = "https://oss.kuriyama.top/static/background.png";
        public string? PicUrl { get => _picUrl; set => SetProperty(ref _picUrl, value); }
        private string? _name;
        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        private long _lastUpdateTime;
        public long LastUpdateTime { get => _lastUpdateTime; set => SetProperty(ref _lastUpdateTime, value); }
        private string? _description;
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        private int _count;
        public int Count { get => _count; set => SetProperty(ref _count, value); }
        private string? _creator;
        public string? Creator { get => _creator; set => SetProperty(ref _creator, value); }
    }

}
