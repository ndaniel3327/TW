using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW.Infrastracture.AppSettings
{
    public class AppSettings : IAppSettings
    {
        public string SpotifyCallbackEndpoint { get; set; }
    }
}
