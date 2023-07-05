namespace TW.UI.Constants
{
    public static class YoutubeConstants
    {
        private static string _mainDirectoryPath = FileSystem.Current.AppDataDirectory;
        private const string _youtubePlaylistsFileName = "YoutubePlaylistsFile";

        public static string YoutubePlaylitsFileFullPath = Path.Combine(_mainDirectoryPath, _youtubePlaylistsFileName);

        public const string StorageNameAccessToken = "YoutubeAccessToken";
        public const string StorageNameRefreshToken = "YoutubeRefreshToken";
        public const string StorageNameTokenType = "YoutubeTokenType";
        public const string StorageNameTokenExpirationDate = "YoutubeTokenExpirationDate";
        public const string StorageNameCodeVerifier = "YoutubeCodeVerifier";
    }
}
