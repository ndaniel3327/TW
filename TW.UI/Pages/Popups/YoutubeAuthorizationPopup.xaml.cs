using CommunityToolkit.Maui.Views;
using TW.UI.ViewModels;

namespace TW.UI.Pages;

public partial class YoutubeAuthorizationPopup : Popup
{
	public YoutubeAuthorizationPopup(Uri loginUri)
	{
		InitializeComponent();
        BindingContext = new YoutubeAuthorizationPopupViewModel(loginUri);
    }
}