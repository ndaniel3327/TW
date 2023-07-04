using CommunityToolkit.Maui.Views;
using TW.UI.Models;
using TW.UI.Models.Youtube.View;
using TW.UI.Services.Youtube;

namespace TW.UI.Pages.PopupPages;

public partial class YoutubePlaylistsPopup : Popup
{
    private readonly IYoutubeService _youtubeService;
    
    private List<PlaylistDisplayGroup> _youtubePlaylists;
    public List<PlaylistDisplayGroup> YoutubePlaylists 
    {
        get 
        {
            return _youtubePlaylists;
        } 
        set
        {
            if (_youtubePlaylists != value)
            {
                _youtubePlaylists = value;
                OnPropertyChanged(nameof(YoutubePlaylists));
            }
        }
    }
    public YoutubePlaylistsPopup(IYoutubeService youtubeService)
	{
		InitializeComponent();
        BindingContext = this;
        _youtubeService = youtubeService;
        GetPlaylists();
    }

    private async void GetPlaylists()
    {
        var playlistGroup =await _youtubeService.GetYoutubePlaylists();
        var playlistsModel = new List<PlaylistDisplayGroup>(); 
        foreach (var playlist in playlistGroup.Playlists)
        {
            var tracks = new List<PlaylistDisplayTracks>();
            var trackNameList = playlist.Tracks.Select(q => q.TrackInfo.Name);
            foreach (var trackName in trackNameList)
            {
                tracks.Add(new PlaylistDisplayTracks() { Name = trackName });
            }
            var playlistModel = new PlaylistDisplayGroup(playlist.Id,playlist.PlaylistInfo.Name, tracks);
            playlistsModel.Add(playlistModel);
        }
        YoutubePlaylists = playlistsModel;
    }
}