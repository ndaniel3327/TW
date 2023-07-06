using TW.UI.Models;

namespace TW.UI.Services.Local
{
    public interface ILocalFilesService
    {
        List<PlaylistDisplayGroup> GetLocalPlaylists();
    }
}
