namespace QianShi.Music.Common.Models.Response
{
    public class ArtistMvResponse
    {
        public int Code { get; set; }
        public bool HasMore { get; set; }
        public List<MovieVideo> Mvs { get; set; } = default!;
        //public long? Time { get; set; }
    }
}