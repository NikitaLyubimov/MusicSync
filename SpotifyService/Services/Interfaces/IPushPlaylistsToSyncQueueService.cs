﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.Services.Interfaces
{
    public interface IPushPlaylistsToSyncQueueService
    {
        Task PushPlaylists(string queuetype);
    }
}
