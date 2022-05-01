using System.ComponentModel;

namespace QianShi.Music.Common.Models.Request
{
    public class TopPlaylistRequest : PagedRequestBase
    {
        /// <summary>
        /// 类别
        /// </summary>
        [Description("cat")]
        public string? Cat { get; set; }

        /// <summary>
        /// 排序 now hot(默认)
        /// </summary>
        [Description("order")]
        public string? Order { get; set; } = "now";

        //public OrderType Order { get; set; } = OrderType.Now;

        //public enum OrderType
        //{
        //    [Description("now")]
        //    Now,
        //    [Description("hot")]
        //    Hot
        //}
    }
}