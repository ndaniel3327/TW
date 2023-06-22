using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TW.UI.Models.Youtube.Data;

namespace TW.UI.Services.Youtube
{
    public interface IYoutubeService
    {
        Task<bool> RefreshAccessToken();
        Task<YoutubePlaylistList> GetYoutubePlaylists();
        Uri GetAuthorizationLink();
        void GetAuthorizationToken(string authorizationCode);
    }
}
