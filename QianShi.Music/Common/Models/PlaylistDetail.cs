using QianShi.Music.Common.Models.Response;
using QianShi.Music.Models;

namespace QianShi.Music.Common.Models
{
    public class PlaylistDetail : BindableBase, IDataModel
    {
        private long _id;
        private string? _picUrl = "https://oss.kuriyama.top/static/background.png";
        private string? _name;
        private long _lastUpdateTime;
        private string? _description;
        private int _count;
        private string? _creator;
        private long _creatorId;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string? PicUrl
        {
            get => _picUrl;
            set => SetProperty(ref _picUrl, value);
        }
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public long LastUpdateTime
        {
            get => _lastUpdateTime;
            set => SetProperty(ref _lastUpdateTime, value);
        }
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
        public string? Creator
        {
            get => _creator;
            set => SetProperty(ref _creator, value);
        }
        public long CreatorId
        {
            get => _creatorId;
            set => SetProperty(ref _creatorId, value);
        }

        public bool HasMore => Songs.Count < Count;

        public PlaylistDetail()
        {
            Songs = new();
            SongsIds = new();
        }

        public void AddSongs(IEnumerable<Song> songs)
        {
            Songs.AddRange(songs);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMore)));
        }

        public List<Song> Songs { get; private set; }

        public List<long> SongsIds { get; set; }
    }
}