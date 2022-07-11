using QianShi.Music.Common.Models;
using QianShi.Music.Services;

namespace QianShi.Music.Data
{
    public class PlaylistDataProvider : DataCaching<PlaylistDetail, long>
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistStoreService _playlistStoreService;

        public PlaylistDataProvider(
            IPlaylistService playlistService,
            IPlaylistStoreService playlistStoreService)
        {
            _playlistService = playlistService;
            _playlistStoreService = playlistStoreService;
        }

        protected override async Task<PlaylistDetail?> Source(long id)
        {
            var detail = new PlaylistDetail();
            var response = await _playlistService.GetPlaylistDetailAsync(id);
            if (response.Code != 200)
            {
                return null;
            }

            detail.Id = response.PlaylistDetail.Id;
            detail.Name = response.PlaylistDetail.Name;
            detail.Description = response.PlaylistDetail.Description ?? string.Empty;
            detail.LastUpdateTime = response.PlaylistDetail.UpdateTime;
            detail.PicUrl = response.PlaylistDetail.CoverImgUrl;
            detail.Count = response.PlaylistDetail.TrackCount;
            detail.Creator = response.PlaylistDetail.Creator?.Nickname;
            detail.CreatorId = response.PlaylistDetail.Creator?.UserId ?? 0;

            // 获取所有歌曲
            if (response.PlaylistDetail.TrackIds.Count > 0)
            {
                var ids = string.Join(',', response.PlaylistDetail.TrackIds.Take(20).Select(x => x.Id));
                var songResponse = await _playlistService.SongDetail(ids);
                if (songResponse.Code == 200)
                {
                    foreach (var song in songResponse.Songs)
                    {
                        song.Album.CoverImgUrl += "?param=48y48";
                        song.IsLike = _playlistStoreService.HasLikedSong(song);
                        detail.Songs.Add(song);
                    }
                }
                detail.SongsIds.AddRange(response.PlaylistDetail.TrackIds.Select(x => x.Id));
            }
            return detail;
        }
    }
}