using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TW.Application.Models;

namespace TW.Application.Services
{
    public interface ISpotifyService
    {
        Task<Uri> AuthorizeWithPKCE();
        Task GetCallback(string code);
        Task<List<Playlist>> GetPlaylists();
        Task<bool> IsLoggedIn();
    }
}
