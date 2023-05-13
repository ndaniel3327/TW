using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TW.Infrastructure.Models;

namespace TW.Infrastructure.Services
{
    public interface ISpotifyServerService
    {
        bool IsLoggedIn { get; set; }

        Task<Uri> AuthorizeWithPKCE();
        Task GetCallback(string code);
        Task<List<Playlist>> GetPlaylists();
    }
}
