using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using SpotifyLib.Interfaces.BaseWeb;
using SpotifyLib.Constants;
using SpotifyLib.DTO.Tracks;
using SpotifyLib.Interfaces;

namespace SpotifyLib.Clients
{
    public class TracksClient : ITracksClient
    {
        private IAPIConnector _apiConnector;
        public TracksClient(IAPIConnector apiConnector)
        {
            _apiConnector = apiConnector;
        }
        public async Task<GetTrackResponse> GetTrack(string trackName, string artistName)
        {
            var queryString = BuildQuery(trackName, artistName);
            return await _apiConnector.Get<GetTrackResponse>(SpotifyUrls.GetTrack(queryString));
        }

        public async Task<GetTracksResponse> GetTracks(int offset = 0, int limit = 20)
        {
            var response = await _apiConnector.Get<GetTracksResponse>(SpotifyUrls.GetTracksUri(offset, limit));
            return response;
        }

        public async Task<bool> AddTracksToLibrary(IList<string> ids)
        {
            var idsString = string.Join(",", ids);
            var response = await _apiConnector.Put(SpotifyUrls.AddTrackToLibraryUri(idsString));
            if (response.StatusCode != HttpStatusCode.OK)
                return false;
            return true;
        }

        private string BuildQuery(string trackName, string artistName)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"track:{trackName}");
            stringBuilder.Append($" artist:{artistName}");

            var resultString = stringBuilder.ToString();
            return HttpUtility.UrlEncode(resultString);
        }
    }
}
