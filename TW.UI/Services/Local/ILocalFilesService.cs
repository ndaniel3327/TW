using TW.UI.Models;

namespace TW.UI.Services
{
    public interface ILocalFilesService
    {
        List<PlaylistDisplayGroup> GetLocalPlaylists();
    }
}
