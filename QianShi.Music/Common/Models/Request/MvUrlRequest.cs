namespace QianShi.Music.Common.Models.Request
{
    public class MvUrlRequest
    {
        /// <summary>
        /// mv id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 分辨率,默认 1080,可从 /mv/detail 接口获取分辨率列表
        /// </summary>
        public int R { get; set; }
    }
}
