using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW.UI.Services.Youtube
{
    public interface IYoutubeClientService
    {
        Uri AuthorizeYoutube();
        void GetAuthorizationToken(string authorizationCode);
    }
}
