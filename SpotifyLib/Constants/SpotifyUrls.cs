﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.Constants
{
    public class SpotifyUrls
    {
        public static readonly Uri AuthTokenRequest = new("https://accounts.spotify.com/api/token");
        public static readonly Uri SpotifyApiUri = new("https://api.spotify.com/");
        public static readonly Uri GetPlaylistsUri = new("v1/me/playlists", UriKind.Relative);
        public static readonly Uri PlaylistUri = new("v1/playlists", UriKind.Relative);
        public static Uri CreatePlaylistUri(string userId) => new($"v1/users/{userId}/playlists", UriKind.Relative);
        public static Uri AddItemsToPlaylist(string playlistId) => new($"v1/playlists/{playlistId}/tracks", UriKind.Relative);
    }
}
