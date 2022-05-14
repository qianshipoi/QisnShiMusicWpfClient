namespace QianShi.Music.Common.Models.Response
{
    public class SimiMvResponse
    {
        public int Code { get; set; }
        public List<MovieVideo> Mvs { get; set; } = default!;
    }
}