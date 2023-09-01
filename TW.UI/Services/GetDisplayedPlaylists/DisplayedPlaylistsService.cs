using TW.UI.Helpers;
using TW.UI.Models;

namespace TW.UI.Services
{
    public class DisplayedPlaylistsService : IDisplayedPlaylistsService
    {
        public List<PlaylistDisplayGroup> UpdateDisplayedLocalPlaylists(List<PlaylistDisplayGroup> allPlaylists)
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

            var displayedLocalPlaylists = new List<PlaylistDisplayGroup>();
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

        public List<PlaylistDisplayGroup> UpdateDisplayedSpotifyPlaylists(List<PlaylistDisplayGroup> allPlaylists)
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

            var spotifyPlaylistGroupsDisplay = new List<PlaylistDisplayGroup>();
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

        public List<PlaylistDisplayGroup> UpdateDisplayedYoutbePlaylists(List<PlaylistDisplayGroup> allPlaylists)
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

            var youtubePlaylistGroupsDisplay = new List<PlaylistDisplayGroup>();
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
