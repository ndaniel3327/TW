using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TW.UI.Models;

namespace TW.UI.Services
{
    public interface IRefreshLocalDataService
    {
        void RefreshSpotifyLocalData(List<PlaylistDisplayGroupModel> remotePlaylists);
        void RefreshYoutubeLocalData(List<PlaylistDisplayGroupModel> remotePlaylists);
    }
}
