using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QianShi.Music.Common.Models.Response
{
    public class PlaylistDetailResponse
    {
        public int Code { get; set; }

        [JsonPropertyName("Playlist")]
        public Playlist PlaylistDetail { get; set; } = new Playlist();

        public List<string>? Urls { get; set; }
    }
}
