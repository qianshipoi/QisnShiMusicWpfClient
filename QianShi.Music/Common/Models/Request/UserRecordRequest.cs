namespace QianShi.Music.Common.Models.Request
{
    public class UserRecordRequest : PagedRequestBase
    {
        /// <summary>
        ///  用户 id
        /// </summary>
        [Description("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// type=1 时只返回 weekData, type=0 时返回 allData
        /// </summary>
        [Description("type")]
        public sbyte? Type { get; set; } = 0;
    }
}