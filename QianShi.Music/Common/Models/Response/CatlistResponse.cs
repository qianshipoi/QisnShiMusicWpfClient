namespace QianShi.Music.Common.Models.Response
{
    public class CatlistResponse
    {
        public int Code { get; set; }
        public Cat? All { get; set; }
        public List<Cat> Sub { get; set; } = new List<Cat>();
        public Dictionary<int, string> Categories { get; set; } = new Dictionary<int, string>();
    }
}