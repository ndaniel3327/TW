using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TW.UI.Constants
{
    public class YoutubeConstants
    {
        public static string AuthorizationCode { get; set; }

        public static string GetAuthorizationCode()
        {
            while(AuthorizationCode == null)
            {
                continue;
            }

            return AuthorizationCode;
        }
    }
}
