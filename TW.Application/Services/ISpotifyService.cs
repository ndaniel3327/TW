using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TW.Application.Services
{
    public interface ISpotifyService
    {
        Task<Uri> AuthorizeWithPKCE();
        Task GetCallback(string code);
        Task<List<string>> GetPlaylists();
        Task<bool> IsLoggedIn();
    }
}
