using System;
using System.Threading.Tasks;

namespace TW.Application.Services
{
    public interface ISpotifyService
    {
        Task<Uri> AuthorizeWithPKCE();
        Task GetCallback(string code);
    }
}
