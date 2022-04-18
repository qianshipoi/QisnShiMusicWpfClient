using QianShi.Music.Common.Models.Request;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;

using RestSharp;

namespace QianShi.Music.Services
{
    public class PlaylistService : IPlaylistService
    {
        private static RestClient _client => new RestClient(new RestClientOptions("https://netease-cloud-music-api-qianshi.vercel.app")
        {
            Timeout = -1
        });

        private static Task<T?> Get<T>(RestRequest request)
        {
            //var client = new RestClient(new RestClientOptions("https://netease-cloud-music-api-qianshi.vercel.app")
            //{
            //    Timeout = -1
            //});
            return _client.GetAsync<T>(request);
        }

        public async Task<CatlistResponse> GetCatlistAsync()
            => await Get<CatlistResponse>(new RestRequest("/playlist/catlist"))
            ?? new CatlistResponse();

        public Task<PlaylistHighqualityTagsResponse> GetPlaylistHighqualityTagsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RelatedPlaylistResponse> GetRelatedPlaylistAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<TopPlaylistResponse?> GetTopPlaylistAsync(TopPlaylistRequest parameter)
        {
            var request = new RestRequest("/top/playlist");
            request.AddQueryParameters(parameter);

            return await Get<TopPlaylistResponse>(request);
        }

        public async Task<TopPlaylistHighqualityResponse> GetTopPlaylistHighqualityAsnyc(TopPlaylistHighqualityRequest parameter)
        {
            var request = new RestRequest("/top/playlist/highquality");

            request.AddQueryParameters(parameter);

            return await Get<TopPlaylistHighqualityResponse>(request) ?? new TopPlaylistHighqualityResponse();
        }

        public async Task<PersonalizedResponse> GetPersonalizedAsync(int? limit)
        {
            var request = new RestRequest("/personalized");
            if (limit != null)
                request.AddQueryParameter<int>("limit", limit.Value);
            return await Get<PersonalizedResponse>(request) ?? new PersonalizedResponse();
        }

        public async Task<ToplistResponse> GetToplistAsync()
        {
            var request = new RestRequest("/toplist");
            return await Get<ToplistResponse>(request) ?? new ToplistResponse();
        }

        public Task<PlaylistHotResponse> GetHotAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AlbumNewestResponse> GetAlbumNewestAsync()
        {
            var request = new RestRequest("/album/newest");
            return await Get<AlbumNewestResponse>(request) ?? new AlbumNewestResponse();
        }

        public async Task<PlaylistDetailResponse> GetPlaylistDetailAsync(long playlistId)
        {
            var request = new RestRequest("/playlist/detail");
            request.AddParameter("id",playlistId);
            return await Get<PlaylistDetailResponse>(request) ?? new PlaylistDetailResponse();
        }
    }
}
