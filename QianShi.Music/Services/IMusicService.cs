using QianShi.Music.Common.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace QianShi.Music.Services
{
    public interface IMusicService
    {
        Task<List<PlayList>> GetApplyMusic();
        Task<List<PlayList>> GetRecommendMusic();
        Task<List<Singer>> GetRecommmendSinger();
        Task<List<Album>> GetNewAlbum();
        Task<List<Ranking>> GetRanking();
    }
}
