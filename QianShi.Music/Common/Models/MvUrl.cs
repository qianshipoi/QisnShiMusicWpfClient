using Prism.Mvvm;

namespace QianShi.Music.Common.Models
{
    public class MvUrl : BindableBase
    {
        public int Br { get; set; }
        public string Url { get; set; } = string.Empty;
        private bool _isActive = false;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
    }
}