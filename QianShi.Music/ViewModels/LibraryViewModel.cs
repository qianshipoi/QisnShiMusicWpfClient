using Prism.Ioc;

namespace QianShi.Music.ViewModels
{
    public class LibraryViewModel : NavigationViewModel
    {
        public LibraryViewModel() : base(App.Current.Container.Resolve<IContainerProvider>())
        {
        }

        public LibraryViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
        }
    }
}