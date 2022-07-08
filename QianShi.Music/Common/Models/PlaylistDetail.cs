using QianShi.Music.Common.Models.Response;
using QianShi.Music.Models;

namespace QianShi.Music.Common.Models
{
    public class PlaylistDetail : BindableBase, IDataModel
    {
        private long _id;
        public long Id { get => _id; set => SetProperty(ref _id, value); }
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

        private long _creatorId;

        public long CreatorId
        {
            get => _creatorId;
            set => SetProperty(ref _creatorId, value);
        }

        public PlaylistDetail()
        {
            Songs = new List<Song>();
        }

        public List<Song> Songs { get; set; } 
    }
}