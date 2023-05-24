using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW.UI.ViewModels
{
    public class YoutubeAuthorizationPopupViewModel:INotifyPropertyChanged
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
            set 
            { 
                _popupSize = value;
                OnPropertyChanged(nameof(PopupSize));   
            }
        }
        public string LoginUri
        {
            get
            {
                return _loginUri;
            }
            set
            {
                _loginUri = value;
                OnPropertyChanged(nameof(LoginUri));
            }
        }
        public YoutubeAuthorizationPopupViewModel(Uri loginUri)
        {
            LoginUri = loginUri.ToString();
            var screeenWidth = DeviceDisplay.MainDisplayInfo.Width;
            var screeenHeight = DeviceDisplay.MainDisplayInfo.Height;
            //TODO: consider different screen sizes
            PopupSize = new Size((screeenWidth - 400) / 2, (screeenHeight - 800) / 2);
        }


        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
