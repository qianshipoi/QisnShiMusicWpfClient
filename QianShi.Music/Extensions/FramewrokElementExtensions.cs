namespace QianShi.Music.Extensions
{
    public static class FramewrokElementExtensions
    {
        /// <summary>
        /// WPF中查找元素的父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject element) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            if (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                else
                {
                    parent = FindParent<T>(parent);
                    if (parent != null && parent is T)
                    {
                        return (T)parent;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查找子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject element) where T : DependencyObject
        {
            if(element != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);
                    if(child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    if(child != null)
                    {
                        foreach (T childOfChild in FindVisualChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }
    }
}
