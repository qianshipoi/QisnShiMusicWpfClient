namespace QianShi.Music.Common.Models.Request
{
    public class AllMVRequest : PagedRequestBase
    {
        /// <summary>
        /// 地区,可选值为全部,内地,港台,欧美,日本,韩国,不填则为全部
        /// </summary>
        public string? Area { get; set; }

        /// <summary>
        /// 类型,可选值为全部,官方版,原生,现场版,网易出品,不填则为全部
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 排序,可选值为上升最快,最热,最新,不填则为上升最快
        /// </summary>
        public string? Order { get; set; }
    }
}