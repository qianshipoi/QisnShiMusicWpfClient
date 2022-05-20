using System.Windows;
using System.Windows.Controls;

namespace QianShi.Music.Extensions;

public class MediaElementAttached : DependencyObject
{
    #region IsPlaying

    public static readonly DependencyProperty IsPlayingProperty =
        DependencyProperty.RegisterAttached("IsPlaying", typeof(bool), typeof(MediaElementAttached), new PropertyMetadata(false, OnIsPlayingChanged));

    public static bool GetIsPlaying(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsPlayingProperty);
    }

    public static void SetIsPlaying(DependencyObject obj, bool value)
    {
        obj.SetValue(IsPlayingProperty, value);
    }

    public static void OnIsPlayingChanged(DependencyObject obj,
        DependencyPropertyChangedEventArgs args)
    {
        if (obj is MediaElement me)
        {
            bool isPlaying = (bool)args.NewValue;
            if (isPlaying)
                me.Play();
            else
                me.Pause();
        }
    }

    #endregion

    #region Progress

    public static DependencyProperty ProgressProperty =
        DependencyProperty.RegisterAttached(
            "Progress", typeof(TimeSpan), typeof(MediaElementAttached),
            new PropertyMetadata(new TimeSpan(0, 0, 0), OnProgressChanged));
    public static TimeSpan GetProgress(DependencyObject d)
    {
        return (TimeSpan)d.GetValue(ProgressProperty);
    }
    public static void SetProgress(DependencyObject d, TimeSpan value)
    {
        d.SetValue(ProgressProperty, value);
    }
    private static void OnProgressChanged(
        DependencyObject obj,
        DependencyPropertyChangedEventArgs args)
    {
        if (obj is MediaElement me)
        {
            me.LoadedBehavior = MediaState.Manual;
            me.Position = (TimeSpan)args.NewValue;
        }
    }

    #endregion

    #region Volume

    public static DependencyProperty VolumeProperty =
        DependencyProperty.RegisterAttached(
            "Volume", typeof(double), typeof(MediaElementAttached),
            new PropertyMetadata(0.5d, OnVolumeChanged));
    public static TimeSpan GetVolume(DependencyObject d)
    {
        return (TimeSpan)d.GetValue(VolumeProperty);
    }
    public static void SetVolume(DependencyObject d, double value)
    {
        d.SetValue(VolumeProperty, value);
    }
    private static void OnVolumeChanged(
        DependencyObject obj,
        DependencyPropertyChangedEventArgs args)
    {
        if (obj is MediaElement me && args.NewValue is double value)
        {
            if (value > 1) value = 1;
            else if (value < 0) value = 0;
            me.Volume = value;
        }
    }

    #endregion
}