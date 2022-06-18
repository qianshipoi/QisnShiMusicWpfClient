namespace QianShi.Music.Common.Helpers
{
    public class ItemsControlHelper
    {
        #region MarkIndex

        public static readonly DependencyProperty MarkIndexProperty = DependencyProperty.RegisterAttached(
            "MarkIndex", typeof(bool), typeof(ItemsControlHelper), new PropertyMetadata(default(bool), OnMarkIndexPropertyChanged));

        public static bool GetMarkIndex(DependencyObject obj)
        {
            return (bool)obj.GetValue(MarkIndexProperty);
        }

        public static void SetMarkIndex(DependencyObject obj, bool value)
        {
            obj.SetValue(MarkIndexProperty, value);
        }

        private static void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs itemsChangedEventArgs)
        {
            var itemContainerGenerator = (ItemContainerGenerator)sender;

            if (itemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                for (int i = 0; i < itemContainerGenerator.Items.Count; i++)
                {
                    var dp = itemContainerGenerator.ContainerFromIndex(i);

                    if (dp != null)
                    {
                        var oldIndex = (int)dp.GetValue(ItemIndexProperty);
                        if (oldIndex != i)
                        {
                            dp.SetValue(ItemIndexProperty, i);
                        }
                    }
                }
            }
        }

        private static void ItemContainerGeneratorOnStatusChanged(object sender, EventArgs eventArgs)
        {
            var itemContainerGenerator = (ItemContainerGenerator)sender;

            if (itemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                for (int i = 0; i < itemContainerGenerator.Items.Count; i++)
                {
                    var dp = itemContainerGenerator.ContainerFromIndex(i);

                    if (dp != null)
                    {
                        var oldIndex = (int)dp.GetValue(ItemIndexProperty);
                        if (oldIndex != i)
                        {
                            dp.SetValue(ItemIndexProperty, i);
                        }
                    }
                }
            }
        }

        private static void OnMarkIndexPropertyChanged(DependencyObject dependencyObject,
                            DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue)
            {
                var itemsControl = dependencyObject as ItemsControl;
                if (itemsControl != null)
                {
#pragma warning disable CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
                    itemsControl.ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorOnStatusChanged;
#pragma warning restore CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
                    itemsControl.ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorOnItemsChanged;

#pragma warning disable CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
                    itemsControl.ItemContainerGenerator.StatusChanged += ItemContainerGeneratorOnStatusChanged;
#pragma warning restore CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
                    itemsControl.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
                }
            }
            else
            {
                var itemsControl = dependencyObject as ItemsControl;
                if (itemsControl != null)
                {
#pragma warning disable CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
                    itemsControl.ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorOnStatusChanged;
#pragma warning restore CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
                    itemsControl.ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorOnItemsChanged;
                }
            }
        }

        #endregion MarkIndex

        #region ItemIndex

        public static readonly DependencyProperty ItemIndexProperty = DependencyProperty.RegisterAttached(
            "ItemIndex", typeof(int), typeof(ItemsControlHelper), new PropertyMetadata(default(int)));

        public static int GetItemIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(ItemIndexProperty);
        }

        public static void SetItemIndex(DependencyObject obj, bool value)
        {
            obj.SetValue(ItemIndexProperty, value);
        }

        #endregion ItemIndex
    }
}