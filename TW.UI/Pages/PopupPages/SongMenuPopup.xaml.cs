using CommunityToolkit.Maui.Views;

namespace TW.UI.Pages.PopupPages;

public partial class SongMenuPopup : Popup
{
	public double MyPopupAnchorX { get; set; } = 200;
    public SongMenuPopup()
	{
		InitializeComponent();
		BindingContext = this;
		myPopup.Size = new Size(300, 300);
	}
}