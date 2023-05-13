using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW.UI.Services
{
    public interface ISpotifyClientService

    {
        Task<Uri> AuthorizeSpotify();
        Task<List<string>> GetPlaylists();
        Task<bool> IsLoggedIn();
    }
}


