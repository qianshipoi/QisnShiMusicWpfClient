namespace QianShi.Music.Common.Models.Response
{
    public class LikelistResponse
    {
        public int Code { get; set; }
        public long CheckPoint { get; set; }
        public List<long> Ids { get; set; } = new();
    }
}