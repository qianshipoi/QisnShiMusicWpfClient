using Prism.Mvvm;

namespace QianShi.Music.Common.Models
{
    public class CategoryItem : BindableBase
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public CategoryType CategoryType { get; set; }
    }

    public enum CategoryType
    {
        Normal
    }
}