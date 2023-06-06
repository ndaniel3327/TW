using TW.UI.Services.Youtube;

namespace TW.UI.Pages;

public partial class YoutubePlaylistsPage : ContentPage
{
    private readonly IYoutubeClientService _youtubeService;
    
    private List<YoutubePlaylistModel> _youtubePlaylists;
    public List<YoutubePlaylistModel> YoutubePlaylists 
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
    public YoutubePlaylistsPage(IYoutubeClientService youtubeService)
	{
		InitializeComponent();
        BindingContext = this;
        _youtubeService = youtubeService;
        GetPlaylists();
    }

    private async void GetPlaylists()
    {
        var playlistGroup =await _youtubeService.GetYoutubePlaylists();
        var playlistsModel = new List<YoutubePlaylistModel>(); 
        foreach (var playlist in playlistGroup.Playlists)
        {
            var playlistModel = new YoutubePlaylistModel() { Name = playlist.PlaylistInfo.Name, };
            playlistsModel.Add(playlistModel);
        }
        YoutubePlaylists = playlistsModel;
    }
}