using CommunityToolkit.Maui.Views;
using TW.UI.Helpers;
using TW.UI.Models;
using TW.UI.Pages.PopupPages;
using TW.UI.Services;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    private readonly ISpotifyService _spotifyService;
    private readonly IYoutubeService _youtubeService;
    private readonly ILocalFilesService _localFilesService;
    private readonly IRefreshLocalDataService _refreshLocalDataService;
    private readonly IDisplayedPlaylistsService _displayedPlaylistsService;

    private readonly Action RefreshSpotifyDisplayedItemsAction;
    private readonly Action RefreshYoutubeDisplayedItemsAction;
    private readonly Action RefreshLocalDisplayedItemsAction;

    private readonly Action PopupPlayerStartsAction;
    private event Action OnPopupPlayerStartedEvent;
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
                OnPopupPlayerStartedEvent.Invoke();
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

    private List<PlaylistDisplayGroupModel> _displayedLocalPlaylists = new();
    public List<PlaylistDisplayGroupModel> DisplayedLocalPlaylists
    {
        get { return _displayedLocalPlaylists; }
        set
        {
            _displayedLocalPlaylists = value;
            DisplayedPlaylists = _displayedSpotifyPlaylists.Concat(_displayedYoutubePlaylists.Concat(_displayedLocalPlaylists)).ToList();
        }
    }

    private List<PlaylistDisplayGroupModel> _displayedSpotifyPlaylists = new();
    public List<PlaylistDisplayGroupModel> DisplayedSpotifyPlaylists
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

    private List<PlaylistDisplayGroupModel> _displayedYoutubePlaylists = new();
    public List<PlaylistDisplayGroupModel> DisplayedYoutubePlaylists
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

    private List<PlaylistDisplayGroupModel> _displayedPlaylists = new();

    public List<PlaylistDisplayGroupModel> DisplayedPlaylists
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

        PopupPlayerStartsAction = MovingText;
        OnPopupPlayerStartedEvent = PopupPlayerStartsAction;

        RefreshSpotifyDisplayedItemsAction = GetSpotifyPlaylistData;
        RefreshYoutubeDisplayedItemsAction = GetYoutubePlaylistData;
        RefreshLocalDisplayedItemsAction = GetDisplayedLocalPlaylists;

        _spotifyService = spotifyService;
        _youtubeService = youtubeService;
        _localFilesService = localFilesService;
        _refreshLocalDataService = refreshLocalDataService;
        _displayedPlaylistsService = displayedPlaylistsService;

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
            var youtubePlaylistGroupsData = await _youtubeService.GetYoutubePlaylists();

            _refreshLocalDataService.RefreshYoutubeLocalData(youtubePlaylistGroupsData);

            DisplayedYoutubePlaylists = _displayedPlaylistsService.UpdateDisplayedYoutbePlaylists(youtubePlaylistGroupsData);
        });
    }

    private async void GetSpotifyPlaylistData()
    {
        await Task.Run(async () =>
        {
            var spotifyPlaylistGroupsData = await _spotifyService.GetPlaylists();

            _refreshLocalDataService.RefreshSpotifyLocalData(spotifyPlaylistGroupsData);

            DisplayedSpotifyPlaylists = _displayedPlaylistsService.UpdateDisplayedSpotifyPlaylists(spotifyPlaylistGroupsData);

        });
    }

    private void GetDisplayedLocalPlaylists()
    {
        var localPlaylists = _localFilesService.GetLocalPlaylists();
        DisplayedLocalPlaylists = _displayedPlaylistsService.UpdateDisplayedLocalPlaylists(localPlaylists);
    }

    private void OnYoutubeButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new YoutubePlaylistsPopup(RefreshYoutubeDisplayedItemsAction));
    }

    private void OnSpotifyButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SpotifyPlaylistsPopup(RefreshSpotifyDisplayedItemsAction));
    }

    private void OnLocalButtonClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new LocalPlaylistsPopup(RefreshLocalDisplayedItemsAction));
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedItem.IsSelected = true;
        if (e.PreviousSelection.Count > 0)
            (e.PreviousSelection[0] as PlaylistDisplayTrack).IsSelected = false;

#if ANDROID
        AndroidHelper.ShowPopup(sender as ImageButton);
#endif

        popupPlayerImage.Source = SelectedItem.PopupPlayerImage;
        popupPlayerName.Text = SelectedItem.Name;
        popupPLayerArtist.Text = SelectedItem.Artists;

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