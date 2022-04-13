using Prism.Mvvm;

namespace QianShi.Music.Common.Models
{
    /// <summary>
    /// 歌单类别
    /// </summary>
    public class Cat : BindableBase
    {
        public string Name { get; set; } = null!;
        public int ResourceCount { get; set; }
        public int ImgId { get; set; }
        public string? ImgUrl { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public int ResourceType { get; set; }
        public bool Hot { get; set; }
        public bool Activity { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string? DisplayName { get; set; }

        public bool IsLastOne { get; set; }

        private bool _isSelected;
        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }

        private bool _isActivation;
        public bool IsActivation { get => _isActivation; set => SetProperty(ref _isActivation, value); }
    }
}
