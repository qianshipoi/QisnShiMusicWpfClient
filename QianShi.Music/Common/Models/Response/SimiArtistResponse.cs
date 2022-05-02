namespace QianShi.Music.Common.Models.Response
{
    public class SimiArtistResponse
    {
        public int Code { get; set; }

        public List<Artist> Artists { get; set; } = new List<Artist>();
    }
}
