using MaterialDesignColors;

using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class SettingViewModel : NavigationViewModel
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private readonly IPreferenceService _preferenceService;

        private Color? _primaryColor;
        private Color? _secondaryColor;
        private Color? _primaryForegroundColor;
        private Color? _secondaryForegroundColor;

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        private ColorScheme _activeScheme;
        public ColorScheme ActiveScheme
        {
            get => _activeScheme;
            set => SetProperty(ref _activeScheme, value);
        }

        private Color? _selectedColor;
        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    SetProperty(ref _selectedColor, value);
                    var currentSchemeColor = ActiveScheme switch
                    {
                        ColorScheme.Primary => _primaryColor,
                        ColorScheme.Secondary => _secondaryColor,
                        ColorScheme.PrimaryForeground => _primaryForegroundColor,
                        ColorScheme.SecondaryForeground => _secondaryForegroundColor,
                        _ => throw new NotSupportedException($"{ActiveScheme} is not a handled ColorScheme.. Ye daft programmer!")
                    };

                    if (_selectedColor != currentSchemeColor && value is Color color)
                    {
                        ChangeCustomColor(color);
                    }
                }
            }
        }

        private bool _isDark;
        public bool IsDark
        {
            get => _isDark;
            set
            {
                if (SetProperty(ref _isDark, value))
                {
                    _paletteHelper.ChangeBaseTheme(value ? Theme.Dark : Theme.Light);
                    _preferenceService.SaveTheme(value);
                }
            }
        }

        public SettingViewModel(IContainerProvider containerProvider, IPreferenceService preferenceService) : base(containerProvider)
        {
            _preferenceService = preferenceService;
            Title = "Setting View";

            var theme = _paletteHelper.GetTheme();

            IsDark = theme.GetBaseTheme() == BaseTheme.Dark;

            _primaryColor = theme.PrimaryMid.Color;
            _secondaryColor = theme.SecondaryMid.Color;

            SelectedColor = _primaryColor;

            var color = _preferenceService.GetCurrentColor();

            if (color.HasValue)
            {
                ExecuteChangeHueCommand(color.Value);
            }
        }

        private DelegateCommand<Color?> _changeHueCommand = default!;
        public DelegateCommand<Color?> ChangeHueCommand =>
            _changeHueCommand ??= new(ExecuteChangeHueCommand);

        private void ExecuteChangeHueCommand(Color? obj)
        {
            if (obj == null) return;
            var hue = obj.Value;
            SelectedColor = hue;
            if (ActiveScheme == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(hue);
                _primaryColor = hue;
                _primaryForegroundColor = _paletteHelper.GetTheme().PrimaryMid.GetForegroundColor();
            }
            else if (ActiveScheme == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(hue);
                _secondaryColor = hue;
                _secondaryForegroundColor = _paletteHelper.GetTheme().SecondaryMid.GetForegroundColor();
            }
            else if (ActiveScheme == ColorScheme.PrimaryForeground)
            {
                _paletteHelper.SetPrimaryForegroundToSingleColor(hue);
                _primaryForegroundColor = hue;
            }
            else if (ActiveScheme == ColorScheme.SecondaryForeground)
            {
                _paletteHelper.SetSecondaryForegroundToSingleColor(hue);
                _secondaryForegroundColor = hue;
            }

            _preferenceService.SaveTheme(hue);
        }

        private void ChangeCustomColor(object? obj)
        {
            var color = (Color)obj!;

            if (ActiveScheme == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(color);
                _primaryColor = color;
            }
            else if (ActiveScheme == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(color);
                _secondaryColor = color;
            }
            else if (ActiveScheme == ColorScheme.PrimaryForeground)
            {
                _paletteHelper.SetPrimaryForegroundToSingleColor(color);
                _primaryForegroundColor = color;
            }
            else if (ActiveScheme == ColorScheme.SecondaryForeground)
            {
                _paletteHelper.SetSecondaryForegroundToSingleColor(color);
                _secondaryForegroundColor = color;
            }
        }
    }
}