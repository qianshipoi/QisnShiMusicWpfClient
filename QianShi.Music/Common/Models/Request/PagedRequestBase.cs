using System.ComponentModel;

namespace QianShi.Music.Common.Models.Request
{
    public class PagedRequestBase
    {
        /// <summary>
        /// 返回数量
        /// </summary>
        [Description("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// 偏移量
        /// </summary>
        [Description("offset")]
        public int? Offset { get; set; }
    }
}