using QianShi.Music.Common.Models;

namespace QianShi.Music.Common.UserControls
{
    public class ItemsControlsDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate? dt = null;
            if (item is Cat obj && container is FrameworkElement fe && fe != null)
            {
                if (obj.IsLastOne)
                {
                    dt = fe.FindResource("MoreItemTemplate") as DataTemplate;
                }
                else
                {
                    dt = fe.FindResource("SelectItemTemplate") as DataTemplate;
                }
            }
            return dt ?? new DataTemplate();
        }
    }
}