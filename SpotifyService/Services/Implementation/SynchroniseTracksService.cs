using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyLib.DTO.Tracks;
using SpotifyLib.Interfaces;
using SpotifyService.DTOs.Response;
using SpotifyService.Services.Interfaces;

namespace SpotifyService.Services.Implementation
{
    public class SynchroniseTracksService : ISynchroniseTracksService
    {
        private ISpotifyClient _spotifyClient;

        public SynchroniseTracksService(ISpotifyClient spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }
        public async Task<bool> SynchroniseTracks(TracksForQueueResponse tracks)
        {
            var tracksList = new List<GetTrackResponse>();
            foreach(var track in tracks.Tracks)
            {
                var searchResponse = await _spotifyClient.TracksClient.GetTrack(track.TrackName, track.ArtistName);
                tracksList.Add(searchResponse);
            }

            var ids = tracksList.Select(tl => tl.Tracks.Items[0].Id).ToList();
            var addTracksResponse = await _spotifyClient.TracksClient.AddTracksToLibrary(ids);
            return addTracksResponse;
            
        }
    }
}
