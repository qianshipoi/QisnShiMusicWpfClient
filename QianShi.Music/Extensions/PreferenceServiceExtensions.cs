using QianShi.Music.Services;

namespace QianShi.Music.Extensions
{
    public static class PreferenceServiceExtensions
    {
        private const string ColorR = nameof(ColorR);
        private const string ColorG = nameof(ColorG);
        private const string ColorB = nameof(ColorB);
        private const string ColorA = nameof(ColorA);
        private const string BaseTheme = nameof(BaseTheme);

        public static void InitTheme(this IPreferenceService preferenceService)
        {
            var helper = new PaletteHelper();
            var color = preferenceService.GetCurrentColor();
            if (color != null)
            {
                helper.ChangePrimaryColor(color.Value);
            }

            var isDark = preferenceService.GetCurrentModeIsDark();
            helper.ChangeBaseTheme(isDark ? Theme.Dark : Theme.Light);
        }

        public static Color? GetCurrentColor(this IPreferenceService preferenceService)
        {
            if (preferenceService.ContainsKey(ColorR) &&
               preferenceService.ContainsKey(ColorG) &&
               preferenceService.ContainsKey(ColorB) &&
               preferenceService.ContainsKey(ColorA))
            {
                var r = (byte)preferenceService.Get(ColorR, -1);
                var g = (byte)preferenceService.Get(ColorG, -1);
                var b = (byte)preferenceService.Get(ColorB, -1);
                var a = (byte)preferenceService.Get(ColorA, -1);
                return Color.FromArgb(a, r, g, b);
            }
            return null;
        }

        public static bool GetCurrentModeIsDark(this IPreferenceService preferenceService)
        {
            if (preferenceService.ContainsKey(BaseTheme))
            {
                return preferenceService.Get(BaseTheme, false);
            }
            return false;
        }

        public static void SaveTheme(this IPreferenceService preferenceService, Color color)
            => preferenceService.SaveTheme(color, null);

        public static void SaveTheme(this IPreferenceService preferenceService, bool isDark)
            => preferenceService.SaveTheme(null, isDark);

        public static void SaveTheme(this IPreferenceService preferenceService, Color? color, bool? isDark)
        {
            if (color.HasValue)
            {
                preferenceService.Set(ColorR, color.Value.R);
                preferenceService.Set(ColorG, color.Value.G);
                preferenceService.Set(ColorB, color.Value.B);
                preferenceService.Set(ColorA, color.Value.A);
            }

            if (isDark.HasValue)
            {
                preferenceService.Set(BaseTheme, isDark.Value);
            }
        }

        public static void ClearColor(this IPreferenceService preferenceService)
        {
            preferenceService.RemoveKey(ColorR);
            preferenceService.RemoveKey(ColorG);
            preferenceService.RemoveKey(ColorB);
            preferenceService.RemoveKey(ColorA);
        }

        public static void ClearIsDark(this IPreferenceService preferenceService)
        {
            preferenceService.RemoveKey(BaseTheme);
        }
    }
}
