namespace QianShi.Music.Common.Models
{
    /// <summary>
    /// 专辑
    /// </summary>
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }   
        public Singer Singer { get; set; }
    }
}
