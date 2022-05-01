using QianShi.Music.Common.Models.Response;

using System.Windows;
using System.Windows.Controls;

namespace QianShi.Music.Common.UserControls
{
    public class DefaultDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fe = container as FrameworkElement;
            DataTemplate? dt = null;

            if (fe != null)
            {
                switch (item)
                {
                    case Artist:
                        dt = fe.FindResource("ArtistDataTemplate") as DataTemplate;
                        break;

                    case Album:
                        dt = fe.FindResource("AlbumDataTemplate") as DataTemplate;
                        break;

                    case Song:
                        dt = fe.FindResource("SongDataTemplate") as DataTemplate;
                        break;

                    case MovieVideo:
                        dt = fe.FindResource("MovieVideoDataTemplate") as DataTemplate;
                        break;

                    case Playlist:
                        dt = fe.FindResource("PlaylistDataTemplate") as DataTemplate;
                        break;

                    default:
                        break;
                }
            }

            return dt ?? new DataTemplate();
        }
    }

    public class DefaultItemContainerStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var fe = container as FrameworkElement;
            Style? dt = null;

            if (fe != null)
            {
                switch (item)
                {
                    case Artist:
                        dt = fe.FindResource("UniformGrid6") as Style;
                        break;

                    case Album:
                    case MovieVideo:
                    case Playlist:
                        dt = fe.FindResource("UniformGrid5") as Style;
                        break;

                    case Song:
                    default:
                        break;
                }
            }

            return dt ?? new Style();
        }
    }
}