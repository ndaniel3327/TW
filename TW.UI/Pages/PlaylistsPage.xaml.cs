using CommunityToolkit.Maui.Views;
using TW.UI.Helpers;
using TW.UI.Models;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Local;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
   // private bool _execute = true;

    private readonly ISpotifyService _spotifyService;
    private readonly IYoutubeService _youtubeService;
    private readonly ILocalFilesService _localFilesService;

    private List<PlaylistDisplayGroup> _spotifyPlaylistGroupsData;
    private List<PlaylistDisplayGroup> _youtubePlaylistGroupsData;

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
            if (value == true &&_popupPlayerIsVisible!=true)
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
        set { _scrollViewSize = value;
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
        ILocalFilesService localFilesService)
    {
        BindingContext = this;

        InitializeComponent();

        ScrollViewSize = new Rect(0, 0, 1, 1);
        PopupPlayerIsVisible = false;

        //Add the method that will refresh the "selected" playliste in the delegate
        //Refresh is done via the DisplayedPlaylists property specific to each service (Youtube/Spotify/Local)
        PopupPlayerStarts = MovingText;
        OnPopupPlayerStarted = PopupPlayerStarts;

        RefreshSpotifyDisplayedItemsDelegate = GetDisplayedSpotifyPlaylists;
        RefreshYoutubeDisplayedItemsDelegate = GetDisplayedYoutubePlaylists;
        RefreshLocalDisplayedItemsDelegate = GetDisplayedLocalPlaylists;

        _spotifyService = spotifyService;
        _youtubeService = youtubeService;
        _localFilesService = localFilesService;

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
        if (popupPlayerName.Width > popupPlayerTextSection.Width)
        { 
            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                {
                    popupPlayerName.TranslationX -= 5f;

                    if (Math.Abs(popupPlayerName.TranslationX) > popupPlayerName.Width)
                    {
                        popupPlayerName.TranslationX = popupPlayerName.Width + (popupPlayerTextSection.Width - popupPlayerName.Width);
                    }

                    return true;
                });
        }
        
    }

    // Updates local stored data based on spotify account data
    private async void GetYoutubePlaylistData()
    {
        await Task.Run(async () =>
        {
            _youtubePlaylistGroupsData = await _youtubeService.GetYoutubePlaylists();
            var youtubePlaylistsStorageData = new List<string>();

            // If you log-in for the first time store all playlists with the status "selected"

            if (!FileStorageHelper.YoutubePlaylistsFileExists())
            {
                foreach (var playlist in _youtubePlaylistGroupsData)
                {
                    youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                }

                FileStorageHelper.CreateYoutubePlaylistsFile(youtubePlaylistsStorageData);

                DisplayedYoutubePlaylists = _youtubePlaylistGroupsData;
            }
            else
            {
                var oldYoutubePlaylistsStorageData = FileStorageHelper.ReadYoutubePlaylistsFile();

                // If the file exists but there are no playlists in it (ex: deleted all)
                // store all the new playlists with status "selected"
                if (oldYoutubePlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in _youtubePlaylistGroupsData)
                    {
                        youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                    }

                    FileStorageHelper.CreateYoutubePlaylistsFile(youtubePlaylistsStorageData);

                    DisplayedYoutubePlaylists = _youtubePlaylistGroupsData;
                }

                // Preserve the status of the old stored playlists , remove the deleted ones and add the new
                // ones  with the status "selected
                else if (oldYoutubePlaylistsStorageData.Count > 0)
                {
                    foreach (var playlist in _youtubePlaylistGroupsData)
                    {
                        bool isNew = true;
                        foreach (var oldPlaylist in oldYoutubePlaylistsStorageData)
                        {
                            string oldPlaylistId = FileStorageHelper.ReturnId(oldPlaylist);
                            if (oldPlaylistId == playlist.Id)
                            {
                                youtubePlaylistsStorageData.Add(oldPlaylist);
                                isNew = false;
                            }

                        }
                        if (isNew)
                        {
                            youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                        }
                    }

                    FileStorageHelper.CreateYoutubePlaylistsFile(youtubePlaylistsStorageData);

                    GetDisplayedYoutubePlaylists();
                }
            }
        });
    }

    // Display only playlists that are "selected"
    private void GetDisplayedYoutubePlaylists()
    {
        var playlists = FileStorageHelper.ReadYoutubePlaylistsFile();
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in playlists)
        {
            var isSelected = FileStorageHelper.ReturnIsSelected(playlist);
            if (isSelected)
            {
                selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
            }
        }

        var youtubePlaylistGroupsDisplay = new List<PlaylistDisplayGroup>();
        foreach (var playlist in _youtubePlaylistGroupsData)
        {
            foreach (var id in selectedPlaylistsIds)
            {
                if (playlist.Id == id)
                {
                    youtubePlaylistGroupsDisplay.Add(playlist);
                }
            }
        }
        DisplayedYoutubePlaylists = youtubePlaylistGroupsDisplay;
    }

    private async void GetSpotifyPlaylistData()
    {
        await Task.Run(async () =>
        {
            _spotifyPlaylistGroupsData = await _spotifyService.GetPlaylists();
            var spotifyPlaylistsStorageData = new List<string>();

            if (!FileStorageHelper.SpotifyPlaylitsFileExists())
            {
                foreach (var playlist in _spotifyPlaylistGroupsData)
                {
                    spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                }

                FileStorageHelper.CreateSpotifyPlaylistsFile(spotifyPlaylistsStorageData);

                DisplayedSpotifyPlaylists = _spotifyPlaylistGroupsData;
            }
            else
            {
                var oldSpotifyPlaylistsStorageData = FileStorageHelper.ReadSpotifyPlaylistsFile();
                if (oldSpotifyPlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in _spotifyPlaylistGroupsData)
                    {
                        spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                    }

                    FileStorageHelper.CreateSpotifyPlaylistsFile(spotifyPlaylistsStorageData);

                    DisplayedSpotifyPlaylists = _spotifyPlaylistGroupsData;
                }
                else if (oldSpotifyPlaylistsStorageData.Count > 0)
                {
                    foreach (var playlist in _spotifyPlaylistGroupsData)
                    {
                        bool isNew = true;
                        foreach (var oldPlaylist in oldSpotifyPlaylistsStorageData)
                        {
                            string oldPlaylistId = FileStorageHelper.ReturnId(oldPlaylist);
                            if (oldPlaylistId == playlist.Id)
                            {
                                spotifyPlaylistsStorageData.Add(oldPlaylist);
                                isNew = false;
                            }

                        }
                        if (isNew)
                        {
                            spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name));
                        }
                    }

                    FileStorageHelper.CreateSpotifyPlaylistsFile(spotifyPlaylistsStorageData);

                    GetDisplayedSpotifyPlaylists();
                }
            }
        });
    }
    private void GetDisplayedSpotifyPlaylists()
    {
        var playlists = FileStorageHelper.ReadSpotifyPlaylistsFile();
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in playlists)
        {
            var isSelected = FileStorageHelper.ReturnIsSelected(playlist);
            if (isSelected)
            {
                selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
            }
        }

        var spotifyPlaylistGroupsDisplay = new List<PlaylistDisplayGroup>();
        foreach (var playlist in _spotifyPlaylistGroupsData)
        {
            foreach (var id in selectedPlaylistsIds)
            {
                if (playlist.Id == id)
                {
                    spotifyPlaylistGroupsDisplay.Add(playlist);
                }
            }
        }
        DisplayedSpotifyPlaylists = spotifyPlaylistGroupsDisplay;
    }

    private void GetDisplayedLocalPlaylists()
    {
        var localPlaylists = _localFilesService.GetLocalPlaylists();

        var localPlaylistsData = FileStorageHelper.ReadLocalPlaylistsFile();
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in localPlaylistsData)
        {
            var isSelected = FileStorageHelper.ReturnIsSelected(playlist);
            if (isSelected)
            {
                selectedPlaylistsIds.Add(FileStorageHelper.ReturnId(playlist));
            }
        }

        var displayedLocalPlaylists = new List<PlaylistDisplayGroup>();
        foreach (var playlist in localPlaylists)
        {
            foreach (var id in selectedPlaylistsIds)
            {
                if (playlist.Id == id)
                {
                    displayedLocalPlaylists.Add(playlist);
                }
            }
        }
        DisplayedLocalPlaylists = displayedLocalPlaylists;
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
        bool Execute = false;

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
        popupPlayerName.Text= SelectedItem.Name;
        popupPLayerArtist.Text = SelectedItem.Artists;

        //Show PopupPlayer when a song is selected from list
        ScrollViewSize = new Rect(0, 0, 1, 0.8);
        PopupPlayerIsVisible = true;

        Execute = true;
        //popupPlayerName.TranslationX = popupPlayerName.Width;
     

    }

    private void menuButton_Clicked(object sender, EventArgs e)
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