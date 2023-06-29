namespace TW.UI.Constants
{
    public static class SpotifyConstants
    {
        private static string _mainDirectoryPath = FileSystem.Current.AppDataDirectory;
        private const string _spotifyPlaylistsFileName = "SpotifyPlaylistsFile";

        public static string SpotifyPlaylitsFileFullPath = Path.Combine(_mainDirectoryPath, _spotifyPlaylistsFileName);

        public const string StorageNameAccessToken = "SpotifyAccessToken";
        public const string StorageNameRefreshToken = "SpotifyRefreshToken";
        public const string StorageNameTokenType = "SpotifyTokenType";
        public const string StorageNameTokenExpirationDate = "SpotifyTokenExpirationDate";
        public const string StorageNameCodeVerifier = "SpotifyCodeVerifier";
    }
}
