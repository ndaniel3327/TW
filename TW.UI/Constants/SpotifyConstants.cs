namespace TW.Infrastracture.Constants
{
    public static class SpotifyConstants
    {
        private const string baseApiEndpoint = "api/Spotify";

        public const int HTTPSPort = 5001;
        public const string AuthorizationEndpoint = baseApiEndpoint;
        public const string PlaylistsEndpoint = $"{baseApiEndpoint}/Playlists";
        public const string IsLoggedInEndpoint = $"{baseApiEndpoint}/IsLoggedIn";
    }
}
