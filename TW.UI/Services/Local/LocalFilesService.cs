using TW.UI.Constants;
using TW.UI.Enums;
using TW.UI.Helpers;
using TW.UI.Models;

namespace TW.UI.Services.Local
{
    public class LocalFilesService : ILocalFilesService
    {
        public List<PlaylistDisplayGroup> GetLocalPlaylists()
        {
            var playlists = File.ReadAllLines(LocalFilesConstants.LocalPlaylistsFileFullPath);
            List<PlaylistDisplayGroup> playlistGroups = new();
            foreach (string playlist in playlists)
            {
                string playlistId = FileStorageHelper.ReturnId(playlist);
                string playlistName = FileStorageHelper.ReturnName(playlist);
                var trackDataArray = File.ReadAllLines(Path.Combine(FileSystem.Current.AppDataDirectory, playlistId));
                List<PlaylistDisplayTrack> trackViewList = new();
                foreach (string trackData in trackDataArray)
                {
                    string trackName = FileStorageHelper.ReturnName(trackData);
                    trackViewList.Add(new PlaylistDisplayTrack() { Name = trackName });
                }
                playlistGroups.Add(new PlaylistDisplayGroup(playlistId, playlistName, trackViewList, PlaylistSource.Local, ImageSource.FromFile("foldericon.svg")));
            }
            return playlistGroups;
        }
    }
}
