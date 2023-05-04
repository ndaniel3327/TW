using System.Threading.Tasks;

namespace TW.Application.Services
{
    public interface ISpotifyService
    {
        Task AuthorizeWithPKCE();
        Task GetCallback(string code);
    }
}
