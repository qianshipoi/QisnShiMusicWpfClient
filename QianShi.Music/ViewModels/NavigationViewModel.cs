using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace QianShi.Music.ViewModels
{
    public class NavigationViewModel : BindableBase, IConfirmNavigationRequest
    {
        public readonly IEventAggregator _aggregator;
        private readonly IContainerProvider _containerProvider;
        private bool _isBusy;
        private string? _title;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            _aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _ = SetProperty(ref _isBusy, value); }
        }

        public string? Title
        {
            get => _title;
            set { _ = SetProperty(ref _title, value); }
        }
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public void UpdateLoding(bool isOpen)
        {
        }
    }
}