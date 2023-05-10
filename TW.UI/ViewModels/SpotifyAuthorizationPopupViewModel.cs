using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices;
using System.ComponentModel;

namespace TW.UI.ViewModels
{
    public class SpotifyAuthorizationPopupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _loginUri;
        private Size _screenSize;

        public Size ScreenSize
        {
            get
            {
                return _screenSize;
            }
        }
        public string LoginUri
        {
            set
            {
                _loginUri = value;
                OnPropertyChanged(nameof(LoginUri));
            }
            get
            {
                return _loginUri;
            }
        }

        public SpotifyAuthorizationPopupViewModel(Uri loginUri)
        {
            _loginUri = loginUri.ToString();
            var screeenWidth = DeviceDisplay.MainDisplayInfo.Width;
            var screeenHeight = DeviceDisplay.MainDisplayInfo.Height;
            _screenSize = new Size((screeenWidth - 400) / 2, (screeenHeight - 800) / 2);
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
