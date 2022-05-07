namespace QianShi.Music.Common.Models.Response
{
    public class ArtistSublistResponse
    {
        public int Code { get; set; }
        public int Count { get; set; }
        public bool HasMore { get; set; }
        public List<Artist> Data { get; set; } = new List<Artist>();

        //public partial class Subject : IPlaylist
        //{
        //    public int AlbumSize { get; set; }
        //    public List<string> Alias { get; set; } = new();
        //    public long Id { get; set; }
        //    public string Img1v1Url { get; set; } = default!;
        //    public string Name { get; set; } = default!;
        //    public long PicId { get; set; }
        //    public string PicUrl { get; set; } = default!;
        //    public string CoverImgUrl { get => PicUrl; set => PicUrl = value; }
        //    public string Info { get; set; } = default!;
        //    public int MvSize { get; set; }
        //    public string? Trans { get; set; }
        //}
    }
}