using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models;

namespace QianShi.Music.ViewModels
{
    public class LoginViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        private LoginMode _loginMode = LoginMode.QrCode;
        public LoginMode LoginMode
        {
            get { return _loginMode; }
            set { SetProperty(ref _loginMode, value); }
        }

        private DelegateCommand<LoginMode?> _switchLoginModeCommand = default!;
        public DelegateCommand<LoginMode?> SwitchLoginModeCommand =>
            _switchLoginModeCommand ?? (_switchLoginModeCommand = new DelegateCommand<LoginMode?>(ExecuteSwitchLoginModeCommand));

        void ExecuteSwitchLoginModeCommand(LoginMode? mode)
        {
            LoginMode = mode ?? LoginMode;
        }

        public LoginViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
        }

        public bool KeepAlive => true;
    }

}
