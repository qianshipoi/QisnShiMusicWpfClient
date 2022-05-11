using QianShi.Music.ViewModels;
using Prism.Ioc;

namespace QianShi.Music.Common.Data
{
    public class ViewModelLocator
    {
        public PlayingListViewModel DesignPlayingListViewModel => App.Current.Container.Resolve<PlayingListViewModel>();
    }
}
