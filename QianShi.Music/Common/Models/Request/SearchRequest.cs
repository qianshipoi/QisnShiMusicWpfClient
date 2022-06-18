namespace QianShi.Music.Common.Models.Request
{
    public class SearchRequest : PagedRequestBase
    {
        [Description("keywords")]
        public string? Keywords { get; set; }

        /// <summary>
        /// 搜索类型；默认为 1 即单曲 , 取值意义 : 1: 单曲, 10: 专辑, 100: 歌手, 1000: 歌单, 1002: 用户, 1004: MV, 1006: 歌词, 1009: 电台, 1014: 视频, 1018:综合, 2000:声音(搜索声音返回字段格式会不一样)
        /// </summary>
        [Description("type")]
        public SearchType Type { get; set; } = SearchType.单曲;
    }

    public enum SearchType
    {
        单曲 = 1,
        专辑 = 10,
        歌手 = 100,
        歌单 = 1000,
        用户 = 1002,
        MV = 1004,
        歌词 = 1006,
        电台 = 1009,
        视频 = 1014,
        综合 = 1018,
        声音 = 2000
    }
}