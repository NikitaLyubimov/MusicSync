using System.Text;
using System.Threading.Tasks;
using System.Web;

using BaseWeb.Interfaces;
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
