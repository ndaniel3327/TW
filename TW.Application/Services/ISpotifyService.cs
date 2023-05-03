using System.Threading.Tasks;

namespace TW.Application.Services
{
    public interface ISpotifyService
    {
        void AuthorizeAndGetToken();

        Task AuthorizeAndGetTokenOld();
    }
}
