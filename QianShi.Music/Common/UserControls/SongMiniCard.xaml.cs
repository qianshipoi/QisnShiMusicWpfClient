using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// SongMiniCard.xaml 的交互逻辑
    /// </summary>
    public partial class SongMiniCard : UserControl
    {
        private INavigationService _navigationService => App.Current.Container.Resolve<INavigationService>();

        public SongMiniCard()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.DataContext is Artist artist)
                {
                    _navigationService.NavigateToArtist(artist.Id);
                }
            }
        }

        private void FilletImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FilletImage image)
            {
                if (image.DataContext is Song song)
                {
                    _navigationService.NavigateToAlbum(song.Album.Id);
                }
            }
        }
    }
}