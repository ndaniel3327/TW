using System.ComponentModel;

namespace TW.UI.ViewModels
{
    public class SpotifyAuthorizationPopupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Uri _loginUri;
        public string LoginUri
        {
            get
            {
                return _loginUri.ToString();
            }
        }

        public SpotifyAuthorizationPopupViewModel(Uri loginUri)
        {
            _loginUri = loginUri;
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
