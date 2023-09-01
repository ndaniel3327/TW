using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TW.UI.Helpers;
using TW.UI.Models;

namespace TW.UI.Services
{
    public class RefreshLocalDataService : IRefreshLocalDataService
    {
        public void RefreshSpotifyLocalData(List<PlaylistDisplayGroup> remotePlaylists)
        {
            var spotifyPlaylistsStorageData = new List<string>();

            // If you log-in for the first time store all playlists with the status "selected"
            if (!FileStorageHelper.SpotifyPlaylitsFileExists())
            {
                foreach (var playlist in remotePlaylists)
                {
                    spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                }

                FileStorageHelper.CreateSpotifyPlaylistsFile(spotifyPlaylistsStorageData);

                // DisplayedSpotifyPlaylists = remotePlaylists;
            }
            else
            {
                // If the file exists but there are no playlists in it (ex: deleted all)
                // store all the new playlists with status "selected"
                var oldSpotifyPlaylistsStorageData = FileStorageHelper.ReadSpotifyPlaylistsFile();
                if (oldSpotifyPlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in remotePlaylists)
                    {
                        spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                    }

                    FileStorageHelper.CreateSpotifyPlaylistsFile(spotifyPlaylistsStorageData);

                    // DisplayedSpotifyPlaylists = remotePlaylists;
                }
                // Preserve the status of the old stored playlists , remove the deleted ones and add the new
                // ones  with the status "selected
                else if (oldSpotifyPlaylistsStorageData.Count > 0)
                {
                    foreach (var playlist in remotePlaylists)
                    {
                        bool isNew = true;
                        foreach (var oldPlaylist in oldSpotifyPlaylistsStorageData)
                        {
                            string oldPlaylistId = FileStorageHelper.ReturnId(oldPlaylist);
                            if (oldPlaylistId == playlist.Id)
                            {
                                spotifyPlaylistsStorageData.Add(oldPlaylist);
                                isNew = false;
                            }

                        }
                        if (isNew)
                        {
                            spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                        }
                    }

                    FileStorageHelper.CreateSpotifyPlaylistsFile(spotifyPlaylistsStorageData);
                }
            }
        }

        public void RefreshYoutubeLocalData(List<PlaylistDisplayGroup> remotePlaylists)
        {
            var youtubePlaylistsStorageData = new List<string>();

            // If you log-in for the first time store all playlists with the status "selected"

            if (!FileStorageHelper.YoutubePlaylistsFileExists())
            {
                foreach (var playlist in remotePlaylists)
                {
                    youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                }

                FileStorageHelper.CreateYoutubePlaylistsFile(youtubePlaylistsStorageData);

                //DisplayedYoutubePlaylists = remotePlaylists;
            }
            else
            {
                var oldYoutubePlaylistsStorageData = FileStorageHelper.ReadYoutubePlaylistsFile();

                // If the file exists but there are no playlists in it (ex: deleted all)
                // store all the new playlists with status "selected"
                if (oldYoutubePlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in remotePlaylists)
                    {
                        youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                    }

                    FileStorageHelper.CreateYoutubePlaylistsFile(youtubePlaylistsStorageData);

                    //DisplayedYoutubePlaylists = remotePlaylists;
                }

                // Preserve the status of the old stored playlists , remove the deleted ones and add the new
                // ones  with the status "selected
                else if (oldYoutubePlaylistsStorageData.Count > 0)
                {
                    foreach (var playlist in remotePlaylists)
                    {
                        bool isNew = true;
                        foreach (var oldPlaylist in oldYoutubePlaylistsStorageData)
                        {
                            string oldPlaylistId = FileStorageHelper.ReturnId(oldPlaylist);
                            if (oldPlaylistId == playlist.Id)
                            {
                                youtubePlaylistsStorageData.Add(oldPlaylist);
                                isNew = false;
                            }

                        }
                        if (isNew)
                        {
                            youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                        }
                    }

                    FileStorageHelper.CreateYoutubePlaylistsFile(youtubePlaylistsStorageData);
                }
            }
        }
    }
}