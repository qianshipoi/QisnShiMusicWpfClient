using System.ComponentModel;

namespace QianShi.Music.Common.Models.Request
{
    public class ArtistMvRequest : PagedRequestBase
    {
        /// <summary>
        /// 歌手Id
        /// </summary>
        [Description("id")]
        public long Id { get; set; }
    }
}
