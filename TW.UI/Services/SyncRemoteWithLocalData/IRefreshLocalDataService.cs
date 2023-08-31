using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TW.UI.Models;

namespace TW.UI.Services.SyncRemoteWithLocalData
{
    public interface IRefreshLocalDataService
    {
        void RefreshSpotifyLocalData(List<PlaylistDisplayGroup> remotePlaylists);
        void RefreshYoutubeLocalData(List<PlaylistDisplayGroup> remotePlaylists);
    }
}
