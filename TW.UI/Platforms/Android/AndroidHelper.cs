using Android.Views;
using Android.Widget;

namespace TW.UI
{
    public static class AndroidHelper
    {
        public static void ShowPopup(object sender)
        {
            if(sender is Microsoft.Maui.Controls.ImageButton)
            {
                var imageButton = (Microsoft.Maui.Controls.ImageButton)sender;
                var platfromViewImageButton = imageButton.Handler.PlatformView as AndroidX.AppCompat.Widget.AppCompatImageView;

                var popupMenu = new PopupMenu(MainActivity.Instance/*Android.App.Application.Context*/, platfromViewImageButton);
                popupMenu.Menu.Add(Menu.None, 1, 1, "Select");
                popupMenu.Menu.Add(Menu.None, 2, 2, "Move To:");
                popupMenu.Menu.Add(Android.Views.IMenu.None, 3, 3, "Delete");
                popupMenu.Show();
            }
        }
        public static void ShowPlayerPopup()
        {

        }
            //var imageButton = (Microsoft.Maui.Controls.ImageButton)sender;
            //var menuPopup = new MoveToMenu();
            //menuPopup.BackgroundColor = Colors.Transparent;

            //var platformViewImageButton = imageButton.Handler.PlatformView as global::Android.Widget.ImageButton;

            //global::Android.Widget.PopupMenu popupMenu = new global::Android.Widget.PopupMenu(global::Android.App.Application.Context, platformViewImageButton);

            //popupMenu.Show();
        
    }
}
