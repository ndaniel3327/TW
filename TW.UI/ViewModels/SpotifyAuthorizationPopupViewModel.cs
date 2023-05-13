using System.ComponentModel;

namespace TW.UI.ViewModels
{
    public class SpotifyAuthorizationPopupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _loginUri;
        private Size _popupSize;

        public Size PopupSize
        {
            get
            {
                return _popupSize;
            }
        }
        public string LoginUri
        {
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
            //TODO: consider different screen sizes
            _popupSize = new Size((screeenWidth - 400) / 2, (screeenHeight - 800) / 2);
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
