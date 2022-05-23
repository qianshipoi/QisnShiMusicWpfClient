namespace QianShi.Music.Common.Models
{
    /// <summary>
    /// 品质
    /// </summary>
    public record Quality
    {
        public int Br { get; set; }
        public double Fid { get; set; }
        public long Size { get; set; }
        public int Sr { get; set; }
        public double Vd { get; set; }
    }
}