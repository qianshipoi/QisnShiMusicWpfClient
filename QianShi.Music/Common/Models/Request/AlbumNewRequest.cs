namespace QianShi.Music.Common.Models.Request
{
    public class AlbumNewRequest : PagedRequestBase
    {
        [Description("area")]
        public string? Area { get; set; } = "ALL";
    }
}