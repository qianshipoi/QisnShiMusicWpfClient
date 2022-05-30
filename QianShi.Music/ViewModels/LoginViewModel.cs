using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models;
using QianShi.Music.Services;

using QRCoder;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using QianShi.Music.Views;

namespace QianShi.Music.ViewModels
{
    public class LoginViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string NavigationName = nameof(LoginView);
        public const string ParameterRedirectUri = "RedirectUri";

        private readonly IPlaylistService _playlistService;
        private readonly IRegionManager _regionManager;
        private readonly UserData _userData;
        private IRegionNavigationJournal _journal = default!;
        private string? _qrKey;
        private DispatcherTimer? _dispatcherTimer;
        private string? _redirectUri;

        private LoginMode _loginMode = LoginMode.QrCode;

        public LoginMode LoginMode
        {
            get { return _loginMode; }
            set { SetProperty(ref _loginMode, value); }
        }

        private DelegateCommand<LoginMode?> _switchLoginModeCommand = default!;

        public DelegateCommand<LoginMode?> SwitchLoginModeCommand =>
            _switchLoginModeCommand ??= new(ExecuteSwitchLoginModeCommand);

        private void ExecuteSwitchLoginModeCommand(LoginMode? mode)
        {
            LoginMode = mode ?? LoginMode;
            if (mode == LoginMode.QrCode)
            {
                _ = CreateQrCode();
            }
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
                        _userData.IsLogin = true;
                        _userData.Cookie = response.Cookie;
                        var statusResponse = await _playlistService.LoginStatus();
                        if (statusResponse.Data.Code == 200)
                        {
                            _userData.NickName = statusResponse.Data.Profile?.Nickname;
                            _userData.Cover = statusResponse.Data.Profile?.AvatarUrl;
                            _userData.Id = statusResponse.Data.Account?.Id ?? 0;
                            _userData.VipType = statusResponse.Data.Account?.VipType ?? 0;
                        }
                        _userData.Save();
                        Back();
                    }
                }
            };
            _dispatcherTimer.Start();
        }

        private void EndCheck()
        {
            if (_dispatcherTimer == null) return;
            _dispatcherTimer.Stop();
            _dispatcherTimer = null;
        }

        /// <summary>
        /// 返回页面
        /// </summary>
        private void Back()
        {
            EndCheck();
            _journal.GoBack();
        }

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

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        private ImageSource? _qrCodeSource;

        public ImageSource? QrCodeSource
        {
            get { return _qrCodeSource; }
            set { SetProperty(ref _qrCodeSource, value); }
        }

        public LoginViewModel(IContainerProvider containerProvider, IPlaylistService playlistService, IRegionManager regionManager) : base(containerProvider)
        {
            _playlistService = playlistService;
            _regionManager = regionManager;
            _userData = UserData.Instance;
        }

        public bool KeepAlive => false;
    }
}