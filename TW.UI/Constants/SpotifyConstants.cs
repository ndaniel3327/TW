using TW.UI.Models.Spotify.View;

namespace TW.UI.Constants
{
    public static class SpotifyConstants
    {
        public static List<SpotifyPlaylistGroup> playlistGroups;

        public const string StorageNameAccessToken = "SpotifyAccessToken";
        public const string StorageNameRefreshToken = "SpotifyRefreshToken";
        public const string StorageNameTokenType = "SpotifyTokenType";
        public const string StorageNameTokenExpirationDate = "SpotifyTokenExpirationDate";
        public const string StorageNameCodeVerifier = "SpotifyCodeVerifier";
    }
}
