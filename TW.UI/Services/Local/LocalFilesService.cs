using static TW.UI.Constants.AppConstants;
using TW.UI.Helpers;
using TW.UI.Models;

namespace TW.UI.Services
{
    public class LocalFilesService : ILocalFilesService
    {
        public List<PlaylistDisplayGroup> GetLocalPlaylists()
        {
            var playlists = FileStorageHelper.ReadLocalPlaylistsFile();
            List<PlaylistDisplayGroup> playlistGroups = new();
            foreach (string playlist in playlists)
            {
                string playlistId = FileStorageHelper.ReturnId(playlist);
                string playlistName = FileStorageHelper.ReturnName(playlist);
                var trackDataArray = FileStorageHelper.RealLocalPlaylistSongsFile(playlistId);
             
                List<PlaylistDisplayTrack> trackViewList = new();
                foreach (string trackData in trackDataArray)
                {
                    string trackName = FileStorageHelper.ReturnName(trackData);
                    trackViewList.Add(new PlaylistDisplayTrack() { Name = trackName });
                }
                playlistGroups.Add(new PlaylistDisplayGroup(playlistId, playlistName, trackViewList, PlaylistSourceEnum.Local, ImageSource.FromFile("foldericon.svg")));
            }
            return playlistGroups;
        }
    }
}
