using TW.UI.Helpers;
using TW.UI.Models;

namespace TW.UI.Services
{
    public class DisplayedPlaylistsService : IDisplayedPlaylistsService
    {
        public List<PlaylistDisplayGroupModel> UpdateDisplayedLocalPlaylists(List<PlaylistDisplayGroupModel> allPlaylists)
        {
            var localPlaylistsData = FileStorageHelper.ReadLocalPlaylistsFile();
            List<string> selectedPlaylistsIds = new();
            foreach (var playlist in localPlaylistsData)
            {
                var isSelected = FileStorageHelper.ReturnIsSelected(playlist);
                if (isSelected)
                {
                    selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
                }
            }

            var displayedLocalPlaylists = new List<PlaylistDisplayGroupModel>();
            foreach (var playlist in allPlaylists)
            {
                foreach (var id in selectedPlaylistsIds)
                {
                    if (playlist.Id == id)
                    {
                        displayedLocalPlaylists.Add(playlist);
                    }
                }
            }
            return displayedLocalPlaylists;
        }

        public List<PlaylistDisplayGroupModel> UpdateDisplayedSpotifyPlaylists(List<PlaylistDisplayGroupModel> allPlaylists)
        {
            var playlists = FileStorageHelper.ReadSpotifyPlaylistsFile();
            List<string> selectedPlaylistsIds = new();
            foreach (var playlist in playlists)
            {
                var isSelected = FileStorageHelper.ReturnIsSelected(playlist);
                if (isSelected)
                {
                    selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
                }
            }

            var spotifyPlaylistGroupsDisplay = new List<PlaylistDisplayGroupModel>();
            foreach (var playlist in allPlaylists)
            {
                foreach (var id in selectedPlaylistsIds)
                {
                    if (playlist.Id == id)
                    {
                        spotifyPlaylistGroupsDisplay.Add(playlist);
                    }
                }
            }
            return spotifyPlaylistGroupsDisplay;
        }

        public List<PlaylistDisplayGroupModel> UpdateDisplayedYoutbePlaylists(List<PlaylistDisplayGroupModel> allPlaylists)
        {
            var playlists = FileStorageHelper.ReadYoutubePlaylistsFile();
            List<string> selectedPlaylistsIds = new();
            foreach (var playlist in playlists)
            {
                var isSelected = FileStorageHelper.ReturnIsSelected(playlist);
                if (isSelected)
                {
                    selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
                }
            }

            var youtubePlaylistGroupsDisplay = new List<PlaylistDisplayGroupModel>();
            foreach (var playlist in allPlaylists)
            {
                foreach (var id in selectedPlaylistsIds)
                {
                    if (playlist.Id == id)
                    {
                        youtubePlaylistGroupsDisplay.Add(playlist);
                    }
                }
            }
            return youtubePlaylistGroupsDisplay;
        }
    }
}
