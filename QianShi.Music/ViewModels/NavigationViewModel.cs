using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace QianShi.Music.ViewModels
{
    public class NavigationViewModel : BindableBase, IConfirmNavigationRequest
    {
        private readonly IContainerProvider _containerProvider;

        public readonly IEventAggregator _aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            _aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public void UpdateLoding(bool isOpen)
        {
        }
    }
}