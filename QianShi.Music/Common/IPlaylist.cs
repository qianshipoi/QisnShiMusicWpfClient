namespace QianShi.Music.Common
{
    public interface IPlaylist
    {
        long Id { get; set; }
        string Name { get; set; }
        string CoverImgUrl { get; set; }
    }
}