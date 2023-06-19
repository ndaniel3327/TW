namespace TW.Infrastracture.Constants
{
    public static class SpotifyConstants
    {
        private const string baseApiEndpoint = "/api/Spotify";

        public const int HTTPSPort = 5001;

        public const string AuthorizationEndpoint = baseApiEndpoint;
        public const string PlaylistsEndpoint = $"{baseApiEndpoint}/Playlists";
        public const string IsLoggedInEndpoint = $"{baseApiEndpoint}/IsLoggedIn";
        public const string RefreshAccessTokenEndpoint = $"{baseApiEndpoint}/RefreshToken";

        public const string StorageNameAccessToken = "SpotifyAccessToken";
        public const string StorageNameRefreshToken = "SpotifyRefreshToken";
        public const string StorageNameTokenType = "SpotifyTokenType";
        public const string StorageNameTokenExpirationDate = "SpotifyTokenExpirationDate";
        public const string StorageNameCodeVerifier = "SpotifyCodeVerifier";
    }
}
