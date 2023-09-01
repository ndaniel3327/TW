using CommunityToolkit.Maui.Views;
using TW.UI.Helpers;
using TW.UI.Models;
using TW.UI.Pages.PopupPages;
using TW.UI.Services;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    // private bool _execute = true;

    private readonly ISpotifyService _spotifyService;
    private readonly IYoutubeService _youtubeService;
    private readonly ILocalFilesService _localFilesService;
    private readonly IRefreshLocalDataService _refreshLocalDataService;
    private readonly IDisplayedPlaylistsService _displayedPlaylistsService;

    //When you hit the close button on the popup where you select playlists to display them this delegate is used to refresh the displayed list of playlists 
    private Action RefreshSpotifyDisplayedItemsDelegate;
    private Action RefreshYoutubeDisplayedItemsDelegate;
    private Action RefreshLocalDisplayedItemsDelegate;

    private Action PopupPlayerStarts;
    private event Action OnPopupPlayerStarted;
    private bool _popupPlayerIsVisible;
    public bool PopupPlayerIsVisible
    {
        get
        {
            return _popupPlayerIsVisible;
        }
        set
        {
            if (value == true && _popupPlayerIsVisible != true)
            {
                OnPopupPlayerStarted.Invoke();
            }
            _popupPlayerIsVisible = value;
            OnPropertyChanged(nameof(PopupPlayerIsVisible));
        }
    }

    private Rect _scrollViewSize;

    public Rect ScrollViewSize
    {
        get { return _scrollViewSize; }
        set
        {
            _scrollViewSize = value;
            OnPropertyChanged(nameof(ScrollViewSize));
        }
    }

    #region DisplayedPlaylists

    //What makes up the displayed list
    private List<PlaylistDisplayGroup> _displayedLocalPlaylists = new();
    public List<PlaylistDisplayGroup> DisplayedLocalPlaylists
    {
        get { return _displayedLocalPlaylists; }
        set
        {
            _displayedLocalPlaylists = value;
            DisplayedPlaylists = _displayedSpotifyPlaylists.Concat(_displayedYoutubePlaylists.Concat(_displayedLocalPlaylists)).ToList();
        }
    }

    private List<PlaylistDisplayGroup> _displayedSpotifyPlaylists = new();
    public List<PlaylistDisplayGroup> DisplayedSpotifyPlaylists
    {
        get
        {
            return _displayedSpotifyPlaylists;
        }
        set
        {
            _displayedSpotifyPlaylists = value;
            DisplayedPlaylists = _displayedSpotifyPlaylists.Concat(_displayedYoutubePlaylists.Concat(_displayedLocalPlaylists)).ToList();
        }
    }

    private List<PlaylistDisplayGroup> _displayedYoutubePlaylists = new();
    public List<PlaylistDisplayGroup> DisplayedYoutubePlaylists
    {
        get
        {

            return _displayedYoutubePlaylists;
        }
        set
        {
            _displayedYoutubePlaylists = value;
            DisplayedPlaylists = _displayedSpotifyPlaylists.Concat(_displayedYoutubePlaylists.Concat(_displayedLocalPlaylists)).ToList();
        }
    }

    private List<PlaylistDisplayGroup> _displayedPlaylists = new();

    //What is actaully displayed
    public List<PlaylistDisplayGroup> DisplayedPlaylists
    {
        get
        {
            return _displayedPlaylists;
        }
        set
        {
            _displayedPlaylists = value;
            OnPropertyChanged(nameof(DisplayedPlaylists));
        }
    }
    #endregion
    public PlaylistDisplayTrack SelectedItem { get; set; }

    public PlaylistsPage(
        MainPage mainPage,
        ISpotifyService spotifyService,
        IYoutubeService youtubeService,
        ILocalFilesService localFilesService,
        IRefreshLocalDataService refreshLocalDataService,
        IDisplayedPlaylistsService displayedPlaylistsService)
    {
        BindingContext = this;

        InitializeComponent();

        ScrollViewSize = new Rect(0, 0, 1, 1);
        PopupPlayerIsVisible = false;

        //Add the method that will refresh the "selected" playliste in the delegate
        //Refresh is done via the DisplayedPlaylists property specific to each service (Youtube/Spotify/Local)
        PopupPlayerStarts = MovingText;
        OnPopupPlayerStarted = PopupPlayerStarts;

        RefreshSpotifyDisplayedItemsDelegate = GetSpotifyPlaylistData;
        RefreshYoutubeDisplayedItemsDelegate = GetYoutubePlaylistData;
        RefreshLocalDisplayedItemsDelegate = GetDisplayedLocalPlaylists;

        _spotifyService = spotifyService;
        _youtubeService = youtubeService;
        _localFilesService = localFilesService;
        _refreshLocalDataService = refreshLocalDataService;
        _displayedPlaylistsService = displayedPlaylistsService;

        //Check if the user is logged in so the button to select what playlists to display is enabled/disabled
        #region LoginCheck
        if (mainPage.IsSpotifyLoggedIn == true)
        {
            GetSpotifyPlaylistData();
        }
        else
        {
            spotifyButton.IsEnabled = false;
            spotifyButton.BackgroundColor = Colors.Gray;
        }

        if (mainPage.IsYoutubeLoggedIn == true)
        {
            GetYoutubePlaylistData();
        }
        else if (mainPage.IsYoutubeLoggedIn == false)
        {
            youtubeButton.IsEnabled = false;
            youtubeButton.BackgroundColor = Colors.Gray;
        }

        if (FileStorageHelper.LocalPlaylitsFileExists())
        {
            GetDisplayedLocalPlaylists();
        }
        else
        {
            localButton.IsEnabled = false;
            localButton.BackgroundColor = Colors.Gray;
        }
        #endregion
    }

    private void MovingText()
    {

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                popupPlayerName.TranslationX -= 5f;

                if (Math.Abs(popupPlayerName.TranslationX) > popupPlayerName.Width)
                {
                    if (popupPlayerName.Width > popupPlayerTextSection.Width)
                    {
                        popupPlayerName.TranslationX = popupPlayerName.Width + (popupPlayerTextSection.Width - popupPlayerName.Width);
                    }
                    if (popupPlayerName.Width < popupPlayerTextSection.Width)
                    {
                        popupPlayerName.TranslationX = popupPlayerName.Width + popupPlayerTextSection.Width;
                    }
                }
                return true;
            });
    }

    private async void GetYoutubePlaylistData()
    {
        await Task.Run(async () =>
        {
            //Get Remote Data
            var youtubePlaylistGroupsData = await _youtubeService.GetYoutubePlaylists();

            //Refresh data in local files 
            _refreshLocalDataService.RefreshYoutubeLocalData(youtubePlaylistGroupsData);

            // Display only playlists that are "selected"
            DisplayedYoutubePlaylists = _displayedPlaylistsService.UpdateDisplayedYoutbePlaylists(youtubePlaylistGroupsData);
        });
    }

    private async void GetSpotifyPlaylistData()
    {
        await Task.Run(async () =>
        {
            //Get Remote Data
            var spotifyPlaylistGroupsData = await _spotifyService.GetPlaylists();

            //Refresh data in local files
            _refreshLocalDataService.RefreshSpotifyLocalData(spotifyPlaylistGroupsData);

            // Display only playlists that are "selected"
            DisplayedSpotifyPlaylists = _displayedPlaylistsService.UpdateDisplayedSpotifyPlaylists(spotifyPlaylistGroupsData);

        });
    }

    private void GetDisplayedLocalPlaylists()
    {
        // Display only playlists that are "selected"
        var localPlaylists = _localFilesService.GetLocalPlaylists();
        DisplayedLocalPlaylists = _displayedPlaylistsService.UpdateDisplayedLocalPlaylists(localPlaylists);
    }
    private void OnYoutubeButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new YoutubePlaylistsPopup(RefreshYoutubeDisplayedItemsDelegate));
    }

    private void OnSpotifyButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SpotifyPlaylistsPopup(RefreshSpotifyDisplayedItemsDelegate));
    }

    private void OnLocalButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new LocalPlaylistsPopup(RefreshLocalDisplayedItemsDelegate));
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //For PopupMenu Button
        SelectedItem.IsSelected = true;
        if (e.PreviousSelection.Count > 0)
            (e.PreviousSelection[0] as PlaylistDisplayTrack).IsSelected = false;

#if ANDROID
        //Show PopupMenu when button is clicked
        AndroidHelper.ShowPopup(sender as ImageButton);
#endif

        //PopupPlayer change image , names ,artits
        popupPlayerImage.Source = SelectedItem.PopupPlayerImage;
        popupPlayerName.Text = SelectedItem.Name;
        popupPLayerArtist.Text = SelectedItem.Artists;

        //Show PopupPlayer when a song is selected from list
        ScrollViewSize = new Rect(0, 0, 1, 0.8);
        PopupPlayerIsVisible = true;
    }

    private void OnMenuButtonClicked(object sender, EventArgs e)
    {
        SelectedItem.MenuIsVisible = !SelectedItem.MenuIsVisible;

#if ANDROID
        AndroidHelper.ShowPopup((ImageButton)sender);
#endif
        var song = SelectedItem;
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PlayPage));
    }
}