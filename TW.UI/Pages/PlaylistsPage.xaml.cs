using CommunityToolkit.Maui.Views;
using TW.UI.Constants;
using TW.UI.Helpers;
using TW.UI.Models;
using TW.UI.Pages.PopupPages;
using TW.UI.Services.Local;
using TW.UI.Services.Spotify;
using TW.UI.Services.Youtube;

namespace TW.UI.Pages;

public partial class PlaylistsPage : ContentPage
{
    private string _selected = "true"; //default selected value
    //private string _deselected = "false";

    private readonly ISpotifyService _spotifyService;
    private readonly IYoutubeService _youtubeService;
    private readonly ILocalFilesService _localFilesService;

    private List<PlaylistDisplayGroup> _spotifyPlaylistGroupsData;
    private List<PlaylistDisplayGroup> _youtubePlaylistGroupsData;

    //When you hit the close button on the popup where you select playlists to display them this delegate is used to refresh the displayed list of playlists 
    private Action RefreshSpotifyDisplayedItemsDelegate;
    private Action RefreshYoutubeDisplayedItemsDelegate;
    private Action RefreshLocalDisplayedItemsDelegate;

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

        //Add the method that will refresh the "selected" playliste in the delegate
        //Refresh is done via the DisplayedPlaylists property specific to each service (Youtube/Spotify/Local)
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

        if (File.Exists(LocalFilesConstants.LocalPlaylistsFileFullPath))
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

    private async void GetYoutubePlaylistData()
    {
        await Task.Run(async () =>
        {
            _youtubePlaylistGroupsData = await _youtubeService.GetYoutubePlaylists();
            var youtubePlaylistsStorageData = new List<string>();

            if (!File.Exists(YoutubeConstants.YoutubePlaylitsFileFullPath))
            {
                var stream = File.Create(YoutubeConstants.YoutubePlaylitsFileFullPath);
                stream.Close();

                foreach (var playlist in _youtubePlaylistGroupsData)
                {
                    youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                }

                File.WriteAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath, youtubePlaylistsStorageData.ToArray());

                DisplayedYoutubePlaylists = _youtubePlaylistGroupsData;
            }
            else
            {
                var oldYoutubePlaylistsStorageData = File.ReadAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath).ToList();
                if (oldYoutubePlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in _youtubePlaylistGroupsData)
                    {
                        youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                    }

                    File.WriteAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath, youtubePlaylistsStorageData.ToArray());

                    DisplayedYoutubePlaylists = _youtubePlaylistGroupsData;
                }
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
                            youtubePlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                        }
                    }
                    File.WriteAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath, youtubePlaylistsStorageData.ToArray());
                    GetDisplayedYoutubePlaylists();
                }
            }
        });
    }

    private void GetDisplayedYoutubePlaylists()
    {
        var playlists = File.ReadAllLines(YoutubeConstants.YoutubePlaylitsFileFullPath).ToList();
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in playlists)
        {
            string isSelected = FileStorageHelper.ReturnSelected(playlist);
            if (isSelected == "true")
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

            if (!File.Exists(SpotifyConstants.SpotifyPlaylitsFileFullPath))
            {
                var stream = File.Create(SpotifyConstants.SpotifyPlaylitsFileFullPath);
                stream.Close();

                foreach (var playlist in _spotifyPlaylistGroupsData)
                {
                    spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                }

                File.WriteAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath, spotifyPlaylistsStorageData.ToArray());

                DisplayedSpotifyPlaylists = _spotifyPlaylistGroupsData;
            }
            else
            {
                var oldSpotifyPlaylistsStorageData = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath).ToList();
                if (oldSpotifyPlaylistsStorageData.Count == 0)
                {
                    foreach (var playlist in _spotifyPlaylistGroupsData)
                    {
                        spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                    }

                    File.WriteAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath, spotifyPlaylistsStorageData.ToArray());

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
                            spotifyPlaylistsStorageData.Add(FileStorageHelper.GenerateAndReturnEntry(playlist.Id, playlist.Name, _selected));
                        }
                    }
                    File.WriteAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath, spotifyPlaylistsStorageData.ToArray());
                    GetDisplayedSpotifyPlaylists();
                }
            }
        });
    }
    private void GetDisplayedSpotifyPlaylists()
    {
        var playlists = File.ReadAllLines(SpotifyConstants.SpotifyPlaylitsFileFullPath).ToList();
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in playlists)
        {
            string isSelected = FileStorageHelper.ReturnSelected(playlist);
            if (isSelected == "true")
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

        var localPlaylistsData = File.ReadAllLines(LocalFilesConstants.LocalPlaylistsFileFullPath);
        List<string> selectedPlaylistsIds = new();
        foreach (var playlist in localPlaylistsData)
        {
            string isSelected = FileStorageHelper.ReturnSelected(playlist);
            if (isSelected == "true")
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
        SelectedItem.IsSelected = true;
        //e.CurrentSelection as PlaylistDisplayTrack;
        //selecteItem.MenuImageSource = ImageSource.FromFile("menuicon.svg");

        if (e.PreviousSelection.Count > 0)
            (e.PreviousSelection[0] as PlaylistDisplayTrack).IsSelected =false;
    }

    private void menuButton_Clicked(object sender, EventArgs e)
    {
        SelectedItem.MenuIsVisible = !SelectedItem.MenuIsVisible;

        AndroidHelper.ShowPopup((ImageButton)sender);

        var song = SelectedItem;
        //await DisplayActionSheet("", "", null, "Move to 1", "Move to 2");

        //var imageButton = (Microsoft.Maui.Controls.ImageButton)sender;
        //var menuPopup = new MoveToMenu();
        //menuPopup.BackgroundColor = Colors.Transparent;

        //var platformViewImageButton = imageButton.Handler.PlatformView as global::Android.Widget.ImageButton;

        //global::Android.Widget.PopupMenu popupMenu = new global::Android.Widget.PopupMenu(global::Android.App.Application.Context, platformViewImageButton);

        //popupMenu.Show();


        //Microsoft.UI.Xaml.Window window = (Microsoft.UI.Xaml.Window)App.Current.Windows.First<Window>().Handler.PlatformView;
        //var platformview = CounterBtn.Handler.PlatformView as Microsoft.Maui.Platform.MauiButton;
        //var point = platformview.TransformToVisual(window.Content).TransformPoint(new Windows.Foundation.Point(0, 0));

        //menuPopup.TranslationX = imageButton.TranslationX;
        //menuPopup.TranslationY= imageButton.Y-menuPopup.Y;

        //MopupService.Instance.PushAsync(menuPopup);
        //ShowMenuPopup();
    }

    //private partial void ShowMenuPopup(ImageButton imageButton);
        

}