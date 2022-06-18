namespace QianShi.Music.ViewModels
{
    public class SettingViewModel : NavigationViewModel
    {
        public SettingViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            Title = "Setting View";
        }
    }
}