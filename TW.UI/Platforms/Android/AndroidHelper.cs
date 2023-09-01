using Android.Views;
using Android.Widget;

namespace TW.UI
{
    public static class AndroidHelper
    {
        public static void ShowPopup(object sender)
        {
            if (sender is Microsoft.Maui.Controls.ImageButton)
            {
                var imageButton = (Microsoft.Maui.Controls.ImageButton)sender;
                var platfromViewImageButton = imageButton.Handler.PlatformView as AndroidX.AppCompat.Widget.AppCompatImageView;

                var popupMenu = new PopupMenu(MainActivity.Instance, platfromViewImageButton);
                popupMenu.Menu.Add(IMenu.None, 1, 1, "Select");
                popupMenu.Menu.Add(IMenu.None, 2, 2, "Move To:");
                popupMenu.Menu.Add(IMenu.None, 3, 3, "Delete");
                popupMenu.Show();
            }
        }
    }
}
