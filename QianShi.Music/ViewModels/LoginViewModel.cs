using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Helpers;
using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Request;
using QianShi.Music.Services;
using QianShi.Music.Views;

using QRCoder;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace QianShi.Music.ViewModels
{
    public class LoginViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string NavigationName = nameof(LoginView);
        public const string ParameterRedirectUri = "RedirectUri";

        private readonly IPlaylistService _playlistService;
        private string? _account;
        private DispatcherTimer? _dispatcherTimer;
        private IRegionNavigationJournal _journal = default!;
        private DelegateCommand _loginCommand = default!;
        private LoginMode _loginMode = LoginMode.Email;
        private string? _password;
        private ImageSource? _qrCodeSource;
        private string? _qrKey;
        private string? _redirectUri;
        private DelegateCommand<LoginMode?> _switchLoginModeCommand = default!;
        public LoginViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
        }

        public string? Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        public bool KeepAlive => false;
        public DelegateCommand LoginCommand =>
            _loginCommand ??= new(ExecuteLoginCommand);

        public LoginMode LoginMode
        {
            get { return _loginMode; }
            set { SetProperty(ref _loginMode, value); }
        }

        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ImageSource? QrCodeSource
        {
            get => _qrCodeSource;
            set => SetProperty(ref _qrCodeSource, value);
        }

        public DelegateCommand<LoginMode?> SwitchLoginModeCommand =>
            _switchLoginModeCommand ??= new(ExecuteSwitchLoginModeCommand);

        private UserData _userData => UserData.Instance;
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _journal = navigationContext.NavigationService.Journal;

            var parameters = navigationContext.Parameters;
            if (parameters.ContainsKey(NavigationName))
            {
                _redirectUri = parameters.GetValue<string>(ParameterRedirectUri);
            }
        }

        /// <summary>
        /// 返回页面
        /// </summary>
        private void Back()
        {
            EndCheck();
            _journal.GoBack();
        }

        private async Task CreateQrCode()
        {
            if (string.IsNullOrWhiteSpace(_qrKey))
            {
                var response = await _playlistService.LoginQrKey();
                if (response.Code != 200 || response.Data.Code != 200)
                {
                    return;
                }
                _qrKey = response.Data.Unikey;
            }

            var createResponse = await _playlistService.LoginQrCreate(_qrKey);
            if (createResponse.Code != 200) return;

            var url = createResponse.Data.Qrurl;

            var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M);
            var qrCode = new QRCode(qrData);

            var qrCodeImage = qrCode.GetGraphic(10, System.Drawing.Color.Black, System.Drawing.Color.White, true);

            var hBitmap = qrCodeImage.GetHbitmap();

            QrCodeSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            StartCheck();
        }

        private async Task EmailLogin()
        {
            var response = await _playlistService.Login(new LoginRequest()
            {
                Email = Account!,
                Md5Password = MD5Helper.MD5Encrypt32(Password!).ToLower(),
                Time = DateTime.Now.Ticks
            });

            if (response.Code != 200)
            {
                MessageBox.Show(response.Msg);
                return;
            }
            LoginSuccess(
                response.Profile?.Nickname,
                response.Profile?.AvatarUrl,
                response.Account?.Id ?? 0,
                response.Account?.VipType ?? 0,
                response.Cookie);
        }

        private void EndCheck()
        {
            if (_dispatcherTimer == null) return;
            _dispatcherTimer.Stop();
            _dispatcherTimer = null;
        }

        private void ExecuteLoginCommand()
        {
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("请完善信息");
                return;
            }

            if (LoginMode == LoginMode.Email)
            {
                _ = EmailLogin();
            }
            else if (LoginMode == LoginMode.Phone)
            {
                _ = PhoneLogin();
            }
        }

        private void ExecuteSwitchLoginModeCommand(LoginMode? mode)
        {
            LoginMode = mode ?? LoginMode;
            if (mode == LoginMode.QrCode)
            {
                _ = CreateQrCode();
            }
            else if (mode == LoginMode.Phone)
            {
                EndCheck();
            }
        }

        private void LoginSuccess(string? nickname, string? avatarUrl, long id, sbyte vipType, string? cookie)
        {
            _userData.IsLogin = true;
            _userData.Cookie = cookie;
            _userData.NickName = nickname;
            _userData.Cover = avatarUrl;
            _userData.Id = id;
            _userData.VipType = vipType;
            _userData.Save();
            Back();
        }

        private async Task PhoneLogin()
        {
            var response = await _playlistService.LoginCellPhone(new()
            {
                Phone = Account!,
                Md5Password = MD5Helper.MD5Encrypt32(Password!).ToLower(),
                Time = DateTime.Now.Ticks
            });

            if (response.Code != 200)
            {
                MessageBox.Show(response.Msg);
                return;
            }
            LoginSuccess(
                response.Profile?.Nickname,
                response.Profile?.AvatarUrl,
                response.Account?.Id ?? 0,
                response.Account?.VipType ?? 0,
                response.Cookie);
        }

        private void StartCheck()
        {
            EndCheck();
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _dispatcherTimer.Tick += async (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(_qrKey))
                {
                    var response = await _playlistService.LoginQrCheck(_qrKey);
                    if (response.Code == 800)
                    {
                        _qrKey = null;
                        await CreateQrCode();
                    }
                    else if (response.Code == 803)
                    {
                        var statusResponse = await _playlistService.LoginStatus();
                        if (statusResponse.Data.Code == 200)
                        {
                            LoginSuccess(
                                statusResponse.Data.Profile?.Nickname,
                                statusResponse.Data.Profile?.AvatarUrl,
                                statusResponse.Data.Account?.Id ?? 0,
                                statusResponse.Data.Account?.VipType ?? 0,
                                response.Cookie);
                        }
                        else
                        {
                            MessageBox.Show("用户信息获取异常，请重新尝试。");
                            await CreateQrCode();
                        }
                    }
                }
            };
            _dispatcherTimer.Start();
        }
    }
}