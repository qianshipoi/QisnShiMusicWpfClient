using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Common.UserControls
{
    public class PlaylistDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fe = container as FrameworkElement;
            DataTemplate? dt = null;

            if (fe != null)
            {
                switch (item)
                {
                    case Playlist:
                        dt = fe.FindResource("PlaylistItemTemplate") as DataTemplate;
                        break;

                    case PersonalizedPlaylist:
                        dt = fe.FindResource("PersonalizedPlaylistItemTemplate") as DataTemplate;
                        break;

                    case Toplist:
                        dt = fe.FindResource("ToplistItemTemplate") as DataTemplate;
                        break;

                    default:
                        break;
                }
            }

            return dt ?? new DataTemplate();
        }
    }
}