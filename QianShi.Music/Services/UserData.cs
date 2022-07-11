namespace QianShi.Music.Services
{
    public class UserData : BindableBase
    {
        public const string SettingFileName = "Setting.json";
        private static UserData? _instance;
        public static UserData Instance => _instance ??= Create();

        public static UserData Create()
        {
            var settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingFileName);
            if (File.Exists(settingPath))
            {
                var jsonData = File.ReadAllText(settingPath);
                return JsonSerializer.Deserialize<UserData>(jsonData, new JsonSerializerOptions()) ?? new UserData();
            }
            return new UserData();
        }

        public event EventHandler<PropertyChangedEventArgs<bool>>? LoginChanged;

        private bool _isLogin;

        public bool IsLogin
        {
            get => _isLogin;
            set
            {
                if (_isLogin != value && LoginChanged != null)
                {
                    LoginChanged.Invoke(this, new PropertyChangedEventArgs<bool>(value, _isLogin));
                }
                SetProperty(ref _isLogin, value);
            }
        }

        private string? _cover;
        public string? Cover { get => _cover; set => SetProperty(ref _cover, value); }
        private string? _nickname;
        public string? NickName { get => _nickname; set => SetProperty(ref _nickname, value); }
        public string? Cookie { get; set; }
        public long Id { get; set; }
        public sbyte VipType { get; set; }

        public async Task LogoutAsync()
        {
            var playlistService = App.Current.Container.Resolve<IPlaylistService>();

            var response = await playlistService.Logout();
            if (response.Code == 200)
            {
                Clear();
                Save();
            }
        }

        public void Clear()
        {
            IsLogin = false;
            Cover = null;
            NickName = null;
            Cookie = null;
            Id = 0;
            VipType = 0;
        }

        public void Save()
        {
            var settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingFileName);
            var jsonData = JsonSerializer.Serialize(this);
            File.WriteAllText(settingPath, jsonData);
        }
    }
}