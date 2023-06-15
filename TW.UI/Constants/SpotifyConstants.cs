namespace TW.Infrastracture.Constants
{
    public static class SpotifyConstants
    {
        private const string baseApiEndpoint = "/api/Spotify";

        public const int HTTPSPort = 5001;
        public const string SpotifyCallbackAdress = "oauth://localhost:5001/api/Spotify/callback";

        public const string AuthorizationEndpoint = baseApiEndpoint;
        public const string PlaylistsEndpoint = $"{baseApiEndpoint}/Playlists";
        public const string IsLoggedInEndpoint = $"{baseApiEndpoint}/IsLoggedIn";
        public const string RefreshAccessTokenEndpoint = $"{baseApiEndpoint}/RefreshToken";

        public static string StorageNameAccessToken = "SpotifyAccessToken";
        public static string StorageNameRefreshToken = "SpotifyRefreshToken";
        public static string StorageNameTokenType = "SpotifyTokenType";
        public static string StorageNameSpotifyTokenExpirationDate = "SpotifyTokenExpirationDate";
    }
}
