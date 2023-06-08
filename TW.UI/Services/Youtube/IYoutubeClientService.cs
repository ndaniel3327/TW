using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW.UI.Services.Youtube
{
    public interface IYoutubeClientService
    {
        void RefreshAccessToken();
        Task<YoutubePlaylistGroup> GetYoutubePlaylists();
        Uri GetAuthorizationLink();
        void GetAuthorizationToken(string authorizationCode);
    }
}
