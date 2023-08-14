namespace TW.UI.Helpers
{
    public static class FileStorageHelper
    {
        private static char _separator = '@';
        private static readonly int _indexOfId = 0;
        private static readonly int _indexOfName = 1;
        private static readonly int _indexOfSelected = 2;

        private readonly static string _mainDirectoryPath;

        private const string YoutubePlaylistsFileName = "YoutubePlaylistsFile";
        private const string SpotifyPlaylistsFileName = "SpotifyPlaylistsFile";
        private const string LocalPlaylistsFileName = "LocalPlaylistsFile";

        private readonly static string _youtubePlaylitsFileFullPath;
        private readonly static string _spotifyPlaylitsFileFullPath;
        public readonly static string _localPlaylistsFileFullPath;

        static FileStorageHelper()
        {
            _mainDirectoryPath = FileSystem.Current.AppDataDirectory;

            _youtubePlaylitsFileFullPath = Path.Combine(_mainDirectoryPath, YoutubePlaylistsFileName);
            _spotifyPlaylitsFileFullPath = Path.Combine(_mainDirectoryPath, SpotifyPlaylistsFileName);
            _localPlaylistsFileFullPath = Path.Combine(_mainDirectoryPath, LocalPlaylistsFileName);
        }

        public static bool YoutubePlaylistsFileExists()
        {
           return File.Exists(_youtubePlaylitsFileFullPath);
        }

        public static bool SpotifyPlaylitsFileExists()
        {
            return File.Exists(_spotifyPlaylitsFileFullPath);
        }

        public static bool LocalPlaylitsFileExists()
        {
            return File.Exists(_localPlaylistsFileFullPath);
        }

        public static void CreateYoutubePlaylistsFile(List<string> playlists)
        {
            File.WriteAllLines(_youtubePlaylitsFileFullPath, playlists.ToArray());
        }

        public static void CreateSpotifyPlaylistsFile(List<string> playlists)
        {
            File.WriteAllLines(_spotifyPlaylitsFileFullPath, playlists.ToArray());
        }

        public static void CreateLocalPlaylistsFile(List<string> playlists)
        {
            File.WriteAllLines(_localPlaylistsFileFullPath, playlists.ToArray());
        }

        public static List<string> ReadYoutubePlaylistsFile()
        {
           return File.ReadAllLines(_youtubePlaylitsFileFullPath).ToList();
        }

        public static List<string> ReadSpotifyPlaylistsFile()
        {
           return File.ReadAllLines(_spotifyPlaylitsFileFullPath).ToList();
        }

        public static List<string> ReadLocalPlaylistsFile()
        {
           return File.ReadAllLines(_localPlaylistsFileFullPath).ToList();
        }

        public static void AddPlaylistToLocalPlaylistsFile(string name)
        {

        }
        public static string ReturnId(string playlist)
        {
            var contents = playlist.Split(_separator);
            string id = contents[_indexOfId].Substring(contents[_indexOfId].IndexOf("id=") + 3);
            return id;
        }

        public static string ReturnName(string playlist)
        {
            var contents = playlist.Split(_separator);
            string fullProperty = contents[_indexOfName];
            string name = fullProperty.Substring(fullProperty.IndexOf("name=") + 5);
            return name;
        }

        public static string ReturnSelected(string playlist)
        {
            var contents = playlist.Split(_separator);
            string selected = contents[_indexOfSelected].Substring(contents[_indexOfSelected].IndexOf("selected=") + 9);
            return selected;
        }
        public static string GenerateAndReturnEntry(string id , string name , string selected)
        {
            return "id=" + id + "@name=" + name + "@selected="+selected;
        }

        public class LocalPlatlist
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
