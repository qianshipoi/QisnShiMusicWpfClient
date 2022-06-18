namespace QianShi.Music.Common.Models
{
    public class MenuBar : BindableBase
    {
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 菜单命名空间
        /// </summary>
        public string NameSpace { get; set; } = string.Empty;

        /// <summary>
        /// 登录认证
        /// </summary>
        public bool Auth { get; set; }
    }
}