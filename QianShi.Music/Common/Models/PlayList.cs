using System.Collections.Generic;

namespace QianShi.Music.Common.Models
{
    /// <summary>
    /// 歌单
    /// </summary>
    public class PlayList
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Cover { get; set; }

        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
