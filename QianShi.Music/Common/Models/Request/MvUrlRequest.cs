using System.ComponentModel;

namespace QianShi.Music.Common.Models.Request
{
    public class MvUrlRequest
    {
        /// <summary>
        /// mv id
        /// </summary>
        [Description("id")]
        public long Id { get; set; }

        /// <summary>
        /// 分辨率,默认 1080,可从 /mv/detail 接口获取分辨率列表
        /// </summary>
        [Description("r")]
        public int? R { get; set; } = 1080;
    }
}