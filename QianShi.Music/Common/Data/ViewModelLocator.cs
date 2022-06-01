using Prism.Ioc;

using QianShi.Music.ViewModels;

namespace QianShi.Music.Common.Data
{
    public class ViewModelLocator
    {
        public PlayingListViewModel DesignPlayingListViewModel => App.Current.Container.Resolve<PlayingListViewModel>();
    }
}