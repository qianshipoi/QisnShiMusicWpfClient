namespace QianShi.Music.Common.Models.Request
{
    public class ArtistAlbumRequest : PagedRequestBase
    {
        /// <summary>
        /// 歌手ID
        /// </summary>
        [Description("id")]
        public long Id { get; set; }
    }
}