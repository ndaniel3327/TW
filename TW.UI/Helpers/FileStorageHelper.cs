
namespace TW.UI.Helpers
{
    public static class FileStorageHelper
    {
        private const char Separator = '@';
        private const int IndexOfId = 0;
        private const int IndexOfName = 1;
        private const int IndexOfIsSelected = 2;

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

        public static List<string> RealLocalPlaylistSongsFile(string fileName)
        {
            var path = Path.Combine(_mainDirectoryPath,fileName);
            return File.ReadAllLines(path).ToList();
        }

        public static void UpdateLocalPlaylistsFile(string name , bool selected = true)
        {
            string id = Guid.NewGuid().ToString();

            var entry = string.Format("id={0}@name={1}@selected={2}{3}", id, name, selected.ToString().ToLower(), Environment.NewLine);
            File.AppendAllText(_localPlaylistsFileFullPath,entry);
        }

        public static void UpdateLocalPlaylistSongsFile(string fileName , string name , string path)
        {
            string id = Guid.NewGuid().ToString();

            var playlistFilePath = Path.Combine(_mainDirectoryPath, fileName);
            if (!File.Exists(playlistFilePath))
            {
                var stream = File.Create(playlistFilePath);
                stream.Close();
            }
            
            var entry = string.Format("id={0}@name={1}@path={2}{3}", id, name, path, Environment.NewLine);
            File.AppendAllText(playlistFilePath,entry);
        }

        public static void DeleteLocalPlaylistSongsFile(string fileName)
        {
            var path = Path.Combine(_mainDirectoryPath, fileName);
            File.Delete(path);
        }

        public static string ReturnId(string playlist)
        {
            var contents = playlist.Split(Separator);

            return contents[IndexOfId];
        }

        public static string ReturnName(string playlist)
        {
            var contents = playlist.Split(Separator);

            return contents[IndexOfName];
        }

        public static bool ReturnIsSelected(string playlist)
        {
            var contents = playlist.Split(Separator);


            return bool.Parse(contents[IndexOfIsSelected]);
        }

        public static string GenerateAndReturnEntry(string id , string name , bool isSelected = true)
        {
            return $"{id}{Separator}{name}{Separator}{(isSelected ? bool.TrueString : bool.FalseString)}";
        }
    }
}
