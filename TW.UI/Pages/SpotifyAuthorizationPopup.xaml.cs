using CommunityToolkit.Maui.Views;
using TW.UI.ViewModels;

namespace TW.UI.Pages;

public partial class SpotifyAuthorizationPopup : Popup
{

    public SpotifyAuthorizationPopup(Uri loginUri)
	{
		BindingContext=new SpotifyAuthorizationPopupViewModel(loginUri);
		InitializeComponent();
    }

}