namespace QianShi.Music.Common.Models.Request
{
    public class UserPlaylistRequest : PagedRequestBase
    {
        [Description("uid")]
        public long Uid { get; set; }
    }
}