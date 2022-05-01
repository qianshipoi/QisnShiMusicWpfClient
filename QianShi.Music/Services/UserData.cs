using Prism.Mvvm;

using System.IO;
using System.Text.Json;

namespace QianShi.Music.Services
{
    public class LoginChangedEventArgs : EventArgs
    {
        public bool OldVal { get; set; }
        public bool NewVal { get; set; }

        public LoginChangedEventArgs(bool newVal, bool oldVal)
        {
            NewVal = newVal;
            OldVal = oldVal;
        }
    }

    public class UserData : BindableBase
    {
        public const string SettingFileName = "Setting.json";
        private static UserData? _instance;
        public static UserData Instance => _instance ?? (_instance = Create());

        public static UserData Create()
        {
            var settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingFileName);
            if (File.Exists(settingPath))
            {
                var jsonData = File.ReadAllText(settingPath);
                return JsonSerializer.Deserialize<UserData>(jsonData, new JsonSerializerOptions()
                {
                }) ?? new UserData();
            }
            return new UserData();
        }

        public event EventHandler<LoginChangedEventArgs>? LoginChanged;

        private bool _isLogin;

        public bool IsLogin
        {
            get => _isLogin;
            set
            {
                if (_isLogin != value && LoginChanged != null)
                {
                    LoginChanged.Invoke(this, new LoginChangedEventArgs(value, _isLogin));
                }
                SetProperty(ref _isLogin, value);
            }
        }

        private string? _cover;
        public string? Cover { get => _cover; set => SetProperty(ref _cover, value); }
        private string? _nickname;
        public string? NickName { get => _nickname; set => SetProperty(ref _nickname, value); }
        public string? Cookie { get; set; }

        public void Clear()
        {
            IsLogin = false;
            Cover = null;
            NickName = null;
            Cookie = null;
        }

        public void Save()
        {
            var settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingFileName);
            var jsonData = JsonSerializer.Serialize(this);
            File.WriteAllText(settingPath, jsonData);
        }
    }
}